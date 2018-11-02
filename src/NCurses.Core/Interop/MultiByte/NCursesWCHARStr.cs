using System;
using System.Collections.Generic;
using System.Linq;
using System.Buffers;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.MultiByte
{
    public class NCursesWCHARStr<TWide> : INCursesWCHARStr
        where TWide : unmanaged, INCursesWCHAR, IEquatable<TWide>
    {
        internal Memory<TWide> WCHAR;
        //internal ref readonly TWide this[int index] => ref this.wchar[index];
        internal TWide[] wchar;

        private int position;

        INCursesWCHAR IEnumerator<INCursesWCHAR>.Current => new NCursesWCHAR<TWide>(ref this.wchar[this.position]);
        INCursesChar IEnumerator<INCursesChar>.Current => new NCursesWCHAR<TWide>(ref this.wchar[this.position]);
        public object Current => new NCursesWCHAR<TWide>(ref this.wchar[this.position]);
        public int Length { get; private set; }
        public INCursesChar this[int index] => new NCursesWCHAR<TWide>(ref this.wchar[index]);

        internal static Func<ArraySegment<byte>, TWide> arrayCreate;
        internal static Func<ArraySegment<byte>, ulong, TWide> arrayAttrCreate;
        internal static Func<ArraySegment<byte>, ulong, short, TWide> arrayAttrColorCreate;

        static NCursesWCHARStr()
        {
            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(ArraySegment<byte>));
            ctor = typeof(TWide).GetConstructor(new Type[] { typeof(ArraySegment<byte>) });
            arrayCreate = Expression.Lambda<Func<ArraySegment<byte>, TWide>>(Expression.New(ctor, par1), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TWide).GetConstructor(new Type[] { typeof(ArraySegment<byte>), typeof(ulong) });
            arrayAttrCreate = Expression.Lambda<Func<ArraySegment<byte>, ulong, TWide>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = typeof(TWide).GetConstructor(new Type[] { typeof(ArraySegment<byte>), typeof(ulong), typeof(short) });
            arrayAttrColorCreate = Expression.Lambda<Func<ArraySegment<byte>, ulong, short, TWide>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();
        }

        public NCursesWCHARStr(string str)
        {
            this.Length = str.Length;
            this.CreateWCHARArray(str.AsSpan());
        }

        public NCursesWCHARStr(string str, ulong attrs)
        {
            this.Length = str.Length;
            this.CreateWCHARArray(str.AsSpan(), attrs);
        }

        public NCursesWCHARStr(string str, ulong attrs, short pair)
        {
            this.Length = str.Length;
            this.CreateWCHARArray(str.AsSpan(), attrs, pair);
        }

        public NCursesWCHARStr(byte[] str, Encoding encoding)
        {
            this.CreateWCHARArray(str, encoding);
        }

        public NCursesWCHARStr(byte[] str, Encoding encoding, ulong attrs)
        {
            this.CreateWCHARArray(
                str, encoding,
                attrs);
        }

        public NCursesWCHARStr(byte[] str, Encoding encoding, ulong attrs, short color)
        {
            this.CreateWCHARArray(
                str, encoding,
                attrs, color);
        }

        public NCursesWCHARStr(int length)
        {
            this.Length = length;
            this.wchar = ArrayPool<TWide>.Shared.Rent(length);
            this.WCHAR = new Memory<TWide>(this.wchar);
        }

        ~NCursesWCHARStr()
        {
            this.Dispose();
        }

        private unsafe void CreateWCHARArray(ReadOnlySpan<char> charArray, ulong attrs = 0, short colorPair = 0, bool addNullTerminator = true)
        {
            this.wchar = ArrayPool<TWide>.Shared.Rent(charArray.Length + (addNullTerminator ? 1 : 0));
            this.WCHAR = new Memory<TWide>(this.wchar);

            fixed (char* originalChars = charArray)
            {
                int byteCount = NativeNCurses.Encoding.GetEncoder().GetByteCount(originalChars, charArray.Length, false);
                byte[] encodedBytes = new byte[byteCount]; //TODO: use buffer? check if IL uses stack
                int bytesUsed = 0, charsUsed = 0, charPosition = 0, bytePosition = 0;
                bool completed = false;

                fixed (byte* bytePtr = encodedBytes)
                {
                    for (int i = 0; i < charArray.Length; i++)
                    {
                        charPosition += charsUsed;
                        bytePosition += bytesUsed;
                        NativeNCurses.Encoding.GetEncoder().Convert(
                            originalChars + charPosition, 1,
                            bytePtr + bytePosition, byteCount - bytePosition,
                            i == charArray.Length - 1 ? true : false,
                            out charsUsed, out bytesUsed, out completed);

                        if (!completed)
                            throw new InvalidOperationException("Could not complete encoding string");

                        this.wchar[i] = this.CreateWideChar(new ArraySegment<byte>(encodedBytes, bytePosition, bytesUsed), attrs, colorPair);
                    }
                }
            }

            if (addNullTerminator)
                this.wchar[this.wchar.Length - 1] = NCursesWCHAR<TWide>.charCreate('\0');
        }

        private TWide CreateWideChar(ArraySegment<byte> encodedBytes, ulong attrs = 0, short colorPair = 0)
        {
            if (attrs == 0 && colorPair == 0)
                return arrayCreate(encodedBytes);
            else if (colorPair == 0)
                return arrayAttrCreate(encodedBytes, attrs);
            else
                return arrayAttrColorCreate(encodedBytes, attrs, colorPair);
        }

        private unsafe void CreateWCHARArray(ReadOnlySpan<byte> bytes, Encoding encoding, ulong attrs = 0, short colorPair = 0)
        {
            fixed (byte* originalBytes = bytes)
            {
                this.Length = encoding.GetDecoder().GetCharCount(originalBytes, bytes.Length, false);
                char* chars = stackalloc char[this.Length];
                encoding.GetDecoder().Convert(originalBytes, bytes.Length, chars, this.Length, true, out int bytesUsed, out int charsUsed, out bool completed);
                if (!completed)
                    throw new InvalidOperationException($"Could not cast {encoding.EncodingName} to characters");

                ReadOnlySpan<char> charSpan = charSpan = new ReadOnlySpan<char>(chars, this.Length);
                this.CreateWCHARArray(charSpan, attrs, colorPair);
            }
        }

        public static explicit operator NCursesWCHARStr<TWide>(string str)
        {
            return new NCursesWCHARStr<TWide>(str);
        }

        public IEnumerator<INCursesWCHAR> GetEnumerator()
        {
            return this;
        }

        IEnumerator<INCursesChar> IEnumerable<INCursesChar>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            return ++this.position < this.WCHAR.Length;
        }

        public void Reset()
        {
            this.position = 0;
        }

        public static explicit operator string(NCursesWCHARStr<TWide> wStr)
        {
            unsafe
            {
                char* charArr = stackalloc char[wStr.Length];
                for (int i = 0; i < wStr.Length; i++)
                    charArr[i] = wStr.wchar[i].Char;
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, wStr.Length);
                return charSpan.ToString();
            }
        }

        public static bool operator ==(in NCursesWCHARStr<TWide> wchStrLeft, in NCursesWCHARStr<TWide> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public static bool operator !=(in NCursesWCHARStr<TWide> wchStrLeft, in NCursesWCHARStr<TWide> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public bool Equals(INCursesCharStr obj)
        {
            if (obj is INCursesWCHARStr other)
                return this.Equals(other);
            return false;
        }

        public bool Equals(INCursesWCHARStr obj)
        {
            ReadOnlySpan<TWide> left = new ReadOnlySpan<TWide>(this.wchar);
            ReadOnlySpan<TWide> right = new ReadOnlySpan<TWide>(this.wchar);
            return left.SequenceEqual(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is INCursesWCHARStr other)
                return this.Equals(other);
            return false;
        }

        public override string ToString()
        {
            return (string)this;
        }

        public override int GetHashCode()
        {
            return 1594223146 + EqualityComparer<TWide[]>.Default.GetHashCode(this.wchar);
        }

        public void Dispose()
        {
            this.position = 0;
            ArrayPool<TWide>.Shared.Return(this.wchar, true);
            GC.SuppressFinalize(this);
        }
    }
}
