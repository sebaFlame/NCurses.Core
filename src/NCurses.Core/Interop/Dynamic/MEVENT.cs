using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.Dynamic
{
    /*
    typedef struct {
        short id;         ID to distinguish multiple devices 
        int x, y, z;      event coordinates 
        mmask_t bstate;   button state bits 
    } MEVENT;
    */

    [StructLayout(LayoutKind.Sequential)]
    internal struct MEVENT : IMEVENT
    {
        public short id;        /* ID to distinguish multiple devices */
        public int x, y, z;     /* event coordinates (character-cell) */
        public chtype bstate;     /* button state bits */

        public MEVENT(short id, int x, int y, int z, ulong bstate)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
            this.bstate = bstate;
        }

        public short ID => this.id;
        public int X => this.x;
        public int Y => this.y;
        public int Z => this.z;
        public ulong BState => this.bstate;
    }
}
