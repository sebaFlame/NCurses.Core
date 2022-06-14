using System;

using NippyWard.NCurses.Interop;
using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.SafeHandles;
using NippyWard.NCurses.Interop.Wrappers;

namespace NippyWard.NCurses.StdScr
{
    public abstract class StdScrBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal StdScrBase(WindowBaseSafeHandle stdScr)
            : base(stdScr) { }

        internal StdScrBase(StdScrBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> stdScr)
            : base(stdScr) 
        {  }

        public override void AttributesOn(ulong attrs)
        {
            StdScr.attr_on(attrs);
        }

        public override void AttributesOff(ulong attrs)
        {
            StdScr.attr_off(attrs);
        }

        public override void Clear()
        {
            StdScr.clear();
        }

        public override void ClearToBottom()
        {
            StdScr.clrtobot();
        }

        public override void ClearToEol()
        {
            StdScr.clrtoeol();
        }

        public override void CurrentAttributesAndColor(out ulong attrs, out ushort colorPair)
        {
            StdScr.attr_get(out attrs, out colorPair);
        }

        public override void EnableAttributesAndColor(ulong attrs, ushort colorPair)
        {
            StdScr.attr_set(attrs, colorPair);
        }

        public override void EnableColor(short colorPair)
        {
            StdScr.color_set(colorPair);
        }

        public override void Erase()
        {
            StdScr.erase();
        }

        public override void MoveCursor(int lineNumber, int columnNumber)
        {
            StdScr.move(lineNumber, columnNumber);
        }

        /// <summary>
        /// Refreshes the current StdScr, might not be useful when you're only using stdscr
        /// </summary>
        public override void NoOutRefresh()
        {
            Window.wnoutrefresh(this.WindowBaseSafeHandle);
        }

        public override void Refresh()
        {
            StdScr.refresh();
        }

        public override void ScrollWindow(int lines)
        {
            StdScr.scrl(lines);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> ToSingleByteWindowInternal()
        {
            return new SingleByteStdScr<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(this);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> ToMultiByteWindowInternal()
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteStdScr<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(this);
            }
            else
            {
                throw new InvalidOperationException("Unicode not supported");
            }
        }
    }
}
