using System;
using NCurses.Core.Interop;

namespace NCurses.Core.StdScr
{
    public abstract class StdScrBase : WindowBase
    {
        internal StdScrBase(IntPtr stdScr)
            : base(stdScr) { }

        public override void AttributesOn(ulong attrs)
        {
            NativeStdScr.attr_on(attrs);
        }

        public override void AttributesOff(ulong attrs)
        {
            NativeStdScr.attr_off(attrs);
        }

        public override void Clear()
        {
            NativeStdScr.clear();
        }

        public override void ClearToBottom()
        {
            NativeStdScr.clrtobot();
        }

        public override void ClearToEol()
        {
            NativeStdScr.clrtoeol();
        }

        public override void CurrentAttributesAndColor(out ulong attrs, out short colorPair)
        {
            NativeStdScr.attr_get(out attrs, out colorPair);
        }

        public override void EnableAttributesAndColor(ulong attrs, short colorPair)
        {
            NativeStdScr.attr_set(attrs, colorPair);
        }

        public override void EnableColor(short colorPair)
        {
            NativeStdScr.color_set(colorPair);
        }

        public override void Erase()
        {
            NativeStdScr.erase();
        }

        public override void MoveCursor(int lineNumber, int columnNumber)
        {
            NativeStdScr.move(lineNumber, columnNumber);
        }

        /// <summary>
        /// Refreshes the current StdScr, might not be useful when you're only using stdscr
        /// </summary>
        public override void NoOutRefresh()
        {
            NativeWindow.wnoutrefresh(this.WindowPtr);
        }

        public override void Refresh()
        {
            NativeStdScr.refresh();
        }

        public override void ScrollWindow(int lines)
        {
            NativeStdScr.scrl(lines);
        }
    }
}
