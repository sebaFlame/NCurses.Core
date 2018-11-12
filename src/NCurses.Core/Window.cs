using System;
using System.IO;
using NCurses.Core.Interop;

namespace NCurses.Core
{
    public abstract class Window : WindowBase
    {
        //for usage with subwindows etc
        internal Window(IntPtr windowPtr, bool ownsHandle = true, bool initizalize = true)
            : base(windowPtr, ownsHandle, initizalize)
        {  }

        public override void AttributesOn(ulong attrs)
        {
            NativeWindow.wattr_on(this.WindowPtr, attrs);
        }

        public override void AttributesOff(ulong attrs)
        {
            NativeWindow.wattr_off(this.WindowPtr, attrs);
        }

        public override void Clear()
        {
            NativeWindow.wclear(this.WindowPtr);
        }

        public override void ClearToBottom()
        {
            NativeWindow.wclrtobot(this.WindowPtr);
        }

        public override void ClearToEol()
        {
            NativeWindow.wclrtoeol(this.WindowPtr);
        }

        public override void CurrentAttributesAndColor(out ulong attrs, out short colorPair)
        {
            NativeWindow.wattr_get(this.WindowPtr, out attrs, out colorPair);
        }

        public override void EnableAttributesAndColor(ulong attrs, short colorPair)
        {
            NativeWindow.wattr_set(this.WindowPtr, attrs, colorPair);
        }

        public override void EnableColor(short colorPair)
        {
            NativeWindow.wcolor_set(this.WindowPtr, colorPair);
        }

        public override void Erase()
        {
            NativeWindow.werase(this.WindowPtr);
        }

        /// <summary>
        /// efficient window refresh. follow up with <see cref="NCurses.Update"/>
        /// </summary>
        public override void NoOutRefresh()
        {
            NativeWindow.wnoutrefresh(this.WindowPtr);
        }

        public override void MoveCursor(int lineNumber, int columnNumber)
        {
            NativeWindow.wmove(this.WindowPtr, lineNumber, columnNumber);
        }

        /// <summary>
        /// Calling mvwin moves the window so that the upper left-hand corner is at position(<paramref name="ncols"/>, <paramref name="nlines"/>).
        /// </summary>
        /// <param name="nlines">The line to move the window to</param>
        /// <param name="ncols">The column to move the window to</param>
        public void MoveWindow(int nlines, int ncols)
        {
            NativeNCurses.mvwin(this.WindowPtr, nlines, ncols);
        }

        /// <summary>
        /// refresh the window with newly added characters
        /// </summary>
        public override void Refresh()
        {
            NativeWindow.wrefresh(this.WindowPtr);
        }

        public override void ScrollWindow(int lines)
        {
            NativeWindow.wscrl(this.WindowPtr, lines);
        }

        /// <summary>
        /// create a subwindow with the current window as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        public Window SubWindow(int nlines, int ncols, int begin_y, int begin_x)
        {
            return CreateWindow(NativeNCurses.subwin(this.WindowPtr, nlines, ncols, begin_y, begin_x));
        }

        /// <summary>
        /// create a subwindow with the current window as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        public Window DerWindow(int nlines, int ncols, int begin_y, int begin_x)
        {
            return CreateWindow(NativeNCurses.derwin(this.WindowPtr, nlines, ncols, begin_y, begin_x));
        }

        /// <summary>
        /// Creates an exact duplicate of the window win.
        /// </summary>
        /// <returns></returns>
        public abstract Window Duplicate();

        /// <summary>
        /// Draw a box around the edges of the window
        /// </summary>
        /// <param name="verticalChar">the vertical char with attributes to use</param>
        /// <param name="horizontalChar">the horizontal char with attributes to use</param>
        public abstract void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar);

        /// <summary>
        /// Draw a default box around the edges of the window
        /// </summary>
        public abstract void Box();

        #region window creation
        public abstract Window ToSingleByteWindow();
        public abstract Window ToMultiByteWindow();

        internal static Window CreateWindow(IntPtr windowPtr, bool initialize = true)
        {
            if (NCurses.UnicodeSupported)
                return CreateMultiByteWindow(windowPtr, initialize);
            return CreateSingleByteWindow(windowPtr, initialize);
        }

        public static Window CreateWindow(int nlines, int ncols, int begy, int begx)
        {
            if (NCurses.UnicodeSupported)
                return CreateMultiByteWindow(nlines, ncols, begy, begx);
            return CreateSingleByteWindow(nlines, ncols, begy, begx);
        }

        internal static Window CreateMultiByteWindow(IntPtr windowPtr, bool initialize = true)
        {
            return new MultiByteWindow(windowPtr, true, initialize);
        }

        public static Window CreateMultiByteWindow(int nlines, int ncols, int begy, int begx)
        {
            return new MultiByteWindow(nlines, ncols, begy, begx);
        }

        internal static Window CreateSingleByteWindow(IntPtr windowPtr, bool initialize = true)
        {
            return new SingleByteWindow(windowPtr, true, initialize);
        }

        public static Window CreateSingleByteWindow(int nlines, int ncols, int begy, int begx)
        {
            return new SingleByteWindow(nlines, ncols, begy, begx);
        }
        #endregion
    }
}
