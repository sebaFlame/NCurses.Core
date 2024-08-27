using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;

namespace NippyWard.NCurses.Tasks
{
    internal delegate IntPtr GetVersion();

    public class LibraryVersionExtractor : Task
    {
        [Required]
        public string Directory { get; set; }

        [Output]
        public string NCursesVersion { get; private set; }

        private const string _LibraryName = "libncursesw6.dll";
        private const string _MethodName = "curses_version";

#if NET8_0
        public LibraryVersionExtractor()
        {

            try
            {
                NativeLibrary.SetDllImportResolver(typeof(LibraryVersionExtractor).Assembly, this.DllImportResolver);
            }
            catch (InvalidOperationException)
            {
                //ignore 2nd registration error
            }

        }

        private IntPtr DllImportResolver(string libraryName, Assembly assembly, Nullable<DllImportSearchPath> searchPath)
        {
            if(string.Equals(libraryName, _LibraryName))
            {
                return NCursesWrapper.LoadLibrary(Path.Combine(this.Directory, libraryName));
            }

            return NativeLibrary.Load(libraryName);
        }

        public override bool Execute()
        {
            try
            {
                IntPtr versionPtr = NCursesWrapper.curses_version();

                string version;
                unsafe
                {
                    byte* bArr = (byte*)versionPtr.ToPointer();
                    int stringLength = FindStringLength(bArr);
                    char* charArr = stackalloc char[stringLength];
                    Encoding.ASCII.GetChars(bArr, stringLength, charArr, stringLength);
                    version = new ReadOnlySpan<char>(charArr, stringLength).ToString();

                    this.NCursesVersion = version.Split(' ')[1];
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
#else
        public override bool Execute()
        {
            IntPtr module = IntPtr.Zero;

            try
            {
                module = NCursesWrapper.LoadLibrary(Path.Combine(this.Directory, _LibraryName));
                IntPtr method = NCursesWrapper.GetProcAddress(module, _MethodName);
                GetVersion getVersion = Marshal.GetDelegateForFunctionPointer<GetVersion>(method);
                IntPtr versionPtr = getVersion();

                string version;
                unsafe
                {
                    byte* bArr = (byte*)versionPtr.ToPointer();
                    int stringLength = FindStringLength(bArr);
                    char* charArr = stackalloc char[stringLength];
                    Encoding.ASCII.GetChars(bArr, stringLength, charArr, stringLength);
                    version = new string(charArr);

                    this.NCursesVersion = version.Split(' ')[1];
                }
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                NCursesWrapper.FreeLibrary(module);
            }
        }
#endif

        internal unsafe static int FindStringLength(byte* strArr)
        {
            int length = 0;
            byte val = 0;

            while (true)
            {
                val = *(strArr + (length++));
                if(val == 0)
                {
                    break;
                }
            }

            return --length;
        }
    }
}
