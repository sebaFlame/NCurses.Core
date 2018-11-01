using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SingleByteString
{
    public struct schar_t //char
    {
        public sbyte ch;

        public schar_t(byte ch)
        {
            this.ch = (sbyte)ch;
        }

        public schar_t(char ch)
        {
            if(ch > sbyte.MaxValue)
                throw new InvalidOperationException("This character can not be expressed in 1 byte, please use the correct wide overrides");

            this.ch = (sbyte)ch;
        }

        public static explicit operator char(schar_t ch)
        {
            return (char)ch.ch;
        }

        public static explicit operator schar_t(char ch)
        {
            return new schar_t(ch);
        }
    }
}
