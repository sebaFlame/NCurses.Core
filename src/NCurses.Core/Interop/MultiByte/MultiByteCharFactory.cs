using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.MultiByte
{
    internal delegate TMultiByte CreateMultiByteCharFromSpan<TMultiByte>(Span<byte> byteSpan)
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>;

    internal delegate TMultiByte CreateMultiByteCharWithAttributeFromSpan<TMultiByte>(Span<byte> byteSpan, ulong attrs)
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>;

    internal delegate TMultiByte CreateMultiByteCharWithAttributeAndColorPairFromSpan<TMultiByte>(Span<byte> byteSpan, ulong attrs, short color)
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>;

    /// <summary>
    /// Create native chars strings
    /// These methods should not be used, because these incur heap allocations and alot of boxing
    /// </summary>
    public class MultiByteCharFactory : INCursesCharFactory<IMultiByteChar, IMultiByteCharString>
    {
        public static MultiByteCharFactory Instance { get; }

        internal static INCursesCharFactory<IMultiByteChar, IMultiByteCharString> Factory { get; }

        private static Type FactoryType;

        static MultiByteCharFactory()
        {
            Instance = new MultiByteCharFactory();

            FactoryType = typeof(MultiByteCharFactoryInternal<>).MakeGenericType(DynamicTypeBuilder.cchar_t);

            PropertyInfo property = FactoryType.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo getMethod = property.GetGetMethod(true);

            Factory = (INCursesCharFactory<IMultiByteChar, IMultiByteCharString>)getMethod.Invoke(null, Array.Empty<object>());
        }

        public IMultiByteChar GetNativeEmptyChar() => 
            Factory.GetNativeEmptyChar();

        public IMultiByteChar GetNativeChar(char ch) => 
            Factory.GetNativeChar(ch);

        public IMultiByteChar GetNativeChar(char ch, ulong attrs) => 
            Factory.GetNativeChar(ch, attrs);

        public IMultiByteChar GetNativeChar(char ch, ulong attrs, short colorPair) =>
            Factory.GetNativeChar(ch, attrs, colorPair);

        public IMultiByteCharString GetNativeEmptyString(int length) =>
            Factory.GetNativeEmptyString(length);

        public IMultiByteCharString GetNativeString(string str) =>
            Factory.GetNativeString(str);

        public IMultiByteCharString GetNativeString(string str, ulong attrs) =>
            Factory.GetNativeString(str, attrs);

        public IMultiByteCharString GetNativeString(string str, ulong attrs, short colorPair) =>
            Factory.GetNativeString(str, attrs, colorPair);

        public unsafe IMultiByteCharString GetNativeEmptyString(byte* buffer, int length)
            => Factory.GetNativeEmptyString(buffer, length);

        public IMultiByteCharString GetNativeEmptyString(byte[] buffer)
            => Factory.GetNativeEmptyString(buffer);

        public unsafe IMultiByteCharString GetNativeString(byte* buffer, int length, string str)
            => Factory.GetNativeString(buffer, length, str);

        public IMultiByteCharString GetNativeString(byte[] buffer, string str)
            => Factory.GetNativeString(buffer, str);

        public unsafe IMultiByteCharString GetNativeString(byte* buffer, int length, string str, ulong attrs)
            => Factory.GetNativeString(buffer, length, str, attrs);

        public IMultiByteCharString GetNativeString(byte[] buffer, string str, ulong attrs)
            => Factory.GetNativeString(buffer, str, attrs);

        public unsafe IMultiByteCharString GetNativeString(byte* buffer, int length, string str, ulong attrs, short colorPair)
            => Factory.GetNativeString(buffer, length, str, attrs, colorPair);

        public IMultiByteCharString GetNativeString(byte[] buffer, string str, ulong attrs, short colorPair)
            => Factory.GetNativeString(buffer, str, attrs, colorPair);

        public int GetByteCount(string str)
            => Factory.GetByteCount(str);

        public int GetByteCount(int length)
            => Factory.GetByteCount(length);

        public int GetCharLength()
            => Factory.GetCharLength();
    }

    internal class MultiByteCharFactoryInternal<TMultiByte> :
        INCursesCharFactoryInternal<IMultiByteChar, IMultiByteCharString, TMultiByte, MultiByteCharString<TMultiByte>>
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
    {
        internal static Func<ArraySegment<byte>, TMultiByte> CreateCharFromArraySegment;
        internal static Func<ArraySegment<byte>, ulong, TMultiByte> CreateCharWithAttributeFromArraySegment;
        internal static Func<ArraySegment<byte>, ulong, short, TMultiByte> CreateCharWithAttributeAndColorPairFromArraySegment;

        internal static CreateMultiByteCharFromSpan<TMultiByte> CreateCharFromSpan;
        internal static CreateMultiByteCharWithAttributeFromSpan<TMultiByte> CreateCharWithAttributeFromSpan;
        internal static CreateMultiByteCharWithAttributeAndColorPairFromSpan<TMultiByte> CreateCharWithAttributeAndColorPairFromSpan;

        internal static Func<char, TMultiByte> CreateCharFromChar;
        internal static Func<char, ulong, TMultiByte> CreateCharWithAttributeFromChar;
        internal static Func<char, ulong, short, TMultiByte> CreateCharWithAttributeAndColorPairFromChar;

        internal static MultiByteCharFactoryInternal<TMultiByte> Instance { get; }

        static MultiByteCharFactoryInternal()
        {
            Instance = new MultiByteCharFactoryInternal<TMultiByte>();

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(ArraySegment<byte>));
            par2 = Expression.Parameter(typeof(ulong));
            par3 = Expression.Parameter(typeof(short));

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(ArraySegment<byte>) });
            CreateCharFromArraySegment =
                Expression.Lambda<Func<ArraySegment<byte>, TMultiByte>>(Expression.New(ctor, par1), par1).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(ArraySegment<byte>), typeof(ulong) });
            CreateCharWithAttributeFromArraySegment =
                Expression.Lambda<Func<ArraySegment<byte>, ulong, TMultiByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(ArraySegment<byte>), typeof(ulong), typeof(short) });
            CreateCharWithAttributeAndColorPairFromArraySegment =
                Expression.Lambda<Func<ArraySegment<byte>, ulong, short, TMultiByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(Span<byte>));

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(Span<byte>) });
            CreateCharFromSpan =
                Expression.Lambda<CreateMultiByteCharFromSpan<TMultiByte>>(Expression.New(ctor, par1), par1).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(Span<byte>), typeof(ulong) });
            CreateCharWithAttributeFromSpan =
                Expression.Lambda<CreateMultiByteCharWithAttributeFromSpan<TMultiByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(Span<byte>), typeof(ulong), typeof(short) });
            CreateCharWithAttributeAndColorPairFromSpan =
                Expression.Lambda<CreateMultiByteCharWithAttributeAndColorPairFromSpan<TMultiByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(char));
            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(char) });
            CreateCharFromChar =
                Expression.Lambda<Func<char, TMultiByte>>(Expression.New(ctor, par1), par1).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(char), typeof(ulong) });
            CreateCharWithAttributeFromChar =
                Expression.Lambda<Func<char, ulong, TMultiByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(char), typeof(ulong), typeof(short) });
            CreateCharWithAttributeAndColorPairFromChar =
                Expression.Lambda<Func<char, ulong, short, TMultiByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();
        }

        #region ICharFactoryInternal<TChar, TString>
        public TMultiByte GetNativeEmptyCharInternal() =>
            CreateCharFromChar('\0');

        public TMultiByte GetNativeCharInternal(char ch) =>
            CreateCharFromChar(ch);

        public unsafe MultiByteCharString<TMultiByte> GetNativeEmptyStringInternal(byte* buffer, int length) =>
            new MultiByteCharString<TMultiByte>(buffer, length);

        public MultiByteCharString<TMultiByte> GetNativeEmptyStringInternal(byte[] buffer) =>
            new MultiByteCharString<TMultiByte>(buffer);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, string str) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, string str) =>
            new MultiByteCharString<TMultiByte>(buffer, str);

        public int GetByteCount(string str, bool addNullTerminator = true) =>
            (str.Length * Marshal.SizeOf<TMultiByte>()) + (addNullTerminator ? Marshal.SizeOf<TMultiByte>() : 0);

        public int GetByteCount(int length, bool addNullTerminator = true) =>
            (length * Marshal.SizeOf<TMultiByte>()) + (addNullTerminator ? Marshal.SizeOf<TMultiByte>() : 0);

        public int GetCharLength() => Marshal.SizeOf<TMultiByte>();

        public MultiByteCharString<TMultiByte> CreateNativeString(ref TMultiByte strRef)
            => new MultiByteCharString<TMultiByte>(ref strRef);
        #endregion

        #region ICharFactoryInternal<TCharType, TStringType, TChar, TString>
        public TMultiByte GetNativeCharInternal(char ch, ulong attrs) => 
            CreateCharWithAttributeFromChar(ch, attrs);

        public TMultiByte GetNativeCharInternal(char ch, ulong attrs, short colorPair) => 
            CreateCharWithAttributeAndColorPairFromChar(ch, attrs, colorPair);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, string str, ulong attrs) => 
            new MultiByteCharString<TMultiByte>(buffer, length, str, attrs);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, string str, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, str, attrs);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, string str, ulong attrs, short colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str, attrs, colorPair);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, string str, ulong attrs, short colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, str, attrs, colorPair);
        #endregion

        #region ICharFactory
        public IMultiByteChar GetNativeEmptyChar() =>
            this.GetNativeEmptyCharInternal();

        public IMultiByteChar GetNativeChar(char ch) =>
            this.GetNativeCharInternal(ch);

        public IMultiByteChar GetNativeChar(char ch, ulong attr) =>
            this.GetNativeCharInternal(ch, attr);

        public IMultiByteChar GetNativeChar(char ch, ulong attr, short pair) =>
            this.GetNativeCharInternal(ch, attr, pair);

        public unsafe IMultiByteCharString GetNativeEmptyString(byte* buffer, int length)
            => this.GetNativeEmptyString(buffer, length);

        public IMultiByteCharString GetNativeEmptyString(byte[] buffer)
            => this.GetNativeEmptyStringInternal(buffer);

        public IMultiByteCharString GetNativeEmptyString(int length)
            => this.GetNativeEmptyString(new byte[this.GetByteCount(length)]);

        public unsafe IMultiByteCharString GetNativeString(byte* buffer, int length, string str)
            => this.GetNativeStringInternal(buffer, length, str);

        public IMultiByteCharString GetNativeString(byte[] buffer, string str)
            => this.GetNativeStringInternal(buffer,  str);

        public IMultiByteCharString GetNativeString(string str)
            => this.GetNativeString(new byte[this.GetByteCount(str)], str);

        public unsafe IMultiByteCharString GetNativeString(byte* buffer, int length, string str, ulong attrs)
            => this.GetNativeStringInternal(buffer, length, str, attrs);

        public IMultiByteCharString GetNativeString(byte[] buffer, string str, ulong attrs)
            => this.GetNativeStringInternal(buffer, str, attrs);

        public IMultiByteCharString GetNativeString(string str, ulong attrs)
            => this.GetNativeString(new byte[this.GetByteCount(str)], str, attrs);

        public unsafe IMultiByteCharString GetNativeString(byte* buffer, int length, string str, ulong attrs, short colorPair)
            => this.GetNativeStringInternal(buffer, length, str, attrs, colorPair);

        public IMultiByteCharString GetNativeString(byte[] buffer, string str, ulong attrs, short colorPair)
            => this.GetNativeStringInternal(buffer, str, attrs, colorPair);

        public IMultiByteCharString GetNativeString(string str, ulong attrs, short colorPair)
            => this.GetNativeStringInternal(new byte[this.GetByteCount(str)], str, attrs, colorPair);

        public int GetByteCount(string str)
            => this.GetByteCount(str, true);

        public int GetByteCount(int length)
            => this.GetByteCount(length, true);
        #endregion

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, ReadOnlySpan<byte> str, Encoding encoding) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str, encoding);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, ReadOnlySpan<byte> str, Encoding encoding) =>
            new MultiByteCharString<TMultiByte>(buffer, str, encoding);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, ReadOnlySpan<byte> str, Encoding encoding, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str, encoding, attrs);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, ReadOnlySpan<byte> str, Encoding encoding, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, str, encoding, attrs);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, short colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str, encoding, attrs, colorPair);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, short colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, str, encoding, attrs, colorPair);

        public unsafe int GetByteCount(ReadOnlySpan<byte> bytes, Encoding encoding, bool addNullTerminator = true)
        {
            Decoder decoder = encoding.GetDecoder();
            fixed (byte* originalBytes = bytes)
            {
                return (decoder.GetCharCount(originalBytes, bytes.Length, false) * Marshal.SizeOf<TMultiByte>()) + (addNullTerminator ? Marshal.SizeOf<TMultiByte>() : 0);
            }
        }

        public unsafe IMultiByteCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<byte> str, Encoding encoding)
            => this.GetNativeStringInternal(buffer, length, str, encoding);

        public IMultiByteCharString GetNativeString(byte[] buffer, ReadOnlySpan<byte> str, Encoding encoding)
            => this.GetNativeStringInternal(buffer, str, encoding);

        public IMultiByteCharString GetNativeString(ReadOnlySpan<byte> str, Encoding encoding)
            => this.GetNativeString(new byte[this.GetByteCount(str, encoding)], str, encoding);

        public unsafe IMultiByteCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<byte> str, Encoding encoding, ulong attr)
            => this.GetNativeStringInternal(buffer, length, str, encoding, attr);

        public IMultiByteCharString GetNativeString(byte[] buffer, ReadOnlySpan<byte> str, Encoding encoding, ulong attr)
            => this.GetNativeStringInternal(buffer, str, encoding, attr);

        public IMultiByteCharString GetNativeString(ReadOnlySpan<byte> str, Encoding encoding, ulong attrs)
            => this.GetNativeString(new byte[this.GetByteCount(str, encoding)], str, encoding, attrs);

        public unsafe IMultiByteCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<byte> str, Encoding encoding, ulong attr, short pair)
            => this.GetNativeStringInternal(buffer, length, str, encoding, attr, pair);

        public IMultiByteCharString GetNativeString(byte[] buffer, ReadOnlySpan<byte> str, Encoding encoding, ulong attr, short pair)
            => this.GetNativeStringInternal(buffer, str, encoding, attr, pair);

        public IMultiByteCharString GetNativeString(ReadOnlySpan<byte> str, Encoding encoding, ulong attr, short pair)
            => this.GetNativeString(new byte[this.GetByteCount(str, encoding)], str, encoding, attr, pair);
    }
}
