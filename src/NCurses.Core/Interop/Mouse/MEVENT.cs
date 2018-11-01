using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.SingleByte;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.Mouse
{
    public interface IMEVENT
    {
        short ID { get; }
        int X { get; }
        int Y { get; }
        int Z { get; }
        ulong BState { get; }
    }

    /*
    typedef struct {
        short id;         ID to distinguish multiple devices 
        int x, y, z;      event coordinates 
        mmask_t bstate;   button state bits 
    } MEVENT;
    */

    [StructLayout(LayoutKind.Sequential)]
    internal struct MEVENT<TSMall> : IMEVENT
        where TSMall : unmanaged, INCursesSCHAR
    {
        public short id;        /* ID to distinguish multiple devices */
        public int x, y, z;     /* event coordinates (character-cell) */
        public TSMall bstate;     /* button state bits */

        public MEVENT(short id, int x, int y, int z, TSMall bstate)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.z = z;
            this.bstate = bstate;
        }

        public short ID => this.id;
        public int X => this.x;
        public int Y => this.Y;
        public int Z => this.z;
        public ulong BState => this.bstate.Attributes;
    }
}
