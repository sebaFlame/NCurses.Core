using System;
using System.IO;
using NCurses.Core.Interop;

namespace NCurses.Core
{
    public class Window : WindowBase
    {
        internal Window(IntPtr winPtr, bool ownsHandle)
            : base(winPtr, ownsHandle)
        { }

        /// <summary>
        /// create a new window
        /// </summary>
        /// <param name="nlines">number of lines of the new window</param>
        /// <param name="ncols">number of columns of the new window</param>
        /// <param name="begy">line of the upper left corner of the new window</param>
        /// <param name="begx">column of the upper left corent of the new window</param>
        public Window(int nlines, int ncols, int begy, int begx)
            : base()
        {
            DictPtrWindows.Add(this.WindowPtr = NativeNCurses.newwin(nlines, ncols, begy, begx), this);
        }

        /// <summary>
        /// create a new window
        /// </summary>
        /// <param name="nlines">number of lines of the new window</param>
        /// <param name="ncols">number of columns of the new window</param>
        public Window(int nlines, int ncols)
            : this (nlines, ncols, 0, 0)
        { }

        public Window()
            : this (0, 0, 0, 0)
        { }

        public Window(Window parentWindow, int nlines, int ncols, int begy, int begx)
            : base()
        {
            DictPtrWindows.Add(this.WindowPtr = NativeNCurses.subwin(parentWindow.WindowPtr, nlines, ncols, begy, begx), this);
        }

        public Window(Window parentWindow)
            : this(parentWindow, 0, 0, 0, 0) { }

        /// <summary>
        /// refresh the window with newly added characters
        /// </summary>
        public override void Refresh()
        {
            NativeWindow.wrefresh(this.WindowPtr);
        }

        /// <summary>
        /// efficient window refresh. follow up with <see cref="NCurses.Update"/>
        /// </summary>
        public override void NoOutRefresh()
        {
            NativeWindow.wnoutrefresh(this.WindowPtr);
        }

        /// <summary>
        /// create a subwindow with the current window as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        public override WindowBase SubWindow()
        {
            return new Window(this);
        }

        public void ExecuteThreadSafeMethod(Action<Window> method)
        {
            Func<IntPtr, int> callback = (IntPtr window) =>
            {
                method(this);
                return Constants.OK;
            };
            NativeNCurses.use_window_v(this.WindowPtr, callback);
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
