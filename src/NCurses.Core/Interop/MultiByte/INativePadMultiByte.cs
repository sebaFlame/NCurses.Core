using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativePadMultiByte<TChar, TCharString>
        where TChar : IMultiByteChar
        where TCharString : IMultiByteCharString
    {
        void pecho_wchar(WindowBaseSafeHandle pad, in TChar wch);
    }
}
