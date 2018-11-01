using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.SingleByte
{
    public static class SmallCharFactory
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
        //private static Func<byte[], INCursesSCHARStr> byteArrCreate;
        //private static Func<byte[], ulong, INCursesSCHARStr> byteArrAttrCreate;
        //private static Func<char[], INCursesSCHARStr> chArrCreate;
        //private static Func<char[], ulong, INCursesSCHARStr> chArrAttrCreate;
        private static Func<int, INCursesSCHARStr> emptyCreate;

        static SmallCharFactory()
        {
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

            //par1 = Expression.Parameter(typeof(byte[]));
            //ctor = SmallStringType.GetConstructor(new Type[] { typeof(byte[]) });
            //byteArrCreate = Expression.Lambda<Func<byte[], INCursesSCHARStr>>(Expression.Convert(
            //    Expression.New(ctor, par1), typeof(INCursesSCHARStr)), par1).Compile();

            //par2 = Expression.Parameter(typeof(ulong));
            //ctor = SmallStringType.GetConstructor(new Type[] { typeof(byte[]), typeof(ulong) });
            //byteArrAttrCreate = Expression.Lambda<Func<byte[], ulong, INCursesSCHARStr>>(
            //    Expression.Convert(Expression.New(ctor, par1, par2), typeof(INCursesSCHARStr)), par1, par2).Compile();

            //par1 = Expression.Parameter(typeof(char[]));
            //ctor = SmallStringType.GetConstructor(new Type[] { typeof(char[]) });
            //chArrCreate = Expression.Lambda<Func<char[], INCursesSCHARStr>>(
            //    Expression.Convert(Expression.New(ctor, par1), typeof(INCursesSCHARStr)), par1).Compile();

            //par2 = Expression.Parameter(typeof(ulong));
            //ctor = SmallStringType.GetConstructor(new Type[] { typeof(char[]), typeof(ulong) });
            //chArrAttrCreate = Expression.Lambda<Func<char[], ulong, INCursesSCHARStr>>(
            //    Expression.Convert(Expression.New(ctor, par1, par2), typeof(INCursesSCHARStr)), par1, par2).Compile();

            par1 = Expression.Parameter(typeof(int));
            ctor = SmallStringType.GetConstructor(new Type[] { typeof(int) });
            emptyCreate = Expression.Lambda<Func<int, INCursesSCHARStr>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(INCursesSCHARStr)), par1).Compile();
        }

        public static INCursesSCHAR GetAttribute(ulong attrs)
        {
            return attrCreate(attrs);
        }

        public static INCursesSCHAR GetSmallChar()
        {
            return charCreate('\0');
        }

        public static INCursesSCHAR GetSmallChar(INCursesChar ch)
        {
            return ncursesCharCreate(ch);
        }

        public static INCursesSCHAR GetSmallChar(char ch)
        {
            return charCreate(ch);
        }

        public static INCursesSCHAR GetSmallChar(char ch, ulong attr)
        {
            return charAttrCreate(ch, attr);
        }

        public static INCursesSCHAR GetSmallChar(char ch, ulong attr, short pair)
        {
            return charAttrColorCreate(ch, attr, pair);
        }

        public static INCursesSCHAR GetSmallChar(sbyte ch)
        {
            return byteCreate(ch);
        }

        public static INCursesSCHAR GetSmallChar(sbyte ch, ulong attr)
        {
            return byteAttrCreate(ch, attr);
        }

        public static INCursesSCHAR GetSmallChar(sbyte ch, ulong attr, short pair)
        {
            return byteAttrColorCreate(ch, attr, pair);
        }

        //public static INCursesSCHARStr GetSmallString(byte[] str)
        //{
        //    return byteArrCreate(str);
        //}

        //public static INCursesSCHARStr GetSmallString(byte[] str, ulong attr)
        //{
        //    return byteArrAttrCreate(str, attr);
        //}

        public static INCursesSCHARStr GetSmallString(string str)
        {
            return strCreate(str);
        }

        public static INCursesSCHARStr GetSmallString(string str, ulong attr)
        {
            return strAttrCreate(str, attr);
        }

        public static INCursesSCHARStr GetSmallString(string str, ulong attr, short color)
        {
            return strAttrColorCreate(str, attr, color);
        }

        //public static INCursesSCHARStr GetSmallString(char[] str)
        //{
        //    return chArrCreate(str);
        //}

        //public static INCursesSCHARStr GetSmallString(char[] str, ulong attr)
        //{
        //    return chArrAttrCreate(str, attr);
        //}

        public static INCursesSCHARStr GetSmallString(int length)
        {
            return emptyCreate(length);
        }
    }
}
