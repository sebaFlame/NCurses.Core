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
            INativeScreenMultiByte<TMultiByte, MultiByteCharString<TMultiByte>>
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TChar : unmanaged, IChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeScreenMultiByte(IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> wrapper)
            : base(wrapper) { }

        public void wunctrl_sp(IntPtr screen, in TMultiByte wch, out string str)
        {
            ref TWideChar strRef = ref this.Wrapper.wunctrl_sp(screen, in wch);

            WideCharString<TWideChar> wideStr = WideCharFactoryInternal<TWideChar>.Instance.CreateNativeString(ref strRef);

            str = wideStr.ToString();
        }
    }
}
