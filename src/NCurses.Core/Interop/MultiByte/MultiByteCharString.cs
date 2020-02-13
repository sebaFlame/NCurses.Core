﻿using System;
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
    internal struct MultiByteCharString<TMultiByte> : IMultiByteNCursesCharString, IEquatable<MultiByteCharString<TMultiByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
    {
        public unsafe Span<byte> ByteSpan => 
            this.BufferArray is null ? new Span<byte>(this.BufferPointer, this.BufferPointerLength) : new Span<byte>(this.BufferArray);
        public unsafe Span<TMultiByte> CharSpan =>
            this.BufferArray is null ? new Span<TMultiByte>(this.BufferPointer, this.BufferPointerLength / Marshal.SizeOf<TMultiByte>()) : MemoryMarshal.Cast<byte, TMultiByte>(this.ByteSpan);

        public int Length { get; }

        public ref TMultiByte GetPinnableReference() => ref this.CharSpan.GetPinnableReference();

        public INCursesChar this[int index] => this.CharSpan[index];

        private unsafe byte* BufferPointer;
        private int BufferPointerLength;
        private byte[] BufferArray;

        public unsafe MultiByteCharString(
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

        public unsafe MultiByteCharString(
            byte[] buffer,
            string str)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer), str.AsSpan());
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int bufferLength, 
            string str, 
            ulong attrs)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = bufferLength;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer, bufferLength), str.AsSpan(), attrs);
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            string str,
            ulong attrs)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = str.Length;

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
            this.Length = str.Length;

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
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer), str.AsSpan(), attrs, pair);
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int bufferLength, 
            ReadOnlySpan<byte> str,
            int stringLength,
            Encoding encoding)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = bufferLength;
            this.BufferArray = null;
            this.Length = stringLength;

            CreateCharString(new Span<byte>(buffer, bufferLength), str, encoding);
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            ReadOnlySpan<byte> str,
            int stringLength,
            Encoding encoding)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = stringLength;

            CreateCharString(new Span<byte>(buffer), str, encoding);
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int bufferLength, 
            ReadOnlySpan<byte> str,
            int stringLength,
            Encoding encoding, 
            ulong attrs)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = bufferLength;
            this.BufferArray = null;
            this.Length = stringLength;

            CreateCharString(
                new Span<byte>(buffer, bufferLength),
                str, 
                encoding,
                attrs);
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            ReadOnlySpan<byte> str,
            int stringLength,
            Encoding encoding,
            ulong attrs)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = stringLength;

            CreateCharString(
                new Span<byte>(buffer),
                str,
                encoding,
                attrs);
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int bufferLength, 
            ReadOnlySpan<byte> str,
            int stringLength,
            Encoding encoding, 
            ulong attrs, 
            short color)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = bufferLength;
            this.BufferArray = null;
            this.Length = stringLength;

            CreateCharString(
                new Span<byte>(buffer, bufferLength),
                str, 
                encoding,
                attrs, 
                color);
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            ReadOnlySpan<byte> str,
            int stringLength,
            Encoding encoding,
            ulong attrs,
            short color)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = stringLength;

            CreateCharString(
                new Span<byte>(buffer),
                str,
                encoding,
                attrs,
                color);
        }

        public unsafe MultiByteCharString(
            byte* buffer, 
            int bufferLength,
            int stringLength)
        {
            this.BufferPointer = buffer;
            this.BufferPointerLength = bufferLength;
            this.BufferArray = null;
            this.Length = stringLength;
        }

        public unsafe MultiByteCharString(
            byte[] buffer,
            int stringLength)
        {
            this.BufferArray = buffer;
            this.BufferPointer = (byte*)0;
            this.BufferPointerLength = 0;
            this.Length = stringLength;
        }

        public unsafe MultiByteCharString(ref TMultiByte strRef)
        {
            TMultiByte* wideArr = (TMultiByte*)Unsafe.AsPointer<TMultiByte>(ref strRef);

            this.BufferPointer = (byte*)wideArr;
            this.Length = FindStringLength(wideArr, out this.BufferPointerLength);
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

        internal unsafe static int FindStringLength(TMultiByte* strArr, out int byteLength)
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

            byteLength = length * Marshal.SizeOf<TMultiByte>();
            return --length;
        }

        IEnumerator IEnumerable.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        IEnumerator<IChar> IEnumerable<IChar>.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        IEnumerator<IMultiByteChar> IEnumerable<IMultiByteChar>.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        IEnumerator<IMultiByteNCursesChar> IEnumerable<IMultiByteNCursesChar>.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        IEnumerator<INCursesChar> IEnumerable<INCursesChar>.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        public static explicit operator string(MultiByteCharString<TMultiByte> wStr)
        {
            return wStr.ToString();
        }

        public static bool operator ==(in MultiByteCharString<TMultiByte> wchStrLeft, in MultiByteCharString<TMultiByte> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public static bool operator !=(in MultiByteCharString<TMultiByte> wchStrLeft, in MultiByteCharString<TMultiByte> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public override bool Equals(object obj)
        {
            if (obj is MultiByteCharString<TMultiByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if (obj is MultiByteCharString<TMultiByte> charString)
            {
                return this.Equals(charString);
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

        public bool Equals(INCursesCharString obj)
        {
            if (obj is MultiByteCharString<TMultiByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(IMultiByteNCursesCharString obj)
        {
            if (obj is MultiByteCharString<TMultiByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(MultiByteCharString<TMultiByte> other)
        {
            //remove null terminator
            Span<byte> thisSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TMultiByte>());
            Span<byte> otherSpan = other.ByteSpan.Slice(0, other.Length * Marshal.SizeOf<TMultiByte>());

            return thisSpan.SequenceEqual(otherSpan);
        }

        public override string ToString()
        {
            unsafe
            {
                Span<TMultiByte> chSpan = this.CharSpan;

                //copy all chars (without attributes) into a single array
                int byteLength = Constants.SIZEOF_WCHAR_T * this.Length;
                byte* bArr = stackalloc byte[byteLength];
                Span<byte> destination;
                Span<byte> encodedChar;

                for(int i = 0; i < this.Length; i++)
                {
                    destination = new Span<byte>(bArr + (Constants.SIZEOF_WCHAR_T * i), Constants.SIZEOF_WCHAR_T);
                    encodedChar = chSpan[i].EncodedChar;
                    encodedChar.CopyTo(destination);
                }

                char* charArr = stackalloc char[this.Length];
                NativeNCurses.Encoding.GetChars(bArr, byteLength, charArr, this.Length);

                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, this.Length);
                return charSpan.ToString();
            }
        }

        public override int GetHashCode()
        {
            return this.CharSpan.GetHashCode();
        }

        private class MultiByteCharStringEnumerator : IEnumerator<IMultiByteNCursesChar>, IEnumerator<IMultiByteChar>, IEnumerator<IChar>
        {
            IMultiByteNCursesChar IEnumerator<IMultiByteNCursesChar>.Current => this.MultiByteCharString.CharSpan[this.Position];

            IMultiByteChar IEnumerator<IMultiByteChar>.Current => this.MultiByteCharString.CharSpan[this.Position];

            IChar IEnumerator<IChar>.Current => this.MultiByteCharString.CharSpan[this.Position];

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
