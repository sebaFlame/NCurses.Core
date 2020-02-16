using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NCurses.Core.Interop.Platform
{
    internal class LinuxLoader : INativeLoader
    {
        [DllImport("libdl.so.2")]
        internal static extern IntPtr dlopen(string dllToLoad, int flags);

        [DllImport("libdl.so.2")]
        internal static extern IntPtr dlsym(IntPtr libHandle, string symbol);

        [DllImport("libdl.so.2")]
        internal static extern int dlclose(IntPtr libHandle);

        [DllImport("libc")]
        internal static extern string setlocale(int category, string locale);

        public IntPtr LoadModule(string moduleName)
        {
            moduleName = moduleName.Contains(".so") ? moduleName : string.Concat(moduleName, ".so");
            return dlopen(moduleName, 2);
        }

        public IntPtr GetSymbolPointer(IntPtr modulePtr, string symbolName)
        {
            return dlsym(modulePtr, symbolName);
        }

        public bool FreeModule(IntPtr modulePtr)
        {
            return dlclose(modulePtr) == 0;
        }

        //guarantee this gets called only once!!! causes random "corrupted double-linked list"
        public void SetLocale()
        {
            //LC_ALL = 6
            string res = setlocale(6, "");

            /* Return values:
             * ubuntu-18.04-x64 (WSL): C.UTF-8
             */
        }
    }
}
