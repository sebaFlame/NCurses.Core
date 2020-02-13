using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SafeHandles
{
    public class StdScrSafeHandle : WindowBaseSafeHandle
    {
        public StdScrSafeHandle()
            : base(true) { }

        protected override bool ReleaseHandle()
        {
            try
            {
                NativeNCurses.NCurses.endwin();
                return true;
            }
            catch (NCursesException)
            {
                return false;
            }
        }
    }
}
