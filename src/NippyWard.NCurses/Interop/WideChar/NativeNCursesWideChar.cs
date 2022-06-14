using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

using System.Runtime.InteropServices;

using NippyWard.NCurses.Interop.Char;

namespace NippyWard.NCurses.Interop.WideChar
{
    internal class NativeNCursesWideChar<TWideChar, TChar> 
            : WideCharWrapper<TWideChar, TChar>, 
            INativeNCursesWideChar<TWideChar, WideCharString<TWideChar>>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
    {
        internal NativeNCursesWideChar(IWideCharWrapper<TWideChar, TChar> wrapper)
            : base(wrapper) { }

        public void erasewchar(out TWideChar wch)
        {
            wch = default;
            NCursesException.Verify(this.Wrapper.erasewchar(ref wch), "erasewTWideChar");
        }

        public string key_name(in TWideChar ch)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                ref NCursesException.Verify(ref this.Wrapper.key_name(ch), "key_name"),
                out CharString<TChar> @string))
            {
                return @string.ToString();
            }
        }

        public void killwchar(out TWideChar wch)
        {
            wch = default;
            NCursesException.Verify(this.Wrapper.killwchar(ref wch), "killwTWideChar");
        }

        public void slk_wset(int labnum, in WideCharString<TWideChar> label, int fmt)
        {
            NCursesException.Verify(this.Wrapper.slk_wset(labnum, in label.GetPinnableReference(), fmt), "slk_wset");
        }

        public void unget_wch(in TWideChar wch)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                NativeNCurses.NCursesWrapper.ungetch(ToPrimitiveType<int>(wch));
            }
            else
            {
                NCursesException.Verify(this.Wrapper.unget_wch(wch), "unget_wch");
            }
        }
    }
}
