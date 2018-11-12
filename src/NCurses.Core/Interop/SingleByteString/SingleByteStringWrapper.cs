using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop.SingleByteString
{
    public class SingleByteStringWrapper<TSingleByteString> //handles char
        where TSingleByteString : unmanaged
    {
        private ISingleByteStringWrapper<TSingleByteString> wrapper;
        internal ISingleByteStringWrapper<TSingleByteString> Wrapper => 
            wrapper ?? (wrapper = NativeNCurses.NCursesCharTypeWrapper as ISingleByteStringWrapper<TSingleByteString>);

        #region readonly string
        //internal static ref readonly TSmallStr MarshalStringReadonly(in string str)
        //{
        //    ReadOnlySpan<char> chSpan = str.AsSpan();
        //    ReadOnlySpan<TSmallStr> span;

        //    unsafe
        //    {
        //        int byteLength;
        //        byte* byteArray = stackalloc byte[byteLength = str.Length + Marshal.SizeOf<TSmallStr>()];
        //        fixed (char* chars = chSpan)
        //        {
        //            Encoding.ASCII.GetBytes(chars, chSpan.Length, byteArray, byteLength); //TODO: ASCII not completely correct
        //        }
        //        span = MemoryMarshal.Cast<byte, TSmallStr>(new ReadOnlySpan<byte>(byteArray, byteLength));
        //    }

        //    return ref span.GetPinnableReference();
        //}

        internal static unsafe ref readonly TSingleByteString MarshalStringReadonly(in string str, byte* byteArray, int byteLength)
        {
            ReadOnlySpan<char> chSpan = str.AsSpan();
            ReadOnlySpan<TSingleByteString> span;
            fixed (char* chars = chSpan)
            {
                Encoding.ASCII.GetBytes(chars, chSpan.Length, byteArray, byteLength);
            }
            span = MemoryMarshal.Cast<byte, TSingleByteString>(new ReadOnlySpan<byte>(byteArray, byteLength));
            return ref span.GetPinnableReference();
        }

        internal static int GetNullTerminatedStringLength(in string str)
        {
            return (str.Length * Marshal.SizeOf<TSingleByteString>()) + Marshal.SizeOf<TSingleByteString>();
        }
        #endregion

        #region readonly char (or by value)
        //internal unsafe static ref TSmallStr MarshalChar(char ch)
        //{
        //    TSmallStr* arr = stackalloc TSmallStr[1];
        //    char* charArr = stackalloc char[1];
        //    charArr[0] = ch;

        //    Span<TSmallStr> span = new Span<TSmallStr>(arr, 1);
        //    Span<byte> chars = MemoryMarshal.AsBytes(span);

        //    fixed (byte* bytes = chars)
        //    {
        //        if (Encoding.ASCII.GetBytes(charArr, 1, bytes, Marshal.SizeOf<TSmallStr>()) != 1)
        //            throw new InvalidCastException("Can not convert character to correct encoding");
        //    }

        //    return ref span.GetPinnableReference();
        //}

        internal unsafe static ref readonly TSingleByteString MarshalChar(in char ch, byte* byteArray, int byteLength)
        {
            ReadOnlySpan<TSingleByteString> span;

            char* charArr = stackalloc char[1];
            charArr[0] = ch;

            if (Encoding.ASCII.GetBytes(charArr, 1, byteArray, byteLength) == 0)
                throw new InvalidCastException("Can not convert character to correct encoding");

            span = MemoryMarshal.Cast<byte, TSingleByteString>(new ReadOnlySpan<byte>(byteArray, byteLength));
            return ref span.GetPinnableReference();
        }

        internal static int GetCharLength()
        {
            return Marshal.SizeOf<TSingleByteString>();
        }
        #endregion

        #region non-readonly string (unsafe to pointer)
        internal unsafe static ref TSingleByteString MarshalString(in string str, byte* byteArray, int byteLength)
        {
            ReadOnlySpan<char> chSpan = str.AsSpan();
            Span<TSingleByteString> span;
            fixed (char* chars = chSpan)
            {
                Encoding.ASCII.GetBytes(chars, chSpan.Length, byteArray, byteLength);
            }
            span = MemoryMarshal.Cast<byte, TSingleByteString>(new Span<byte>(byteArray, byteLength));
            return ref span.GetPinnableReference();
        }
        #endregion

        #region writable string (or char if length == 1)
        //internal static ref TSmallStr MarshalString(int length, out Span<TSmallStr> span)
        //{
        //    unsafe
        //    {
        //        TSmallStr* arr = stackalloc TSmallStr[length];
        //        span = new Span<TSmallStr>(arr, length);
        //    }

        //    return ref span.GetPinnableReference();
        //}

        internal static unsafe ref TSingleByteString MarshalString(TSingleByteString* strPtr, int length, out Span<TSingleByteString> span)
        {
            span = new Span<TSingleByteString>(strPtr, length);
            return ref span.GetPinnableReference();
        }
        #endregion

        //1 byte char strings
        //TODO: use Encoding Span overrides
        internal unsafe static string ReadString(ref Span<TSingleByteString> str)
        {
            Span<byte> span = MemoryMarshal.AsBytes(str);
            fixed (byte* chars = span)
            {
                return Encoding.ASCII.GetString(chars, str.Length);
            }
        }

        internal unsafe static char ReadChar(TSingleByteString str)
        {
            TSingleByteString* arr = stackalloc TSingleByteString[1];
            char* charArr = stackalloc char[1];
            arr[0] = str;

            Span<TSingleByteString> span = new Span<TSingleByteString>(arr, 1);
            Span<byte> chars = MemoryMarshal.AsBytes(span);

            fixed (byte* bytes = chars)
            {
                if (Encoding.ASCII.GetChars(bytes, Marshal.SizeOf<TSingleByteString>(), charArr, 1) != 1)
                    throw new InvalidCastException("Can not convert character to correct encoding");
            }

            return charArr[0];
        }

        public unsafe static string ReadString(ref TSingleByteString strRef)
        {
            TSingleByteString* arr = (TSingleByteString*)Unsafe.AsPointer<TSingleByteString>(ref strRef);
            Span<TSingleByteString> strSpan = new Span<TSingleByteString>(arr, FindStringLength(arr));
            return ReadString(ref strSpan);
        }

        internal unsafe static int FindStringLength(TSingleByteString* smallStrArr)
        {
            TSingleByteString val;
            int length = 0;
            while (true)
            {
                val = *(smallStrArr + (length++ * Marshal.SizeOf<TSingleByteString>()));
                if (EqualityComparer<TSingleByteString>.Default.Equals(val, default(TSingleByteString)))
                    break;
            }
            return --length;
        }

        //TODO: verify OR replace with __arglist
        /// <summary>
        /// Free the returned pointer after usage
        /// </summary>
        /// <param name="argList"></param>
        /// <returns></returns>
        internal unsafe IntPtr CreateVarArgList(string[]strArg, byte** argList)
        {
            IntPtr ptr = Marshal.AllocHGlobal(strArg.Length * Marshal.SizeOf<IntPtr>());
            try
            {
                for (int i = 0; i < strArg.Length; i++)
                    Marshal.WriteIntPtr(
                        ptr,
                        i * Marshal.SizeOf<IntPtr>(),
                        new IntPtr(Unsafe.AsPointer<TSingleByteString>(ref MarshalString
                                                                    (
                                                                        strArg[i], 
                                                                        argList[i], 
                                                                        GetNullTerminatedStringLength(strArg[i]
                                                                    )
                                                                    ))));
            }
            catch
            {
                Marshal.FreeHGlobal(ptr);
                throw;
            }

            return ptr;
        }
    }
}
