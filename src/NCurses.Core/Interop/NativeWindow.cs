using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Buffers;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.MultiByteString;
using NCurses.Core.Interop.SingleByteString;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop
{
    /// <summary>
    /// native window methods.
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    internal static class NativeWindow
    {
        #region Custom type wrapper fields
        private static INativeWindowMultiByte multiByteNCursesWrapper;
        private static INativeWindowMultiByte MultiByteNCursesWrapper => NativeNCurses.HasUnicodeSupport
              ? multiByteNCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage)
              : throw new InvalidOperationException(Constants.NoUnicodeExceptionMessage);
        private static INativeWindowMultiByteString multiByteStringNCursesWrapper;
        private static INativeWindowMultiByteString MultiByteStringNCursesWrapper => NativeNCurses.HasUnicodeSupport
              ? multiByteStringNCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage)
              : throw new InvalidOperationException(Constants.NoUnicodeExceptionMessage);

        private static INativeWindowSingleByte singleByteNCursesWrapper;
        private static INativeWindowSingleByte SingleByteNCursesWrapper => singleByteNCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);
        private static INativeWindowSingleByteString singleByteStringNCursesWrapper;
        private static INativeWindowSingleByteString SingleByteStringNCursesWrapper => singleByteStringNCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);
        #endregion

        #region custom type initialization
        internal static void CreateCharCustomWrappers()
        {
            if (DynamicTypeBuilder.schar is null)
                throw new InvalidOperationException("Custom types haven't been generated yet.");

            Type customType;
            if (singleByteStringNCursesWrapper is null)
            {
                customType = typeof(NativeWindowSingleByteString<>).MakeGenericType(DynamicTypeBuilder.schar);
                singleByteStringNCursesWrapper = (INativeWindowSingleByteString)Activator.CreateInstance(customType);
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
                if (multiByteNCursesWrapper is null)
                {
                    customType = typeof(NativeWindowMultiByte<,,,,>).MakeGenericType(DynamicTypeBuilder.cchar_t, DynamicTypeBuilder.wchar_t, 
                        DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar, DynamicTypeBuilder.MEVENT);
                    multiByteNCursesWrapper = (INativeWindowMultiByte)Activator.CreateInstance(customType);
                }

                if (multiByteStringNCursesWrapper is null)
                {
                    customType = typeof(NativeWindowMultiByteString<,>).MakeGenericType(DynamicTypeBuilder.wchar_t, DynamicTypeBuilder.schar);
                    multiByteStringNCursesWrapper = (INativeWindowMultiByteString)Activator.CreateInstance(customType);
                }
            }

            if (singleByteNCursesWrapper is null)
            {
                customType = typeof(NativeWindowSingleByte<,,>).MakeGenericType(DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar, DynamicTypeBuilder.MEVENT);
                singleByteNCursesWrapper = (INativeWindowSingleByte)Activator.CreateInstance(customType);
            }
        }
        #endregion

        #region redrawwin
        /// <summary>
        /// The wredrawln routine indicates to curses that some screen
        /// lines are corrupted and should be thrown away before  anything  is  written over  them.It touches the indicated
        /// lines(marking them  changed).   The routine  redrawwin
        /// touches the entire window.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void redrawwin(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.redrawwin(window), "redrawwin");
        }
        #endregion

        #region waddch
        /// <summary>
        /// see <see cref="NativeStdScr.addch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddch(IntPtr window, in ISingleByteChar ch)
        {
            SingleByteNCursesWrapper.waddch(window, ch);
        }
        #endregion

        #region waddchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.addchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddchnstr(IntPtr window, in ISingleByteCharString txt, int number)
        {
            SingleByteNCursesWrapper.waddchnstr(window, txt, number);
        }
        #endregion

        #region waddchstr
        /// <summary>
        /// see <see cref="NativeStdScr.addchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddchstr(IntPtr window, in ISingleByteCharString txt)
        {
            SingleByteNCursesWrapper.waddchstr(window, txt);
        }
        #endregion

        #region waddnstr
        /// <summary>
        /// see <see cref="NativeStdScr.addnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddnstr(IntPtr window, in string txt, int number)
        {
            SingleByteStringNCursesWrapper.waddnstr(window, txt, number);
        }
        #endregion

        #region waddstr
        /// <summary>
        /// see <see cref="NativeStdScr.addstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddstr(IntPtr window, in string txt)
        {
            SingleByteStringNCursesWrapper.waddstr(window, txt);
        }
        #endregion

        #region wattroff
        /// <summary>
        /// see <see cref="NativeStdScr.attroff"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattroff(IntPtr window, int attrs)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wattroff(window, attrs), "wattroff");
        }
        #endregion

        #region wattron
        /// <summary>
        /// see <see cref="NativeStdScr.attron"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattron(IntPtr window, int attrs)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wattron(window, attrs), "wattron");
        }
        #endregion

        #region wattrset
        /// <summary>
        /// see <see cref="NativeStdScr.attrset"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattrset(IntPtr window, int attrs)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wattrset(window, attrs), "wattrset");
        }
        #endregion

        #region wattr_on
        /// <summary>
        /// see <see cref="NativeStdScr.attr_on"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattr_on(IntPtr window, ulong attrs)
        {
            SingleByteNCursesWrapper.wattr_on(window, attrs);
        }
        #endregion

        #region wattr_off
        /// <summary>
        /// see <see cref="NativeStdScr.attr_off"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattr_off(IntPtr window, ulong attrs)
        {
            SingleByteNCursesWrapper.wattr_off(window, attrs);
        }
        #endregion

        #region wattr_set
        /// <summary>
        /// see <see cref="NativeStdScr.attr_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattr_set(IntPtr window, ulong attrs, short pair)
        {
            SingleByteNCursesWrapper.wattr_set(window, attrs, pair);
        }
        #endregion

        #region wattr_get
        /// <summary>
        /// see <see cref="NativeStdScr.attr_get"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattr_get(IntPtr window, out ulong attrs, out short pair)
        {
            SingleByteNCursesWrapper.wattr_get(window, out attrs, out pair);
        }
        #endregion

        #region wbkgd
        /// <summary>
        /// see <see cref="NativeStdScr.bkgd"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wbkgd(IntPtr window, in ISingleByteChar bkgd)
        {
            SingleByteNCursesWrapper.wbkgd(window, bkgd);
        }
        #endregion

        #region wbkgdset
        /// <summary>
        /// see <see cref="NativeStdScr.bkgdset"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wbkgdset(IntPtr window, in ISingleByteChar bkgd)
        {
            SingleByteNCursesWrapper.wbkgdset(window, bkgd);
        }
        #endregion

        #region wborder
        /// <summary>
        /// see <see cref="NativeStdScr.border"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wborder(IntPtr window, in ISingleByteChar ls, in ISingleByteChar rs, in ISingleByteChar ts, in ISingleByteChar bs, in ISingleByteChar tl, in ISingleByteChar tr, in ISingleByteChar bl, in ISingleByteChar br)
        {
            SingleByteNCursesWrapper.wborder(window, ls, rs, ts, bs, tl, tr, bl, br);
        }
        #endregion

        #region box
        /// <summary>
        /// box(win, verch, horch) is a shorthand  for  the following
        /// call:  wborder(win, verch, verch, horch, horch, 0, 0, 0, 0).
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="verch">vertical character (lr, rs)</param>
        /// <param name="horch">horizontal character (ts, bs)</param>
        public static void box(IntPtr window, in ISingleByteChar verch, in ISingleByteChar horch)
        {
            SingleByteNCursesWrapper.box(window, verch, horch);
        }
        #endregion

        #region wchgat
        /// <summary>
        /// see <see cref="NativeStdScr.chgat"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wchgat(IntPtr window, int number, ulong attrs, short pair)
        {
            SingleByteNCursesWrapper.wchgat(window, number, attrs, pair);
        }
        #endregion

        #region wclear
        /// <summary>
        /// see <see cref="NativeStdScr.clear"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wclear(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wclear(window), "wclear");
        }
        #endregion

        #region clearok
        /// <summary>
        /// If clearok is called with TRUE as argument, the next call
        /// to wrefresh with this window will clear the  screen completely and  redraw the entire screen from scratch.This
        /// is useful when the contents of the screen are  uncertain,
        /// or  in  some cases for a more pleasing visual effect.If
        /// the win argument to clearok is the global variable curscr,
        /// the next  call to  wrefresh with  any window causes the
        /// screen to be cleared and repainted from scratch.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="bf">true if you want to force a full redraw</param>
        public static void clearok(IntPtr window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.clearok(window, bf), "clearok");
        }
        #endregion

        #region wclrtobot
        /// <summary>
        /// see <see cref="NativeStdScr.clrtobot"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wclrtobot(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wclrtobot(window), "wclrtobot");
        }
        #endregion

        #region wclrtoeol
        /// <summary>
        /// see <see cref="NativeStdScr.clrtoeol"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wclrtoeol(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wclrtoeol(window), "wclrtoeol");
        }
        #endregion

        #region wcolor_set
        /// <summary>
        /// see <see cref="NativeStdScr.color_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wcolor_set(IntPtr window, short pair)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wcolor_set(window, pair, IntPtr.Zero), "wcolor_set");
        }
        #endregion

        #region wdelch
        /// <summary>
        /// see <see cref="NativeStdScr.delch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wdelch(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wdelch(window), "wdelch");
        }
        #endregion

        #region wdeleteln
        /// <summary>
        /// see <see cref="NativeStdScr.deleteln"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wdeleteln(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wdeleteln(window), "wdeleteln");
        }
        #endregion

        #region wechochar
        /// <summary>
        /// see <see cref="NativeStdScr.echochar"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wechochar(IntPtr window, in ISingleByteChar ch)
        {
            SingleByteNCursesWrapper.wechochar(window, ch);
        }
        #endregion

        #region werase
        /// <summary>
        /// see <see cref="NativeStdScr.erase"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void werase(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.werase(window), "werase");
        }
        #endregion

        #region getbkgd
        /// <summary>
        /// The getbkgd function returns the given  window's  current background character/attribute pair.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <returns>Current window background character/attribute pair</returns>
        public static ISingleByteChar getbkgd(IntPtr window)
        {
            return SingleByteNCursesWrapper.getbkgd(window);
        }
        #endregion

        #region wgetch
        /// <summary>
        /// see <see cref="NativeStdScr.getch"/>
        /// when <see cref="NativeNCurses.UseWindowsOverride"/> is set, the default NCurses behaviour gets overridden with a managed implementation
        /// which allows for control modifiers on function keys.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static int wgetch(IntPtr window)
        {
            int ret = 0;
            NCursesException.Verify(ret = NativeNCurses.NCursesWrapper.wgetch(window), "wgetch");
            return ret;
        }

        /// <summary>
        /// see <see cref="NativeWindow.wgetch"/>
        /// </summary>
        public static bool wgetch(IntPtr window, out char ch, out Key key)
        {
            return NativeNCurses.VerifyInput("wgetch", NativeNCurses.NCursesWrapper.wgetch(window), out ch, out key);
        }
        #endregion

        #region wgetnstr
        /// <summary>
        /// see <see cref="NativeStdScr.getnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wgetnstr(IntPtr window, out string str, int count)
        {
            SingleByteStringNCursesWrapper.wgetnstr(window, out str, count);
        }
        #endregion

        #region wgetstr
        /// <summary>
        /// see <see cref="NativeStdScr.getstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wgetstr(IntPtr window, out string str)
        {
            SingleByteStringNCursesWrapper.wgetstr(window, out str);
        }
        #endregion

        #region whline
        /// <summary>
        /// see <see cref="NativeStdScr.hline"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void whline(IntPtr window, in ISingleByteChar ch, int count)
        {
            SingleByteNCursesWrapper.whline(window, ch, count);
        }
        #endregion

        #region idcok
        /// <summary>
        /// If idcok is called with FALSE as second argument, curses
        /// no longer considers using the hardware insert/delete character feature of terminals so equipped.Use of character
        /// insert/delete  is  enabled by default.  Calling idcok with
        /// TRUE as second argument re-enables use of character insertion and deletion.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="bf">enable/disable use of character insertion and deletion</param>
        public static void idcok(IntPtr window, bool bf)
        {
            NativeNCurses.NCursesWrapper.idcok(window, bf);
        }
        #endregion

        #region idlok
        /// <summary>
        /// If idlok is called with TRUE as  second argument, curses
        /// considers using the hardware insert/delete line feature of
        /// terminals so equipped.Calling idlok with FALSE as second
        /// argument  disables use  of line  insertion and deletion.
        /// This option should be  enabled only  if  the application
        /// needs insert/delete line, for example, for a screen editor.It is disabled by default because insert/delete line
        /// tends to  be visually annoying when used in applications
        /// where it is not really needed.If insert/delete line cannot be  used,  curses redraws the changed portions of all lines.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="bf"></param>
        /// <returns>Always returns Constants.OK</returns>
        public static int idlok(IntPtr window, bool bf)
        {
            return NativeNCurses.NCursesWrapper.idlok(window, bf);
        }
        #endregion

        #region immedok
        /// <summary>
        /// If immedok is called with TRUE as argument, any change in
        /// the window image, such as the ones caused by waddch, wclrtobot,  wscrl,  etc.,
        /// automatically cause a call to wrefresh.However, it may degrade performance  considerably,
        /// due to repeated calls to wrefresh.It is disabled by default.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="bf">true if you wanne enble refresh on addch</param>
        public static void immedok(IntPtr window, bool bf)
        {
            NativeNCurses.NCursesWrapper.immedok(window, bf);
        }
        #endregion

        #region winch
        /// <summary>
        /// see <see cref="NativeStdScr.inch"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <returns>characther with attributes at current position</returns>
        public static void winch(IntPtr window, out ISingleByteChar ch)
        {
            SingleByteNCursesWrapper.winch(window, out ch);
        }
        #endregion

        #region winchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.inchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winchnstr(IntPtr window, out ISingleByteCharString chStr, int count, out int read)
        {
            SingleByteNCursesWrapper.winchnstr(window, out chStr, count, out read);
        }
