using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.MultiByte;

namespace NCurses.Core.Interop.Dynamic.cchar_t
{
    /* TODO
     * temporary test class -> REMOVE
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct cchar_t : IMultiByteNCursesChar, IEquatable<cchar_t> //cchar_t
    {
        private const int charGlobalLength = 10; //Constants.SIZEOF_WCHAR_T * Constants.CCHARW_MAX

        public chtype.chtype attr;
        public unsafe fixed byte chars[charGlobalLength];
        //can be 32767 instead of 0 on linux (short.Max? == 16bit max color of (short)ColorPair)
        public int ext_color;

        public char Char => (char)this;
        public short ColorPair => attr.ColorPair;
        public ulong Attributes => (ulong)(this.attr ^ (this.attr & Attrs.COLOR));

        public unsafe Span<byte> EncodedChar
        {
            get
            {
                fixed (byte* bArr = chars)
                {
                    return new Span<byte>(bArr, Constants.SIZEOF_WCHAR_T);
                }
            }
        }

        public cchar_t(char c)
        {
            this.attr = 0;
            this.ext_color = 0;

            unsafe
            {
                char* ch = stackalloc char[1];
                ch[0] = c;
                //TODO: cache bytesUsed as length
                fixed (byte* b = this.chars)
                {
                    NativeNCurses.Encoding.GetBytes(ch, 1, b, charGlobalLength);
                }
            }
        }

        public cchar_t(char c, ulong attrs)
            : this(c)
        {
            this.attr = attrs;
        }

        public cchar_t(char c, ulong attrs, short pair)
            : this(c, attrs)
        {
            this.ext_color = pair;
            this.attr |= (ulong)NativeNCurses.COLOR_PAIR(pair);
        }

        public cchar_t(ArraySegment<byte> encodedBytesChar)
        {
            unsafe
            {
                for (int i = 0; i < encodedBytesChar.Count; i++)
                {
                    this.chars[i] = encodedBytesChar.Array[encodedBytesChar.Offset + i];
                }
            }

            this.attr = 0;
            this.ext_color = 0;
        }

        public cchar_t(ArraySegment<byte> encodedBytesChar, ulong attrs)
            : this(encodedBytesChar)
        {
            this.attr = attrs;
        }

        public cchar_t(ArraySegment<byte> encodedBytesChar, ulong attrs, short pair)
            : this(encodedBytesChar, attrs)
        {
            this.ext_color = pair;
            this.attr |= (ulong)NativeNCurses.COLOR_PAIR(pair);
        }

        public cchar_t(Span<byte> encodedBytesChar)
        {
            unsafe
            {
                for (int i = 0; i < encodedBytesChar.Length; i++)
                {
                    this.chars[i] = encodedBytesChar[i];
                }
            }

            this.attr = 0;
            this.ext_color = 0;
        }

        public cchar_t(Span<byte> encodedBytesChar, ulong attrs)
            : this(encodedBytesChar)
        {
            this.attr = attrs;
        }

        public cchar_t(Span<byte> encodedBytesChar, ulong attrs, short pair)
            : this(encodedBytesChar, attrs)
        {
            this.ext_color = pair;
            this.attr |= (ulong)NativeNCurses.COLOR_PAIR(pair);
        }

        public static explicit operator cchar_t(char ch)
        {
            return new cchar_t(ch);
        }

        //TODO: cache bytesUsed as length
        public static explicit operator char(cchar_t ch)
        {
            char ret = '\0';
            unsafe
            {
                char* chArr = stackalloc char[charGlobalLength];
                if (NativeNCurses.Encoding.GetChars(ch.chars, charGlobalLength, chArr, charGlobalLength / 2) > 0)
                    ret = chArr[0];
                else
                    throw new InvalidOperationException("Failed to cast to character");
            }
            return ret;
        }

        /* TODO
        * ReadOnlySpan.SequenceEqual returns false on linux (using netcoreapp2.0 or netcoreapp2.1)
        */
        public static bool operator ==(in cchar_t wchLeft, in cchar_t wchRight)
        {
            unsafe
            {
                fixed (byte* leftPtr = wchLeft.chars, rightPtr = wchRight.chars)
                    return NativeNCurses.EqualBytesLongUnrolled(leftPtr, rightPtr, charGlobalLength)
                        && wchLeft.attr == wchRight.attr
                        && wchLeft.ext_color == wchRight.ext_color;
            }
        }

        public static bool operator !=(in cchar_t wchLeft, in cchar_t wchRight)
        {
            unsafe
            {
                fixed (byte* leftPtr = wchLeft.chars, rightPtr = wchRight.chars)
                    return !(NativeNCurses.EqualBytesLongUnrolled(leftPtr, rightPtr, charGlobalLength)
                        && wchLeft.attr == wchRight.attr
                        && wchLeft.ext_color == wchRight.ext_color);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IChar obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IMultiByteChar obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(INCursesChar obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IMultiByteNCursesChar obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(cchar_t other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            int hashCode = -946517152;
            hashCode = hashCode * -1521134295 + EqualityComparer<chtype.chtype>.Default.GetHashCode(this.attr);
            unsafe
            {
                fixed(byte* b = this.chars)
                {
                    hashCode = hashCode * -1521134295 + (int)b;
                }
            }
            hashCode = hashCode * -1521134295 + this.ext_color.GetHashCode();
            return hashCode;
        }
    }
}
