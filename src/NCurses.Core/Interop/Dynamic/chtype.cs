using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.Dynamic
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct chtype : ISingleByteNCursesChar, IEquatable<chtype> //chtype & attr_t
    {
        public UInt32 charWithAttr;

        public ulong Attributes => (ulong)((this.charWithAttr ^ (this.charWithAttr & Attrs.COLOR)) & Attrs.ATTRIBUTES);
        public ushort ColorPair => (ushort)Constants.PAIR_NUMBER(this.charWithAttr);

        public int Char => (int)(this.charWithAttr & Attrs.CHARTEXT);

        public chtype(byte ch)
        {
            this.charWithAttr = (UInt32)ch;
        }

        public chtype(byte ch, ulong attr)
            :this(ch)
        {
            this.charWithAttr |= (UInt32)attr;
        }

        public chtype(byte ch, ulong attr, ushort pair)
            : this(ch, attr)
        {
            this.charWithAttr |= (UInt32)NativeNCurses.COLOR_PAIR(pair);
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

        public static explicit operator chtype(byte ch)
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
