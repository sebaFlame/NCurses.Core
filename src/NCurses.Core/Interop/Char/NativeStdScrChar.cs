using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.Char
{
    internal class NativeStdScrChar<TChar>
            : CharWrapper<TChar>,
            INativeStdScrChar<TChar, CharString<TChar>>
        where TChar : unmanaged, IChar, IEquatable<TChar>
    {
        internal NativeStdScrChar(ICharWrapper<TChar> wrapper)
            : base(wrapper) { }

        public void addnstr(in CharString<TChar> str, int number)
        {
            NCursesException.Verify(this.Wrapper.addnstr(in str.GetPinnableReference(), number), "addnstr");
        }

        public void addstr(in CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.addstr(in str.GetPinnableReference()), "addstr");
        }

        public void getnstr(ref CharString<TChar> str, int count)
        {
            NCursesException.Verify(this.Wrapper.getnstr(ref str.GetPinnableReference(), count), "getnstr");
        }

        public void getstr(ref CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.getstr(ref str.GetPinnableReference()), "getstr");
        }

        public void innstr(ref CharString<TChar> str, int count, out int read)
        {
            read = this.Wrapper.innstr(ref str.GetPinnableReference(), count);
            NCursesException.Verify(read, "innstr");
        }

        public void insnstr(in CharString<TChar> str, int n)
        {
            NCursesException.Verify(this.Wrapper.insnstr(in str.GetPinnableReference(), n), "insnstr");
        }

        public void insstr(in CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.insstr(in str.GetPinnableReference()), "insstr");
        }

        public void instr(ref CharString<TChar> str, out int read)
        {
            read = this.Wrapper.instr(ref str.GetPinnableReference());
            NCursesException.Verify(read, "instr");
        }

        public void mvaddnstr(int y, int x, in CharString<TChar> str, int n)
        {
            NCursesException.Verify(this.Wrapper.mvaddnstr(y, x, in str.GetPinnableReference(), n), "mvaddnstr");
        }

        public void mvaddstr(int y, int x, in CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.mvaddstr(y, x, in str.GetPinnableReference()), "mvaddstr");
        }

        public void mvgetnstr(int y, int x, ref CharString<TChar> str, int count)
        {
            NCursesException.Verify(this.Wrapper.mvgetnstr(y, x, ref str.GetPinnableReference(), count), "mvgetnstr");
        }

        public void mvgetstr(int y, int x, ref CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.mvgetstr(y, x, ref str.GetPinnableReference()), "mvgetstr");
        }

        public void mvinnstr(int y, int x, ref CharString<TChar> str, int n, out int read)
        {
            read = this.Wrapper.mvinnstr(y, x, ref str.GetPinnableReference(), n);
            NCursesException.Verify(read, "mvinnstr");
        }

        public void mvinsnstr(int y, int x, in CharString<TChar> str, int n)
        {
            NCursesException.Verify(this.Wrapper.mvinsnstr(y, x, in str.GetPinnableReference(), n), "mvinsnstr");
        }

        public void mvinsstr(int y, int x, in CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.mvinsstr(y, x, in str.GetPinnableReference()), "mvinsstr");
        }

        public void mvinstr(int y, int x, ref CharString<TChar> str, out int read)
        {
            read = this.Wrapper.mvinstr(y, x, ref str.GetPinnableReference());
            NCursesException.Verify(read, "mvinstr");
        }

        public void mvprintw(int y, int x, in CharString<TChar> str, params CharString<TChar>[] argList)
        {
            IntPtr argPtr = IntPtr.Zero;
            try
            {
                unsafe
                {
                    byte** ptrArr = stackalloc byte*[argList.Length];
                    for(int i = 0; i < argList.Length; i++)
                    {
                        byte* arr = stackalloc byte[argList[i].Length * CharFactoryInternal<TChar>.Instance.GetCharLength()];
                        ptrArr[i] = arr;
                    }
                    argPtr = CreateVarArgList(argList, ptrArr);

                    NCursesException.Verify(this.Wrapper.mvprintw(y, x, in str.GetPinnableReference(), argPtr), "mvprintw");
                }
            }
            finally
            {
                if(argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void mvscanw(int y, int x, ref CharString<TChar> str, params CharString<TChar>[] argList)
        {
            IntPtr argPtr = IntPtr.Zero;
            try
            {
                unsafe
                {
                    byte** ptrArr = stackalloc byte*[argList.Length];
                    for (int i = 0; i < argList.Length; i++)
                    {
                        byte* arr = stackalloc byte[argList[i].Length * CharFactoryInternal<TChar>.Instance.GetCharLength()];
                        ptrArr[i] = arr;
                    }
                    argPtr = CreateVarArgList(argList, ptrArr);

                    NCursesException.Verify(this.Wrapper.mvscanw(y, x, ref str.GetPinnableReference(), argPtr), "mvscanw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void printw(in CharString<TChar> format, params CharString<TChar>[] argList)
        {
            IntPtr argPtr = IntPtr.Zero;
            try
            {
                unsafe
                {
                    byte** ptrArr = stackalloc byte*[argList.Length];
                    for (int i = 0; i < argList.Length; i++)
                    {
                        byte* arr = stackalloc byte[argList[i].Length * CharFactoryInternal<TChar>.Instance.GetCharLength()];
                        ptrArr[i] = arr;
                    }
                    argPtr = CreateVarArgList(argList, ptrArr);

                    NCursesException.Verify(this.Wrapper.printw(format.GetPinnableReference(), argPtr), "printw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void scanw(ref CharString<TChar> str, params CharString<TChar>[] argList)
        {
            //TODO: can overflow
            IntPtr argPtr = IntPtr.Zero;
            try
            {
                unsafe
                {
                    byte** ptrArr = stackalloc byte*[argList.Length];
                    for (int i = 0; i < argList.Length; i++)
                    {
                        byte* arr = stackalloc byte[argList[i].Length * CharFactoryInternal<TChar>.Instance.GetCharLength()];
                        ptrArr[i] = arr;
                    }
                    argPtr = CreateVarArgList(argList, ptrArr);

                    NCursesException.Verify(this.Wrapper.scanw(ref str.GetPinnableReference(), argPtr), "scanw");
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
