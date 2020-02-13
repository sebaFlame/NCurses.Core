using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.SingleByte
{
    /// <summary>
    /// Create native chars strings
    /// These methods should not be used, because these incur heap allocations and alot of boxing
    /// </summary>
    public class SingleByteCharFactory : INCursesCharFactory<ISingleByteChar, ISingleByteCharString>
    {
        public static SingleByteCharFactory Instance { get; }

        internal static INCursesCharFactory<ISingleByteChar, ISingleByteCharString> Factory { get; }

        private static Type FactoryType;

        static SingleByteCharFactory()
        {
            Instance = new SingleByteCharFactory();

            FactoryType = typeof(SingleByteCharFactoryInternal<>).MakeGenericType(DynamicTypeBuilder.chtype);

            PropertyInfo property = FactoryType.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo getMethod = property.GetGetMethod(true);

            Factory = (INCursesCharFactory<ISingleByteChar, ISingleByteCharString>)getMethod.Invoke(null, Array.Empty<object>());
        }

        public ISingleByteChar GetNativeEmptyChar() =>
            Factory.GetNativeEmptyChar();

        public ISingleByteChar GetNativeChar(char ch) =>
            Factory.GetNativeChar(ch);

        public ISingleByteChar GetNativeChar(char ch, ulong attrs) =>
            Factory.GetNativeChar(ch, attrs);

        public ISingleByteChar GetNativeChar(char ch, ulong attrs, short colorPair) =>
            Factory.GetNativeChar(ch, attrs, colorPair);

        public ISingleByteCharString GetNativeEmptyString(int length) =>
            Factory.GetNativeEmptyString(length);

        public ISingleByteCharString GetNativeString(string str) =>
            Factory.GetNativeString(str);

        public ISingleByteCharString GetNativeString(string str, ulong attrs) =>
            Factory.GetNativeString(str, attrs);

        public ISingleByteCharString GetNativeString(string str, ulong attrs, short colorPair) =>
            Factory.GetNativeString(str, attrs, colorPair);

        public unsafe ISingleByteCharString GetNativeEmptyString(byte* buffer, int length)
            => Factory.GetNativeEmptyString(buffer, length);

        public ISingleByteCharString GetNativeEmptyString(byte[] buffer)
            => Factory.GetNativeEmptyString(buffer);

        public unsafe ISingleByteCharString GetNativeString(byte* buffer, int length, string str)
            => Factory.GetNativeString(buffer, length, str);

        public ISingleByteCharString GetNativeString(byte[] buffer, string str)
            => Factory.GetNativeString(buffer, str);

        public unsafe ISingleByteCharString GetNativeString(byte* buffer, int length, string str, ulong attrs)
            => Factory.GetNativeString(buffer, length, str, attrs);

        public ISingleByteCharString GetNativeString(byte[] buffer, string str, ulong attrs)
            => Factory.GetNativeString(buffer, str, attrs);

        public unsafe ISingleByteCharString GetNativeString(byte* buffer, int length, string str, ulong attrs, short colorPair)
            => Factory.GetNativeString(buffer, length, str, attrs, colorPair);

        public ISingleByteCharString GetNativeString(byte[] buffer, string str, ulong attrs, short colorPair)
            => Factory.GetNativeString(buffer, str, attrs, colorPair);

        public int GetByteCount(string str) => Factory.GetByteCount(str);

        public int GetByteCount(int length) => Factory.GetByteCount(length);

        public int GetCharLength() => Factory.GetCharLength();
    }

    internal class SingleByteCharFactoryInternal<TSingleByte> :
        INCursesCharFactoryInternal<ISingleByteChar, ISingleByteCharString, TSingleByte, SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
    {
        internal static Func<sbyte, TSingleByte> CreateCharFromByte;
        internal static Func<sbyte, ulong, TSingleByte> CreateCharWithAttributeFromByte;
        internal static Func<sbyte, ulong, short, TSingleByte> CreateCharWithAttributeAndColorPairFromByte;
        internal static Func<ulong, TSingleByte> CreateCharFromAttribute;

        internal static SingleByteCharFactoryInternal<TSingleByte> Instance { get; }

        static SingleByteCharFactoryInternal()
        {
            Instance = new SingleByteCharFactoryInternal<TSingleByte>();

            ConstructorInfo ctor;
            ParameterExpression instance, par1, par2, par3;

            instance = Expression.Parameter(typeof(TSingleByte));
            par1 = Expression.Parameter(typeof(sbyte));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(sbyte) });
            CreateCharFromByte =
                Expression.Lambda<Func<sbyte, TSingleByte>>(Expression.New(ctor, par1), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(sbyte), typeof(ulong) });
            CreateCharWithAttributeFromByte =
                Expression.Lambda<Func<sbyte, ulong, TSingleByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(sbyte), typeof(ulong), typeof(short) });
            CreateCharWithAttributeAndColorPairFromByte =
                Expression.Lambda<Func<sbyte, ulong, short, TSingleByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(ulong) });
            CreateCharFromAttribute =
                Expression.Lambda<Func<ulong, TSingleByte>>(Expression.New(ctor, par1), par1).Compile();
        }

        //only used in MouseEventFactory
        internal TSingleByte GetAttributeInternal(ulong attrs) =>
            CreateCharFromAttribute(attrs);

        #region ICharFactoryInternal<TChar, TString>
        public TSingleByte GetNativeEmptyCharInternal() =>
            CreateCharFromByte(0);

        public TSingleByte GetNativeCharInternal(char ch) =>
            CreateCharFromByte((sbyte)ch);

        public unsafe SingleByteCharString<TSingleByte> GetNativeEmptyStringInternal(byte* buffer, int length) =>
            new SingleByteCharString<TSingleByte>(buffer, length);

        public SingleByteCharString<TSingleByte> GetNativeEmptyStringInternal(byte[] buffer) =>
            new SingleByteCharString<TSingleByte>(buffer);

        public unsafe SingleByteCharString<TSingleByte> GetNativeStringInternal(byte* buffer, int length, string str) =>
            new SingleByteCharString<TSingleByte>(buffer, length, str);

        public SingleByteCharString<TSingleByte> GetNativeStringInternal(byte[] buffer, string str) =>
            new SingleByteCharString<TSingleByte>(buffer, str);

        public int GetByteCount(string str, bool addNullTerminator = true) =>
            (str.Length * Marshal.SizeOf<TSingleByte>()) + (addNullTerminator ? Marshal.SizeOf<TSingleByte>() : 0);

        public int GetByteCount(int length, bool addNullTerminator = true) =>
            (length * Marshal.SizeOf<TSingleByte>()) + (addNullTerminator ? Marshal.SizeOf<TSingleByte>() : 0);

        public int GetCharLength() => Marshal.SizeOf<TSingleByte>();

        public SingleByteCharString<TSingleByte> CreateNativeString(ref TSingleByte strRef)
            => new SingleByteCharString<TSingleByte>(ref strRef);
        #endregion

        #region ICharFactoryInternal<TCharType, TStringType, TChar, TString>
        public TSingleByte GetNativeCharInternal(char ch, ulong attrs) =>
            CreateCharWithAttributeFromByte((sbyte)ch, attrs);

        public TSingleByte GetNativeCharInternal(char ch, ulong attrs, short colorPair) =>
            CreateCharWithAttributeAndColorPairFromByte((sbyte)ch, attrs, colorPair);

        public unsafe SingleByteCharString<TSingleByte> GetNativeStringInternal(byte* buffer, int length, string str, ulong attrs) =>
            new SingleByteCharString<TSingleByte>(buffer, length, str, attrs);

        public SingleByteCharString<TSingleByte> GetNativeStringInternal(byte[] buffer, string str, ulong attrs) =>
            new SingleByteCharString<TSingleByte>(buffer, str, attrs);

        public unsafe SingleByteCharString<TSingleByte> GetNativeStringInternal(byte* buffer, int length, string str, ulong attrs, short colorPair) =>
            new SingleByteCharString<TSingleByte>(buffer, length, str, attrs, colorPair);

        public unsafe SingleByteCharString<TSingleByte> GetNativeStringInternal(byte[] buffer, string str, ulong attrs, short colorPair) =>
            new SingleByteCharString<TSingleByte>(buffer, str, attrs, colorPair);
        #endregion

        #region ICharFactory
        public ISingleByteChar GetNativeEmptyChar() =>
            GetNativeEmptyCharInternal();

        public ISingleByteChar GetNativeChar(char ch) =>
            GetNativeCharInternal(ch);

        public ISingleByteChar GetNativeChar(char ch, ulong attrs) =>
            GetNativeCharInternal(ch, attrs);

        public ISingleByteChar GetNativeChar(char ch, ulong attrs, short colorPair) =>
            GetNativeChar(ch, attrs, colorPair);

        public unsafe ISingleByteCharString GetNativeEmptyString(byte* buffer, int length)
            => this.GetNativeEmptyStringInternal(buffer, length);

        public ISingleByteCharString GetNativeEmptyString(byte[] buffer)
            => this.GetNativeEmptyStringInternal(buffer);

        public ISingleByteCharString GetNativeEmptyString(int length)
            => this.GetNativeEmptyString(new byte[this.GetByteCount(length)]);

        public unsafe ISingleByteCharString GetNativeString(byte* buffer, int length, string str)
            => this.GetNativeStringInternal(buffer, length, str);

        public ISingleByteCharString GetNativeString(byte[] buffer, string str)
            => this.GetNativeStringInternal(buffer, str);

        public ISingleByteCharString GetNativeString(string str)
            => this.GetNativeString(new byte[this.GetByteCount(str)], str);

        public unsafe ISingleByteCharString GetNativeString(byte* buffer, int length, string str, ulong attrs)
            => this.GetNativeStringInternal(buffer, length, str, attrs);

        public ISingleByteCharString GetNativeString(byte[] buffer, string str, ulong attrs)
            => this.GetNativeStringInternal(buffer,  str, attrs);

        public ISingleByteCharString GetNativeString(string str, ulong attrs)
            => this.GetNativeString(new byte[this.GetByteCount(str)], str, attrs);

        public unsafe ISingleByteCharString GetNativeString(byte* buffer, int length, string str, ulong attrs, short colorPair)
            => this.GetNativeStringInternal(buffer, length, str, attrs, colorPair);

        public ISingleByteCharString GetNativeString(byte[] buffer, string str, ulong attrs, short colorPair)
            => this.GetNativeStringInternal(buffer, str, attrs, colorPair);

        public ISingleByteCharString GetNativeString(string str, ulong attrs, short colorPair)
            => this.GetNativeString(new byte[this.GetByteCount(str)], str, attrs, colorPair);

        public int GetByteCount(string str) => this.GetByteCount(str, true);

        public int GetByteCount(int length) => this.GetByteCount(length, true);
        #endregion
    }
}
