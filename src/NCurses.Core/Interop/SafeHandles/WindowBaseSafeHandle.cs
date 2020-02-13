using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.SafeHandles
{
    public abstract class WindowBaseSafeHandle : NCursesSafeHandle
    {
        public WindowBaseSafeHandle(bool ownsHandle)
            : base(ownsHandle) { }

        protected override bool ReleaseHandle()
        {
            try
            {
                NativeNCurses.NCursesWrapper.delwin(this.handle);
                return true;
            }
            catch (NCursesException)
            {
                return false;
            }
        }
    }
}