#endregion

#region winchstr
        /// <summary>
        /// see <see cref="NativeStdScr.inchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winchstr(IntPtr window, out ISingleByteCharString chStr, out int read)
        {
            SingleByteNCursesWrapper.winchstr(window, out chStr, out read);
        }
#endregion

#region winnstr
        /// <summary>
        /// see <see cref="NativeStdScr.innstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winnstr(IntPtr window, out string str, int n, out int read)
        {
            SingleByteStringNCursesWrapper.winnstr(window, out str, n, out read);
        }
#endregion

#region winsch
        /// <summary>
        /// see <see cref="NativeStdScr.insch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winsch(IntPtr window, in ISingleByteChar ch)
        {
            SingleByteNCursesWrapper.winsch(window, ch);
        }
#endregion

#region winsdelln
        /// <summary>
        /// see <see cref="NativeStdScr.insdelln"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winsdelln(IntPtr window, int n)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.winsdelln(window, n), "winsdelln");
        }
#endregion

#region winsertln
        /// <summary>
        /// see <see cref="NativeStdScr.insertln"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winsertln(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.winsertln(window), "winsertln");
        }
#endregion

#region winsnstr
        /// <summary>
        /// see <see cref="NativeStdScr.insnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winsnstr(IntPtr window, in string str, int n)
        {
            SingleByteStringNCursesWrapper.winsnstr(window, str, n);
        }
