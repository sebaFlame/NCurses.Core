using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SafeHandles
{
    public class NewWindowSafeHandle : WindowBaseSafeHandle
    {
        public NewWindowSafeHandle()
            : base(true) { }

        internal NewWindowSafeHandle(IntPtr windowPtr)
            : base(true) 
        {
            this.SetHandle(windowPtr);
        }
    }
}
