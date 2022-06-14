using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NippyWard.NCurses.Tasks
{
    internal static class NCursesWrapper
    {
        [DllImport("libncursesw6.dll")]
        public static extern IntPtr curses_version();

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern bool SetDllDirectoryA(string lpPathName);
    }
}
