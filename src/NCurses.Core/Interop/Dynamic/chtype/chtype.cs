using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.Dynamic.chtype
{
    //TODO: temporary test class -> REMOVE
    [StructLayout(LayoutKind.Sequential)]
    internal struct chtype : ISingleByteNCursesChar, IEquatable<chtype> //chtype & attr_t
    {
        public UInt32 charWithAttr;

        public char Char => (char)(this.charWithAttr & Attrs.CHARTEXT);
        public ulong Attributes => (ulong)((this.charWithAttr ^ (this.charWithAttr & Attrs.COLOR)) & Attrs.ATTRIBUTES);
        public short Color => (short)Constants.PAIR_NUMBER(this.charWithAttr);

        public byte EncodedChar => (byte)(this.charWithAttr & Attrs.CHARTEXT);

        public chtype(sbyte ch)
        {
            this.charWithAttr = (UInt32)ch;
        }

        public chtype(sbyte ch, ulong attr)
            :this(ch)
        {
            this.charWithAttr |= (UInt32)attr;
        }

        public chtype(sbyte ch, ulong attr, short pair)
            : this(ch, attr)
        {
            this.charWithAttr |= (UInt32)NativeNCurses.NCurses.COLOR_PAIR(pair);
            //this.charWithAttr |= (UInt64)((UInt32)NativeNCurses.COLOR_PAIR(pair));
        }

        //attributes only
        public chtype(ulong attr)
        {
            this.charWithAttr = (UInt32)attr;
        }

        public static chtype operator | (chtype ch, ulong attr)
        {
            ch.charWithAttr |= (UInt32)attr;
            return ch;
        }

        public static chtype operator & (chtype ch, ulong attr)
        {
            ch.charWithAttr &= (UInt32)attr;
            return ch;
        }

        public static explicit operator char(chtype ch)
        {
            return ch.Char;
        }

        public static explicit operator sbyte(chtype ch)
        {
            return (sbyte)ch.Char;
        }

        public static explicit operator chtype(char ch)
        {
            if (ch > sbyte.MaxValue)
            {
                throw new InvalidOperationException("This character can not be expressed in 1 byte");
            }
            return new chtype((sbyte)ch);
        }

        public static explicit operator chtype(sbyte ch)
        {
            return new chtype(ch);
        }

        public static implicit operator ulong(chtype ch)
        {
            return ch.charWithAttr;
        }

        public static implicit operator chtype(ulong val)
        {
            return new chtype(val);
        }

        public static bool operator ==(in chtype chLeft, in chtype chRight)
        {
            return chLeft.charWithAttr == chRight.charWithAttr;
        }

        public static bool operator !=(in chtype chLeft, in chtype chRight)
        {
            return chLeft.charWithAttr != chRight.charWithAttr;
        }

        public override bool Equals(object obj)
        {
            if (obj is chtype other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IChar obj)
        {
            if (obj is chtype other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ISingleByteChar obj)
        {
            if (obj is chtype other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(INCursesChar obj)
        {
            if (obj is chtype other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ISingleByteNCursesChar obj)
        {
            if (obj is chtype other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(chtype other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return -1027107954 + this.charWithAttr.GetHashCode();
        }
    }
}
