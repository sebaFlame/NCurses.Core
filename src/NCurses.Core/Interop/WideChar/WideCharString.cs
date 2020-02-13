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

namespace NCurses.Core.Interop.WideChar
{
    internal struct WideCharString<TWideChar> : IMultiByteCharString, IEquatable<WideCharString<TWideChar>>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
    {
        public unsafe Span<byte> ByteSpan =>
            this.BufferArray is null ? new Span<byte>(this.BufferPointer, this.BufferPointerLength) : new Span<byte>(this.BufferArray);
        public unsafe Span<TWideChar> CharSpan =>
            this.BufferArray is null ? new Span<TWideChar>(this.BufferPointer, this.BufferPointerLength / Marshal.SizeOf<TWideChar>()) : MemoryMarshal.Cast<byte, TWideChar>(this.ByteSpan);

        public int Length { get; }

        public ref TWideChar GetPinnableReference() => ref this.CharSpan.GetPinnableReference();

        private unsafe byte* BufferPointer;
        private int BufferPointerLength;

        private byte[] BufferArray;

        public unsafe WideCharString(
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

        public unsafe WideCharString(
            byte[] buffer,
            string str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer), str.AsSpan());
        }

        public unsafe WideCharString(
            byte* buffer, 
            int bufferLength,
            int stringLength)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = bufferLength;
            this.BufferArray = null;
            this.Length = stringLength;
        }

        public unsafe WideCharString(
            byte[] buffer,
            int stringLength)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = stringLength;
        }

        public unsafe WideCharString(ref TWideChar strRef)
        {
            TWideChar* wideArr = (TWideChar*)Unsafe.AsPointer<TWideChar>(ref strRef);

            this.BufferPointer = (byte*)wideArr;
            this.Length = FindStringLength(wideArr, out this.BufferPointerLength);
            this.BufferArray = null;
        }

        private static unsafe void CreateCharString(
            Span<byte> buffer,
            ReadOnlySpan<char> charArray)
        {
            Span<TWideChar> charString = MemoryMarshal.Cast<byte, TWideChar>(buffer);

            fixed (char* originalChars = charArray)
            {
                int bytesUsed = 0, charsUsed = 0, charPosition = 0, bytePosition = 0;
                bool completed = false;

                Encoder encoder = NativeNCurses.Encoding.GetEncoder();
                int byteCount = encoder.GetByteCount(originalChars, charArray.Length, false);

                byte* encodedBytes = stackalloc byte[byteCount];

                for (int i = 0; i < charArray.Length; i++)
                {
                    charPosition += charsUsed;
                    bytePosition += bytesUsed;
                    encoder.Convert(
                        originalChars + charPosition,
                        1,
                        encodedBytes + bytePosition,
                        byteCount - bytePosition,
                        i == charArray.Length - 1 ? true : false,
                        out charsUsed,
                        out bytesUsed,
                        out completed);

                    if (!completed)
                    {
                        throw new InvalidOperationException("Could not complete encoding string");
                    }

                    charString[i] = WideCharFactoryInternal<TWideChar>.CreateCharFromSpan(new Span<byte>(encodedBytes + bytePosition, bytesUsed));
                }
            }
        }

        internal unsafe static int FindStringLength(TWideChar* strArr, out int byteLength)
        {
            TWideChar zero = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyCharInternal();
            TWideChar val;
            int length = 0;

            while (true)
            {
                val = *(strArr + (length++ * Marshal.SizeOf<TWideChar>()));
                if (zero.Equals(val))
                {
                    break;
                }
            }

            byteLength = length * Marshal.SizeOf<TWideChar>();
            return --length;
        }

        IEnumerator IEnumerable.GetEnumerator() => new WideCharStringEnumerator(this);

        IEnumerator<IChar> IEnumerable<IChar>.GetEnumerator() => new WideCharStringEnumerator(this);

        IEnumerator<IMultiByteChar> IEnumerable<IMultiByteChar>.GetEnumerator() => new WideCharStringEnumerator(this);

        public static explicit operator string(WideCharString<TWideChar> wStr) => wStr.ToString();

        public override bool Equals(object obj)
        {
            if (obj is WideCharString<TWideChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if (obj is WideCharString<TWideChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IMultiByteCharString obj)
        {
            if (obj is WideCharString<TWideChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(WideCharString<TWideChar> other)
        {
            //remove null terminator
            Span<byte> thisSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TWideChar>());
            Span<byte> otherSpan = other.ByteSpan.Slice(0, other.Length * Marshal.SizeOf<TWideChar>());

            return thisSpan.SequenceEqual(otherSpan);
        }

        public override string ToString()
        {
            unsafe
            {
                Span<byte> bSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TWideChar>());
                char* charArr = stackalloc char[this.Length];

                fixed (byte* b = bSpan)
                {
                    NativeNCurses.Encoding.GetChars(b, bSpan.Length, charArr, this.Length);
                }

                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, this.Length);
                return charSpan.ToString();
            }
        }

        public override int GetHashCode()
        {
            return this.CharSpan.GetHashCode();
        }

        private class WideCharStringEnumerator : IEnumerator<IMultiByteChar>, IEnumerator<IChar>
        {
            IMultiByteChar IEnumerator<IMultiByteChar>.Current => this.WideCharString.CharSpan[this.Position];

            IChar IEnumerator<IChar>.Current => this.WideCharString.CharSpan[this.Position];

            object IEnumerator.Current => this.WideCharString.CharSpan[this.Position];

            private WideCharString<TWideChar> WideCharString;
            private int Position;

            public WideCharStringEnumerator(WideCharString<TWideChar> wideCharString)
            {
                this.WideCharString = wideCharString;
                this.Reset();
            }

            public bool MoveNext() => ++this.Position < this.WideCharString.Length;

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
