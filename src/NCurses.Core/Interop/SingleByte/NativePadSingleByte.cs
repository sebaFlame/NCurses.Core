using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    internal class NativePadSingleByte<TSingleByte, TChar, TMouseEvent> 
            : SingleByteWrapper<TSingleByte, TChar, TMouseEvent>, 
            INativePadSingleByte<TSingleByte, SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TChar : unmanaged, IChar, IEquatable<TChar>
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
