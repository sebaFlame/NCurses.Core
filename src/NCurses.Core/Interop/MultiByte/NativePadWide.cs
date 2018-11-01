using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativePadWide
    {
        void pecho_wchar(IntPtr pad, in INCursesWCHAR wch);
    }
    
    public class NativePadWide<TWide, TWideStr, TSmall, TSmallStr> : NativeWideBase<TWide, TWideStr, TSmall, TSmallStr>, INativePadWide
        where TWide : unmanaged, INCursesWCHAR
        where TWideStr : unmanaged
        where TSmall : unmanaged, INCursesSCHAR
        where TSmallStr : unmanaged
    {
        public void pecho_wchar(IntPtr pad, in INCursesWCHAR wch)
        {
            NCursesException.Verify(Wrapper.pecho_wchar(pad, MarshallArrayReadonly(wch)), "pecho_wchar");
        }
    }
}
