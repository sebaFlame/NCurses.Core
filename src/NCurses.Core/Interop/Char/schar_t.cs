using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.Char
{
    public struct schar_t : ISingleByteChar, IEquatable<schar_t>//char
    {
        public byte @char;

        public int Char => @char;

        public schar_t(byte ch)
        {
            this.@char = ch;
        }

        public static explicit operator char(schar_t ch)
        {
            return (char)ch.@char;
        }

        public static bool operator ==(in schar_t wchLeft, in schar_t wchRight) => wchLeft.@char == wchRight.@char;

        public static bool operator !=(in schar_t wchLeft, in schar_t wchRight) => wchLeft.@char != wchRight.@char;

        public override bool Equals(object obj)
        {
            if(obj is schar_t ch)
            {
                return this.Equals(obj);
            }
            return false;
        }

        public bool Equals(ISingleByteChar obj)
        {
            if (obj is schar_t ch)
            {
                return this.Equals(ch);
            }
            return false;
        }

        public bool Equals(IChar obj)
        {
            if (obj is schar_t ch)
            {
                return this.Equals(ch);
            }
            return false;
        }

        public bool Equals(schar_t other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return -355065691 + @char.GetHashCode();
        }
    }
}
