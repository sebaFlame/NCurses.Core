using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.MultiByteString
{
    public interface INativeScreenWideStr
    {
        void unget_wch_sp(IntPtr screen, in char wch);
    }

    internal class NativeScreenWideStr<TWideStr, TSmallStr> : NativeWideStrBase<TWideStr, TSmallStr>, INativeScreenWideStr
        where TWideStr : unmanaged
        where TSmallStr : unmanaged
    {
        public void unget_wch_sp(IntPtr screen, in char wch)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetCharLength()];
                NCursesException.Verify(this.Wrapper.unget_wch_sp(screen, MarshalChar(wch, byteArray, byteLength)), "unget_wch_sp");
            }
        }
    }
}
