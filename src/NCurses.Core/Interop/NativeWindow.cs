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
    /// native window methods.
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    internal static class NativeWindow
    {
        #region waddch
        /// <summary>
        /// see <see cref="NativeStdScr.addch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddch(IntPtr window, chtype ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.waddch(window, ch), "waddch");
        }

        /// <summary>
        /// see <see cref="waddch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void waddch_t(IntPtr window, chtype ch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.waddch(window, ch);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "waddch");
        }
        #endregion

        #region AlternateThreading wadch
        //public static int waddch_t(IntPtr window, uint ch)
        //{
        //    IntPtr chPtr = Marshal.AllocHGlobal(Marshal.SizeOf(ch));
        //    Marshal.StructureToPtr(ch, chPtr, true);
        //    GC.AddMemoryPressure(Marshal.SizeOf(ch));
        //    try
        //    {
        //        return NCurses.NativeNCurses.NCursesWrapper.use_window(window, Marshal.GetFunctionPointerForDelegate<NCURSES_WINDOW_CB>(waddch_t_callback), IntPtr.Zero);
        //    }
        //    finally
        //    {
        //        Marshal.FreeHGlobal(chPtr);
        //        GC.RemoveMemoryPressure(Marshal.SizeOf(ch));
        //    }
        //}

        //public static int waddch_t_callback(IntPtr window, IntPtr args)
        //{
        //    uint ch = Marshal.PtrToStructure<uint>(args);
        //    return NativeNCurses.NCursesWrapper.waddch(window, ch);
        //}
        #endregion

        #region waddchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.addchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddchnstr(IntPtr window, chtype[] txt, int number)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.waddchnstr(window, arrayPtr, number), "waddchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="waddchnstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void waddchnstr_t(IntPtr window, chtype[] txt, int number)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.waddchnstr(window, arrayPtr, number);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "waddchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region waddchstr
        /// <summary>
        /// see <see cref="NativeStdScr.addchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddchstr(IntPtr window, chtype[] txt)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.waddchstr(window, arrayPtr), "waddchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="waddchstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void waddchstr_t(IntPtr window, chtype[] txt)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.waddchstr(window, arrayPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "waddchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region waddnstr
        /// <summary>
        /// see <see cref="NativeStdScr.addnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddnstr(IntPtr window, string txt, int number)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.waddnstr(window, txt, number), "waddnstr");
        }

        /// <summary>
        /// see <see cref="waddnstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void waddnstr_t(IntPtr window, string txt, int number)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.waddnstr(window, txt, number);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "waddnstr");
        }
        #endregion

        #region waddstr
        /// <summary>
        /// see <see cref="NativeStdScr.addstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddstr(IntPtr window, string txt)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.waddstr(window, txt), "waddstr");
        }

        /// <summary>
        /// see <see cref="waddstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void waddstr_t(IntPtr window, string txt)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.waddstr(window, txt);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "waddstr");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wattroff(window, attrs), "wattroff");
        }

        /// <summary>
        /// see <see cref="wattroff"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wattroff_t(IntPtr window, int attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wattroff(window, attrs);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wattroff");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wattron(window, attrs), "wattron");
        }

        /// <summary>
        /// see <see cref="wattron"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wattron_t(IntPtr window, int attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wattron(window, attrs);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wattron");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wattrset(window, attrs), "wattrset");
        }

        /// <summary>
        /// see <see cref="wattrset"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wattrset_t(IntPtr window, int attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wattrset(window, attrs);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wattrset");
        }
        #endregion

        #region wattr_on
        /// <summary>
        /// see <see cref="NativeStdScr.attr_on"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattr_on(IntPtr window, chtype attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wattr_on(window, attrs, IntPtr.Zero), "wattr_on");
        }

        /// <summary>
        /// see <see cref="wattr_on"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wattr_on_t(IntPtr window, chtype attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wattr_on(window, attrs, IntPtr.Zero);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wattr_on");
        }
        #endregion

        #region wattr_off
        /// <summary>
        /// see <see cref="NativeStdScr.attr_off"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattr_off(IntPtr window, chtype attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wattr_off(window, attrs, IntPtr.Zero), "wattr_off");
        }

        /// <summary>
        /// see <see cref="wattr_off"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wattr_off_t(IntPtr window, chtype attrs)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wattr_off(window, attrs, IntPtr.Zero);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wattr_off");
        }
        #endregion

        #region wattr_set
        /// <summary>
        /// see <see cref="NativeStdScr.attr_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattr_set(IntPtr window, chtype attrs, short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wattr_set(window, attrs, pair, IntPtr.Zero), "wattr_set");
        }

        /// <summary>
        /// see <see cref="wattr_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wattr_set_t(IntPtr window, chtype attrs, short pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wattr_set(window, attrs, pair, IntPtr.Zero);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wattr_set");
        }
        #endregion

        #region wattr_get
        /// <summary>
        /// see <see cref="NativeStdScr.attr_get"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wattr_get(IntPtr window, out chtype attrs, out short pair)
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.wattr_get(window, out attrs, out pair, IntPtr.Zero), "wattr_get");
        }

        /// <summary>
        /// see <see cref="wattr_get"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void attr_get_t(IntPtr window, ref chtype attrs, ref short pair)
        {
            IntPtr aPtr = Marshal.AllocHGlobal(Marshal.SizeOf(attrs));
            Marshal.StructureToPtr(attrs, aPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(attrs));

            IntPtr pPtr = Marshal.AllocHGlobal(Marshal.SizeOf(pair));
            Marshal.StructureToPtr(pair, pPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(pair));

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wattr_get(window, aPtr, pPtr, IntPtr.Zero);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wattr_get");
            }
            finally
            {
                Marshal.FreeHGlobal(aPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(attrs));

                Marshal.FreeHGlobal(pPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(pair));
            }
        }
        #endregion

        #region wbkgd
        /// <summary>
        /// see <see cref="NativeStdScr.bkgd"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wbkgd(IntPtr window, chtype bkgd)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wbkgd(window, bkgd), "wbkgd");
        }

        /// <summary>
        /// see <see cref="wbkgd"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wbkgd_t(IntPtr window, chtype bkgd)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wbkgd(window, bkgd);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wbkgd");
        }
        #endregion

        #region wbkgdset
        /// <summary>
        /// see <see cref="NativeStdScr.bkgdset"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wbkgdset(IntPtr window, chtype bkgd)
        {
            NativeNCurses.NCursesWrapper.wbkgdset(window, bkgd);
        }
        #endregion

        #region wborder
        /// <summary>
        /// see <see cref="NativeStdScr.border"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wborder(IntPtr window, chtype ls, chtype rs, chtype ts, chtype bs, chtype tl, chtype tr, chtype bl, chtype br)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wborder(window, ls, rs, ts, bs, tl, tr, bl, br), "wborder");
        }

        /// <summary>
        /// see <see cref="wborder"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wborder_t(IntPtr window, chtype ls, chtype rs, chtype ts, chtype bs, chtype tl, chtype tr, chtype bl, chtype br)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wborder(window, ls, rs, ts, bs, tl, tr, bl, br);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wborder");
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
        public static void box(IntPtr window, chtype verch, chtype horch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.box(window, verch, horch), "box");
        }

        /// <summary>
        /// see <see cref="box"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void box_t(IntPtr window, chtype verch, chtype horch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.box(window, verch, horch);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "box");
        }
        #endregion

        #region wchgat
        /// <summary>
        /// see <see cref="NativeStdScr.chgat"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wchgat(IntPtr window, int number, chtype attrs, short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wchgat(window, number, attrs, pair, IntPtr.Zero), "wchgat");
        }

        /// <summary>
        /// see <see cref="wchgat"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wchgat_t(IntPtr window, int number, chtype attrs, short pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wchgat(window, number, attrs, pair, IntPtr.Zero);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wchgat");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wclear(window), "wclear");
        }

        /// <summary>
        /// see <see cref="wclear"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wclear_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wclear(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wclear");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.clearok(window, bf), "clearok");
        }

        /// <summary>
        /// see <see cref="clearok"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void clearok_t(IntPtr window, bool bf)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.clearok(window, bf);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "clearok");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wclrtobot(window), "wclrtobot");
        }

        /// <summary>
        /// see <see cref="wclrtobot"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wclrtobot_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wclrtobot(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wclrtobot");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wclrtoeol(window), "wclrtoeol");
        }

        /// <summary>
        /// see <see cref="wclrtoeol"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wclrtoeol_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wclrtoeol(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wclrtoeol");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wcolor_set(window, pair, IntPtr.Zero), "wcolor_set");
        }

        /// <summary>
        /// see <see cref="wcolor_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wcolor_set_t(IntPtr window, short pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wcolor_set(window, pair, IntPtr.Zero);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wcolor_set");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wdelch(window), "wdelch");
        }

        /// <summary>
        /// see <see cref="wdelch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wdelch_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wdelch(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wdelch");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wdeleteln(window), "wdeleteln");
        }

        /// <summary>
        /// see <see cref="wdeleteln"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wdeleteln_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wdeleteln(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wdeleteln");
        }
        #endregion

        #region wechochar
        /// <summary>
        /// see <see cref="NativeStdScr.echochar"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wechochar(IntPtr window, chtype ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wechochar(window, ch), "wechochar");
        }

        /// <summary>
        /// see <see cref="wechochar"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wechochar_t(IntPtr window, chtype ch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wechochar(window, ch);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wechochar");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.werase(window), "werase");
        }

        /// <summary>
        /// see <see cref="werase"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void werase_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.werase(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "werase");
        }
        #endregion

        #region getbkgd
        /// <summary>
        /// The getbkgd function returns the given  window's  current background character/attribute pair.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <returns>Current window background character/attribute pair</returns>
        public static chtype getbkgd(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.getbkgd(window);
        }
        #endregion

        #region wgetch
        private static bool receivedModkey;

        /// <summary>
        /// see <see cref="NativeStdScr.getch"/>
        /// when <see cref="NativeNCurses.UseWindowsOverride"/> is set, the default NCurses behaviour gets overridden with a managed implementation
        /// which allows for control modifiers on function keys.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static int wgetch(IntPtr window)
        {
            //if (NativeNCurses.UseWindowsInputOverride)
            //{
            //    return NativeWindows.NativeWindowsConsoleCharInput(window);
            //}
            //else
                return NativeNCurses.NCursesWrapper.wgetch(window);
        }
        #endregion

        #region wgetnstr
        /// <summary>
        /// see <see cref="NativeStdScr.getnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wgetnstr(IntPtr window, StringBuilder builder, int count)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wgetnstr(window, builder, count), "wgetnstr");
        }
        #endregion

        #region wgetstr
        /// <summary>
        /// see <see cref="NativeStdScr.getstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wgetstr(IntPtr window, StringBuilder builder)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wgetstr(window, builder), "wgetstr");
        }
        #endregion

        #region whline
        /// <summary>
        /// see <see cref="NativeStdScr.hline"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void whline(IntPtr window, chtype ch, int count)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.whline(window, ch, count), "whline");
        }

        /// <summary>
        /// see <see cref="whline"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void whline_t(IntPtr window, chtype ch, int count)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.whline(window, ch, count);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "whline");
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
        public static chtype winch(IntPtr window)
        {
            return NativeNCurses.NCursesWrapper.winch(window);
        }
        #endregion

        #region winchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.inchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winchnstr(IntPtr window, ref chtype[] txt, int count)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winchnstr(window, arrayPtr, count), "winchnstr");
                for (int i = 0; i < txt.Length; i++)
