using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.Char
{
    public class CharFactory : ICharFactory<IChar, ICharString>
    {
        public static CharFactory Instance { get; }

        internal static ICharFactory<IChar, ICharString> Factory { get; }

        private static Type FactoryType;

        static CharFactory()
        {
            Instance = new CharFactory();

            FactoryType = typeof(CharFactoryInternal<>).MakeGenericType(typeof(schar_t));

            PropertyInfo property = FactoryType.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo getMethod = property.GetGetMethod(true);

            Factory = (ICharFactory<IChar, ICharString>)getMethod.Invoke(null, Array.Empty<object>());
        }

        public IChar GetNativeChar(char ch)
            => Factory.GetNativeChar(ch);

        public IChar GetNativeEmptyChar()
            => Factory.GetNativeEmptyChar();

        public ICharString GetNativeEmptyString(int length)
            => Factory.GetNativeEmptyString(length);

        public unsafe ICharString GetNativeEmptyString(byte* buffer, int length)
            => Factory.GetNativeEmptyString(buffer, length);

        public ICharString GetNativeEmptyString(byte[] buffer)
            => Factory.GetNativeEmptyString(buffer);

        public ICharString GetNativeString(string str)
            => Factory.GetNativeString(str);

        public unsafe ICharString GetNativeString(byte* buffer, int length, string str)
            => Factory.GetNativeString(buffer, length, str);

        public ICharString GetNativeString(byte[] buffer, string str)
            => Factory.GetNativeString(buffer, str);

        public int GetByteCount(string str)
            => Factory.GetByteCount(str);

        public int GetByteCount(int length)
            => Factory.GetByteCount(length);

        public int GetCharLength()
            => Factory.GetCharLength();
    }

    internal class CharFactoryInternal<TChar> : ICharFactoryInternal<TChar, CharString<TChar>>
        where TChar : unmanaged, IChar, IEquatable<TChar>
    {
        internal static Func<char, TChar> CreateCharFromChar;
        internal static Func<byte, TChar> CreateCharFromByte;

        internal static CharFactoryInternal<TChar> Instance { get; }

        static CharFactoryInternal()
        {
            Instance = new CharFactoryInternal<TChar>();

            ConstructorInfo ctor;
            ParameterExpression par1;

            par1 = Expression.Parameter(typeof(char));
            ctor = typeof(TChar).GetConstructor(new Type[] { typeof(char) });
            CreateCharFromChar =
                Expression.Lambda<Func<char, TChar>>(Expression.New(ctor, par1), par1).Compile();

            par1 = Expression.Parameter(typeof(byte));
            ctor = typeof(TChar).GetConstructor(new Type[] { typeof(byte) });
            CreateCharFromByte =
                Expression.Lambda<Func<byte, TChar>>(Expression.New(ctor, par1), par1).Compile();
        }

        public TChar GetNativeEmptyCharInternal()
            => CreateCharFromChar('\0');

        public TChar GetNativeCharInternal(char ch)
            => CreateCharFromChar(ch);

        public unsafe CharString<TChar> GetNativeEmptyStringInternal(byte* buffer, int length)
            => new CharString<TChar>(buffer, length);

        public CharString<TChar> GetNativeEmptyStringInternal(byte[] buffer)
            => new CharString<TChar>(buffer);

        public unsafe CharString<TChar> GetNativeStringInternal(byte* buffer, int length, string str)
            => new CharString<TChar>(buffer, length, str);

        public CharString<TChar> GetNativeStringInternal(byte[] buffer, string str)
            => new CharString<TChar>(buffer, str);

        public CharString<TChar> CreateNativeString(ref TChar strRef)
            => new CharString<TChar>(ref strRef);

        public int GetByteCount(string str, bool addNullTerminator = true) =>
            (str.Length * Marshal.SizeOf<TChar>()) + (addNullTerminator ? Marshal.SizeOf<TChar>() : 0);

        public int GetByteCount(int length, bool addNullTerminator = true) =>
            (length * Marshal.SizeOf<TChar>()) + (addNullTerminator ? Marshal.SizeOf<TChar>() : 0);

        public int GetCharLength() => Marshal.SizeOf<TChar>();
    }
}
