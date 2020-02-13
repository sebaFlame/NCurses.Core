using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SafeHandles
{
    public class WindowSafeHandle : WindowBaseSafeHandle
    {
        public WindowSafeHandle()
            : base(false) { }
    }
}
