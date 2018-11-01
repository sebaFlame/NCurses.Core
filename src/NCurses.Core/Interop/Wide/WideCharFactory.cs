using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.Wide
{
    public static class WideCharFactory
    {
        private static Type WideCharType;
        private static Type WideStringType;

        private static Func<INCursesChar, INCursesWCHAR> ncursesCharCreate;

        private static Func<char, INCursesWCHAR> charCreate;
        private static Func<char, ulong, INCursesWCHAR> charAttrCreate;
        private static Func<char, ulong, short, INCursesWCHAR> charAttrColorCreate;

        private static Func<string, INCursesWCHARStr> strCreate;
        private static Func<string, ulong, INCursesWCHARStr> strAttrCreate;
        private static Func<string, ulong, short, INCursesWCHARStr> strAttrColorCreate;
        //private static Func<char[], INCursesWCHARStr> chArrCreate;
        //private static Func<char[], ulong, INCursesWCHARStr> chArrAttrCreate;

        private static Func<int, INCursesWCHARStr> emptyCreate;

        private static Func<byte[], Encoding, INCursesWCHARStr> byteCreate;
        private static Func<byte[], Encoding, ulong, INCursesWCHARStr> byteAttrCreate;
        private static Func<byte[], Encoding, ulong, short, INCursesWCHARStr> byteAttrColorCreate;

        static WideCharFactory()
        {
            WideCharType = typeof(NCursesWCHAR<>).MakeGenericType(DynamicTypeBuilder.cchar_t);
            WideStringType = typeof(NCursesWCHARStr<>).MakeGenericType(DynamicTypeBuilder.cchar_t);

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3, par4;

            par1 = Expression.Parameter(typeof(INCursesChar));
            ctor = WideCharType.GetConstructor(new Type[] { typeof(INCursesChar) });
            ncursesCharCreate = Expression.Lambda<Func<INCursesChar, INCursesWCHAR>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(INCursesWCHAR)), par1).Compile();

            par1 = Expression.Parameter(typeof(char));
            ctor = WideCharType.GetConstructor(new Type[] { typeof(char) });
            charCreate = Expression.Lambda<Func<char, INCursesWCHAR>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(INCursesWCHAR)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = WideCharType.GetConstructor(new Type[] { typeof(char), typeof(ulong) });
            charAttrCreate = Expression.Lambda<Func<char, ulong, INCursesWCHAR>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(INCursesWCHAR)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = WideCharType.GetConstructor(new Type[] { typeof(char), typeof(ulong), typeof(short) });
            charAttrColorCreate = Expression.Lambda<Func<char, ulong, short, INCursesWCHAR>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(INCursesWCHAR)), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(string));
            ctor = WideStringType.GetConstructor(new Type[] { typeof(string) });
            strCreate = Expression.Lambda<Func<string, INCursesWCHARStr>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(INCursesWCHARStr)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = WideStringType.GetConstructor(new Type[] { typeof(string), typeof(ulong) });
            strAttrCreate = Expression.Lambda<Func<string, ulong, INCursesWCHARStr>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(INCursesWCHARStr)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = WideStringType.GetConstructor(new Type[] { typeof(string), typeof(ulong), typeof(short) });
            strAttrColorCreate = Expression.Lambda<Func<string, ulong, short, INCursesWCHARStr>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(INCursesWCHARStr)), par1, par2,par3).Compile();

            //par1 = Expression.Parameter(typeof(char[]));
            //ctor = WideStringType.GetConstructor(new Type[] { typeof(char[]) });
            //chArrCreate = Expression.Lambda<Func<char[], INCursesWCHARStr>>(
            //    Expression.Convert(Expression.New(ctor, par1), typeof(INCursesWCHARStr)), par1).Compile();

            //par2 = Expression.Parameter(typeof(ulong));
            //ctor = WideStringType.GetConstructor(new Type[] { typeof(char[]), typeof(ulong) });
            //chArrAttrCreate = Expression.Lambda<Func<char[], ulong, INCursesWCHARStr>>(
            //    Expression.Convert(Expression.New(ctor, par1, par2), typeof(INCursesWCHARStr)), par1, par2).Compile();

            par1 = Expression.Parameter(typeof(int));
            ctor = WideStringType.GetConstructor(new Type[] { typeof(int) });
            emptyCreate = Expression.Lambda<Func<int, INCursesWCHARStr>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(INCursesWCHARStr)), par1).Compile();

            par1 = Expression.Parameter(typeof(byte[]));
            par2 = Expression.Parameter(typeof(Encoding));
            ctor = WideStringType.GetConstructor(new Type[] { typeof(byte[]), typeof(Encoding) });
            byteCreate = Expression.Lambda<Func<byte[], Encoding, INCursesWCHARStr>>(Expression.Convert(
                Expression.New(ctor, par1, par2), typeof(INCursesWCHARStr)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(ulong));
            ctor = WideStringType.GetConstructor(new Type[] { typeof(byte[]), typeof(Encoding), typeof(ulong) });
            byteAttrCreate = Expression.Lambda<Func<byte[], Encoding, ulong, INCursesWCHARStr>>(Expression.Convert(
                Expression.New(ctor, par1, par2, par3), typeof(INCursesWCHARStr)), par1, par2, par3).Compile();

            par4 = Expression.Parameter(typeof(short));
            ctor = WideStringType.GetConstructor(new Type[] { typeof(byte[]), typeof(Encoding), typeof(ulong), typeof(short) });
            byteAttrColorCreate = Expression.Lambda<Func<byte[], Encoding, ulong, short, INCursesWCHARStr>>(Expression.Convert(
                Expression.New(ctor, par1, par2, par3, par4), typeof(INCursesWCHARStr)), par1, par2, par3, par4).Compile();
        }

        public static INCursesWCHAR GetWideChar()
        {
            return charCreate('\0');
        }

        public static INCursesWCHAR GetWideChar(INCursesChar wch)
        {
            return ncursesCharCreate(wch);
        }

        public static INCursesWCHAR GetWideChar(char ch)
        {
            return charCreate(ch);
        }

        public static INCursesWCHAR GetWideChar(char ch, ulong attr)
        {
            return charAttrCreate(ch, attr);
        }

        public static INCursesWCHAR GetWideChar(char ch, ulong attr, short pair)
        {
            return charAttrColorCreate(ch, attr, pair);
        }

        public static INCursesWCHARStr GetWideString(string str)
        {
            return strCreate(str);
        }

        public static INCursesWCHARStr GetWideString(string str, ulong attr)
        {
            return strAttrCreate(str, attr);
        }

        public static INCursesWCHARStr GetWideString(string str, ulong attr, short pair)
        {
            return strAttrColorCreate(str, attr, pair);
        }

        public static INCursesWCHARStr GetWideString(byte[] str, Encoding encoding)
        {
            return byteCreate(str, encoding);
        }

        public static INCursesWCHARStr GetWideString(byte[] str, Encoding encoding, ulong attr)
        {
            return byteAttrCreate(str, encoding, attr);
        }

        public static INCursesWCHARStr GetWideString(byte[] str, Encoding encoding, ulong attr, short pair)
        {
            return byteAttrColorCreate(str, encoding, attr, pair);
        }

        //public static INCursesWCHARStr GetWideString(char[] str)
        //{
        //    return chArrCreate(str);
        //}

        //public static INCursesWCHARStr GetWideString(char[] str, ulong attr)
        //{
        //    return chArrAttrCreate(str, attr);
        //}

        public static INCursesWCHARStr GetWideString(int length)
        {
            return emptyCreate(length);
        }
    }
}
