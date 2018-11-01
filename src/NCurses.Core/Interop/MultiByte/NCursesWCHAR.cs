using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Buffers;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.MultiByte
{
    public class NCursesWCHAR<TWide> : INCursesWCHAR, IDisposable
        where TWide : unmanaged, INCursesWCHAR
    {
        internal Memory<TWide> WCHAR;
        internal ref TWide wchar => ref this.buffer[0];

        internal static Func<char, TWide> charCreate;
        internal static Func<char, ulong, TWide> charAttrCreate;
        internal static Func<char, ulong, short, TWide> charAttrColorCreate;

        private TWide[] buffer;

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

        static NCursesWCHAR()
        {
            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(char));
            ctor = typeof(TWide).GetConstructor(new Type[] { typeof(char) });
            charCreate = Expression.Lambda<Func<char, TWide>>(Expression.New(ctor, par1), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TWide).GetConstructor(new Type[] { typeof(char), typeof(ulong) });
            charAttrCreate = Expression.Lambda<Func<char, ulong, TWide>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = typeof(TWide).GetConstructor(new Type[] { typeof(char), typeof(ulong), typeof(short) });
            charAttrColorCreate = Expression.Lambda<Func<char, ulong, short, TWide>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();
        }

        public char Char => this.wchar.Char;
        public ulong Attributes => this.wchar.Attributes;
        public short Color => this.wchar.Color;

        private NCursesWCHAR()
        {
            this.buffer = ArrayPool<TWide>.Shared.Rent(1);
            this.WCHAR = new Memory<TWide>(buffer);
        }

        internal NCursesWCHAR(ref TWide wCh)
            : this()
        {
            this.buffer[0] = wCh;
        }

        public NCursesWCHAR(INCursesChar wCh)
            : this()
        {
            if (wCh is TWide wChParsed)
                this.buffer[0] = wChParsed;
            else
                throw new InvalidOperationException("Unsupported character type discovered");
        }

        public NCursesWCHAR(char ch)
            : this()
        {
            this.buffer[0] = charCreate(ch);
        }

        public NCursesWCHAR(char ch, ulong attr)
            : this()
        {
            this.buffer[0] = charAttrCreate(ch, attr);
        }

        public NCursesWCHAR(char ch, ulong attr, short pair)
            : this()
        {
            this.buffer[0] = charAttrColorCreate(ch, attr, pair);
        }

        ~NCursesWCHAR()
        {
            this.Dispose();
        }

        internal ref readonly TWide GetCharReference()
        {
            return ref this.buffer[0];
        }

        public void Dispose()
        {
            ArrayPool<TWide>.Shared.Return(this.buffer);
        }

        public static explicit operator TWide(NCursesWCHAR<TWide> ch)
        {
            return ch.wchar;
        }

        public static explicit operator NCursesWCHAR<TWide>(char ch)
        {
            return new NCursesWCHAR<TWide>(ch);
        }

        public static explicit operator char(NCursesWCHAR<TWide> ch)
        {
            return ch.Char;
        }

        public static bool operator ==(in NCursesWCHAR<TWide> wchLeft, in NCursesWCHAR<TWide> wchRight)
        {
            //return wchLeft.wchar.Equals(wchRight.wchar);
            return wchLeft.Equals(wchRight);
        }

        public static bool operator !=(in NCursesWCHAR<TWide> wchLeft, in NCursesWCHAR<TWide> wchRight)
        {
            //return !wchLeft.wchar.Equals(wchRight.wchar);
            return !wchLeft.Equals(wchRight);
        }

        //TODO: boxing
        public override bool Equals(object obj)
        {
            if (obj is INCursesWCHAR other)
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

        /* TODO
         * use cchar_t (generated overrides)
         */
        public bool Equals(INCursesWCHAR obj)
        {
            if (obj is NCursesWCHAR<TWide> other)
            {
                //return this.wchar.Equals(obj.wchar);
                ReadOnlySpan<byte> left = MemoryMarshal.AsBytes<TWide>(this.WCHAR.Span);
                ReadOnlySpan<byte> right = MemoryMarshal.AsBytes<TWide>(other.WCHAR.Span);
                return left.SequenceEqual(right);
            }
            return false;
        }

        //TODO: boxing
        public bool Equals(INCursesChar obj)
        {
            if (obj is INCursesWCHAR other)
                return this.Equals(other);
            return false;
        }
    }
}
