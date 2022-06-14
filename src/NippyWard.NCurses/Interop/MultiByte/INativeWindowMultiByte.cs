using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.SafeHandles;

namespace NippyWard.NCurses.Interop.MultiByte
{
    public interface INativeWindowMultiByte<TMultiByte, TMultiByteString> 
        where TMultiByte : IMultiByteNCursesChar
        where TMultiByteString : IMultiByteNCursesCharString
    {
        void box_set(WindowBaseSafeHandle win, in TMultiByte verch, in TMultiByte horch);
        void mvwadd_wch(WindowBaseSafeHandle win, int y, int x, in TMultiByte wch);
        void mvwadd_wchnstr(WindowBaseSafeHandle win, int y, int x, in TMultiByteString wchStr, int n);
        void mvwadd_wchstr(WindowBaseSafeHandle win, int y, int x, in TMultiByteString wchStr);
        void mvwhline_set(WindowBaseSafeHandle win, int y, int x, in TMultiByte wch, int n);
        void mvwin_wch(WindowBaseSafeHandle win, int y, int x, out TMultiByte wch);
        void mvwin_wchnstr(WindowBaseSafeHandle win, int y, int x, ref TMultiByteString wchStr, int n);
        void mvwin_wchstr(WindowBaseSafeHandle win, int y, int x, ref TMultiByteString wchStr);
        void mvwins_wch(WindowBaseSafeHandle win, int y, int x, in TMultiByte wch);
        void mvwvline_set(WindowBaseSafeHandle win, int y, int x, in TMultiByte wch, int n);
        void wadd_wch(WindowBaseSafeHandle win, in TMultiByte wch);
        void wadd_wchnstr(WindowBaseSafeHandle win, in TMultiByteString wchStr, int n);
        void wadd_wchstr(WindowBaseSafeHandle win, in TMultiByteString wchStr);
        void wbkgrnd(WindowBaseSafeHandle win, in TMultiByte wch);
        void wbkgrndset(WindowBaseSafeHandle win, in TMultiByte wch);
        void wborder_set(
            WindowBaseSafeHandle win, 
            in TMultiByte ls, 
            in TMultiByte rs, 
            in TMultiByte ts, 
            in TMultiByte bs, 
            in TMultiByte tl,
            in TMultiByte tr, 
            in TMultiByte bl, 
            in TMultiByte br);
        void wecho_wchar(WindowBaseSafeHandle win, in TMultiByte wch);
        void wgetbkgrnd(WindowBaseSafeHandle win, out TMultiByte wch);
        void whline_set(WindowBaseSafeHandle win, in TMultiByte wch, int n);
        void win_wch(WindowBaseSafeHandle win, out TMultiByte wch);
        void win_wchnstr(WindowBaseSafeHandle win, ref TMultiByteString wchStr, int n);
        void win_wchstr(WindowBaseSafeHandle win, ref TMultiByteString wchStr);
        void wins_wch(WindowBaseSafeHandle win, in TMultiByte wch);
        void wvline_set(WindowBaseSafeHandle win, in TMultiByte wch, int n);
    }
}
