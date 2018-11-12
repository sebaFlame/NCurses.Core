using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INativePadSingleByte
    {
        void pechochar(IntPtr pad, in ISingleByteChar ch);
    }
    
    public class NativePadSingleByte<TSingleByte, TSingleByteString, TMouseEvent> : SingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent>, INativePadSingleByte
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public void pechochar(IntPtr pad, in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.pechochar(pad, MarshallArrayReadonly(ch)), "pechochar");
        }
    }
}
