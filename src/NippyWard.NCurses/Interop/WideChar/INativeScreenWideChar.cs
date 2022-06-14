using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.WideChar
{
    public interface INativeScreenWideChar<TChar, TCharString>
        where TChar : IMultiByteChar
        where TCharString : IMultiByteCharString
    {
        void unget_wch_sp(IntPtr screen, in TChar wch);
    }
}