#if NCURSES_VERSION_6
                    txt[i] = (chtype)Marshal.ReadInt32(arrayPtr + (i * sizeof(chtype)));
#elif NCURSES_VERSION_5
                    txt[i] = (chtype)Marshal.ReadInt64(arrayPtr + (i * sizeof(chtype)));
#endif
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="winchnstr"/>
        /// <para />native method wrapped with verification and thread safety.
        ///
        /// </summary>
        public static void winchnstr_t(IntPtr window, ref chtype[] txt, int count)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winchnstr(window, arrayPtr, count);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winchnstr");
                for (int i = 0; i < txt.Length; i++)
#if NCURSES_VERSION_6
                    txt[i] = (chtype)Marshal.ReadInt32(arrayPtr + (i * sizeof(chtype)));
#elif NCURSES_VERSION_5
                    txt[i] = (chtype)Marshal.ReadInt64(arrayPtr + (i * sizeof(chtype)));
#endif
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
#endregion

#region winchstr
        /// <summary>
        /// see <see cref="NativeStdScr.inchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winchstr(IntPtr window, ref chtype[] txt)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winchstr(window, arrayPtr), "winchstr");
                for (int i = 0; i < txt.Length; i++)
#if NCURSES_VERSION_6
                    txt[i] = (chtype)Marshal.ReadInt32(arrayPtr + (i * sizeof(chtype)));
#elif NCURSES_VERSION_5
                    txt[i] = (chtype)Marshal.ReadInt64(arrayPtr + (i * sizeof(chtype)));
#endif
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="winchstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winchstr_t(IntPtr window, chtype[] txt)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winchstr(window, arrayPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
#endregion

#region winnstr
        /// <summary>
        /// see <see cref="NativeStdScr.innstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winnstr(IntPtr window, StringBuilder str, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winnstr(window, str, n), "winnstr");
        }

        /// <summary>
        /// see <see cref="winnstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winnstr_t(IntPtr window, StringBuilder str, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winnstr(window, str, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winnstr");
        }
#endregion

#region winsch
        /// <summary>
        /// see <see cref="NativeStdScr.insch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winsch(IntPtr window, chtype ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winsch(window, ch), "winsch");
        }

        /// <summary>
        /// see <see cref="winsch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winsch_t(IntPtr window, chtype ch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winsch(window, ch);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winsch");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winsdelln(window, n), "winsdelln");
        }

        /// <summary>
        /// see <see cref="winsdelln"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winsdelln_t(IntPtr window, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winsdelln(window, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winsdelln");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winsertln(window), "winsertln");
        }

        /// <summary>
        /// see <see cref="winsertln"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winsertln_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winsertln(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winsertln");
        }
#endregion

