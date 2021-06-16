using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;
using System.Buffers;

namespace NCurses.Core.Interop.MultiByte
{
    internal delegate TMultiByte CreateMultiByteCharFromSpan<TMultiByte>(Span<byte> byteSpan)
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>;

    internal delegate TMultiByte CreateMultiByteCharWithAttributeFromSpan<TMultiByte>(Span<byte> byteSpan, ulong attrs)
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>;

    internal delegate TMultiByte CreateMultiByteCharWithAttributeAndColorPairFromSpan<TMultiByte>(Span<byte> byteSpan, ulong attrs, ushort color)
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>;

    /// <summary>
    /// Create native chars strings
    /// These methods should not be used, because these incur heap allocations and alot of boxing
    /// </summary>
    public class MultiByteCharFactory : INCursesCharFactory<IMultiByteNCursesChar, IMultiByteNCursesCharString>
    {
        public static MultiByteCharFactory Instance { get; }

        internal static INCursesCharFactory<IMultiByteNCursesChar, IMultiByteNCursesCharString> Factory { get; }

        private static Type FactoryType;

        static MultiByteCharFactory()
        {
            Instance = new MultiByteCharFactory();

            FactoryType = typeof(MultiByteCharFactoryInternal<>).MakeGenericType(Constants.MultiByteChar);

            PropertyInfo property = FactoryType.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo getMethod = property.GetGetMethod(true);

            Factory = (INCursesCharFactory<IMultiByteNCursesChar, IMultiByteNCursesCharString>)getMethod.Invoke(null, Array.Empty<object>());
        }

        public IMultiByteNCursesChar GetNativeEmptyChar() => 
            Factory.GetNativeEmptyChar();

        public IMultiByteNCursesChar GetNativeChar(char ch) => 
            Factory.GetNativeChar(ch);

        public IMultiByteNCursesChar GetNativeChar(char ch, ulong attrs) => 
            Factory.GetNativeChar(ch, attrs);

        public IMultiByteNCursesChar GetNativeChar(char ch, ulong attrs, ushort colorPair) =>
            Factory.GetNativeChar(ch, attrs, colorPair);

        public IMultiByteNCursesCharString GetNativeEmptyString(int length) =>
            Factory.GetNativeEmptyString(length);

        public IMultiByteNCursesCharString GetNativeString(string str) =>
            Factory.GetNativeString(str);

        public IMultiByteNCursesCharString GetNativeString(ReadOnlySpan<char> str) =>
            Factory.GetNativeString(str);

        public IMultiByteNCursesCharString GetNativeString(string str, ulong attrs) =>
            Factory.GetNativeString(str, attrs);

        public IMultiByteNCursesCharString GetNativeString(ReadOnlySpan<char> str, ulong attrs) =>
            Factory.GetNativeString(str, attrs);

        public IMultiByteNCursesCharString GetNativeString(string str, ulong attrs, ushort colorPair) =>
            Factory.GetNativeString(str, attrs, colorPair);

        public IMultiByteNCursesCharString GetNativeString(ReadOnlySpan<char> str, ulong attrs, ushort colorPair) =>
            Factory.GetNativeString(str, attrs, colorPair);

        public unsafe IMultiByteNCursesCharString GetNativeEmptyString(byte* buffer, int bufferLenght, int charLength)
            => Factory.GetNativeEmptyString(buffer, bufferLenght, charLength);

