using System;
using System.Collections;
using System.Collections.Generic;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace NCurses.Core.Interop.SingleByte
{
    public class SingleByteCharString<TSingleByte> : ISingleByteCharString
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
    {
        internal Memory<TSingleByte> CharString;
        //internal ref readonly TSmall this[int index] => ref this.schar[index];
        internal TSingleByte[] charString;

        private int position = -1;

        ISingleByteChar IEnumerator<ISingleByteChar>.Current => new SingleByteChar<TSingleByte>(ref this.charString[this.position]);
        INCursesChar IEnumerator<INCursesChar>.Current => new SingleByteChar<TSingleByte>(ref this.charString[this.position]);
        public object Current => new SingleByteChar<TSingleByte>(ref this.charString[this.position]);
        public int Length { get; private set; }
        public INCursesChar this[int index] => new SingleByteChar<TSingleByte>(ref this.charString[index]);

        public SingleByteCharString(string str)
        {
            this.Length = str.Length;
            this.CreateSCHARArray(str.AsSpan());
        }

        public SingleByteCharString(string str, ulong attrs)
        {
            this.Length = str.Length;
            this.CreateSCHARArray(str.AsSpan(), attrs);
        }

        public SingleByteCharString(string str, ulong attrs, short pair)
        {
            this.Length = str.Length;
            this.CreateSCHARArray(str.AsSpan(), attrs, pair);
        }

        public SingleByteCharString(int length)
        {
            this.Length = length;
            this.charString = ArrayPool<TSingleByte>.Shared.Rent(length);
            this.CharString = new Memory<TSingleByte>(this.charString);
        }

        ~SingleByteCharString()
        {
            this.Dispose();
        }

        private unsafe void CreateSCHARArray(ReadOnlySpan<char> charArray, ulong attrs = 0, short colorPair = 0, bool addNullTerminator = true)
        {
            int bytesUsed = 0, charsUsed = 0;
            bool completed = false;

            this.charString = ArrayPool<TSingleByte>.Shared.Rent(charArray.Length + (addNullTerminator ? 1 : 0));
            this.CharString = new Memory<TSingleByte>(this.charString);

            fixed (char* originalChars = charArray)
            {
                Encoder encoder = Encoding.ASCII.GetEncoder();
                int byteCount = encoder.GetByteCount(originalChars, charArray.Length, false);
                byte* bytePtr = stackalloc byte[byteCount];


                encoder.Convert(
                    originalChars, 
                    charArray.Length,
                    bytePtr,
                    byteCount,
                    true,
                    out charsUsed, 
                    out bytesUsed, 
                    out completed);

                if (!completed)
                {
                    throw new InvalidOperationException("Could not complete encoding string");
                }

                for (int i = 0; i < byteCount; i++)
                {
                    this.charString[i] = this.CreateSmallChar((sbyte)bytePtr[i], attrs, colorPair);
                }
            }

            if (addNullTerminator)
            {
                this.charString[charArray.Length] = SingleByteChar<TSingleByte>.byteCreate(0);
            }
        }

        private TSingleByte CreateSmallChar(sbyte encodedByte, ulong attrs = 0, short colorPair = 0)
        {
            if (attrs == 0 && colorPair == 0)
                return SingleByteChar<TSingleByte>.byteCreate(encodedByte);
            else if (colorPair == 0)
                return SingleByteChar<TSingleByte>.byteAttrCreate(encodedByte, attrs);
            else
                return SingleByteChar<TSingleByte>.byteAttrColorCreate(encodedByte, attrs, colorPair);
        }

        public IEnumerator<ISingleByteChar> GetEnumerator()
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
            this.position = -1;
        }

        //TODO: crashes Visual Studio debugger when using netcoreapp2.0
        public static explicit operator string(SingleByteCharString<TSingleByte> str)
        {
            unsafe
            {
                char* charArr = stackalloc char[str.Length];
                for (int i = 0; i < str.Length; i++)
                    charArr[i] = str.charString[i].Char;
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, str.Length);
                return charSpan.ToString();
            }
        }

        public static bool operator ==(in SingleByteCharString<TSingleByte> chStrLeft, in SingleByteCharString<TSingleByte> chStrRight)
        {
            return chStrLeft.Equals(chStrRight);
        }

        public static bool operator !=(in SingleByteCharString<TSingleByte> chStrLeft, in SingleByteCharString<TSingleByte> chStrRight)
        {
            return chStrLeft.Equals(chStrRight);
        }

        public bool Equals(INCursesCharString obj)
        {
            if (obj is ISingleByteCharString other)
                return this.Equals(other);
            return false;
        }

        public bool Equals(ISingleByteCharString obj)
        {
            ReadOnlySpan<TSingleByte> left = new ReadOnlySpan<TSingleByte>(this.charString);
            ReadOnlySpan<TSingleByte> right = new ReadOnlySpan<TSingleByte>(this.charString);
            return left.SequenceEqual(right);
        }

        public override bool Equals(object obj)
        {
            if (obj is ISingleByteCharString other)
                return this.Equals(other);
            return false;
        }

        public override string ToString()
        {
            return (string)this;
        }

        public override int GetHashCode()
        {
            return -158990394 + EqualityComparer<TSingleByte[]>.Default.GetHashCode(this.charString);
        }

        public void Dispose()
        {
            this.position = -1;
            ArrayPool<TSingleByte>.Shared.Return(this.charString, true);
            GC.SuppressFinalize(this);
        }
    }
}
