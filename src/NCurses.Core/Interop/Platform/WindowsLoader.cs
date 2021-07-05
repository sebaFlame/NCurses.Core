using System;
using System.Runtime.InteropServices;
using System.Text;

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

        //[DllImport("kernel32.dll")]
        //internal static extern uint GetConsoleOutputCP();

        //[DllImport("kernel32.dll")]
        //internal extern static bool SetConsoleOutputCP(uint codePage);

        ////size_t mbstowcs(wchar_t* wcstr, const char* mbstr, size_t count);
        //[DllImport("api-ms-win-crt-multibyte-l1-1-0")]
        //internal extern static int mbstowcs(ref byte wcstr, in byte mbstr, int count);

        //[DllImport("api-ms-win-crt-multibyte-l1-1-0")]
        //internal extern static int mbstowcs(IntPtr wcstr, in byte mbstr, int count);

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

        public void SetLocale()
        {
            //LC_ALL = 0
            //default should always be 1252 (cmd.exe)

            /* NCurses uses CHAR_INFO
             * which uses WCHAR
             * which is UTF16 by default */
            setlocale(0, "");

            /* return values:
             * Local machine (Windows 10): Dutch_Belgium.1252
             *   After UTF-8: Dutch_Belgium.utf8
             */

            //setlocale(0, ".UTF8");

            //uint cp = GetConsoleOutputCP();
            //if (!SetConsoleOutputCP(65001))
            //{
            //    throw new NotSupportedException("Could not set console to support UTF-8");
            //}
        }
    }
}
