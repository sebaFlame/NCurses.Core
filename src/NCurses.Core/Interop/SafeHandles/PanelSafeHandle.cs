using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SafeHandles
{
    public class PanelSafeHandle : PanelBaseSafeHandle
    {
        public PanelSafeHandle()
            : base(false) { }
    }
}
