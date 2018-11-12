using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NCurses.Core.Interop.SingleByteString
{
    public interface INativeWindowSingleByteString
    {
        void mvwaddnstr(IntPtr window, int y, int x, in string str, int n);
        void mvwaddstr(IntPtr window, int y, int x, in string str);
        void mvwgetnstr(IntPtr window, int y, int x, out string str, int n);
        void mvwgetstr(IntPtr window, int y, int x, out string str);
        void mvwinnstr(IntPtr window, int y, int x, out string str, int n, out int read);
        void mvwinsnstr(IntPtr window, int y, int x, in string str, int n);
        void mvwinsstr(IntPtr window, int y, int x, in string str);
        void mvwinstr(IntPtr window, int y, int x, out string str, out int read);
        void mvwprintw(IntPtr window, int y, int x, in string format, params string[] argList);
        void mvwscanw(IntPtr window, int y, int x, out string format, params string[] argList);
        void waddnstr(IntPtr window, in string str, int number);
        void waddstr(IntPtr window, in string str);
        void wgetnstr(IntPtr window, out string str, int count);
        void wgetstr(IntPtr window, out string str);
        void winnstr(IntPtr window, out string str, int count, out int read);
        void winstr(IntPtr window, out string str, out int read);
        void winsnstr(IntPtr window, in string str, int n);
        void winsstr(IntPtr window, in string str);
        void wprintw(IntPtr window, in string format, params string[] argList);
        void wscanw(IntPtr window, out string str, params string[] argList);
    }

    internal class NativeWindowSingleByteString<TSingleByteString> : SingleByteStringWrapper<TSingleByteString>, INativeWindowSingleByteString
        where TSingleByteString : unmanaged
    {
        public void mvwaddnstr(IntPtr window, int y, int x, in string str, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.mvwaddnstr(window, y, x, MarshalStringReadonly(str, byteArray, byteLength), n), "mvwaddnstr");
            }
        }

        public void mvwaddstr(IntPtr window, int y, int x, in string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.mvwaddstr(window, y, x, MarshalStringReadonly(str, byteArray, byteLength)), "mvwaddstr");
            }
        }

        public void mvwgetnstr(IntPtr window, int y, int x, out string str, int n)
        {
            unsafe
            {
                TSingleByteString* strPtr = stackalloc TSingleByteString[n];
                NCursesException.Verify(this.Wrapper.mvwgetnstr(window, y, x, ref MarshalString(strPtr, n, out Span<TSingleByteString> span), n), "mvwgetnstr");
                str = ReadString(ref span);
            }
        }

        public void mvwgetstr(IntPtr window, int y, int x, out string str)
        {
            unsafe
            {
                TSingleByteString* strPtr = stackalloc TSingleByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.mvwgetstr(window, y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TSingleByteString> span)), "mvwgetstr");
                str = ReadString(ref span);
            }
        }

        public void mvwinnstr(IntPtr window, int y, int x, out string str, int n, out int read)
        {
            unsafe
            {
                TSingleByteString* strPtr = stackalloc TSingleByteString[n];
                read = this.Wrapper.mvwinnstr(window, y, x, ref MarshalString(strPtr, n, out Span<TSingleByteString> span), n);
                NCursesException.Verify(read, "mvwinnstr");
                str = ReadString(ref span);
            }
        }

        public void mvwinsnstr(IntPtr window, int y, int x, in string str, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.mvwinsnstr(window, y, x, MarshalStringReadonly(str, byteArray, byteLength), n), "mvwinsnstr");
            }
        }

        public void mvwinsstr(IntPtr window, int y, int x, in string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.mvwinsstr(window, y, x, MarshalStringReadonly(str, byteArray, byteLength)), "mvwinsstr");
            }
        }

        public void mvwinstr(IntPtr window, int y, int x, out string str, out int read)
        {
            unsafe
            {
                TSingleByteString* strPtr = stackalloc TSingleByteString[Constants.MAX_STRING_LENGTH];
                read = this.Wrapper.mvwinstr(window, y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TSingleByteString> span));
                NCursesException.Verify(read, "mvwinstr");
                str = ReadString(ref span);
            }
        }

        public void mvwprintw(IntPtr window, int y, int x, in string format, params string[] argList)
        {
            IntPtr argPtr = IntPtr.Zero;
            try
            {
                unsafe
                {
                    byte** ptrArr = stackalloc byte*[argList.Length];
                    for (int i = 0; i < argList.Length; i++)
                    {
                        byte* arr = stackalloc byte[GetNullTerminatedStringLength(argList[i])];
                        ptrArr[i] = arr;
                    }
                    argPtr = CreateVarArgList(argList, ptrArr);

                    int byteLength;
                    byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(format)];
                    NCursesException.Verify(this.Wrapper.mvwprintw(window, y, x, MarshalStringReadonly(format, byteArray, byteLength), argPtr), "mvwprintw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void mvwscanw(IntPtr window, int y, int x, out string format, params string[] argList)
        {
            Span<TSingleByteString> span;
            IntPtr argPtr = IntPtr.Zero;
            try
            {
                unsafe
                {
                    byte** ptrArr = stackalloc byte*[argList.Length];
                    for (int i = 0; i < argList.Length; i++)
                    {
                        byte* arr = stackalloc byte[GetNullTerminatedStringLength(argList[i])];
                        ptrArr[i] = arr;
                    }
                    argPtr = CreateVarArgList(argList, ptrArr);

                    TSingleByteString* strPtr = stackalloc TSingleByteString[Constants.MAX_STRING_LENGTH];
                    NCursesException.Verify(this.Wrapper.mvwscanw(window, y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out span), argPtr), "mvwscanw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
            format = ReadString(ref span);
        }

        public void waddnstr(IntPtr window, in string str, int number)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.waddnstr(window, MarshalStringReadonly(str, byteArray, byteLength), number), "waddnstr");
            }
        }

        public void waddstr(IntPtr window, in string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.waddstr(window, MarshalStringReadonly(str, byteArray, byteLength)), "waddstr");
            }
        }

        public void wgetnstr(IntPtr window, out string str, int count)
        {
            unsafe
            {
                TSingleByteString* strPtr = stackalloc TSingleByteString[count];
                NCursesException.Verify(this.Wrapper.wgetnstr(window, ref MarshalString(strPtr, count, out Span<TSingleByteString> span), count), "wgetnstr");
                str = ReadString(ref span);
            }
        }

        public void wgetstr(IntPtr window, out string str)
        {
            unsafe
            {
                TSingleByteString* strPtr = stackalloc TSingleByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.wgetstr(window, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TSingleByteString> span)), "wgetstr");
                str = ReadString(ref span);
            }
        }

        public void winnstr(IntPtr window, out string str, int count, out int read)
        {
            unsafe
            {
                TSingleByteString* strPtr = stackalloc TSingleByteString[count];
                read = this.Wrapper.winnstr(window, ref MarshalString(strPtr, count, out Span<TSingleByteString> span), count);
                NCursesException.Verify(read, "winnstr");
                str = ReadString(ref span);
            }
        }

        public void winstr(IntPtr window, out string str, out int read)
        {
            unsafe
            {
                TSingleByteString* strPtr = stackalloc TSingleByteString[Constants.MAX_STRING_LENGTH];
                read = this.Wrapper.winstr(window, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TSingleByteString> span));
                NCursesException.Verify(read, "winstr");
                str = ReadString(ref span);
            }
        }

        public void winsnstr(IntPtr window, in string str, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.winsnstr(window, MarshalStringReadonly(str, byteArray, byteLength), n), "winsnstr");
            }
        }

        public void winsstr(IntPtr window, in string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.winsstr(window, MarshalStringReadonly(str, byteArray, byteLength)), "winsstr");
            }
        }

        public void wprintw(IntPtr window, in string format, params string[] argList)
        {
            IntPtr argPtr = IntPtr.Zero;
            try
            {
                unsafe
                {
                    byte** ptrArr = stackalloc byte*[argList.Length];
                    for (int i = 0; i < argList.Length; i++)
                    {
                        byte* arr = stackalloc byte[GetNullTerminatedStringLength(argList[i])];
                        ptrArr[i] = arr;
                    }
                    argPtr = CreateVarArgList(argList, ptrArr);

                    int byteLength;
                    byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(format)];
                    NCursesException.Verify(this.Wrapper.wprintw(window, MarshalStringReadonly(format, byteArray, byteLength), argPtr), "wprintw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void wscanw(IntPtr window, out string str, params string[] argList)
        {
            //TODO: can overflow
            Span<TSingleByteString> span;
            IntPtr argPtr = IntPtr.Zero;
            try
            {
                unsafe
                {
                    byte** ptrArr = stackalloc byte*[argList.Length];
                    for (int i = 0; i < argList.Length; i++)
                    {
                        byte* arr = stackalloc byte[GetNullTerminatedStringLength(argList[i])];
                        ptrArr[i] = arr;
                    }
                    argPtr = CreateVarArgList(argList, ptrArr);

                    TSingleByteString* strPtr = stackalloc TSingleByteString[Constants.MAX_STRING_LENGTH];
                    NCursesException.Verify(this.Wrapper.wscanw(window, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out span), argPtr), "wscanw");
                    str = ReadString(ref span);
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }
    }
}