#endregion

#region winsstr
        /// <summary>
        /// see <see cref="NativeStdScr.insstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winsstr(IntPtr window, string str)
        {
            SingleByteStringNCursesWrapper.winsstr(window, str);
        }
#endregion

#region winstr
        /// <summary>
        /// see <see cref="NativeStdScr.instr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winstr(IntPtr window, out string str, out int read)
        {
            SingleByteStringNCursesWrapper.winstr(window, out str, out read);
        }
#endregion

#region is_linetouched
        /// <summary>
        /// The  is_linetouched and is_wintouched routines return TRUE
        /// if the specified <paramref name="line"/>/window was modified since  the last
        /// call to  wrefresh; otherwise they return FALSE.In addition, is_linetouched returns ERR if <paramref name="line"/> is not valid  for
        /// the given window.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="line">The number of the line to check</param>
        /// <returns>true if line has been changed</returns>
        public static bool is_linetouched(IntPtr window, int line)
        {
            return NativeNCurses.NCursesWrapper.is_linetouched(window, line);
        }
#endregion

#region is_wintouched
        /// <summary>
        /// The  is_linetouched and is_wintouched routines return TRUE
        /// if the specified line/window was modified since  the last
        /// call to  wrefresh; otherwise they return FALSE.In addition, is_linetouched returns ERR if <paramref name="line"/> is not valid  for
        /// the given window.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <returns>true if window has been changed</returns>
        public static bool is_wintouched(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_wintouched(window);
        }
#endregion

#region keypad
        /// <summary>
        /// The keypad option enables the keypad of the user's terminal.If enabled(bf is TRUE), the user can press a func-
        /// tion key(such as an arrow key) and wgetch(3x) returns a
        /// single value  representing the  function key,   as   in
        /// KEY_LEFT.If disabled(bf  is  FALSE), curses does not
        /// treat function keys specially and the program has to  interpret the escape sequences itself.If the keypad in the
        /// terminal can be turned on(made to transmit) and off(made
        /// to work locally), turning on this option causes the termi-
        /// nal keypad to be turned on when wgetch(3x) is called.The
        /// default value for keypad is FALSE.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="bf">enable/disable keypad</param>
        public static void keypad(IntPtr window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.keypad(window, bf), "keypad");
        }
#endregion

#region leaveok
        /// <summary>
        /// Normally, the hardware cursor is left at the location  of
        /// the window cursor being refreshed.The leaveok option allows the cursor to be left wherever the update happens  to
        /// leave  it.It is useful for applications where the cursor
        /// is not used, since it reduces the need for cursor motions.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="bf">enable/disable</param>
        public static void leaveok(IntPtr window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.leaveok(window, bf), "leaveok");
        }
#endregion

#region wmove
        /// <summary>
        /// see <see cref="NativeStdScr.move"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wmove(IntPtr window, int y, int x)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wmove(window, y, x), "wmove");
        }
#endregion

#region mvwaddch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and add character, see <see cref="waddch(IntPtr, chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddch(IntPtr window, int y, int x, in ISingleByteChar ch)
        {
            SingleByteNCursesWrapper.mvwaddch(window, y, x, ch);
        }
#endregion