#region winsnstr
        /// <summary>
        /// see <see cref="NativeStdScr.insnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winsnstr(IntPtr window, string str, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winsnstr(window, str, n), "winsnstr");
        }

        /// <summary>
        /// see <see cref="winsnstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winsnstr_t(IntPtr window, string str, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winsnstr(window, str, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winsnstr");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winsstr(window, str), "winsstr");
        }

        /// <summary>
        /// see <see cref="winsstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winsstr_t(IntPtr window, string str)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winsstr(window, str);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winsstr");
        }
#endregion

#region winstr
        /// <summary>
        /// see <see cref="NativeStdScr.instr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winstr(IntPtr window, StringBuilder str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winstr(window, str), "winstr");
        }

        /// <summary>
        /// see <see cref="winstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winstr_t(IntPtr window, StringBuilder str)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winstr(window, str);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winstr");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.keypad(window, bf), "keypad");
        }

        /// <summary>
        /// see <see cref="keypad"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void keypad_t(IntPtr window, bool bf)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.keypad(window, bf);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winstr");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.leaveok(window, bf), "leaveok");
        }

        /// <summary>
        /// see <see cref="leaveok"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void leaveok_t(IntPtr window, bool bf)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.leaveok(window, bf);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "leaveok");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wmove(window, y, x), "wmove");
        }

        /// <summary>
        /// see <see cref="wmove"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wmove_t(IntPtr window, int y, int x)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wmove(window, y, x);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wmove");
        }
#endregion

#region mvwaddch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and add character, see <see cref="waddch(IntPtr, chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddch(IntPtr window, int y, int x, chtype ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwaddch(window, y, x, ch), "mvwaddch");
        }

        /// <summary>
        /// see <see cref="mvwaddch(IntPtr, int, int, chtype)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwaddch_t(IntPtr window, int y, int x, chtype ch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwaddch(window, y, x, ch);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwaddch");
        }
#endregion

#region mvwaddchnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="waddchnstr(IntPtr, IntPtr, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddchnstr(IntPtr window, int y, int x, IntPtr chstr, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwaddchnstr(window, y, x, chstr, n), "mvwaddchnstr");
        }

        /// <summary>
        /// see <see cref="mvwaddchnstr(IntPtr, int, int, IntPtr, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwaddchnstr_t(IntPtr window, int y, int x, IntPtr chstr, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwaddchnstr(window, y, x, chstr, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwaddchnstr");
        }
#endregion

#region mvwaddchstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="waddchstr(IntPtr, IntPtr)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddchstr(IntPtr window, int y, int x, IntPtr chstr)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwaddchstr(window, y, x, chstr), "mvwaddchstr");
        }

        /// <summary>
        /// see <see cref="mvwaddchstr(IntPtr, int, int, IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwaddchstr_t(IntPtr window, int y, int x, IntPtr chstr)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwaddchstr(window, y, x, chstr);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwaddchstr");
        }
#endregion

#region mvwaddnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="waddnstr(IntPtr, string, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddnstr(IntPtr window, int y, int x, string txt, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwaddnstr(window, y, x, txt, n), "mvwaddnstr");
        }

        /// <summary>
        /// see <see cref="mvwaddnstr(IntPtr, int, int, string, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwaddnstr_t(IntPtr window, int y, int x, string txt, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwaddnstr(window, y, x, txt, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwaddchstr");
        }
#endregion

#region mvwaddstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="waddstr(IntPtr, string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddstr(IntPtr window, int y, int x, string txt)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwaddstr(window, y, x, txt), "mvwaddstr");
        }

        /// <summary>
        /// see <see cref="mvwaddstr(IntPtr, int, int, string)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwaddstr_t(IntPtr window, int y, int x, string txt)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwaddstr(window, y, x, txt);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwaddstr");
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
        public static void mvwchgat(IntPtr window, int y, int x, int number, chtype attrs, short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwchgat(window, y, x, number, attrs, pair), "mvwchgat");
        }

        /// <summary>
        /// see <see cref="mvwchgat(IntPtr, int, int, int, chtype, short)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwchgat_t(IntPtr window, int y, int x, int number, chtype attrs, short pair)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwchgat(window, y, x, number, attrs, pair);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwaddstr");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwdelch(window, y, x), "mvwdelch");
        }

        /// <summary>
        /// see <see cref="mvwdelch(IntPtr, int, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwdelch_t(IntPtr window, int y, int x)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwdelch(window, y, x);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwdelch");
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
        public static void mvwgetch(IntPtr window, int y, int x)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwgetch(window, y, x), "mvwgetch");
        }

        /// <summary>
        /// see <see cref="mvwgetch(IntPtr, int, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwgetch_t(IntPtr window, int y, int x)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwgetch(window, y, x);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwgetch");
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
        public static void mvwgetnstr(IntPtr window, int y, int x, StringBuilder str, int count)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwgetnstr(window, y, x, str, count), "mvwgetnstr");
        }

        /// <summary>
        /// see <see cref="mvwgetnstr(IntPtr, int, int, StringBuilder, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwgetnstr_t(IntPtr window, int y, int x, StringBuilder str, int count)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwgetnstr(window, y, x, str, count);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwgetnstr");
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
        public static void mvwgetstr(IntPtr window, int y, int x, StringBuilder str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwgetstr(window, y, x, str), "mvwgetstr");
        }

        /// <summary>
        /// see <see cref="mvwgetstr(IntPtr, int, int, StringBuilder)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwgetstr_t(IntPtr window, int y, int x, StringBuilder str)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwgetstr(window, y, x, str);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwgetstr");
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
        public static void mvwhline(IntPtr window, int y, int x, chtype ch, int count)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwhline(window, y, x, ch, count), "mvwhline");
        }

        /// <summary>
        /// see <see cref="mvwhline(IntPtr, int, int, chtype, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwhline_t(IntPtr window, int y, int x, chtype ch, int count)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwhline(window, y, x, ch, count);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwhline");
        }
#endregion

#region mvwinch
        /// <summary>
        /// see <see cref="NativeStdScr.mvinch(int, int)"/>
        /// </summary>
        public static chtype mvwinch(IntPtr window, int y, int x)
        {
            return NativeNCurses.NCursesWrapper.mvwinch(window, y, x);
        }
#endregion

