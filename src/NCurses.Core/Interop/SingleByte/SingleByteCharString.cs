using System;
using System.Collections;
using System.Collections.Generic;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop.SingleByte
{
    internal struct SingleByteCharString<TSingleByte> : ISingleByteCharString, IEquatable<SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
    {
        public unsafe Span<byte> ByteSpan =>
            this.BufferArray is null ? new Span<byte>(this.BufferPointer, this.BufferPointerLength) : new Span<byte>(this.BufferArray);
        public unsafe Span<TSingleByte> CharSpan =>
            this.BufferArray is null ? new Span<TSingleByte>(this.BufferPointer, this.BufferPointerLength / Marshal.SizeOf<TSingleByte>()) : MemoryMarshal.Cast<byte, TSingleByte>(this.ByteSpan);

        public int Length => this.CharSpan.Length;

        public ref TSingleByte GetPinnableReference() => ref this.CharSpan.GetPinnableReference();

        private unsafe byte* BufferPointer;
        private int BufferPointerLength;

        private byte[] BufferArray;

        public INCursesChar this[int index] => this.CharSpan[index];

        public unsafe SingleByteCharString(
            byte* buffer, 
            int length, 
            string str)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(
                new Span<byte>(buffer, length),
                str.AsSpan());
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            string str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(
                new Span<byte>(buffer),
                str.AsSpan());
        }

        public unsafe SingleByteCharString(
            byte* buffer,
            int length,
            string str,
            ulong attrs)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(
                new Span<byte>(buffer, length),
                str.AsSpan(), 
                attrs);
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            string str,
            ulong attrs)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(
                new Span<byte>(buffer),
                str.AsSpan(),
                attrs);
        }

        public unsafe SingleByteCharString(
            byte* buffer,
            int length,
            string str, 
            ulong attrs, 
            short pair)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(
                new Span<byte>(buffer, length),
                str.AsSpan(), 
                attrs, 
                pair);
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            string str,
            ulong attrs,
            short pair)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(
                new Span<byte>(buffer),
                str.AsSpan(),
                attrs,
                pair);
        }

        public unsafe SingleByteCharString(
            byte* buffer,
            int length)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;
        }

        public unsafe SingleByteCharString(
            byte[] buffer)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
        }

        public unsafe SingleByteCharString(ref TSingleByte strRef)
        {
            TSingleByte* wideArr = (TSingleByte*)Unsafe.AsPointer<TSingleByte>(ref strRef);

            this.BufferPointer = (byte*)wideArr;
            this.BufferPointerLength = FindStringLength(wideArr);
            this.BufferArray = null;
        }

        private static unsafe void CreateCharString(
            Span<byte> buffer,
            ReadOnlySpan<char> charArray, 
            ulong attrs = 0, 
            short colorPair = 0)
        {
            Span<TSingleByte> charString = MemoryMarshal.Cast<byte, TSingleByte>(buffer);

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
                    charString[i] = CreateSmallChar((sbyte)bytePtr[i], attrs, colorPair);
                }
            }
        }

        public static int GetByteCount(string str, bool addNullTerminator = true) =>
            SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str, addNullTerminator);

        public static int GetByteCount(int length, bool addNullTerminator = true) =>
            SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(length, addNullTerminator);

        private static TSingleByte CreateSmallChar(sbyte encodedByte, ulong attrs = 0, short colorPair = 0)
        {
            if (attrs == 0 && colorPair == 0)
            {
                return SingleByteCharFactoryInternal<TSingleByte>.CreateCharFromByte(encodedByte);
            }
            else if (colorPair == 0)
            {
                return SingleByteCharFactoryInternal<TSingleByte>.CreateCharWithAttributeFromByte(encodedByte, attrs);
            }
            else
            {
                return SingleByteCharFactoryInternal<TSingleByte>.CreateCharWithAttributeAndColorPairFromByte(encodedByte, attrs, colorPair);
            }
        }

        internal unsafe static int FindStringLength(TSingleByte* strArr)
        {
            TSingleByte zero = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();
            TSingleByte val;
            int length = 0;

            while (true)
            {
                val = *(strArr + (length++ * Marshal.SizeOf<TSingleByte>()));
                if (zero.Equals(val))
                {
                    break;
                }
            }
            return --length;
        }

        public IEnumerator<ISingleByteChar> GetEnumerator()
        {
            return new SingleByteCharStringEnumerator(this);
        }

        IEnumerator<INCursesChar> IEnumerable<INCursesChar>.GetEnumerator()
        {
            return new SingleByteCharStringEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SingleByteCharStringEnumerator(this);
        }

        IEnumerator<IChar> IEnumerable<IChar>.GetEnumerator()
        {
            return new SingleByteCharStringEnumerator(this);
        }

        //TODO: crashes Visual Studio debugger when using netcoreapp2.0
        public static explicit operator string(SingleByteCharString<TSingleByte> str)
        {
            unsafe
            {
                char* charArr = stackalloc char[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    charArr[i] = str.CharSpan[i].Char;
                }   
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, str.Length);
                return charSpan.ToString();
            }
        }

        public static bool operator ==(in SingleByteCharString<TSingleByte> chStrLeft, in SingleByteCharString<TSingleByte> chStrRight)
        {
            return chStrLeft.Equals(chStrRight);
        }

        public static bool operator !=(in SingleByteCharString<TSingleByte> chStrLeft, in SingleByteCharString<TSingleByte> chStrRight)
        {
            return chStrLeft.Equals(chStrRight);
        }

        public override bool Equals(object obj)
        {
            if (obj is ISingleByteCharString other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if (obj is ISingleByteCharString other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(INCursesCharString obj)
        {
            if (obj is ISingleByteCharString other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ISingleByteCharString obj)
        {
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(SingleByteCharString<TSingleByte> other)
        {
            return this.ByteSpan.SequenceEqual(other.ByteSpan);
        }

        public override string ToString()
        {
            return (string)this;
        }

        public override int GetHashCode()
        {
            return this.CharSpan.GetHashCode();
        }

        private class SingleByteCharStringEnumerator : IEnumerator<ISingleByteChar>
        {
            private SingleByteCharString<TSingleByte> SingleByteCharString;
            private int Position;

            public SingleByteCharStringEnumerator(SingleByteCharString<TSingleByte> singleByteCharString)
            {
                this.SingleByteCharString = singleByteCharString;
                this.Position = -1;
            }

            public ISingleByteChar Current => this.SingleByteCharString.CharSpan[this.Position];

            object IEnumerator.Current => this.SingleByteCharString.CharSpan[this.Position];

            public bool MoveNext() => ++this.Position < this.SingleByteCharString.Length;

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
