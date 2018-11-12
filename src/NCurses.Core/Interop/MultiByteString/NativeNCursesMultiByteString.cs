using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.SingleByteString;

namespace NCurses.Core.Interop.MultiByteString
{
    public interface INativeNCursesMultiByteString
    {
        void erasewchar(out char wch);
        string key_name(in char ch);
        void killwchar(out char wch);
        void slk_wset(int labnum, in string label, int fmt);
        void unget_wch(in char wch);
    }

    internal class NativeNCursesMultiByteString<TMultiByteString, TSingleByteString> : MultiByteStringWrapper<TMultiByteString, TSingleByteString>, INativeNCursesMultiByteString
        where TMultiByteString : unmanaged
        where TSingleByteString : unmanaged
    {
        public void erasewchar(out char wch)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[1];
                NCursesException.Verify(this.Wrapper.erasewchar(ref MarshalString(strPtr, 1, out Span<TMultiByteString> span)), "erasewchar");
                wch = ReadChar(span);
            }
        }

        public string key_name(in char ch)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetCharLength()];
                return SingleByteStringWrapper<TSingleByteString>.ReadString(ref this.Wrapper.key_name(MarshalChar(ch, byteArray, byteLength)));
            }
        }

        public void killwchar(out char wch)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[1];
                NCursesException.Verify(this.Wrapper.killwchar(ref MarshalString(strPtr, 1, out Span<TMultiByteString> span)), "killwchar");
                wch = ReadChar(span);
            }
        }

        public void slk_wset(int labnum, in string label, int fmt)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(label)];
                NCursesException.Verify(this.Wrapper.slk_wset(labnum, MarshalStringReadonly(label, byteArray, byteLength), fmt), "slk_wset");
            }
        }

        public void unget_wch(in char wch)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetCharLength()];
                NCursesException.Verify(this.Wrapper.unget_wch(MarshalChar(wch, byteArray, byteLength)), "unget_wch");
            }
        }
    }
}
