using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop
{
    public class NCursesException : Exception
    {
        internal NCursesException(string message)
            : base(message)
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Verify(int result, string method)
        {
            if (result == Constants.ERR)
            {
                throw new NCursesException($"{method} returned ERR");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static IntPtr Verify(IntPtr result, string method)
        {
            if (result == IntPtr.Zero)
            {
                throw new NCursesException($"{method} returned NULL");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T Verify<T>(T result, string method)
            where T : NCursesSafeHandle
        {
            if (result.IsInvalid)
            {
                throw new NCursesException($"{method} returned NULL");
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ref T Verify<T>(ref T result, string method)
            where T : unmanaged
        {
            IntPtr resPtr;

            unsafe
            {
                resPtr = new IntPtr(Unsafe.AsPointer(ref result));
            }

            if (resPtr == IntPtr.Zero)
            {
                throw new NCursesException($"{method} returned NULL");
            }

            return ref result;
        }
    }
}
