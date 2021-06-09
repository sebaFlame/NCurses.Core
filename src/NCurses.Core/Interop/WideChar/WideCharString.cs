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
            this.BufferArray is null ? new Span<byte>(this.BufferPointer, this.BufferLength) : new Span<byte>(this.BufferArray, 0, this.BufferLength);
        public unsafe Span<TWideChar> CharSpan =>
            this.BufferArray is null ? new Span<TWideChar>(this.BufferPointer, this.BufferLength / Marshal.SizeOf<TWideChar>()) : MemoryMarshal.Cast<byte, TWideChar>(this.ByteSpan);

        public int Length { get; }

        public ref TWideChar GetPinnableReference() => ref this.CharSpan.GetPinnableReference();

        private unsafe byte* BufferPointer;
        private byte[] BufferArray;
        private int BufferLength;

        public unsafe WideCharString(
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

        public unsafe WideCharString(
            byte* buffer,
            int bufferLength,
            ReadOnlySpan<char> str)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer, bufferLength), str);
        }

        public WideCharString(
            byte[] buffer,
            int bufferLength,
            string str)
        {
            this.BufferArray = buffer;

            unsafe
            {
                this.BufferPointer = (byte*)0;
            }
            
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer), str.AsSpan());
        }

        public WideCharString(
            byte[] buffer,
            int bufferLength,
            ReadOnlySpan<char> str)
        {
            this.BufferArray = buffer;

            unsafe
            {
                this.BufferPointer = (byte*)0;
            }
            
            this.BufferLength = bufferLength;
            this.Length = str.Length;

            CreateCharString(new Span<byte>(buffer), str);
        }

        public unsafe WideCharString(
            byte* buffer,
            int bufferLength,
            ReadOnlySpan<byte> str,
            int charLength,
            Encoding encoding)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = charLength;

            CreateCharString(new Span<byte>(buffer, bufferLength), str, charLength, encoding);
        }

        public unsafe WideCharString(
            byte* buffer,
            int bufferLength,
            ReadOnlySequence<byte> str,
            int charLength,
            Encoding encoding)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = charLength;

            CreateCharString(new Span<byte>(buffer, bufferLength), str, charLength, encoding);
        }

        public WideCharString(
            byte[] buffer,
            int bufferLength,
            ReadOnlySpan<byte> str,
            int charLength,
            Encoding encoding)
        {
            this.BufferArray = buffer;

            unsafe
            {
                this.BufferPointer = (byte*)0;
            }
            
            this.BufferLength = bufferLength;
            this.Length = charLength;

            CreateCharString(new Span<byte>(buffer), str, charLength, encoding);
        }

        public WideCharString(
            byte[] buffer,
            int bufferLength,
            ReadOnlySequence<byte> str,
            int charLength,
            Encoding encoding)
        {
            this.BufferArray = buffer;

            unsafe
            {
                this.BufferPointer = (byte*)0;
            }
            
            this.BufferLength = bufferLength;
            this.Length = charLength;

            CreateCharString(new Span<byte>(buffer), str, charLength, encoding);
        }

        public unsafe WideCharString(
            byte* buffer, 
            int bufferLength,
            int stringLength)
        {
            this.BufferPointer = buffer;
            this.BufferLength = bufferLength;
            this.BufferArray = null;
            this.Length = stringLength;
        }

        public WideCharString(
            byte[] buffer,
            int bufferLength,
            int stringLength)
        {
            this.BufferArray = buffer;

            unsafe
            {
                this.BufferPointer = (byte*)0;
            }
            
            this.BufferLength = bufferLength;
            this.Length = stringLength;
        }

        public unsafe WideCharString(ref TWideChar strRef)
        {
            TWideChar* wideArr = (TWideChar*)Unsafe.AsPointer<TWideChar>(ref strRef);

            this.BufferPointer = (byte*)wideArr;
            this.Length = FindStringLength(wideArr, out this.BufferLength);
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

                lock (NativeNCurses.SyncRoot)
                {
                    Encoder encoder = NativeNCurses.MultiByteEncoder;
                    encoder.Reset();

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
        }

        private static unsafe void CreateCharString(
            Span<byte> buffer,
            ReadOnlySpan<byte> bytes,
            int charLength,
            Encoding encoding)
        {
            fixed (byte* originalBytes = bytes)
            {
                Decoder decoder = encoding.GetDecoder();
                char* chars = stackalloc char[charLength];

                decoder.Convert(
                    originalBytes,
                    bytes.Length,
                    chars,
                    charLength,
                    true,
                    out int bytesUsed,
                    out int charsUsed,
                    out bool completed);

                if (!completed)
                {
                    throw new InvalidOperationException($"Could not cast {encoding.EncodingName} to characters");
                }

                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(chars, charLength);
                CreateCharString(buffer, charSpan);
            }
        }

        private static unsafe void CreateCharString(
            Span<byte> buffer,
            ReadOnlySequence<byte> sequence,
            int charLength,
            Encoding encoding)
        {
            Decoder decoder = encoding.GetDecoder();
            char* chars = stackalloc char[charLength];

            int totalCharsUsed = 0;
            int charsUsed = 0;
            bool completed = false;

            foreach (ReadOnlyMemory<byte> memory in sequence)
            {
                if (memory.IsEmpty)
                {
                    continue;
                }

                fixed (byte* originalBytes = memory.Span)
                {
                    decoder.Convert
                    (
                        originalBytes,
                        memory.Length,
                        chars + totalCharsUsed,
                        charLength - totalCharsUsed,
                        false,
                        out _,
                        out charsUsed,
                        out completed
                    );

                    totalCharsUsed += charsUsed;
                }
            }

            if (!completed)
            {
                throw new InvalidOperationException($"Could not cast {encoding.EncodingName} to characters");
            }

            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(chars, charLength);
            CreateCharString(buffer, charSpan);
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

        internal static int FindStringLength(Span<TWideChar> str, out int byteLength)
        {
            TWideChar zero = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyCharInternal();
            
            int length = str.IndexOf(zero);

            if (length == -1)
            {
                length = str.Length;
            }

            byteLength = length * Marshal.SizeOf<TWideChar>();

            return length;
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
            int length = FindStringLength(this.CharSpan, out int byteLength);
            if (length == 0)
            {
                return string.Empty;
            }

            unsafe
            {
                Span<byte> bSpan = this.ByteSpan.Slice(0, length * Marshal.SizeOf<TWideChar>());
                char* charArr = stackalloc char[length];

                lock (NativeNCurses.SyncRoot)
                {
                    Decoder decoder = NativeNCurses.MultiByteDecoder;
                    decoder.Reset();

                    fixed (byte* b = bSpan)
                    {
                        decoder.GetChars(b, bSpan.Length, charArr, length, true);
                    }
                }

                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, length);
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
