using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
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
