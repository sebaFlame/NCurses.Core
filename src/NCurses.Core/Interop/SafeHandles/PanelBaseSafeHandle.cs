﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.Panel;

namespace NCurses.Core.Interop.SafeHandles
{
    public abstract class PanelBaseSafeHandle : NCursesSafeHandle
    {
        public PanelBaseSafeHandle(bool ownsHandle)
            : base(ownsHandle) { }

        protected override bool ReleaseHandle()
        {
            try
            {
                NativePanel.NCursesWrapper.del_panel(this.handle);
                return true;
            }
            catch (NCursesException)
            {
                return false;
            }
        }
    }
}
