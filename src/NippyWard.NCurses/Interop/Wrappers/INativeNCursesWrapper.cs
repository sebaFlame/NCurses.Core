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
    public interface INativeNCursesWrapper<TMultiByte, TMultiByteString, TWideChar, TWideCharString, TSingleByte, TSingleByteString, TChar, TCharString, TMouseEvent>
            : INativeNCursesMultiByte<TMultiByte, TMultiByteString>,
            INativeNCursesWideChar<TWideChar, TWideCharString>,
            INativeNCursesSingleByte<TSingleByte, TSingleByteString, TMouseEvent>,
            INativeNCursesChar<TChar, TCharString>
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
        IACSMap ACSMap { get; }
        IACSMap WACSMap { get; }

        void assume_default_colors(int fg, int bg);
        int baudrate();
        void beep();
        bool can_change_color();
        void cbreak();
        int COLORS();
        void color_content(short color, ref short red, ref short green, ref short blue);
        void extended_color_content(int color, out int r, out int g, out int b);
        int COLOR_PAIRS();
        int COLS();
        void copywin(WindowBaseSafeHandle srcwin, WindowBaseSafeHandle dstwin, int sminrow, int smincol, int dminrow, int dmincol, int dmaxrow, int dmaxcol, int overlay);
        IntPtr curscr();
        void curs_set(int visibility);
        int def_prog_mode();
        int def_shell_mode();
        void delay_output(int ms);
        void delscreen_sp(IntPtr screen);
        void delwin(IntPtr window);
        WindowBaseSafeHandle derwin(WindowBaseSafeHandle window, int nlines, int ncols, int begin_y, int begin_x);
        void doupdate();
        WindowBaseSafeHandle dupwin(WindowBaseSafeHandle window);
        void echo();
        void endwin();
        int ESCDELAY();
        void filter();
        void flash();
        void flushinp();
        int get_escdelay();
        void halfdelay(int tenths);
        bool has_colors();
        bool has_ic();
        bool has_il();
        bool has_key(int ch, out Key key);
        bool has_mouse();
        StdScrSafeHandle initscr();
        void init_color(short color, short r, short g, short b);
        void init_extended_color(int color, int r, int g, int b);
        void init_pair(short pair, short f, short b);
        void init_extended_pair(int pair, int f, int b);
        void intrflush(bool bf);
        bool isendwin();
        bool is_term_resized(int lines, int columns);
        void keyok(int keycode, bool enable);
        int LINES();
        void meta(WindowBaseSafeHandle win, bool bf);
        int mouseinterval(int erval);
        bool mouse_trafo(ref int pY, ref int pX, bool to_screen);
        void mvcur(int oldrow, int oldcol, int newrow, int newcol);
        void mvderwin(WindowBaseSafeHandle window, int par_y, int par_x);
        void mvwin(WindowBaseSafeHandle win, int y, int x);
        void napms(int ms);
        WindowBaseSafeHandle newpad(int nlines, int ncols);
        IntPtr newscr();
        WindowBaseSafeHandle newwin(int nlines, int ncols, int begin_y, int begin_x);
        void nl();
        void nocbreak();
        void noecho();
        void nofilter();
        void nonl();
        void noqiflush();
        void noraw();
        void overlay(WindowBaseSafeHandle srcWin, WindowBaseSafeHandle destWin);
        void overwrite(WindowBaseSafeHandle srcWin, WindowBaseSafeHandle destWin);
        void pair_content(short pair, out short fg, out short bg);
        void extended_pair_content(int pair, out int f, out int b);
        int PAIR_NUMBER(uint attrs);
        void qiflush();
        void raw();
        void resetty();
        void reset_prog_mode();
        void reset_shell_mode();
        void resizeterm(int lines, int columns);
        void resize_term(int lines, int columns);
        void savetty();
        void set_escdelay(int size);
        void set_tabsize(int size);
        IntPtr set_term(IntPtr newScr);
        void slk_clear();
        void slk_color(short color_pair);
        void slk_init(int fmt);
        void slk_noutrefresh();
        void slk_refresh();
        void slk_restore();
        void slk_touch();
        void start_color();
        IntPtr stdscr();
        WindowBaseSafeHandle subpad(WindowBaseSafeHandle orig, int nlines, int ncols, int begin_y, int begin_x);
        WindowBaseSafeHandle subwin(WindowBaseSafeHandle orig, int nlines, int ncols, int begin_y, int begin_x);
        int TABSIZE();
        string ttytype();
        void typeahead(int fd);
        void ungetch(int ch);
        void use_default_colors();
        void use_env(bool f);
        int use_extended_names(bool enable);
        int use_legacy_coding(int level);
        void use_tioctl(bool f);
        IntPtr _nc_screen_of(WindowBaseSafeHandle window);
        bool _nc_unicode_locale();
    }
}
