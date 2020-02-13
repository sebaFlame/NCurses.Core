using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativeScreenMultiByte<TChar, TCharString>
        where TChar : IMultiByteChar
        where TCharString : IMultiByteCharString
    {
        void wunctrl_sp(IntPtr screen, in TChar wch, out string str);
    }
}
