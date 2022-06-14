using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NippyWard.NCurses.Interop.SafeHandles
{
    public abstract class NCursesSafeHandle : SafeHandle, IEquatable<NCursesSafeHandle>
    {
        public override bool IsInvalid => this.handle == IntPtr.Zero;

        public NCursesSafeHandle(bool ownsHandle)
            : base(IntPtr.Zero, ownsHandle)
        { }

        public bool Equals(NCursesSafeHandle other)
        {
            IntPtr currentPtr = this.handle;
            IntPtr otherPtr = other.DangerousGetHandle();

            return currentPtr == otherPtr;
        }

        public override bool Equals(object obj)
        {
            if (obj is NCursesSafeHandle other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            IntPtr currentPtr = this.handle;
            return currentPtr.GetHashCode();
        }
    }
}
