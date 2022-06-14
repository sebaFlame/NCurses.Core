using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.SafeHandles;

namespace NippyWard.NCurses.Interop.SingleByte
{
    public interface INativePadSingleByte<TChar, TCharString>
        where TChar : ISingleByteNCursesChar
        where TCharString : ISingleByteNCursesCharString
    {
        void pechochar(WindowBaseSafeHandle pad, in TChar ch);
    }
}