#region mvwaddchnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="waddchnstr(IntPtr, IntPtr, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddchnstr(IntPtr window, int y, int x, in ISingleByteCharString chstr, int n)
        {
            SingleByteNCursesWrapper.mvwaddchnstr(window, y, x, chstr, n);
        }
#endregion

#region mvwaddchstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="waddchstr(IntPtr, IntPtr)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddchstr(IntPtr window, int y, int x, in ISingleByteCharString chstr)
        {
            SingleByteNCursesWrapper.mvwaddchstr(window, y, x, chstr);
        }
#endregion

#region mvwaddnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="waddnstr(IntPtr, string, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddnstr(IntPtr window, int y, int x, in string txt, int n)
        {
            SingleByteStringNCursesWrapper.mvwaddnstr(window, y, x, txt, n);
        }
#endregion

#region mvwaddstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="waddstr(IntPtr, string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddstr(IntPtr window, int y, int x, in string txt)
        {
            SingleByteStringNCursesWrapper.mvwaddstr(window, y, x, txt);
        }
#endregion

#region mvwchgat
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wchgat(IntPtr, int, chtype, short)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwchgat(IntPtr window, int y, int x, int number, ulong attrs, short pair)
        {
            SingleByteNCursesWrapper.mvwchgat(window, y, x, number, attrs, pair);
        }
#endregion

#region mvwdelch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wdelch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwdelch(IntPtr window, int y, int x)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.mvwdelch(window, y, x), "mvwdelch");
        }
        #endregion

        #region mvwgetch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wgetch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static int mvwgetch(IntPtr window, int y, int x)
        {
            int ret = 0;
            NCursesException.Verify(NativeNCurses.NCursesWrapper.mvwgetch(window, y, x), "mvwgetch");
            return ret;
        }

        /// <summary>
        /// see <see cref="NativeWindow.mvwgetch(IntPtr, int, int)"/>
        /// </summary>
        public static bool mvwgetch(IntPtr window, int y, int x, out char ch, out Key key)
        {
            return NativeNCurses.VerifyInput("mvwgetch", NativeNCurses.NCursesWrapper.mvwgetch(window, y, x), out ch, out key);
        }
#endregion

#region mvwgetnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wgetnstr(IntPtr, StringBuilder, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwgetnstr(IntPtr window, int y, int x, out string str, int count)
        {
            SingleByteStringNCursesWrapper.mvwgetnstr(window, y, x, out str, count);
        }
#endregion

#region mvwgetstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wgetstr(IntPtr, StringBuilder)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwgetstr(IntPtr window, int y, int x, out string str)
        {
            SingleByteStringNCursesWrapper.mvwgetstr(window, y, x, out str);
        }
#endregion

#region mvwhline
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="whline(IntPtr, chtype, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwhline(IntPtr window, int y, int x, in ISingleByteChar ch, int count)
        {
            SingleByteNCursesWrapper.mvwhline(window, y, x, ch, count);
        }
#endregion

#region mvwinch
        /// <summary>
        /// see <see cref="NativeStdScr.mvinch(int, int)"/>
        /// </summary>
        public static void mvwinch(IntPtr window, int y, int x, out ISingleByteChar ch)
        {
            SingleByteNCursesWrapper.mvwinch(window, y, x, out ch);
        }
#endregion

#region mvwinchnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winchnstr(IntPtr, IntPtr, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwinchnstr(IntPtr window, int y, int x, out ISingleByteCharString chStr, int count, out int read)
        {
            SingleByteNCursesWrapper.mvwinchnstr(window, y, x, out chStr, count, out read);
        }
#endregion

#region mvwinchstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winchstr(IntPtr, IntPtr)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwinchstr(IntPtr window, int y, int x, out ISingleByteCharString chStr, out int read)
        {
            SingleByteNCursesWrapper.mvwinchstr(window, y, x, out chStr, out read);
        }
#endregion

#region mvwinnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winnstr(IntPtr, StringBuilder, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwinnstr(IntPtr window, int y, int x, out string str, int n, out int read)
        {
            SingleByteStringNCursesWrapper.mvwinnstr(window, y, x, out str, n, out read);
        }
#endregion

#region mvwinsch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winsch(IntPtr, chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwinsch(IntPtr window, int y, int x, in ISingleByteChar ch)
        {
            SingleByteNCursesWrapper.mvwinsch(window, y, x, ch);
        }
#endregion

#region mvwinsnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winsnstr(IntPtr, string, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwinsnstr(IntPtr window, int y, int x, in string str, int n)
        {
            SingleByteStringNCursesWrapper.mvwinsnstr(window, y, x, str, n);
        }
#endregion

#region mvwinsstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winsstr(IntPtr, string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwinsstr(IntPtr window, int y, int x, in string str)
        {
            SingleByteStringNCursesWrapper.mvwinsstr(window, y, x, str);
        }
#endregion

#region mvwinstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winstr(IntPtr, StringBuilder)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwinstr(IntPtr window, int y, int x, out string str, out int read)
        {
            SingleByteStringNCursesWrapper.mvwinstr(window, y, x, out str, out read);
        }
#endregion

#region mvwprintw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wprintw(IntPtr, string, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwprintw(IntPtr window, int y, int x, string format, params string[] argList)
        {
            SingleByteStringNCursesWrapper.mvwprintw(window, y, x, format, argList);
        }
#endregion

#region mvwscanw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wscanw(IntPtr, StringBuilder, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwscanw(IntPtr window, int y, int x, out string format, params string[] argList)
        {
            SingleByteStringNCursesWrapper.mvwscanw(window, y, x, out format, argList);
        }
#endregion

#region mvwvline
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wvline(IntPtr, chtype, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwvline(IntPtr window, int y, int x, in ISingleByteChar ch, int n)
        {
            SingleByteNCursesWrapper.mvwvline(window, y, x, ch, n);
        }
#endregion

#region nodelay
        /// <summary>
        /// The nodelay option causes getch to be a non-blocking call.
        /// If no input is ready, getch returns ERR. If disabled(bf
        /// is FALSE), getch waits until a key is pressed.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="bf">enable/disable</param>
        public static void nodelay(IntPtr window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.nodelay(window, bf), "nodelay");
        }
#endregion

#region notimeout
        /// <summary>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="bf">enable/disable</param>
        public static void notimeout(IntPtr window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.notimeout(window, bf), "notimeout");
        }
#endregion

