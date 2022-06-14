using System;
using System.Collections.Generic;
using System.IO;

using NippyWard.NCurses.Window;
using NippyWard.NCurses.Interop;
using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.SafeHandles;
using NippyWard.NCurses.Interop.Wrappers;

namespace NippyWard.NCurses
{
    public abstract class WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        protected HashSet<WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>> SubWindows
            = new HashSet<WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>>();

        private WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> ParentWindow;

        protected static NotImplementedException _MultiByteException = new NotImplementedException("Only useful in multibyte mode");
        protected static ArgumentOutOfRangeException _RangeException = new ArgumentOutOfRangeException("Char is not ASCII range");

        internal WindowInternal(WindowBaseSafeHandle windowBaseSafeHandle)
            : base(windowBaseSafeHandle)
        { }

        internal WindowInternal(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
            : base(existingWindow)
        { }

        internal WindowInternal(
            WindowBaseSafeHandle windowBaseSafeHandle,
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow)
            : base(windowBaseSafeHandle)
        {
            this.ParentWindow = parentWindow;
        }

        internal void RemoveSubWindow(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> childWindow)
        {
            this.SubWindows.Remove(childWindow);
        }

        internal void Advance<TString>(TString charString)
            where TString : ICharString
        {
            int currentLine = this.CursorLine;
            int currentColumn = this.CursorColumn;

            int newPosition = (currentColumn + (currentLine * this.MaxColumn)) + charString.Length;

            int newLine = (int)Math.Floor(newPosition / (double)this.MaxColumn);
            int newColumn = newPosition - (newLine * this.MaxColumn);

            if (newLine > this.MaxLine)
            {
                this.ScrollWindow(newLine - this.MaxLine);
                newLine = this.MaxLine;
            }

            this.MoveCursor(newLine, newColumn);
        }

        public override void AttributesOn(ulong attrs)
        {
            Window.wattr_on(this.WindowBaseSafeHandle, attrs);
        }

        public override void AttributesOff(ulong attrs)
        {
            Window.wattr_off(this.WindowBaseSafeHandle, attrs);
        }

        public override void Clear()
        {
            Window.wclear(this.WindowBaseSafeHandle);
        }

        public override void ClearToBottom()
        {
            Window.wclrtobot(this.WindowBaseSafeHandle);
        }

        public override void ClearToEol()
        {
            Window.wclrtoeol(this.WindowBaseSafeHandle);
        }

        public override void CurrentAttributesAndColor(out ulong attrs, out ushort colorPair)
        {
            Window.wattr_get(this.WindowBaseSafeHandle, out attrs, out colorPair);
        }

        public override void EnableAttributesAndColor(ulong attrs, ushort colorPair)
        {
            Window.wattr_set(this.WindowBaseSafeHandle, attrs, colorPair);
        }

        public override void EnableColor(short colorPair)
        {
            Window.wcolor_set(this.WindowBaseSafeHandle, colorPair);
        }

        public override void Erase()
        {
            Window.werase(this.WindowBaseSafeHandle);
        }

        /// <summary>
        /// efficient window refresh. follow up with <see cref="NCurses.Update"/>
        /// </summary>
        public override void NoOutRefresh()
        {
            Window.wnoutrefresh(this.WindowBaseSafeHandle);
        }

        public override void MoveCursor(int lineNumber, int columnNumber)
        {
            Window.wmove(this.WindowBaseSafeHandle, lineNumber, columnNumber);
        }

        /// <summary>
        /// Calling mvwin moves the window so that the upper left-hand corner is at position(<paramref name="ncols"/>, <paramref name="nlines"/>).
        /// </summary>
        /// <param name="nlines">The line to move the window to</param>
        /// <param name="ncols">The column to move the window to</param>
        public override void MoveWindow(int nlines, int ncols)
        {
            NCurses.mvwin(this.WindowBaseSafeHandle, nlines, ncols);
        }

        /// <summary>
        /// refresh the window with newly added characters
        /// </summary>
        public override void Refresh()
        {
            Window.wrefresh(this.WindowBaseSafeHandle);
        }

        public override void ScrollWindow(int lines)
        {
            Window.wscrl(this.WindowBaseSafeHandle, lines);
        }

        /// <summary>
        /// create a subwindow with the current window as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        internal virtual WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> SubWindowInternal(int nlines, int ncols, int begin_y, int begin_x)
        {
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window =
                this.CreateWindow(NCurses.subwin(this.WindowBaseSafeHandle, nlines, ncols, begin_y, begin_x), this);

            this.SubWindows.Add(window);

            return window;
        }

        /// <summary>
        /// create a subwindow with the current window as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        internal virtual WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> DerWindowInternal(int nlines, int ncols, int begin_y, int begin_x)
        {
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window =
                CreateWindow(NCurses.derwin(this.WindowBaseSafeHandle, nlines, ncols, begin_y, begin_x), this);

            this.SubWindows.Add(window);

            return window;
        }

        public override IWindow SubWindow(int nlines, int ncols, int begin_y, int begin_x) => this.DerWindowInternal(nlines, ncols, begin_y, begin_x);
        public override IWindow DerWindow(int nlines, int ncols, int begin_y, int begin_x) => this.DerWindowInternal(nlines, ncols, begin_y, begin_x);

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

        /// <summary>
        /// Resize the current window to <paramref name="lines"/> and <paramref name="columns"/>
        /// </summary>
        /// <param name="lines">The number of lines to resize to</param>
        /// <param name="columns">The numbe rof columns to resize to</param>
        public override void ResizeWindow(int lines, int columns)
        {
            Window.wresize(this.WindowBaseSafeHandle, lines, columns);
        }

        public override void Delete()
        {
            Window.wdelch(this.WindowBaseSafeHandle);
        }

        public override void Delete(int y, int x)
        {
            Window.mvwdelch(this.WindowBaseSafeHandle, y, x);
        }

        #region window creation
        internal abstract WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowBaseSafeHandle windowBaseSafeHandle, 
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow);

