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
    internal struct SingleByteCharString<TSingleByte> : ISingleByteNCursesCharString, IEquatable<SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
    {
        public unsafe Span<byte> ByteSpan =>
            this.BufferArray is null ? new Span<byte>(this.BufferPointer, this.BufferLength) : new Span<byte>(this.BufferArray, 0, this.BufferLength);
        public unsafe Span<TSingleByte> CharSpan =>
            this.BufferArray is null ? new Span<TSingleByte>(this.BufferPointer, this.BufferLength / Marshal.SizeOf<TSingleByte>()) : MemoryMarshal.Cast<byte, TSingleByte>(this.ByteSpan);

        public int Length { get; }

        public ref TSingleByte GetPinnableReference() => ref this.CharSpan.GetPinnableReference();

        public INCursesChar this[int index] => this.CharSpan[index];

        private unsafe byte* BufferPointer;
        private byte[] BufferArray;
        private int BufferLength;

        public unsafe SingleByteCharString(
            byte* buffer, 
            int length, 
            string str)
        {
            this.BufferPointer = buffer;
            this.BufferLength = length;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer, length),
                str.AsSpan());
        }

        public unsafe SingleByteCharString(
            byte* buffer,
            int length,
            ReadOnlySpan<char> str)
        {
            this.BufferPointer = buffer;
            this.BufferLength = length;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer, length),
                str);
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            int bufferLength,
            string str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer),
                str.AsSpan());
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            int bufferLength,
            ReadOnlySpan<char> str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer),
                str);
        }

        public unsafe SingleByteCharString(
            byte* buffer,
            int bufferLength,
            string str,
            ulong attrs)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer, bufferLength),
                str.AsSpan(), 
                attrs);
        }

        public unsafe SingleByteCharString(
            byte* buffer,
            int bufferLength,
            ReadOnlySpan<char> str,
            ulong attrs)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer, bufferLength),
                str,
                attrs);
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            int bufferLength,
            string str,
            ulong attrs)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer),
                str.AsSpan(),
                attrs);
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            int bufferLength,
            ReadOnlySpan<char> str,
            ulong attrs)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer),
                str,
                attrs);
        }

        public unsafe SingleByteCharString(
            byte* buffer,
            int bufferLength,
            string str, 
            ulong attrs, 
            short pair)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer, bufferLength),
                str.AsSpan(), 
                attrs, 
                pair);
        }

        public unsafe SingleByteCharString(
            byte* buffer,
            int bufferLength,
            ReadOnlySpan<char> str,
            ulong attrs,
            short pair)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer, bufferLength),
                str,
                attrs,
                pair);
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            int bufferLength,
            string str,
            ulong attrs,
            short pair)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer),
                str.AsSpan(),
                attrs,
                pair);
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            int bufferLength,
            ReadOnlySpan<char> str,
            ulong attrs,
            short pair)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(
                new Span<byte>(buffer),
                str,
                attrs,
                pair);
        }

        public unsafe SingleByteCharString(
            byte* buffer,
            int bufferLength,
            int stringLength)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = stringLength;
        }

        public unsafe SingleByteCharString(
            byte[] buffer,
            int bufferLength,
            int stringLength)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferLength = bufferLength;
            this.Length = stringLength;
        }

        public unsafe SingleByteCharString(ref TSingleByte strRef)
        {
            TSingleByte* wideArr = (TSingleByte*)Unsafe.AsPointer<TSingleByte>(ref strRef);

            this.BufferPointer = (byte*)wideArr;
            this.Length = FindStringLength(wideArr, out this.BufferLength);
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
                        charString[i] = CreateSmallChar((sbyte)bytePtr[i], attrs, colorPair);
                    }
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

        internal unsafe static int FindStringLength(TSingleByte* strArr, out int byteLength)
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

            byteLength = length * Marshal.SizeOf<TSingleByte>();
            return --length;
        }

        internal static int FindStringLength(Span<TSingleByte> str, out int byteLength)
        {
            TSingleByte zero = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();

            int length = str.IndexOf(zero);

            if (length == -1)
            {
                length = str.Length;
            }

            byteLength = length * Marshal.SizeOf<TSingleByte>();

            return length;
        }

        IEnumerator IEnumerable.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        IEnumerator<IChar> IEnumerable<IChar>.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        IEnumerator<ISingleByteChar> IEnumerable<ISingleByteChar>.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        IEnumerator<ISingleByteNCursesChar> IEnumerable<ISingleByteNCursesChar>.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        IEnumerator<INCursesChar> IEnumerable<INCursesChar>.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        //TODO: crashes Visual Studio debugger when using netcoreapp2.0
        public static explicit operator string(SingleByteCharString<TSingleByte> str)
        {
            return str.ToString();
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
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
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

        public bool Equals(INCursesCharString obj)
        {
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(ISingleByteNCursesCharString obj)
        {
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(SingleByteCharString<TSingleByte> other)
        {
            //remove null terminator
            Span<byte> thisSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TSingleByte>());
            Span<byte> otherSpan = other.ByteSpan.Slice(0, other.Length * Marshal.SizeOf<TSingleByte>());

            return thisSpan.SequenceEqual(otherSpan);
        }

        public override string ToString()
        {
            Span<TSingleByte> chSpan = this.CharSpan;
            int length = FindStringLength(chSpan, out int byteLength);
            if (length == 0)
            {
                return string.Empty;
            }

            unsafe
            {
                char* charArr = stackalloc char[length];
                for (int i = 0; i < length; i++)
                {
                    charArr[i] = (char)chSpan[i].EncodedChar;
                }

                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, length);
                return charSpan.ToString();
            }
        }

        public override int GetHashCode()
        {
            return this.CharSpan.GetHashCode();
        }

        private class SingleByteCharStringEnumerator : IEnumerator<ISingleByteNCursesChar>, IEnumerator<ISingleByteChar>, IEnumerator<IChar>
        {
            ISingleByteNCursesChar IEnumerator<ISingleByteNCursesChar>.Current => this.SingleByteCharString.CharSpan[this.Position];

            ISingleByteChar IEnumerator<ISingleByteChar>.Current => this.SingleByteCharString.CharSpan[this.Position];

            IChar IEnumerator<IChar>.Current => this.SingleByteCharString.CharSpan[this.Position];

            object IEnumerator.Current => this.SingleByteCharString.CharSpan[this.Position];

            private SingleByteCharString<TSingleByte> SingleByteCharString;
            private int Position;

            public SingleByteCharStringEnumerator(SingleByteCharString<TSingleByte> singleByteCharString)
            {
                this.SingleByteCharString = singleByteCharString;
                this.Position = -1;
            }

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