#region mvwinchnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winchnstr(IntPtr, IntPtr, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwinchnstr(IntPtr window, int y, int x, chtype[] txt, int count)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwinchnstr(window, y, x, arrayPtr, count), "mvwinchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="mvwinchnstr(IntPtr, int, int, IntPtr, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwinchnstr_t(IntPtr window, int y, int x, chtype[] txt, int count)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwinchnstr(window, y, x, arrayPtr, count);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwinchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
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
        public static void mvwinchstr(IntPtr window, int y, int x, chtype[] txt)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwinchstr(window, y, x, arrayPtr), "mvwinchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="mvwinchstr(IntPtr, int, int, IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwinchstr_t(IntPtr window, int y, int x, chtype[] txt)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwinchstr(window, y, x, arrayPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwinchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
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
        public static void mvwinnstr(IntPtr window, int y, int x, StringBuilder str, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwinnstr(window, y, x, str, n), "mvwinnstr");
        }

        /// <summary>
        /// see <see cref="mvwinnstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwinnstr_t(IntPtr window, int y, int x, StringBuilder str, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwinnstr(window, y, x, str, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwinnstr");
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
        public static void mvwinsch(IntPtr window, int y, int x, chtype ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwinsch(window, y, x, ch), "mvwinsch");
        }

        /// <summary>
        /// see <see cref="mvwinsch(IntPtr, int, int, chtype)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwinsch_t(IntPtr window, int y, int x, chtype ch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwinsch(window, y, x, ch);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwinsch");
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
        public static void mvwinsnstr(IntPtr window, int y, int x, string str, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwinsnstr(window, y, x, str, n), "mvwinsnstr");
        }

        /// <summary>
        /// see <see cref="mvwinsnstr(IntPtr, int, int, string, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwinsnstr_t(IntPtr window, int y, int x, string str, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwinsnstr(window, y, x, str, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwinsnstr");
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
        public static void mvwinsstr(IntPtr window, int y, int x, string str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwinsstr(window, y, x, str), "mvwinsstr");
        }

        /// <summary>
        /// see <see cref="mvwinsstr(IntPtr, int, int, string)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwinsstr_t(IntPtr window, int y, int x, string str)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwinsstr(window, y, x, str);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwinsstr");
        }
#endregion

#region mvwinstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="winstr(IntPtr, StringBuilder)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwinstr(IntPtr window, int y, int x, StringBuilder str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwinstr(window, y, x, str), "mvwinstr");
        }

        /// <summary>
        /// see <see cref="mvwinstr(IntPtr, int, int, StringBuilder)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwinstr_t(IntPtr window, int y, int x, StringBuilder str)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwinstr(window, y, x, str);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwinstr");
        }
#endregion

#region mvwprintw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wprintw(IntPtr, string, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwprintw(IntPtr window, int y, int x, string format, params object[] var)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwprintw(window, y, x, format, var), "mvwprintw");
        }

        /// <summary>
        /// see <see cref="mvwprintw(IntPtr, int, int, string, object[])"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwprintw_t(IntPtr window, int y, int x, string format, params object[] var)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwprintw(window, y, x, format, var);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwprintw");
        }
#endregion

#region mvwscanw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wscanw(IntPtr, StringBuilder, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwscanw(IntPtr window, int y, int x, StringBuilder format, params object[] var)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwscanw(window, y, x, format, var), "mvwscanw");
        }

        /// <summary>
        /// see <see cref="mvwscanw"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwscanw_t(IntPtr window, int y, int x, StringBuilder format, params object[] var)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwscanw(window, y, x, format, var);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwscanw");
        }
#endregion

#region mvwvline
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="wvline(IntPtr, chtype, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvwvline(IntPtr window, int y, int x, chtype ch, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwvline(window, y, x, ch, n), "mvwvline");
        }

        /// <summary>
        /// see <see cref="mvwvline(IntPtr, int, int, chtype, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwvline_t(IntPtr window, int y, int x, chtype ch, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwvline(window, y, x, ch, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwvline");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.nodelay(window, bf), "nodelay");
        }

        /// <summary>
        /// see <see cref="nodelay(IntPtr, bool)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void nodelay_t(IntPtr window, bool bf)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.nodelay(window, bf);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "nodelay");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.notimeout(window, bf), "notimeout");
        }

        /// <summary>
        /// see <see cref="notimeout(IntPtr, bool)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void notimeout_t(IntPtr window, bool bf)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.notimeout(window, bf);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "notimeout");
        }
#endregion

#region wredrawwin
        /// <summary>
        /// see <see cref="NativeStdScr.redrawwin"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wredrawwin(IntPtr window)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wredrawwin(window), "wredrawwin");
        }

        /// <summary>
        /// see <see cref="wredrawwin(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wredrawwin_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wredrawwin(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wredrawwin");
        }
#endregion

#region wrefresh
        /// <summary>
        /// see <see cref="NativeStdScr.refresh"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wrefresh(IntPtr window)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wrefresh(window), "wrefresh");
        }

        /// <summary>
        /// see <see cref="wrefresh(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wrefresh_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wrefresh(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wrefresh");
        }
#endregion

#region wprintw
        /// <summary>
        /// see <see cref="NativeStdScr.printw(string, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wprintw(IntPtr window, string format, params object[] var)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wprintw(window, format, var), "wprintw");
        }

        /// <summary>
        /// see <see cref="wprintw(IntPtr, string, object[])"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wprintw_t(IntPtr window, string format, params object[] var)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wprintw(window, format, var);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wprintw");
        }
#endregion

#region wscanw
        /// <summary>
        /// see <see cref="NativeStdScr.scanw"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wscanw(IntPtr window, StringBuilder format, params object[] var)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wscanw(window, format, var), "wscanw");
        }

        /// <summary>
        /// see <see cref="wscanw"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wscanw_t(IntPtr window, StringBuilder format, params object[] var)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wscanw(window, format, var);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wscanw");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wscrl(window, n), "wscrl");
        }

        /// <summary>
        /// see <see cref="wscrl(IntPtr, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wscrl_t(IntPtr window, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wscrl(window, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wscrl");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.scroll(window), "scroll");
        }

        /// <summary>
        /// see <see cref="scroll(IntPtr)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void scroll_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.scroll(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "scroll");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.scrollok(window, bf), "scrollok");
        }

        /// <summary>
        /// see <see cref="scrollok(IntPtr, bool)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void scrollok_t(IntPtr window, bool bf)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.scrollok(window, bf);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "scrollok");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wsetscrreg(window, top, bot), "wsetscrreg");
        }

        /// <summary>
        /// see <see cref="wsetscrreg(IntPtr, int, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wsetscrreg_t(IntPtr window, int top, int bot)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wsetscrreg(window, top, bot);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wsetscrreg");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wstandout(window), "wstandout");
        }

        /// <summary>
        /// see <see cref="wstandout"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wstandout_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wstandout(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wstandout");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wstandend(window), "wstandend");
        }

        /// <summary>
        /// see <see cref="wstandend"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wstandend_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wstandend(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wstandend");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.syncok(window, bf), "syncok");
        }

        /// <summary>
        /// see <see cref="syncok"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void syncok_t(IntPtr window, bool bf)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.syncok(window, bf);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "syncok");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.touchline(window, start, count), "touchline");
        }

        /// <summary>
        /// see <see cref="touchline"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void touchline_t(IntPtr window, int start, int count)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.touchline(window, start, count);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "touchline");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.touchwin(window), "touchwin");
        }

        /// <summary>
        /// see <see cref="touchwin"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void touchwin_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.touchwin(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "touchwin");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.untouchwin(window), "untouchwin");
        }

        /// <summary>
        /// see <see cref="untouchwin"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void untouchwin_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.untouchwin(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "untouchwin");
        }
#endregion

