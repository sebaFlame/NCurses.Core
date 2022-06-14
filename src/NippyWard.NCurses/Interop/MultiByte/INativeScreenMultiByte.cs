using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.MultiByte
{
    public interface INativeScreenMultiByte<TChar, TCharString>
        where TChar : IMultiByteNCursesChar
        where TCharString : IMultiByteNCursesCharString
    {
        void wunctrl_sp(IntPtr screen, in TChar wch, out string str);
    }
}
