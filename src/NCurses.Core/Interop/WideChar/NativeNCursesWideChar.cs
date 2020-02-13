using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

using System.Runtime.InteropServices;

using NCurses.Core.Interop.Char;

namespace NCurses.Core.Interop.WideChar
{
    internal class NativeNCursesWideChar<TWideChar, TChar> 
            : WideCharWrapper<TWideChar, TChar>, 
            INativeNCursesWideChar<TWideChar, WideCharString<TWideChar>>
        where TWideChar : unmanaged, IChar, IEquatable<TWideChar>
        where TChar : unmanaged, IChar, IEquatable<TChar>
    {
        internal NativeNCursesWideChar(IWideCharWrapper<TWideChar, TChar> wrapper)
            : base(wrapper) { }

        public void erasewchar(out TWideChar wch)
        {
            wch = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyCharInternal();
            NCursesException.Verify(this.Wrapper.erasewchar(ref wch), "erasewTWideChar");
        }

        public string key_name(in TWideChar ch)
        {
            return CharFactoryInternal<TChar>.Instance.CreateNativeString(ref this.Wrapper.key_name(ch)).ToString();
        }

        public void killwchar(out TWideChar wch)
        {
            wch = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyCharInternal();
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
