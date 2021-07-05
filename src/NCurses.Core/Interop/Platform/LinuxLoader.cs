using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Globalization;

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

        /*
        //std::size_t mbstowcs( wchar_t* dst, const char* src, std::size_t len);
        [DllImport("libc")]
        internal static extern int mbstowcs(ref byte wideChars, in byte mbString, int charLength);

        [DllImport("libc")]
        internal static extern int mbstowcs(IntPtr wideChars, in byte mbString, int charLength);
        */

        //std::size_t wcsrtombs( char* dst, const wchar_t** src, std::size_t len, std::mbstate_t* ps );
        [DllImport("libc")]
        internal static extern int wcsrtombs(IntPtr mbString, IntPtr src, int len, ref mbstate ps);

        //std::size_t mbsrtowcs( wchar_t* dst, const char** src, std::size_t len, std::mbstate_t* ps );
        [DllImport("libc")]
        internal static extern int mbsrtowcs(IntPtr dst, IntPtr src, int len, ref mbstate ps);

        //std::size_t mbrtowc( wchar_t* pwc, const char* s, std::size_t n,std::mbstate_t* ps );
        [DllImport("libc")]
        internal static extern int mbrtowc(IntPtr pwc, IntPtr s, int n, ref mbstate ps);

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
            /* default should always be C.UTF-8 */
            string res = setlocale(6, "");

            /* Return values:
             * ubuntu-18.04-x64 (WSL): C.UTF-8
             *   - use mbstowcs to widen instead of using UTF32
             */

            if (!res.Contains("UTF-8"))
            {
                throw new NotSupportedException("Could not set console to support UTF-8");
            }
        }
    }
}
