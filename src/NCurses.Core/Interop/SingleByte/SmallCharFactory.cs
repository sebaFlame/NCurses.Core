using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.SingleByte
{
    internal class SmallCharFactory : ICharFactory<INCursesSCHAR, INCursesSCHARStr>
    {
        private static Type SmallCharType;
        private static Type SmallStringType;

        private static Func<INCursesChar, INCursesSCHAR> ncursesCharCreate;

        private static Func<sbyte, INCursesSCHAR> byteCreate;
        private static Func<sbyte, ulong, INCursesSCHAR> byteAttrCreate;
        private static Func<sbyte, ulong, short, INCursesSCHAR> byteAttrColorCreate;
        private static Func<char, INCursesSCHAR> charCreate;
        private static Func<char, ulong, INCursesSCHAR> charAttrCreate;
        private static Func<char, ulong, short, INCursesSCHAR> charAttrColorCreate;
        private static Func<ulong, INCursesSCHAR> attrCreate;

        private static Func<string, INCursesSCHARStr> strCreate;
        private static Func<string, ulong, INCursesSCHARStr> strAttrCreate;
        private static Func<string, ulong, short, INCursesSCHARStr> strAttrColorCreate;

        private static Func<int, INCursesSCHARStr> emptyCreate;

        private static SmallCharFactory instance;
        internal static SmallCharFactory Instance => instance ?? (instance = new SmallCharFactory());
        ICharFactory<INCursesSCHAR, INCursesSCHARStr> ICharFactory<INCursesSCHAR, INCursesSCHARStr>.Instance => Instance;

        static SmallCharFactory()
        {
            instance = new SmallCharFactory();

            SmallCharType = typeof(NCursesSCHAR<>).MakeGenericType(DynamicTypeBuilder.chtype);
            SmallStringType = typeof(NCursesSCHARStr<>).MakeGenericType(DynamicTypeBuilder.chtype);

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(INCursesChar));
            ctor = SmallCharType.GetConstructor(new Type[] { typeof(INCursesChar) });
            ncursesCharCreate = Expression.Lambda<Func<INCursesChar, INCursesSCHAR>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(INCursesSCHAR)), par1).Compile();

            par1 = Expression.Parameter(typeof(sbyte));
            ctor = SmallCharType.GetConstructor(new Type[] { typeof(sbyte) });
            byteCreate = Expression.Lambda<Func<sbyte, INCursesSCHAR>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(INCursesSCHAR)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = SmallCharType.GetConstructor(new Type[] { typeof(sbyte), typeof(ulong) });
            byteAttrCreate = Expression.Lambda<Func<sbyte, ulong, INCursesSCHAR>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(INCursesSCHAR)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = SmallCharType.GetConstructor(new Type[] { typeof(sbyte), typeof(ulong), typeof(short) });
            byteAttrColorCreate = Expression.Lambda<Func<sbyte, ulong, short, INCursesSCHAR>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(INCursesSCHAR)), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(char));
            ctor = SmallCharType.GetConstructor(new Type[] { typeof(char) });
            charCreate = Expression.Lambda<Func<char, INCursesSCHAR>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(INCursesSCHAR)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = SmallCharType.GetConstructor(new Type[] { typeof(char), typeof(ulong) });
            charAttrCreate = Expression.Lambda<Func<char, ulong, INCursesSCHAR>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(INCursesSCHAR)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = SmallCharType.GetConstructor(new Type[] { typeof(char), typeof(ulong), typeof(short) });
            charAttrColorCreate = Expression.Lambda<Func<char, ulong, short, INCursesSCHAR>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(INCursesSCHAR)), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(ulong));
            ctor = SmallCharType.GetConstructor(new Type[] { typeof(ulong) });
            attrCreate = Expression.Lambda<Func<ulong, INCursesSCHAR>>(Expression.Convert(
                    Expression.New(ctor, par1), typeof(INCursesSCHAR)), par1).Compile();

            par1 = Expression.Parameter(typeof(string));
            ctor = SmallStringType.GetConstructor(new Type[] { typeof(string) });
            strCreate = Expression.Lambda<Func<string, INCursesSCHARStr>>(
                Expression.Convert(Expression.New(ctor, par1), typeof(INCursesSCHARStr)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = SmallStringType.GetConstructor(new Type[] { typeof(string), typeof(ulong) });
            strAttrCreate = Expression.Lambda<Func<string, ulong, INCursesSCHARStr>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(INCursesSCHARStr)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = SmallStringType.GetConstructor(new Type[] { typeof(string), typeof(ulong), typeof(short) });
            strAttrColorCreate = Expression.Lambda<Func<string, ulong, short, INCursesSCHARStr>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(INCursesSCHARStr)), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(int));
            ctor = SmallStringType.GetConstructor(new Type[] { typeof(int) });
            emptyCreate = Expression.Lambda<Func<int, INCursesSCHARStr>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(INCursesSCHARStr)), par1).Compile();
        }

        public void GetAttribute(ulong attrs, out INCursesSCHAR res)
        {
            res = attrCreate(attrs);
        }

        public void GetNativeEmptyChar(out INCursesSCHAR res)
        {
            res = charCreate('\0');
        }

        internal void GetNativeChar(INCursesChar ch, out INCursesSCHAR res)
        {
            res = ncursesCharCreate(ch);
        }

        public void GetNativeChar(char ch, out INCursesSCHAR res)
        {
            res = charCreate(ch);
        }

        public void GetNativeChar(char ch, ulong attr, out INCursesSCHAR res)
        {
            res = charAttrCreate(ch, attr);
        }

        public void GetNativeChar(char ch, ulong attr, short pair, out INCursesSCHAR res)
        {
            res = charAttrColorCreate(ch, attr, pair);
        }

        public void GetNativeEmptyString(int length, out INCursesSCHARStr res)
        {
            res = emptyCreate(length);
        }

        public void GetNativeString(string str, out INCursesSCHARStr res)
        {
            res = strCreate(str);
        }

        public void GetNativeString(string str, ulong attr, out INCursesSCHARStr res)
        {
            res = strAttrCreate(str, attr);
        }

        public void GetNativeString(string str, ulong attr, short color, out INCursesSCHARStr res)
        {
            res = strAttrColorCreate(str, attr, color);
        }

        public void GetNativeChar(sbyte ch, out INCursesSCHAR res)
        {
            res = byteCreate(ch);
        }

        public void GetNativeChar(sbyte ch, ulong attr, out INCursesSCHAR res)
        {
            res = byteAttrCreate(ch, attr);
        }

        public void GetNativeChar(sbyte ch, ulong attr, short pair, out INCursesSCHAR res)
        {
            res = byteAttrColorCreate(ch, attr, pair);
        }
    }
}
