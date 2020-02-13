using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INativePadSingleByte<TChar, TCharString>
        where TChar : ISingleByteChar
        where TCharString : ISingleByteCharString
    {
        void pechochar(WindowBaseSafeHandle pad, in TChar ch);
    }
}
