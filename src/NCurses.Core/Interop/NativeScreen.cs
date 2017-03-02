using System;
using System.Text;
using System.Runtime.InteropServices;

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
        [DllImport(Constants.DLLNAME, EntryPoint = "baudrate_sp")]
        public extern static int baudrate(IntPtr screen);

        /// <summary>
        /// see <see cref="baudrate"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static int baudrate_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => baudrate(screen);
            return NativeNCurses.use_screen(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)));
        }
        #endregion

        #region beep_sp
        /// <summary>
        /// see <see cref="beep"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "beep_sp")]
        internal extern static int ncurses_beep(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.beep"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void beep(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_beep(screen), "beep_sp");
        }

        /// <summary>
        /// <see cref="beep"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void beep_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_beep(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "beep_sp");
        }
        #endregion

        #region can_change_color_sp
        /// <summary>
        /// see <see cref="NativeNCurses.can_change_color"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "can_change_color_sp")]
        public extern static bool can_change_color(IntPtr screen);
        #endregion

        #region cbreak_sp
        /// <summary>
        /// see <see cref="cbreak"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "cbreak_sp")]
        internal extern static int ncurses_cbreak(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.cbreak"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void cbreak(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_cbreak(screen), "cbreak_sp");
        }

        /// <summary>
        /// see <see cref="cbreak"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void cbreak_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_cbreak(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "cbreak_sp");
        }
        #endregion

        #region nocbreak_sp
        /// <summary>
        /// see <see cref="nocbreak"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "nocbreak_sp")]
        internal extern static int ncurses_nocbreak(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.nocbreak"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void nocbreak(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_nocbreak(screen), "nocbreak_sp");
        }

        /// <summary>
        /// see <see cref="nocbreak"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void nocbreak_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_nocbreak(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "nocbreak_sp");
        }
        #endregion

        #region color_content_sp
        /// <summary>
        /// see <see cref="color_content"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "color_content_sp")]
        internal extern static int ncurses_color_content(IntPtr screen, short color, IntPtr red, IntPtr green, IntPtr blue);

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
                NativeNCurses.VerifyNCursesMethod(() => ncurses_color_content(screen, color, rPtr, gPtr, bPtr), "color_content_sp");
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
                Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_color_content(screen, color, rPtr, gPtr, bPtr);
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
        /// see <see cref="curs_set"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "curs_set_sp")]
        internal extern static int ncurses_curs_set(IntPtr screen, int visibility);

        /// <summary>
        /// see <see cref="NativeNCurses.curs_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void curs_set(IntPtr screen, int visibility)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_curs_set(screen, visibility), "curs_set_sp");
        }

        /// <summary>
        /// see <see cref="curs_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void curs_set_t(IntPtr screen, int visibility)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_curs_set(screen, visibility);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "curs_set_sp");
        }
        #endregion

        #region def_prog_mode_sp
        /// <summary>
        /// see <see cref="NativeNCurses.def_prog_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "def_prog_mode_sp")]
        public extern static int def_prog_mode(IntPtr screen);
        #endregion

        #region def_shell_mode_sp
        /// <summary>
        /// see <see cref="NativeNCurses.def_shell_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "def_shell_mode_sp")]
        public extern static int def_shell_mode(IntPtr screen);
        #endregion

        #region delay_output_sp
        /// <summary>
        /// see <see cref="delay_output"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "delay_output_sp")]
        internal extern static int ncurses_delay_output(IntPtr screen, int ms);

        /// <summary>
        /// see <see cref="NativeNCurses.delay_output"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void delay_output(IntPtr screen, int ms)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_delay_output(screen, ms), "delay_output_sp");
        }

        /// <summary>
        /// see <see cref="delay_output"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void delay_output_t(IntPtr screen, int ms)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_delay_output(screen, ms);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "delay_output_sp");
        }
        #endregion

        #region doupdate_sp
        /// <summary>
        /// see <see cref="doupdate"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "doupdate_sp")]
        internal extern static int ncurses_doupdate(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.doupdate"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void doupdate(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_doupdate(screen), "doupdate_sp");
        }

        /// <summary>
        /// see <see cref="doupdate"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void doupdate_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_doupdate(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "doupdate_sp");
        }
        #endregion

        #region echo_sp
        /// <summary>
        /// see <see cref="echo"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "echo_sp")]
        internal extern static int ncurses_echo(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.echo"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void echo(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_echo(screen), "echo_sp");
        }

        /// <summary>
        /// see <see cref="echo"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void echo_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_echo(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "echo_sp");
        }
        #endregion

        #region endwin_sp
        /// <summary>
        /// see <see cref="endwin"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "endwin_sp")]
        internal extern static int ncurses_endwin(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.endwin"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void endwin(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_endwin(screen), "endwin_sp");
        }

        /// <summary>
        /// see <see cref="endwin"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void endwin_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_endwin(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "endwin_sp");
        }
        #endregion

        #region erasechar_sp
        /// <summary>
        /// see <see cref="NativeNCurses.erasechar"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        /// <returns>The user's current erase character</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "erasechar_sp")]
        public extern static char erasechar(IntPtr screen);
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
        [DllImport(Constants.DLLNAME, EntryPoint = "filter_sp")]
        public extern static void filter(IntPtr screen);
        #endregion

        #region flash_sp
        /// <summary>
        /// see <see cref="flash"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "flash_sp")]
        internal extern static int ncurses_flash(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.flash"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void flash(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_flash(screen), "flash_sp");
        }

        /// <summary>
        /// see <see cref="flash"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void flash_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_flash(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "flash_sp");
        }
        #endregion

        #region flushinp_sp
        /// <summary>
        /// see <see cref="flushinp"/>
        /// </summary>
        /// <returns>Constants.ERR on error</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "flushinp_sp")]
        internal extern static int ncurses_flushinp(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.flushinp"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void flushinp(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_flushinp(screen), "flushinp_sp");
        }

        /// <summary>
        /// see <see cref="flushinp"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void flushinp_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_flushinp(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "flushinp_sp");
        }
        #endregion

        #region halfdelay_sp
        /// <summary>
        /// see <see cref="halfdelay"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "halfdelay_sp")]
        internal extern static int ncurses_halfdelay(IntPtr screen, int tenths);

        /// <summary>
        /// see <see cref="NativeNCurses.halfdelay"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void halfdelay(IntPtr screen, int tenths)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_halfdelay(screen, tenths), "halfdelay_sp");
        }

        /// <summary>
        /// see <see cref="halfdelay"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void halfdelay_t(IntPtr screen, int tenths)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_halfdelay(screen, tenths);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "halfdelay_sp");
        }
        #endregion

        #region has_colors_sp
        /// <summary>
        /// see <see cref="NativeNCurses.has_colors"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "has_colors_sp")]
        public extern static bool has_colors(IntPtr screen);
        #endregion

        #region has_ic_sp
        /// <summary>
        /// see <see cref="NativeNCurses.has_ic"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "has_ic_sp")]
        public extern static bool has_ic(IntPtr screen);
        #endregion

        #region has_il_sp
        /// <summary>
        /// see <see cref="NativeNCurses.has_il"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "has_il_sp")]
        public extern static bool has_il(IntPtr screen);
        #endregion

        #region init_color
        /// <summary>
        /// see <see cref="init_color"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "init_color")]
        internal extern static int ncurses_init_color(IntPtr screen, short color, short r, short g, short b);

        /// <summary>
        /// see <see cref="NativeNCurses.init_color"/>
        /// </summary>
        public static void init_color(IntPtr screen, short color, short r, short g, short b)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_init_color(screen, color, r, g, b), "init_color");
        }
        #endregion

        #region init_pair_sp
        /// <summary>
        /// see <see cref="init_pair"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "init_pair_sp")]
        internal extern static int ncurses_init_pair(IntPtr screen, short pair, short f, short b);

        /// <summary>
        /// see <see cref="NativeNCurses.init_pair"/>
        /// </summary>
        public static void init_pair(IntPtr screen, short pair, short f, short b)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_init_pair(screen, pair, f, b), "init_pair_sp");
        }
        #endregion

        #region intrflush_sp
        /// <summary>
        /// see <see cref="intrflush"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "intrflush_sp")]
        internal extern static int ncurses_intrflush(IntPtr screen, IntPtr win, bool bf);

        /// <summary>
        /// see <see cref="NativeNCurses.intrflush"/>
        /// </summary>
        /// <param name="win"></param>
        /// <param name="bf"></param>
        public static void intrflush(IntPtr screen, IntPtr win, bool bf)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_intrflush(screen, win, bf), "intrflush_sp");
        }
        #endregion

        #region isendwin_sp
        /// <summary>
        /// see <see cref="NativeNCurses.isendwin"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "isendwin_sp")]
        public extern static bool isendwin(IntPtr screen);
        #endregion

        #region keyname_sp
        /// <summary>
        /// see <see cref="NativeNCurses.keyname(int)"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "keyname_sp")]
        public extern static string keyname(IntPtr screen, int c);
        #endregion

        #region killchar_sp
        /// <summary>
        /// see <see cref="NativeNCurses.killchar"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "killchar_sp")]
        public extern static char killchar(IntPtr screen);
        #endregion

        #region longname_sp
        /// <summary>
        /// see <see cref="NativeNCurses.longname"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "longname_sp")]
        public extern static string longname(IntPtr screen);
        #endregion

        #region mvcur_sp
        /// <summary>
        /// see <see cref="mvcur(IntPtr, int, int, int, int)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "mvcur_sp")]
        internal extern static int ncurses_mvcur(IntPtr screen, int oldrow, int oldcol, int newrow, int newcol);

        /// <summary>
        /// see <see cref="NativeNCurses.mvcur(int, int, int, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void mvcur(IntPtr screen, int oldrow, int oldcol, int newrow, int newcol)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_mvcur(screen, oldrow, oldcol, newrow, newcol), "mvcur_sp");
        }

        /// <summary>
        /// see <see cref="mvcur(IntPtr, int, int, int, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvcur_t(IntPtr screen, int oldrow, int oldcol, int newrow, int newcol)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_mvcur(screen, oldrow, oldcol, newrow, newcol);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "mvcur_sp");
        }
        #endregion

        #region napms_sp
        /// <summary>
        /// see <see cref="napms"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "napms_sp")]
        internal extern static int ncurses_napms(IntPtr screen, int ms);

        /// <summary>
        /// see <see cref="NativeNCurses.napms"/>
        /// </summary>
        public static void napms(IntPtr screen, int ms)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_napms(screen, ms), "napms_sp");
        }
        #endregion

        #region newpad_sp
        /// <summary>
        /// see <see cref="newpad"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "newpad_sp")]
        internal extern static IntPtr ncurses_newpad(IntPtr screen, int nlines, int ncols);

        /// <summary>
        /// see <see cref="NativeNCurses.newpad"/>
        /// </summary>
        public static IntPtr newpad(IntPtr screen, int nlines, int ncols)
        {
            return NativeNCurses.VerifyNCursesMethod(() => ncurses_newpad(screen, nlines, ncols), "newpad_sp");
        }
        #endregion

        #region newwin_sp
        /// <summary>
        /// see <see cref="newwin"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "newwin_sp")]
        internal extern static IntPtr ncurses_newwin(IntPtr screen, int nlines, int ncols, int begin_y, int begin_x);

        /// <summary>
        /// see <see cref="NativeNCurses.newwin"/>
        /// </summary>
        public static IntPtr newwin(IntPtr screen, int nlines, int ncols, int begin_y, int begin_x)
        {
            return NativeNCurses.VerifyNCursesMethod(() => ncurses_newwin(screen, nlines, ncols, begin_y, begin_x), "newwin_sp");
        }
        #endregion

        #region nl_sp
        /// <summary>
        /// see <see cref="nl(IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "nl_sp")]
        internal extern static int ncurses_nl(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.nl"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void nl(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_nl(screen), "nl_sp");
        }

        /// <summary>
        /// see <see cref="nl(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void nl_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_nl(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "nl_sp");
        }
        #endregion

        #region noecho_sp
        /// <summary>
        /// see <see cref="noecho(IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "noecho_sp")]
        internal extern static int ncurses_noecho(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.noecho"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void noecho(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_noecho(screen), "noecho_sp");
        }

        /// <summary>
        /// see <see cref="noecho(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void noecho_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_noecho(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "noecho_sp");
        }
        #endregion

        #region nonl_sp
        /// <summary>
        /// see <see cref="nonl(IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "nonl_sp")]
        internal extern static int ncurses_nonl(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.nonl"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void nonl(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_nonl(screen), "nonl_sp");
        }

        /// <summary>
        /// see <see cref="nonl(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void nonl_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_nonl(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "nonl_sp");
        }
        #endregion

        #region noqiflush_sp
        /// <summary>
        /// see <see cref="qiflush"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "noqiflush_sp")]
        internal extern static void noqiflush(IntPtr screen);
        #endregion

        #region noraw_sp
        /// <summary>
        /// see <see cref="noraw(IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "noraw_sp")]
        internal extern static int ncurses_noraw(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.raw"/>
        /// <para />native method wrapped with verification.
        /// <param name="screen">A pointer to a screen</param>
        /// </summary>
        public static void noraw(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_noraw(screen), "noraw_sp");
        }

        /// <summary>
        /// see <see cref="noraw(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void noraw_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_noraw(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "noraw_sp");
        }
        #endregion

        #region pair_content_sp
        /// <summary>
        /// see <see cref="pair_content(IntPtr, short, ref short, ref short)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "pair_content_sp")]
        internal extern static int ncurses_pair_content(IntPtr screen, short pair, IntPtr f, IntPtr b);

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
                NativeNCurses.VerifyNCursesMethod(() => ncurses_pair_content(screen, pair, fgPtr, bgPtr), "pair_content_sp");
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
                NativeNCurses.VerifyNCursesMethod(() => ncurses_pair_content(screen, pair, fgPtr, bgPtr), "pair_content_sp");
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
        [DllImport(Constants.DLLNAME, EntryPoint = "qiflush_sp")]
        internal extern static void qiflush(IntPtr screen);
        #endregion

        #region raw_sp
        /// <summary>
        /// see <see cref="raw(IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "raw_sp")]
        internal extern static int ncurses_raw(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.raw"/>
        /// <para />native method wrapped with verification.
        /// <param name="screen">A pointer to a screen</param>
        /// </summary>
        public static void raw(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_raw(screen), "raw_sp");
        }

        /// <summary>
        /// see <see cref="raw(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void raw_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_raw(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "raw_sp");
        }
        #endregion

        #region resetty_sp
        /// <summary>
        /// see <see cref="resetty(IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "resetty_sp")]
        internal extern static int ncurses_resetty(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.resetty"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void resetty(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_resetty(screen), "resetty_sp");
        }

        /// <summary>
        /// see <see cref="resetty(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void resetty_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_resetty(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "resetty_sp");
        }
        #endregion

        #region reset_prog_mode_sp
        /// <summary>
        /// see <see cref="reset_prog_mode(IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "reset_prog_mode_sp")]
        internal extern static int ncurses_reset_prog_mode(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.reset_prog_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void reset_prog_mode(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_reset_prog_mode(screen), "reset_prog_mode_sp");
        }

        /// <summary>
        /// see <see cref="reset_prog_mode(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void reset_prog_mode_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_reset_prog_mode(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "reset_prog_mode_sp");
        }
        #endregion

        #region reset_shell_mode_sp
        /// <summary>
        /// see <see cref="reset_shell_mode(IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "reset_shell_mode_sp")]
        internal extern static int ncurses_reset_shell_mode(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.reset_shell_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void reset_shell_mode(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_reset_shell_mode(screen), "reset_shell_mode_sp");
        }

        /// <summary>
        /// see <see cref="reset_shell_mode(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void reset_shell_mode_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_reset_shell_mode(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "reset_shell_mode_sp");
        }
        #endregion

        #region ripoffline_sp
        /// <summary>
        /// see <see cref="ripoffline(IntPtr, int, Action{IntPtr, int})"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "ripoffline_sp")]
        internal extern static int ncurses_ripoffline(IntPtr screen, int line, IntPtr method);

        /// <summary>
        /// see <see cref="NativeNCurses.ripoffline(int, Action{IntPtr, int})"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="line">a positive or negative integer</param>
        /// <param name="init">a method to be called on initscr (a window pointer and number of columns gets passed)</param>
        public static void ripoffline(IntPtr screen, int line, Action<IntPtr, int> init)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_ripoffline(screen, line, Marshal.GetFunctionPointerForDelegate(init)), "ripoffline_sp");
        }

        /// <summary>
        /// see <see cref="ripoffline(IntPtr, int, Action{IntPtr, int})"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void ripoffline_t(IntPtr screen, int line, Action<IntPtr, int> init)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_ripoffline(screen, line, Marshal.GetFunctionPointerForDelegate(init));
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "ripoffline_sp");
        }
        #endregion

        #region savetty_sp
        /// <summary>
        /// see <see cref="savetty(IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "savetty_sp")]
        internal extern static int ncurses_savetty(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.savetty"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void savetty(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_savetty(screen), "savetty_sp");
        }

        /// <summary>
        /// see <see cref="savetty(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void savetty_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_savetty(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "savetty_sp");
        }
        #endregion

        #region scr_dump_sp
        /// <summary>
        /// see <see cref="scr_dump(IntPtr, string)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "scr_dump_sp")]
        internal extern static int ncurses_scr_dump(IntPtr screen, string filename);

        /// <summary>
        /// see <see cref="src_init(string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void scr_dump(IntPtr screen, string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_scr_dump(screen, filename), "scr_dump_sp");
        }

        /// <summary>
        /// see <see cref="scr_dump(IntPtr, string)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void scr_dump_t(IntPtr screen, string filename)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_scr_dump(screen, filename);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "scr_dump_sp");
        }
        #endregion

        #region scr_init_sp
        /// <summary>
        /// see <see cref="scr_init(IntPtr, string)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "scr_init_sp")]
        internal extern static int ncurses_scr_init(IntPtr screen, string filename);

        /// <summary>
        /// see <see cref="NativeNCurses.scr_init(string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void scr_init(IntPtr screen, string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_scr_init(screen, filename), "scr_init_sp");
        }

        /// <summary>
        /// see <see cref="scr_init(IntPtr, string)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void scr_init_t(IntPtr screen, string filename)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_scr_init(screen, filename);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "scr_init_sp");
        }
        #endregion

        #region scr_restore_sp
        /// <summary>
        /// see <see cref="scr_restore(IntPtr, string)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "scr_restore_sp")]
        internal extern static int ncurses_scr_restore(IntPtr screen, string filename);

        /// <summary>
        /// see <see cref="NativeNCurses.scr_restore(string)"/>
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void scr_restore(IntPtr screen, string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_scr_restore(screen, filename), "scr_restore_sp");
        }

        /// <summary>
        /// see <see cref="scr_restore(IntPtr, string)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void scr_restore_t(IntPtr screen, string filename)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_scr_restore(screen, filename);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "scr_restore_sp");
        }
        #endregion

        #region scr_set_sp
        /// <summary>
        /// see <see cref="scr_set"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "scr_set_sp")]
        internal extern static int ncurses_scr_set(IntPtr screen, string filename);

        /// <summary>
        /// see <see cref="NativeNCurses.scr_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void scr_set(IntPtr screen, string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_scr_set(screen, filename), "src_set_sp");
        }

        /// <summary>
        /// see <see cref="scr_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void scr_set_t(IntPtr screen, string filename)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_scr_set(screen, filename);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "scr_set_sp");
        }
        #endregion

        #region slk_attroff_sp
        /// <summary>
        /// see <see cref="slk_attroff(IntPtr, uint)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attroff_sp")]
        internal extern static int ncurses_slk_attroff(IntPtr screen, uint attrs);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_attroff(uint)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_attroff(IntPtr screen, uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attroff(screen, attrs), "slk_attroff_sp");
        }

        /// <summary>
        /// see <see cref="slk_attroff(IntPtr, uint)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_attroff_t(IntPtr screen, uint attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_attroff(screen, attrs);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_attroff_sp");
        }
        #endregion

        #region slk_attron_sp
        /// <summary>
        /// see <see cref="slk_attron(IntPtr, uint)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attron_sp")]
        internal extern static int ncurses_slk_attron(IntPtr screen, uint attrs);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_attroff(uint)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_attron(IntPtr screen, uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attron(screen, attrs), "slk_attron_sp");
        }

        /// <summary>
        /// see <see cref="slk_attron(IntPtr, uint)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_attron_t(IntPtr screen, uint attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_attron(screen, attrs);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_attron_sp");
        }
        #endregion

        #region slk_attrset_sp
        /// <summary>
        /// see <see cref="slk_attrset(IntPtr, uint, short)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attrset_sp")]
        internal extern static int ncurses_slk_attrset(IntPtr screen, uint attrs, short color_pair, IntPtr opts);

        /// <summary>
        /// see <see cref="slk_attroff(IntPtr, uint)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_attrset(IntPtr screen, uint attrs, short color_pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attrset(screen, attrs, color_pair, IntPtr.Zero), "slk_attrset_sp");
        }

        /// <summary>
        /// see <see cref="slk_attrset(IntPtr, uint, short)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_attrset_t(IntPtr screen, uint attrs, short color_pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_attrset(screen, attrs, color_pair, IntPtr.Zero);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_attrset_sp");
        }
        #endregion

        #region slk_attr_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_attr"/>
        /// </summary>
        /// <returns>an attribute</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attr_sp")]
        public extern static uint slk_attr(IntPtr screen);
        #endregion

        #region slk_attr_set_sp
        /// <summary>
        /// see <see cref="slk_attr_set(IntPtr, uint, short)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attr_set_sp")]
        internal extern static int ncurses_slk_attr_set(IntPtr screen, uint attrs, short color_pair, IntPtr opts);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_attr_set(uint, short)"/> (for soft function keys)
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_attr_set(IntPtr screen, uint attrs, short color_pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attr_set(screen, attrs, color_pair, IntPtr.Zero), "slk_attr_set_sp");
        }

        /// <summary>
        /// see <see cref="slk_attr_set(IntPtr, uint, short)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_attr_set_t(IntPtr screen, uint attrs, short color_pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_attr_set(screen, attrs, color_pair, IntPtr.Zero);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_attr_set_sp");
        }
        #endregion

        #region slk_clear_sp
        /// <summary>
        /// see <see cref="slk_clear"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_clear_sp")]
        internal extern static int ncurses_slk_clear(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_clear"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_clear(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_clear(screen), "slk_clear_sp");
        }

        /// <summary>
        /// see <see cref="slk_clear"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_clear_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_clear(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_clear_sp");
        }
        #endregion

        #region slk_color_sp
        /// <summary>
        /// see <see cref="slk_color(IntPtr, short)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_color_sp")]
        internal extern static int ncurses_slk_color(IntPtr screen, short color_pair);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_color(short)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_color(IntPtr screen, short color_pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_color(screen, color_pair), "slk_color_sp");
        }

        /// <summary>
        /// see <see cref="slk_color(IntPtr, short)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_color_t(IntPtr screen, short color_pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_color(screen, color_pair);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_color_sp");
        }
        #endregion

        #region slk_init_sp
        /// <summary>
        /// see <see cref="slk_init(IntPtr, int)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_init_sp")]
        internal extern static int ncurses_slk_init(IntPtr screen, int fmt);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_init(int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_init(IntPtr screen, int fmt)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_init(screen, fmt), "slk_init_sp");
        }

        /// <summary>
        /// see <see cref="slk_init(IntPtr, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_init_t(IntPtr screen, int fmt)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_init(screen, fmt);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_init_sp");
        }
        #endregion

        #region slk_label_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_label(int)"/>
        /// </summary>
        /// <param name="labnum">number of the label for which you want to return the label</param>
        /// <returns>label</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_label_sp")]
        public extern static string slk_label(int labnum);
        #endregion

        #region slk_noutrefresh_sp
        /// <summary>
        /// see <see cref="slk_noutrefresh"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_noutrefresh_sp")]
        internal extern static int ncurses_slk_noutrefresh(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_noutrefresh"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_noutrefresh(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_noutrefresh(screen), "slk_noutrefresh_sp");
        }

        /// <summary>
        /// see <see cref="slk_noutrefresh"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_noutrefresh_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_noutrefresh(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_noutrefresh_sp");
        }
        #endregion

        #region slk_refresh_sp
        /// <summary>
        /// see <see cref="slk_refresh"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_refresh_sp")]
        internal extern static int ncurses_slk_refresh(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_refresh"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_refresh(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_refresh(screen), "slk_refresh_sp");
        }

        /// <summary>
        /// see <see cref="slk_refresh"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_refresh_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_refresh(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_refresh_sp");
        }
        #endregion

        #region slk_restore_sp
        /// <summary>
        /// see <see cref="slk_restore"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_restore_sp")]
        internal extern static int ncurses_slk_restore(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_restore"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_restore(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_restore(screen), "slk_restore_sp");
        }

        /// <summary>
        /// see <see cref="slk_restore"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_restore_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_restore(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_restore_sp");
        }
        #endregion

        #region slk_set_sp
        /// <summary>
        /// see <see cref="slk_set"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_set_sp")]
        internal extern static int ncurses_slk_set(IntPtr screen, int labnum, string label, int fmt);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_set(int, string, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_set(IntPtr screen, int labnum, string label, int fmt)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_set(screen, labnum, label, fmt), "slk_set_sp");
        }

        /// <summary>
        /// see <see cref="slk_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_set_t(IntPtr screen, int labnum, string label, int fmt)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_set(screen, labnum, label, fmt);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_set_sp");
        }
        #endregion

        #region slk_touch_sp
        /// <summary>
        /// see <see cref="slk_touch"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_touch_sp")]
        internal extern static int ncurses_slk_touch(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.slk_touch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_touch(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_touch(screen), "slk_touch_sp");
        }

        /// <summary>
        /// see <see cref="slk_touch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void slk_touch_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_slk_touch(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "slk_touch_sp");
        }
        #endregion

        #region start_color_sp
        /// <summary>
        /// see <see cref="start_color"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "start_color_sp")]
        internal extern static int ncurses_start_color(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.start_color"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void start_color(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_start_color(screen), "start_color_sp");
        }

        /// <summary>
        /// see <see cref="start_color"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void start_color_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_start_color(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "start_color_sp");
        }
        #endregion

        #region termattrs_sp
        /// <summary>
        /// see <see cref="NativeNCurses.termattrs"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "termattrs_sp")]
        public extern static uint termattrs(IntPtr screen);
        #endregion

        #region termname_sp
        /// <summary>
        /// see <see cref="NativeNCurses.termname"/>
        /// </summary>
        /// <returns>the terminal name</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "termname_sp")]
        public extern static string termname(IntPtr screen);
        #endregion

        #region typeahead_sp
        /// <summary>
        /// see <see cref="typeahead"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "typeahead_sp")]
        internal extern static int ncurses_typeahead(IntPtr screen, int fd);

        /// <summary>
        /// see <see cref="NativeNCurses.typeahead"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void typeahead(IntPtr screen, int fd)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_typeahead(screen, fd), "typeahead_sp");
        }

        /// <summary>
        /// see <see cref="typeahead"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void typeahead_t(IntPtr screen, int fd)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_typeahead(screen, fd);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "typeahead_sp");
        }
        #endregion

        #region ungetch_sp
        /// <summary>
        /// see <see cref="ungetch"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "ungetch_sp")]
        internal extern static int ncurses_ungetch(IntPtr screen, int ch);

        /// <summary>
        /// see <see cref="NativeNCurses.ungetch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void ungetch(IntPtr screen, int ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_ungetch(screen, ch), "ungetch_sp");
        }

        /// <summary>
        /// see <see cref="ungetch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void ungetch_t(IntPtr screen, int ch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_ungetch(screen, ch);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "ungetch_sp");
        }
        #endregion

        #region use_env_sp
        /// <summary>
        /// see <see cref="NativeNCurses.use_env"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_env_sp")]
        public extern static void use_env(IntPtr screen, bool f);
        #endregion

        #region use_tioctl_sp
        /// <summary>
        /// see <see cref="NativeNCurses.use_tioctl"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_tioctl_sp")]
        public extern static void use_tioctl(IntPtr screen, bool f);
        #endregion

        #region vidattr_sp
        /// <summary>
        /// see <see cref="vidattr"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "vidattr_sp")]
        internal extern static int ncurses_vidattr(IntPtr screen, uint attrs);

        /// <summary>
        /// see <see cref="NativeNCurses.vidattr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vidattr(IntPtr screen, uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_vidattr(screen, attrs), "vidattr_sp");
        }

        /// <summary>
        /// see <see cref="vidattr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void vidattr_t(IntPtr screen, uint attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_vidattr(screen, attrs);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "vidattr_sp");
        }
        #endregion

        #region vidputs_sp
        /// <summary>
        /// see <see cref="vidputs"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "vidputs_sp")]
        internal extern static int ncurses_vidputs(IntPtr screen, uint attrs, IntPtr NCURSES_OUTC);

        /// <summary>
        /// see <see cref="NativeNCurses.vidputs"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vidputs(IntPtr screen, uint attrs, Func<int, int> NCURSES_OUTC)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_vidputs(screen, attrs, Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC)), "vidputs_sp");
        }

        /// <summary>
        /// see <see cref="vidputs"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void vidputs_t(IntPtr screen, uint attrs, Func<int, int> NCURSES_OUTC)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_vidputs(screen, attrs, Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC));
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "vidputs_sp");
        }
        #endregion

        #region tigetflag_sp
        /// <summary>
        /// see <see cref="NativeNCurses.tigetflag"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "tigetflag_sp")]
        public extern static int tigetflag(IntPtr screen, string capname);
        #endregion

        #region tigetnum_sp
        /// <summary>
        /// see <see cref="NativeNCurses.tigetnum"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "tigetnum_sp")]
        public extern static int tigetnum(IntPtr screen, string capname);
        #endregion

        #region tigetstr_sp
        /// <summary>
        /// see <see cref="NativeNCurses.tigetstr"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "tigetstr_sp")]
        public extern static string tigetstr(IntPtr screen, string capname);
        #endregion

        #region putp_sp
        /// <summary>
        /// see <see cref="putp"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "putp_sp")]
        internal extern static int ncurses_putp(IntPtr screen, string str);

        /// <summary>
        /// see <see cref="NativeNCurses.putp"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void putp(IntPtr screen, string str)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_putp(screen, str), "putp_sp");
        }

        /// <summary>
        /// see <see cref="putp"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void putp_t(IntPtr screen, string str)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_putp(screen, str);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "putp_sp");
        }
        #endregion

        #region is_term_resized_sp
        /// <summary>
        /// see <see cref="NativeNCurses.is_term_resized"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "is_term_resized_sp")]
        internal extern static bool is_term_resized(IntPtr screen, int lines, int columns);
        #endregion

        #region keybound_sp
        /// <summary>
        /// see <see cref="NativeNCurses.keybound"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "keybound_sp")]
        internal extern static string keybound(IntPtr screen, int keycode, int count);
        #endregion

        #region assume_default_colors_sp
        /// <summary>
        /// see <see cref="assume_default_colors"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "assume_default_colors_sp")]
        internal extern static int ncurses_assume_default_colors(IntPtr screen, int fg, int bg);

        /// <summary>
        /// see <see cref="NativeNCurses.assume_default_colors"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void assume_default_colors(IntPtr screen, int fg, int bg)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_assume_default_colors(screen, fg, bg), "assume_default_colors_sp");
        }

        /// <summary>
        /// see <see cref="assume_default_colors"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void assume_default_colors_t(IntPtr screen, int fg, int bg)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_assume_default_colors(screen, fg, bg);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "assume_default_colors_sp");
        }
        #endregion

        #region define_key_sp
        /// <summary>
        /// see <see cref="define_key"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "define_key_sp")]
        internal extern static int ncurses_define_key(IntPtr screen, string definition, int keycode);

        /// <summary>
        /// see <see cref="NativeNCurses.define_key"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void define_key(IntPtr screen, string definition, int keycode)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_define_key(screen, definition, keycode), "define_key_sp");
        }

        /// <summary>
        /// see <see cref="define_key"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void define_key_t(IntPtr screen, string definition, int keycode)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_define_key(screen, definition, keycode);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "define_key_sp");
        }
        #endregion

        #region get_escdelay_sp
        /// <summary>
        /// see <see cref="NativeNCurses.get_escdelay"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "get_escdelay_sp")]
        public extern static int get_escdelay(IntPtr screen);
        #endregion

        #region key_defined_sp
        /// <summary>
        /// see <see cref="NativeNCurses.key_defined"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "key_defined_sp")]
        public extern static int key_defined(IntPtr screen, string definition);
        #endregion

        #region keyok_sp
        /// <summary>
        /// see <see cref="NativeNCurses.keyok"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "keyok_sp")]
        public extern static int keyok(IntPtr screen, int keycode, bool enable);
        #endregion

        #region resize_term_sp
        /// <summary>
        /// see <see cref="resize_term"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "resize_term_sp")]
        internal extern static int ncurses_resize_term(IntPtr screen, int lines, int columns);

        /// <summary>
        /// see <see cref="NativeNCurses.resize_term"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void resize_term(IntPtr screen, int lines, int columns)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_resize_term(screen, lines, columns), "resize_term_sp");
        }

        /// <summary>
        /// see <see cref="resize_term"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void resize_term_t(IntPtr screen, int lines, int columns)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_resize_term(screen, lines, columns);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "resize_term_sp");
        }
        #endregion

        #region resizeterm_sp
        /// <summary>
        /// see <see cref="resizeterm"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "resizeterm_sp")]
        internal extern static int ncurses_resizeterm(IntPtr screen, int lines, int columns);

        /// <summary>
        /// see <see cref="NativeNCurses.resizeterm"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void resizeterm(IntPtr screen, int lines, int columns)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_resizeterm(screen, lines, columns), "resizeterm_sp");
        }

        /// <summary>
        /// see <see cref="resizeterm"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void resizeterm_t(IntPtr screen, int lines, int columns)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_resizeterm(screen, lines, columns);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "resizeterm_sp");
        }
        #endregion

        #region set_escdelay_sp
        /// <summary>
        /// see <see cref="set_escdelay"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "set_escdelay_sp")]
        internal extern static int ncurses_set_escdelay(IntPtr screen, int size);

        /// <summary>
        /// see <see cref="NativeNCurses.set_escdelay"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void set_escdelay(IntPtr screen, int size)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_set_escdelay(screen, size), "set_escdelay_sp");
        }

        /// <summary>
        /// see <see cref="set_escdelay"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void set_escdelay_t(IntPtr screen, int size)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_set_escdelay(screen, size);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "set_escdelay_sp");
        }
        #endregion

        #region set_tabsize_sp
        /// <summary>
        /// see <see cref="set_escdelay"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "set_tabsize_sp")]
        internal extern static int ncurses_set_tabsize(IntPtr screen, int size);

        /// <summary>
        /// see <see cref="NativeNCurses.set_tabsize"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void _set_tabsize(IntPtr screen, int size)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_set_tabsize(screen, size), "set_tabsize_sp");
        }

        /// <summary>
        /// see <see cref="set_escdelay"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void _set_tabsize_t(IntPtr screen, int size)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_set_tabsize(screen, size);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "set_tabsize_sp");
        }
        #endregion

        #region use_default_colors_sp
        /// <summary>
        /// see <see cref="use_default_colors"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_default_colors_sp")]
        internal extern static int ncurses_use_default_colors(IntPtr screen);

        /// <summary>
        /// see <see cref="NativeNCurses.use_default_colors"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void use_default_colors(IntPtr screen)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_use_default_colors(screen), "use_default_colors_sp");
        }

        /// <summary>
        /// see <see cref="use_default_colors"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void use_default_colors_t(IntPtr screen)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_use_default_colors(screen);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "use_default_colors_sp");
        }
        #endregion

        #region use_legacy_coding_sp
        /// <summary>
        /// see <see cref="NativeNCurses.use_legacy_coding"/>
        /// </returns>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_legacy_coding_sp")]
        public extern static int use_legacy_coding(IntPtr screen, int level);
        #endregion

        #region nofilter_sp
        /// <summary>
        /// see <see cref="NativeNCurses.nofilter"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "nofilter_sp")]
        public extern static void nofilter(IntPtr screen);
        #endregion

        #region new_prescr
        /// <summary>
        /// see <see cref="new_prescr"/>
        /// </summary>
        /// <returns></returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "new_prescr")]
        public extern static IntPtr ncurses_new_prescr();

        /// <summary>
        /// </summary>
        /// <returns>pointer to a new screen</returns>
        public static IntPtr new_prescr(IntPtr screen)
        {
            return NativeNCurses.VerifyNCursesMethod(() => ncurses_new_prescr(), "new_prescr");
        }
        #endregion

        #region term_attrs_sp
        /// <summary>
        /// see <see cref="NativeNCurses.term_attrs"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "term_attrs_sp")]
        public extern static uint term_attrs(IntPtr screen);
        #endregion

        #region unget_wch_sp
        /// <summary>
        /// see <see cref="unget_wch"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "unget_wch_sp")]
        internal extern static int ncurses_unget_wch(IntPtr screen, char wch);

        /// <summary>
        /// see <see cref="NativeNCurses.unget_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void unget_wch(IntPtr screen, char wch)
        {
            NCursesException.Verify(ncurses_unget_wch(screen, wch), "unget_wch_sp");
        }

        /// <summary>
        /// see <see cref="unget_wch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void unget_wch_t(IntPtr screen, char wch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_unget_wch(screen, wch);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "unget_wch_sp");
        }
        #endregion

        #region wunctrl_sp
        /// <summary>
        /// see <see cref="NativeNCurses.wunctrl"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "wunctrl")]
        internal extern static string wunctrl(IntPtr screen, NCURSES_CH_T ch);
        #endregion

        #region vid_attr_sp
        /// <summary>
        /// see <see cref="vid_attr"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "vid_attr_sp")]
        internal extern static int ncurses_vid_attr(IntPtr screen, uint attrs, short pair, IntPtr opts);

        /// <summary>
        /// see <see cref="NativeNCurses.vid_attr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vid_attr(IntPtr screen, uint attrs, short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_vid_attr(screen, attrs, pair, IntPtr.Zero), "vid_attr_sp");
        }

        /// <summary>
        /// see <see cref="vid_attr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void vid_attr_t(IntPtr screen, uint attrs, short pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_vid_attr(screen, attrs, pair, IntPtr.Zero);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "vid_attr_sp");
        }
        #endregion

        #region vid_puts_sp
        /// <summary>
        /// see <see cref="vid_puts"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "vid_puts_sp")]
        internal extern static int ncurses_vid_puts(IntPtr screen, uint attrs, short pair, IntPtr opts, IntPtr NCURSES_OUTC);

        /// <summary>
        /// see <see cref="vid_attr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vid_puts(IntPtr screen, uint attrs, short pair, Func<int, int> NCURSES_OUTC)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_vid_puts(screen, attrs, pair, IntPtr.Zero,
                Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC)), "vid_puts_sp");
        }

        /// <summary>
        /// see <see cref="vid_puts"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void vid_puts_t(IntPtr screen, uint attrs, short pair, Func<int, int> NCURSES_OUTC)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_vid_puts(screen, attrs, pair, IntPtr.Zero,
                Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC));
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "vid_puts_sp");
        }
        #endregion

        #region has_mouse_sp
        /// <summary>
        /// see <see cref="NativeNCurses.has_mouse"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "has_mouse_sp")]
        public extern static bool has_mouse(IntPtr screen);
        #endregion

        #region getmouse_sp
        /// <summary>
        /// see <see cref="getmouse"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "getmouse_sp")]
        internal extern static int ncurses_getmouse(IntPtr screen, ref MEVENT ev);

        /// <summary>
        /// see <see cref="getmouse"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "getmouse_sp")]
        internal extern static int ncurses_getmouse(IntPtr screen, IntPtr ev);

        /// <summary>
        /// see <see cref="NativeNCurses.getmouse"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void getmouse(IntPtr screen, ref MEVENT ev)
        {
            NCursesException.Verify(ncurses_getmouse(screen, ref ev), "getmouse_sp");
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

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_getmouse(screen, sPtr);

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
        /// see <see cref="ungetmouse"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "ungetmouse_sp")]
        internal extern static int ncurses_ungetmouse(IntPtr screen, MEVENT ev);

        /// <summary>
        /// see <see cref="NativeNCurses.ungetmouse"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void ungetmouse(IntPtr screen, MEVENT ev)
        {
            NCursesException.Verify(ncurses_ungetmouse(screen, ev), "ungetmouse_sp");
        }

        /// <summary>
        /// see <see cref="ungetmouse"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void ungetmouse_t(IntPtr screen, MEVENT ev)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_ungetmouse(screen, ev);
            NativeNCurses.use_screen_v(screen, Marshal.GetFunctionPointerForDelegate(new NCURSES_SCREEN_CB(callback)), "ungetmouse_sp");
        }
        #endregion

        #region mousemask_sp
        /// <summary>
        /// see <see cref="NativeNCurses.mousemask"/>
        /// mouse event mask.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "mousemask_sp")]
        public extern static uint ncurses_mousemask(IntPtr screen, uint newmask, ref uint? oldmask);
        #endregion

        #region mouseinterval_sp
        /// <summary>
        /// see <see cref="NativeNCurses.mouseinterval"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "mouseinterval_sp")]
        public extern static int mouseinterval(IntPtr screen, int erval);
        #endregion
    }
}
