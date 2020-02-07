using System;
using NCurses.Core.Interop;

namespace NCurses.Core
{
    public abstract class Pad : Window
    {
        protected Window Window { get; set; }

        internal Pad(IntPtr windowPtr, bool ownsHandle = true, bool initizalize = true)
            : base(windowPtr, ownsHandle, initizalize)
        { }

        /// <summary>
        /// create a subwindow with the current window as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        public override Window SubWindow(int nlines, int ncols, int begin_y, int begin_x)
        {
            return CreatePad(NativeNCurses.subwin(this.WindowPtr, nlines, ncols, begin_y, begin_x));
        }

        /// <summary>
        /// create a subwindow with the current window as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        public override Window DerWindow(int nlines, int ncols, int begin_y, int begin_x)
        {
            return CreatePad(NativeNCurses.derwin(this.WindowPtr, nlines, ncols, begin_y, begin_x));
        }

        /// <summary>
        /// create a subwindow with the current pad as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        public Pad SubPad(int nlines, int ncols, int begin_y, int begin_x)
        {
            return CreatePad(NativeNCurses.subpad(this.WindowPtr, nlines, ncols, begin_y, begin_x));
        }

        internal static Pad CreatePad(IntPtr windowPtr, bool initialize = true)
        {
            if (NCurses.UnicodeSupported)
                return CreateMultiBytePad(windowPtr, initialize);
            return CreateSingleBytePad(windowPtr, initialize);
        }

        internal static Pad CreateSingleBytePad(IntPtr windowPtr, bool initialize = true)
        {
            return new SingleBytePad(windowPtr, true, initialize);
        }

        public static Pad CreateSingleBytePad(int nlines, int ncols)
        {
            return new SingleBytePad(nlines, ncols);
        }

        internal static Pad CreateMultiBytePad(IntPtr windowPtr, bool initialize = true)
        {
            return new MultiBytePad(windowPtr, true, initialize);
        }

        public static Pad CreateMultiBytePad(int nlines, int ncols)
        {
            return new MultiBytePad(nlines, ncols);
        }

        #region NoOutRefresh
        /// <summary>
        /// efficient non-instant pad update. follow up with <see cref="NCurses.Update"/> to render to the console.
        /// the area to render gets computed from the screen rectangle
        /// </summary>
        /// <param name="pminrow">the line of the top left corner to refresh</param>
        /// <param name="pmincol">the column of the top left corner to refresh</param>
        /// <param name="sminrow">the line of the top left corner of the screen to start the refresh (0)</param>
        /// <param name="smincol">the column of the top left corner of the screen to start the refresh (0)</param>
        /// <param name="smaxrow">the line of the bottom right corner of the screen to end the refresh (MaxLine - 1)</param>
        /// <param name="smaxcol">the column of the bottom right corner of the screen to end the refresh (MaxColumn - 1)</param>
        public void NoOutRefresh(int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            NativePad.pnoutrefresh(this.WindowPtr, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol);
        }

        /// <summary>
        /// not thread-safe
        /// see <see cref="NoOutRefresh(int, int, int, int, int, int)"/>
        /// </summary>
        public override void NoOutRefresh()
        {
            this.NoOutRefresh(0, 0, 0, 0, NCurses.StdScr.MaxLine - 1, NCurses.StdScr.MaxColumn - 1);
        }
        #endregion

        #region Refresh
        /// <summary>
        /// refresh the pad. the area to render gets computed from the screen rectangle
        /// </summary>
        /// <param name="pminrow">the line of the top left corner to refresh</param>
        /// <param name="pmincol">the column of the top left corner to refresh</param>
        /// <param name="sminrow">the line of the top left corner of the screen to start the refresh (0)</param>
        /// <param name="smincol">the column of the top left corner of the screen to start the refresh (0)</param>
        /// <param name="smaxrow">the line of the bottom right corner of the screen to end the refresh (MaxLine - 1)</param>
        /// <param name="smaxcol">the column of the bottom right corner of the screen to end the refresh (MaxColumn - 1)</param>
        public void Refresh(int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            NativePad.prefresh(this.WindowPtr, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol);
        }

        /// <summary>
        /// not thread-safe
        /// see <see cref="Refresh(int, int, int, int, int, int)"/>
        /// </summary>
        public override void Refresh()
        {
            this.Refresh(0, 0, 0, 0, NCurses.StdScr.MaxLine - 1, NCurses.StdScr.MaxColumn - 1);
        }
        #endregion

        #region echo
        public abstract void Echo(char ch);
        #endregion

        public override void Put(int ch)
        {
            this.Window.Put(ch);
        }
    }
}
