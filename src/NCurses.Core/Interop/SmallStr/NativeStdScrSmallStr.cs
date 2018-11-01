using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.SmallStr
{
    public interface INativeStdScrSmallStr
    {
        void addnstr(string txt, int number);
        void addstr(string txt);
        void getnstr(out string str, int count);
        void getstr(out string txt);
        void innstr(out string str, int n, out int read);
        void insnstr(in string str, int n);
        void insstr(in string str);
        void instr(out string str, out int read);
        void mvaddnstr(int y, int x, in string txt, int n);
        void mvaddstr(int y, int x, in string txt);
        void mvgetnstr(int y, int x, out string str, int count);
        void mvgetstr(int y, int x, out string str);
        void mvinnstr(int y, int x, out string str, int n, out int read);
        void mvinsnstr(int y, int x, in string str, int n);
        void mvinsstr(int y, int x, in string str);
        void mvinstr(int y, int x, out string str, out int read);
        void mvprintw(int y, int x, in string str, params string[] argList);
        void mvscanw(int y, int x, out string str, params string[] argList);
        void printw(in string format, params string[] argList);
        void scanw(out string str, params string[] argList);

    }

    public class NativeStdScrSmallStr<TSmallStr> : NativeSmallStrBase<TSmallStr>, INativeStdScrSmallStr
        where TSmallStr : unmanaged
    {
        public void addnstr(string str, int number)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.addnstr(MarshalStringReadonly(str, byteArray, byteLength), number), "addnstr");
            }
        }

        public void addstr(string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.addstr(MarshalStringReadonly(str, byteArray, byteLength)), "addstr");
            }
        }

        public void getnstr(out string str, int count)
        {
            unsafe
            {
                TSmallStr* strPtr = stackalloc TSmallStr[count];
                NCursesException.Verify(this.Wrapper.getnstr(ref MarshalString(strPtr, count, out Span<TSmallStr> span), count), "getnstr");
                str = ReadString(ref span);
            }
        }

        public void getstr(out string str)
        {
            unsafe
            {
                TSmallStr* strPtr = stackalloc TSmallStr[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.getstr(ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TSmallStr> span)), "getstr");
                str = ReadString(ref span);
            }
        }

        public void innstr(out string str, int count, out int read)
        {
            unsafe
            {
                TSmallStr* strPtr = stackalloc TSmallStr[count];
                read = this.Wrapper.innstr(ref MarshalString(strPtr, count, out Span<TSmallStr> span), count);
                NCursesException.Verify(read, "innstr");
                str = ReadString(ref span);
            }
        }

        public void insnstr(in string str, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.insnstr(MarshalStringReadonly(str, byteArray, byteLength), n), "insnstr");
            }
        }

        public void insstr(in string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.insstr(MarshalStringReadonly(str, byteArray, byteLength)), "insstr");
            }
        }

        public void instr(out string str, out int read)
        {
            unsafe
            {
                TSmallStr* strPtr = stackalloc TSmallStr[Constants.MAX_STRING_LENGTH];
                read = this.Wrapper.instr(ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TSmallStr> span));
                NCursesException.Verify(read, "instr");
                str = ReadString(ref span);
            }
        }

        public void mvaddnstr(int y, int x, in string str, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.mvaddnstr(y, x, MarshalStringReadonly(str, byteArray, byteLength), n), "mvaddnstr");
            }
        }

        public void mvaddstr(int y, int x, in string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.mvaddstr(y, x, MarshalStringReadonly(str, byteArray, byteLength)), "mvaddstr");
            }
        }

        public void mvgetnstr(int y, int x, out string str, int count)
        {
            unsafe
            {
                TSmallStr* strPtr = stackalloc TSmallStr[count];
                NCursesException.Verify(this.Wrapper.mvgetnstr(y, x, ref MarshalString(strPtr, count, out Span<TSmallStr> span), count), "mvgetnstr");
                str = ReadString(ref span);
            }
        }

        public void mvgetstr(int y, int x, out string str)
        {
            unsafe
            {
                TSmallStr* strPtr = stackalloc TSmallStr[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.mvgetstr(y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TSmallStr> span)), "mvgetstr");
                str = ReadString(ref span);
            }
        }

        public void mvinnstr(int y, int x, out string str, int n, out int read)
        {
            unsafe
            {
                TSmallStr* strPtr = stackalloc TSmallStr[n];
                read = this.Wrapper.mvinnstr(y, x, ref MarshalString(strPtr, n, out Span<TSmallStr> span), n);
                NCursesException.Verify(read, "mvinnstr");
                str = ReadString(ref span);
            }
        }

        public void mvinsnstr(int y, int x, in string str, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.mvinsnstr(y, x, MarshalStringReadonly(str, byteArray, byteLength), n), "mvinsnstr");
            }
        }

        public void mvinsstr(int y, int x, in string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.mvinsstr(y, x, MarshalStringReadonly(str, byteArray, byteLength)), "mvinsstr");
            }
        }

        public void mvinstr(int y, int x, out string str, out int read)
        {
            unsafe
            {
                TSmallStr* strPtr = stackalloc TSmallStr[Constants.MAX_STRING_LENGTH];
                read = this.Wrapper.mvinstr(y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TSmallStr> span));
                NCursesException.Verify(read, "mvinstr");
                str = ReadString(ref span);
            }
        }

        public void mvprintw(int y, int x, in string str, params string[] argList)
        {

            IntPtr argPtr = IntPtr.Zero;
            try
            {
                unsafe
                {
                    byte** ptrArr = stackalloc byte*[argList.Length];
                    for(int i = 0; i < argList.Length; i++)
                    {
                        byte* arr = stackalloc byte[GetNullTerminatedStringLength(argList[i])];
                        ptrArr[i] = arr;
                    }
                    argPtr = CreateVarArgList(argList, ptrArr);

                    int byteLength;
                    byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                    NCursesException.Verify(this.Wrapper.mvprintw(y, x, MarshalStringReadonly(str, byteArray, byteLength), argPtr), "mvprintw");
                }
            }
            finally
            {
                if(argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void mvscanw(int y, int x, out string str, params string[] argList)
        {
            Span<TSmallStr> span;
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

                    TSmallStr* strPtr = stackalloc TSmallStr[Constants.MAX_STRING_LENGTH];
                    NCursesException.Verify(this.Wrapper.mvscanw(y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out span), argPtr), "mvscanw");
                    str = ReadString(ref span);
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void printw(in string format, params string[] argList)
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
                    NCursesException.Verify(this.Wrapper.printw(MarshalStringReadonly(format, byteArray, byteLength), argPtr), "printw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void scanw(out string str, params string[] argList)
        {
            //TODO: can overflow
            Span<TSmallStr> span;
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

                    TSmallStr* strPtr = stackalloc TSmallStr[Constants.MAX_STRING_LENGTH];
                    NCursesException.Verify(this.Wrapper.scanw(ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out span), argPtr), "scanw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
            str = ReadString(ref span);
        }
    }
}
