using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.MultiByte;

namespace NippyWard.NCurses.Interop
{
    public class ACSMap<TChar> : IACSMap
        where TChar : INCursesChar
    {
        private IntPtr acs_map_handle;

        public ACSMap(IntPtr ptr)
        {
            acs_map_handle = ptr;
        }

        public unsafe TChar this[char index]
        {
            get
            {
                ReadOnlySpan<TChar> acsSpan = new ReadOnlySpan<TChar>(this.acs_map_handle.ToPointer(), 128);
                return acsSpan[index];
            }
        }

        INCursesChar IACSMap.this[char index] => this[index];
    }
}
