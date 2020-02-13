using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.Char
{
    public interface INativeWindowChar<TChar, TCharString>
        where TChar : IChar
        where TCharString : ICharString
    {
        void mvwaddnstr(WindowBaseSafeHandle window, int y, int x, in TCharString str, int n);
        void mvwaddstr(WindowBaseSafeHandle window, int y, int x, in TCharString str);
        void mvwgetnstr(WindowBaseSafeHandle window, int y, int x, ref TCharString str, int n);
        void mvwgetstr(WindowBaseSafeHandle window, int y, int x, ref TCharString str);
        void mvwinnstr(WindowBaseSafeHandle window, int y, int x, ref TCharString str, int n, out int read);
        void mvwinsnstr(WindowBaseSafeHandle window, int y, int x, in TCharString str, int n);
        void mvwinsstr(WindowBaseSafeHandle window, int y, int x, in TCharString str);
        void mvwinstr(WindowBaseSafeHandle window, int y, int x, ref TCharString str, out int read);
        void mvwprintw(WindowBaseSafeHandle window, int y, int x, in TCharString format, params TCharString[] argList);
        void mvwscanw(WindowBaseSafeHandle window, int y, int x, ref TCharString format, params TCharString[] argList);
        void waddnstr(WindowBaseSafeHandle window, in TCharString str, int number);
        void waddstr(WindowBaseSafeHandle window, in TCharString str);
        void wgetnstr(WindowBaseSafeHandle window, ref TCharString str, int count);
        void wgetstr(WindowBaseSafeHandle window, ref TCharString str);
        void winnstr(WindowBaseSafeHandle window, ref TCharString str, int count, out int read);
        void winstr(WindowBaseSafeHandle window, ref TCharString str, out int read);
        void winsnstr(WindowBaseSafeHandle window, in TCharString str, int n);
        void winsstr(WindowBaseSafeHandle window, in TCharString str);
        void wprintw(WindowBaseSafeHandle window, in TCharString format, params TCharString[] argList);
        void wscanw(WindowBaseSafeHandle window, ref TCharString str, params TCharString[] argList);
    }
}
