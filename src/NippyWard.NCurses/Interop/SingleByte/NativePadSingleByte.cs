using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.SafeHandles;
using NippyWard.NCurses.Interop.Mouse;

namespace NippyWard.NCurses.Interop.SingleByte
{
    internal class NativePadSingleByte<TSingleByte, TChar, TMouseEvent> 
            : SingleByteWrapper<TSingleByte, TChar, TMouseEvent>, 
            INativePadSingleByte<TSingleByte, SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativePadSingleByte(ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> wrapper)
            : base(wrapper) { }

        public void pechochar(WindowBaseSafeHandle pad, in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.pechochar(pad, ch), "pechochar");
        }
    }
}
