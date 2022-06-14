using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NippyWard.NCurses.Interop
{
    internal struct InternalLockDisposable : IDisposable
    {
        private bool prevVal;

        internal InternalLockDisposable(bool previousValue)
        {
            Monitor.Enter(NativeNCurses.SyncRoot);
            this.prevVal = previousValue && NativeNCurses.EnableLocking;
            NativeNCurses.EnableLocking = true;
        }

        public void Dispose()
        {
            NativeNCurses.EnableLocking = prevVal;
            Monitor.Exit(NativeNCurses.SyncRoot);
        }
    }
}
