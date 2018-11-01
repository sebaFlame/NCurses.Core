using System;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop
{
    internal class NCursesException : Exception
    {
        internal NCursesException(string message)
            : base(message)
        { }

        internal static int Verify(int result, string method)
        {
            if (result == Constants.ERR)
                throw new NCursesException($"{method} returned ERR");
            return result;
        }

        internal static IntPtr Verify(IntPtr result, string method)
        {
            if (result == IntPtr.Zero)
                throw new NCursesException($"{method} returned NULL");
            return result;
        }
    }
}
