using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NCurses.Core.Interop
{
    internal class InternalLockDisposable : IDisposable
    {
        private bool prevVal;

        internal InternalLockDisposable()
        {
            Monitor.Enter(NativeNCurses.SyncRoot);
            this.prevVal = NativeNCurses.EnableLocking;
            NativeNCurses.EnableLocking = true;
        }

        public void Dispose()
        {
            NativeNCurses.EnableLocking = prevVal;
            Monitor.Exit(NativeNCurses.SyncRoot);
        }
    }
}
