using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Buffers;

namespace NCurses.Core.Interop.SingleByte
{
    public class NCursesSCHAR<TSmall> : INCursesSCHAR, IDisposable
        where TSmall : unmanaged, INCursesSCHAR
    {
        internal Memory<TSmall> SCHAR;
        internal ref TSmall schar => ref this.buffer[0];

        internal static Func<sbyte, TSmall> byteCreate;
        internal static Func<sbyte, ulong, TSmall> byteAttrCreate;
        internal static Func<sbyte, ulong, short, TSmall> byteAttrColorCreate;
        internal static Func<ulong, TSmall> attrCreate;

        private TSmall[] buffer;

        static NCursesSCHAR()
        {
            ConstructorInfo ctor;
            ParameterExpression instance, par1, par2, par3;

            instance = Expression.Parameter(typeof(TSmall));
            par1 = Expression.Parameter(typeof(sbyte));
            ctor = typeof(TSmall).GetConstructor(new Type[] { typeof(sbyte) });
            byteCreate = Expression.Lambda<Func<sbyte, TSmall>>(Expression.New(ctor, par1), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TSmall).GetConstructor(new Type[] { typeof(sbyte), typeof(ulong) });
            byteAttrCreate = Expression.Lambda<Func<sbyte, ulong, TSmall>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = typeof(TSmall).GetConstructor(new Type[] { typeof(sbyte), typeof(ulong), typeof(short) });
            byteAttrColorCreate = Expression.Lambda<Func<sbyte, ulong, short, TSmall>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TSmall).GetConstructor(new Type[] { typeof(ulong) });
            attrCreate = Expression.Lambda<Func<ulong, TSmall>>(Expression.New(ctor, par1), par1).Compile();
        }

        public char Char => this.schar.Char;
        public ulong Attributes => this.schar.Attributes;
        public short Color => this.schar.Color;

        private NCursesSCHAR()
        {
            this.buffer = ArrayPool<TSmall>.Shared.Rent(1);
            this.SCHAR = new Memory<TSmall>(buffer);
        }

        public NCursesSCHAR(ref TSmall ch)
            :this()
        {
            this.buffer[0] = ch;
        }

        public NCursesSCHAR(INCursesChar ch)
            :this()
        {
            if (ch is TSmall chParsed)
                this.buffer[0] = chParsed;
            else
                throw new InvalidOperationException("Unsupported character type discovered");
        }

        public NCursesSCHAR(sbyte ch)
            :this()
        {
            this.buffer[0] = byteCreate(ch);
        }

        public NCursesSCHAR(sbyte ch, ulong attrs)
            :this()
        {
            this.buffer[0] = byteAttrCreate(ch, attrs);
        }

        public NCursesSCHAR(sbyte ch, ulong attrs, short pair)
            :this()
        {
            this.buffer[0] = byteAttrColorCreate(ch, attrs, pair);
        }

        public NCursesSCHAR(char ch)
            :this()
        {
            if (ch > sbyte.MaxValue)
                throw new InvalidOperationException("This character can not be expressed in 1 byte");
            this.buffer[0] = byteCreate((sbyte)ch);
        }

        public NCursesSCHAR(char ch, ulong attrs)
            :this()
        {
            if (ch > sbyte.MaxValue)
                throw new InvalidOperationException("This character can not be expressed in 1 byte");
            this.buffer[0] = byteAttrCreate((sbyte)ch, attrs);
        }

        public NCursesSCHAR(char ch, ulong attrs, short pair)
            :this()
        {
            if (ch > sbyte.MaxValue)
                throw new InvalidOperationException("This character can not be expressed in 1 byte");
            this.buffer[0] = byteAttrColorCreate((sbyte)ch, attrs, pair);
        }

        public NCursesSCHAR(ulong attrs)
            :this()
        {
            this.buffer[0] = attrCreate(attrs);
        }

        internal ref readonly TSmall GetCharReference()
        {
            return ref this.schar;
        }

        public void Dispose()
        {
            ArrayPool<TSmall>.Shared.Return(this.buffer);
        }

        public static bool operator ==(in NCursesSCHAR<TSmall> chLeft, in NCursesSCHAR<TSmall> chRight)
        {
            return chLeft.schar.Equals(chRight.schar);
        }

        public static bool operator !=(in NCursesSCHAR<TSmall> chLeft, in NCursesSCHAR<TSmall> chRight)
        {
            return !chLeft.schar.Equals(chRight.schar);
        }

        public bool Equals(INCursesChar obj)
        {
            if (obj is INCursesSCHAR other)
                return this.Equals(other);
            return false;
        }

        public bool Equals(INCursesSCHAR obj)
        {
            if (obj is NCursesSCHAR<TSmall> other)
                return this.schar.Equals(other.schar);
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is NCursesSCHAR<TSmall> other)
                return this.schar.Equals(other.schar);
            return false;
        }

        public override int GetHashCode()
        {
            return -158990394 + EqualityComparer<TSmall>.Default.GetHashCode(this.schar);
        }

        public static explicit operator TSmall(NCursesSCHAR<TSmall> ch)
        {
            return ch.schar;
        }

        public static explicit operator char(NCursesSCHAR<TSmall> ch)
        {
            return ch.Char;
        }

        public static explicit operator NCursesSCHAR<TSmall>(char ch)
        {
            return new NCursesSCHAR<TSmall>(ch);
        }
    }
}