#region wvline
        /// <summary>
        /// see <see cref="NativeStdScr.vline(chtype, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wvline(IntPtr window, chtype ch, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wvline(window, ch, n), "wvline");
        }

        /// <summary>
        /// see <see cref="wvline(IntPtr, chtype, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wvline_t(IntPtr window, chtype ch, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wvline(window, ch, n);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wvline");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wnoutrefresh(window), "wnoutrefresh");
        }

        /// <summary>
        /// see <see cref="wnoutrefresh"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wnoutrefresh_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wnoutrefresh(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wnoutrefresh");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wredrawln(window, beg_line, num_lines), "wredrawln");
        }

        /// <summary>
        /// see <see cref="wnoutrefresh"/>
        /// <para />native wredrawln wrapped with verification and thread safety.
        /// </summary>
        public static void wredrawln_t(IntPtr window, int beg_line, int num_lines)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wredrawln(window, beg_line, num_lines);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wredrawln");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wtouchln(window, y, n, changed), "wtouchln");
        }

        /// <summary>
        /// see <see cref="wtouchln"/>
        /// <para />native wredrawln wrapped with verification and thread safety.
        /// </summary>
        public static void wtouchln_t(IntPtr window, int y, int n, int changed)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wtouchln(window, y, n, changed);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wtouchln");
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
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wresize(window, lines, columns), "wresize");
        }

        /// <summary>
        /// see <see cref="wresize"/>
        /// <para />native wredrawln wrapped with verification and thread safety.
        /// </summary>
        public static void wresize_t(IntPtr window, int lines, int columns)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wresize(window, lines, columns);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wresize");
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
            return NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wgetparent(window), "wgetparent");
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
        public static int wgetscrreg(IntPtr window, ref int top, ref int bottom)
        {
            IntPtr tPtr = Marshal.AllocHGlobal(Marshal.SizeOf(top));
            Marshal.StructureToPtr(top, tPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(top));

            IntPtr bPtr = Marshal.AllocHGlobal(Marshal.SizeOf(bottom));
            Marshal.StructureToPtr(bottom, bPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(bottom));

            try
            {
                return NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wgetscrreg(window, tPtr, bPtr), "wgetscrreg");
            }
            finally
            {
                Marshal.FreeHGlobal(tPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(top));

                Marshal.FreeHGlobal(bPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(bottom));
            }
        }
#endregion

#region wadd_wch
        /// <summary>
        /// see <see cref="NativeStdScr.add_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wadd_wch(IntPtr window, NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wadd_wch(window, wPtr), "wadd_wch");
            }
        }

        /// <summary>
        /// see <see cref="wadd_wch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wadd_wch_t(IntPtr window, NCursesWCHAR wch)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wadd_wch(window, wPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wadd_wch");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region wadd_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.add_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wadd_wchnstr(IntPtr window, NCursesWCHAR[] wchStr, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wadd_wchnstr(window, arrayPtr, n), "wadd_wchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="wadd_wchnstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wadd_wchnstr_t(IntPtr window, NCursesWCHAR[] wchStr, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wadd_wchnstr(window, arrayPtr, n);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wadd_wchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
#endregion

#region wadd_wchstr
        /// <summary>
        /// see <see cref="NativeStdScr.add_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wadd_wchstr(IntPtr window, NCursesWCHAR[] wchStr)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wadd_wchstr(window, arrayPtr), "wadd_wchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="wadd_wchstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wadd_wchstr_t(IntPtr window, NCursesWCHAR[] wchStr)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wadd_wchstr(window, arrayPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wadd_wchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
#endregion

#region waddnwstr
        /// <summary>
        /// see <see cref="NativeStdScr.addnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddnwstr(IntPtr window, string wstr, int n)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.waddnwstr(window, ptr, n), "waddnwstr"),
                wstr);
        }

        /// <summary>
        /// see <see cref="waddnwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void waddnwstr_t(IntPtr window, string wstr, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) =>
            {
                int ret = 0;
                NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) => ret = NativeNCurses.NCursesWrapper.waddnwstr(window, ptr, n), wstr);
                return ret;
            };
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "waddnwstr");
        }
#endregion

#region waddwstr
        /// <summary>
        /// see <see cref="NativeStdScr.addwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void waddwstr(IntPtr window, string wstr)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.waddwstr(window, ptr), "waddwstr"),
                wstr, true);
        }

        /// <summary>
        /// see <see cref="waddwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void waddwstr_t(IntPtr window, string wstr)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) =>
            {
                int ret = 0;
                NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) => ret = NativeNCurses.NCursesWrapper.waddwstr(window, ptr), wstr);
                return ret;
            };
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "waddwstr");
        }
#endregion

#region wbkgrnd
        /// <summary>
        /// see <see cref="NativeStdScr.bkgrnd"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wbkgrnd(IntPtr window, NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.bkgrnd(window, wPtr), "wbkgrnd");
            }
        }

        /// <summary>
        /// see <see cref="wbkgrnd"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wbkgrnd_t(IntPtr window, NCursesWCHAR wch)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.bkgrnd(window, wPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wbkgrnd");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region wbkgrndset
        /// <summary>
        /// see <see cref="NativeStdScr.bkgrndset"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wbkgrndset(IntPtr window, NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.NCursesWrapper.wbkgrndset(window, wPtr);
            }
        }
#endregion

#region wborder_set
        /// <summary>
        /// see <see cref="NativeStdScr.border_set"/>
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wborder_set(IntPtr window, NCursesWCHAR ls, NCursesWCHAR rs, NCursesWCHAR ts, NCursesWCHAR bs, NCursesWCHAR tl, NCursesWCHAR tr,
            NCursesWCHAR bl, NCursesWCHAR br)
        {
            IntPtr lsPtr = ls.ToPointer();
            IntPtr rsPtr = rs.ToPointer();
            IntPtr tsPtr = ts.ToPointer();
            IntPtr bsPtr = bs.ToPointer();
            IntPtr tlPtr = tl.ToPointer();
            IntPtr trPtr = tr.ToPointer();
            IntPtr blPtr = bl.ToPointer();
            IntPtr brPtr = br.ToPointer();

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wborder_set(window, lsPtr, rsPtr, tsPtr, bsPtr, tlPtr, trPtr, blPtr, brPtr), "wborder_set");
            }
            finally
            {
                Marshal.FreeHGlobal(lsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(ls));

                Marshal.FreeHGlobal(rsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(rs));

                Marshal.FreeHGlobal(tsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(ts));

                Marshal.FreeHGlobal(bsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(bs));

                Marshal.FreeHGlobal(tlPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(tl));

                Marshal.FreeHGlobal(trPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(tr));

                Marshal.FreeHGlobal(blPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(bl));

                Marshal.FreeHGlobal(lsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(br));
            }
        }

        /// <summary>
        /// see <see cref="wborder_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wborder_set_t(IntPtr window, NCursesWCHAR ls, NCursesWCHAR rs, NCursesWCHAR ts, NCursesWCHAR bs, NCursesWCHAR tl, NCursesWCHAR tr,
            NCursesWCHAR bl, NCursesWCHAR br)
        {
            IntPtr lsPtr = ls.ToPointer();
            IntPtr rsPtr = rs.ToPointer();
            IntPtr tsPtr = ts.ToPointer();
            IntPtr bsPtr = bs.ToPointer();
            IntPtr tlPtr = tl.ToPointer();
            IntPtr trPtr = tr.ToPointer();
            IntPtr blPtr = bl.ToPointer();
            IntPtr brPtr = br.ToPointer();

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wborder_set(window, lsPtr, rsPtr, tsPtr, bsPtr, tlPtr, trPtr, blPtr, brPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wborder_set");
            }
            finally
            {
                Marshal.FreeHGlobal(lsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(ls));

                Marshal.FreeHGlobal(rsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(rs));

                Marshal.FreeHGlobal(tsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(ts));

                Marshal.FreeHGlobal(bsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(bs));

                Marshal.FreeHGlobal(tlPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(tl));

                Marshal.FreeHGlobal(trPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(tr));

                Marshal.FreeHGlobal(blPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(bl));

                Marshal.FreeHGlobal(lsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(br));
            }
        }
