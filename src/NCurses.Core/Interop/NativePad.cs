using System;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop
{
    internal static class NativePad
    {
        #region pechochar
        /// <summary>
        /// see <see cref="pechochar(IntPtr, uint)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "pechochar")]
        internal extern static int ncurses_pechochar(IntPtr pad, uint ch);

        /// <summary>
        /// The pechochar routine is functionally equivalent to a call
        /// to addch followed by a call to refresh(3x), a call to waddch followed by a call to wrefresh, or a  call to  waddch
        /// followed by a call to prefresh.The knowledge that only a
        /// single character is being output is taken into  consideration and,
        /// for non-control characters, a considerable performance gain might be seen by
        /// using  these  routines  instead of their equivalents.In the case of pechochar, the
        /// last location of the pad on the screen is reused  for  the
        /// arguments to prefresh.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="pad">a pointer to the pad</param>
        /// <param name="ch">the character you want to echo</param>
        public static void pechochar(IntPtr pad, uint ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_pechochar(pad, ch), "pechochar");
        }

        /// <summary>
        /// see <see cref="pechochar(IntPtr, uint)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void pechochar_t(IntPtr pad, uint ch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_pechochar(pad, ch);
            NativeNCurses.use_window_v(pad, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "pechochar");
        }
        #endregion

        #region pnoutrefresh
        /// <summary>
        /// see <see cref="pnoutrefresh"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "pnoutrefresh")]
        internal extern static int ncurses_pnoutrefresh(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol);

        /// <summary>
        /// The prefresh  and pnoutrefresh routines are analogous to
        /// wrefresh and wnoutrefresh except that they relate to  pads
        /// instead  of windows.The additional parameters are needed
        /// to indicate what part of the pad and screen are  involved.
        /// The pminrow and pmincol parameters specify the upper left-
        /// hand corner of the rectangle to be displayed in  the pad.
        /// The sminrow, smincol, smaxrow, and smaxcol parameters
        /// specify the edges of the rectangle to be displayed on the
        /// screen.The lower right-hand corner of the rectangle to
        /// be displayed in the pad is calculated from the screen  coordinates, since the  rectangles must be the same size.
        /// Both rectangles must be entirely contained  within their
        /// respective structures.  Negative values of pminrow, pmin-
        /// col, sminrow, or smincol are treated as if they were zero.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="pad">a pointer to the pad</param>
        /// <param name="pminrow">line nubmer of the upper-left corner of what to display from the pad</param>
        /// <param name="pmincol">column nubmer of the upper-left corner of what to display from the pad</param>
        /// <param name="sminrow">minimum line number of the screen where to display</param>
        /// <param name="smincol">minimum column number of the screen where to display</param>
        /// <param name="smaxrow">maximum line number of the screen where to display</param>
        /// <param name="smaxcol">minimum column number of the screen where to display</param>
        public static void pnoutrefresh(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_pnoutrefresh(pad, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol), "pnoutrefresh");
        }

        /// <summary>
        /// see <see cref="pnoutrefresh"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void pnoutrefresh_t(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_pnoutrefresh(pad, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol);
            NativeNCurses.use_window_v(pad, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "pnoutrefresh");
        }
        #endregion

        #region prefresh
        /// <summary>
        /// see <see cref="prefresh(IntPtr, int, int, int, int, int, int)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "prefresh")]
        internal extern static int ncurses_prefresh(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol);

        /// <summary>
        /// see <see cref="pnoutrefresh(IntPtr, int, int, int, int, int, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void prefresh(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_prefresh(pad, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol), "prefresh");
        }

        /// <summary>
        /// see <see cref="prefresh(IntPtr, int, int, int, int, int, int)"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void prefresh_t(IntPtr pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_prefresh(pad, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol);
            NativeNCurses.use_window_v(pad, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "prefresh");
        }
        #endregion

        #region pecho_wchar
        /// <summary>
        /// see <see cref="pecho_wchar"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "pecho_wchar")]
        internal extern static int ncurses_pecho_wchar(IntPtr pad, NCURSES_CH_T wch);

        /// <summary>
        /// The pechochar routine is functionally equivalent to a call
        /// to addch followed by a call to refresh(3x), a call to waddch followed by a call to wrefresh, or a  call to  waddch
        /// followed by a call to prefresh.The knowledge that only a
        /// single character is being output is taken into  consideration and, for non-control characters,
        /// a considerable performance gain might be seen by using  these
        /// routines  instead of their equivalents.In the case of pechochar, the
        /// last location of the pad on the screen is reused  for  the
        /// arguments to prefresh.
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void pecho_wchar(IntPtr pad, NCURSES_CH_T wch)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_pecho_wchar(pad, wch), "pecho_wchar");
        }

        /// <summary>
        /// see <see cref="pecho_wchar"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void pecho_wchar_t(IntPtr pad, NCURSES_CH_T wch)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_pecho_wchar(pad, wch);
            NativeNCurses.use_window_v(pad, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "pecho_wchar");
        }
        #endregion
    }
}
