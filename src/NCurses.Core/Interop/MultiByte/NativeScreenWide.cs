using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.MultiByteString;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativeScreenWide
    {
        void wunctrl_sp(IntPtr screen, in INCursesWCHAR wch, out string str);
    }

    public class NativeScreenWide<TWide, TWideStr, TSmall, TSmallStr, TMouseEvent> : NativeWideBase<TWide, TWideStr, TSmall, TSmallStr, TMouseEvent>, INativeScreenWide
        where TWide : unmanaged, INCursesWCHAR, IEquatable<TWide>
        where TWideStr : unmanaged
        where TSmall : unmanaged, INCursesSCHAR, IEquatable<TSmall>
        where TSmallStr : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public void wunctrl_sp(IntPtr screen, in INCursesWCHAR wch, out string str)
        {
            str = NativeWideStrBase<TWideStr, TSmallStr>.ReadString(ref this.Wrapper.wunctrl_sp(screen, MarshallArrayReadonly(wch)));
        }
    }
}
