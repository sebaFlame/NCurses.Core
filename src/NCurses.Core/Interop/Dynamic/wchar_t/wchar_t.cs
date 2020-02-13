using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop.Dynamic.wchar_t
{
    internal struct wchar_t : IChar, IEquatable<wchar_t> //wchar_t & wint_t
    {
        private const int wchar_t_size = 2; //Constants.SIZEOF_WCHAR_T

        public unsafe fixed byte @char[wchar_t_size];

        public char Char => (char)this;

        public wchar_t(char c)
        {
            unsafe
            {
                char* charArr = stackalloc char[1];
                charArr[0] = c;

                fixed (byte* bArr = this.@char)
                {
                    NativeNCurses.Encoding.GetBytes(charArr, 1, bArr, wchar_t_size); //Constants.SIZEOF_WCHAR_T
                }
            }
        }

        public wchar_t(Span<byte> encodedBytesChar)
        {
            unsafe
            {
                for (int i = 0; i < encodedBytesChar.Length; i++)
                {
                    this.@char[i] = encodedBytesChar[i];
                }
            }
        }

        public wchar_t(ArraySegment<byte> encodedBytesChar)
        {
            unsafe
            {
                for (int i = 0; i < encodedBytesChar.Count; i++)
                {
                    this.@char[i] = encodedBytesChar.Array[encodedBytesChar.Offset + i];
                }
            }
        }

        public wchar_t(int c)
        {
            unsafe
            {
                fixed (byte* bArr = this.@char)
                {
                    Unsafe.Write<int>(bArr, c);
                }
            }
        }

        /* TODO
        * ReadOnlySpan.SequenceEqual returns false on linux (using netcoreapp2.0 or netcoreapp2.1)
        */
        public static bool operator ==(in wchar_t wchLeft, in wchar_t wchRight)
        {
            unsafe
            {
                fixed (byte* leftPtr = wchLeft.@char, rightPtr = wchRight.@char)
                {
                    return NativeNCurses.EqualBytesLongUnrolled(leftPtr, rightPtr, wchar_t_size);
                }
            }
        }

        public static bool operator !=(in wchar_t wchLeft, in wchar_t wchRight)
        {
            unsafe
            {
                fixed (byte* leftPtr = wchLeft.@char, rightPtr = wchRight.@char)
                {
                    return !NativeNCurses.EqualBytesLongUnrolled(leftPtr, rightPtr, wchar_t_size);
                }
            }
        }

        public static explicit operator char(wchar_t ch)
        {
            char ret;
            unsafe
            {
                char* charArr = stackalloc char[1];
                if (NativeNCurses.Encoding.GetChars(ch.@char, wchar_t_size, charArr, 1) > 0)
                {
                    ret = charArr[0];
                }
                else
                {
                    throw new InvalidCastException("Failed to cast to current encoding");
                }
            }
            return ret;
        }

        public static implicit operator wchar_t(char ch)
        {
            return new wchar_t(ch);
        }

        public bool Equals(IChar other)
        {
            if (other is wchar_t wch)
            {
                return this.Equals(wch);
            }
            return false;
        }

        public bool Equals(wchar_t other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if(obj is wchar_t wch)
            {
                return this.Equals(wch);
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = -355065691;

            unsafe
            {
                fixed (byte* b = this.@char)
                {
                    hashCode = hashCode * -1521134295 + (int)b;
                }
            }

            return hashCode;
        }
    }
}
