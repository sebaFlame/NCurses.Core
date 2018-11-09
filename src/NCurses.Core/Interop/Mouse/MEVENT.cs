using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.SingleByte;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.Mouse
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
