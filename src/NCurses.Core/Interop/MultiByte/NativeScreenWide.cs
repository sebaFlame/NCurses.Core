using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.MultiByteString;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativeScreenWide
    {
        void wunctrl_sp(IntPtr screen, in INCursesWCHAR wch, out string str);
    }

    public class NativeScreenWide<TWide, TWideStr, TSmall, TSmallStr> : NativeWideBase<TWide, TWideStr, TSmall, TSmallStr>, INativeScreenWide
        where TWide : unmanaged, INCursesWCHAR, IEquatable<TWide>
        where TWideStr : unmanaged
        where TSmall : unmanaged, INCursesSCHAR, IEquatable<TSmall>
        where TSmallStr : unmanaged
    {
        public void wunctrl_sp(IntPtr screen, in INCursesWCHAR wch, out string str)
        {
            str = NativeWideStrBase<TWideStr, TSmallStr>.ReadString(ref this.Wrapper.wunctrl_sp(screen, MarshallArrayReadonly(wch)));
        }
    }
}
