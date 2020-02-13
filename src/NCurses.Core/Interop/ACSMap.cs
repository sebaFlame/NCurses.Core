using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.MultiByte;

namespace NCurses.Core.Interop
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
