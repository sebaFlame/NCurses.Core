using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.WideChar
{
    internal delegate TWideChar CreateWideCharStringFromSpan<TWideChar>(Span<byte> byteSpan)
        where TWideChar : unmanaged, IEquatable<TWideChar>;

    public class WideCharFactory : ICharFactory<IChar, ICharString>
    {
        public static WideCharFactory Instance { get; }

        internal static ICharFactory<IChar, ICharString> Factory { get; }

        private static Type FactoryType;

        static WideCharFactory()
        {
            Instance = new WideCharFactory();

            FactoryType = typeof(WideCharFactoryInternal<>).MakeGenericType(DynamicTypeBuilder.wchar_t);

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

    internal class WideCharFactoryInternal<TWideChar> : ICharFactoryInternal<TWideChar, WideCharString<TWideChar>>
        where TWideChar : unmanaged, IChar, IEquatable<TWideChar>
    {
        internal static Func<char, TWideChar> CreateCharFromChar;
        internal static Func<int, TWideChar> CreateCharFromInt;
        internal static Func<ArraySegment<byte>, TWideChar> CreateCharFromArraySegment;
        internal static CreateWideCharStringFromSpan<TWideChar> CreateCharFromSpan;

        internal static WideCharFactoryInternal<TWideChar> Instance { get; }

        static WideCharFactoryInternal()
        {
            Instance = new WideCharFactoryInternal<TWideChar>();

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(ArraySegment<byte>));
            par2 = Expression.Parameter(typeof(ulong));
            par3 = Expression.Parameter(typeof(short));

            ctor = typeof(TWideChar).GetConstructor(new Type[] { typeof(ArraySegment<byte>) });
            CreateCharFromArraySegment =
                Expression.Lambda<Func<ArraySegment<byte>, TWideChar>>(Expression.New(ctor, par1), par1).Compile();

            par1 = Expression.Parameter(typeof(Span<byte>));

            ctor = typeof(TWideChar).GetConstructor(new Type[] { typeof(Span<byte>) });
            CreateCharFromSpan =
                Expression.Lambda<CreateWideCharStringFromSpan<TWideChar>>(Expression.New(ctor, par1), par1).Compile();

            par1 = Expression.Parameter(typeof(char));
            ctor = typeof(TWideChar).GetConstructor(new Type[] { typeof(char) });
            CreateCharFromChar =
                Expression.Lambda<Func<char, TWideChar>>(Expression.New(ctor, par1), par1).Compile();

            par1 = Expression.Parameter(typeof(int));
            ctor = typeof(TWideChar).GetConstructor(new Type[] { typeof(int) });
            CreateCharFromInt =
                Expression.Lambda<Func<int, TWideChar>>(Expression.New(ctor, par1), par1).Compile();
        }

        public TWideChar GetNativeEmptyCharInternal()
            => CreateCharFromChar('\0');

        public TWideChar GetNativeCharInternal(char ch)
            => CreateCharFromChar(ch);

        public TWideChar GetNativeCharInternal(int ch)
            => CreateCharFromInt(ch);

        public unsafe WideCharString<TWideChar> GetNativeEmptyStringInternal(byte* buffer, int length)
            => new WideCharString<TWideChar>(buffer, length);

        public WideCharString<TWideChar> GetNativeEmptyStringInternal(byte[] buffer)
            => new WideCharString<TWideChar>(buffer);

        public unsafe WideCharString<TWideChar> GetNativeStringInternal(byte* buffer, int length, string str)
            => new WideCharString<TWideChar>(buffer, length, str);

        public WideCharString<TWideChar> GetNativeStringInternal(byte[] buffer, string str)
            => new WideCharString<TWideChar>(buffer, str);

        public WideCharString<TWideChar> CreateNativeString(ref TWideChar strRef)
            => new WideCharString<TWideChar>(ref strRef);

        public int GetByteCount(string str, bool addNullTerminator = true) =>
            (str.Length * Marshal.SizeOf<TWideChar>()) + (addNullTerminator ? Marshal.SizeOf<TWideChar>() : 0);

        public int GetByteCount(int length, bool addNullTerminator = true) =>
            (length * Marshal.SizeOf<TWideChar>()) + (addNullTerminator ? Marshal.SizeOf<TWideChar>() : 0);

        public int GetCharLength() => Marshal.SizeOf<TWideChar>();
    }
}
