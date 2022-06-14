using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop
{
    public interface INativeCharWrapper<TCharInterface, TChar, TCharStringInterface, TCharString>
    {
        TCharString CastString(in TCharStringInterface wCharStr);
        TChar CastChar(in TCharInterface wChar);
    }
}
