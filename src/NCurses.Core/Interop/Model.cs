using System;
using System.Runtime.InteropServices;

#if NCURSES_VERSION_6
using chtype = System.UInt32;
#elif NCURSES_VERSION_5
using chtype = System.UInt64;
#endif

namespace NCurses.Core.Interop
{
    public class NCursesException : Exception
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
        public chtype bstate;     /* button state bits */
    }

    internal delegate int NCURSES_WINDOW_CB(IntPtr window, IntPtr args);
    internal delegate int NCURSES_SCREEN_CB(IntPtr screen, IntPtr args);
}
