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

namespace NCurses.Core.Interop.MultiByte
{
    internal struct MultiByteCharString<TMultiByte> : IMultiByteCharString, IEquatable<MultiByteCharString<TMultiByte>>
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
    {
        public unsafe Span<byte> ByteSpan => 
            this.BufferArray is null ? new Span<byte>(this.BufferPointer, this.BufferPointerLength) : new Span<byte>(this.BufferArray);
        public unsafe Span<TMultiByte> CharSpan =>
            this.BufferArray is null ? new Span<TMultiByte>(this.BufferPointer, this.BufferPointerLength / Marshal.SizeOf<TMultiByte>()) : MemoryMarshal.Cast<byte, TMultiByte>(this.ByteSpan);

        public int Length => this.CharSpan.Length;

        public ref TMultiByte GetPinnableReference() => ref this.CharSpan.GetPinnableReference();

        private unsafe byte* BufferPointer;
        private int BufferPointerLength;

        private byte[] BufferArray;

        public INCursesChar this[int index] => this.CharSpan[index];

        public unsafe MultiByteCharString(
            byte* buffer, 
            int length, 
            string str)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(new Span<byte>(buffer, length), str.AsSpan());
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            string str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(new Span<byte>(buffer), str.AsSpan());
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int length, 
            string str, 
            ulong attrs)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(new Span<byte>(buffer, length), str.AsSpan(), attrs);
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            string str,
            ulong attrs)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(new Span<byte>(buffer), str.AsSpan(), attrs);
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int length, 
            string str, 
            ulong attrs, 
            short pair)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(new Span<byte>(buffer, length), str.AsSpan(), attrs, pair);
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            string str,
            ulong attrs,
            short pair)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(new Span<byte>(buffer), str.AsSpan(), attrs, pair);
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int length, 
            ReadOnlySpan<byte> str, 
            Encoding encoding)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(new Span<byte>(buffer, length), str, encoding);
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            ReadOnlySpan<byte> str,
            Encoding encoding)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(new Span<byte>(buffer), str, encoding);
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int length, 
            ReadOnlySpan<byte> str, 
            Encoding encoding, 
            ulong attrs)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(
                new Span<byte>(buffer, length),
                str, 
                encoding,
                attrs);
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            ReadOnlySpan<byte> str,
            Encoding encoding,
            ulong attrs)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(
                new Span<byte>(buffer),
                str,
                encoding,
                attrs);
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int length, 
            ReadOnlySpan<byte> str, 
            Encoding encoding, 
            ulong attrs, 
            short color)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;

            CreateCharString(
                new Span<byte>(buffer, length),
                str, 
                encoding,
                attrs, 
                color);
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            ReadOnlySpan<byte> str,
            Encoding encoding,
            ulong attrs,
            short color)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;

            CreateCharString(
                new Span<byte>(buffer),
                str,
                encoding,
                attrs,
                color);
        }

        public unsafe MultiByteCharString(byte* buffer, int length)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = length;
            this.BufferArray = null;
        }

        public unsafe MultiByteCharString(byte[] buffer)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
        }

        public unsafe MultiByteCharString(ref TMultiByte strRef)
        {
            TMultiByte* wideArr = (TMultiByte*)Unsafe.AsPointer<TMultiByte>(ref strRef);

            this.BufferPointer = (byte*)wideArr;
            this.BufferPointerLength = FindStringLength(wideArr);
            this.BufferArray = null;
        }

        private static unsafe void CreateCharString(
            Span<byte> buffer,
            ReadOnlySpan<char> charArray, 
            ulong attrs = 0, short 
            colorPair = 0)
        {
            Span<TMultiByte> charString = MemoryMarshal.Cast<byte, TMultiByte>(buffer);

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

                    charString[i] = CreateWideChar(new Span<byte>(encodedBytes + bytePosition, bytesUsed), attrs, colorPair);
                }
            }
        }

        private static unsafe void CreateCharString(
            Span<byte> buffer,
            ReadOnlySpan<byte> bytes, 
            Encoding encoding, 
            ulong attrs = 0, 
            short colorPair = 0)
        {
            Span<TMultiByte> charString = MemoryMarshal.Cast<byte, TMultiByte>(buffer);

            fixed (byte* originalBytes = bytes)
            {
                Decoder decoder = encoding.GetDecoder();
                char* chars = stackalloc char[charString.Length];

                decoder.Convert(
                    originalBytes, 
                    bytes.Length, 
                    chars, 
                    charString.Length, 
                    true, 
                    out int bytesUsed, 
                    out int charsUsed, 
                    out bool completed);

                if (!completed)
                {
                    throw new InvalidOperationException($"Could not cast {encoding.EncodingName} to characters");
                }

                ReadOnlySpan<char> charSpan = charSpan = new ReadOnlySpan<char>(chars, charString.Length);
                CreateCharString(buffer, charSpan, attrs, colorPair);
            }
        }

        public static int GetByteCount(string str, bool addNullTerminator = true) =>
            MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, addNullTerminator);

        public static int GetByteCount(int length, bool addNullTerminator = true) =>
            MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(length, addNullTerminator);

        public static unsafe int GetByteCount(ReadOnlySpan<byte> bytes, Encoding encoding, bool addNullTerminator = true) =>
            MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(bytes, encoding, addNullTerminator);

        private static TMultiByte CreateWideChar(ArraySegment<byte> encodedBytes, ulong attrs = 0, short colorPair = 0)
        {
            if (attrs == 0 && colorPair == 0)
            {
                return MultiByteCharFactoryInternal<TMultiByte>.CreateCharFromArraySegment(encodedBytes);
            }
            else if (colorPair == 0)
            {
                return MultiByteCharFactoryInternal<TMultiByte>.CreateCharWithAttributeFromArraySegment(encodedBytes, attrs);
            }
            else
            {
                return MultiByteCharFactoryInternal<TMultiByte>.CreateCharWithAttributeAndColorPairFromArraySegment(encodedBytes, attrs, colorPair);
            }
        }

        private static TMultiByte CreateWideChar(Span<byte> encodedBytes, ulong attrs = 0, short colorPair = 0)
        {
            if (attrs == 0 && colorPair == 0)
            {
                return MultiByteCharFactoryInternal<TMultiByte>.CreateCharFromSpan(encodedBytes);
            }
            else if (colorPair == 0)
            {
                return MultiByteCharFactoryInternal<TMultiByte>.CreateCharWithAttributeFromSpan(encodedBytes, attrs);
            }
            else
            {
                return MultiByteCharFactoryInternal<TMultiByte>.CreateCharWithAttributeAndColorPairFromSpan(encodedBytes, attrs, colorPair);
            }
        }

        internal unsafe static int FindStringLength(TMultiByte* strArr)
        {
            TMultiByte zero = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            TMultiByte val;
            int length = 0;

            while (true)
            {
                val = *(strArr + (length++ * Marshal.SizeOf<TMultiByte>()));
                if (zero.Equals(val))
                {
                    break;
                }
            }
            return --length;
        }

        IEnumerator<IMultiByteChar> IEnumerable<IMultiByteChar>.GetEnumerator()
        {
            return new MultiByteCharStringEnumerator(this);
        }

        IEnumerator<INCursesChar> IEnumerable<INCursesChar>.GetEnumerator()
        {
            return new MultiByteCharStringEnumerator(this);
        }

        public IEnumerator<IChar> GetEnumerator()
        {
            return new MultiByteCharStringEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return  new MultiByteCharStringEnumerator(this);
        }

        public static explicit operator string(MultiByteCharString<TMultiByte> wStr)
        {
            unsafe
            {
                char* charArr = stackalloc char[wStr.Length];
                for (int i = 0; i < wStr.Length; i++)
                {
                    charArr[i] = wStr.CharSpan[i].Char;
                }
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, wStr.Length);
                return charSpan.ToString();
            }
        }

        public static bool operator ==(in MultiByteCharString<TMultiByte> wchStrLeft, in MultiByteCharString<TMultiByte> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public static bool operator !=(in MultiByteCharString<TMultiByte> wchStrLeft, in MultiByteCharString<TMultiByte> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public bool Equals(INCursesCharString obj)
        {
            if (obj is IMultiByteCharString other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if (obj is IMultiByteCharString other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is IMultiByteCharString other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IMultiByteCharString obj)
        {
            if (obj is MultiByteCharString<TMultiByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(MultiByteCharString<TMultiByte> other)
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

        private class MultiByteCharStringEnumerator : IEnumerator<IMultiByteChar>
        {
            public IMultiByteChar Current => this.MultiByteCharString.CharSpan[this.Position];

            object IEnumerator.Current => this.MultiByteCharString.CharSpan[this.Position];

            private MultiByteCharString<TMultiByte> MultiByteCharString;
            private int Position;

            public MultiByteCharStringEnumerator(MultiByteCharString<TMultiByte> multiByteCharString)
            {
                this.MultiByteCharString = multiByteCharString;
                this.Reset();
            }

            public bool MoveNext() => ++this.Position < this.MultiByteCharString.Length;

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
