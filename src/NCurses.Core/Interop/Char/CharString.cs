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
            this.BufferArray is null ? new Span<byte>(this.BufferPointer, this.BufferPointerLength) : new Span<byte>(this.BufferArray);
        public unsafe Span<TChar> CharSpan =>
            this.BufferArray is null ? new Span<TChar>(this.BufferPointer, this.BufferPointerLength / Marshal.SizeOf<TChar>()) : MemoryMarshal.Cast<byte, TChar>(this.ByteSpan);

        public int Length { get; }

        public ref TChar GetPinnableReference() => ref this.CharSpan.GetPinnableReference();

        private unsafe byte* BufferPointer;
        private int BufferPointerLength;

        private byte[] BufferArray;

        public unsafe CharString(
            byte* buffer,
            int bufferLength,
            string str)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = bufferLength;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer, bufferLength), str.AsSpan());
        }

        public unsafe CharString(
            byte[] buffer,
            string str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer), str.AsSpan());
        }

        public unsafe CharString(
            byte* buffer, 
            int bufferLength,
            int stringLength)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = bufferLength;
            this.BufferArray = null;
            this.Length = stringLength;
        }

        public unsafe CharString(
            byte[] buffer,
            int stringLength)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = stringLength;
        }

        public unsafe CharString(ref TChar strRef)
        {
            TChar* wideArr = (TChar*)Unsafe.AsPointer<TChar>(ref strRef);

            this.BufferPointer = (byte*)wideArr;
            this.Length = FindStringLength(wideArr, out this.BufferPointerLength);
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

                Encoder encoder = Encoding.ASCII.GetEncoder();
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

                fixed (byte* b = bSpan)
                {
                    Encoding.ASCII.GetChars(b, bSpan.Length, charArr, this.Length);
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
