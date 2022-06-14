using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.SafeHandles
{
    public class WindowSafeHandle : WindowBaseSafeHandle
    {
        public WindowSafeHandle()
            : base(false) { }
    }
}