#endregion

#region box_set
        /// <summary>
        /// box_set(win, verch, horch); is a shorthand for the follow ing call:
        /// wborder_set(win, verch, verch, horch, horch, NULL, NULL, NULL, NULL);
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void box_set(IntPtr window, NCursesWCHAR verch, NCursesWCHAR horch)
        {
            IntPtr vPtr = verch.ToPointer();
            GC.AddMemoryPressure(verch.Size);

            IntPtr hPtr = horch.ToPointer();
            GC.AddMemoryPressure(horch.Size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.box_set(window, vPtr, hPtr), "box_set");
            }
            finally
            {
                Marshal.FreeHGlobal(vPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(verch));

                Marshal.FreeHGlobal(hPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(horch));
            }
        }

        /// <summary>
        /// see <see cref="box_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void box_set_t(IntPtr window, NCursesWCHAR verch, NCursesWCHAR horch)
        {
            IntPtr vPtr = verch.ToPointer();
            GC.AddMemoryPressure(verch.Size);

            IntPtr hPtr = horch.ToPointer();
            GC.AddMemoryPressure(horch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.box_set(window, vPtr, hPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "box_set");
            }
            finally
            {
                Marshal.FreeHGlobal(vPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(verch));

                Marshal.FreeHGlobal(hPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(horch));
            }
        }
#endregion

#region wecho_wchar
        /// <summary>
        /// seee <see cref="NativeStdScr.echo_wchar"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wecho_wchar(IntPtr window, NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wecho_wchar(window, wPtr), "wecho_wchar");
            }
        }

        /// <summary>
        /// see <see cref="wecho_wchar"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wecho_wchar_t(IntPtr window, NCursesWCHAR wch)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wecho_wchar(window, wPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wecho_wchar");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region wget_wch
        /// <summary>
        /// see <see cref="NativeStdScr.get_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wget_wch(IntPtr window, out char wch)
        {
            //TODO: returns KEY_CODE_YES if a function key gets pressed
            IntPtr chPtr = Marshal.AllocHGlobal(Marshal.SizeOf<chtype>());
            GC.AddMemoryPressure(Marshal.SizeOf<chtype>());

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wget_wch(window, chPtr), "get_wch");

                byte[] arr = new byte[Marshal.SizeOf<chtype>()];
                Marshal.Copy(chPtr, arr, 0, Marshal.SizeOf<chtype>());

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    wch = Encoding.Unicode.GetChars(arr)[0];
                else
                    wch = Encoding.UTF32.GetChars(arr)[0];
            }
            finally
            {
                Marshal.FreeHGlobal(chPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<chtype>());
            }
        }
#endregion

#region wget_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.get_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wget_wstr(IntPtr window, StringBuilder wstr)
        {
            int size = Marshal.SizeOf<chtype>() * wstr.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wget_wstr(window, strPtr), "wget_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, wstr.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<chtype>());
            }
        }

        /// <summary>
        /// see <see cref="wget_wstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wget_wstr_t(IntPtr window, StringBuilder wstr)
        {
            int size = Marshal.SizeOf<chtype>() * wstr.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wget_wstr(window, strPtr);
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wget_wstr");
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
#endregion

#region wgetbkgrnd
        /// <summary>
        /// see <see cref="NativeStdScr.getbkgrnd"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wgetbkgrnd(IntPtr window, out NCursesWCHAR wch)
        {
            IntPtr wPtr;
            wch = new NCursesWCHAR();
            using (wch.ToPointer(out wPtr))
            {
                NCursesException.Verify(NativeNCurses.NCursesWrapper.wgetbkgrnd(window, wPtr), "wgetbkgrnd");
            }
        }

        /// <summary>
        /// see <see cref="wgetbkgrnd"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wgetbkgrnd_t(IntPtr window, out NCursesWCHAR wch)
        {
            IntPtr wPtr = (wch = new NCursesWCHAR()).ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wgetbkgrnd(window, a);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), wPtr, "wget_wch");
                wch.ToStructure(wPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region wgetn_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.getn_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wgetn_wstr(IntPtr window, StringBuilder wstr, int n)
        {
            int size = Marshal.SizeOf<chtype>() * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wgetn_wstr(window, strPtr, n), "wgetn_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }

        /// <summary>
        /// see <see cref="wgetn_wstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wgetn_wstr_t(IntPtr window, StringBuilder wstr, int n)
        {
            int size = Marshal.SizeOf<chtype>() * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wgetn_wstr(window, strPtr, n);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wgetn_wstr");
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
#endregion

#region whline_set
        /// <summary>
        /// see <see cref="NativeStdScr.hline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void whline_set(IntPtr window, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.whline_set(window, wPtr, n), "whline_set");
            }
        }

        /// <summary>
        /// see <see cref="whline_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void whline_set_t(IntPtr window, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.whline_set(window, wPtr, n);
            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "whline_set");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region win_wch
        /// <summary>
        /// see <see cref="NativeStdScr.in_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void win_wch(IntPtr window, out NCursesWCHAR wcval)
        {
            IntPtr wPtr;
            wcval = new NCursesWCHAR();
            using (wcval.ToPointer(out wPtr))
            {
                NCursesException.Verify(NativeNCurses.NCursesWrapper.win_wch(window, wPtr), "win_wch");
            }
        }

        /// <summary>
        /// see <see cref="win_wch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void win_wch_t(IntPtr window, out NCursesWCHAR wcval)
        {
            wcval = new NCursesWCHAR();
            IntPtr wPtr = wcval.ToPointer();
            GC.AddMemoryPressure(wcval.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.win_wch(window, a);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), wPtr, "win_wch");
                wcval.ToStructure(wPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wcval.Size);
            }
        }
#endregion

#region win_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.in_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void win_wchnstr(IntPtr window, ref NCursesWCHAR[] wcval, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wcval, false, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.win_wchnstr(window, arrayPtr, totalSize), "win_wchnstr");
                for (int i = 0; i < wcval.Length; i++)
                    wcval[i].ToStructure(arrayPtr + (i * wcval[i].Size));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="win_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void win_wchnstr_t(IntPtr window, ref NCursesWCHAR[] wcval, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wcval, false, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.win_wchnstr(window, arrayPtr, totalSize);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "win_wchnstr");
                for (int i = 0; i < wcval.Length; i++)
                    wcval[i].ToStructure(arrayPtr + (i * wcval[i].Size));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
#endregion

#region win_wchstr
        /// <summary>
        /// see <see cref="NativeStdScr.in_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void win_wchstr(IntPtr window, ref NCursesWCHAR[] wcval)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wcval, false, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.win_wchstr(window, arrayPtr), "win_wchstr");
                for (int i = 0; i < wcval.Length; i++)
                    wcval[i].ToStructure(arrayPtr + (i * wcval[i].Size));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="win_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void win_wchstr_t(IntPtr window, ref NCursesWCHAR[] wcval)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wcval, false, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.win_wchstr(window, arrayPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "win_wchstr");
                for (int i = 0; i < wcval.Length; i++)
                    wcval[i].ToStructure(arrayPtr + (i * wcval[i].Size));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
