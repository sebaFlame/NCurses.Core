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
    public class MultiByteCharString<TMultiByte> : IMultiByteCharString
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
    {
        internal Memory<TMultiByte> CharString;
        //internal ref readonly TWide this[int index] => ref this.wchar[index];
        internal TMultiByte[] charString;

        private int position;

        IMultiByteChar IEnumerator<IMultiByteChar>.Current => new MultiByteChar<TMultiByte>(ref this.charString[this.position]);
        INCursesChar IEnumerator<INCursesChar>.Current => new MultiByteChar<TMultiByte>(ref this.charString[this.position]);
        public object Current => new MultiByteChar<TMultiByte>(ref this.charString[this.position]);
        public int Length { get; private set; }
        public INCursesChar this[int index] => new MultiByteChar<TMultiByte>(ref this.charString[index]);

        internal static Func<ArraySegment<byte>, TMultiByte> arrayCreate;
        internal static Func<ArraySegment<byte>, ulong, TMultiByte> arrayAttrCreate;
        internal static Func<ArraySegment<byte>, ulong, short, TMultiByte> arrayAttrColorCreate;

        static MultiByteCharString()
        {
            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(ArraySegment<byte>));
            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(ArraySegment<byte>) });
            arrayCreate = Expression.Lambda<Func<ArraySegment<byte>, TMultiByte>>(Expression.New(ctor, par1), par1).Compile();

            par2 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(ArraySegment<byte>), typeof(ulong) });
            arrayAttrCreate = Expression.Lambda<Func<ArraySegment<byte>, ulong, TMultiByte>>(Expression.New(ctor, par1, par2), par1, par2).Compile();

            par3 = Expression.Parameter(typeof(short));
            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(ArraySegment<byte>), typeof(ulong), typeof(short) });
            arrayAttrColorCreate = Expression.Lambda<Func<ArraySegment<byte>, ulong, short, TMultiByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();
        }

        public MultiByteCharString(string str)
        {
            this.Length = str.Length;
            this.CreateWCHARArray(str.AsSpan());
        }

        public MultiByteCharString(string str, ulong attrs)
        {
            this.Length = str.Length;
            this.CreateWCHARArray(str.AsSpan(), attrs);
        }

        public MultiByteCharString(string str, ulong attrs, short pair)
        {
            this.Length = str.Length;
            this.CreateWCHARArray(str.AsSpan(), attrs, pair);
        }

        public MultiByteCharString(byte[] str, Encoding encoding)
        {
            this.CreateWCHARArray(str, encoding);
        }

        public MultiByteCharString(byte[] str, Encoding encoding, ulong attrs)
        {
            this.CreateWCHARArray(
                str, encoding,
                attrs);
        }

        public MultiByteCharString(byte[] str, Encoding encoding, ulong attrs, short color)
        {
            this.CreateWCHARArray(
                str, encoding,
                attrs, color);
        }

        public MultiByteCharString(int length)
        {
            this.Length = length;
            this.charString = ArrayPool<TMultiByte>.Shared.Rent(length);
            this.CharString = new Memory<TMultiByte>(this.charString);
        }

        ~MultiByteCharString()
        {
            this.Dispose();
        }

        private unsafe void CreateWCHARArray(ReadOnlySpan<char> charArray, ulong attrs = 0, short colorPair = 0, bool addNullTerminator = true)
        {
            this.charString = ArrayPool<TMultiByte>.Shared.Rent(charArray.Length + (addNullTerminator ? 1 : 0));
            this.CharString = new Memory<TMultiByte>(this.charString);

            fixed (char* originalChars = charArray)
            {
                Encoder encoder = NativeNCurses.Encoding.GetEncoder();
                int byteCount = encoder.GetByteCount(originalChars, charArray.Length, false);
                byte[] encodedBytes = new byte[byteCount]; //TODO: use buffer? check if IL uses stack
                int bytesUsed = 0, charsUsed = 0, charPosition = 0, bytePosition = 0;
                bool completed = false;

                fixed (byte* bytePtr = encodedBytes)
                {
                    for (int i = 0; i < charArray.Length; i++)
                    {
                        charPosition += charsUsed;
                        bytePosition += bytesUsed;
                        encoder.Convert(
                            originalChars + charPosition, 1,
                            bytePtr + bytePosition, byteCount - bytePosition,
                            i == charArray.Length - 1 ? true : false,
                            out charsUsed, out bytesUsed, out completed);

                        if (!completed)
                            throw new InvalidOperationException("Could not complete encoding string");

                        this.charString[i] = this.CreateWideChar(new ArraySegment<byte>(encodedBytes, bytePosition, bytesUsed), attrs, colorPair);
                    }
                }
            }

            if (addNullTerminator)
                this.charString[this.charString.Length - 1] = MultiByteChar<TMultiByte>.charCreate('\0');
        }

        private TMultiByte CreateWideChar(ArraySegment<byte> encodedBytes, ulong attrs = 0, short colorPair = 0)
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
                Decoder decoder = encoding.GetDecoder();
                this.Length = decoder.GetCharCount(originalBytes, bytes.Length, false);
                char* chars = stackalloc char[this.Length];
                decoder.Convert(originalBytes, bytes.Length, chars, this.Length, true, out int bytesUsed, out int charsUsed, out bool completed);
                if (!completed)
                    throw new InvalidOperationException($"Could not cast {encoding.EncodingName} to characters");

                ReadOnlySpan<char> charSpan = charSpan = new ReadOnlySpan<char>(chars, this.Length);
                this.CreateWCHARArray(charSpan, attrs, colorPair);
            }
        }

        public static explicit operator MultiByteCharString<TMultiByte>(string str)
        {
            return new MultiByteCharString<TMultiByte>(str);
        }

        public IEnumerator<IMultiByteChar> GetEnumerator()
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
            return ++this.position < this.CharString.Length;
        }

        public void Reset()
        {
            this.position = 0;
        }

        public static explicit operator string(MultiByteCharString<TMultiByte> wStr)
        {
            unsafe
            {
                char* charArr = stackalloc char[wStr.Length];
                for (int i = 0; i < wStr.Length; i++)
                    charArr[i] = wStr.charString[i].Char;
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, wStr.Length);
                return charSpan.ToString();
            }
        }

        public static bool operator ==(in MultiByteCharString<TMultiByte> wchStrLeft, in MultiByteCharString<TMultiByte> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public static bool operator !=(in MultiByteCharString<TMultiByte> wchStrLeft, in MultiByteCharString<TMultiByte> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public bool Equals(INCursesCharString obj)
        {
            if (obj is IMultiByteCharString other)
                return this.Equals(other);
            return false;
        }

        public bool Equals(IMultiByteCharString obj)
        {
            ReadOnlySpan<TMultiByte> left = new ReadOnlySpan<TMultiByte>(this.charString);
            ReadOnlySpan<TMultiByte> right = new ReadOnlySpan<TMultiByte>(this.charString);
            return left.SequenceEqual(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is IMultiByteCharString other)
                return this.Equals(other);
            return false;
        }

        public override string ToString()
        {
            return (string)this;
        }

        public override int GetHashCode()
        {
            return 1594223146 + EqualityComparer<TMultiByte[]>.Default.GetHashCode(this.charString);
        }

        public void Dispose()
        {
            this.position = 0;
            ArrayPool<TMultiByte>.Shared.Return(this.charString, true);
            GC.SuppressFinalize(this);
        }
    }
}
