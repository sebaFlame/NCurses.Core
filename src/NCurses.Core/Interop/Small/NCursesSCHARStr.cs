using System;
using System.Collections;
using System.Collections.Generic;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace NCurses.Core.Interop.Small
{
    public class NCursesSCHARStr<TSmall> : INCursesSCHARStr
    where TSmall : unmanaged, INCursesSCHAR
    {
        internal Memory<TSmall> SCHAR;
        //internal ref readonly TSmall this[int index] => ref this.schar[index];
        internal TSmall[] schar;

        private int position;

        INCursesSCHAR IEnumerator<INCursesSCHAR>.Current => new NCursesSCHAR<TSmall>(ref this.schar[this.position]);
        INCursesChar IEnumerator<INCursesChar>.Current => new NCursesSCHAR<TSmall>(ref this.schar[this.position]);
        public object Current => new NCursesSCHAR<TSmall>(ref this.schar[this.position]);
        public int Length { get; private set; }
        public INCursesChar this[int index] => new NCursesSCHAR<TSmall>(ref this.schar[index]);

        public NCursesSCHARStr(string str)
        {
            this.Length = str.Length;
            this.CreateSCHARArray(str.AsSpan());
        }

        public NCursesSCHARStr(string str, ulong attrs)
        {
            this.Length = str.Length;
            this.CreateSCHARArray(str.AsSpan(), attrs);
        }

        public NCursesSCHARStr(string str, ulong attrs, short pair)
        {
            this.Length = str.Length;
            this.CreateSCHARArray(str.AsSpan(), attrs, pair);
        }

        //public NCursesSCHARStr(char[] str)
        //{
        //    this.CheckSCHARArr(str);
        //}

        //public NCursesSCHARStr(char[] str, ulong attrs)
        //{
        //    this.CheckSCHARArr(str);
        //}

        //public NCursesSCHARStr(byte[] str)
        //{
        //    this.CreateSCHARArr(str);
        //}

        //public NCursesSCHARStr(byte[] str, ulong attrs)
        //{
        //    this.CreateSCHARArr(str, attrs);
        //}

        public NCursesSCHARStr(int length)
        {
            this.Length = length;
            this.schar = ArrayPool<TSmall>.Shared.Rent(length);
            this.SCHAR = new Memory<TSmall>(this.schar);
        }

        ~NCursesSCHARStr()
        {
            this.Dispose();
        }

        private unsafe void CreateSCHARArray(ReadOnlySpan<char> charArray, ulong attrs = 0, short colorPair = 0, bool addNullTerminator = true)
        {
            this.schar = ArrayPool<TSmall>.Shared.Rent(charArray.Length + (addNullTerminator ? 1 : 0));
            this.SCHAR = new Memory<TSmall>(this.schar);

            fixed (char* originalChars = charArray)
            {
                int byteCount = NativeNCurses.Encoding.GetEncoder().GetByteCount(originalChars, charArray.Length, false);
                byte* bytePtr = stackalloc byte[byteCount];
                int bytesUsed = 0, charsUsed = 0, charPosition = 0, bytePosition = 0;
                bool completed = false;

                for (int i = 0; i < charArray.Length; i++)
                {
                    charPosition += charsUsed;
                    bytePosition += bytesUsed;
                    Encoding.ASCII.GetEncoder().Convert(
                        originalChars + charPosition, 1,
                        bytePtr + bytePosition, byteCount - bytePosition,
                        i == charArray.Length - 1 ? true : false,
                        out charsUsed, out bytesUsed, out completed);

                    if (!completed)
                        throw new InvalidOperationException("Could not complete encoding string");

                    this.schar[i] = this.CreateSmallChar((sbyte)bytePtr[i], attrs, colorPair);
                }
            }

            if (addNullTerminator)
                this.schar[this.schar.Length - 1] = NCursesSCHAR<TSmall>.byteCreate(0);
        }

        private TSmall CreateSmallChar(sbyte encodedByte, ulong attrs = 0, short colorPair = 0)
        {
            if (attrs == 0 && colorPair == 0)
                return NCursesSCHAR<TSmall>.byteCreate(encodedByte);
            else if (colorPair == 0)
                return NCursesSCHAR<TSmall>.byteAttrCreate(encodedByte, attrs);
            else
                return NCursesSCHAR<TSmall>.byteAttrColorCreate(encodedByte, attrs, colorPair);
        }

        public IEnumerator<INCursesSCHAR> GetEnumerator()
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
            return ++this.position < this.SCHAR.Length;
        }

        public void Reset()
        {
            this.position = 0;
        }

        //TODO: crashes Visual Studio debugger
        public static explicit operator string(NCursesSCHARStr<TSmall> str)
        {
            unsafe
            {
                char* charArr = stackalloc char[str.Length];
                for (int i = 0; i < str.Length; i++)
                    charArr[i] = str.schar[i].Char;
                ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(charArr, str.Length);
                return charSpan.ToString();
            }
        }

        public override string ToString()
        {
            return (string)this;
        }

        public void Dispose()
        {
            this.position = 0;
            ArrayPool<TSmall>.Shared.Return(this.schar, true);
            GC.SuppressFinalize(this);
        }
    }
}
