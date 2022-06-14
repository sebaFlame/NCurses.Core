using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Buffers;
using System.Runtime.CompilerServices;

using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.SafeHandles;
using NippyWard.NCurses.Interop.Wrappers;
using NippyWard.NCurses.Interop.Mouse;

namespace NippyWard.NCurses.Interop
{
    /// <summary>
    /// native window methods.
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    internal class NativeWindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
            : INativeWindowWrapper<
                TMultiByte,
                MultiByteCharString<TMultiByte, TWideChar, TSingleByte>,
                TWideChar,
                WideCharString<TWideChar>,
                TSingleByte,
                SingleByteCharString<TSingleByte>,
                TChar,
                CharString<TChar>,
                TMouseEvent>,
            INativeWindowWrapper<
                IMultiByteNCursesChar,
                IMultiByteNCursesCharString,
                IMultiByteChar,
                IMultiByteCharString,
                ISingleByteNCursesChar,
                ISingleByteNCursesCharString,
                ISingleByteChar,
                ISingleByteCharString,
                IMEVENT>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeWindowMultiByte<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> MultiByteNCursesWrapper { get; }
        internal NativeWindowSingleByte<TSingleByte, TChar, TMouseEvent> SingleByteNCursesWrapper { get; }

        internal NativeWindowWideChar<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> WideCharNCursesWrapper { get; }
        internal NativeWindowChar<TChar> CharNCursesWrapper { get; }

        public NativeWindowInternal(
            IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> multiByteWrapper,
            ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> singleByteWrapper,
            IWideCharWrapper<TWideChar, TChar> wideCharWrapper,
            ICharWrapper<TChar> charWrapper)
        {
            MultiByteNCursesWrapper = new NativeWindowMultiByte<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(multiByteWrapper);
            SingleByteNCursesWrapper = new NativeWindowSingleByte<TSingleByte, TChar, TMouseEvent>(singleByteWrapper);
            WideCharNCursesWrapper = new NativeWindowWideChar<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(wideCharWrapper);
            CharNCursesWrapper = new NativeWindowChar<TChar>(charWrapper);
        }