#region wrefresh
        /// <summary>
        /// see <see cref="NativeStdScr.refresh"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wrefresh(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wrefresh(window), "wrefresh");
        }
#endregion

#region wprintw
        /// <summary>
        /// see <see cref="NativeStdScr.printw(string, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wprintw(IntPtr window, string format, params string[] argList)
        {
            SingleByteStringNCursesWrapper.wprintw(window, format, argList);
        }
#endregion

#region wscanw
        /// <summary>
        /// see <see cref="NativeStdScr.scanw"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wscanw(IntPtr window, out string format, params string[] argList)
        {
            SingleByteStringNCursesWrapper.wscanw(window, out format, argList);
        }
#endregion

#region wscrl
        /// <summary>
        /// see <see cref="NativeStdScr.scrl(int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wscrl(IntPtr window, int n)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wscrl(window, n), "wscrl");
        }
#endregion

#region scroll
        /// <summary>
        /// The scroll  routine scrolls the window up one line.This
        /// involves moving the lines in the window  data structure.
        /// As  an optimization, if the scrolling region of the window
        /// is the entire screen, the physical screen may be scrolled
        /// at the same time.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void scroll(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.scroll(window), "scroll");
        }
#endregion

#region scrollok
        /// <summary>
        /// The scrollok option controls what happens when the cursor
        /// of a window is  moved off  the edge  of the  window or
        /// scrolling region, either as a result of a newline action
        /// on the bottom line, or typing the last character  of the
        /// last line.If disabled, (<paramref name="bf"/> is FALSE), the cursor is left
        /// on the bottom line.If enabled, (<paramref name="bf"/> is TRUE), the  window
        /// is scrolled  up  one  line(Note that to get the physical
        /// scrolling effect on the terminal, it is also necessary  to
        /// call idlok).
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="bf">enable/disable scrolling</param>
        public static void scrollok(IntPtr window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.scrollok(window, bf), "scrollok");
        }
#endregion

#region wsetscrreg
        /// <summary>
        /// see <see cref="NativeStdScr.setscrreg(int, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wsetscrreg(IntPtr window, int top, int bot)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wsetscrreg(window, top, bot), "wsetscrreg");
        }
#endregion

#region wstandout
        /// <summary>
        /// see <see cref="NativeStdScr.standout"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wstandout(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wstandout(window), "wstandout");
        }
#endregion

#region wstandend
        /// <summary>
        /// see <see cref="wstandout"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wstandend(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wstandend(window), "wstandend");
        }
#endregion

#region syncok
        /// <summary>
        /// Calling wsyncup touches all locations in ancestors of  win
        /// that  are changed in win.If syncok is called with second
        /// argument TRUE then wsyncup is called automatically whenever there is a change in the window.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void syncok(IntPtr window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.syncok(window, bf), "syncok");
        }
#endregion

#region wtimeout
        /// <summary>
        /// see <see cref="NativeStdScr.timeout"/>
        /// </summary>
        public static void wtimeout(int delay)
        {
            NativeNCurses.NCursesWrapper.wtimeout(delay);
        }
#endregion

#region touchline
        /// <summary>
        //// The touchwin and touchline routines throw away all  optimization information about which parts of the window have
        /// been touched, by pretending that the  entire window  has
        //// been  drawn on.This  is sometimes necessary when using
        /// overlapping windows, since a change to one window  affects
        /// the other window, but the records of which lines have been
        /// changed in the other window do  not reflect  the change.
        /// The  routine touchline only pretends that count lines have
        /// been changed, beginning with line start.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void touchline(IntPtr window, int start, int count)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.touchline(window, start, count), "touchline");
        }
#endregion

#region touchwin
        /// <summary>
        /// see <see cref="touchline"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void touchwin(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.touchwin(window), "touchwin");
        }
#endregion

#region untouchwin
        /// <summary>
        /// The untouchwin routine marks all lines in  the window  as
        /// unchanged since the last call to wrefresh.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void untouchwin(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.untouchwin(window), "untouchwin");
        }
#endregion

#region wvline
        /// <summary>
        /// see <see cref="NativeStdScr.vline(chtype, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wvline(IntPtr window, in ISingleByteChar ch, int n)
        {
            SingleByteNCursesWrapper.wvline(window, ch, n);
        }
#endregion

        /* TODO (uses va_list from stdarg.h)
         *  int  vwprintw(WINDOW  *win,  const char *fmt, va_list varglist);
         *  int vw_printw(WINDOW *win, const char *fmt,  va_list  varglist);
         */

        /* TODO (uses va_list from stdarg.h)
         *  int vw_scanw(WINDOW *win, char *fmt, va_list varglist);
         *  int vwscanw(WINDOW *win, char *fmt, va_list varglist);
         */

#region wcursyncup
        /// <summary>
        /// The routine wcursyncup updates the current cursor position
        /// of all the ancestors of the window to reflect the current
        /// cursor position of the window.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wcursyncup(IntPtr window)
        {
            NativeNCurses.NCursesWrapper.wcursyncup(window);
        }
#endregion

#region wsyncdown
        /// <summary>
        /// The wsyncdown  routine touches each location in win that
        /// has been touched in any of  its ancestor  windows.This
        /// routine  is  called by wrefresh, so it should almost never
        /// be necessary to call it manually.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wsyncdown(IntPtr window)
        {
            NativeNCurses.NCursesWrapper.wsyncdown(window);
        }
#endregion

#region wnoutrefresh
        /// <summary>
        /// The wnoutrefresh and doupdate routines allow multiple updates with more efficiency than wrefresh alone.
        /// In addition to  all the window structures, curses keeps two data
        /// structures representing the terminal screen:  a physical
        /// screen,  describing what is actually on the screen, and a
        /// virtual screen, describing what the programmer  wants to
        /// have on the screen.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wnoutrefresh(IntPtr window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wnoutrefresh(window), "wnoutrefresh");
        }
#endregion

#region wredrawln
        /// <summary>
        /// The wredrawln routine indicates to curses that some screen
        /// lines are corrupted and should be thrown away before  anything  is  written over  them.It touches the indicated
        /// lines(marking them  changed).   The routine  redrawwin
        /// touches the entire window.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wredrawln(IntPtr window, int beg_line, int num_lines)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wredrawln(window, beg_line, num_lines), "wredrawln");
        }
