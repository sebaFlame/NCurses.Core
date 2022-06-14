using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.SafeHandles
{
    public class StdScrSafeHandle : WindowBaseSafeHandle
    {
        public StdScrSafeHandle()
            : base(true) { }

        protected override bool ReleaseHandle()
        {
            try
            {
                NCurses.End();
                return true;
            }
            catch (NCursesException)
            {
                return false;
            }
        }
    }
}