        public IMultiByteNCursesCharString GetNativeEmptyString(byte[] buffer, int bufferLength, int charLength)
            => Factory.GetNativeEmptyString(buffer, bufferLength, charLength);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, string str)
            => Factory.GetNativeString(buffer, length, str);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<char> str)
            => Factory.GetNativeString(buffer, length, str);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, string str)
            => Factory.GetNativeString(buffer, bufferLength, str);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<char> str)
            => Factory.GetNativeString(buffer, bufferLength, str);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, string str, ulong attrs)
            => Factory.GetNativeString(buffer, length, str, attrs);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<char> str, ulong attrs)
            => Factory.GetNativeString(buffer, length, str, attrs);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, string str, ulong attrs)
            => Factory.GetNativeString(buffer, bufferLength, str, attrs);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs)
            => Factory.GetNativeString(buffer, bufferLength, str, attrs);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, string str, ulong attrs, ushort colorPair)
            => Factory.GetNativeString(buffer, length, str, attrs, colorPair);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<char> str, ulong attrs, ushort colorPair)
            => Factory.GetNativeString(buffer, length, str, attrs, colorPair);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, string str, ulong attrs, ushort colorPair)
            => Factory.GetNativeString(buffer, bufferLength, str, attrs, colorPair);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs, ushort colorPair)
            => Factory.GetNativeString(buffer, bufferLength, str, attrs, colorPair);

        public int GetByteCount(string str)
            => Factory.GetByteCount(str);

        public int GetByteCount(ReadOnlySpan<char> str)
            => Factory.GetByteCount(str);

        public int GetByteCount(int length)
            => Factory.GetByteCount(length);

        public int GetCharLength()
            => Factory.GetCharLength();
    }

    internal class MultiByteCharFactoryInternal<TMultiByte> :
        INCursesCharFactoryInternal<IMultiByteNCursesChar, IMultiByteNCursesCharString, TMultiByte, MultiByteCharString<TMultiByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
    {
        internal static Func<ArraySegment<byte>, TMultiByte> CreateCharFromArraySegment;
        internal static Func<ArraySegment<byte>, ulong, TMultiByte> CreateCharWithAttributeFromArraySegment;
        internal static Func<ArraySegment<byte>, ulong, ushort, TMultiByte> CreateCharWithAttributeAndColorPairFromArraySegment;

        internal static CreateMultiByteCharFromSpan<TMultiByte> CreateCharFromSpan;
        internal static CreateMultiByteCharWithAttributeFromSpan<TMultiByte> CreateCharWithAttributeFromSpan;
        internal static CreateMultiByteCharWithAttributeAndColorPairFromSpan<TMultiByte> CreateCharWithAttributeAndColorPairFromSpan;

        internal static Func<char, TMultiByte> CreateCharFromChar;
        internal static Func<char, ulong, TMultiByte> CreateCharWithAttributeFromChar;
        internal static Func<char, ulong, ushort, TMultiByte> CreateCharWithAttributeAndColorPairFromChar;

        internal static MultiByteCharFactoryInternal<TMultiByte> Instance { get; }

        private const int _BufferSize = 32;

        static MultiByteCharFactoryInternal()
        {
            Instance = new MultiByteCharFactoryInternal<TMultiByte>();

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(ArraySegment<byte>));
            par2 = Expression.Parameter(typeof(ulong));
            par3 = Expression.Parameter(typeof(ushort));

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(ArraySegment<byte>) });
            CreateCharFromArraySegment =
                Expression.Lambda<Func<ArraySegment<byte>, TMultiByte>>(Expression.New(ctor, par1), par1).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(ArraySegment<byte>), typeof(ulong) });
            CreateCharWithAttributeFromArraySegment =
                Expression.Lambda<Func<ArraySegment<byte>, ulong, TMultiByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(ArraySegment<byte>), typeof(ulong), typeof(ushort) });
            CreateCharWithAttributeAndColorPairFromArraySegment =
                Expression.Lambda<Func<ArraySegment<byte>, ulong, ushort, TMultiByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(Span<byte>));

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(Span<byte>) });
            CreateCharFromSpan =
                Expression.Lambda<CreateMultiByteCharFromSpan<TMultiByte>>(Expression.New(ctor, par1), par1).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(Span<byte>), typeof(ulong) });
            CreateCharWithAttributeFromSpan =
                Expression.Lambda<CreateMultiByteCharWithAttributeFromSpan<TMultiByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(Span<byte>), typeof(ulong), typeof(ushort) });
            CreateCharWithAttributeAndColorPairFromSpan =
                Expression.Lambda<CreateMultiByteCharWithAttributeAndColorPairFromSpan<TMultiByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(char));
            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(char) });
            CreateCharFromChar =
                Expression.Lambda<Func<char, TMultiByte>>(Expression.New(ctor, par1), par1).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(char), typeof(ulong) });
            CreateCharWithAttributeFromChar =
                Expression.Lambda<Func<char, ulong, TMultiByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(char), typeof(ulong), typeof(ushort) });
            CreateCharWithAttributeAndColorPairFromChar =
                Expression.Lambda<Func<char, ulong, ushort, TMultiByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();
        }

        #region ICharFactoryInternal<TChar, TString>
        public TMultiByte GetNativeEmptyCharInternal() =>
            CreateCharFromChar('\0');

        public TMultiByte GetNativeCharInternal(char ch) =>
            CreateCharFromChar(ch);

        public unsafe MultiByteCharString<TMultiByte> GetNativeEmptyStringInternal(byte* buffer, int bufferLenght, int charLength) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLenght, charLength);

        public MultiByteCharString<TMultiByte> GetNativeEmptyStringInternal(byte[] buffer, int bufferLength, int charLength) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, charLength);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, string str) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, ReadOnlySpan<char> str) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, string str) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySpan<char> str) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str);

        public int GetByteCount(string str, bool addNullTerminator = true) =>
            (str.Length * Marshal.SizeOf<TMultiByte>()) + (addNullTerminator ? Marshal.SizeOf<TMultiByte>() : 0);

        public int GetByteCount(ReadOnlySpan<char> str, bool addNullTerminator = true) =>
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

        public TMultiByte GetNativeCharInternal(char ch, ulong attrs, ushort colorPair) => 
            CreateCharWithAttributeAndColorPairFromChar(ch, attrs, colorPair);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, string str, ulong attrs) => 
            new MultiByteCharString<TMultiByte>(buffer, length, str, attrs);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, ReadOnlySpan<char> str, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str, attrs);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, string str, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, attrs);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, attrs);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, string str, ulong attrs, ushort colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str, attrs, colorPair);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int length, ReadOnlySpan<char> str, ulong attrs, ushort colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, length, str, attrs, colorPair);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, string str, ulong attrs, ushort colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, attrs, colorPair);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs, ushort colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, attrs, colorPair);
        #endregion

        #region ICharFactory
        public IMultiByteNCursesChar GetNativeEmptyChar() =>
            this.GetNativeEmptyCharInternal();

        public IMultiByteNCursesChar GetNativeChar(char ch) =>
            this.GetNativeCharInternal(ch);

        public IMultiByteNCursesChar GetNativeChar(char ch, ulong attr) =>
            this.GetNativeCharInternal(ch, attr);

        public IMultiByteNCursesChar GetNativeChar(char ch, ulong attr, ushort pair) =>
            this.GetNativeCharInternal(ch, attr, pair);

        public unsafe IMultiByteNCursesCharString GetNativeEmptyString(byte* buffer, int bufferLength, int charLength)
            => this.GetNativeEmptyString(buffer, bufferLength, charLength);

        public IMultiByteNCursesCharString GetNativeEmptyString(byte[] buffer, int bufferLength, int charLength)
            => this.GetNativeEmptyStringInternal(buffer, bufferLength, charLength);

        public IMultiByteNCursesCharString GetNativeEmptyString(int charLength)
            => this.GetNativeEmptyString(new byte[this.GetByteCount(charLength)], this.GetByteCount(charLength), charLength);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, string str)
            => this.GetNativeStringInternal(buffer, length, str);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<char> str)
            => this.GetNativeStringInternal(buffer, length, str);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, string str)
            => this.GetNativeStringInternal(buffer, bufferLength,  str);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<char> str)
            => this.GetNativeStringInternal(buffer, bufferLength, str);

        public IMultiByteNCursesCharString GetNativeString(string str)
            => this.GetNativeString(new byte[this.GetByteCount(str)], this.GetByteCount(str), str);

        public IMultiByteNCursesCharString GetNativeString(ReadOnlySpan<char> str)
            => this.GetNativeString(new byte[this.GetByteCount(str)], this.GetByteCount(str), str);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, string str, ulong attrs)
            => this.GetNativeStringInternal(buffer, length, str, attrs);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<char> str, ulong attrs)
            => this.GetNativeStringInternal(buffer, length, str, attrs);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, string str, ulong attrs)
            => this.GetNativeStringInternal(buffer, bufferLength, str, attrs);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs)
            => this.GetNativeStringInternal(buffer, bufferLength, str, attrs);

        public IMultiByteNCursesCharString GetNativeString(string str, ulong attrs)
            => this.GetNativeString(new byte[this.GetByteCount(str)], this.GetByteCount(str), str, attrs);

        public IMultiByteNCursesCharString GetNativeString(ReadOnlySpan<char> str, ulong attrs)
            => this.GetNativeString(new byte[this.GetByteCount(str)], this.GetByteCount(str), str, attrs);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, string str, ulong attrs, ushort colorPair)
            => this.GetNativeStringInternal(buffer, length, str, attrs, colorPair);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<char> str, ulong attrs, ushort colorPair)
            => this.GetNativeStringInternal(buffer, length, str, attrs, colorPair);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, string str, ulong attrs, ushort colorPair)
            => this.GetNativeStringInternal(buffer, bufferLength, str, attrs, colorPair);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs, ushort colorPair)
            => this.GetNativeStringInternal(buffer, bufferLength, str, attrs, colorPair);

        public IMultiByteNCursesCharString GetNativeString(string str, ulong attrs, ushort colorPair)
            => this.GetNativeStringInternal(new byte[this.GetByteCount(str)], this.GetByteCount(str), str, attrs, colorPair);

        public IMultiByteNCursesCharString GetNativeString(ReadOnlySpan<char> str, ulong attrs, ushort colorPair)
            => this.GetNativeStringInternal(new byte[this.GetByteCount(str)], this.GetByteCount(str), str, attrs, colorPair);

        public int GetByteCount(string str)
            => this.GetByteCount(str, true);

        public int GetByteCount(ReadOnlySpan<char> str)
            => this.GetByteCount(str, true);

        public int GetByteCount(int length)
            => this.GetByteCount(length, true);
        #endregion

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int bufferLength, ReadOnlySpan<byte> str, int charLength, Encoding encoding) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int bufferLength, ReadOnlySequence<byte> str, int charLength, Encoding encoding) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySpan<byte> str, int charLength, Encoding encoding) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySequence<byte> str, int charLength, Encoding encoding) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int bufferLength, ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding, attrs);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int bufferLength, ReadOnlySequence<byte> str, int charLength, Encoding encoding, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding, attrs);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding, attrs);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySequence<byte> str, int charLength, Encoding encoding, ulong attrs) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding, attrs);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int bufferLength, ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attrs, ushort colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding, attrs, colorPair);

        public unsafe MultiByteCharString<TMultiByte> GetNativeStringInternal(byte* buffer, int bufferLength, ReadOnlySequence<byte> str, int charLength, Encoding encoding, ulong attrs, ushort colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding, attrs, colorPair);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attrs, ushort colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding, attrs, colorPair);

        public MultiByteCharString<TMultiByte> GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySequence<byte> str, int charLength, Encoding encoding, ulong attrs, ushort colorPair) =>
            new MultiByteCharString<TMultiByte>(buffer, bufferLength, str, charLength, encoding, attrs, colorPair);

        public unsafe int GetByteCount(ReadOnlySpan<byte> bytes, Encoding encoding, out int charLength, bool addNullTerminator = true)
        {
            Decoder decoder = encoding.GetDecoder();
            fixed (byte* originalBytes = bytes)
            {
                return ((charLength = decoder.GetCharCount(originalBytes, bytes.Length, true)) * Marshal.SizeOf<TMultiByte>()) + (addNullTerminator ? Marshal.SizeOf<TMultiByte>() : 0);
            }
        }

        public int GetByteCount(ReadOnlySequence<byte> sequence, Encoding encoding, out int charLength, bool addNullTerminator = true)
        {
            Decoder decoder = encoding.GetDecoder();
            charLength = 0;

            int charsUsed = 0, bytesUsed = 0, length = 0, bytesLeft, newByteIndex;
            long currentLeft;
            Span<byte> bufferSpan = default;
            Span<char> charSpan = default;
            ReadOnlySpan<byte> readableSpan;
            currentLeft = sequence.Length;
            bool completed = false;

            byte[] decoderByteBuffer = ArrayPool<byte>.Shared.Rent(_BufferSize);
            //use the same buffer size, because a 1 byte UTF8 rune could be a char
            char[] decoderCharBuffer = ArrayPool<char>.Shared.Rent(_BufferSize);

            try
            {
                charSpan = new Span<char>(decoderCharBuffer);
                bufferSpan = new Span<byte>(decoderByteBuffer);

                foreach (ReadOnlyMemory<byte> memory in sequence)
                {
                    if (memory.IsEmpty)
                    {
                        continue;
                    }

                    readableSpan = memory.Span;

                    while (!readableSpan.IsEmpty)
                    {
                        //there's still bytes left from the previous conversion
                        if ((bytesLeft = (length - bytesUsed)) > 0)
                        {
                            if (bytesUsed > 0)
                            {
                                //move these to the beginning of the buffer (1 by 1)
                                newByteIndex = 0;
                                for (int i = bytesUsed; i < length; i++)
                                {
                                    bufferSpan[newByteIndex++] = bufferSpan[i];
                                }
                            }

                            length = (bufferSpan.Length - bytesLeft) > readableSpan.Length ? readableSpan.Length : (bufferSpan.Length - bytesLeft);
                            readableSpan.Slice(0, length).CopyTo(bufferSpan.Slice(bytesLeft));
                            readableSpan = readableSpan.Slice(length);
                            length += bytesLeft;
                        }
                        else
                        {
                            length = bufferSpan.Length > readableSpan.Length ? readableSpan.Length : bufferSpan.Length;
                            readableSpan.Slice(0, length).CopyTo(bufferSpan);
                            readableSpan = readableSpan.Slice(length);
                        }

                        unsafe
                        {
                            fixed (byte* b = bufferSpan)
                            {
                                fixed (char* c = charSpan)
                                {
                                    decoder.Convert
                                    (
                                        b,
                                        length,
                                        c,
                                        decoderCharBuffer.Length,
                                        currentLeft <= length,
                                        out bytesUsed,
                                        out charsUsed,
                                        out completed
                                    );
                                }
                            }
                        }

                        currentLeft -= bytesUsed;
                        charLength += charsUsed;
                    }
                }

                if (!completed)
                {
                    throw new InvalidOperationException("Encoding conversion did not succeed");
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(decoderByteBuffer);
                ArrayPool<char>.Shared.Return(decoderCharBuffer);
            }

            return (charLength * Marshal.SizeOf<TMultiByte>()) + (addNullTerminator ? Marshal.SizeOf<TMultiByte>() : 0);
        }

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<byte> str, int charLength, Encoding encoding)
            => this.GetNativeStringInternal(buffer, length, str, charLength, encoding);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySequence<byte> str, int charLength, Encoding encoding)
            => this.GetNativeStringInternal(buffer, length, str, charLength, encoding);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<byte> str, int charLength, Encoding encoding)
            => this.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySequence<byte> str, int charLength, Encoding encoding)
            => this.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding);

        //public IMultiByteNCursesCharString GetNativeString(ReadOnlySpan<byte> str, int charLength, Encoding encoding)
        //    => this.GetNativeString(new byte[this.GetByteCount(str, encoding)], this.GetByteCount(str, encoding), str, charLength, encoding);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attr)
            => this.GetNativeStringInternal(buffer, length, str, charLength, encoding, attr);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySequence<byte> str, int charLength, Encoding encoding, ulong attr)
            => this.GetNativeStringInternal(buffer, length, str, charLength, encoding, attr);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attr)
            => this.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding, attr);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySequence<byte> str, int charLength, Encoding encoding, ulong attr)
            => this.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding, attr);

        //public IMultiByteNCursesCharString GetNativeString(ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attrs)
        //    => this.GetNativeString(new byte[this.GetByteCount(str, encoding)], this.GetByteCount(str, encoding), str, charLength, encoding, attrs);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attr, ushort pair)
            => this.GetNativeStringInternal(buffer, length, str, charLength, encoding, attr, pair);

        public unsafe IMultiByteNCursesCharString GetNativeString(byte* buffer, int length, ReadOnlySequence<byte> str, int charLength, Encoding encoding, ulong attr, ushort pair)
            => this.GetNativeStringInternal(buffer, length, str, charLength, encoding, attr, pair);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attr, ushort pair)
            => this.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding, attr, pair);

        public IMultiByteNCursesCharString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySequence<byte> str, int charLength, Encoding encoding, ulong attr, ushort pair)
            => this.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding, attr, pair);

        //public IMultiByteNCursesCharString GetNativeString(ReadOnlySpan<byte> str, int charLength, Encoding encoding, ulong attr, ushort pair)
        //    => this.GetNativeString(new byte[this.GetByteCount(str, encoding)], this.GetByteCount(str, encoding), str, charLength, encoding, attr, pair);
    }
}