#endregion

#region wsyncup
        /// <summary>
        /// Calling wsyncup touches all locations in ancestors of  win
        /// that  are changed in win.If syncok is called with second
        /// argument TRUE then wsyncup is called automatically whenever there is a change in the window.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wsyncup(IntPtr window)
        {
            NativeNCurses.NCursesWrapper.wsyncup(window);
        }
#endregion

#region wtouchln
        /// <summary>
        /// The wtouchln routine makes <paramref name="n"/> lines in the window, starting
        /// at line <paramref name="y"/>, look as if they have(<paramref name="changed"/>= 1)  or have  not
        /// (<paramref name="changed"/> = 0) been changed since the last call to wrefresh.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">line to start from</param>
        /// <param name="n">number of lines</param>
        /// <param name="changed">1 if changed, 0 if not changed</param>
        public static void wtouchln(IntPtr window, int y, int n, int changed)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wtouchln(window, y, n, changed), "wtouchln");
        }
#endregion

#region getattrs
        /// <summary>
        /// returns attributes
        /// </summary>
        /// <returns>attributes</returns>
        public static int getattrs(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getattrs(window);
        }
#endregion

#region getcurx
        /// <summary>
        /// The getcury and getcurx functions return the same data as getyx.
        /// </summary>
        /// <returns>attributes</returns>
        public static int getcurx(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getcurx(window);
        }
#endregion

#region getcury
        /// <summary>
        /// The getcury and getcurx functions return the same data as getyx.
        /// </summary>
        /// <returns>attributes</returns>
        public static int getcury(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getcury(window);
        }
#endregion

#region getbegx
        /// <summary>
        /// The getbegy and getbegx functions return the same data  as  getbegyx.
        /// </summary>
        /// <returns>attributes</returns>
        public static int getbegx(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getbegx(window);
        }
#endregion

#region getbegy
        /// <summary>
        /// The getbegy and getbegx functions return the same data  as  getbegyx.
        /// </summary>
        /// <returns>attributes</returns>
        public static int getbegy(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getbegy(window);
        }
#endregion

#region getmaxx
        /// <summary>
        /// The getmaxy and getmaxx functions return the same data  as getmaxyx.
        /// </summary>
        /// <returns>attributes</returns>
        public static int getmaxx(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getmaxx(window);
        }
#endregion

#region getmaxy
        /// <summary>
        /// The getmaxy and getmaxx functions return the same data  as getmaxyx.
        /// </summary>
        /// <returns>attributes</returns>
        public static int getmaxy(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getmaxy(window);
        }
#endregion

#region getparx
        /// <summary>
        /// The getpary and getparx functions return the same data as getparyx.
        /// </summary>
        /// <returns>attributes</returns>
        public static int getparx(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getparx(window);
        }
#endregion

#region getpary
        /// <summary>
        /// The getpary and getparx functions return the same data as getparyx.
        /// </summary>
        /// <returns>attributes</returns>
        public static int getpary(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getpary(window);
        }
#endregion

#region wresize
        /// <summary>
        /// This  is  an extension to the curses library.It reallocates storage for an ncurses window to adjust its
        /// dimensions to  the specified  values.If either dimension is
        /// larger than the current  values, the window's  data  is
        /// filled with blanks that have the current background rendition (as set by wbkgdset) merged into them.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="lines">new size in lines/param>
        /// <param name="columns">new size in columns</param>
        public static void wresize(IntPtr window, int lines, int columns)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wresize(window, lines, columns), "wresize");
        }
#endregion

#region wgetparent
        /// <summary>
        /// returns the parent WINDOW pointer for subwindows,  or NLL for windows having no parent.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static IntPtr wgetparent(IntPtr window)
        {
            return NCursesException.Verify(NativeNCurses.NCursesWrapper.wgetparent(window), "wgetparent");
        }
#endregion

#region is_cleared
        /// <summary>
        /// returns the value set in clearok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_cleared(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_cleared(window);
        }
#endregion

#region is_idcok
        /// <summary>
        /// returns the value set in idcok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_idcok(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_idcok(window);
        }
#endregion

#region is_idlok
        /// <summary>
        /// returns the value set in idlok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_idlok(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_idlok(window);
        }
#endregion

#region is_immedok
        /// <summary>
        /// returns the value set in immedok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_immedok(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_immedok(window);
        }
#endregion

#region is_keypad
        /// <summary>
        /// returns the value set in keypad
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_keypad(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_keypad(window);
        }
#endregion

#region is_leaveok
        /// <summary>
        /// returns the value set in leaveok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_leaveok(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_leaveok(window);
        }
#endregion

#region is_nodelay
        /// <summary>
        /// returns the value set in nodelay
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_nodelay(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_nodelay(window);
        }
#endregion

#region is_notimeout
        /// <summary>
        /// returns the value set in notimeout
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_notimeout(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_notimeout(window);
        }
#endregion

#region is_pad
        /// <summary>
        /// returns TRUE if the window is a pad i.e., created by <see cref="NativeNCurses.newpad"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_pad(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_pad(window);
        }
#endregion

#region is_scrollok
        /// <summary>
        /// returns the value set in scrollok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_scrollok(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_scrollok(window);
        }
#endregion

#region is_subwin
        /// <summary>
        /// returns TRUE if the window is a subwindow, i.e., created by subwin or derwin
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_subwin(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_subwin(window);
        }
#endregion

#region is_syncok
        /// <summary>
        /// returns the value set in syncok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool is_syncok(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.is_syncok(window);
        }
#endregion

#region wgetdelay
        /// <summary>
        /// returns the delay timeout as set in wtimeout.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static int wgetdelay(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.wgetdelay(window);
        }
#endregion

#region wgetscrreg
        /// <summary>
        /// returns the  top and  bottom rows for the scrolling margin as set in wsetscrreg.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="top">reference to the top line</param>
        /// <param name="bottom">reference to the bottom line</param>
        public static void wgetscrreg(IntPtr window, ref int top, ref int bottom)
        { 
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wgetscrreg(window, ref top, ref bottom), "wgetscrreg");
        }
#endregion

#region wadd_wch
        /// <summary>
        /// see <see cref="NativeStdScr.add_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wadd_wch(IntPtr window, IMultiByteChar wch)
        {
            MultiByteNCursesWrapper.wadd_wch(window, wch);
        }
