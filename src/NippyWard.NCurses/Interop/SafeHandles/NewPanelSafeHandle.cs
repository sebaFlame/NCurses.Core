using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.SafeHandles
{
    public class NewPanelSafeHandle : PanelBaseSafeHandle
    {
        public NewPanelSafeHandle()
            : base(true) { }
    }
}
