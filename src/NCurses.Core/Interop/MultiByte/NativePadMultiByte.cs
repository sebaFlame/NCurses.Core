using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativePadMultiByte
    {
        void pecho_wchar(IntPtr pad, in IMultiByteChar wch);
    }
    
    public class NativePadMultiByte<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent> : MultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent>, INativePadMultiByte
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TMultiByteString : unmanaged
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public void pecho_wchar(IntPtr pad, in IMultiByteChar wch)
        {
            NCursesException.Verify(Wrapper.pecho_wchar(pad, MarshallArrayReadonly(wch)), "pecho_wchar");
        }
    }
}
