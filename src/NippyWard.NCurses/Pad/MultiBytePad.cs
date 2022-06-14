using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Window;
using NippyWard.NCurses.Interop;
using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.SafeHandles;
using NippyWard.NCurses.Interop.Wrappers;

namespace NippyWard.NCurses.Pad
{
    public class MultiBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        public override bool HasUnicodeSupport => true;

        internal MultiBytePad(WindowBaseSafeHandle windowBaseSafeHandle)
            : base(windowBaseSafeHandle)
        { }

        internal MultiBytePad(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window)
            : base(window)
        { }

        public MultiBytePad(int nlines, int ncols)
            : base(NCurses.newpad(nlines, ncols))
        { }

        public override void Echo(char ch)
        {
            TMultiByte wCh = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch);
            Pad.pecho_wchar(this.WindowBaseSafeHandle, in wCh);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowBaseSafeHandle windowBaseSafeHandle,
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow)
        {
            return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle, parentWindow);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
        {
            return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(existingWindow);
        }
    }
}
