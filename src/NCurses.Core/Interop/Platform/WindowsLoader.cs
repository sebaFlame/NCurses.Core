using System;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.Platform
{
    public class WindowsLoader : INativeLoader
    {
        [DllImport("kernel32.dll")]
        internal extern static IntPtr LoadLibrary(string libToLoad);

        [DllImport("kernel32.dll")]
        internal extern static IntPtr GetProcAddress(IntPtr libHandle, string symbol);

        [DllImport("kernel32.dll")]
        internal extern static bool FreeLibrary(IntPtr libHandle);

        //should be available in every .NET Core install
        [DllImport("api-ms-win-crt-locale-l1-1-0")]
        internal extern static IntPtr setlocale(int category, string locale);

        internal WindowsLoader() { }

        public IntPtr LoadModule(string moduleName)
        {
            return LoadLibrary(moduleName);
        }

        public IntPtr GetSymbolPointer(IntPtr modulePtr, string symbolName)
        {
            return GetProcAddress(modulePtr, symbolName);
        }

        public bool FreeModule(IntPtr modulePtr)
        {
            return FreeLibrary(modulePtr);
        }

        public void SetLocale(string locale)
        {
            //LC_ALL = 0
            setlocale(0, "");
        }
    }
}
