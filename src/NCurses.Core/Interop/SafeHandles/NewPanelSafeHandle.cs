using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SafeHandles
{
    public class NewPanelSafeHandle : PanelBaseSafeHandle
    {
        public NewPanelSafeHandle()
            : base(true) { }
    }
}
