using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop.SingleByteString
{
    public class NativeSmallStrBase<TSmallStr> //handles char
        where TSmallStr : unmanaged
    {
        private INCursesWrapperSmallStr<TSmallStr> wrapper;
        internal INCursesWrapperSmallStr<TSmallStr> Wrapper => 
            wrapper ?? (wrapper = NativeNCurses.NCursesCharTypeWrapper as INCursesWrapperSmallStr<TSmallStr>);

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

        internal static unsafe ref readonly TSmallStr MarshalStringReadonly(in string str, byte* byteArray, int byteLength)
        {
            ReadOnlySpan<char> chSpan = str.AsSpan();
            ReadOnlySpan<TSmallStr> span;
            fixed (char* chars = chSpan)
            {
                Encoding.ASCII.GetBytes(chars, chSpan.Length, byteArray, byteLength);
            }
            span = MemoryMarshal.Cast<byte, TSmallStr>(new ReadOnlySpan<byte>(byteArray, byteLength));
            return ref span.GetPinnableReference();
        }

        internal static int GetNullTerminatedStringLength(in string str)
        {
            return (str.Length * Marshal.SizeOf<TSmallStr>()) + Marshal.SizeOf<TSmallStr>();
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

        internal unsafe static ref readonly TSmallStr MarshalChar(in char ch, byte* byteArray, int byteLength)
        {
            ReadOnlySpan<TSmallStr> span;

            char* charArr = stackalloc char[1];
            charArr[0] = ch;

            if (Encoding.ASCII.GetBytes(charArr, 1, byteArray, byteLength) == 0)
                throw new InvalidCastException("Can not convert character to correct encoding");

            span = MemoryMarshal.Cast<byte, TSmallStr>(new ReadOnlySpan<byte>(byteArray, byteLength));
            return ref span.GetPinnableReference();
        }

        internal static int GetCharLength()
        {
            return Marshal.SizeOf<TSmallStr>();
        }
        #endregion

        #region non-readonly string (unsafe to pointer)
        internal unsafe static ref TSmallStr MarshalString(in string str, byte* byteArray, int byteLength)
        {
            ReadOnlySpan<char> chSpan = str.AsSpan();
            Span<TSmallStr> span;
            fixed (char* chars = chSpan)
            {
                Encoding.ASCII.GetBytes(chars, chSpan.Length, byteArray, byteLength);
            }
            span = MemoryMarshal.Cast<byte, TSmallStr>(new Span<byte>(byteArray, byteLength));
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

        internal static unsafe ref TSmallStr MarshalString(TSmallStr* strPtr, int length, out Span<TSmallStr> span)
        {
            span = new Span<TSmallStr>(strPtr, length);
            return ref span.GetPinnableReference();
        }
        #endregion

        //1 byte char strings
        //TODO: use Encoding Span overrides
        internal unsafe static string ReadString(ref Span<TSmallStr> str)
        {
            Span<byte> span = MemoryMarshal.AsBytes(str);
            fixed (byte* chars = span)
            {
                return Encoding.ASCII.GetString(chars, str.Length);
            }
        }

        internal unsafe static char ReadChar(TSmallStr str)
        {
            TSmallStr* arr = stackalloc TSmallStr[1];
            char* charArr = stackalloc char[1];
            arr[0] = str;

            Span<TSmallStr> span = new Span<TSmallStr>(arr, 1);
            Span<byte> chars = MemoryMarshal.AsBytes(span);

            fixed (byte* bytes = chars)
            {
                if (Encoding.ASCII.GetChars(bytes, Marshal.SizeOf<TSmallStr>(), charArr, 1) != 1)
                    throw new InvalidCastException("Can not convert character to correct encoding");
            }

            return charArr[0];
        }

        public unsafe static string ReadString(ref TSmallStr strRef)
        {
            TSmallStr* arr = (TSmallStr*)Unsafe.AsPointer<TSmallStr>(ref strRef);
            Span<TSmallStr> strSpan = new Span<TSmallStr>(arr, FindStringLength(arr));
            return ReadString(ref strSpan);
        }

        internal unsafe static int FindStringLength(TSmallStr* smallStrArr)
        {
            TSmallStr val;
            int length = 0;
            while (true)
            {
                val = *(smallStrArr + (length++ * Marshal.SizeOf<TSmallStr>()));
                if (EqualityComparer<TSmallStr>.Default.Equals(val, default(TSmallStr)))
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
                        new IntPtr(Unsafe.AsPointer<TSmallStr>(ref MarshalString
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
