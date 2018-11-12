using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.SingleByte
{
    internal class SingleByteCharFactory : ICharFactory<ISingleByteChar, ISingleByteCharString>
    {
        private static Type SingleByteCharType;
        private static Type SingleByteCharStringType;

        private static Func<INCursesChar, ISingleByteChar> ncursesCharCreate;

        private static Func<sbyte, ISingleByteChar> byteCreate;
        private static Func<sbyte, ulong, ISingleByteChar> byteAttrCreate;
        private static Func<sbyte, ulong, short, ISingleByteChar> byteAttrColorCreate;
        private static Func<char, ISingleByteChar> charCreate;
        private static Func<char, ulong, ISingleByteChar> charAttrCreate;
        private static Func<char, ulong, short, ISingleByteChar> charAttrColorCreate;
        private static Func<ulong, ISingleByteChar> attrCreate;

        private static Func<string, ISingleByteCharString> strCreate;
        private static Func<string, ulong, ISingleByteCharString> strAttrCreate;
        private static Func<string, ulong, short, ISingleByteCharString> strAttrColorCreate;

        private static Func<int, ISingleByteCharString> emptyCreate;

        private static SingleByteCharFactory instance;
        internal static SingleByteCharFactory Instance => instance ?? (instance = new SingleByteCharFactory());
        ICharFactory<ISingleByteChar, ISingleByteCharString> ICharFactory<ISingleByteChar, ISingleByteCharString>.Instance => Instance;

        static SingleByteCharFactory()
        {
            instance = new SingleByteCharFactory();

            SingleByteCharType = typeof(SingleByteChar<>).MakeGenericType(DynamicTypeBuilder.chtype);
            SingleByteCharStringType = typeof(SingleByteCharString<>).MakeGenericType(DynamicTypeBuilder.chtype);

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(INCursesChar));
            ctor = SingleByteCharType.GetConstructor(new Type[] { typeof(INCursesChar) });
            ncursesCharCreate = Expression.Lambda<Func<INCursesChar, ISingleByteChar>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(ISingleByteChar)), par1).Compile();

            par1 = Expression.Parameter(typeof(sbyte));
            ctor = SingleByteCharType.GetConstructor(new Type[] { typeof(sbyte) });
            byteCreate = Expression.Lambda<Func<sbyte, ISingleByteChar>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(ISingleByteChar)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = SingleByteCharType.GetConstructor(new Type[] { typeof(sbyte), typeof(ulong) });
            byteAttrCreate = Expression.Lambda<Func<sbyte, ulong, ISingleByteChar>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(ISingleByteChar)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = SingleByteCharType.GetConstructor(new Type[] { typeof(sbyte), typeof(ulong), typeof(short) });
            byteAttrColorCreate = Expression.Lambda<Func<sbyte, ulong, short, ISingleByteChar>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(ISingleByteChar)), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(char));
            ctor = SingleByteCharType.GetConstructor(new Type[] { typeof(char) });
            charCreate = Expression.Lambda<Func<char, ISingleByteChar>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(ISingleByteChar)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = SingleByteCharType.GetConstructor(new Type[] { typeof(char), typeof(ulong) });
            charAttrCreate = Expression.Lambda<Func<char, ulong, ISingleByteChar>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(ISingleByteChar)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = SingleByteCharType.GetConstructor(new Type[] { typeof(char), typeof(ulong), typeof(short) });
            charAttrColorCreate = Expression.Lambda<Func<char, ulong, short, ISingleByteChar>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(ISingleByteChar)), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(ulong));
            ctor = SingleByteCharType.GetConstructor(new Type[] { typeof(ulong) });
            attrCreate = Expression.Lambda<Func<ulong, ISingleByteChar>>(Expression.Convert(
                    Expression.New(ctor, par1), typeof(ISingleByteChar)), par1).Compile();

            par1 = Expression.Parameter(typeof(string));
            ctor = SingleByteCharStringType.GetConstructor(new Type[] { typeof(string) });
            strCreate = Expression.Lambda<Func<string, ISingleByteCharString>>(
                Expression.Convert(Expression.New(ctor, par1), typeof(ISingleByteCharString)), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = SingleByteCharStringType.GetConstructor(new Type[] { typeof(string), typeof(ulong) });
            strAttrCreate = Expression.Lambda<Func<string, ulong, ISingleByteCharString>>(
                Expression.Convert(Expression.New(ctor, par1, par2), typeof(ISingleByteCharString)), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = SingleByteCharStringType.GetConstructor(new Type[] { typeof(string), typeof(ulong), typeof(short) });
            strAttrColorCreate = Expression.Lambda<Func<string, ulong, short, ISingleByteCharString>>(
                Expression.Convert(Expression.New(ctor, par1, par2, par3), typeof(ISingleByteCharString)), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(int));
            ctor = SingleByteCharStringType.GetConstructor(new Type[] { typeof(int) });
            emptyCreate = Expression.Lambda<Func<int, ISingleByteCharString>>(Expression.Convert(
                Expression.New(ctor, par1), typeof(ISingleByteCharString)), par1).Compile();
        }

        //only used in MouseEventFactory
        internal ISingleByteChar GetAttribute(ulong attrs)
        {
            return attrCreate(attrs);
        }

        public void GetNativeEmptyChar(out ISingleByteChar res)
        {
            res = charCreate('\0');
        }

        internal void GetNativeChar(INCursesChar ch, out ISingleByteChar res)
        {
            res = ncursesCharCreate(ch);
        }

        public void GetNativeChar(char ch, out ISingleByteChar res)
        {
            res = charCreate(ch);
        }

        public void GetNativeChar(char ch, ulong attr, out ISingleByteChar res)
        {
            res = charAttrCreate(ch, attr);
        }

        public void GetNativeChar(char ch, ulong attr, short pair, out ISingleByteChar res)
        {
            res = charAttrColorCreate(ch, attr, pair);
        }

        public void GetNativeEmptyString(int length, out ISingleByteCharString res)
        {
            res = emptyCreate(length);
        }

        public void GetNativeString(string str, out ISingleByteCharString res)
        {
            res = strCreate(str);
        }

        public void GetNativeString(string str, ulong attr, out ISingleByteCharString res)
        {
            res = strAttrCreate(str, attr);
        }

        public void GetNativeString(string str, ulong attr, short color, out ISingleByteCharString res)
        {
            res = strAttrColorCreate(str, attr, color);
        }

        public void GetNativeChar(sbyte ch, out ISingleByteChar res)
        {
            res = byteCreate(ch);
        }

        public void GetNativeChar(sbyte ch, ulong attr, out ISingleByteChar res)
        {
            res = byteAttrCreate(ch, attr);
        }

        public void GetNativeChar(sbyte ch, ulong attr, short pair, out ISingleByteChar res)
        {
            res = byteAttrColorCreate(ch, attr, pair);
        }
    }
}
