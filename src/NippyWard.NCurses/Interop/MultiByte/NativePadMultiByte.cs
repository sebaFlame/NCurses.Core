using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.SafeHandles;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.SingleByte;

namespace NippyWard.NCurses.Interop.MultiByte
{
    internal class NativePadMultiByte<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> 
            : MultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>, 
            INativePadMultiByte<TMultiByte, MultiByteCharString<TMultiByte, TWideChar, TSingleByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativePadMultiByte(IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> wrapper)
            : base(wrapper) { }

        public void pecho_wchar(WindowBaseSafeHandle pad, in TMultiByte wch)
        {
            NCursesException.Verify(Wrapper.pecho_wchar(pad, in wch), "pecho_wchar");
        }
    }
}
