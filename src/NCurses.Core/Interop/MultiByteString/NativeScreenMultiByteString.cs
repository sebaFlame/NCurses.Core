using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.MultiByteString
{
    public interface INativeScreenMultiByteString
    {
        void unget_wch_sp(IntPtr screen, in char wch);
    }

    internal class NativeScreenMultiByteString<TMultiByteString, TSingleByteString> : MultiByteStringWrapper<TMultiByteString, TSingleByteString>, INativeScreenMultiByteString
        where TMultiByteString : unmanaged
        where TSingleByteString : unmanaged
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
