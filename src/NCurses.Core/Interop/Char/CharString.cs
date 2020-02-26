using System;
using System.Collections.Generic;
using System.Linq;
using System.Buffers;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop.Char
{
    internal struct CharString<TChar> : ISingleByteCharString, IEquatable<CharString<TChar>>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
    {
        public unsafe Span<byte> ByteSpan =>
            this.BufferArray is null ? new Span<byte>(this.BufferPointer, this.BufferLength) : new Span<byte>(this.BufferArray, 0, this.BufferLength);
        public unsafe Span<TChar> CharSpan =>
            this.BufferArray is null ? new Span<TChar>(this.BufferPointer, this.BufferLength / Marshal.SizeOf<TChar>()) : MemoryMarshal.Cast<byte, TChar>(this.ByteSpan);

        public int Length { get; }

        public ref TChar GetPinnableReference() => ref this.CharSpan.GetPinnableReference();

        private unsafe byte* BufferPointer;
        private byte[] BufferArray;
        private int BufferLength;
        

        public unsafe CharString(
            byte* buffer,
            int bufferLength,
            string str)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer, bufferLength), str.AsSpan());
        }

        public unsafe CharString(
            byte* buffer,
            int bufferLength,
            Span<char> str)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer, bufferLength), str);
        }

        public unsafe CharString(
            byte[] buffer,
            int bufferLength,
            string str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer), str.AsSpan());
        }

        public unsafe CharString(
            byte[] buffer,
            int bufferLength,
            Span<char> str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer), str);
        }

        public unsafe CharString(
            byte* buffer, 
            int bufferLength,
            int stringLength)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = stringLength;
        }

        public unsafe CharString(
            byte[] buffer,
            int bufferLength,
            int stringLength)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = stringLength;
        }

        public unsafe CharString(ref TChar strRef)
        {
            TChar* wideArr = (TChar*)Unsafe.AsPointer<TChar>(ref strRef);

            this.BufferPointer = (byte*)wideArr;
            this.Length = FindStringLength(wideArr, out this.BufferLength);
            this.BufferArray = null;
        }

        private static unsafe void CreateCharString(
            Span<byte> buffer,
            ReadOnlySpan<char> charArray)
        {
            Span<TChar> charString = MemoryMarshal.Cast<byte, TChar>(buffer);

            fixed (char* originalChars = charArray)
            {
                int bytesUsed = 0, charsUsed = 0;
                bool completed = false;

                lock (NativeNCurses.SyncRoot)
                {
                    Encoder encoder = NativeNCurses.SingleByteEncoder;
                    encoder.Reset();

                    int byteCount = encoder.GetByteCount(originalChars, charArray.Length, false);
                    byte* bytePtr = stackalloc byte[byteCount];

                    encoder.Convert(
                        originalChars,
                        charArray.Length,
                        bytePtr,
                        byteCount,
                        true,
                        out charsUsed,
                        out bytesUsed,
                        out completed);

                    if (!completed)
                    {
                        throw new InvalidOperationException("Could not complete encoding string");
                    }

                    for (int i = 0; i < byteCount; i++)
                    {
                        charString[i] = CharFactoryInternal<TChar>.CreateCharFromByte(bytePtr[i]);
                    }
                }
            }
        }

        internal unsafe static int FindStringLength(TChar* strArr, out int byteLength)
        {
            TChar zero = CharFactoryInternal<TChar>.Instance.GetNativeEmptyCharInternal();
            TChar val;

            int length = 0;

            while (true)
            {
                val = *(strArr + (length++ * Marshal.SizeOf<TChar>()));
                if (zero.Equals(val))
                {
                    break;
                }
            }

            byteLength = length * Marshal.SizeOf<TChar>();
            return --length;
        }

        IEnumerator IEnumerable.GetEnumerator() => new CharStringEnumerator(this);

        IEnumerator<IChar> IEnumerable<IChar>.GetEnumerator() => new CharStringEnumerator(this);

        IEnumerator<ISingleByteChar> IEnumerable<ISingleByteChar>.GetEnumerator() => new CharStringEnumerator(this);

        public static explicit operator string(CharString<TChar> wStr) => wStr.ToString();

        public override bool Equals(object obj)
        {
            if (obj is CharString<TChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if(obj is CharString<TChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ISingleByteCharString obj)
        {
            if (obj is CharString<TChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(CharString<TChar> other)
        {
            //remove null terminator
            Span<byte> thisSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TChar>());
            Span<byte> otherSpan = other.ByteSpan.Slice(0, other.Length * Marshal.SizeOf<TChar>());

            return thisSpan.SequenceEqual(otherSpan);
        }

        public override string ToString()
        {
            unsafe
            {
                Span<byte> bSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TChar>());
                char* charArr = stackalloc char[this.Length];

                lock (NativeNCurses.SyncRoot)
                {
                    Decoder decoder = NativeNCurses.SingleByteDecoder;
                    decoder.Reset();

                    fixed (byte* b = bSpan)
                    {
                        decoder.GetChars(b, bSpan.Length, charArr, this.Length, true);
                    }
                }

                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, this.Length);
                return charSpan.ToString();
            }
        }

        public override int GetHashCode()
        {
            return this.CharSpan.GetHashCode();
        }

        private class CharStringEnumerator : IEnumerator<ISingleByteChar>, IEnumerator<IChar>
        {
            ISingleByteChar IEnumerator<ISingleByteChar>.Current => this.CharString.CharSpan[this.Position];

            IChar IEnumerator<IChar>.Current => this.CharString.CharSpan[this.Position];

            object IEnumerator.Current => this.CharString.CharSpan[this.Position];

            private CharString<TChar> CharString;
            private int Position;

            public CharStringEnumerator(CharString<TChar> charString)
            {
                this.CharString = charString;
                this.Reset();
            }

            public bool MoveNext() => ++this.Position < this.CharString.Length;

            public void Reset()
            {
                this.Position = -1;
            }

            public void Dispose()
            {
                //NOP
            }
        }
    }
}
