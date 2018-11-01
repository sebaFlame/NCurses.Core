using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.Small
{
    public interface INativePadSmall
    {
        void pechochar(IntPtr pad, in INCursesSCHAR ch);
    }
    
    public class NativePadSmall<TSmall, TSmallStr> : NativeSmallBase<TSmall, TSmallStr>, INativePadSmall
        where TSmall : unmanaged, INCursesSCHAR
        where TSmallStr : unmanaged
    {
        public void pechochar(IntPtr pad, in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.pechochar(pad, MarshallArrayReadonly(ch)), "pechochar");
        }
    }
}
