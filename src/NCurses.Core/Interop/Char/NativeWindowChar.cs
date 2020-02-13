using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.Char
{
    internal class NativeWindowChar<TChar>
            : CharWrapper<TChar>,
            INativeWindowChar<TChar, CharString<TChar>>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
    {
        internal NativeWindowChar(ICharWrapper<TChar> wrapper)
            : base(wrapper) { }

        public void mvwaddnstr(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> str, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwaddnstr(window, y, x, in str.GetPinnableReference(), n), "mvwaddnstr");
        }

        public void mvwaddstr(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.mvwaddstr(window, y, x, in str.GetPinnableReference()), "mvwaddstr");
        }

        public void mvwgetnstr(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> str, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwgetnstr(window, y, x, ref str.GetPinnableReference(), n), "mvwgetnstr");
        }

        public void mvwgetstr(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.mvwgetstr(window, y, x, ref str.GetPinnableReference()), "mvwgetstr");
        }

        public void mvwinnstr(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> str, int n, out int read)
        {
            read = this.Wrapper.mvwinnstr(window, y, x, ref str.GetPinnableReference(), n);
            NCursesException.Verify(read, "mvwinnstr");
        }

        public void mvwinsnstr(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> str, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwinsnstr(window, y, x, in str.GetPinnableReference(), n), "mvwinsnstr");
        }

        public void mvwinsstr(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.mvwinsstr(window, y, x, in str.GetPinnableReference()), "mvwinsstr");
        }

        public void mvwinstr(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> str, out int read)
        {
            read = this.Wrapper.mvwinstr(window, y, x, ref str.GetPinnableReference());
            NCursesException.Verify(read, "mvwinstr");
        }

        public void mvwprintw(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> format, params CharString<TChar>[] argList)
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

                    NCursesException.Verify(this.Wrapper.mvwprintw(window, y, x, in format.GetPinnableReference(), argPtr), "mvwprintw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void mvwscanw(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> format, params CharString<TChar>[] argList)
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

                    NCursesException.Verify(this.Wrapper.mvwscanw(window, y, x, ref format.GetPinnableReference(), argPtr), "mvwscanw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void waddnstr(WindowBaseSafeHandle window, in CharString<TChar> str, int number)
        {
            NCursesException.Verify(this.Wrapper.waddnstr(window, in str.GetPinnableReference(), number), "waddnstr");
        }

        public void waddstr(WindowBaseSafeHandle window, in CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.waddstr(window, in str.GetPinnableReference()), "waddstr");
        }

        public void wgetnstr(WindowBaseSafeHandle window, ref CharString<TChar> str, int count)
        {
            NCursesException.Verify(this.Wrapper.wgetnstr(window, ref str.GetPinnableReference(), count), "wgetnstr");
        }

        public void wgetstr(WindowBaseSafeHandle window, ref CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.wgetstr(window, ref str.GetPinnableReference()), "wgetstr");
        }

        public void winnstr(WindowBaseSafeHandle window, ref CharString<TChar> str, int count, out int read)
        {
            read = this.Wrapper.winnstr(window, ref str.GetPinnableReference(), count);
            NCursesException.Verify(read, "winnstr");
        }

        public void winstr(WindowBaseSafeHandle window, ref CharString<TChar> str, out int read)
        {
            read = this.Wrapper.winstr(window, ref str.GetPinnableReference());
            NCursesException.Verify(read, "winstr");
        }

        public void winsnstr(WindowBaseSafeHandle window, in CharString<TChar> str, int n)
        {
            NCursesException.Verify(this.Wrapper.winsnstr(window, in str.GetPinnableReference(), n), "winsnstr");
        }

        public void winsstr(WindowBaseSafeHandle window, in CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.winsstr(window, in str.GetPinnableReference()), "winsstr");
        }

        public void wprintw(WindowBaseSafeHandle window, in CharString<TChar> format, params CharString<TChar>[] argList)
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

                    NCursesException.Verify(this.Wrapper.wprintw(window, in format.GetPinnableReference(), argPtr), "wprintw");
                }
            }
            finally
            {
                if (argPtr != IntPtr.Zero)
                    Marshal.FreeHGlobal(argPtr);
            }
        }

        public void wscanw(WindowBaseSafeHandle window, ref CharString<TChar> str, params CharString<TChar>[] argList)
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

                    NCursesException.Verify(this.Wrapper.wscanw(window, ref str.GetPinnableReference(), argPtr), "wscanw");
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
