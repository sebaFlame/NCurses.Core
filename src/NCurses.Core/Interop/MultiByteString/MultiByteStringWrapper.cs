using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop.MultiByteString
{
    internal class MultiByteStringWrapper<TMultiByteString, TSingleByteString> //handles wchar_t and wint_t
        where TMultiByteString : unmanaged
        where TSingleByteString : unmanaged
    {
        private IMultiByteStringWrapper<TMultiByteString, TSingleByteString> wrapper;
        internal IMultiByteStringWrapper<TMultiByteString, TSingleByteString> Wrapper => 
            wrapper ?? (wrapper = NativeNCurses.NCursesCustomTypeWrapper as IMultiByteStringWrapper<TMultiByteString, TSingleByteString>);

        #region readonly string
        //internal static ref readonly TWideStr MarshallStringReadonly(in string str)
        //{
        //    ReadOnlySpan<char> chSpan = str.AsSpan();
        //    ReadOnlySpan<TWideStr> span;

        //    unsafe
        //    {
        //        int byteLength;
        //        byte* byteArray = stackalloc byte[byteLength = (str.Length * Marshal.SizeOf<TWideStr>()) + Marshal.SizeOf<TWideStr>()];
        //        fixed (char* chars = chSpan)
        //        {
        //            NativeNCurses.Encoding.GetBytes(chars, chSpan.Length, byteArray, byteLength);
        //        }
        //        span = MemoryMarshal.Cast<byte, TWideStr>(new ReadOnlySpan<byte>(byteArray, byteLength));
        //    }

        //    return ref span.GetPinnableReference();
        //}

        internal static unsafe ref readonly TMultiByteString MarshalStringReadonly(in string str, byte* byteArray, int byteLength)
        {
            ReadOnlySpan<char> chSpan = str.AsSpan();
            ReadOnlySpan<TMultiByteString> span;
            fixed (char* chars = chSpan)
            {
                NativeNCurses.Encoding.GetBytes(chars, chSpan.Length, byteArray, byteLength);
            }
            span = MemoryMarshal.Cast<byte, TMultiByteString>(new ReadOnlySpan<byte>(byteArray, byteLength));
            return ref span.GetPinnableReference();
        }

        internal static int GetNullTerminatedStringLength(in string str)
        {
            return (str.Length * Marshal.SizeOf<TMultiByteString>()) + Marshal.SizeOf<TMultiByteString>();
        }
        #endregion

        #region readonly char (or by value)
        //internal unsafe static ref TWideStr MarshalChar(char ch)
        //{
        //    TWideStr* arr = stackalloc TWideStr[1];
        //    char* charArr = stackalloc char[1];
        //    charArr[0] = ch;

        //    Span<TWideStr> span = new Span<TWideStr>(arr, 1);
        //    Span<byte> chars = MemoryMarshal.AsBytes(span);

        //    fixed (byte* bytes = chars)
        //    {
        //        if (NativeNCurses.Encoding.GetBytes(charArr, 1, bytes, Marshal.SizeOf<TWideStr>()) != 1)
        //            throw new InvalidCastException("Can not convert character to correct encoding");
        //    }

        //    return ref span.GetPinnableReference();
        //}

        internal unsafe static ref readonly TMultiByteString MarshalChar(in char ch, byte* byteArray, int byteLength)
        {
            ReadOnlySpan<TMultiByteString> span;

            char* charArr = stackalloc char[1];
            charArr[0] = ch;

            if (NativeNCurses.Encoding.GetBytes(charArr, 1, byteArray, byteLength) == 0)
                throw new InvalidCastException("Can not convert character to correct encoding");

            span = MemoryMarshal.Cast<byte, TMultiByteString>(new ReadOnlySpan<byte>(byteArray, byteLength));
            return ref span.GetPinnableReference();
        }

        internal static int GetCharLength()
        {
            return Marshal.SizeOf<TMultiByteString>();
        }
        #endregion

        #region writable string (or char if length == 1)
        //internal static ref TWideStr MarshalString(int length, out Span<TWideStr> span)
        //{
        //    unsafe
        //    {
        //        TWideStr* arr = stackalloc TWideStr[length];
        //        span = new Span<TWideStr>(arr, length);
        //    }

        //    return ref span.GetPinnableReference();
        //}

        internal static unsafe ref TMultiByteString MarshalString(TMultiByteString* strPtr, int length, out Span<TMultiByteString> span)
        {
            span = new Span<TMultiByteString>(strPtr, length);
            return ref span.GetPinnableReference();
        }
        #endregion

        //2 byte char strings
        //TODO: use Encoding Span overrides
        internal unsafe static string ReadString(ref Span<TMultiByteString> str)
        {
            Span<byte> span = MemoryMarshal.AsBytes(str);
            fixed (byte* chars = span)
            {
                return NativeNCurses.Encoding.GetString(chars, span.Length);
            }
        }

        public unsafe static string ReadString(ref TMultiByteString strRef)
        {
            TMultiByteString* arr = (TMultiByteString*)Unsafe.AsPointer<TMultiByteString>(ref strRef);
            Span<TMultiByteString> strSpan = new Span<TMultiByteString>(arr, FindStringLength(arr));
            return ReadString(ref strSpan);
        }

        internal unsafe static int FindStringLength(TMultiByteString* smallStrArr)
        {
            TMultiByteString val;
            int length = 0;
            while (true)
            {
                val = *(smallStrArr + (length++ * Marshal.SizeOf<TMultiByteString>()));
                if (EqualityComparer<TMultiByteString>.Default.Equals(val, default(TMultiByteString)))
                    break;
            }
            return --length;
        }

        internal unsafe static char ReadChar(Span<TMultiByteString> span)
        {
            char* charArr = stackalloc char[1];
            Span<byte> chars = MemoryMarshal.AsBytes(span);

            fixed (byte* bytes = chars)
            {
                if (NativeNCurses.Encoding.GetChars(bytes, Marshal.SizeOf<TMultiByteString>(), charArr, 1) != 1)
                    throw new InvalidCastException("Can not convert character to correct encoding");
            }

            return charArr[0];
        }

        internal unsafe static char ReadChar(TMultiByteString str)
        {
            TMultiByteString* arr = stackalloc TMultiByteString[1];
            arr[0] = str;

            Span<TMultiByteString> span = new Span<TMultiByteString>(arr, 1);
            return ReadChar(span);
        }

        internal static bool VerifyInput(string method, int ret, in Span<TMultiByteString> span, out char wch, out Key key)
        {
            if (ret == (int)Key.CODE_YES)
            {
                Span<short> spShort = MemoryMarshal.Cast<TMultiByteString, short>(span);
                wch = '\0';
                key = (Key)spShort[0];
                return true;
            }

            //TODO: can still be an escaped function key? (iterate over all function keys????)
            NCursesException.Verify(ret, method);
            key = default(Key);
            wch = ReadChar(span);
            return false;
        }
    }
}
