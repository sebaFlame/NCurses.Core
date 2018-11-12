using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.MultiByteString;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativeScreenMultiByte
    {
        void wunctrl_sp(IntPtr screen, in IMultiByteChar wch, out string str);
    }

    public class NativeScreenMultiByte<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent> : MultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent>, INativeScreenMultiByte
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TMultiByteString : unmanaged
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public void wunctrl_sp(IntPtr screen, in IMultiByteChar wch, out string str)
        {
            str = MultiByteStringWrapper<TMultiByteString, TSingleByteString>.ReadString(ref this.Wrapper.wunctrl_sp(screen, MarshallArrayReadonly(wch)));
        }
    }
}