#endregion

#region wadd_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.add_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wadd_wchnstr(IntPtr window, in IMultiByteCharString wchStr, int n)
        {
            MultiByteNCursesWrapper.wadd_wchnstr(window, wchStr, n);
        }
#endregion

#region wadd_wchstr
        /// <summary>
        /// see <see cref="NativeStdScr.add_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wadd_wchstr(IntPtr window, in IMultiByteCharString wchStr)
        {
            MultiByteNCursesWrapper.wadd_wchstr(window, wchStr);
        }
#endregion

#region waddnwstr
        /// <summary>
        /// see <see cref="NativeStdScr.addnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddnwstr(IntPtr window, in string wstr, int n)
        {
            MultiByteStringNCursesWrapper.waddnwstr(window, wstr, n);
        }
#endregion

#region waddwstr
        /// <summary>
        /// see <see cref="NativeStdScr.addwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddwstr(IntPtr window, in string wstr)
        {
            MultiByteStringNCursesWrapper.waddwstr(window, wstr);
        }
#endregion

#region wbkgrnd
        /// <summary>
        /// see <see cref="NativeStdScr.bkgrnd"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wbkgrnd(IntPtr window, in IMultiByteChar wch)
        {
            MultiByteNCursesWrapper.wbkgrnd(window, wch);
        }
#endregion

#region wbkgrndset
        /// <summary>
        /// see <see cref="NativeStdScr.bkgrndset"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wbkgrndset(IntPtr window, in IMultiByteChar wch)
        {
            MultiByteNCursesWrapper.wbkgrndset(window, wch);
        }
#endregion

#region wborder_set
        /// <summary>
        /// see <see cref="NativeStdScr.border_set"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wborder_set(IntPtr window, in IMultiByteChar ls, in IMultiByteChar rs, in IMultiByteChar ts, in IMultiByteChar bs, in IMultiByteChar tl, in IMultiByteChar tr,
            in IMultiByteChar bl, in IMultiByteChar br)
        {
            MultiByteNCursesWrapper.wborder_set(window, ls, rs, ts, bs, tl, tr, bl, br);
        }
#endregion

#region box_set
        /// <summary>
        /// box_set(win, verch, horch); is a shorthand for the follow ing call:
        /// wborder_set(win, verch, verch, horch, horch, NULL, NULL, NULL, NULL);
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void box_set(IntPtr window, in IMultiByteChar verch, in IMultiByteChar horch)
        {
            MultiByteNCursesWrapper.box_set(window, verch, horch);
        }
#endregion

#region wecho_wchar
        /// <summary>
        /// seee <see cref="NativeStdScr.echo_wchar"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wecho_wchar(IntPtr window, in IMultiByteChar wch)
        {
            MultiByteNCursesWrapper.wecho_wchar(window, wch);
        }
#endregion

#region wget_wch
        /// <summary>
        /// see <see cref="NativeStdScr.get_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool wget_wch(IntPtr window, out char wch, out Key key)
        {
            return MultiByteStringNCursesWrapper.wget_wch(window, out wch, out key);
        }
#endregion

#region wget_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.get_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wget_wstr(IntPtr window, out string wstr)
        {
            MultiByteStringNCursesWrapper.wget_wstr(window, out wstr);
        }
#endregion

#region wgetbkgrnd
        /// <summary>
        /// see <see cref="NativeStdScr.getbkgrnd"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wgetbkgrnd(IntPtr window, out IMultiByteChar wch)
        {
            MultiByteNCursesWrapper.wgetbkgrnd(window, out wch);
        }
#endregion

#region wgetn_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.getn_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wgetn_wstr(IntPtr window, out string wstr, int n)
        {
            MultiByteStringNCursesWrapper.wgetn_wstr(window, out wstr, n);
        }
#endregion

#region whline_set
        /// <summary>
        /// see <see cref="NativeStdScr.hline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void whline_set(IntPtr window, in IMultiByteChar wch, int n)
        {
            MultiByteNCursesWrapper.whline_set(window, wch, n);
        }
#endregion

#region win_wch
        /// <summary>
        /// see <see cref="NativeStdScr.in_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void win_wch(IntPtr window, out IMultiByteChar wcval)
        {
            MultiByteNCursesWrapper.win_wch(window, out wcval);
        }
#endregion

#region win_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.in_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void win_wchnstr(IntPtr window, out IMultiByteCharString wchStr, int n)
        {
            MultiByteNCursesWrapper.win_wchnstr(window, out wchStr, n);
        }
#endregion

#region win_wchstr
        /// <summary>
        /// see <see cref="NativeStdScr.in_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void win_wchstr(IntPtr window, out IMultiByteCharString wchStr)
        {
            MultiByteNCursesWrapper.win_wchstr(window, out wchStr);
        }
#endregion

#region winnwstr
        /// <summary>
        /// see <see cref="NativeStdScr.innwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winnwstr(IntPtr window, out string str, int n, out int read)
        {
            MultiByteStringNCursesWrapper.winnwstr(window, out str, n, out read);
        }
#endregion

#region wins_nwstr
        /// <summary>
        /// see <see cref="NativeStdScr.ins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wins_nwstr(IntPtr window, in string str, int n)
        {
            MultiByteStringNCursesWrapper.wins_nwstr(window, str, n);
        }
#endregion

#region wins_wch
        
        /// <summary>
        /// see <see cref="NativeStdScr.ins_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wins_wch(IntPtr window, in IMultiByteChar wch)
        {
            MultiByteNCursesWrapper.wins_wch(window, wch);
        }
#endregion

#region wins_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.ins_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wins_wstr(IntPtr window, in string str)
        {
            MultiByteStringNCursesWrapper.wins_wstr(window, str);
        }
#endregion

#region winwstr
        /// <summary>
        /// see <see cref="NativeStdScr.inwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winwstr(IntPtr window, out string str)
        {
            MultiByteStringNCursesWrapper.winwstr(window, out str);
        }
#endregion

#region mvwadd_wch
        /// <summary>
        /// see <see cref="NativeStdScr.mvadd_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwadd_wch(IntPtr window, int y, int x, in IMultiByteChar wch)
        {
            MultiByteNCursesWrapper.mvwadd_wch(window, y, x, wch);
        }
#endregion

