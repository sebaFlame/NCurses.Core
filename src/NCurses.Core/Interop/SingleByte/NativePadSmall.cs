using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INativePadSmall
    {
        void pechochar(IntPtr pad, in INCursesSCHAR ch);
    }
    
    public class NativePadSmall<TSmall, TSmallStr, TMouseEvent> : NativeSmallBase<TSmall, TSmallStr, TMouseEvent>, INativePadSmall
        where TSmall : unmanaged, INCursesSCHAR, IEquatable<TSmall>
        where TSmallStr : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public void pechochar(IntPtr pad, in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.pechochar(pad, MarshallArrayReadonly(ch)), "pechochar");
        }
    }
}
