using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Buffers;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.MultiByte
{
    public class MultiByteChar<TMultiByte> : IMultiByteChar, IDisposable
        where TMultiByte : unmanaged, IMultiByteChar
    {
        internal Memory<TMultiByte> Char;
        internal ref TMultiByte @char => ref this.buffer[0];

        internal static Func<char, TMultiByte> charCreate;
        internal static Func<char, ulong, TMultiByte> charAttrCreate;
        internal static Func<char, ulong, short, TMultiByte> charAttrColorCreate;

        private TMultiByte[] buffer;

        //no boxing?
        //https://stackoverflow.com/questions/24259261/avoiding-boxing-in-generic-blackboard
        //public char Char // => (char)(object)this.WCHAR[0];
        //{
        //    get
        //    {
        //        char val = default(char);
        //        __refvalue(__makeref(val), TWide) = this.WCHAR[0];
        //        return val;
        //    }
        //}

        static MultiByteChar()
        {
            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(char));
            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(char) });
            charCreate = Expression.Lambda<Func<char, TMultiByte>>(Expression.New(ctor, par1), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(char), typeof(ulong) });
            charAttrCreate = Expression.Lambda<Func<char, ulong, TMultiByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(char), typeof(ulong), typeof(short) });
            charAttrColorCreate = Expression.Lambda<Func<char, ulong, short, TMultiByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();
        }

        char INCursesChar.Char => this.@char.Char;
        public ulong Attributes => this.@char.Attributes;
        public short Color => this.@char.Color;

        private MultiByteChar()
        {
            this.buffer = ArrayPool<TMultiByte>.Shared.Rent(1);
            this.Char = new Memory<TMultiByte>(buffer);
        }

        internal MultiByteChar(ref TMultiByte wCh)
            : this()
        {
            this.buffer[0] = wCh;
        }

        public MultiByteChar(INCursesChar wCh)
            : this()
        {
            if (wCh is TMultiByte wChParsed)
                this.buffer[0] = wChParsed;
            else
                throw new InvalidOperationException("Unsupported character type discovered");
        }

        public MultiByteChar(char ch)
            : this()
        {
            this.buffer[0] = charCreate(ch);
        }

        public MultiByteChar(char ch, ulong attr)
            : this()
        {
            this.buffer[0] = charAttrCreate(ch, attr);
        }

        public MultiByteChar(char ch, ulong attr, short pair)
            : this()
        {
            this.buffer[0] = charAttrColorCreate(ch, attr, pair);
        }

        ~MultiByteChar()
        {
            this.Dispose();
        }

        internal ref readonly TMultiByte GetCharReference()
        {
            return ref this.buffer[0];
        }

        public void Dispose()
        {
            ArrayPool<TMultiByte>.Shared.Return(this.buffer);
        }

        public static explicit operator TMultiByte(MultiByteChar<TMultiByte> ch)
        {
            return ch.@char;
        }

        public static explicit operator MultiByteChar<TMultiByte>(char ch)
        {
            return new MultiByteChar<TMultiByte>(ch);
        }

        public static explicit operator char(MultiByteChar<TMultiByte> ch)
        {
            return (ch as INCursesChar).Char;
        }

        public static bool operator ==(in MultiByteChar<TMultiByte> wchLeft, in MultiByteChar<TMultiByte> wchRight)
        {
            //return wchLeft.wchar.Equals(wchRight.wchar);
            return wchLeft.Equals(wchRight);
        }

        public static bool operator !=(in MultiByteChar<TMultiByte> wchLeft, in MultiByteChar<TMultiByte> wchRight)
        {
            //return !wchLeft.wchar.Equals(wchRight.wchar);
            return !wchLeft.Equals(wchRight);
        }

        //TODO: boxing
        public override bool Equals(object obj)
        {
            if (obj is IMultiByteChar other)
                return this.Equals(other);
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -2133428647;
            hashCode = hashCode * -1521134295 + this.Char.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Attributes.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Color.GetHashCode();
            return hashCode;
        }

        public bool Equals(IMultiByteChar obj)
        {
            if (obj is MultiByteChar<TMultiByte> other)
            {
                return this.@char.Equals(other.@char);
                //ReadOnlySpan<byte> left = MemoryMarshal.AsBytes<TWide>(this.WCHAR.Span);
                //ReadOnlySpan<byte> right = MemoryMarshal.AsBytes<TWide>(other.WCHAR.Span);
                //return left.SequenceEqual(right);
            }
            return false;
        }

        //TODO: boxing
        public bool Equals(INCursesChar obj)
        {
            if (obj is IMultiByteChar other)
                return this.Equals(other);
            return false;
        }
    }
}
