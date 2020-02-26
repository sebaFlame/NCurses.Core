using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Window;
using NCurses.Core.Interop;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Wrappers;

namespace NCurses.Core.Pad
{
    public class SingleBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        public override bool HasUnicodeSupport => false;

        internal SingleBytePad(WindowBaseSafeHandle windowBaseSafeHandle)
            : base(windowBaseSafeHandle)
        { }

        internal SingleBytePad(
            WindowBaseSafeHandle windowBaseSafeHandle,
            PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentPad)
            : base(windowBaseSafeHandle, parentPad)
        { }

        internal SingleBytePad(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window)
            : base(window)
        { }

        public SingleBytePad(int nlines, int ncols)
            : base(NCurses.newpad(nlines, ncols))
        { }

        public override void Echo(char ch)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            Pad.pechochar(this.WindowBaseSafeHandle, in sch);
        }

        public override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Duplicate()
        {
            return new SingleBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(
                new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(
                    NCurses.dupwin(this.WindowBaseSafeHandle)));
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowBaseSafeHandle windowBaseSafeHandle, 
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow)
        {
            return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle, parentWindow);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
        {
            return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(existingWindow);
        }
    }
}
