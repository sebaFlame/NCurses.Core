using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.MultiByte
{
    internal class MultiByteCharFactory : ICharFactory<IMultiByteChar, IMultiByteCharString>
    {
        private static Type MultiByteCharType;
        private static Type MultiByteCharStringType;

        private static Func<INCursesChar, IMultiByteChar> ncursesCharCreate;

        private static Func<char, IMultiByteChar> charCreate;
        private static Func<char, ulong, IMultiByteChar> charAttrCreate;
        private static Func<char, ulong, short, IMultiByteChar> charAttrColorCreate;

        private static Func<string, IMultiByteCharString> strCreate;
        private static Func<string, ulong, IMultiByteCharString> strAttrCreate;
        private static Func<string, ulong, short, IMultiByteCharString> strAttrColorCreate;

        private static Func<int, IMultiByteCharString> emptyCreate;

        private static Func<byte[], Encoding, IMultiByteCharString> byteCreate;
        private static Func<byte[], Encoding, ulong, IMultiByteCharString> byteAttrCreate;
        private static Func<byte[], Encoding, ulong, short, IMultiByteCharString> byteAttrColorCreate;

        private static MultiByteCharFactory instance;
        internal static MultiByteCharFactory Instance => instance ?? (instance = new MultiByteCharFactory());
        ICharFactory<IMultiByteChar, IMultiByteCharString> ICharFactory<IMultiByteChar, IMultiByteCharString>.Instance => Instance;

        static MultiByteCharFactory()
        {
            instance = new MultiByteCharFactory();

            MultiByteCharType = typeof(MultiByteChar<>).MakeGenericType(DynamicTypeBuilder.cchar_t);
            MultiByteCharStringType = typeof(MultiByteCharString<>).MakeGenericType(DynamicTypeBuilder.cchar_t);

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3, par4;

            par1 = Expression.Parameter(typeof(INCursesChar));
            ctor = MultiByteCharType.GetConstructor(new Type[] { typeof(INCursesChar) });
            ncursesCharCreate = Expression.Lambda<Func<INCursesChar, IMultiByteChar>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(IMultiByteChar)), par1).Compile();

            par1 = Expression.Parameter(typeof(char));
            ctor = MultiByteCharType.GetConstructor(new Type[] { typeof(char) });
            charCreate = Expression.Lambda<Func<char, IMultiByteChar>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(IMultiByteChar)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = MultiByteCharType.GetConstructor(new Type[] { typeof(char), typeof(ulong) });
            charAttrCreate = Expression.Lambda<Func<char, ulong, IMultiByteChar>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(IMultiByteChar)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = MultiByteCharType.GetConstructor(new Type[] { typeof(char), typeof(ulong), typeof(short) });
            charAttrColorCreate = Expression.Lambda<Func<char, ulong, short, IMultiByteChar>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(IMultiByteChar)), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(string));
            ctor = MultiByteCharStringType.GetConstructor(new Type[] { typeof(string) });
            strCreate = Expression.Lambda<Func<string, IMultiByteCharString>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(IMultiByteCharString)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = MultiByteCharStringType.GetConstructor(new Type[] { typeof(string), typeof(ulong) });
            strAttrCreate = Expression.Lambda<Func<string, ulong, IMultiByteCharString>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(IMultiByteCharString)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = MultiByteCharStringType.GetConstructor(new Type[] { typeof(string), typeof(ulong), typeof(short) });
            strAttrColorCreate = Expression.Lambda<Func<string, ulong, short, IMultiByteCharString>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(IMultiByteCharString)), par1, par2,par3).Compile();

            par1 = Expression.Parameter(typeof(int));
            ctor = MultiByteCharStringType.GetConstructor(new Type[] { typeof(int) });
            emptyCreate = Expression.Lambda<Func<int, IMultiByteCharString>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(IMultiByteCharString)), par1).Compile();

            par1 = Expression.Parameter(typeof(byte[]));
            par2 = Expression.Parameter(typeof(Encoding));
            ctor = MultiByteCharStringType.GetConstructor(new Type[] { typeof(byte[]), typeof(Encoding) });
            byteCreate = Expression.Lambda<Func<byte[], Encoding, IMultiByteCharString>>(Expression.Convert(
                Expression.New(ctor, par1, par2), typeof(IMultiByteCharString)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(ulong));
            ctor = MultiByteCharStringType.GetConstructor(new Type[] { typeof(byte[]), typeof(Encoding), typeof(ulong) });
            byteAttrCreate = Expression.Lambda<Func<byte[], Encoding, ulong, IMultiByteCharString>>(Expression.Convert(
                Expression.New(ctor, par1, par2, par3), typeof(IMultiByteCharString)), par1, par2, par3).Compile();

            par4 = Expression.Parameter(typeof(short));
            ctor = MultiByteCharStringType.GetConstructor(new Type[] { typeof(byte[]), typeof(Encoding), typeof(ulong), typeof(short) });
            byteAttrColorCreate = Expression.Lambda<Func<byte[], Encoding, ulong, short, IMultiByteCharString>>(Expression.Convert(
                Expression.New(ctor, par1, par2, par3, par4), typeof(IMultiByteCharString)), par1, par2, par3, par4).Compile();
        }

        public void GetNativeEmptyChar(out IMultiByteChar res)
        {
            res = charCreate('\0');
        }

        internal void GetNativeChar(INCursesChar wch, out IMultiByteChar res)
        {
            res = ncursesCharCreate(wch);
        }

        public void GetNativeChar(char ch, out IMultiByteChar res)
        {
            res = charCreate(ch);
        }

        public void GetNativeChar(char ch, ulong attr, out IMultiByteChar res)
        {
            res = charAttrCreate(ch, attr);
        }

        public void GetNativeChar(char ch, ulong attr, short pair, out IMultiByteChar res)
        {
            res = charAttrColorCreate(ch, attr, pair);
        }

        public void GetNativeEmptyString(int length, out IMultiByteCharString res)
        {
            res = emptyCreate(length);
        }

        public void GetNativeString(string str, out IMultiByteCharString res)
        {
            res = strCreate(str);
        }

        public void GetNativeString(string str, ulong attr, out IMultiByteCharString res)
        {
            res = strAttrCreate(str, attr);
        }

        public void GetNativeString(string str, ulong attr, short pair, out IMultiByteCharString res)
        {
            res = strAttrColorCreate(str, attr, pair);
        }

        public void GetNativeString(byte[] str, Encoding encoding, out IMultiByteCharString res)
        {
            res = byteCreate(str, encoding);
        }

        public void GetNativeString(byte[] str, Encoding encoding, ulong attr, out IMultiByteCharString res)
        {
            res = byteAttrCreate(str, encoding, attr);
        }

        public void GetNativeString(byte[] str, Encoding encoding, ulong attr, short pair, out IMultiByteCharString res)
        {
            res = byteAttrColorCreate(str, encoding, attr, pair);
        }
    }
}
