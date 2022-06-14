using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Runtime.InteropServices;

namespace NippyWard.NCurses.Tests
{
    public class SkipWindowsFact : FactAttribute
    {
        public SkipWindowsFact(string reason)
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                this.Skip = reason;
        }
    }
}
