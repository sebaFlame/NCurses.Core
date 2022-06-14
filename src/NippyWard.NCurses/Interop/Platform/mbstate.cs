using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NippyWard.NCurses.Interop.Platform
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct mbstate
    {
        /* 
        __WINT_TYPE__ unsigned int

        typedef struct
        {
          int __count;
          union
          {
            __WINT_TYPE__ __wch;
            char __wchb[4];
          } __value;
        } __mbstate_t;
        */

        internal int _count;
        internal UInt32 value;
    }
}
