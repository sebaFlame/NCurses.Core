using System;
using System.Collections.Generic;
using System.Text;
using NippyWard.NCurses.Interop.SingleByte;
using System.Runtime.InteropServices;

namespace NippyWard.NCurses.Interop.Mouse
{
    public interface IMEVENT
    {
        short ID { get; }
        int X { get; }
        int Y { get; }
        int Z { get; }
        ulong BState { get; }
    }
}