        #region redrawwin
        /// <summary>
        /// The wredrawln routine indicates to curses that some screen
        /// lines are corrupted and should be thrown away before  anything  is  written over  them.It touches the indicated
        /// lines(marking them  changed).   The routine  redrawwin
        /// touches the entire window.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void redrawwin(WindowBaseSafeHandle window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.redrawwin(window), "redrawwin");
        }
        #endregion

        #region waddch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.addch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void waddch(WindowBaseSafeHandle window, in TSingleByte ch)
        {
            SingleByteNCursesWrapper.waddch(window, ch);
        }
        #endregion

        #region waddchnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.addchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void waddchnstr(WindowBaseSafeHandle window, in SingleByteCharString<TSingleByte> txt, int number)
        {
            SingleByteNCursesWrapper.waddchnstr(window, in txt, number);
        }
        #endregion

        #region waddchstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.addchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void waddchstr(WindowBaseSafeHandle window, in SingleByteCharString<TSingleByte> txt)
        {
            SingleByteNCursesWrapper.waddchstr(window, txt);
        }
        #endregion

        #region waddnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.addnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void waddnstr(WindowBaseSafeHandle window, in CharString<TChar> txt, int number)
        {
            CharNCursesWrapper.waddnstr(window, txt, number);
        }
        #endregion

        #region waddstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.addstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void waddstr(WindowBaseSafeHandle window, in CharString<TChar> txt)
        {
            CharNCursesWrapper.waddstr(window, txt);
        }
        #endregion

        #region wattroff
        /// <summary>
        /// see <see cref="NativeStdScrInternal.attroff"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wattroff(WindowBaseSafeHandle window, int attrs)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wattroff(window, attrs), "wattroff");
        }
        #endregion

        #region wattron
        /// <summary>
        /// see <see cref="NativeStdScrInternal.attron"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wattron(WindowBaseSafeHandle window, int attrs)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wattron(window, attrs), "wattron");
        }
        #endregion

        #region wattrset
        /// <summary>
        /// see <see cref="NativeStdScrInternal.attrset"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wattrset(WindowBaseSafeHandle window, int attrs)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wattrset(window, attrs), "wattrset");
        }
        #endregion

        #region wattr_on
        /// <summary>
        /// see <see cref="NativeStdScrInternal.attr_on"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wattr_on(WindowBaseSafeHandle window, ulong attrs)
        {
            SingleByteNCursesWrapper.wattr_on(window, attrs);
        }
        #endregion

        #region wattr_off
        /// <summary>
        /// see <see cref="NativeStdScrInternal.attr_off"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wattr_off(WindowBaseSafeHandle window, ulong attrs)
        {
            SingleByteNCursesWrapper.wattr_off(window, attrs);
        }
        #endregion

        #region wattr_set
        /// <summary>
        /// see <see cref="NativeStdScrInternal.attr_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wattr_set(WindowBaseSafeHandle window, ulong attrs, ushort pair)
        {
            SingleByteNCursesWrapper.wattr_set(window, attrs, pair);
        }
        #endregion

        #region wattr_get
        /// <summary>
        /// see <see cref="NativeStdScrInternal.attr_get"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wattr_get(WindowBaseSafeHandle window, out ulong attrs, out ushort pair)
        {
            SingleByteNCursesWrapper.wattr_get(window, out attrs, out pair);
        }
        #endregion

        #region wbkgd
        /// <summary>
        /// see <see cref="NativeStdScrInternal.bkgd"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wbkgd(WindowBaseSafeHandle window, in TSingleByte bkgd)
        {
            SingleByteNCursesWrapper.wbkgd(window, bkgd);
        }
        #endregion

        #region wbkgdset
        /// <summary>
        /// see <see cref="NativeStdScrInternal.bkgdset"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wbkgdset(WindowBaseSafeHandle window, in TSingleByte bkgd)
        {
            SingleByteNCursesWrapper.wbkgdset(window, bkgd);
        }
        #endregion

        #region wborder
        /// <summary>
        /// see <see cref="NativeStdScrInternal.border"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wborder(WindowBaseSafeHandle window, in TSingleByte ls, in TSingleByte rs, in TSingleByte ts, in TSingleByte bs, in TSingleByte tl, in TSingleByte tr, in TSingleByte bl, in TSingleByte br)
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
        public void box(WindowBaseSafeHandle window, in TSingleByte verch, in TSingleByte horch)
        {
            SingleByteNCursesWrapper.box(window, verch, horch);
        }
        #endregion

        #region wchgat
        /// <summary>
        /// see <see cref="NativeStdScrInternal.chgat"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wchgat(WindowBaseSafeHandle window, int number, ulong attrs, short pair)
        {
            SingleByteNCursesWrapper.wchgat(window, number, attrs, pair);
        }
        #endregion

        #region wclear
        /// <summary>
        /// see <see cref="NativeStdScrInternal.clear"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wclear(WindowBaseSafeHandle window)
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
        public void clearok(WindowBaseSafeHandle window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.clearok(window, bf), "clearok");
        }
        #endregion

        #region wclrtobot
        /// <summary>
        /// see <see cref="NativeStdScrInternal.clrtobot"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wclrtobot(WindowBaseSafeHandle window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wclrtobot(window), "wclrtobot");
        }
        #endregion

        #region wclrtoeol
        /// <summary>
        /// see <see cref="NativeStdScrInternal.clrtoeol"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wclrtoeol(WindowBaseSafeHandle window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wclrtoeol(window), "wclrtoeol");
        }
        #endregion

        #region wcolor_set
        /// <summary>
        /// see <see cref="NativeStdScrInternal.color_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wcolor_set(WindowBaseSafeHandle window, short pair)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wcolor_set(window, pair, IntPtr.Zero), "wcolor_set");
        }
        #endregion

        #region wdelch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.delch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wdelch(WindowBaseSafeHandle window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wdelch(window), "wdelch");
        }
        #endregion

        #region wdeleteln
        /// <summary>
        /// see <see cref="NativeStdScrInternal.deleteln"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wdeleteln(WindowBaseSafeHandle window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wdeleteln(window), "wdeleteln");
        }
        #endregion

        #region wechochar
        /// <summary>
        /// see <see cref="NativeStdScrInternal.echochar"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wechochar(WindowBaseSafeHandle window, in TSingleByte ch)
        {
            SingleByteNCursesWrapper.wechochar(window, ch);
        }
        #endregion

        #region werase
        /// <summary>
        /// see <see cref="NativeStdScrInternal.erase"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void werase(WindowBaseSafeHandle window)
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
        public TSingleByte getbkgd(WindowBaseSafeHandle window)
        {
            return SingleByteNCursesWrapper.getbkgd(window);
        }
        #endregion

        #region wgetch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.getch"/>
        /// when <see cref="NativeNCurses.UseWindowsOverride"/> is set, the default NCurses behaviour gets overridden with a managed implementation
        /// which allows for control modifiers on function keys.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public int wgetch(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.wgetch(window);
        }

        /// <summary>
        /// see <see cref="NativeWindowInternal.wgetch"/>
        /// </summary>
        public bool wgetch(WindowBaseSafeHandle window, out char ch, out Key key)
        {
            return NativeNCurses.VerifyInput(window, "wgetch", NativeNCurses.NCursesWrapper.wgetch(window), out ch, out key);
        }
        #endregion

        #region wgetnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.getnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wgetnstr(WindowBaseSafeHandle window, ref CharString<TChar> str, int count)
        {
            CharNCursesWrapper.wgetnstr(window, ref str, count);
        }
        #endregion

        #region wgetstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.getstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wgetstr(WindowBaseSafeHandle window, ref CharString<TChar> str)
        {
            CharNCursesWrapper.wgetstr(window, ref str);
        }
        #endregion

        #region whline
        /// <summary>
        /// see <see cref="NativeStdScrInternal.hline"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void whline(WindowBaseSafeHandle window, in TSingleByte ch, int count)
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
        public void idcok(WindowBaseSafeHandle window, bool bf)
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
        public int idlok(WindowBaseSafeHandle window, bool bf)
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
        public void immedok(WindowBaseSafeHandle window, bool bf)
        {
            NativeNCurses.NCursesWrapper.immedok(window, bf);
        }
        #endregion

        #region winch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.inch"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <returns>characther with attributes at current position</returns>
        public void winch(WindowBaseSafeHandle window, out TSingleByte ch)
        {
            SingleByteNCursesWrapper.winch(window, out ch);
        }
        #endregion

        #region winchnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.inchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winchnstr(WindowBaseSafeHandle window, ref SingleByteCharString<TSingleByte> chStr, int count, out int read)
        {
            SingleByteNCursesWrapper.winchnstr(window, ref chStr, count, out read);
        }
        #endregion

        #region winchstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.inchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winchstr(WindowBaseSafeHandle window, ref SingleByteCharString<TSingleByte> chStr, out int read)
        {
            SingleByteNCursesWrapper.winchstr(window, ref chStr, out read);
        }
        #endregion

        #region winnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.innstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winnstr(WindowBaseSafeHandle window, ref CharString<TChar> str, int n, out int read)
        {
            CharNCursesWrapper.winnstr(window, ref str, n, out read);
        }
        #endregion

        #region winsch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.insch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winsch(WindowBaseSafeHandle window, in TSingleByte ch)
        {
            SingleByteNCursesWrapper.winsch(window, ch);
        }
        #endregion

        #region winsdelln
        /// <summary>
        /// see <see cref="NativeStdScrInternal.insdelln"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winsdelln(WindowBaseSafeHandle window, int n)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.winsdelln(window, n), "winsdelln");
        }
        #endregion

        #region winsertln
        /// <summary>
        /// see <see cref="NativeStdScrInternal.insertln"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winsertln(WindowBaseSafeHandle window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.winsertln(window), "winsertln");
        }
        #endregion

        #region winsnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.insnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winsnstr(WindowBaseSafeHandle window, in CharString<TChar> str, int n)
        {
            CharNCursesWrapper.winsnstr(window, in str, n);
        }
        #endregion

        #region winsstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.insstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winsstr(WindowBaseSafeHandle window, in CharString<TChar> str)
        {
            CharNCursesWrapper.winsstr(window, in str);
        }
        #endregion

        #region winstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.instr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winstr(WindowBaseSafeHandle window, ref CharString<TChar> str, out int read)
        {
            CharNCursesWrapper.winstr(window, ref str, out read);
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
        public bool is_linetouched(WindowBaseSafeHandle window, int line)
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
        public bool is_wintouched(WindowBaseSafeHandle window)
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
        public void keypad(WindowBaseSafeHandle window, bool bf)
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
        public void leaveok(WindowBaseSafeHandle window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.leaveok(window, bf), "leaveok");
        }
        #endregion

        #region wmove
        /// <summary>
        /// see <see cref="NativeStdScrInternal.move"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wmove(WindowBaseSafeHandle window, int y, int x)
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
        public void mvwaddch(WindowBaseSafeHandle window, int y, int x, in TSingleByte ch)
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
        public void mvwaddchnstr(WindowBaseSafeHandle window, int y, int x, in SingleByteCharString<TSingleByte> chstr, int n)
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
        public void mvwaddchstr(WindowBaseSafeHandle window, int y, int x, in SingleByteCharString<TSingleByte> chstr)
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
        public void mvwaddnstr(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> txt, int n)
        {
            CharNCursesWrapper.mvwaddnstr(window, y, x, in txt, n);
        }
        #endregion

        #region mvwaddstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="waddstr(IntPtr, string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwaddstr(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> txt)
        {
            CharNCursesWrapper.mvwaddstr(window, y, x, in txt);
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
        public void mvwchgat(WindowBaseSafeHandle window, int y, int x, int number, ulong attrs, short pair)
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
        public void mvwdelch(WindowBaseSafeHandle window, int y, int x)
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
        public int mvwgetch(WindowBaseSafeHandle window, int y, int x)
        {
            int ret = 0;
            NCursesException.Verify(NativeNCurses.NCursesWrapper.mvwgetch(window, y, x), "mvwgetch");
            return ret;
        }

        /// <summary>
        /// see <see cref="NativeWindowInternal.mvwgetch(IntPtr, int, int)"/>
        /// </summary>
        public bool mvwgetch(WindowBaseSafeHandle window, int y, int x, out char ch, out Key key)
        {
            return NativeNCurses.VerifyInput(window, "mvwgetch", NativeNCurses.NCursesWrapper.mvwgetch(window, y, x), out ch, out key);
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
        public void mvwgetnstr(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> str, int count)
        {
            CharNCursesWrapper.mvwgetnstr(window, y, x, ref str, count);
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
        public void mvwgetstr(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> str)
        {
            CharNCursesWrapper.mvwgetstr(window, y, x, ref str);
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
        public void mvwhline(WindowBaseSafeHandle window, int y, int x, in TSingleByte ch, int count)
        {
            SingleByteNCursesWrapper.mvwhline(window, y, x, ch, count);
        }
        #endregion

        #region mvwinch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvinch(int, int)"/>
        /// </summary>
        public void mvwinch(WindowBaseSafeHandle window, int y, int x, out TSingleByte ch)
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
        public void mvwinchnstr(WindowBaseSafeHandle window, int y, int x, ref SingleByteCharString<TSingleByte> chStr, int count, out int read)
        {
            SingleByteNCursesWrapper.mvwinchnstr(window, y, x, ref chStr, count, out read);
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
        public void mvwinchstr(WindowBaseSafeHandle window, int y, int x, ref SingleByteCharString<TSingleByte> chStr, out int read)
        {
            SingleByteNCursesWrapper.mvwinchstr(window, y, x, ref chStr, out read);
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
        public void mvwinnstr(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> str, int n, out int read)
        {
            CharNCursesWrapper.mvwinnstr(window, y, x, ref str, n, out read);
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
        public void mvwinsch(WindowBaseSafeHandle window, int y, int x, in TSingleByte ch)
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
        public void mvwinsnstr(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> str, int n)
        {
            CharNCursesWrapper.mvwinsnstr(window, y, x, in str, n);
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
        public void mvwinsstr(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> str)
        {
            CharNCursesWrapper.mvwinsstr(window, y, x, in str);
        }
        #endregion

        #region mvwinstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winstr(IntPtr, StringBuilder)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwinstr(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> str, out int read)
        {
            CharNCursesWrapper.mvwinstr(window, y, x, ref str, out read);
        }
        #endregion

        #region mvwprintw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wprintw(IntPtr, string, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwprintw(WindowBaseSafeHandle window, int y, int x, in CharString<TChar> format, params CharString<TChar>[] argList)
        {
            CharNCursesWrapper.mvwprintw(window, y, x, in format, argList);
        }
        #endregion

        #region mvwscanw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wscanw(IntPtr, StringBuilder, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwscanw(WindowBaseSafeHandle window, int y, int x, ref CharString<TChar> format, params CharString<TChar>[] argList)
        {
            CharNCursesWrapper.mvwscanw(window, y, x, ref format, argList);
        }
        #endregion

        #region mvwvline
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wvline(IntPtr, chtype, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public void mvwvline(WindowBaseSafeHandle window, int y, int x, in TSingleByte ch, int n)
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
        public void nodelay(WindowBaseSafeHandle window, bool bf)
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
        public void notimeout(WindowBaseSafeHandle window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.notimeout(window, bf), "notimeout");
        }
        #endregion

        #region wrefresh
        /// <summary>
        /// see <see cref="NativeStdScrInternal.refresh"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wrefresh(WindowBaseSafeHandle window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wrefresh(window), "wrefresh");
        }
        #endregion

        #region wprintw
        /// <summary>
        /// see <see cref="NativeStdScrInternal.printw(string, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wprintw(WindowBaseSafeHandle window, in CharString<TChar> format, params CharString<TChar>[] argList)
        {
            CharNCursesWrapper.wprintw(window, in format, argList);
        }
        #endregion

        #region wscanw
        /// <summary>
        /// see <see cref="NativeStdScrInternal.scanw"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wscanw(WindowBaseSafeHandle window, ref CharString<TChar> format, params CharString<TChar>[] argList)
        {
            CharNCursesWrapper.wscanw(window, ref format, argList);
        }
        #endregion

        #region wscrl
        /// <summary>
        /// see <see cref="NativeStdScrInternal.scrl(int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wscrl(WindowBaseSafeHandle window, int n)
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
        public void scroll(WindowBaseSafeHandle window)
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
        public void scrollok(WindowBaseSafeHandle window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.scrollok(window, bf), "scrollok");
        }
        #endregion

        #region wsetscrreg
        /// <summary>
        /// see <see cref="NativeStdScrInternal.setscrreg(int, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wsetscrreg(WindowBaseSafeHandle window, int top, int bot)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wsetscrreg(window, top, bot), "wsetscrreg");
        }
        #endregion

        #region wstandout
        /// <summary>
        /// see <see cref="NativeStdScrInternal.standout"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wstandout(WindowBaseSafeHandle window)
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
        public void wstandend(WindowBaseSafeHandle window)
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
        public void syncok(WindowBaseSafeHandle window, bool bf)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.syncok(window, bf), "syncok");
        }
        #endregion

        #region wtimeout
        /// <summary>
        /// see <see cref="NativeStdScrInternal.timeout"/>
        /// </summary>
        public void wtimeout(int delay)
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
        public void touchline(WindowBaseSafeHandle window, int start, int count)
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
        public void touchwin(WindowBaseSafeHandle window)
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
        public void untouchwin(WindowBaseSafeHandle window)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.untouchwin(window), "untouchwin");
        }
        #endregion

        #region wvline
        /// <summary>
        /// see <see cref="NativeStdScrInternal.vline(chtype, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wvline(WindowBaseSafeHandle window, in TSingleByte ch, int n)
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
        public void wcursyncup(WindowBaseSafeHandle window)
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
        public void wsyncdown(WindowBaseSafeHandle window)
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
        public void wnoutrefresh(WindowBaseSafeHandle window)
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
        public void wredrawln(WindowBaseSafeHandle window, int beg_line, int num_lines)
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
        public void wsyncup(WindowBaseSafeHandle window)
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
        public void wtouchln(WindowBaseSafeHandle window, int y, int n, int changed)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wtouchln(window, y, n, changed), "wtouchln");
        }
        #endregion

        #region getattrs
        /// <summary>
        /// returns attributes
        /// </summary>
        /// <returns>attributes</returns>
        public int getattrs(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.getattrs(window);
        }
        #endregion

        #region getcurx
        /// <summary>
        /// The getcury and getcurx functions return the same data as getyx.
        /// </summary>
        /// <returns>attributes</returns>
        public int getcurx(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.getcurx(window);
        }
        #endregion

        #region getcury
        /// <summary>
        /// The getcury and getcurx functions return the same data as getyx.
        /// </summary>
        /// <returns>attributes</returns>
        public int getcury(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.getcury(window);
        }
        #endregion

        #region getbegx
        /// <summary>
        /// The getbegy and getbegx functions return the same data  as  getbegyx.
        /// </summary>
        /// <returns>attributes</returns>
        public int getbegx(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.getbegx(window);
        }
        #endregion

        #region getbegy
        /// <summary>
        /// The getbegy and getbegx functions return the same data  as  getbegyx.
        /// </summary>
        /// <returns>attributes</returns>
        public int getbegy(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.getbegy(window);
        }
        #endregion

        #region getmaxx
        /// <summary>
        /// The getmaxy and getmaxx functions return the same data  as getmaxyx.
        /// </summary>
        /// <returns>attributes</returns>
        public int getmaxx(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.getmaxx(window);
        }
        #endregion

        #region getmaxy
        /// <summary>
        /// The getmaxy and getmaxx functions return the same data  as getmaxyx.
        /// </summary>
        /// <returns>attributes</returns>
        public int getmaxy(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.getmaxy(window);
        }
        #endregion

        #region getparx
        /// <summary>
        /// The getpary and getparx functions return the same data as getparyx.
        /// </summary>
        /// <returns>attributes</returns>
        public int getparx(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.getparx(window);
        }
        #endregion

        #region getpary
        /// <summary>
        /// The getpary and getparx functions return the same data as getparyx.
        /// </summary>
        /// <returns>attributes</returns>
        public int getpary(WindowBaseSafeHandle window)
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
        public void wresize(WindowBaseSafeHandle window, int lines, int columns)
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
        public IntPtr wgetparent(WindowBaseSafeHandle window)
        {
            return NCursesException.Verify(NativeNCurses.NCursesWrapper.wgetparent(window), "wgetparent");
        }
        #endregion

        #region is_cleared
        /// <summary>
        /// returns the value set in clearok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_cleared(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_cleared(window);
        }
        #endregion

        #region is_idcok
        /// <summary>
        /// returns the value set in idcok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_idcok(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_idcok(window);
        }
        #endregion

        #region is_idlok
        /// <summary>
        /// returns the value set in idlok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_idlok(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_idlok(window);
        }
        #endregion

        #region is_immedok
        /// <summary>
        /// returns the value set in immedok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_immedok(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_immedok(window);
        }
        #endregion

        #region is_keypad
        /// <summary>
        /// returns the value set in keypad
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_keypad(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_keypad(window);
        }
        #endregion

        #region is_leaveok
        /// <summary>
        /// returns the value set in leaveok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_leaveok(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_leaveok(window);
        }
        #endregion

        #region is_nodelay
        /// <summary>
        /// returns the value set in nodelay
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_nodelay(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_nodelay(window);
        }
        #endregion

        #region is_notimeout
        /// <summary>
        /// returns the value set in notimeout
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_notimeout(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_notimeout(window);
        }
        #endregion

        #region is_pad
        /// <summary>
        /// returns TRUE if the window is a pad i.e., created by <see cref="NativeNCurses.newpad"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_pad(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_pad(window);
        }
        #endregion

        #region is_scrollok
        /// <summary>
        /// returns the value set in scrollok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_scrollok(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_scrollok(window);
        }
        #endregion

        #region is_subwin
        /// <summary>
        /// returns TRUE if the window is a subwindow, i.e., created by subwin or derwin
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_subwin(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_subwin(window);
        }
        #endregion

        #region is_syncok
        /// <summary>
        /// returns the value set in syncok
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool is_syncok(WindowBaseSafeHandle window)
        {
            return NativeNCurses.NCursesWrapper.is_syncok(window);
        }
        #endregion

        #region wgetdelay
        /// <summary>
        /// returns the delay timeout as set in wtimeout.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public int wgetdelay(WindowBaseSafeHandle window)
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
        public void wgetscrreg(WindowBaseSafeHandle window, ref int top, ref int bottom)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wgetscrreg(window, ref top, ref bottom), "wgetscrreg");
        }
        #endregion

        #region wadd_wch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.add_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wadd_wch(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            MultiByteNCursesWrapper.wadd_wch(window, wch);
        }
        #endregion

        #region wadd_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.add_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wadd_wchnstr(WindowBaseSafeHandle window, in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr, int n)
        {
            MultiByteNCursesWrapper.wadd_wchnstr(window, in wchStr, n);
        }
        #endregion

        #region wadd_wchstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.add_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wadd_wchstr(WindowBaseSafeHandle window, in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr)
        {
            MultiByteNCursesWrapper.wadd_wchstr(window, in wchStr);
        }
        #endregion

        #region waddnwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.addnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void waddnwstr(WindowBaseSafeHandle window, in WideCharString<TWideChar> wstr, int n)
        {
            WideCharNCursesWrapper.waddnwstr(window, wstr, n);
        }
        #endregion

        #region waddwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.addwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void waddwstr(WindowBaseSafeHandle window, in WideCharString<TWideChar> wstr)
        {
            WideCharNCursesWrapper.waddwstr(window, wstr);
        }
        #endregion

        #region wbkgrnd
        /// <summary>
        /// see <see cref="NativeStdScrInternal.bkgrnd"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wbkgrnd(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            MultiByteNCursesWrapper.wbkgrnd(window, wch);
        }
        #endregion

        #region wbkgrndset
        /// <summary>
        /// see <see cref="NativeStdScrInternal.bkgrndset"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wbkgrndset(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            MultiByteNCursesWrapper.wbkgrndset(window, wch);
        }
        #endregion

        #region wborder_set
        /// <summary>
        /// see <see cref="NativeStdScrInternal.border_set"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wborder_set(WindowBaseSafeHandle window, in TMultiByte ls, in TMultiByte rs, in TMultiByte ts, in TMultiByte bs, in TMultiByte tl, in TMultiByte tr,
            in TMultiByte bl, in TMultiByte br)
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
        public void box_set(WindowBaseSafeHandle window, in TMultiByte verch, in TMultiByte horch)
        {
            MultiByteNCursesWrapper.box_set(window, verch, horch);
        }
        #endregion

        #region wecho_wchar
        /// <summary>
        /// seee <see cref="NativeStdScrInternal.echo_wchar"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wecho_wchar(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            MultiByteNCursesWrapper.wecho_wchar(window, wch);
        }
        #endregion

        #region wget_wch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.get_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool wget_wch(WindowBaseSafeHandle window, out TWideChar wch, out Key key)
        {
            return WideCharNCursesWrapper.wget_wch(window, out wch, out key);
        }
        #endregion

        #region wget_wstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.get_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wget_wstr(WindowBaseSafeHandle window, ref WideCharString<TWideChar> wstr)
        {
            WideCharNCursesWrapper.wget_wstr(window, ref wstr);
        }
        #endregion

        #region wgetbkgrnd
        /// <summary>
        /// see <see cref="NativeStdScrInternal.getbkgrnd"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wgetbkgrnd(WindowBaseSafeHandle window, out TMultiByte wch)
        {
            MultiByteNCursesWrapper.wgetbkgrnd(window, out wch);
        }
        #endregion

        #region wgetn_wstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.getn_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wgetn_wstr(WindowBaseSafeHandle window, ref WideCharString<TWideChar> wstr, int n)
        {
            WideCharNCursesWrapper.wgetn_wstr(window, ref wstr, n);
        }
        #endregion

        #region whline_set
        /// <summary>
        /// see <see cref="NativeStdScrInternal.hline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void whline_set(WindowBaseSafeHandle window, in TMultiByte wch, int n)
        {
            MultiByteNCursesWrapper.whline_set(window, wch, n);
        }
        #endregion

        #region win_wch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.in_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void win_wch(WindowBaseSafeHandle window, out TMultiByte wcval)
        {
            MultiByteNCursesWrapper.win_wch(window, out wcval);
        }
        #endregion

        #region win_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.in_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void win_wchnstr(WindowBaseSafeHandle window, ref MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr, int n)
        {
            MultiByteNCursesWrapper.win_wchnstr(window, ref wchStr, n);
        }
        #endregion

        #region win_wchstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.in_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void win_wchstr(WindowBaseSafeHandle window, ref MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr)
        {
            MultiByteNCursesWrapper.win_wchstr(window, ref wchStr);
        }
        #endregion

        #region winnwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.innwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winnwstr(WindowBaseSafeHandle window, ref WideCharString<TWideChar> str, int n, out int read)
        {
            WideCharNCursesWrapper.winnwstr(window, ref str, n, out read);
        }
        #endregion

        #region wins_nwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.ins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wins_nwstr(WindowBaseSafeHandle window, in WideCharString<TWideChar> str, int n)
        {
            WideCharNCursesWrapper.wins_nwstr(window, str, n);
        }
        #endregion

        #region wins_wch

        /// <summary>
        /// see <see cref="NativeStdScrInternal.ins_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wins_wch(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            MultiByteNCursesWrapper.wins_wch(window, wch);
        }
        #endregion

        #region wins_wstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.ins_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wins_wstr(WindowBaseSafeHandle window, in WideCharString<TWideChar> str)
        {
            WideCharNCursesWrapper.wins_wstr(window, str);
        }
        #endregion

        #region winwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.inwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void winwstr(WindowBaseSafeHandle window, ref WideCharString<TWideChar> str)
        {
            WideCharNCursesWrapper.winwstr(window, ref str);
        }
        #endregion

        #region mvwadd_wch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvadd_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwadd_wch(WindowBaseSafeHandle window, int y, int x, in TMultiByte wch)
        {
            MultiByteNCursesWrapper.mvwadd_wch(window, y, x, wch);
        }
        #endregion

        #region mvwadd_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvadd_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwadd_wchnstr(WindowBaseSafeHandle window, int y, int x, in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr, int n)
        {
            MultiByteNCursesWrapper.mvwadd_wchnstr(window, y, x, wchStr, n);
        }
        #endregion

        #region mvwadd_wchstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvadd_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwadd_wchstr(WindowBaseSafeHandle window, int y, int x, in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr)
        {
            MultiByteNCursesWrapper.mvwadd_wchstr(window, y, x, wchStr);
        }
        #endregion

        #region mvwaddnwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvaddnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwaddnwstr(WindowBaseSafeHandle window, int y, int x, in WideCharString<TWideChar> wstr, int n)
        {
            WideCharNCursesWrapper.mvwaddnwstr(window, y, x, in wstr, n);
        }
        #endregion

        #region mvwaddwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvaddwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwaddwstr(WindowBaseSafeHandle window, int y, int x, in WideCharString<TWideChar> wstr)
        {
            WideCharNCursesWrapper.mvwaddwstr(window, y, x, in wstr);
        }
        #endregion

        #region mvwget_wch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvget_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public bool mvwget_wch(WindowBaseSafeHandle window, int y, int x, out TWideChar wch, out Key key)
        {
            return WideCharNCursesWrapper.mvwget_wch(window, y, x, out wch, out key);
        }
        #endregion

        #region mvwget_wstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvget_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwget_wstr(WindowBaseSafeHandle window, int y, int x, ref WideCharString<TWideChar> wstr)
        {
            WideCharNCursesWrapper.mvwget_wstr(window, y, x, ref wstr);
        }
        #endregion

        #region mvwgetn_wstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvgetn_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwgetn_wstr(WindowBaseSafeHandle window, int y, int x, ref WideCharString<TWideChar> wstr, int n)
        {
            WideCharNCursesWrapper.mvwgetn_wstr(window, y, x, ref wstr, n);
        }
        #endregion

        #region mvwhline_set
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvhline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwhline_set(WindowBaseSafeHandle window, int y, int x, in TMultiByte wch, int n)
        {
            MultiByteNCursesWrapper.mvwhline_set(window, y, x, wch, n);
        }
        #endregion

        #region mvwin_wch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvin_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwin_wch(WindowBaseSafeHandle window, int y, int x, out TMultiByte wcval)
        {
            MultiByteNCursesWrapper.mvwin_wch(window, y, x, out wcval);
        }
        #endregion

        #region mvwin_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvin_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">array reference to store the complex characters in</param>
        public void mvwin_wchnstr(WindowBaseSafeHandle window, int y, int x, ref MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr, int n)
        {
            MultiByteNCursesWrapper.mvwin_wchnstr(window, y, x, ref wchStr, n);
        }
        #endregion

        #region mvwin_wchstr
        /// <summary>
        /// see <see cref="mvwin_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public void mvwin_wchstr(WindowBaseSafeHandle window, int y, int x, ref MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr)
        {
            MultiByteNCursesWrapper.mvwin_wchstr(window, y, x, ref wchStr);
        }
        #endregion

        #region mvwinnwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvinnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwinnwstr(WindowBaseSafeHandle window, int y, int x, ref WideCharString<TWideChar> str, int n, out int read)
        {
            WideCharNCursesWrapper.mvwinnwstr(window, y, x, ref str, n, out read);
        }
        #endregion

        #region mvwins_nwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwins_nwstr(WindowBaseSafeHandle window, int y, int x, in WideCharString<TWideChar> str, int n)
        {
            WideCharNCursesWrapper.mvwins_nwstr(window, y, x, in str, n);
        }
        #endregion

        #region mvwins_wch
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvins_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwins_wch(WindowBaseSafeHandle window, int y, int x, in TMultiByte wch)
        {
            MultiByteNCursesWrapper.mvwins_wch(window, y, x, wch);
        }
        #endregion

        #region mvwins_wstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvins_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwins_wstr(WindowBaseSafeHandle window, int y, int x, in WideCharString<TWideChar> wstr)
        {
            WideCharNCursesWrapper.mvwins_wstr(window, y, x, in wstr);
        }
        #endregion

        #region mvwinwstr
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvinwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwinwstr(WindowBaseSafeHandle window, int y, int x, ref WideCharString<TWideChar> str)
        {
            WideCharNCursesWrapper.mvwinwstr(window, y, x, ref str);
        }
        #endregion

        #region mvwvline_set
        /// <summary>
        /// see <see cref="NativeStdScrInternal.mvvline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void mvwvline_set(WindowBaseSafeHandle window, int y, int x, in TMultiByte wch, int n)
        {
            MultiByteNCursesWrapper.mvwvline_set(window, y, x, wch, n);
        }
        #endregion

        #region wvline_set
        /// <summary>
        /// see <see cref="NativeStdScrInternal.vline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public void wvline_set(WindowBaseSafeHandle window, in TMultiByte wch, int n)
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
        public bool wenclose(WindowBaseSafeHandle window, int y, int x)
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
        public bool wmouse_trafo(WindowBaseSafeHandle win, ref int pY, ref int pX, bool to_screen)
        {
            return NativeNCurses.NCursesWrapper.wmouse_trafo(win, ref pY, ref pX, to_screen);
        }
        #endregion

        #region Interface implementations
        public void box_set(WindowBaseSafeHandle win, in IMultiByteNCursesChar verch, in IMultiByteNCursesChar horch)
        {
            TMultiByte verchCasted = this.MultiByteNCursesWrapper.CastChar(in verch);
            TMultiByte horchCasted = this.MultiByteNCursesWrapper.CastChar(in horch);
            this.box_set(win, in verchCasted, in horchCasted);
        }

        public void mvwadd_wch(WindowBaseSafeHandle win, int y, int x, in IMultiByteNCursesChar wch)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.mvwadd_wch(win, y, x, in casted);
        }

        public void mvwadd_wchnstr(WindowBaseSafeHandle win, int y, int x, in IMultiByteNCursesCharString wchStr, int n)
        {
            MultiByteCharString<TMultiByte, TWideChar, TSingleByte> casted = this.MultiByteNCursesWrapper.CastString(wchStr);
            this.mvwadd_wchnstr(win, y, x, in casted, n);
        }

        public void mvwadd_wchstr(WindowBaseSafeHandle win, int y, int x, in IMultiByteNCursesCharString wchStr)
        {
            MultiByteCharString<TMultiByte, TWideChar, TSingleByte> casted = this.MultiByteNCursesWrapper.CastString(wchStr);
            this.mvwadd_wchstr(win, y, x, in casted);
        }

        public void mvwhline_set(WindowBaseSafeHandle win, int y, int x, in IMultiByteNCursesChar wch, int n)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.mvwhline_set(win, y, x, in casted, n);
        }

        public void mvwin_wch(WindowBaseSafeHandle win, int y, int x, out IMultiByteNCursesChar wch)
        {
            this.mvwin_wch(win, y, x, out TMultiByte wcVal);
            wch = wcVal;
        }

        public void mvwin_wchnstr(WindowBaseSafeHandle win, int y, int x, ref IMultiByteNCursesCharString wchStr, int n)
        {
            MultiByteCharString<TMultiByte, TWideChar, TSingleByte> casted = this.MultiByteNCursesWrapper.CastString(wchStr);
            this.mvwin_wchnstr(win, y, x, ref casted, n);
        }

        public void mvwin_wchstr(WindowBaseSafeHandle win, int y, int x, ref IMultiByteNCursesCharString wchStr)
        {
            MultiByteCharString<TMultiByte, TWideChar, TSingleByte> casted = this.MultiByteNCursesWrapper.CastString(wchStr);
            this.mvwin_wchstr(win, y, x, ref casted);
        }

        public void mvwins_wch(WindowBaseSafeHandle win, int y, int x, in IMultiByteNCursesChar wch)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.mvwins_wch(win, y, x, in casted);
        }

        public void mvwvline_set(WindowBaseSafeHandle win, int y, int x, in IMultiByteNCursesChar wch, int n)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.mvwvline_set(win, y, x, in casted, n);
        }

        public void wadd_wch(WindowBaseSafeHandle win, in IMultiByteNCursesChar wch)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.wadd_wch(win, in casted);
        }

        public void wadd_wchnstr(WindowBaseSafeHandle win, in IMultiByteNCursesCharString wchStr, int n)
        {
            MultiByteCharString<TMultiByte, TWideChar, TSingleByte> casted = this.MultiByteNCursesWrapper.CastString(wchStr);
            this.wadd_wchnstr(win, in casted, n);
        }

        public void wadd_wchstr(WindowBaseSafeHandle win, in IMultiByteNCursesCharString wchStr)
        {
            MultiByteCharString<TMultiByte, TWideChar, TSingleByte> casted = this.MultiByteNCursesWrapper.CastString(wchStr);
            this.wadd_wchstr(win, in casted);
        }

        public void wbkgrnd(WindowBaseSafeHandle win, in IMultiByteNCursesChar wch)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.wbkgrnd(win, in casted);
        }

        public void wbkgrndset(WindowBaseSafeHandle win, in IMultiByteNCursesChar wch)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.wbkgrndset(win, in casted);
        }

        public void wborder_set(WindowBaseSafeHandle win, in IMultiByteNCursesChar ls, in IMultiByteNCursesChar rs, in IMultiByteNCursesChar ts, in IMultiByteNCursesChar bs, in IMultiByteNCursesChar tl, in IMultiByteNCursesChar tr, in IMultiByteNCursesChar bl, in IMultiByteNCursesChar br)
        {
            TMultiByte cls = this.MultiByteNCursesWrapper.CastChar(ls);
            TMultiByte crs = this.MultiByteNCursesWrapper.CastChar(rs);
            TMultiByte cts = this.MultiByteNCursesWrapper.CastChar(ts);
            TMultiByte cbs = this.MultiByteNCursesWrapper.CastChar(bs);
            TMultiByte ctl = this.MultiByteNCursesWrapper.CastChar(tl);
            TMultiByte ctr = this.MultiByteNCursesWrapper.CastChar(tr);
            TMultiByte cbl = this.MultiByteNCursesWrapper.CastChar(bl);
            TMultiByte cbr = this.MultiByteNCursesWrapper.CastChar(br);
            this.wborder_set(win, in cls, in crs, in cts, in cbs, in ctl, in ctr, in cbl, in cbr);
        }

        public void wecho_wchar(WindowBaseSafeHandle win, in IMultiByteNCursesChar wch)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.wecho_wchar(win, in wch);
        }

        public void wgetbkgrnd(WindowBaseSafeHandle win, out IMultiByteNCursesChar wch)
        {
            this.wgetbkgrnd(win, out TMultiByte wcVal);
            wch = wcVal;
        }

        public void whline_set(WindowBaseSafeHandle win, in IMultiByteNCursesChar wch, int n)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.whline_set(win, in casted, n);
        }

        public void win_wch(WindowBaseSafeHandle win, out IMultiByteNCursesChar wch)
        {
            this.win_wch(win, out TMultiByte wcVal);
            wch = wcVal;
        }

        public void win_wchnstr(WindowBaseSafeHandle win, ref IMultiByteNCursesCharString wchStr, int n)
        {
            MultiByteCharString<TMultiByte, TWideChar, TSingleByte> casted = this.MultiByteNCursesWrapper.CastString(wchStr);
            this.win_wchnstr(win, ref casted, n);
        }

        public void win_wchstr(WindowBaseSafeHandle win, ref IMultiByteNCursesCharString wchStr)
        {
            MultiByteCharString<TMultiByte, TWideChar, TSingleByte> casted = this.MultiByteNCursesWrapper.CastString(wchStr);
            this.win_wchstr(win, ref casted);
        }

        public void wins_wch(WindowBaseSafeHandle win, in IMultiByteNCursesChar wch)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.wins_wch(win, in casted);
        }

        public void wvline_set(WindowBaseSafeHandle win, in IMultiByteNCursesChar wch, int n)
        {
            TMultiByte casted = this.MultiByteNCursesWrapper.CastChar(wch);
            this.wvline_set(win, in casted, n);
        }

        public void mvwaddnwstr(WindowBaseSafeHandle window, int y, int x, in IMultiByteCharString wstr, int n)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.mvwaddnwstr(window, y, x, in casted, n);
        }

        public void mvwaddwstr(WindowBaseSafeHandle window, int y, int x, in IMultiByteCharString wstr)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.mvwaddwstr(window, y, x, in casted);
        }

        public bool mvwget_wch(WindowBaseSafeHandle window, int y, int x, out IMultiByteChar wch, out Key key)
        {
            bool ret = this.mvwget_wch(window, y, x, out TWideChar wcVal, out key);
            wch = wcVal;
            return ret;
        }

        public void mvwget_wstr(WindowBaseSafeHandle window, int y, int x, ref IMultiByteCharString wstr)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.mvwget_wstr(window, y, x, ref casted);
        }

        public void mvwgetn_wstr(WindowBaseSafeHandle window, int y, int x, ref IMultiByteCharString wstr, int n)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.mvwgetn_wstr(window, y, x, ref casted, n);
        }

        public void mvwinnwstr(WindowBaseSafeHandle window, int y, int x, ref IMultiByteCharString wstr, int n, out int read)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.mvwinnwstr(window, y, x, ref casted, n, out read);
        }

        public void mvwins_nwstr(WindowBaseSafeHandle window, int y, int x, in IMultiByteCharString wstr, int n)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.mvwins_nwstr(window, y, x, in casted, n);
        }

        public void mvwins_wstr(WindowBaseSafeHandle window, int y, int x, in IMultiByteCharString wstr)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.mvwins_wstr(window, y, x, in casted);
        }

        public void mvwinwstr(WindowBaseSafeHandle window, int y, int x, ref IMultiByteCharString wstr)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.mvwinwstr(window, y, x, ref casted);
        }

        public void waddnwstr(WindowBaseSafeHandle window, in IMultiByteCharString wstr, int n)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.waddnwstr(window, in casted, n);
        }

        public void waddwstr(WindowBaseSafeHandle window, in IMultiByteCharString wstr)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.waddwstr(window, in casted);
        }

        public bool wget_wch(WindowBaseSafeHandle window, out IMultiByteChar wch, out Key key)
        {
            bool ret = this.wget_wch(window, out TWideChar wcVal, out key);
            wch = wcVal;
            return ret;
        }

        public void wget_wstr(WindowBaseSafeHandle window, ref IMultiByteCharString wstr)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.wget_wstr(window, ref casted);
        }

        public void wgetn_wstr(WindowBaseSafeHandle window, ref IMultiByteCharString wstr, int n)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.wgetn_wstr(window, ref casted, n);
        }

        public void winnwstr(WindowBaseSafeHandle window, ref IMultiByteCharString wstr, int count, out int read)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.winnwstr(window, ref casted, count, out read);
        }

        public void winwstr(WindowBaseSafeHandle window, ref IMultiByteCharString wstr)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.winwstr(window, ref casted);
        }

        public void wins_nwstr(WindowBaseSafeHandle window, in IMultiByteCharString wstr, int n)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.wins_nwstr(window, in casted, n);
        }

        public void wins_wstr(WindowBaseSafeHandle window, in IMultiByteCharString wstr)
        {
            WideCharString<TWideChar> casted = this.WideCharNCursesWrapper.CastString(wstr);
            this.wins_wstr(window, in casted);
        }

        public void box(WindowBaseSafeHandle window, in ISingleByteNCursesChar verch, in ISingleByteNCursesChar horch)
        {
            TSingleByte cv = this.SingleByteNCursesWrapper.CastChar(verch);
            TSingleByte hv = this.SingleByteNCursesWrapper.CastChar(horch);
            this.box(window, in cv, in hv);
        }

        ISingleByteNCursesChar INativeWindowSingleByte<ISingleByteNCursesChar, ISingleByteNCursesCharString>.getbkgd(WindowBaseSafeHandle window)
        {
            return this.getbkgd(window);
        }

        public void mvwaddch(WindowBaseSafeHandle window, int y, int x, in ISingleByteNCursesChar ch)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(ch);
            this.mvwaddch(window, y, x, casted);
        }

        public void mvwaddchnstr(WindowBaseSafeHandle window, int y, int x, in ISingleByteNCursesCharString chstr, int n)
        {
            SingleByteCharString<TSingleByte> casted = this.SingleByteNCursesWrapper.CastString(chstr);
            this.mvwaddchnstr(window, y, x, in casted, n);
        }

        public void mvwaddchstr(WindowBaseSafeHandle window, int y, int x, in ISingleByteNCursesCharString chstr)
        {
            SingleByteCharString<TSingleByte> casted = this.SingleByteNCursesWrapper.CastString(chstr);
            this.mvwaddchstr(window, y, x, in casted);
        }

        public void mvwhline(WindowBaseSafeHandle window, int y, int x, in ISingleByteNCursesChar ch, int count)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(ch);
            this.mvwhline(window, y, x, in casted, count);
        }

        public void mvwinch(WindowBaseSafeHandle window, int y, int x, out ISingleByteNCursesChar ch)
        {
            this.mvwinch(window, y, x, out TSingleByte cVal);
            ch = cVal;
        }

        public void mvwinchnstr(WindowBaseSafeHandle window, int y, int x, ref ISingleByteNCursesCharString chStr, int count, out int read)
        {
            SingleByteCharString<TSingleByte> casted = this.SingleByteNCursesWrapper.CastString(chStr);
            this.mvwinchnstr(window, y, x, ref casted, count, out read);
        }

        public void mvwinchstr(WindowBaseSafeHandle window, int y, int x, ref ISingleByteNCursesCharString chStr, out int read)
        {
            SingleByteCharString<TSingleByte> casted = this.SingleByteNCursesWrapper.CastString(chStr);
            this.mvwinchstr(window, y, x, ref casted, out read);
        }

        public void mvwinsch(WindowBaseSafeHandle window, int y, int x, in ISingleByteNCursesChar ch)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(ch);
            this.mvwinsch(window, y, x, in casted);
        }

        public void mvwvline(WindowBaseSafeHandle window, int y, int x, in ISingleByteNCursesChar ch, int n)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(ch);
            this.mvwvline(window, y, x, in casted, n);
        }

        public void waddch(WindowBaseSafeHandle window, in ISingleByteNCursesChar ch)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(ch);
            this.waddch(window, in casted);
        }

        public void waddchnstr(WindowBaseSafeHandle window, in ISingleByteNCursesCharString chstr, int number)
        {
            SingleByteCharString<TSingleByte> casted = this.SingleByteNCursesWrapper.CastString(chstr);
            this.waddchnstr(window, in casted, number);
        }

        public void waddchstr(WindowBaseSafeHandle window, in ISingleByteNCursesCharString chstr)
        {
            SingleByteCharString<TSingleByte> casted = this.SingleByteNCursesWrapper.CastString(chstr);
            this.waddchstr(window, in casted);
        }

        public void wbkgd(WindowBaseSafeHandle window, in ISingleByteNCursesChar bkgd)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(bkgd);
            this.wbkgd(window, in casted);
        }

        public void wbkgdset(WindowBaseSafeHandle window, in ISingleByteNCursesChar bkgd)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(bkgd);
            this.wbkgdset(window, casted);
        }

        public void wborder(WindowBaseSafeHandle window, in ISingleByteNCursesChar ls, in ISingleByteNCursesChar rs, in ISingleByteNCursesChar ts, in ISingleByteNCursesChar bs, in ISingleByteNCursesChar tl, in ISingleByteNCursesChar tr, in ISingleByteNCursesChar bl, in ISingleByteNCursesChar br)
        {
            TSingleByte cls = this.SingleByteNCursesWrapper.CastChar(ls);
            TSingleByte crs = this.SingleByteNCursesWrapper.CastChar(rs);
            TSingleByte cts = this.SingleByteNCursesWrapper.CastChar(ts);
            TSingleByte cbs = this.SingleByteNCursesWrapper.CastChar(bs);
            TSingleByte ctl = this.SingleByteNCursesWrapper.CastChar(tl);
            TSingleByte ctr = this.SingleByteNCursesWrapper.CastChar(tr);
            TSingleByte cbl = this.SingleByteNCursesWrapper.CastChar(bl);
            TSingleByte cbr = this.SingleByteNCursesWrapper.CastChar(br);
            this.wborder(window, in cls, in crs, in cts, in cbs, in ctl, in ctr, in cbl, in cbr);
        }

        public void wechochar(WindowBaseSafeHandle window, in ISingleByteNCursesChar ch)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(ch);
            this.wechochar(window, in casted);
        }

        public void whline(WindowBaseSafeHandle window, in ISingleByteNCursesChar ch, int count)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(ch);
            this.whline(window, in casted, count);
        }

        public void winch(WindowBaseSafeHandle window, out ISingleByteNCursesChar ch)
        {
            this.winch(window, out TSingleByte cVal);
            ch = cVal;
        }

        public void winchnstr(WindowBaseSafeHandle window, ref ISingleByteNCursesCharString txt, int count, out int read)
        {
            SingleByteCharString<TSingleByte> casted = this.SingleByteNCursesWrapper.CastString(txt);
            this.winchnstr(window, ref casted, count, out read);
        }

        public void winchstr(WindowBaseSafeHandle window, ref ISingleByteNCursesCharString txt, out int read)
        {
            SingleByteCharString<TSingleByte> casted = this.SingleByteNCursesWrapper.CastString(txt);
            this.winchstr(window, ref casted, out read);
        }

        public void winsch(WindowBaseSafeHandle window, in ISingleByteNCursesChar ch)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(ch);
            this.winsch(window, casted);
        }

        public void wvline(WindowBaseSafeHandle window, in ISingleByteNCursesChar ch, int n)
        {
            TSingleByte casted = this.SingleByteNCursesWrapper.CastChar(ch);
            this.wvline(window, in casted, n);
        }

        public void mvwaddnstr(WindowBaseSafeHandle window, int y, int x, in ISingleByteCharString str, int n)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.mvwaddnstr(window, y, x, in casted, n);
        }

        public void mvwaddstr(WindowBaseSafeHandle window, int y, int x, in ISingleByteCharString str)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.mvwaddstr(window, y, x, in casted);
        }

        public void mvwgetnstr(WindowBaseSafeHandle window, int y, int x, ref ISingleByteCharString str, int n)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.mvwgetnstr(window, y, x, ref casted, n);
        }

        public void mvwgetstr(WindowBaseSafeHandle window, int y, int x, ref ISingleByteCharString str)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.mvwgetstr(window, y, x, ref casted);
        }

        public void mvwinnstr(WindowBaseSafeHandle window, int y, int x, ref ISingleByteCharString str, int n, out int read)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.mvwinnstr(window, y, x, ref casted, n, out read);
        }

        public void mvwinsnstr(WindowBaseSafeHandle window, int y, int x, in ISingleByteCharString str, int n)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.mvwinsnstr(window, y, x, in casted, n);
        }

        public void mvwinsstr(WindowBaseSafeHandle window, int y, int x, in ISingleByteCharString str)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.mvwinsstr(window, y, x, in casted);
        }

        public void mvwinstr(WindowBaseSafeHandle window, int y, int x, ref ISingleByteCharString str, out int read)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.mvwinstr(window, y, x, ref casted, out read);
        }

        public void mvwprintw(WindowBaseSafeHandle window, int y, int x, in ISingleByteCharString format, params ISingleByteCharString[] argList)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(format);

            CharString<TChar>[] argListCasted = new CharString<TChar>[argList.Length];
            for (int i = 0; i < argList.Length; i++)
            {
                argListCasted[i] = this.CharNCursesWrapper.CastString(argList[i]);
            }

            this.mvwprintw(window, y, x, in casted, argListCasted);
        }

        public void mvwscanw(WindowBaseSafeHandle window, int y, int x, ref ISingleByteCharString format, params ISingleByteCharString[] argList)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(format);

            CharString<TChar>[] argListCasted = new CharString<TChar>[argList.Length];
            for (int i = 0; i < argList.Length; i++)
            {
                argListCasted[i] = this.CharNCursesWrapper.CastString(argList[i]);
            }

            this.mvwscanw(window, y, x, ref casted, argListCasted);
        }

        public void waddnstr(WindowBaseSafeHandle window, in ISingleByteCharString str, int number)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.waddnstr(window, in casted, number);
        }

        public void waddstr(WindowBaseSafeHandle window, in ISingleByteCharString str)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.waddstr(window, in casted);
        }

        public void wgetnstr(WindowBaseSafeHandle window, ref ISingleByteCharString str, int count)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.wgetnstr(window, ref casted, count);
        }

        public void wgetstr(WindowBaseSafeHandle window, ref ISingleByteCharString str)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.wgetstr(window, ref casted);
        }

        public void winnstr(WindowBaseSafeHandle window, ref ISingleByteCharString str, int count, out int read)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.winnstr(window, ref casted, count, out read);
        }

        public void winstr(WindowBaseSafeHandle window, ref ISingleByteCharString str, out int read)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.winstr(window, ref casted, out read);
        }

        public void winsnstr(WindowBaseSafeHandle window, in ISingleByteCharString str, int n)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.winsnstr(window, in casted, n);
        }

        public void winsstr(WindowBaseSafeHandle window, in ISingleByteCharString str)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);
            this.winsstr(window, in casted);
        }

        public void wprintw(WindowBaseSafeHandle window, in ISingleByteCharString format, params ISingleByteCharString[] argList)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(format);

            CharString<TChar>[] argListCasted = new CharString<TChar>[argList.Length];
            for (int i = 0; i < argList.Length; i++)
            {
                argListCasted[i] = this.CharNCursesWrapper.CastString(argList[i]);
            }

            this.wprintw(window, in casted, argListCasted);
        }

        public void wscanw(WindowBaseSafeHandle window, ref ISingleByteCharString str, params ISingleByteCharString[] argList)
        {
            CharString<TChar> casted = this.CharNCursesWrapper.CastString(str);

            CharString<TChar>[] argListCasted = new CharString<TChar>[argList.Length];
            for (int i = 0; i < argList.Length; i++)
            {
                argListCasted[i] = this.CharNCursesWrapper.CastString(argList[i]);
            }

            this.wscanw(window, ref casted, argListCasted);
        }
        #endregion
    }
}
