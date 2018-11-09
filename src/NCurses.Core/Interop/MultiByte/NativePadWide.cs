using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativePadWide
    {
        void pecho_wchar(IntPtr pad, in INCursesWCHAR wch);
    }
    
    public class NativePadWide<TWide, TWideStr, TSmall, TSmallStr, TMouseEvent> : NativeWideBase<TWide, TWideStr, TSmall, TSmallStr, TMouseEvent>, INativePadWide
        where TWide : unmanaged, INCursesWCHAR, IEquatable<TWide>
        where TWideStr : unmanaged
        where TSmall : unmanaged, INCursesSCHAR, IEquatable<TSmall>
        where TSmallStr : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public void pecho_wchar(IntPtr pad, in INCursesWCHAR wch)
        {
            NCursesException.Verify(Wrapper.pecho_wchar(pad, MarshallArrayReadonly(wch)), "pecho_wchar");
        }
    }
}
