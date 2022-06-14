using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.SafeHandles;

namespace NippyWard.NCurses.Interop.Wrappers
{
    public interface INativeWindowWrapper<TMultiByte, TMultiByteString, TWideChar, TWideCharString, TSingleByte, TSingleByteString, TChar, TCharString, TMouseEvent>
            : INativeWindowMultiByte<TMultiByte, TMultiByteString>, 
            INativeWindowWideChar<TWideChar, TWideCharString>,
            INativeWindowSingleByte<TSingleByte, TSingleByteString>,
            INativeWindowChar<TChar, TCharString>
        where TMultiByte : IMultiByteNCursesChar
        where TMultiByteString : IMultiByteNCursesCharString
        where TWideChar : IMultiByteChar
        where TWideCharString : IMultiByteCharString
        where TSingleByte : ISingleByteNCursesChar
        where TSingleByteString : ISingleByteNCursesCharString
        where TChar : ISingleByteChar
        where TCharString : ISingleByteCharString
        where TMouseEvent : IMEVENT
    {
        void clearok(WindowBaseSafeHandle window, bool bf);
        int getattrs(WindowBaseSafeHandle window);
        int getbegx(WindowBaseSafeHandle window);
        int getbegy(WindowBaseSafeHandle window);
        int getcurx(WindowBaseSafeHandle window);
        int getcury(WindowBaseSafeHandle window);
        int getmaxx(WindowBaseSafeHandle window);
        int getmaxy(WindowBaseSafeHandle window);
        int getparx(WindowBaseSafeHandle window);
        int getpary(WindowBaseSafeHandle window);
        void idcok(WindowBaseSafeHandle window, bool bf);
        int idlok(WindowBaseSafeHandle window, bool bf);
        void immedok(WindowBaseSafeHandle window, bool bf);
        bool is_cleared(WindowBaseSafeHandle window);
        bool is_idcok(WindowBaseSafeHandle window);
        bool is_idlok(WindowBaseSafeHandle window);
        bool is_immedok(WindowBaseSafeHandle window);
        bool is_keypad(WindowBaseSafeHandle window);
        bool is_leaveok(WindowBaseSafeHandle window);
        bool is_linetouched(WindowBaseSafeHandle window, int line);
        bool is_nodelay(WindowBaseSafeHandle window);
        bool is_notimeout(WindowBaseSafeHandle window);
        bool is_pad(WindowBaseSafeHandle window);
        bool is_scrollok(WindowBaseSafeHandle window);
        bool is_subwin(WindowBaseSafeHandle window);
        bool is_syncok(WindowBaseSafeHandle window);
        bool is_wintouched(WindowBaseSafeHandle window);
        void keypad(WindowBaseSafeHandle window, bool bf);
        void leaveok(WindowBaseSafeHandle window, bool bf);
        void mvwdelch(WindowBaseSafeHandle window, int y, int x);
        int mvwgetch(WindowBaseSafeHandle window, int y, int x);
        bool mvwgetch(WindowBaseSafeHandle window, int y, int x, out char ch, out Key key);
        void nodelay(WindowBaseSafeHandle window, bool bf);
        void notimeout(WindowBaseSafeHandle window, bool bf);
        void redrawwin(WindowBaseSafeHandle window);
        void scroll(WindowBaseSafeHandle window);
        void scrollok(WindowBaseSafeHandle window, bool bf);
        void syncok(WindowBaseSafeHandle window, bool bf);
        void touchline(WindowBaseSafeHandle window, int start, int count);
        void touchwin(WindowBaseSafeHandle window);
        void untouchwin(WindowBaseSafeHandle window);
        void wattroff(WindowBaseSafeHandle window, int attrs);
        void wattron(WindowBaseSafeHandle window, int attrs);
        void wattrset(WindowBaseSafeHandle window, int attrs);
        void wclear(WindowBaseSafeHandle window);
        void wclrtobot(WindowBaseSafeHandle window);
        void wclrtoeol(WindowBaseSafeHandle window);
        void wcolor_set(WindowBaseSafeHandle window, short pair);
        void wcursyncup(WindowBaseSafeHandle window);
        void wdelch(WindowBaseSafeHandle window);
        void wdeleteln(WindowBaseSafeHandle window);
        bool wenclose(WindowBaseSafeHandle window, int y, int x);
        void werase(WindowBaseSafeHandle window);
        int wgetch(WindowBaseSafeHandle window);
        bool wgetch(WindowBaseSafeHandle window, out char ch, out Key key);
        int wgetdelay(WindowBaseSafeHandle window);
        IntPtr wgetparent(WindowBaseSafeHandle window);
        void wgetscrreg(WindowBaseSafeHandle window, ref int top, ref int bottom);
        void winsdelln(WindowBaseSafeHandle window, int n);
        void winsertln(WindowBaseSafeHandle window);
        bool wmouse_trafo(WindowBaseSafeHandle win, ref int pY, ref int pX, bool to_screen);
        void wmove(WindowBaseSafeHandle window, int y, int x);
        void wnoutrefresh(WindowBaseSafeHandle window);
        void wredrawln(WindowBaseSafeHandle window, int beg_line, int num_lines);
        void wrefresh(WindowBaseSafeHandle window);
        void wresize(WindowBaseSafeHandle window, int lines, int columns);
        void wscrl(WindowBaseSafeHandle window, int n);
        void wsetscrreg(WindowBaseSafeHandle window, int top, int bot);
        void wstandend(WindowBaseSafeHandle window);
        void wstandout(WindowBaseSafeHandle window);
        void wsyncdown(WindowBaseSafeHandle window);
        void wsyncup(WindowBaseSafeHandle window);
        void wtimeout(int delay);
        void wtouchln(WindowBaseSafeHandle window, int y, int n, int changed);
    }
}
