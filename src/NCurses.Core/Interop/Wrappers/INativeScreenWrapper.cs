using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.Wrappers
{
    public interface INativeScreenWrapper<TMultiByte, TMultiByteString, TWideChar, TWideCharString, TSingleByte, TSingleByteString, TChar, TCharString, TMouseEvent>
            : INativeScreenMultiByte<TMultiByte, TMultiByteString>,
            INativeScreenWideChar<TWideChar, TWideCharString>,
            INativeScreenSingleByte<TSingleByte, TSingleByteString>,
            INativeScreenChar<TChar, TCharString>
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
        void assume_default_colors_sp(IntPtr screen, int fg, int bg);
        int baudrate_sp(IntPtr screen);
        void beep_sp(IntPtr screen);
        bool can_change_color_sp(IntPtr screen);
        void cbreak_sp(IntPtr screen);
        void color_content_sp(IntPtr screen, short color, ref short red, ref short green, ref short blue);
        void curs_set_sp(IntPtr screen, int visibility);
        int def_prog_mode_sp(IntPtr screen);
        int def_shell_mode_sp(IntPtr screen);
        void delay_output_sp(IntPtr screen, int ms);
        void doupdate_sp(IntPtr screen);
        void echo_sp(IntPtr screen);
        void endwin_sp(IntPtr screen);
        void filter_sp(IntPtr screen);
        void flash_sp(IntPtr screen);
        void flushinp_sp(IntPtr screen);
        int get_escdelay_sp(IntPtr screen);
        void halfdelay_sp(IntPtr screen, int tenths);
        bool has_colors_sp(IntPtr screen);
        bool has_ic_sp(IntPtr screen);
        bool has_il_sp(IntPtr screen);
        bool has_mouse_sp(IntPtr screen);
        void init_color_sp(IntPtr screen, short color, short r, short g, short b);
        void init_pair_sp(IntPtr screen, short pair, short f, short b);
        void intrflush_sp(IntPtr screen, WindowBaseSafeHandle win, bool bf);
        bool isendwin_sp(IntPtr screen);
        bool is_term_resized_sp(IntPtr screen, int lines, int columns);
        int keyok_sp(IntPtr screen, int keycode, bool enable);
        int mouseinterval_sp(IntPtr screen, int erval);
        void mvcur_sp(IntPtr screen, int oldrow, int oldcol, int newrow, int newcol);
        void napms_sp(IntPtr screen, int ms);
        WindowBaseSafeHandle newpad_sp(IntPtr screen, int nlines, int ncols);
        WindowBaseSafeHandle newwin_sp(IntPtr screen, int nlines, int ncols, int begin_y, int begin_x);
        IntPtr new_prescr(IntPtr screen);
        void nl_sp(IntPtr screen);
        void nocbreak_sp(IntPtr screen);
        void noecho_sp(IntPtr screen);
        void nofilter_sp(IntPtr screen);
        void nonl_sp(IntPtr screen);
        void noqiflush_sp(IntPtr screen);
        void noraw_sp(IntPtr screen);
        void pair_content_sp(IntPtr screen, short pair, ref short fg, ref short bg);
        void qiflush_sp(IntPtr screen);
        void raw_sp(IntPtr screen);
        void resetty_sp(IntPtr screen);
        void reset_prog_mode_sp(IntPtr screen);
        void reset_shell_mode_sp(IntPtr screen);
        void resizeterm_sp(IntPtr screen, int lines, int columns);
        void resize_term_sp(IntPtr screen, int lines, int columns);
        void ripoffline_sp(IntPtr screen, int line, Action<IntPtr, int> init);
        void savetty(IntPtr screen);
        void set_escdelay_sp(IntPtr screen, int size);
        void set_tabsize_sp(IntPtr screen, int size);
        void slk_clear_sp(IntPtr screen);
        void slk_color_sp(IntPtr screen, short color_pair);
        void slk_init_sp(IntPtr screen, int fmt);
        void slk_noutrefresh_sp(IntPtr screen);
        void slk_refresh_sp(IntPtr screen);
        void slk_restore_sp(IntPtr screen);
        void slk_touch_sp(IntPtr screen);
        void start_color_sp(IntPtr screen);
        void typeahead_sp(IntPtr screen, int fd);
        void ungetch_sp(IntPtr screen, int ch);
        void use_default_colors_sp(IntPtr screen);
        void use_env_sp(IntPtr screen, bool f);
        int use_legacy_coding_sp(IntPtr screen, int level);
        void use_tioctl_sp(IntPtr screen, bool f);
    }
}
