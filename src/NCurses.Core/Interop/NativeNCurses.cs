using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.Platform;
using NCurses.Core.Interop.Dynamic;
using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Wrappers;

[assembly: InternalsVisibleTo("NCurses.Core.Interop.Dynamic.Generated")]

namespace NCurses.Core.Interop
{
    public delegate int NCURSES_WINDOW_CB(WindowBaseSafeHandle window, IntPtr args);
    public delegate int NCURSES_SCREEN_CB(IntPtr screen, IntPtr args);

    internal delegate int RipoffDelegateInternal(IntPtr winPtr, int cols);
    public delegate void RipoffDelegate(IWindow window, int columns);

    /// <summary>
    /// native curses methods.
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    public static class NativeNCurses
    {
        #region Native wrappers
        public static INativeNCursesWrapper<
            IMultiByteNCursesChar,
            IMultiByteNCursesCharString,
            IMultiByteChar,
            IMultiByteCharString,
            ISingleByteNCursesChar,
            ISingleByteNCursesCharString,
            ISingleByteChar,
            ISingleByteCharString,
            IMEVENT> NCurses => NCursesCustomTypeWrapper.NCurses;

        public static INativeWindowWrapper<
            IMultiByteNCursesChar,
            IMultiByteNCursesCharString,
            IMultiByteChar,
            IMultiByteCharString,
            ISingleByteNCursesChar,
            ISingleByteNCursesCharString,
            ISingleByteChar,
            ISingleByteCharString,
            IMEVENT> Window => NCursesCustomTypeWrapper.Window;

        public static INativeStdScrWrapper<
            IMultiByteNCursesChar,
            IMultiByteNCursesCharString,
            IMultiByteChar,
            IMultiByteCharString,
            ISingleByteNCursesChar,
            ISingleByteNCursesCharString,
            ISingleByteChar,
            ISingleByteCharString,
            IMEVENT> StdScr => NCursesCustomTypeWrapper.StdScr;

        public static INativeScreenWrapper<
            IMultiByteNCursesChar,
            IMultiByteNCursesCharString,
            IMultiByteChar,
            IMultiByteCharString,
            ISingleByteNCursesChar,
            ISingleByteNCursesCharString,
            ISingleByteChar,
            ISingleByteCharString,
            IMEVENT> Screen => NCursesCustomTypeWrapper.Screen;

        public static INativePadWrapper<
            IMultiByteNCursesChar,
            IMultiByteNCursesCharString,
            IMultiByteChar,
            IMultiByteCharString,
            ISingleByteNCursesChar,
            ISingleByteNCursesCharString,
            ISingleByteChar,
            ISingleByteCharString,
            IMEVENT> Pad => NCursesCustomTypeWrapper.Pad;

        internal static IWindowFactory WindowFactory => NCursesCustomTypeWrapper.WindowFactory;

        //DO NOT add ref, state should be handled by consumer, this is just to verify if NCurses has already been started
        internal static StdScrSafeHandle StdScrSafeHandle { get; set; }
        #endregion

        #region locking
        internal static readonly object SyncRoot = new object();
        internal static bool EnableLocking { get; set; }
        #endregion

        private static bool? hasUnicodeSupport;
        internal static bool HasUnicodeSupport
        {
            get
            {
                lock (SyncRoot)
                {
                    if (hasUnicodeSupport.HasValue)
                    {
                        return hasUnicodeSupport.Value;
                    }

                    NativeLoader.SetLocale();

                    hasUnicodeSupport = NCursesWrapper._nc_unicode_locale();

                    return hasUnicodeSupport.Value;
                }
            }
        }

        #region Platform specific properties
        internal static Encoding Encoding { get; }
        internal static Encoder MultiByteEncoder { get; }
        internal static Decoder MultiByteDecoder { get; }

        internal static Encoding SingleByteEncoding { get; }
        internal static Encoder SingleByteEncoder { get; }
        internal static Decoder SingleByteDecoder { get; }

        internal static INativeLoader NativeLoader { get; }
        internal static INCursesWrapper NCursesWrapper { get; }
        internal static ICustomTypeWrapper NCursesCustomTypeWrapper { get; }
        #endregion

        [ThreadStatic]
        private static byte[] Buffer;

        static NativeNCurses()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Encoding = Encoding.Unicode;
                NativeLoader = new WindowsLoader();
            }
            else
            {
                Encoding = Encoding.UTF32;
                NativeLoader = new LinuxLoader();
            }

            MultiByteDecoder = Encoding.GetDecoder();
            MultiByteEncoder = Encoding.GetEncoder();

            SingleByteEncoding = Encoding.ASCII;
            SingleByteDecoder = SingleByteEncoding.GetDecoder();
            SingleByteEncoder = SingleByteEncoding.GetEncoder();

