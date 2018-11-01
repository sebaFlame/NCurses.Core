using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.Dynamic.wchar_t
{
    internal struct wchar_t //wchar_t & wint_t
    {
        private const int wchar_t_size = 2; //Constants.SIZEOF_WCHAR_T

        public unsafe fixed byte ch[wchar_t_size];

        public wchar_t(char c)
        {
            unsafe
            {
                char* charArr = stackalloc char[1];
                charArr[0] = c;

                fixed (byte* bArr = this.ch)
                {
                    NativeNCurses.Encoding.GetBytes(charArr, 1, bArr, wchar_t_size); //Constants.SIZEOF_WCHAR_T
                }
            }
        }

        public static explicit operator char(wchar_t ch)
        {
            char ret;
            unsafe
            {
                char* charArr = stackalloc char[1];
                if (NativeNCurses.Encoding.GetChars(ch.ch, wchar_t_size, charArr, 1) > 0)
                    ret = charArr[0];
                else
                    throw new InvalidCastException("Failed to cast to current encoding");
            }
            return ret;
        }

        public static implicit operator wchar_t(char ch)
        {
            return new wchar_t(ch);
        }
    }
}
