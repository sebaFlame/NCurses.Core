using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.WideChar
{
    public interface INativeWindowWideChar<TChar, TCharString>
        where TChar : IChar
        where TCharString : ICharString
    {
        void mvwaddnwstr(WindowBaseSafeHandle window, int y, int x, in TCharString wstr, int n);
        void mvwaddwstr(WindowBaseSafeHandle window, int y, int x, in TCharString wstr);
        bool mvwget_wch(WindowBaseSafeHandle window, int y, int x, out TChar wch, out Key key);
        void mvwget_wstr(WindowBaseSafeHandle window, int y, int x, ref TCharString wstr);
        void mvwgetn_wstr(WindowBaseSafeHandle window, int y, int x, ref TCharString wstr, int n);
        void mvwinnwstr(WindowBaseSafeHandle window, int y, int x, ref TCharString wstr, int n, out int read);
        void mvwins_nwstr(WindowBaseSafeHandle window, int y, int x, in TCharString wstr, int n);
        void mvwins_wstr(WindowBaseSafeHandle window, int y, int x, in TCharString wstr);
        void mvwinwstr(WindowBaseSafeHandle window, int y, int x, ref TCharString wstr);
        void waddnwstr(WindowBaseSafeHandle window, in TCharString wstr, int n);
        void waddwstr(WindowBaseSafeHandle window, in TCharString wstr);
        bool wget_wch(WindowBaseSafeHandle window, out TChar wch, out Key key);
        void wget_wstr(WindowBaseSafeHandle window, ref TCharString wstr);
        void wgetn_wstr(WindowBaseSafeHandle window, ref TCharString wstr, int n);
        void winnwstr(WindowBaseSafeHandle window, ref TCharString wstr, int count, out int read);
        void winwstr(WindowBaseSafeHandle window, ref TCharString wstr);
        void wins_nwstr(WindowBaseSafeHandle window, in TCharString wstr, int n);
        void wins_wstr(WindowBaseSafeHandle window, in TCharString wstr);
    }
}
