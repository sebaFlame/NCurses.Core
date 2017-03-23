using System;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop
{
    internal class NCursesException : Exception
    {
        internal NCursesException(string message)
            : base(message)
        { }

        internal static void Verify(int result, string method)
        {
            if (result == Constants.ERR)
                throw new NCursesException(string.Format("{0} returned ERR", method));
        }

        internal static void Verify(IntPtr result, string method)
        {
            if (result == IntPtr.Zero)
                throw new NCursesException(string.Format("{0} returned NULL", method));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MEVENT
    {
        public short id;        /* ID to distinguish multiple devices */
        public int x, y, z;     /* event coordinates (character-cell) */
        public uint bstate;     /* button state bits */
    }

    internal delegate int NCURSES_WINDOW_CB(IntPtr window, IntPtr args);
    internal delegate int NCURSES_SCREEN_CB(IntPtr screen, IntPtr args);
}
