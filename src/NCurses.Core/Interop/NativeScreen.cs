using System;
using System.Text;
using System.Runtime.InteropServices;

#if NCURSES_VERSION_6
using chtype = System.UInt32;
#elif NCURSES_VERSION_5
using chtype = System.UInt64;
#endif

namespace NCurses.Core.Interop
{
    /// <summary>
    /// native screen methods (wrapped into *_sp when ncurses was compiled with NCURSES_SP_FUNCS).
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    internal static class NativeScreen
    {
        #region baudrate_sp
        /// <summary>
        /// see <see cref="NativeNCurses.baudrate"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        /// <returns>The number returned is in  bits per  second,  for example 9600, and is an integer.</returns>
        public static int baudrate(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.baudrate_sp(screen);
        }

        /// <summary>
        /// see <see cref="baudrate"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static int baudrate_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.baudrate_sp(screen);
            return NativeNCurses.use_screen(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)));
        }
        #endregion

        #region beep_sp
        /// <summary>
        /// see <see cref="NativeNCurses.beep"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void beep(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.beep_sp(screen), "beep_sp");
        }

        /// <summary>
        /// <see cref="beep"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void beep_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.beep_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "beep_sp");
        }
        #endregion

        #region can_change_color_sp
        /// <summary>
        /// see <see cref="NativeNCurses.can_change_color"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static bool can_change_color(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.can_change_color_sp(screen);
        }
        #endregion

        #region cbreak_sp
        /// <summary>
        /// see <see cref="NativeNCurses.cbreak"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void cbreak(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.cbreak_sp(screen), "cbreak_sp");
        }

        /// <summary>
        /// see <see cref="cbreak"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void cbreak_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.cbreak_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "cbreak_sp");
        }
        #endregion

        #region nocbreak_sp
        /// <summary>
        /// see <see cref="NativeNCurses.nocbreak"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void nocbreak(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.nocbreak_sp(screen), "nocbreak_sp");
        }

        /// <summary>
        /// see <see cref="nocbreak"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void nocbreak_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.nocbreak_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "nocbreak_sp");
        }
        #endregion

        #region color_content_sp
        /// <summary>
        /// see <see cref="NativeNCurses.color_content"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void color_content(IntPtr screen, short color, ref short red, ref short green, ref short blue)
        {
            IntPtr rPtr = Marshal.AllocHGlobal(Marshal.SizeOf(red));
            Marshal.StructureToPtr(red, rPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(red));

            IntPtr gPtr = Marshal.AllocHGlobal(Marshal.SizeOf(green));
            Marshal.StructureToPtr(green, gPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(green));

            IntPtr bPtr = Marshal.AllocHGlobal(Marshal.SizeOf(blue));
            Marshal.StructureToPtr(blue, bPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(blue));

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.color_content_sp(screen, color, rPtr, gPtr, bPtr), "color_content_sp");
            }
            finally
            {
                Marshal.FreeHGlobal(rPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(red));

                Marshal.FreeHGlobal(gPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(green));

                Marshal.FreeHGlobal(bPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(blue));
            }
        }

        /// <summary>
        /// see <see cref="color_content"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void color_content_t(IntPtr screen, short color, ref short red, ref short green, ref short blue)
        {
            IntPtr rPtr = Marshal.AllocHGlobal(Marshal.SizeOf(red));
            Marshal.StructureToPtr(red, rPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(red));

            IntPtr gPtr = Marshal.AllocHGlobal(Marshal.SizeOf(green));
            Marshal.StructureToPtr(green, gPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(green));

            IntPtr bPtr = Marshal.AllocHGlobal(Marshal.SizeOf(blue));
            Marshal.StructureToPtr(blue, bPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(blue));

            try
            {
                Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.color_content_sp(screen, color, rPtr, gPtr, bPtr);
                NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "color_content_sp");
            }
            finally
            {
                Marshal.FreeHGlobal(rPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(red));

                Marshal.FreeHGlobal(gPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(green));

                Marshal.FreeHGlobal(bPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(blue));
            }
        }
        #endregion

        #region curs_set_sp
        /// <summary>
        /// see <see cref="NativeNCurses.curs_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void curs_set(IntPtr screen, int visibility)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.curs_set_sp(screen, visibility), "curs_set_sp");
        }

        /// <summary>
        /// see <see cref="curs_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void curs_set_t(IntPtr screen, int visibility)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.curs_set_sp(screen, visibility);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "curs_set_sp");
        }
        #endregion

        #region def_prog_mode_sp
        /// <summary>
        /// see <see cref="NativeNCurses.def_prog_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int def_prog_mode(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.def_prog_mode_sp(screen);
        }
        #endregion

        #region def_shell_mode_sp
        /// <summary>
        /// see <see cref="NativeNCurses.def_shell_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int def_shell_mode(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.def_shell_mode_sp(screen);
        }
        #endregion

        #region delay_output_sp
        /// <summary>
        /// see <see cref="NativeNCurses.delay_output"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void delay_output(IntPtr screen, int ms)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.delay_output_sp(screen, ms), "delay_output_sp");
        }

        /// <summary>
        /// see <see cref="delay_output"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void delay_output_t(IntPtr screen, int ms)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.delay_output_sp(screen, ms);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "delay_output_sp");
        }
        #endregion

        #region doupdate_sp
        /// <summary>
        /// see <see cref="NativeNCurses.doupdate"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void doupdate(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.doupdate_sp(screen), "doupdate_sp");
        }

        /// <summary>
        /// see <see cref="doupdate"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void doupdate_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.doupdate_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "doupdate_sp");
        }
        #endregion

        #region echo_sp
        /// <summary>
        /// see <see cref="NativeNCurses.echo"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void echo(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.echo_sp(screen), "echo_sp");
        }

        /// <summary>
        /// see <see cref="echo"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void echo_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.echo_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "echo_sp");
        }
        #endregion

        #region endwin_sp
        /// <summary>
        /// see <see cref="NativeNCurses.endwin"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void endwin(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.endwin_sp(screen), "endwin_sp");
        }

        /// <summary>
        /// see <see cref="endwin"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void endwin_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.endwin_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "endwin_sp");
        }
        #endregion

        #region erasechar_sp
        /// <summary>
        /// see <see cref="NativeNCurses.erasechar"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        /// <returns>The user's current erase character</returns>
        public static char erasechar(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.erasechar_sp(screen);
        }
        #endregion

        #region filter_sp
        /// <summary>
        /// The filter routine, if used, must be called before initscr
        /// or newterm are called.The effect is that, during those
        /// calls, LINES  is  set to 1; the capabilities clear, cup,
        /// cud, cud1, cuu1, cuu,  vpa are  disabled;  and the  home
        /// string is set to the value of cr.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void filter(IntPtr screen)
        {
            NativeNCurses.NCursesWrapper.filter_sp(screen);
        }
        #endregion

        #region flash_sp
        /// <summary>
        /// see <see cref="NativeNCurses.flash"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void flash(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.flash_sp(screen), "flash_sp");
        }

        /// <summary>
        /// see <see cref="flash"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void flash_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.flash_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "flash_sp");
        }
        #endregion

        #region flushinp_sp
        /// <summary>
        /// see <see cref="NativeNCurses.flushinp"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void flushinp(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.flushinp_sp(screen), "flushinp_sp");
        }

        /// <summary>
        /// see <see cref="flushinp"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void flushinp_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.flushinp_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "flushinp_sp");
        }
        #endregion

        #region halfdelay_sp
        /// <summary>
        /// see <see cref="NativeNCurses.halfdelay"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void halfdelay(IntPtr screen, int tenths)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.halfdelay_sp(screen, tenths), "halfdelay_sp");
        }

        /// <summary>
        /// see <see cref="halfdelay"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void halfdelay_t(IntPtr screen, int tenths)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.halfdelay_sp(screen, tenths);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "halfdelay_sp");
        }
        #endregion

        #region has_colors_sp
        /// <summary>
        /// see <see cref="NativeNCurses.has_colors"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static bool has_colors(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.has_colors_sp(screen);
        }
        #endregion

        #region has_ic_sp
        /// <summary>
        /// see <see cref="NativeNCurses.has_ic"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static bool has_ic(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.has_ic_sp(screen);
        }
        #endregion

        #region has_il_sp
        /// <summary>
        /// see <see cref="NativeNCurses.has_il"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static bool has_il(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.has_il_sp(screen);
        }
        #endregion

        #region init_color
        /// <summary>
        /// see <see cref="NativeNCurses.init_color"/>
        /// </summary>
        public static void init_color(IntPtr screen, short color, short r, short g, short b)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.init_color_sp(screen, color, r, g, b), "init_color");
        }
        #endregion

        #region init_pair_sp
        /// <summary>
        /// see <see cref="NativeNCurses.init_pair"/>
        /// </summary>
        public static void init_pair(IntPtr screen, short pair, short f, short b)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.init_pair_sp(screen, pair, f, b), "init_pair_sp");
        }
        #endregion

        #region intrflush_sp
        /// <summary>
        /// see <see cref="NativeNCurses.intrflush"/>
        /// </summary>
        /// <param name="win"></param>
        /// <param name="bf"></param>
        public static void intrflush(IntPtr screen, IntPtr win, bool bf)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.intrflush_sp(screen, win, bf), "intrflush_sp");
        }
        #endregion

        #region isendwin_sp
        /// <summary>
        /// see <see cref="NativeNCurses.isendwin"/>
        /// </summary>
        public static bool isendwin(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.isendwin_sp(screen);
        }
        #endregion

        #region keyname_sp
        /// <summary>
        /// see <see cref="NativeNCurses.keyname(int)"/>
        /// </summary>
        public static string keyname(IntPtr screen, int c)
        {
            return NativeNCurses.NCursesWrapper.keyname_sp(screen, c);
        }
        #endregion

        #region killchar_sp
        /// <summary>
        /// see <see cref="NativeNCurses.killchar"/>
        /// </summary>
        public static char killchar(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.killchar_sp(screen);
        }
        #endregion

        #region longname_sp
        /// <summary>
        /// see <see cref="NativeNCurses.longname"/>
        /// </summary>
        public static string longname(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.longname_sp(screen);
        }
        #endregion

        #region mvcur_sp
        /// <summary>
        /// see <see cref="NativeNCurses.mvcur(int, int, int, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void mvcur(IntPtr screen, int oldrow, int oldcol, int newrow, int newcol)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvcur_sp(screen, oldrow, oldcol, newrow, newcol), "mvcur_sp");
        }

        /// <summary>
        /// see <see cref="mvcur(IntPtr, int, int, int, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvcur_t(IntPtr screen, int oldrow, int oldcol, int newrow, int newcol)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvcur_sp(screen, oldrow, oldcol, newrow, newcol);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "mvcur_sp");
        }
        #endregion

        #region napms_sp
        /// <summary>
        /// see <see cref="NativeNCurses.napms"/>
        /// </summary>
        public static void napms(IntPtr screen, int ms)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.napms_sp(screen, ms), "napms_sp");
        }
        #endregion

        #region newpad_sp
        /// <summary>
        /// see <see cref="NativeNCurses.newpad"/>
        /// </summary>
        public static IntPtr newpad(IntPtr screen, int nlines, int ncols)
        {
            return NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.newpad_sp(screen, nlines, ncols), "newpad_sp");
        }
        #endregion

        #region newwin_sp
        /// <summary>
        /// see <see cref="NativeNCurses.newwin"/>
        /// </summary>
        public static IntPtr newwin(IntPtr screen, int nlines, int ncols, int begin_y, int begin_x)
        {
            return NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.newwin_sp(screen, nlines, ncols, begin_y, begin_x), "newwin_sp");
        }
        #endregion

        #region nl_sp
        /// <summary>
        /// see <see cref="NativeNCurses.nl"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void nl(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.nl_sp(screen), "nl_sp");
        }

        /// <summary>
        /// see <see cref="nl(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void nl_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.nl_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "nl_sp");
        }
        #endregion

        #region noecho_sp
        /// <summary>
        /// see <see cref="NativeNCurses.noecho"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void noecho(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.noecho_sp(screen), "noecho_sp");
        }

        /// <summary>
        /// see <see cref="noecho(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void noecho_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.noecho_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "noecho_sp");
        }
        #endregion

        #region nonl_sp
        /// <summary>
        /// see <see cref="NativeNCurses.nonl"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void nonl(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.nonl_sp(screen), "nonl_sp");
        }

        /// <summary>
        /// see <see cref="nonl(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void nonl_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.nonl_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "nonl_sp");
        }
        #endregion

        #region noqiflush_sp
        /// <summary>
        /// see <see cref="qiflush"/>
        /// </summary>
        public static void noqiflush(IntPtr screen)
        {
            NativeNCurses.NCursesWrapper.noqiflush_sp(screen);
        }
        #endregion

        #region noraw_sp
        /// <summary>
        /// see <see cref="NativeNCurses.raw"/>
        /// <para />native method wrapped with verification.
        /// <param name="screen">A pointer to a screen</param>
        /// </summary>
        public static void noraw(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.noraw_sp(screen), "noraw_sp");
        }

        /// <summary>
        /// see <see cref="noraw(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void noraw_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.noraw_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "noraw_sp");
        }
        #endregion

        #region pair_content_sp
        /// <summary>
        /// see <see cref="NativeNCurses.pair_content(short, ref short, ref short)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void pair_content(IntPtr screen, short pair, ref short fg, ref short bg)
        {
            IntPtr fgPtr = Marshal.AllocHGlobal(Marshal.SizeOf(fg));
            Marshal.StructureToPtr(fg, fgPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(fg));

            IntPtr bgPtr = Marshal.AllocHGlobal(Marshal.SizeOf(bg));
            Marshal.StructureToPtr(bg, bgPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(bg));

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.pair_content_sp(screen, pair, fgPtr, bgPtr), "pair_content_sp");
            }
            finally
            {
                Marshal.FreeHGlobal(fgPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(fg));

                Marshal.FreeHGlobal(bgPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(bg));
            }
        }

        /// <summary>
        /// see <see cref="pair_content(IntPtr, short, ref short, ref short)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void pair_content_t(IntPtr screen, short pair, ref short fg, ref short bg)
        {
            IntPtr fgPtr = Marshal.AllocHGlobal(Marshal.SizeOf(fg));
            Marshal.StructureToPtr(fg, fgPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(fg));

            IntPtr bgPtr = Marshal.AllocHGlobal(Marshal.SizeOf(bg));
            Marshal.StructureToPtr(bg, bgPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(bg));

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.pair_content_sp(screen, pair, fgPtr, bgPtr), "pair_content_sp");
            }
            finally
            {
                Marshal.FreeHGlobal(fgPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(fg));

                Marshal.FreeHGlobal(bgPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(bg));
            }
        }
        #endregion

        #region qiflush_sp
        /// <summary>
        /// see <see cref="NativeNCurses.qiflush"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void qiflush(IntPtr screen)
        {
            NativeNCurses.NCursesWrapper.qiflush_sp(screen);
        }
        #endregion

        #region raw_sp
        /// <summary>
        /// see <see cref="NativeNCurses.raw"/>
        /// <para />native method wrapped with verification.
        /// <param name="screen">A pointer to a screen</param>
        /// </summary>
        public static void raw(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.raw_sp(screen), "raw_sp");
        }

        /// <summary>
        /// see <see cref="raw(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void raw_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.raw_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "raw_sp");
        }
        #endregion

        #region resetty_sp
        /// <summary>
        /// see <see cref="NativeNCurses.resetty"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void resetty(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.resetty_sp(screen), "resetty_sp");
        }

        /// <summary>
        /// see <see cref="resetty(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void resetty_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.resetty_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "resetty_sp");
        }
        #endregion

        #region reset_prog_mode_sp
        /// <summary>
        /// see <see cref="NativeNCurses.reset_prog_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void reset_prog_mode(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.reset_prog_mode_sp(screen), "reset_prog_mode_sp");
        }

        /// <summary>
        /// see <see cref="reset_prog_mode(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void reset_prog_mode_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.reset_prog_mode_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "reset_prog_mode_sp");
        }
        #endregion

        #region reset_shell_mode_sp
        /// <summary>
        /// see <see cref="NativeNCurses.reset_shell_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void reset_shell_mode(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.reset_shell_mode_sp(screen), "reset_shell_mode_sp");
        }

        /// <summary>
        /// see <see cref="reset_shell_mode(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void reset_shell_mode_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.reset_shell_mode_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "reset_shell_mode_sp");
        }
        #endregion

        #region ripoffline_sp
        /// <summary>
        /// see <see cref="NativeNCurses.ripoffline(int, Action{IntPtr, int})"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="line">a positive or negative integer</param>
        /// <param name="init">a method to be called on initscr (a window pointer and number of columns gets passed)</param>
        public static void ripoffline(IntPtr screen, int line, Action<IntPtr, int> init)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.ripoffline_sp(screen, line, Marshal.GetFunctionPointerForDelegate(init)), "ripoffline_sp");
        }

        /// <summary>
        /// see <see cref="ripoffline(IntPtr, int, Action{IntPtr, int})"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void ripoffline_t(IntPtr screen, int line, Action<IntPtr, int> init)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.ripoffline_sp(screen, line, Marshal.GetFunctionPointerForDelegate(init));
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "ripoffline_sp");
        }
        #endregion

        #region savetty_sp
        /// <summary>
        /// see <see cref="NativeNCurses.savetty"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void savetty(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.savetty_sp(screen), "savetty_sp");
        }

        /// <summary>
        /// see <see cref="savetty(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void savetty_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.savetty_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "savetty_sp");
        }
        #endregion

        #region scr_dump_sp
        /// <summary>
        /// see <see cref="src_init(string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void scr_dump(IntPtr screen, string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.scr_dump_sp(screen, filename), "scr_dump_sp");
        }

        /// <summary>
        /// see <see cref="scr_dump(IntPtr, string)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void scr_dump_t(IntPtr screen, string filename)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.scr_dump_sp(screen, filename);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "scr_dump_sp");
        }
        #endregion

        #region scr_init_sp
        /// <summary>
        /// see <see cref="NativeNCurses.scr_init(string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void scr_init(IntPtr screen, string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.scr_init_sp(screen, filename), "scr_init_sp");
        }

        /// <summary>
        /// see <see cref="scr_init(IntPtr, string)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void scr_init_t(IntPtr screen, string filename)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.scr_init_sp(screen, filename);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "scr_init_sp");
        }
        #endregion

        #region scr_restore_sp
        /// <summary>
        /// see <see cref="NativeNCurses.scr_restore(string)"/>
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void scr_restore(IntPtr screen, string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.scr_restore_sp(screen, filename), "scr_restore_sp");
        }

        /// <summary>
        /// see <see cref="scr_restore(IntPtr, string)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void scr_restore_t(IntPtr screen, string filename)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.scr_restore_sp(screen, filename);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "scr_restore_sp");
        }
        #endregion

        #region scr_set_sp
        /// <summary>
        /// see <see cref="NativeNCurses.scr_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void scr_set(IntPtr screen, string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.scr_set_sp(screen, filename), "src_set_sp");
        }

        /// <summary>
        /// see <see cref="scr_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void scr_set_t(IntPtr screen, string filename)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.scr_set_sp(screen, filename);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "scr_set_sp");
        }
        #endregion

        #region slk_attroff_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_attroff(chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_attroff(IntPtr screen, chtype attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_attroff_sp(screen, attrs), "slk_attroff_sp");
        }

        /// <summary>
        /// see <see cref="slk_attroff(IntPtr, chtype)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_attroff_t(IntPtr screen, chtype attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_attroff_sp(screen, attrs);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_attroff_sp");
        }
        #endregion

        #region slk_attron_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_attroff(chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_attron(IntPtr screen, chtype attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_attron_sp(screen, attrs), "slk_attron_sp");
        }

        /// <summary>
        /// see <see cref="slk_attron(IntPtr, chtype)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_attron_t(IntPtr screen, chtype attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_attron_sp(screen, attrs);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_attron_sp");
        }
        #endregion

        #region slk_attrset_sp
        /// <summary>
        /// see <see cref="slk_attroff(IntPtr, chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_attrset(IntPtr screen, chtype attrs, short color_pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_attrset_sp(screen, attrs, color_pair, IntPtr.Zero), "slk_attrset_sp");
        }

        /// <summary>
        /// see <see cref="slk_attrset(IntPtr, chtype, short)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_attrset_t(IntPtr screen, chtype attrs, short color_pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_attrset_sp(screen, attrs, color_pair, IntPtr.Zero);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_attrset_sp");
        }
        #endregion

        #region slk_attr_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_attr"/>
        /// </summary>
        /// <returns>an attribute</returns>
        public static chtype slk_attr(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.slk_attr_sp(screen);
        }
        #endregion

        #region slk_attr_set_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_attr_set(chtype, short)"/> (for soft function keys)
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_attr_set(IntPtr screen, chtype attrs, short color_pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_attr_set_sp(screen, attrs, color_pair, IntPtr.Zero), "slk_attr_set_sp");
        }

        /// <summary>
        /// see <see cref="slk_attr_set(IntPtr, chtype, short)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_attr_set_t(IntPtr screen, chtype attrs, short color_pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_attr_set_sp(screen, attrs, color_pair, IntPtr.Zero);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_attr_set_sp");
        }
        #endregion

        #region slk_clear_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_clear"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_clear(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_clear_sp(screen), "slk_clear_sp");
        }

        /// <summary>
        /// see <see cref="slk_clear"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_clear_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_clear_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_clear_sp");
        }
        #endregion

        #region slk_color_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_color(short)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_color(IntPtr screen, short color_pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_color_sp(screen, color_pair), "slk_color_sp");
        }

        /// <summary>
        /// see <see cref="slk_color(IntPtr, short)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_color_t(IntPtr screen, short color_pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_color_sp(screen, color_pair);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_color_sp");
        }
        #endregion

        #region slk_init_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_init(int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_init(IntPtr screen, int fmt)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_init_sp(screen, fmt), "slk_init_sp");
        }

        /// <summary>
        /// see <see cref="slk_init(IntPtr, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_init_t(IntPtr screen, int fmt)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_init_sp(screen, fmt);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_init_sp");
        }
        #endregion

        #region slk_label_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_label(int)"/>
        /// </summary>
        /// <param name="labnum">number of the label for which you want to return the label</param>
        /// <returns>label</returns>
        public static string slk_label(IntPtr screen, int labnum)
        {
            return NativeNCurses.NCursesWrapper.slk_label_sp(screen, labnum);
        }
        #endregion

        #region slk_noutrefresh_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_noutrefresh"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_noutrefresh(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_noutrefresh_sp(screen), "slk_noutrefresh_sp");
        }

        /// <summary>
        /// see <see cref="slk_noutrefresh"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_noutrefresh_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_noutrefresh_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_noutrefresh_sp");
        }
        #endregion

        #region slk_refresh_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_refresh"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_refresh(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_refresh_sp(screen), "slk_refresh_sp");
        }

        /// <summary>
        /// see <see cref="slk_refresh"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_refresh_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_refresh_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_refresh_sp");
        }
        #endregion

        #region slk_restore_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_restore"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_restore(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_restore_sp(screen), "slk_restore_sp");
        }

        /// <summary>
        /// see <see cref="slk_restore"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_restore_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_restore_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_restore_sp");
        }
        #endregion

        #region slk_set_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_set(int, string, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_set(IntPtr screen, int labnum, string label, int fmt)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_set_sp(screen, labnum, label, fmt), "slk_set_sp");
        }

        /// <summary>
        /// see <see cref="slk_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_set_t(IntPtr screen, int labnum, string label, int fmt)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_set_sp(screen, labnum, label, fmt);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_set_sp");
        }
        #endregion

        #region slk_touch_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_touch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_touch(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.slk_touch_sp(screen), "slk_touch_sp");
        }

        /// <summary>
        /// see <see cref="slk_touch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_touch_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.slk_touch_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_touch_sp");
        }
        #endregion

        #region start_color_sp
        /// <summary>
        /// see <see cref="NativeNCurses.start_color"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void start_color(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.start_color_sp(screen), "start_color_sp");
        }

        /// <summary>
        /// see <see cref="start_color"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void start_color_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.start_color_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "start_color_sp");
        }
        #endregion

        #region termattrs_sp
        /// <summary>
        /// see <see cref="NativeNCurses.termattrs"/>
        /// </summary>
        public static chtype termattrs(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.termattrs_sp(screen);
        }
        #endregion

        #region termname_sp
        /// <summary>
        /// see <see cref="NativeNCurses.termname"/>
        /// </summary>
        /// <returns>the terminal name</returns>
        public static string termname(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.termname_sp(screen);
        }
        #endregion

        #region typeahead_sp
        /// <summary>
        /// see <see cref="NativeNCurses.typeahead"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void typeahead(IntPtr screen, int fd)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.typeahead_sp(screen, fd), "typeahead_sp");
        }

        /// <summary>
        /// see <see cref="typeahead"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void typeahead_t(IntPtr screen, int fd)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.typeahead_sp(screen, fd);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "typeahead_sp");
        }
        #endregion

        #region ungetch_sp
        /// <summary>
        /// see <see cref="NativeNCurses.ungetch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void ungetch(IntPtr screen, int ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.ungetch_sp(screen, ch), "ungetch_sp");
        }

        /// <summary>
        /// see <see cref="ungetch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void ungetch_t(IntPtr screen, int ch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.ungetch_sp(screen, ch);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "ungetch_sp");
        }
        #endregion

        #region use_env_sp
        /// <summary>
        /// see <see cref="NativeNCurses.use_env"/>
        /// </summary>
        public static void use_env(IntPtr screen, bool f)
        {
            NativeNCurses.NCursesWrapper.use_env_sp(screen, f);
        }
        #endregion

        #region use_tioctl_sp
        /// <summary>
        /// see <see cref="NativeNCurses.use_tioctl"/>
        /// </summary>
        public static void use_tioctl(IntPtr screen, bool f)
        {
            NativeNCurses.NCursesWrapper.use_tioctl_sp(screen, f);
        }
        #endregion

        #region vidattr_sp
        /// <summary>
        /// see <see cref="NativeNCurses.vidattr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vidattr(IntPtr screen, chtype attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.vidattr_sp(screen, attrs), "vidattr_sp");
        }

        /// <summary>
        /// see <see cref="vidattr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void vidattr_t(IntPtr screen, chtype attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.vidattr_sp(screen, attrs);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "vidattr_sp");
        }
        #endregion

        #region vidputs_sp
        /// <summary>
        /// see <see cref="NativeNCurses.vidputs"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vidputs(IntPtr screen, chtype attrs, Func<int, int> NCURSES_OUTC)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.vidputs_sp(screen, attrs, Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC)), "vidputs_sp");
        }

        /// <summary>
        /// see <see cref="vidputs"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void vidputs_t(IntPtr screen, chtype attrs, Func<int, int> NCURSES_OUTC)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.vidputs_sp(screen, attrs, Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC));
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "vidputs_sp");
        }
        #endregion

        #region tigetflag_sp
        /// <summary>
        /// see <see cref="NativeNCurses.tigetflag"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int tigetflag(IntPtr screen, string capname)
        {
            return NativeNCurses.NCursesWrapper.tigetflag_sp(screen, capname);
        }
        #endregion

        #region tigetnum_sp
        /// <summary>
        /// see <see cref="NativeNCurses.tigetnum"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int tigetnum(IntPtr screen, string capname)
        {
            return NativeNCurses.NCursesWrapper.tigetnum_sp(screen, capname);
        }
        #endregion

        #region tigetstr_sp
        /// <summary>
        /// see <see cref="NativeNCurses.tigetstr"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static string tigetstr(IntPtr screen, string capname)
        {
            return NativeNCurses.NCursesWrapper.tigetstr_sp(screen, capname);
        }
        #endregion

        #region putp_sp
        /// <summary>
        /// see <see cref="NativeNCurses.putp"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void putp(IntPtr screen, string str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.putp_sp(screen, str), "putp_sp");
        }

        /// <summary>
        /// see <see cref="putp"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void putp_t(IntPtr screen, string str)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.putp_sp(screen, str);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "putp_sp");
        }
        #endregion

        #region is_term_resized_sp
        /// <summary>
        /// see <see cref="NativeNCurses.is_term_resized"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static bool is_term_resized(IntPtr screen, int lines, int columns)
        {
            return NativeNCurses.NCursesWrapper.is_term_resized_sp(screen, lines, columns);
        }
        #endregion

        #region keybound_sp
        /// <summary>
        /// see <see cref="NativeNCurses.keybound"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static string keybound(IntPtr screen, int keycode, int count)
        {
            return NativeNCurses.NCursesWrapper.keybound_sp(screen, keycode, count);
        }
        #endregion

        #region assume_default_colors_sp
        /// <summary>
        /// see <see cref="NativeNCurses.assume_default_colors"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void assume_default_colors(IntPtr screen, int fg, int bg)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.assume_default_colors_sp(screen, fg, bg), "assume_default_colors_sp");
        }

        /// <summary>
        /// see <see cref="assume_default_colors"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void assume_default_colors_t(IntPtr screen, int fg, int bg)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.assume_default_colors_sp(screen, fg, bg);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "assume_default_colors_sp");
        }
        #endregion

        #region define_key_sp
        /// <summary>
        /// see <see cref="NativeNCurses.define_key"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void define_key(IntPtr screen, string definition, int keycode)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.define_key_sp(screen, definition, keycode), "define_key_sp");
        }

        /// <summary>
        /// see <see cref="define_key"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void define_key_t(IntPtr screen, string definition, int keycode)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.define_key_sp(screen, definition, keycode);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "define_key_sp");
        }
        #endregion

        #region get_escdelay_sp
        /// <summary>
        /// see <see cref="NativeNCurses.get_escdelay"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int get_escdelay(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.get_escdelay_sp(screen);
        }
        #endregion

        #region key_defined_sp
        /// <summary>
        /// see <see cref="NativeNCurses.key_defined"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int key_defined(IntPtr screen, string definition)
        {
            return NativeNCurses.NCursesWrapper.key_defined_sp(screen, definition);
        }
        #endregion

        #region keyok_sp
        /// <summary>
        /// see <see cref="NativeNCurses.keyok"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int keyok_sp(IntPtr screen, int keycode, bool enable)
        {
            return NativeNCurses.NCursesWrapper.keyok_sp(screen, keycode, enable);
        }
        #endregion

        #region resize_term_sp
        /// <summary>
        /// see <see cref="NativeNCurses.resize_term"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void resize_term(IntPtr screen, int lines, int columns)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.resize_term_sp(screen, lines, columns), "resize_term_sp");
        }

        /// <summary>
        /// see <see cref="resize_term"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void resize_term_t(IntPtr screen, int lines, int columns)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.resize_term_sp(screen, lines, columns);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "resize_term_sp");
        }
        #endregion

        #region resizeterm_sp
        /// <summary>
        /// see <see cref="NativeNCurses.resizeterm"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void resizeterm(IntPtr screen, int lines, int columns)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.resizeterm_sp(screen, lines, columns), "resizeterm_sp");
        }

        /// <summary>
        /// see <see cref="resizeterm"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void resizeterm_t(IntPtr screen, int lines, int columns)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.resizeterm_sp(screen, lines, columns);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "resizeterm_sp");
        }
        #endregion

        #region set_escdelay_sp
        /// <summary>
        /// see <see cref="NativeNCurses.set_escdelay"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void set_escdelay(IntPtr screen, int size)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.set_escdelay_sp(screen, size), "set_escdelay_sp");
        }

        /// <summary>
        /// see <see cref="set_escdelay"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void set_escdelay_t(IntPtr screen, int size)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.set_escdelay_sp(screen, size);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "set_escdelay_sp");
        }
        #endregion

        #region set_tabsize_sp
        /// <summary>
        /// see <see cref="NativeNCurses.set_tabsize"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void _set_tabsize(IntPtr screen, int size)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.set_tabsize_sp(screen, size), "set_tabsize_sp");
        }

        /// <summary>
        /// see <see cref="set_escdelay"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void _set_tabsize_t(IntPtr screen, int size)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.set_tabsize_sp(screen, size);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "set_tabsize_sp");
        }
        #endregion

        #region use_default_colors_sp
        /// <summary>
        /// see <see cref="NativeNCurses.use_default_colors"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void use_default_colors(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.use_default_colors_sp(screen), "use_default_colors_sp");
        }

        /// <summary>
        /// see <see cref="use_default_colors"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void use_default_colors_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.use_default_colors_sp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "use_default_colors_sp");
        }
        #endregion

        #region use_legacy_coding_sp
        /// <summary>
        /// see <see cref="NativeNCurses.use_legacy_coding"/>
        /// </returns>
        /// <param name="screen">A pointer to a screen</param>
        public static int use_legacy_coding(IntPtr screen, int level)
        {
            return NativeNCurses.NCursesWrapper.use_legacy_coding_sp(screen, level);
        }
        #endregion

        #region nofilter_sp
        /// <summary>
        /// see <see cref="NativeNCurses.nofilter"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void nofilter(IntPtr screen)
        {
            NativeNCurses.NCursesWrapper.nofilter_sp(screen);
        }
        #endregion

        #region new_prescr
        /// <summary>
        /// </summary>
        /// <returns>pointer to a new screen</returns>
        public static IntPtr new_prescr(IntPtr screen)
        {
            return NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.new_prescr(), "new_prescr");
        }
        #endregion

        #region term_attrs_sp
        /// <summary>
        /// see <see cref="NativeNCurses.term_attrs"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static chtype term_attrs(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.term_attrs_sp(screen);
        }
        #endregion

        #region unget_wch_sp
        /// <summary>
        /// see <see cref="NativeNCurses.unget_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void unget_wch(IntPtr screen, char wch)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.unget_wch_sp(screen, wch), "unget_wch_sp");
        }

        /// <summary>
        /// see <see cref="unget_wch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void unget_wch_t(IntPtr screen, char wch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.unget_wch_sp(screen, wch);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "unget_wch_sp");
        }
        #endregion

        #region wunctrl_sp
        /// <summary>
        /// see <see cref="NativeNCurses.wunctrl"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static string wunctrl(IntPtr screen, NCursesWCHAR wch)
        {
            IntPtr wPtr, strPtr = IntPtr.Zero;
            using (wch.ToPointer(out wPtr))
            {
                try
                {
                    strPtr = NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wunctrl_sp(screen, wPtr), "wunctrl");
                    return NativeNCurses.MarshalStringFromNativeWideString(strPtr, Constants.SIZEOF_WCHAR_T * Constants.CCHARW_MAX);
                }
                finally
                {
                    if (strPtr != IntPtr.Zero)
                        Marshal.FreeHGlobal(strPtr);
                }
            }
        }
        #endregion

        #region vid_attr_sp
        /// <summary>
        /// see <see cref="NativeNCurses.vid_attr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vid_attr(IntPtr screen, chtype attrs, short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.vid_attr_sp(screen, attrs, pair, IntPtr.Zero), "vid_attr_sp");
        }

        /// <summary>
        /// see <see cref="vid_attr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void vid_attr_t(IntPtr screen, chtype attrs, short pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.vid_attr_sp(screen, attrs, pair, IntPtr.Zero);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "vid_attr_sp");
        }
        #endregion

        #region vid_puts_sp
        /// <summary>
        /// see <see cref="vid_attr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vid_puts(IntPtr screen, chtype attrs, short pair, Func<int, int> NCURSES_OUTC)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.vid_puts_sp(screen, attrs, pair, IntPtr.Zero,
                Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC)), "vid_puts_sp");
        }

        /// <summary>
        /// see <see cref="vid_puts"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void vid_puts_t(IntPtr screen, chtype attrs, short pair, Func<int, int> NCURSES_OUTC)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.vid_puts_sp(screen, attrs, pair, IntPtr.Zero,
                Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC));
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "vid_puts_sp");
        }
        #endregion

        #region has_mouse_sp
        /// <summary>
        /// see <see cref="NativeNCurses.has_mouse"/>
        /// </summary>
        public static bool has_mouse(IntPtr screen)
        {
            return NativeNCurses.NCursesWrapper.has_mouse_sp(screen);
        }
        #endregion

        #region getmouse_sp
        /// <summary>
        /// see <see cref="NativeNCurses.getmouse"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void getmouse(IntPtr screen, ref MEVENT ev)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.getmouse_sp(screen, ref ev), "getmouse_sp");
        }

        /// <summary>
        /// see <see cref="getmouse"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void getmouse_t(IntPtr screen, ref MEVENT ev)
        {
            IntPtr sPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ev));
            Marshal.StructureToPtr(ev, sPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(ev));

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.getmouse_sp(screen, sPtr);

            try
            {
                NativeNCurses.use_window_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "getmouse_sp");
                Marshal.PtrToStructure(sPtr, ev);
            }
            finally
            {
                Marshal.FreeHGlobal(sPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(ev));
            }
        }
        #endregion

        #region ungetmouse_sp
        /// <summary>
        /// see <see cref="NativeNCurses.ungetmouse"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void ungetmouse(IntPtr screen, MEVENT ev)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.ungetmouse_sp(screen, ev), "ungetmouse_sp");
        }

        /// <summary>
        /// see <see cref="ungetmouse"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void ungetmouse_t(IntPtr screen, MEVENT ev)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.ungetmouse_sp(screen, ev);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "ungetmouse_sp");
        }
        #endregion

        #region mousemask_sp
        /// <summary>
        /// see <see cref="NativeNCurses.mousemask"/>
        /// mouse event mask.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static chtype mousemask(IntPtr screen, chtype newmask, ref chtype? oldmask)
        {
            return NativeNCurses.NCursesWrapper.mousemask_sp(screen, newmask, ref oldmask);
        }
        #endregion

        #region mouseinterval_sp
        /// <summary>
        /// see <see cref="NativeNCurses.mouseinterval"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int mouseinterval(IntPtr screen, int erval)
        {
            return NativeNCurses.NCursesWrapper.mouseinterval_sp(screen, erval);
        }
        #endregion
    }
}
