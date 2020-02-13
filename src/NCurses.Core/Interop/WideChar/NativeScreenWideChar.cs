using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.WideChar
{
    internal class NativeScreenWideChar<TWideChar, TChar> 
            : WideCharWrapper<TWideChar, TChar>, 
            INativeScreenWideChar<TWideChar, WideCharString<TWideChar>>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
    {
        internal NativeScreenWideChar(IWideCharWrapper<TWideChar, TChar> wrapper)
            : base(wrapper) { }

        public void unget_wch_sp(IntPtr screen, in TWideChar wch)
        {
            NCursesException.Verify(this.Wrapper.unget_wch_sp(screen, wch), "unget_wch_sp");
        }
    }
}