#endregion

#region winnwstr
        /// <summary>
        /// see <see cref="NativeStdScr.innwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winnwstr(IntPtr window, StringBuilder str, int n)
        {
            int size = Constants.SIZEOF_WCHAR_T * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winnwstr(window, strPtr, n), "winnwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }

        /// <summary>
        /// see <see cref="winnwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winnwstr_t(IntPtr window, StringBuilder str, int n)
        {
            int size = Constants.SIZEOF_WCHAR_T * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winnwstr(window, strPtr, n);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winnwstr");
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
#endregion

#region wins_nwstr
        /// <summary>
        /// see <see cref="NativeStdScr.ins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wins_nwstr(IntPtr window, string str, int n)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((strPtr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wins_nwstr(window, strPtr, n), "wins_nwstr"),
                str);
        }

        /// <summary>
        /// see <see cref="wins_nwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wins_nwstr_t(IntPtr window, string str, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) =>
            {
                int ret = 0;
                NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) => ret = NativeNCurses.NCursesWrapper.wins_nwstr(window, ptr, n), str);
                return ret;
            };
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wins_nwstr");
        }
#endregion

#region wins_wch
        
        /// <summary>
        /// see <see cref="NativeStdScr.ins_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wins_wch(IntPtr window, NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wins_wch(window, wPtr), "wins_wch");
            }
        }

        /// <summary>
        /// see <see cref="wins_wch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wins_wch_t(IntPtr window, NCursesWCHAR wch)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wins_wch(window, wPtr);
            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wins_wch");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region wins_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.ins_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wins_wstr(IntPtr window, string str)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((strPtr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wins_wstr(window, strPtr), "wins_wstr"),
                str, true);
        }

        /// <summary>
        /// see <see cref="wins_wstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wins_wstr_t(IntPtr window, string str)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) =>
            {
                int ret = 0;
                NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) => ret = NativeNCurses.NCursesWrapper.wins_wstr(window, ptr), str);
                return ret;
            };
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wins_wstr");
        }
#endregion

#region winwstr
        /// <summary>
        /// see <see cref="NativeStdScr.inwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void winwstr(IntPtr window, StringBuilder str)
        {
            int size = Constants.SIZEOF_WCHAR_T * str.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.winwstr(window, strPtr), "winwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, str.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }

        /// <summary>
        /// see <see cref="winwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void winwstr_t(IntPtr window, StringBuilder str)
        {
            int size = Constants.SIZEOF_WCHAR_T * str.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.winwstr(window, strPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "winwstr");
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
#endregion

#region mvwadd_wch
        /// <summary>
        /// see <see cref="NativeStdScr.mvadd_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwadd_wch(IntPtr window, int y, int x, NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwadd_wch(window, y, x, wPtr), "mvwadd_wch");
            }
        }

        /// <summary>
        /// see <see cref="mvwadd_wch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwadd_wch_t(IntPtr window, int y, int x, NCursesWCHAR wch)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwadd_wch(window, y, x, wPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwadd_wch");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region mvwadd_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvadd_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwadd_wchnstr(IntPtr window, int y, int x, NCursesWCHAR[] wchStr, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwadd_wchnstr(window, y, x, arrayPtr, n), "mvwadd_wchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="mvwadd_wchnstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwadd_wchnstr_t(IntPtr window, int y, int x, NCursesWCHAR[] wchStr, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwadd_wchnstr(window, y, x, arrayPtr, n);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwadd_wchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
#endregion

#region mvwadd_wchstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvadd_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwadd_wchstr(IntPtr window, int y, int x, NCursesWCHAR[] wchStr)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwadd_wchstr(window, y, x, arrayPtr), "mvwadd_wchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="mvwadd_wchstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwadd_wchstr_t(IntPtr window, int y, int x, NCursesWCHAR[] wchStr)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwadd_wchstr(window, y, x, arrayPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwadd_wchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
#endregion

#region mvwaddnwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvaddnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddnwstr(IntPtr window, int y, int x, string wstr, int n)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwaddnwstr(window, y, x, ptr, n), "mvwaddnwstr"),
                wstr);
        }

        /// <summary>
        /// see <see cref="mvwaddnwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwaddnwstr_t(IntPtr window, int y, int x, string wstr, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) =>
            {
                int ret = 0;
                NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) => ret = NativeNCurses.NCursesWrapper.mvwaddnwstr(window, y, x, ptr, n), wstr);
                return ret;
            };
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwaddnwstr");
        }
#endregion

#region mvwaddwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvaddwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwaddwstr(IntPtr window, int y, int x, string wstr)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwaddwstr(window, y, x, ptr), "mvwaddwstr"),
                wstr, true);
        }

        /// <summary>
        /// see <see cref="waddwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwaddwstr_t(IntPtr window, int y, int x, string wstr)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) =>
            {
                int ret = 0;
                NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) => ret = NativeNCurses.NCursesWrapper.mvwaddwstr(window, y, x, ptr), wstr);
                return ret;
            };
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwaddwstr");
        }
#endregion

#region mvwget_wch
        /// <summary>
        /// see <see cref="NativeStdScr.mvget_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwget_wch(IntPtr window, int y, int x, out char wch)
        {
            IntPtr chPtr = Marshal.AllocHGlobal(Marshal.SizeOf<chtype>());
            GC.AddMemoryPressure(Marshal.SizeOf<chtype>());

            try
            {
                NCursesException.Verify(NativeNCurses.NCursesWrapper.mvwget_wch(window, y, x, chPtr), "mvwget_wch");

                byte[] arr = new byte[Marshal.SizeOf<chtype>()];
                Marshal.Copy(chPtr, arr, 0, Marshal.SizeOf<chtype>());

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    wch = Encoding.Unicode.GetChars(arr)[0];
                else
                    wch = Encoding.UTF32.GetChars(arr)[0];
            }
            finally
            {
                Marshal.FreeHGlobal(chPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<chtype>());
            }
        }

        /// <summary>
        /// see <see cref="mvwget_wch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwget_wch_t(IntPtr window, int y, int x, out char wch)
        {
            IntPtr chPtr = Marshal.AllocHGlobal(Marshal.SizeOf<chtype>());
            GC.AddMemoryPressure(Marshal.SizeOf<chtype>());

            try
            {
                Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwget_wch(window, y, x, chPtr);
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwget_wch");

                byte[] arr = new byte[Marshal.SizeOf<chtype>()];
                Marshal.Copy(chPtr, arr, 0, Marshal.SizeOf<chtype>());

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    wch = Encoding.Unicode.GetChars(arr)[0];
                else
                    wch = Encoding.UTF32.GetChars(arr)[0];
            }
            finally
            {
                Marshal.FreeHGlobal(chPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<chtype>());
            }
        }
#endregion

