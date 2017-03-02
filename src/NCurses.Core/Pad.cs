using System;
using NCurses.Core.Interop;

namespace NCurses.Core
{
    public class Pad : WindowBase
    {
        public Pad(int nlines, int ncols)
            : base()
        {
            DictPtrWindows.Add(this.WindowPtr = NativeNCurses.newpad(nlines, ncols), this);
        }

        public Pad(Pad parentPad, int nlines, int ncols, int begin_y, int begin_x)
            : base()
        {
            DictPtrWindows.Add(this.WindowPtr = NativeNCurses.subpad(parentPad.WindowPtr, nlines, ncols, begin_y, begin_x), this);
        }

        public Pad(Pad parentPad)
            : this(parentPad, 0, 0, 0, 0) { }

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
        /// not thread-safe.
        /// see <see cref="NoOutRefresh(int, int, int, int, int, int)"/>
        /// </summary>
        public void NoOutRefresh(int pminrow, int pmincol)
        {
            this.NoOutRefresh(pminrow, pmincol, 0, 0, NCurses.stdscr.MaxLine - 1, NCurses.stdscr.MaxColumn - 1);
        }

        /// <summary>
        /// not thread-safe
        /// see <see cref="NoOutRefresh(int, int, int, int, int, int)"/>
        /// </summary>
        public override void NoOutRefresh()
        {
            this.NoOutRefresh(0, 0, 0, 0, NCurses.stdscr.MaxLine - 1, NCurses.stdscr.MaxColumn - 1);
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
        public void Refresh(int pminrow, int pmincol)
        {
            this.Refresh(pminrow, pmincol, 0, 0, NCurses.stdscr.MaxLine - 1, NCurses.stdscr.MaxColumn - 1);
        }

        /// <summary>
        /// not thread-safe
        /// see <see cref="Refresh(int, int, int, int, int, int)"/>
        /// </summary>
        public override void Refresh()
        {
            this.Refresh(0, 0, 0, 0, NCurses.stdscr.MaxLine - 1, NCurses.stdscr.MaxColumn - 1);
        }
        #endregion

        /// <summary>
        /// returns a subpad with the current pad as parent
        /// </summary>
        /// <returns>the new subpad</returns>
        public override WindowBase SubWindow()
        {
            return new Pad(this);
        }

        #region IDisposable
        protected override void Dispose(bool disposing)
        {
            if (this.WindowPtr != IntPtr.Zero && this.OwnsHandle)
                NativeNCurses.delwin(this.WindowPtr);
            base.Dispose(disposing);
        }
        #endregion
    }
}
