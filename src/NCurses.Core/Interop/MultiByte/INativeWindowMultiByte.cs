using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativeWindowMultiByte<TChar, TCharString> 
        where TChar : IMultiByteNCursesChar
        where TCharString : IMultiByteNCursesCharString
    {
        void box_set(WindowBaseSafeHandle win, in TChar verch, in TChar horch);
        void mvwadd_wch(WindowBaseSafeHandle win, int y, int x, in TChar wch);
        void mvwadd_wchnstr(WindowBaseSafeHandle win, int y, int x, in TCharString wchStr, int n);
        void mvwadd_wchstr(WindowBaseSafeHandle win, int y, int x, in TCharString wchStr);
        void mvwhline_set(WindowBaseSafeHandle win, int y, int x, in TChar wch, int n);
        void mvwin_wch(WindowBaseSafeHandle win, int y, int x, out TChar wch);
        void mvwin_wchnstr(WindowBaseSafeHandle win, int y, int x, ref TCharString wchStr, int n);
        void mvwin_wchstr(WindowBaseSafeHandle win, int y, int x, ref TCharString wchStr);
        void mvwins_wch(WindowBaseSafeHandle win, int y, int x, in TChar wch);
        void mvwvline_set(WindowBaseSafeHandle win, int y, int x, in TChar wch, int n);
        void wadd_wch(WindowBaseSafeHandle win, in TChar wch);
        void wadd_wchnstr(WindowBaseSafeHandle win, in TCharString wchStr, int n);
        void wadd_wchstr(WindowBaseSafeHandle win, in TCharString wchStr);
        void wbkgrnd(WindowBaseSafeHandle win, in TChar wch);
        void wbkgrndset(WindowBaseSafeHandle win, in TChar wch);
        void wborder_set(
            WindowBaseSafeHandle win, 
            in TChar ls, 
            in TChar rs, 
            in TChar ts, 
            in TChar bs, 
            in TChar tl,
            in TChar tr, 
            in TChar bl, 
            in TChar br);
        void wecho_wchar(WindowBaseSafeHandle win, in TChar wch);
        void wgetbkgrnd(WindowBaseSafeHandle win, out TChar wch);
        void whline_set(WindowBaseSafeHandle win, in TChar wch, int n);
        void win_wch(WindowBaseSafeHandle win, out TChar wch);
        void win_wchnstr(WindowBaseSafeHandle win, ref TCharString wchStr, int n);
        void win_wchstr(WindowBaseSafeHandle win, ref TCharString wchStr);
        void wins_wch(WindowBaseSafeHandle win, in TChar wch);
        void wvline_set(WindowBaseSafeHandle win, in TChar wch, int n);
    }
}
