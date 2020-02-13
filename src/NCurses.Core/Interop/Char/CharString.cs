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
    internal struct CharString<TChar> : ICharString, IEquatable<CharString<TChar>>
        where TChar : unmanaged, IChar, IEquatable<TChar>
    {
        public unsafe Span<byte> ByteSpan =>
            this.BufferArray is null ? new Span<byte>(this.BufferPointer, this.BufferPointerLength) : new Span<byte>(this.BufferArray);
        public unsafe Span<TChar> CharSpan =>
            this.BufferArray is null ? new Span<TChar>(this.BufferPointer, this.BufferPointerLength / Marshal.SizeOf<TChar>()) : MemoryMarshal.Cast<byte, TChar>(this.ByteSpan);

        public int Length => this.CharSpan.Length;

        public ref TChar GetPinnableReference() => ref this.CharSpan.GetPinnableReference();

        private unsafe byte* BufferPointer;
        private int BufferPointerLength;

        private byte[] BufferArray;

        public unsafe CharString(
            byte* buffer,
            int length,
            string str)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(new Span<byte>(buffer, length), str.AsSpan());
        }

        public unsafe CharString(
            byte[] buffer,
            string str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(new Span<byte>(buffer), str.AsSpan());
        }

        public unsafe CharString(byte* buffer, int length)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;
        }

        public unsafe CharString(byte[] buffer)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
        }

        public unsafe CharString(ref TChar strRef)
        {
            TChar* wideArr = (TChar*)Unsafe.AsPointer<TChar>(ref strRef);

            this.BufferPointer = (byte*)wideArr;
            this.BufferPointerLength = FindStringLength(wideArr);
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

        internal unsafe static int FindStringLength(TChar* strArr)
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
            return --length;
        }

        public IEnumerator<IChar> GetEnumerator() => new CharStringEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => new CharStringEnumerator(this);

        public static explicit operator string(CharString<TChar> wStr) => wStr.ToString();

        public bool Equals(ICharString obj)
        {
            if(obj is CharString<TChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(CharString<TChar> other)
        {
            return this.ByteSpan.SequenceEqual(other.ByteSpan);
        }

        public override string ToString()
        {
            unsafe
            {
                Span<TChar> mbSpan = this.CharSpan;
                Span<byte> byteSpan = this.ByteSpan;

                char* charArr = stackalloc char[mbSpan.Length];

                fixed (byte* b = byteSpan)
                {
                    Encoding.ASCII.GetChars(b, byteSpan.Length, charArr, mbSpan.Length);
                }

                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, mbSpan.Length);
                return charSpan.ToString();
            }
        }

        public override int GetHashCode()
        {
            return this.CharSpan.GetHashCode();
        }

        private class CharStringEnumerator : IEnumerator<IChar>
        {
            public IChar Current => this.CharString.CharSpan[this.Position];

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