#region mvwget_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvget_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwget_wstr(IntPtr window, int y, int x, StringBuilder wstr)
        {
            int size = Marshal.SizeOf<chtype>() * wstr.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwget_wstr(window, y, x, strPtr), "mvwget_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, wstr.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<chtype>());
            }
        }

        /// <summary>
        /// see <see cref="mvwget_wstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwget_wstr_t(IntPtr window, int y, int x, StringBuilder wstr)
        {
            int size = Marshal.SizeOf<chtype>() * wstr.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwget_wstr(window, y, x, strPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwget_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, wstr.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<chtype>());
            }
        }
#endregion

#region mvwgetn_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvgetn_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwgetn_wstr(IntPtr window, int y, int x, StringBuilder wstr, int n)
        {
            int size = Marshal.SizeOf<chtype>() * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwgetn_wstr(window, y, x, strPtr, n), "mvwgetn_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }

        /// <summary>
        /// see <see cref="mvwget_wstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwgetn_wstr_t(IntPtr window, int y, int x, StringBuilder wstr, int n)
        {
            int size = Marshal.SizeOf<chtype>() * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwgetn_wstr(window, y, x, strPtr, n);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwgetn_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
#endregion

#region mvwhline_set
        /// <summary>
        /// see <see cref="NativeStdScr.mvhline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwhline_set(IntPtr window, int y, int x, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwhline_set(window, y, x, wPtr, n), "mvwhline_set");
            }
        }

        /// <summary>
        /// see <see cref="mvwhline_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwhline_set_t(IntPtr window, int y, int x, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwhline_set(window, y, x, wPtr, n);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwhline_set");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region mvwin_wch
        /// <summary>
        /// see <see cref="NativeStdScr.mvin_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwin_wch(IntPtr window, int y, int x, ref NCursesWCHAR wcval)
        {
            IntPtr wPtr;
            using (wcval.ToPointer(out wPtr))
            {
                NCursesException.Verify(NativeNCurses.NCursesWrapper.mvwin_wch(window, y, x, wPtr), "mvwin_wch");
            }
        }

        /// <summary>
        /// see <see cref="in_wch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwin_wch_t(IntPtr window, int y, int x, ref NCursesWCHAR wcval)
        {
            IntPtr wPtr = wcval.ToPointer();
            GC.AddMemoryPressure(wcval.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwin_wch(window, y, x, wPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwin_wch");
                wcval.ToStructure(wPtr);
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wcval.Size);
            }
        }
#endregion

#region mvwin_wchnstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvin_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">array reference to store the complex characters in</param>
        public static void mvwin_wchnstr(IntPtr window, int y, int x, ref NCursesWCHAR[] wcval, int n)
        {
            if (n != wcval.Length)
                throw new ArgumentException("lenght of the array and n should be the same");

            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wcval, false, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwin_wchnstr(window, y, x, arrayPtr, totalSize), "mvwin_wchnstr");
                for (int i = 0; i < wcval.Length; i++)
                    wcval[i].ToStructure(arrayPtr + (i * wcval[i].Size));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }

        /// <summary>
        /// see <see cref="mvwin_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void mvwin_wchnstr_t(IntPtr window, int y, int x, ref NCursesWCHAR[] wcval, int n)
        {
            if (n != wcval.Length)
                throw new ArgumentException("lenght of the array and n should be the same");

            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wcval, false, out totalSize);
            GC.AddMemoryPressure(totalSize);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwin_wchnstr(window, y, x, arrayPtr, totalSize);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwin_wchnstr");
                for (int i = 0; i < wcval.Length; i++)
                    wcval[i].ToStructure(arrayPtr + (i * wcval[i].Size));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
#endregion

#region mvwinnwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvinnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwinnwstr(IntPtr window, int y, int x, StringBuilder str, int n)
        {
            int size = Constants.SIZEOF_WCHAR_T * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwinnwstr(window, y, x, strPtr, n), "mvwinnwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }

        /// <summary>
        /// see <see cref="mvwinnwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwinnwstr_t(IntPtr window, int y, int x, StringBuilder str, int n)
        {
            int size = Constants.SIZEOF_WCHAR_T * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwinnwstr(window, y, x, strPtr, n);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwinnwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
#endregion

#region mvwins_nwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwins_nwstr(IntPtr window, int y, int x, string str, int n)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((strPtr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwins_nwstr(window, y, x, strPtr, n), "mvwins_nwstr"),
                str);
        }

        /// <summary>
        /// see <see cref="mvwins_nwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwins_nwstr_t(IntPtr window, int y, int x, string str, int n)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) =>
            {
                int ret = 0;
                NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) => ret = NativeNCurses.NCursesWrapper.mvwins_nwstr(window, y, x, ptr, n), str);
                return ret;
            };
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwins_nwstr");
        }
#endregion

#region mvwins_wch
        /// <summary>
        /// see <see cref="NativeStdScr.mvins_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwins_wch(IntPtr window, int y, int x, NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwins_wch(window, y, x, wPtr), "mvwins_wch");
            }
        }

        /// <summary>
        /// see <see cref="mvwins_wch"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwins_wch_t(IntPtr window, int y, int x, NCursesWCHAR wch)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwins_wch(window, y, x, wPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwins_wch");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region mvwins_wstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvins_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwins_wstr(IntPtr window, int y, int x, string str)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((strPtr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwins_wstr(window, y, x, strPtr), "mvwins_wstr"),
                str, true);
        }

        /// <summary>
        /// see <see cref="mvins_wstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwins_wstr_t(IntPtr window, int y, int x, string str)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) =>
            {
                int ret = 0;
                NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) => ret = NativeNCurses.NCursesWrapper.mvwins_wstr(window, y, x, ptr), str);
                return ret;
            };
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwins_wstr");
        }
#endregion

#region mvwinwstr
        /// <summary>
        /// see <see cref="NativeStdScr.mvinwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwinwstr(IntPtr window, int y, int x, StringBuilder str)
        {
            int size = Constants.SIZEOF_WCHAR_T * str.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwinwstr(window, y, x, strPtr), "mvwinwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, str.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }

        /// <summary>
        /// see <see cref="mvwinwstr"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwinwstr_t(IntPtr window, int y, int x, StringBuilder str)
        {
            int size = Constants.SIZEOF_WCHAR_T * str.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwinwstr(window, y, x, strPtr);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwinwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, str.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
#endregion

#region mvwvline_set
        /// <summary>
        /// see <see cref="NativeStdScr.mvvline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void mvwvline_set(IntPtr window, int y, int x, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvwvline_set(window, y, x, wPtr, n), "mvwvline_set");
            }
        }

        /// <summary>
        /// see <see cref="mvwvline_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void mvwvline_set_t(IntPtr window, int y, int x, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.mvwvline_set(window, y, x, wPtr, n);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "mvwvline_set");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
        }
#endregion

#region wvline_set
        /// <summary>
        /// see <see cref="NativeStdScr.vline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void wvline_set(IntPtr window, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.wvline_set(window, wPtr, n), "wvline_set");
            }
        }

        /// <summary>
        /// see <see cref="wvline_set"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void wvline_set_t(IntPtr window, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr = wch.ToPointer();
            GC.AddMemoryPressure(wch.Size);

            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => NativeNCurses.NCursesWrapper.wvline_set(window, wPtr, n);

            try
            {
                NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "wvline_set");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(wch.Size);
            }
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
