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

        internal SingleBytePad(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window)
            : base(window)
        { }

        public SingleBytePad(int nlines, int ncols)
            : base(NCurses.newpad(nlines, ncols))
        { }

        public override void Echo(char ch)
        {
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            TSingleByte sch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch);
            Pad.pechochar(this.WindowBaseSafeHandle, in sch);
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