            NCursesWrapper = (INCursesWrapper)Activator.CreateInstance(DynamicTypeBuilder.CreateDefaultWrapper<INCursesWrapper>(Constants.DLLNAME));

            NCursesCustomTypeWrapper = (ICustomTypeWrapper)Activator
                .CreateInstance(typeof(NativeCustomTypeWrapper<,,,,>)
                .MakeGenericType(
                    DynamicTypeBuilder.cchar_t, 
                    DynamicTypeBuilder.wchar_t,
                    DynamicTypeBuilder.chtype, 
                    DynamicTypeBuilder.schar, 
                    DynamicTypeBuilder.MEVENT));
        }

        #region native thread-safety
        /// <summary>
        /// Execute a thread-safe WINDOW method with verification.
        /// </summary>
        /// <param name="window">A pointer to the window</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        /// <param name="args">A pointer to the arguments</param>
        public static void use_window(WindowBaseSafeHandle window, NCURSES_WINDOW_CB callback, IntPtr args)
        {
            IntPtr func = Marshal.GetFunctionPointerForDelegate(callback);
            try
            {
                NCursesException.Verify(NCursesWrapper.use_window(window, func, args), "use_window");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }

        /// <summary>
        /// Execute a thread-safe SCREEN method.
        /// </summary>
        /// <param name="screen">A pointer to the screen</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        /// <param name="args">A pointer to the arguments</param>
        public static void use_screen(IntPtr screen, NCURSES_SCREEN_CB callback, IntPtr args)
        {
            IntPtr func = Marshal.GetFunctionPointerForDelegate(callback);
            try
            {
                NCursesException.Verify(NCursesWrapper.use_screen(screen, func, args), "use_screen");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }
        #endregion

        #region platform specific property loading
        /// <summary>
        /// load a property from a shared library and execute a method on the property
        /// </summary>
        /// <param name="propertyName">name of the property</param>
        /// <param name="handleProperty">method to excute on the property pointer</param>
        public static void LoadProperty(string propertyName, Action<IntPtr> handleProperty)
        {
            IntPtr propertyPtr, libPtr;

            libPtr = NativeLoader.LoadModule(Constants.DLLNAME);

            if (libPtr == IntPtr.Zero)
                return;
                //throw new ArgumentNullException(string.Format("Couldn't load library {0}", Constants.DLLNAME));

            propertyPtr = NativeLoader.GetSymbolPointer(libPtr, propertyName);

            if (propertyPtr == IntPtr.Zero)
                return;
                //throw new ArgumentNullException(string.Format("Couldn't find symbol {0} in {1}", propertyName, Constants.DLLNAME));

            try
            {
                handleProperty(propertyPtr);
            }
            finally
            {
                NativeLoader.FreeModule(libPtr);
                //if(!NativeLoader.FreeModule(libPtr))
                //throw new ArgumentException(string.Format("Couldn't free {0}", Constants.DLLNAME));
            }
        }
        #endregion

        #region input validation
        /// <summary>
        /// checks if a function key has been pressed
        /// </summary>
        /// <param name="ch">the character you want check</param>
        /// <param name="key">the returned key</param>
        /// <returns>true if a function key has been pressed</returns>
        public static bool HasKey(int ch, out Key key)
        {
            key = 0;
            if (Enum.IsDefined(typeof(Key), (short)ch))
            {
                key = (Key)ch;
                return true;
            }
            return false;
        }

        public static bool IsCtrl(int key, int value)
        {
            return value == CTRL(key);
        }

        public static bool IsCtrlKey(char ch, out char key)
        {
            key = default;
            string keyName = NCurses.keyname(ch).ToString();
            bool isCtrl = keyName[0] == '^';
            if (isCtrl)
            {
                key = keyName[1];
            }
            return isCtrl;
        }

        public static bool IsALT(int key, int value)
        {
            return value == ALT(key);
        }

        //https://github.com/rofl0r/motor/blob/master/kkconsui/include/conscommon.h
        private static int CTRL(int key)
        {
            return (key & 0x1F);
        }

        //https://github.com/rofl0r/motor/blob/master/kkconsui/include/conscommon.h
        private static int ALT(int key)
        {
            return (int)(0x200 | (uint)key);
        }

        internal static bool VerifyInput(WindowBaseSafeHandle windowPtr, string method, int val, out char ch, out Key key)
        {
            return VerifyInput(method, Window.is_keypad(windowPtr), Window.is_nodelay(windowPtr), val, out ch, out key);
        }

        internal static bool VerifyInput(string method, bool keyPadEnabled, bool isNoDelay, int val, out char ch, out Key key)
        {
            key = default;
            ch = default;

            if (isNoDelay 
                && val == Constants.ERR)
            {
                return false;
            }

            NCursesException.Verify(val, method);

            if (keyPadEnabled
                && HasKey(val, out key))
            {
                return true;
            }

            if (val < 0
                && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Encoding encoding = Encoding.GetEncoding(437);

                unsafe
                {
                    byte* bArr = stackalloc byte[1];
                    char* cArr = stackalloc char[1];

                    bArr[0] = (byte)val;

                    encoding.GetChars(bArr, 1, cArr, 1);

                    ch = cArr[0];
                }
            }
            else
            {
                ch = (char)(sbyte)val;
            }

            return false;
        }
        #endregion

        #region COLOR_PAIR
        /// <summary>
        /// Converts a color pair number to an attribute.
        /// Attributes can hold color pairs in the range 0 to 255.
        /// </summary>
        /// <param name="pair">A color pair index</param>
        /// <returns>The attribute of the specified color pair</returns>
        public static int COLOR_PAIR(int pair)
        {
            //Attributes do not support color pairs > 255
            if (pair > 255)
            {
                return 255 << 8;
            }

            return NCursesWrapper.COLOR_PAIR(pair);
        }

        public static int PAIR_NUMBER(uint attrs)
        {
            return NCursesWrapper.PAIR_NUMBER(attrs);
        }
        #endregion

        #region ripoffline
        /// <summary>
        /// The ripoffline routine provides access to the same facility that  slk_init[see  curs_slk(3x)] uses to reduce the
        /// size of the screen.ripoffline must  be called  before
        /// initscr or newterm is called
        /// http://invisible-island.net/ncurses/man/curs_kernel.3x.html
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="line">a positive or negative integer</param>
        /// <param name="init">a method to be called on initscr (a window pointer and number of columns gets passed)</param>
        public static void ripoffline(
            int line,
            RipoffDelegate assignWindowDelegate)
        {
            Func<IntPtr, int, int> initCallback = (IntPtr winPtr, int cols) =>
            {
                NewWindowSafeHandle windowSafeHandle = new NewWindowSafeHandle(winPtr);
                try
                {
                    assignWindowDelegate(WindowFactory.CreateWindow(windowSafeHandle), cols);
                    return Constants.OK;
                }
                catch (Exception)
                {
                    return Constants.ERR;
                }
            };

            /* Needs to be assigned in a non-generic class */
            IntPtr function = Marshal.GetFunctionPointerForDelegate(new RipoffDelegateInternal(initCallback));

            NCursesException.Verify(NCursesWrapper.ripoffline(line, function), "ripoffline");
        }
        #endregion

        #region unsafe helper methods
        //source: https://stackoverflow.com/questions/43289/comparing-two-byte-arrays-in-net
        internal static unsafe bool EqualBytesLongUnrolled(byte* bytes1, byte* bytes2, int length)
        {
            int rem = length % (sizeof(long) * 16);
            long* b1 = (long*)bytes1;
            long* b2 = (long*)bytes2;
            long* e1 = (long*)(bytes1 + length - rem);

            while (b1 < e1)
            {
                if (*(b1) != *(b2) || *(b1 + 1) != *(b2 + 1) ||
                    *(b1 + 2) != *(b2 + 2) || *(b1 + 3) != *(b2 + 3) ||
                    *(b1 + 4) != *(b2 + 4) || *(b1 + 5) != *(b2 + 5) ||
                    *(b1 + 6) != *(b2 + 6) || *(b1 + 7) != *(b2 + 7) ||
                    *(b1 + 8) != *(b2 + 8) || *(b1 + 9) != *(b2 + 9) ||
                    *(b1 + 10) != *(b2 + 10) || *(b1 + 11) != *(b2 + 11) ||
                    *(b1 + 12) != *(b2 + 12) || *(b1 + 13) != *(b2 + 13) ||
                    *(b1 + 14) != *(b2 + 14) || *(b1 + 15) != *(b2 + 15))
                    return false;
                b1 += 16;
                b2 += 16;
            }

            for (int i = 0; i < rem; i++)
            {
                if (bytes1[length - 1 - i] != bytes2[length - 1 - i])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Buffer "pooling"
        internal static byte[] GetBuffer(int byteLength = Constants.MAX_STRING_LENGTH)
        {
            if (Buffer is null)
            {
                Buffer = new byte[Constants.MAX_STRING_LENGTH];
            }

            if (byteLength <= Buffer.Length)
            {
                Array.Clear(Buffer, 0, Buffer.Length);
                return Buffer;
            }

            return new byte[byteLength];
        }
        #endregion
    }
}
