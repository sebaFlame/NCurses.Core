using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.SafeHandles;

namespace NippyWard.NCurses.Interop.MultiByte
{
    public interface INativePadMultiByte<TChar, TCharString>
        where TChar : IMultiByteNCursesChar
        where TCharString : IMultiByteNCursesCharString
    {
        void pecho_wchar(WindowBaseSafeHandle pad, in TChar wch);
    }
}
