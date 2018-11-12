using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Buffers;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.SingleByte
{
    public class SingleByteChar<TSingleByte> : ISingleByteChar, IDisposable
        where TSingleByte : unmanaged, ISingleByteChar
    {
        internal Memory<TSingleByte> Char;
        internal ref TSingleByte @char => ref this.buffer[0];

        internal static Func<sbyte, TSingleByte> byteCreate;
        internal static Func<sbyte, ulong, TSingleByte> byteAttrCreate;
        internal static Func<sbyte, ulong, short, TSingleByte> byteAttrColorCreate;
        internal static Func<ulong, TSingleByte> attrCreate;

        private TSingleByte[] buffer;

        static SingleByteChar()
        {
            ConstructorInfo ctor;
            ParameterExpression instance, par1, par2, par3;

            instance = Expression.Parameter(typeof(TSingleByte));
            par1 = Expression.Parameter(typeof(sbyte));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(sbyte) });
            byteCreate = Expression.Lambda<Func<sbyte, TSingleByte>>(Expression.New(ctor, par1), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(sbyte), typeof(ulong) });
            byteAttrCreate = Expression.Lambda<Func<sbyte, ulong, TSingleByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(sbyte), typeof(ulong), typeof(short) });
            byteAttrColorCreate = Expression.Lambda<Func<sbyte, ulong, short, TSingleByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(ulong) });
            attrCreate = Expression.Lambda<Func<ulong, TSingleByte>>(Expression.New(ctor, par1), par1).Compile();
        }

        char INCursesChar.Char => this.@char.Char;
        public ulong Attributes => this.@char.Attributes;
        public short Color => this.@char.Color;

        private SingleByteChar()
        {
            this.buffer = ArrayPool<TSingleByte>.Shared.Rent(1);
            this.Char = new Memory<TSingleByte>(buffer);
        }

        public SingleByteChar(ref TSingleByte ch)
            :this()
        {
            this.buffer[0] = ch;
        }

        public SingleByteChar(INCursesChar ch)
            :this()
        {
            if (ch is TSingleByte chParsed)
                this.buffer[0] = chParsed;
            else
                throw new InvalidOperationException("Unsupported character type discovered");
        }

        public SingleByteChar(sbyte ch)
            :this()
        {
            this.buffer[0] = byteCreate(ch);
        }

        public SingleByteChar(sbyte ch, ulong attrs)
            :this()
        {
            this.buffer[0] = byteAttrCreate(ch, attrs);
        }

        public SingleByteChar(sbyte ch, ulong attrs, short pair)
            :this()
        {
            this.buffer[0] = byteAttrColorCreate(ch, attrs, pair);
        }

        public SingleByteChar(char ch)
            :this()
        {
            if (ch > sbyte.MaxValue)
                throw new InvalidOperationException("This character can not be expressed in 1 byte");
            this.buffer[0] = byteCreate((sbyte)ch);
        }

        public SingleByteChar(char ch, ulong attrs)
            :this()
        {
            if (ch > sbyte.MaxValue)
                throw new InvalidOperationException("This character can not be expressed in 1 byte");
            this.buffer[0] = byteAttrCreate((sbyte)ch, attrs);
        }

        public SingleByteChar(char ch, ulong attrs, short pair)
            :this()
        {
            if (ch > sbyte.MaxValue)
                throw new InvalidOperationException("This character can not be expressed in 1 byte");
            this.buffer[0] = byteAttrColorCreate((sbyte)ch, attrs, pair);
        }

        public SingleByteChar(ulong attrs)
            :this()
        {
            this.buffer[0] = attrCreate(attrs);
        }

        internal ref readonly TSingleByte GetCharReference()
        {
            return ref this.@char;
        }

        public void Dispose()
        {
            ArrayPool<TSingleByte>.Shared.Return(this.buffer);
        }

        public static bool operator ==(in SingleByteChar<TSingleByte> chLeft, in SingleByteChar<TSingleByte> chRight)
        {
            return chLeft.@char.Equals(chRight.@char);
        }

        public static bool operator !=(in SingleByteChar<TSingleByte> chLeft, in SingleByteChar<TSingleByte> chRight)
        {
            return !chLeft.@char.Equals(chRight.@char);
        }

        public static explicit operator ulong(SingleByteChar<TSingleByte> ch)
        {
            Span<TSingleByte> span = new Span<TSingleByte>(ch.buffer);
            Span<byte> spanBytes = MemoryMarshal.AsBytes<TSingleByte>(span);
            ulong ret;
            unsafe
            {
                fixed (byte* b = spanBytes)
                {
                    ulong* val = (ulong*)b;
                    ret = *val;
                }
            }
            return ret;
        }

        public bool Equals(INCursesChar obj)
        {
            if (obj is ISingleByteChar other)
                return this.Equals(other);
            return false;
        }

        public bool Equals(ISingleByteChar obj)
        {
            if (obj is SingleByteChar<TSingleByte> other)
                return this.@char.Equals(other.@char);
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is SingleByteChar<TSingleByte> other)
                return this.@char.Equals(other.@char);
            return false;
        }

        public override int GetHashCode()
        {
            return -158990394 + EqualityComparer<TSingleByte>.Default.GetHashCode(this.@char);
        }

        public static explicit operator TSingleByte(SingleByteChar<TSingleByte> ch)
        {
            return ch.@char;
        }

        public static explicit operator char(SingleByteChar<TSingleByte> ch)
        {
            return (ch as INCursesChar).Char;
        }

        public static explicit operator SingleByteChar<TSingleByte>(char ch)
        {
            return new SingleByteChar<TSingleByte>(ch);
        }
    }
}
