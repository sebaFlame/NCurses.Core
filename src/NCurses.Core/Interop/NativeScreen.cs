using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.SingleByteString;
using NCurses.Core.Interop.MultiByteString;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop
{
    /// <summary>
    /// native screen methods (wrapped into *_sp when ncurses was compiled with NCURSES_SP_FUNCS).
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    internal static class NativeScreen
    {
        #region Custom type wrapper fields
        private static INativeScreenWide wideCursesWrapper;
        private static INativeScreenWide WideCursesWrapper => NativeNCurses.HasUnicodeSupport
              ? wideCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage)
              : throw new InvalidOperationException(Constants.NoUnicodeExceptionMessage);
        private static INativeScreenWideStr wideStrCursesWrapper;
        private static INativeScreenWideStr WideStrCursesWrapper => NativeNCurses.HasUnicodeSupport
              ? wideStrCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage)
              : throw new InvalidOperationException(Constants.NoUnicodeExceptionMessage);

        private static INativeScreenSmall smallCursesWrapper;
        private static INativeScreenSmall SmallCursesWrapper => smallCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);
        private static INativeScreenSmallStr smallStrCursesWrapper;
        private static INativeScreenSmallStr SmallStrCursesWrapper => smallStrCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);
        #endregion

        #region custom type initialization
        internal static void CreateCharCustomWrappers()
        {
            if (DynamicTypeBuilder.schar is null)
                throw new InvalidOperationException("Custom types haven't been generated yet.");

            Type customType;
            if (smallStrCursesWrapper is null)
            {
                customType = typeof(NativeScreenSmallStr<>).MakeGenericType(DynamicTypeBuilder.schar);
                smallStrCursesWrapper = (INativeScreenSmallStr)Activator.CreateInstance(customType);
            }
        }

        internal static void CreateCustomTypeWrappers()
        {
            if ((DynamicTypeBuilder.chtype is null
                || DynamicTypeBuilder.schar is null)
                || (NativeNCurses.HasUnicodeSupport
                    && (DynamicTypeBuilder.cchar_t is null || DynamicTypeBuilder.wchar_t is null)))
                throw new InvalidOperationException("Custom types haven't been generated yet.");

            Type customType;
            if (NativeNCurses.HasUnicodeSupport)
            {
                if (wideCursesWrapper is null)
                {
                    customType = typeof(NativeScreenWide<,,,,>).MakeGenericType(DynamicTypeBuilder.cchar_t, DynamicTypeBuilder.wchar_t, 
                        DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar, DynamicTypeBuilder.MEVENT);
                    wideCursesWrapper = (INativeScreenWide)Activator.CreateInstance(customType);
                }

                if (wideStrCursesWrapper is null)
                {
                    customType = typeof(NativeScreenWideStr<,>).MakeGenericType(DynamicTypeBuilder.wchar_t, DynamicTypeBuilder.schar);
                    wideStrCursesWrapper = (INativeScreenWideStr)Activator.CreateInstance(customType);
                }
            }

            if (smallCursesWrapper is null)
            {
                customType = typeof(NativeScreenSmall<,,>).MakeGenericType(DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar, DynamicTypeBuilder.MEVENT);
                smallCursesWrapper = (INativeScreenSmall)Activator.CreateInstance(customType);
            }
        }
        #endregion

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
        #endregion

        #region beep_sp
        /// <summary>
        /// see <see cref="NativeNCurses.beep"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void beep(IntPtr screen)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.beep_sp(screen), "beep_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.cbreak_sp(screen), "cbreak_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.nocbreak_sp(screen), "nocbreak_sp");
        }
        #endregion

        #region color_content_sp
        /// <summary>
        /// see <see cref="NativeNCurses.color_content"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static unsafe void color_content(IntPtr screen, short color, ref short red, ref short green, ref short blue)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.color_content_sp(screen, color, ref red, ref green, ref blue), "color_content_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.curs_set_sp(screen, visibility), "curs_set_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.delay_output_sp(screen, ms), "delay_output_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.doupdate_sp(screen), "doupdate_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.echo_sp(screen), "echo_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.endwin_sp(screen), "endwin_sp");
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
            return SmallStrCursesWrapper.erasechar_sp(screen);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.flash_sp(screen), "flash_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.flushinp_sp(screen), "flushinp_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.halfdelay_sp(screen, tenths), "halfdelay_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.init_color_sp(screen, color, r, g, b), "init_color");
        }
        #endregion

        #region init_pair_sp
        /// <summary>
        /// see <see cref="NativeNCurses.init_pair"/>
        /// </summary>
        public static void init_pair(IntPtr screen, short pair, short f, short b)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.init_pair_sp(screen, pair, f, b), "init_pair_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.intrflush_sp(screen, win, bf), "intrflush_sp");
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
            return SmallStrCursesWrapper.keyname_sp(screen, c);
        }
        #endregion

        #region killchar_sp
        /// <summary>
        /// see <see cref="NativeNCurses.killchar"/>
        /// </summary>
        public static char killchar(IntPtr screen)
        {
            return SmallStrCursesWrapper.killchar_sp(screen);
        }
        #endregion

        #region longname_sp
        /// <summary>
        /// see <see cref="NativeNCurses.longname"/>
        /// </summary>
        public static string longname(IntPtr screen)
        {
            return SmallStrCursesWrapper.longname_sp(screen);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.mvcur_sp(screen, oldrow, oldcol, newrow, newcol), "mvcur_sp");
        }
        #endregion

        #region napms_sp
        /// <summary>
        /// see <see cref="NativeNCurses.napms"/>
        /// </summary>
        public static void napms(IntPtr screen, int ms)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.napms_sp(screen, ms), "napms_sp");
        }
        #endregion

        #region newpad_sp
        /// <summary>
        /// see <see cref="NativeNCurses.newpad"/>
        /// </summary>
        public static IntPtr newpad(IntPtr screen, int nlines, int ncols)
        {
            return NCursesException.Verify(NativeNCurses.NCursesWrapper.newpad_sp(screen, nlines, ncols), "newpad_sp");
        }
        #endregion

        #region newwin_sp
        /// <summary>
        /// see <see cref="NativeNCurses.newwin"/>
        /// </summary>
        public static IntPtr newwin(IntPtr screen, int nlines, int ncols, int begin_y, int begin_x)
        {
            return NCursesException.Verify(NativeNCurses.NCursesWrapper.newwin_sp(screen, nlines, ncols, begin_y, begin_x), "newwin_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.nl_sp(screen), "nl_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.noecho_sp(screen), "noecho_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.nonl_sp(screen), "nonl_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.noraw_sp(screen), "noraw_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.pair_content_sp(screen, pair, ref fg, ref bg), "pair_content_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.raw_sp(screen), "raw_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.resetty_sp(screen), "resetty_sp");
        }
        #endregion

        #region reset_prog_mode_sp
        /// <summary>
        /// see <see cref="NativeNCurses.reset_prog_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void reset_prog_mode(IntPtr screen)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.reset_prog_mode_sp(screen), "reset_prog_mode_sp");
        }
        #endregion

        #region reset_shell_mode_sp
        /// <summary>
        /// see <see cref="NativeNCurses.reset_shell_mode"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void reset_shell_mode(IntPtr screen)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.reset_shell_mode_sp(screen), "reset_shell_mode_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.ripoffline_sp(screen, line, Marshal.GetFunctionPointerForDelegate(init)), "ripoffline_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.savetty_sp(screen), "savetty_sp");
        }
        #endregion

        #region scr_dump_sp
        /// <summary>
        /// see <see cref="src_init(string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void scr_dump(IntPtr screen, in string filename)
        {
            SmallStrCursesWrapper.scr_dump_sp(screen, filename);
        }
        #endregion

        #region scr_init_sp
        /// <summary>
        /// see <see cref="NativeNCurses.scr_init(string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void scr_init(IntPtr screen, in string filename)
        {
            SmallStrCursesWrapper.scr_init_sp(screen, filename);
        }
        #endregion

        #region scr_restore_sp
        /// <summary>
        /// see <see cref="NativeNCurses.scr_restore(string)"/>
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void scr_restore(IntPtr screen, in string filename)
        {
            SmallStrCursesWrapper.scr_restore_sp(screen, filename);
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
            SmallStrCursesWrapper.scr_set_sp(screen, filename);
        }
        #endregion

        #region slk_attroff_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_attroff(chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_attroff(IntPtr screen, ulong attrs)
        {
            SmallCursesWrapper.slk_attroff_sp(screen, attrs);
        }
        #endregion

        #region slk_attron_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_attroff(chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_attron(IntPtr screen, ulong attrs)
        {
            SmallCursesWrapper.slk_attron_sp(screen, attrs);
        }
        #endregion

        #region slk_attrset_sp
        /// <summary>
        /// see <see cref="slk_attroff(IntPtr, chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// /// <param name="screen">A pointer to a screen</param>
        public static void slk_attrset(IntPtr screen, ulong attrs)
        {
            SmallCursesWrapper.slk_attrset_sp(screen, attrs);
        }
        #endregion

        #region slk_attr_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_attr"/>
        /// </summary>
        /// <returns>an attribute</returns>
        public static ulong slk_attr(IntPtr screen)
        {
            return SmallCursesWrapper.slk_attr_sp(screen);
        }
        #endregion

        #region slk_attr_set_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_attr_set(chtype, short)"/> (for soft function keys)
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_attr_set(IntPtr screen, ulong attrs, short color_pair)
        {
            SmallCursesWrapper.slk_attr_set_sp(screen, attrs, color_pair);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.slk_clear_sp(screen), "slk_clear_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.slk_color_sp(screen, color_pair), "slk_color_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.slk_init_sp(screen, fmt), "slk_init_sp");
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
            return SmallStrCursesWrapper.slk_label_sp(screen, labnum);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.slk_noutrefresh_sp(screen), "slk_noutrefresh_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.slk_refresh_sp(screen), "slk_refresh_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.slk_restore_sp(screen), "slk_restore_sp");
        }
        #endregion

        #region slk_set_sp
        /// <summary>
        /// see <see cref="NativeNCurses.slk_set(int, string, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void slk_set(IntPtr screen, int labnum, in string label, int fmt)
        {
            SmallStrCursesWrapper.slk_set_sp(screen, labnum, label, fmt);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.slk_touch_sp(screen), "slk_touch_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.start_color_sp(screen), "start_color_sp");
        }
        #endregion

        #region termattrs_sp
        /// <summary>
        /// see <see cref="NativeNCurses.termattrs"/>
        /// </summary>
        public static ulong termattrs(IntPtr screen)
        {
            return SmallCursesWrapper.termattrs_sp(screen);
        }
        #endregion

        #region termname_sp
        /// <summary>
        /// see <see cref="NativeNCurses.termname"/>
        /// </summary>
        /// <returns>the terminal name</returns>
        public static string termname(IntPtr screen)
        {
            return SmallStrCursesWrapper.termname_sp(screen);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.typeahead_sp(screen, fd), "typeahead_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.ungetch_sp(screen, ch), "ungetch_sp");
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
        public static void vidattr(IntPtr screen, ulong attrs)
        {
            SmallCursesWrapper.vidattr_sp(screen, attrs);
        }
        #endregion

        #region vidputs_sp
        /// <summary>
        /// see <see cref="NativeNCurses.vidputs"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vidputs(IntPtr screen, ulong attrs, Func<int, int> NCURSES_OUTC)
        {
            SmallCursesWrapper.vidputs_sp(screen, attrs, NCURSES_OUTC);
        }
        #endregion

        #region tigetflag_sp
        /// <summary>
        /// see <see cref="NativeNCurses.tigetflag"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int tigetflag(IntPtr screen, in string capname)
        {
            return SmallStrCursesWrapper.tigetflag_sp(screen, capname);
        }
        #endregion

        #region tigetnum_sp
        /// <summary>
        /// see <see cref="NativeNCurses.tigetnum"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int tigetnum(IntPtr screen, in string capname)
        {
            return SmallStrCursesWrapper.tigetnum_sp(screen, capname);
        }
        #endregion

        #region tigetstr_sp
        /// <summary>
        /// see <see cref="NativeNCurses.tigetstr"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static int tigetstr(IntPtr screen, in string capname)
        {
            return SmallStrCursesWrapper.tigetstr_sp(screen, capname);
        }
        #endregion

        #region putp_sp
        /// <summary>
        /// see <see cref="NativeNCurses.putp"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void putp(IntPtr screen, in string str)
        {
            SmallStrCursesWrapper.putp_sp(screen, str);
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
            return SmallStrCursesWrapper.keybound_sp(screen, keycode, count);
        }
        #endregion

        #region assume_default_colors_sp
        /// <summary>
        /// see <see cref="NativeNCurses.assume_default_colors"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void assume_default_colors(IntPtr screen, int fg, int bg)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.assume_default_colors_sp(screen, fg, bg), "assume_default_colors_sp");
        }
        #endregion

        #region define_key_sp
        /// <summary>
        /// see <see cref="NativeNCurses.define_key"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void define_key(IntPtr screen, in string definition, int keycode)
        {
            SmallStrCursesWrapper.define_key_sp(screen, definition, keycode);
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
            return SmallStrCursesWrapper.key_defined_sp(screen, definition);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.resize_term_sp(screen, lines, columns), "resize_term_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.resizeterm_sp(screen, lines, columns), "resizeterm_sp");
        }
        #endregion

        #region set_escdelay_sp
        /// <summary>
        /// see <see cref="NativeNCurses.set_escdelay"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void set_escdelay(IntPtr screen, int size)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.set_escdelay_sp(screen, size), "set_escdelay_sp");
        }
        #endregion

        #region set_tabsize_sp
        /// <summary>
        /// see <see cref="NativeNCurses.set_tabsize"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void _set_tabsize(IntPtr screen, int size)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.set_tabsize_sp(screen, size), "set_tabsize_sp");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.use_default_colors_sp(screen), "use_default_colors_sp");
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
            return NCursesException.Verify(NativeNCurses.NCursesWrapper.new_prescr(), "new_prescr");
        }
        #endregion

        #region term_attrs_sp
        /// <summary>
        /// see <see cref="NativeNCurses.term_attrs"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static ulong term_attrs(IntPtr screen)
        {
            return SmallCursesWrapper.term_attrs_sp(screen);
        }
        #endregion

        #region unget_wch_sp
        /// <summary>
        /// see <see cref="NativeNCurses.unget_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void unget_wch(IntPtr screen, in char wch)
        {
            WideStrCursesWrapper.unget_wch_sp(screen, wch);
        }
        #endregion

        #region wunctrl_sp
        /// <summary>
        /// see <see cref="NativeNCurses.wunctrl"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void wunctrl(IntPtr screen, in INCursesWCHAR wch, out string str)
        {
            WideCursesWrapper.wunctrl_sp(screen, wch, out str);
        }
        #endregion

        #region vid_attr_sp
        /// <summary>
        /// see <see cref="NativeNCurses.vid_attr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vid_attr(IntPtr screen, ulong attrs, short pair)
        {
            SmallCursesWrapper.vid_attr_sp(screen, attrs, pair);
        }
        #endregion

        #region vid_puts_sp
        /// <summary>
        /// see <see cref="vid_attr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void vid_puts(IntPtr screen, ulong attrs, short pair, Func<int, int> NCURSES_OUTC)
        {
            SmallCursesWrapper.vid_puts_sp(screen, attrs, pair, NCURSES_OUTC);
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
        public static void getmouse(IntPtr screen, out IMEVENT ev)
        {
            SmallCursesWrapper.getmouse_sp(screen, out ev);
        }
        #endregion

        #region ungetmouse_sp
        /// <summary>
        /// see <see cref="NativeNCurses.ungetmouse"/>
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void ungetmouse(IntPtr screen, in IMEVENT ev)
        {
            SmallCursesWrapper.ungetmouse_sp(screen, ev);
        }
        #endregion

        #region mousemask_sp
        /// <summary>
        /// see <see cref="NativeNCurses.mousemask"/>
        /// mouse event mask.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static ulong mousemask(IntPtr screen, ulong newmask, out ulong oldmask)
        {
            return SmallCursesWrapper.mousemask_sp(screen, newmask, out oldmask);
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