#region mvwadd_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvadd_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwadd_wchnstr(IntPtr window, int y, int x, in IMultiByteCharString wchStr, int n)
        {
            MultiByteNCursesWrapper.mvwadd_wchnstr(window, y, x, wchStr, n);
        }
#endregion

#region mvwadd_wchstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvadd_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwadd_wchstr(IntPtr window, int y, int x, in IMultiByteCharString wchStr)
        {
            MultiByteNCursesWrapper.mvwadd_wchstr(window, y, x, wchStr);
        }
#endregion

#region mvwaddnwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvaddnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddnwstr(IntPtr window, int y, int x, in string wstr, int n)
        {
            MultiByteStringNCursesWrapper.mvwaddnwstr(window, y, x, wstr, n);
        }
#endregion

#region mvwaddwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvaddwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddwstr(IntPtr window, int y, int x, in string wstr)
        {
            MultiByteStringNCursesWrapper.mvwaddwstr(window, y, x, wstr);
        }
#endregion

#region mvwget_wch
        /// <summary>
        /// see <see cref="NativeStdScr.mvget_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static bool mvwget_wch(IntPtr window, int y, int x, out char wch, out Key key)
        {
            return MultiByteStringNCursesWrapper.mvwget_wch(window, y, x, out wch, out key);
        }
#endregion

#region mvwget_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvget_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwget_wstr(IntPtr window, int y, int x, out string wstr)
        {
            MultiByteStringNCursesWrapper.mvwget_wstr(window, y, x, out wstr);
        }
#endregion

#region mvwgetn_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvgetn_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwgetn_wstr(IntPtr window, int y, int x, out string wstr, int n)
        {
            MultiByteStringNCursesWrapper.mvwgetn_wstr(window, y, x, out wstr, n);
        }
#endregion

#region mvwhline_set
        /// <summary>
        /// see <see cref="NativeStdScr.mvhline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwhline_set(IntPtr window, int y, int x, in IMultiByteChar wch, int n)
        {
            MultiByteNCursesWrapper.mvwhline_set(window, y, x, wch, n);
        }
#endregion

#region mvwin_wch
        /// <summary>
        /// see <see cref="NativeStdScr.mvin_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwin_wch(IntPtr window, int y, int x, out IMultiByteChar wcval)
        {
            MultiByteNCursesWrapper.mvwin_wch(window, y, x, out wcval);
        }
#endregion

#region mvwin_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvin_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">array reference to store the complex characters in</param>
        public static void mvwin_wchnstr(IntPtr window, int y, int x, out IMultiByteCharString wchStr, int n)
        {
            MultiByteNCursesWrapper.mvwin_wchnstr(window, y, x, out wchStr, n);
        }
        #endregion

        #region mvwin_wchstr
        /// <summary>
        /// see <see cref="mvwin_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwin_wchstr(IntPtr window, int y, int x, out IMultiByteCharString wchStr)
        {
            MultiByteNCursesWrapper.mvwin_wchstr(window, y, x, out wchStr);
        }
        #endregion

        #region mvwinnwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvinnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwinnwstr(IntPtr window, int y, int x, out string str, int n, out int read)
        {
            MultiByteStringNCursesWrapper.mvwinnwstr(window, y, x, out str, n, out read);
        }
#endregion

#region mvwins_nwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwins_nwstr(IntPtr window, int y, int x, in string str, int n)
        {
            MultiByteStringNCursesWrapper.mvwins_nwstr(window, y, x, str, n);
        }
#endregion

#region mvwins_wch
        /// <summary>
        /// see <see cref="NativeStdScr.mvins_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwins_wch(IntPtr window, int y, int x, in IMultiByteChar wch)
        {
            MultiByteNCursesWrapper.mvwins_wch(window, y, x, wch);
        }
#endregion

#region mvwins_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvins_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwins_wstr(IntPtr window, int y, int x, in string wstr)
        {
            MultiByteStringNCursesWrapper.mvwins_wstr(window, y, x, wstr);
        }
#endregion

#region mvwinwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvinwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwinwstr(IntPtr window, int y, int x, out string str)
        {
            MultiByteStringNCursesWrapper.mvwinwstr(window, y, x, out str);
        }
#endregion

#region mvwvline_set
        /// <summary>
        /// see <see cref="NativeStdScr.mvvline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwvline_set(IntPtr window, int y, int x, in IMultiByteChar wch, int n)
        {
            MultiByteNCursesWrapper.mvwvline_set(window, y, x, wch, n);
        }
#endregion

#region wvline_set
        /// <summary>
        /// see <see cref="NativeStdScr.vline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wvline_set(IntPtr window, in IMultiByteChar wch, int n)
        {
            MultiByteNCursesWrapper.wvline_set(window, wch, n);
        }
#endregion

#region wenclose
        /// <summary>
        /// The wenclose  function tests  whether a  given pair of
        /// screen-relative character-cell coordinates is enclosed by
        /// a given  window, returning TRUE if it is and FALSE other-
        /// wise.It is useful for determining what  subset of  the
        /// screen windows enclose the location of a mouse event.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <param name="y">number of line</param>
        /// <param name="x">number of column</param>
        /// <returns>true if coordinates are withing window/returns>
        public static bool wenclose(IntPtr window, int y, int x)
        {
            return NativeNCurses.NCursesWrapper.wenclose(window, y, x);
        }
#endregion

#region wmouse_trafo
        /// <summary>
        /// The wmouse_trafo function transforms a given pair of coordinates from stdscr-relative coordinates  to coordinates
        /// relative to the given window or vice versa.The resulting
        /// stdscr-relative coordinates are not  always identical  to
        /// window-relative coordinates  due to the mechanism to reserve lines on top or
        /// bottom of the screen for other purposes  (see the ripoffline and slk_init(3x) calls, for example).
        /// <param name="win">A pointer to a window</param>
        /// <param name="pY">reference to the number of lines to transform</param>
        /// <param name="pX">reference to the number of columns to transform/param>
        /// <param name="to_screen">true if you want to transform to screen coordinates</param>
        /// <returns>true if transform succeeded/returns>
        public static bool wmouse_trafo(IntPtr win, ref int pY, ref int pX, bool to_screen)
        {
            return NativeNCurses.NCursesWrapper.wmouse_trafo(win, ref pY, ref pX, to_screen);
        }
#endregion
    }
}
