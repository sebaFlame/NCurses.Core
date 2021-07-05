using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.MultiByte
{
    internal class NativeScreenMultiByte<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> 
            : MultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>, 
            INativeScreenMultiByte<TMultiByte, MultiByteCharString<TMultiByte, TWideChar, TSingleByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeScreenMultiByte(IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> wrapper)
            : base(wrapper) { }

        public void wunctrl_sp(IntPtr screen, in TMultiByte wch, out string str)
        {
            ref TWideChar strRef = ref this.Wrapper.wunctrl_sp(screen, in wch);

            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(WideCharFactory<TWideChar>._CreatePooledBuffer, ref strRef, out WideCharString<TWideChar> wideStr))
            {
                str = wideStr.ToString();
            }
        }
    }
}