        internal abstract WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow);

        internal static WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(WindowBaseSafeHandle windowBaseSafeHandle)
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle);
            }
            else
            {
                return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle);
            }
        }

        internal static WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(int nlines, int ncols, int begy, int begx)
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols, begy, begx);
            }
            else
            {
                return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols, begy, begx);
            }
        }
        #endregion

        #region Window encoding switch
        internal virtual WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> ToSingleByteWindowInternal()
        {
            return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(this);
        }

        internal virtual WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> ToMultiByteWindowInternal()
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(this);
            }
            else
            {
                throw new InvalidOperationException("Unicode not supported");
            }
        }

        public override IWindow ToSingleByteWindow() => this.ToSingleByteWindowInternal();
        public override IWindow ToMultiByteWindow() => this.ToMultiByteWindowInternal();
        #endregion

        public override IWindow Duplicate()
        {
            return CreateWindow(NCurses.dupwin(this.WindowBaseSafeHandle));
        }

        public override void Overlay(IWindow destination)
        {
            if(!(destination is WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> destWindow))
            {
                throw new NotSupportedException("Unknown window type");
            }

            NCurses.overlay(this.WindowBaseSafeHandle, destWindow.WindowBaseSafeHandle);
        }

        public override void Overwrite(IWindow destination)
        {
            if (!(destination is WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> destWindow))
            {
                throw new NotSupportedException("Unknown window type");
            }

            NCurses.overwrite(this.WindowBaseSafeHandle, destWindow.WindowBaseSafeHandle);
        }

        public override void Copy(IWindow destination, int sminrow, int smincol, int dminrow, int dmincol, int dmaxrow, int dmaxcol, bool overlay)
        {
            if (!(destination is WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> destWindow))
            {
                throw new NotSupportedException("Unknown window type");
            }

            NCurses.copywin(this.WindowBaseSafeHandle, destWindow.WindowBaseSafeHandle, sminrow, smincol, dminrow, dmincol, dmaxrow, dmaxcol, overlay ? 1 : 0);
        }

        public override void Touch()
        {
            Window.touchwin(this.WindowBaseSafeHandle);
        }

        public override void TouchLines(int start, int count)
        {
            Window.touchline(this.WindowBaseSafeHandle, start, count);
        }

        public override void Dispose()
        {
            this.ParentWindow?.RemoveSubWindow(this);

            if(this.SubWindows.Count > 0)
            {
                throw new InvalidOperationException("Subwindows need to be disposed first");
            }

            base.Dispose();
        }
    }
}
