using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop
{
    internal class NCursesException : Exception
    {
        public NCursesException(string message)
            : base(message)
        { }

        public static void Verify(int result, string method)
        {
            if (result == Constants.ERR)
                throw new NCursesException(string.Format("{0} returned ERR", method));
        }

        public static void Verify(IntPtr result, string method)
        {
            if (result == IntPtr.Zero)
                throw new NCursesException(string.Format("{0} returned NULL", method));
        }
    }

    //TODO: create CustomMarshaller to create a null terminated array of NCURSES_CH_T (without size)
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NCURSES_CH_T /* cchar_t */
    {
        public NCURSES_CH_T(char ch)
            : this()
        {
            this.chars = new char[5];
            this.chars[0] = ch;
        }

        public NCURSES_CH_T(char ch, uint attr)
            :this(ch)
        {
            this.attr = attr;
        }

        public NCURSES_CH_T(uint c)
            : this()
        {
            this.chars = new char[5];
            BitConverter.GetBytes(c).CopyTo(this.chars, 0);
        }

        public NCURSES_CH_T(uint ch, uint attr)
            : this(ch)
        {
            this.attr = attr;
        }

        public uint attr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public char[] chars;
        /// <summary>
        /// color pair, must be more than 16-bits
        /// </summary>
        public int ext_color;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MEVENT
    {
        public short id;        /* ID to distinguish multiple devices */
        public int x, y, z;     /* event coordinates (character-cell) */
        public uint bstate;     /* button state bits */
    }

    internal delegate int NCURSES_WINDOW_CB(IntPtr window, IntPtr args);
    internal delegate int NCURSES_SCREEN_CB(IntPtr screen, IntPtr args);

    /// <summary>
    /// native curses methods.
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    internal static class NativeNCurses
    {
        internal static uint[] Acs_Map = new uint[128];
        internal static char[] Wacs_Map = new char[128];

        #region verification
        /// <summary>
        /// Verify any NCurses method returning a Constants.ERR or Constants.OK
        /// </summary>
        /// <param name="f">The NCurses method needing verification wrapped into a Func&lt;int&gt;</param>
        /// <param name="methodName">The name of the NCurses method (needed for better debugging)</param>
        internal static int VerifyNCursesMethod(Func<int> f, string methodName)
        {
            int ret = f();
            NCursesException.Verify(ret, methodName);
            return ret;
        }

        /// <summary>
        /// Verify any NCurses method returning an IntPtr
        /// </summary>
        /// <param name="f">The NCurses method needing verification wrapped into a Func&lt;IntPtr&gt;</param>
        /// <param name="methodName">The name of the NCurses method (needed for better debugging)</param>
        internal static IntPtr VerifyNCursesMethod(Func<IntPtr> f, string methodName)
        {
            IntPtr ptr = f();
            NCursesException.Verify(ptr, methodName);
            return ptr;
        }
        #endregion

        #region thread-safety
        /// <summary>
        /// provides unmanaged thread-safety for WINDOW methods.
        /// </summary>
        /// <param name="window">A pointer to the window</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        /// <param name="args">A pointer to the arguments</param>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_window")]
        internal extern static int ncurses_use_window(IntPtr window, IntPtr callback, IntPtr args);

        /// <summary>
        /// Execute a thread-safe WINDOW method with verification.
        /// </summary>
        /// <param name="window">A pointer to the window</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        /// <param name="args">A pointer to the arguments</param>
        public static int use_window_v(IntPtr window, IntPtr callback, IntPtr args, string method, bool collect = true)
        {
            try
            {
                return VerifyNCursesMethod(() => ncurses_use_window(window, callback, args), method);
            }
            finally
            {
                if (collect)
                {
                    if (callback != IntPtr.Zero)
                        Marshal.FreeHGlobal(callback);
                    if (args != IntPtr.Zero)
                        Marshal.FreeHGlobal(args);
                }
            }
        }

        /// <summary>
        /// Execute a thread-safe WINDOW method with verification.
        /// </summary>
        /// <param name="window">A pointer to the window</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        public static int use_window_v(IntPtr window, IntPtr callback, string method, bool collect = true)
        {
            try
            {
                return VerifyNCursesMethod(() => ncurses_use_window(window, callback, IntPtr.Zero), method);
            }
            finally
            {
                if (collect && callback != IntPtr.Zero)
                    Marshal.FreeHGlobal(callback);
            }
        }

        /// <summary>
        /// Execute a thread-safe WINDOW method with verification.
        /// </summary>
        /// <param name="window">A pointer to the window</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        /// <param name="args">A pointer to the arguments</param>
        public static int use_window(IntPtr window, IntPtr callback, IntPtr args, bool collect = true)
        {
            try
            {
                return ncurses_use_window(window, callback, args);
            }
            finally
            {
                if (collect)
                {
                    if (callback != IntPtr.Zero)
                        Marshal.FreeHGlobal(callback);
                    if (args != IntPtr.Zero)
                        Marshal.FreeHGlobal(args);
                }
            }
        }

        /// <summary>
        /// Execute a thread-safe WINDOW method with verification.
        /// </summary>
        /// <param name="window">A pointer to the window</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        public static int use_window(IntPtr window, IntPtr callback, bool collect = true)
        {
            try
            {
                return ncurses_use_window(window, callback, IntPtr.Zero);
            }
            finally
            {
                if (collect && callback != IntPtr.Zero)
                    Marshal.FreeHGlobal(callback);
            }
        }

        /// <summary>
        /// Execute a thread-safe WINDOW method with verification.
        /// </summary>
        /// <param name="window">A pointer to the window</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        public static int use_window_v(IntPtr window, Func<IntPtr, int> callback)
        {
            Func<IntPtr, IntPtr, int> c = (IntPtr w, IntPtr a) => callback(w);
            return VerifyNCursesMethod(() => ncurses_use_window(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(c)), IntPtr.Zero), "UserCallback");
        }

        /// <summary>
        /// provides unmanaged thread-safety for SCREEN methods.
        /// </summary>
        /// <param name="screen">A pointer to the screen</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_SCREEN_CB</param>
        /// <param name="args">A pointer to the arguments</param>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_screen")]
        internal extern static int ncurses_use_screen(IntPtr screen, IntPtr callback, IntPtr args);

        /// <summary>
        /// Execute a thread-safe SCREEN method with verification.
        /// </summary>
        /// <param name="screen">A pointer to the screen</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        /// <param name="args">A pointer to the arguments</param>
        public static int use_screen_v(IntPtr screen, IntPtr callback, IntPtr args, string method, bool collect = true)
        {
            try
            {
                return VerifyNCursesMethod(() => ncurses_use_screen(screen, callback, args), method);
            }
            finally
            {
                if (collect)
                {
                    if (callback != IntPtr.Zero)
                        Marshal.FreeHGlobal(callback);
                    if (args != IntPtr.Zero)
                        Marshal.FreeHGlobal(args);
                }
            }
        }

        /// <summary>
        /// Execute a thread-safe SCREEN method with verification.
        /// </summary>
        /// <param name="screen">A pointer to the screen</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        public static int use_screen_v(IntPtr screen, IntPtr callback, string method, bool collect = true)
        {
            try
            {
                return VerifyNCursesMethod(() => ncurses_use_screen(screen, callback, IntPtr.Zero), method);
            }
            finally
            {
                if (collect && callback != IntPtr.Zero)
                    Marshal.FreeHGlobal(callback);
            }
        }

        /// <summary>
        /// Execute a thread-safe SCREEN method.
        /// </summary>
        /// <param name="screen">A pointer to the screen</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        /// <param name="args">A pointer to the arguments</param>
        public static int use_screen(IntPtr screen, IntPtr callback, IntPtr args, bool collect = true)
        {
            try
            {
                return ncurses_use_screen(screen, callback, args);
            }
            finally
            {
                if (collect)
                {
                    if (callback != IntPtr.Zero)
                        Marshal.FreeHGlobal(callback);
                    if (args != IntPtr.Zero)
                        Marshal.FreeHGlobal(args);
                }
            }
        }

        /// <summary>
        /// Execute a thread-safe SCREEN method.
        /// </summary>
        /// <param name="screen">A pointer to the screen</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        public static int use_screen(IntPtr screen, IntPtr callback, bool collect = true)
        {
            try
            {
                return ncurses_use_screen(screen, callback, IntPtr.Zero);
            }
            finally
            {
                if (collect && callback != IntPtr.Zero)
                    Marshal.FreeHGlobal(callback);
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
            IntPtr propertyPtr;
            IntPtr libPtr;
            bool freed;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                libPtr = NativeWindows.LoadLibrary(Constants.DLLNAME);
            else
                libPtr = NativeLinux.dlopen(Constants.DLLNAME, 2);

            if (libPtr == IntPtr.Zero)
                throw new ArgumentNullException(string.Format("Couldn't load library {0}", Constants.DLLNAME));

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                propertyPtr = NativeWindows.GetProcAddress(libPtr, propertyName);
            else
                propertyPtr = NativeLinux.dlsym(libPtr, propertyName);

            if (propertyPtr == IntPtr.Zero)
                throw new ArgumentNullException(string.Format("Couldn't find symbol {0} in {1}", propertyName, Constants.DLLNAME));

            try
            {
                handleProperty(propertyPtr);
            }
            finally
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    freed = NativeWindows.FreeLibrary(libPtr);
                else
                {
                    int success = NativeLinux.dlclose(libPtr);
                    freed = success == 0;
                }

                if (!freed)
                    throw new ArgumentException(String.Format("Couldn't free {0}", Constants.DLLNAME));
            }
        }
        #endregion

        #region custom array marshaller
        /// <summary>
        /// create a Null terminated array of a custom time, remember to add to GC and to call Marshal.FreeHGlobal
        /// </summary>
        /// <typeparam name="T">type of the array</typeparam>
        /// <param name="array">array of T to convert</param>
        /// <param name="addNullTerminator">add a null terminator after allocation</param>
        /// <returns></returns>
        internal static IntPtr MarshallArray<T>(T[] array, bool addNullTerminator, out int pointerSize)
        {
            pointerSize = (Marshal.SizeOf<T>() * array.Length) + (addNullTerminator ? Marshal.SizeOf<T>() : 0);
            IntPtr ptr = Marshal.AllocHGlobal(pointerSize);

            try
            {
                for (int i = 0; i < array.Length; i++)
                    Marshal.StructureToPtr(array[i], (ptr + (Marshal.SizeOf<T>() * i)), true);

                if (addNullTerminator)
                    Marshal.Copy(new byte[Marshal.SizeOf<T>()], 0, ptr + (Marshal.SizeOf<T>() * array.Length), Marshal.SizeOf<T>());

                return ptr;
            }
            catch (Exception)
            {
                Marshal.FreeHGlobal(ptr);
                throw;
            }
        }
        #endregion

        #region baudrate
        /// <summary>
        /// returns the output speed of the terminal.
        /// </summary>
        /// <returns>The number returned is in  bits per  second,  for example 9600, and is an integer.</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "baudrate")]
        public extern static int baudrate();
        #endregion

        #region beep
        /// <summary>
        /// sounds an  audible  alarm  on  the terminal,  if  possible;
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "beep")]
        internal extern static int ncurses_beep();

        /// <summary>
        /// sounds an  audible  alarm  on  the terminal,  if  possible;
        /// native method wrapped with verification.
        /// </summary>
        public static void beep()
        {
            VerifyNCursesMethod(() => ncurses_beep(), "beep");
        }
        #endregion

        #region can_change_color
        /// <summary>
        /// check if the  terminal  supports  colors and can change their definitions
        /// </summary>
        /// <returns>returns true if the  terminal  supports  colors and can change their definitions</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "can_change_color")]
        public extern static bool can_change_color();
        #endregion

        #region cbreak
        /// <summary>
        /// see <see cref="cbreak"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "cbreak")]
        internal extern static int ncurses_cbreak();

        /// <summary>
        /// Normally, the tty driver buffers typed characters until a
        /// newline or  carriage return is typed.The cbreak routine
        /// disables line buffering and erase/kill character-processing(interrupt and flow control characters
        /// are unaffected), making characters  typed by  the user  immediately
        /// available  to the  program.The nocbreak routine returns
        /// the terminal to normal(cooked) mode.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void cbreak()
        {
            VerifyNCursesMethod(() => ncurses_cbreak(), "cbreak");
        }
        #endregion

        #region nocbreak
        /// <summary>
        /// see <see cref="nocbreak"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "nocbreak")]
        internal extern static int ncurses_nocbreak();

        /// <summary>
        /// returns the terminal to normal(cooked) mode
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void nocbreak()
        {
            VerifyNCursesMethod(() => ncurses_nocbreak(), "nocbreak");
        }
        #endregion

        #region color_content
        /// <summary>
        /// see <see cref="color_content"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "color_content")]
        internal extern static int ncurses_color_content(short color, IntPtr red, IntPtr green, IntPtr blue);

        /// <summary>
        /// The color_content routine gives programmers a way to find
        /// the intensity of the red, green, and blue(RGB) components
        /// in a color.It requires four arguments: the color number,
        /// and three addresses of shorts for storing the  information
        /// about  the amounts  of red, green, and blue components in
        /// the given color.The first argument must be a legal color
        /// value, i.e.,  0  through COLORS-1, inclusive.The values
        /// that are stored at the addresses pointed to  by the  last
        /// three  arguments are in the range 0 (no component) through
        /// 1000 (maximum amount of component), inclusive.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="color">Color number eg Color.Red</param>
        /// <param name="red">The intensity of red</param>
        /// <param name="green">The intensity fo green</param>
        /// <param name="blue">The intensity of blue</param>
        public static void color_content(short color, ref short red, ref short green, ref short blue)
        {
            IntPtr rPtr = Marshal.AllocHGlobal(Marshal.SizeOf(red));
            Marshal.StructureToPtr(red, rPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(red));

            IntPtr gPtr = Marshal.AllocHGlobal(Marshal.SizeOf(green));
            Marshal.StructureToPtr(green, gPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(green));

            IntPtr bPtr = Marshal.AllocHGlobal(Marshal.SizeOf(blue));
            Marshal.StructureToPtr(blue, bPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(blue));

            try
            {
                VerifyNCursesMethod(() => ncurses_color_content(color, rPtr, gPtr, bPtr), "color_content");
            }
            finally
            {
                Marshal.FreeHGlobal(rPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(red));

                Marshal.FreeHGlobal(gPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(green));

                Marshal.FreeHGlobal(bPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(blue));
            }
        }
        #endregion

        #region COLOR_PAIR
        /// <summary>
        /// Converts a color pair number to an attribute.
        /// Attributes can hold color pairs in the range 0 to 255.
        /// </summary>
        /// <param name="pair">A color pair index</param>
        /// <returns>The attribute of the specified color pair</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "COLOR_PAIR")]
        public extern static int COLOR_PAIR(int pair);
        #endregion

        #region copywin
        /// <summary>
        /// see <see cref="copywin"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "copywin")]
        internal extern static int ncurses_copywin(IntPtr srcwin, IntPtr dstwin, int sminrow, int smincol, int dminrow, int dmincol, int dmaxrow, int dmaxcol, int overlay);

        /// <summary>
        /// Text where the two windows overlap gets copied to destination within the rectangle defined
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="srcwin">A pointer to the source window</param>
        /// <param name="dstwin">A pointer to the destination window</param>
        /// <param name="sminrow">Minimal row to be copied from</param>
        /// <param name="smincol">Minimal column to be copied from</param>
        /// <param name="dminrow">Minimal row to be copied to</param>
        /// <param name="dmincol">Minimal column to be copied to</param>
        /// <param name="dmaxrow">Maximal row to be copied to</param>
        /// <param name="dmaxcol">Maximal column to be copied to</param>
        /// <param name="overlay">1 if copying is non-destructive</param>
        public static void copywin(IntPtr srcwin, IntPtr dstwin, int sminrow, int smincol, int dminrow, int dmincol, int dmaxrow, int dmaxcol, int overlay)
        {
            NativeNCurses.VerifyNCursesMethod(() =>
                ncurses_copywin(srcwin, dstwin, sminrow, smincol, dminrow, dmincol, dmaxrow, dmaxcol, overlay), "copywin");
        }

        /// <summary>
        /// see <see cref="copywin"/>
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        public static void copywin_t(IntPtr srcwin, IntPtr dstwin, int sminrow, int smincol, int dminrow, int dmincol, int dmaxrow, int dmaxcol, int overlay)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) =>
                ncurses_copywin(srcwin, dstwin, sminrow, smincol, dminrow, dmincol, dmaxrow, dmaxcol, overlay);
            NativeNCurses.use_window_v(srcwin, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "copywin");
        }
        #endregion

        #region curs_set
        /// <summary>
        /// see <see cref="curs_set"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "curs_set")]
        internal extern static int ncurses_curs_set(int visibility);

        /// <summary>
        /// sets the cursor visbility
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="visibility">invisible, normal, or very visible for visibility equal to 0, 1, or 2 respectively</param>
        public static void curs_set(int visibility)
        {
            VerifyNCursesMethod(() => ncurses_curs_set(visibility), "curs_set");
        }
        #endregion

        #region def_prog_mode
        /// <summary>
        /// save the
        /// current terminal modes as the  "program"  (in  curses)  or
        /// "shell"   (not  in  curses)  state  for  use by  the re-
        /// set_prog_mode and reset_shell_mode routines.
        /// This is done automatically by initscr.
        /// </summary>
        /// <returns>Always returns Constants.OK</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "def_prog_mode")]
        public extern static int def_prog_mode();
        #endregion

        #region def_shell_mode
        /// <summary>
        /// save the
        /// current terminal modes as the  "program"  (in  curses)  or
        /// "shell"   (not  in  curses)  state  for  use by  the re-
        /// set_prog_mode and reset_shell_mode routines.
        /// This is done automatically by initscr.
        /// </summary>
        /// <returns>Always returns Constants.OK</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "def_shell_mode")]
        public extern static int def_shell_mode();
        #endregion

        #region delay_output
        /// <summary>
        /// The delay_output  routine inserts an <paramref name="ms"/> millisecond pause
        /// in output. This routine should not  be used  extensively
        /// because  padding characters  are used  rather than a CPU
        /// pause.If no padding character is  specified,  this  uses
        /// napms to perform the delay.
        /// </summary>
        /// <param name="ms">the amount of milliseconds to delay the output</param>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "delay_output")]
        internal extern static int ncurses_delay_output(int ms);

        /// <summary>
        /// The delay_output  routine inserts an <paramref name="ms"/> millisecond pause
        /// in output. This routine should not  be used  extensively
        /// because  padding characters  are used  rather than a CPU
        /// pause.If no padding character is  specified,  this  uses
        /// napms to perform the delay.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ms">the amount of milliseconds to delay the output</param>
        public static void delay_output(int ms)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_delay_output(ms), "delay_output");
        }
        #endregion

        #region delscreen
        /// <summary>
        /// The delscreen  routine frees storage associated with the
        /// SCREEN data structure.The endwin  routine does  not  do
        /// this, so delscreen should be called after endwin if a particular SCREEN is no longer needed.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "delscreen")]
        public extern static void delscreen(IntPtr screen);
        #endregion

        #region delwin
        /// <summary>
        /// Calling delwin deletes the named window,
        /// freeing all memory associated with it(it does not actually erase the window's screen image).
        /// Subwindows must  be  deleted  before the main window can be deleted.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "delwin")]
        internal extern static int ncurses_delwin(IntPtr window);

        /// <summary>
        /// Calling delwin deletes the named window,
        /// freeing all memory associated with it(it does not actually erase the window's screen image).
        /// Subwindows must  be  deleted  before the main window can be deleted.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void delwin(IntPtr window)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_delwin(window), "delwin");
        }

        /// <summary>
        /// Calling delwin deletes the named window,
        /// freeing all memory associated with it(it does not actually erase the window's screen image).
        /// Subwindows must  be  deleted  before the main window can be deleted.
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void delwin_t(IntPtr window)
        {
            Func<IntPtr, IntPtr, int> callback = (IntPtr w, IntPtr a) => ncurses_delwin(window);
            NativeNCurses.use_window_v(window, Marshal.GetFunctionPointerForDelegate(new NCURSES_WINDOW_CB(callback)), "delwin");
        }
        #endregion

        #region derwin
        /// <summary>
        /// see <see cref="derwin"/>
        /// </summary>
        /// <returns>A pointer to a new window</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "derwin")]
        internal extern static IntPtr ncurses_derwin(IntPtr window, int nlines, int ncols, int begin_y, int begin_x);

        /// <summary>
        /// Calling derwin is the same as calling subwin, except  that
        /// begin_y and begin_x are relative to the origin of the window orig rather than the screen.There is  no difference
        /// between the subwindows and the derived windows.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">The window to create a subwindow for</param>
        /// <param name="nlines">number of lines of new window</param>
        /// <param name="ncols">number of columns of new window</param>
        /// <param name="begin_y">relative x position to parent</param>
        /// <param name="begin_x">relative y position to parent</param>
        /// <returns>A pointer to a new window</returns>
        public static IntPtr derwin(IntPtr window, int nlines, int ncols, int begin_y, int begin_x)
        {
            return NativeNCurses.VerifyNCursesMethod(() => ncurses_derwin(window, nlines, ncols, begin_y, begin_x), "derwin");
        }
        #endregion

        #region doupdate
        /// <summary>
        /// see <see cref="doupdate"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "doupdate")]
        internal extern static int ncurses_doupdate();

        /// <summary>
        /// update the current screen
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void doupdate()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_doupdate(), "doupdate");
        }
        #endregion

        #region dupwin
        /// <summary>
        /// see <see cref="dupwin"/>
        /// </summary>
        /// <returns>A pointer to a new window</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "dupwin")]
        internal extern static IntPtr ncurses_dupwin(IntPtr window);

        /// <summary>
        /// Calling  dupwin  creates  an exact duplicate of the window <paramref name="window"/>.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to the window to copy</param>
        /// <returns>A pointer to a new window</returns>
        public static IntPtr dupwin(IntPtr window)
        {
            return NativeNCurses.VerifyNCursesMethod(() => dupwin(window), "dupwin");
        }
        #endregion

        #region echo
        /// <summary>
        /// see <see cref="echo"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "echo")]
        internal extern static int ncurses_echo();

        /// <summary>
        /// The echo  and noecho routines control whether characters
        /// typed by the user are echoed  by getch(3x)  as  they are
        /// typed.Echoing by the tty driver is always disabled, but
        /// initially getch is in echo mode, so characters typed  are
        /// echoed.Authors of most interactive programs prefer to do
        /// their own echoing in a controlled area of the screen, or
        /// not  to echo  at all, so they disable echoing by calling
        /// noecho.  [See curs_getch(3x) for a discussion of how these
        /// routines interact with cbreak and nocbreak.]
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void echo()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_echo(), "echo");
        }
        #endregion

        #region endwin
        /// <summary>
        /// see <see cref="endwin"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "endwin")]
        internal extern static int ncurses_endwin();

        /// <summary>
        /// The program must also call endwin for each terminal being
        /// used before exiting from curses. If  newterm  is  called
        /// more  than once for the same terminal, the first terminal
        /// referred to must be the  last one  for  which endwin  is  called.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void endwin()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_endwin(), "endwin");
        }
        #endregion

        #region erasechar
        /// <summary>
        ///  The erasechar  routine returns  the user's current erase character.
        /// The erasewchar routine stores the current erase character
        /// in  the location referenced by ch.If no erase character
        /// has been defined, the routine fails and the location  ref-
        /// erenced by ch is not changed.
        /// </summary>
        /// <returns>The user's current erase character</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "erasechar")]
        public extern static char erasechar();
        #endregion

        #region filter
        /// <summary>
        /// The filter routine, if used, must be called before initscr
        /// or newterm are called.The effect is that, during those
        /// calls, LINES  is  set to 1; the capabilities clear, cup,
        /// cud, cud1, cuu1, cuu,  vpa are  disabled;  and the  home
        /// string is set to the value of cr.
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "filter")]
        public extern static void filter();
        #endregion

        #region flash
        /// <summary>
        /// see <see cref="flash"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "flash")]
        internal extern static int ncurses_flash();

        /// <summary>
        /// The routine flash flashes the screen, and
        /// if  that  is  not possible, sounds the alert.If neither
        /// alert is possible, nothing happens.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void flash()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_flash(), "flash");
        }
        #endregion

        #region flushinp
        /// <summary>
        /// see <see cref="flushinp"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "flushinp")]
        internal extern static int ncurses_flushinp();

        /// <summary>
        /// The flushinp  routine throws away any typeahead that has
        /// been typed by the user and has not yet been read  by the
        /// program.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void flushinp()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_flushinp(), "flushinp");
        }
        #endregion

        /*
         * TODO: getwin (need FILE*)
         * [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
         * public static extern IntPtr fopen(String filename, String mode);
         * [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
         * public static extern Int32 fclose(IntPtr file);
        */

        #region halfdelay
        /// <summary>
        /// see <see cref="halfdelay"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "halfdelay")]
        internal extern static int ncurses_halfdelay(int tenths);

        /// <summary>
        /// The  halfdelay  routine is used for half-delay mode, which
        /// is similar to cbreak mode in that characters typed by  the
        /// user  are immediately available to the program.However,
        /// after blocking for tenths tenths of seconds, ERR  is  re-
        /// turned  if  nothing has  been typed.  The value of tenths
        /// must be a number between 1 and 255.  Use nocbreak to leave
        /// half-delay mode.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="tenths">tenths tenths of seconds between 1 and 255</param>
        public static void halfdelay(int tenths)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_halfdelay(tenths), "halfdelay");
        }
        #endregion

        #region has_colors
        /// <summary>
        /// The has_colors routine requires no arguments.It returns
        /// TRUE if the terminal can manipulate colors; otherwise,  it
        /// returns FALSE.This routine facilitates writing terminal-
        /// independent programs.For example, a programmer  can use
        /// it to decide whether to use color or some other video at-
        /// tribute.
        /// </summary>
        /// <returns>true if the terminal supports colors else false</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "has_colors")]
        public extern static bool has_colors();
        #endregion

        #region has_ic
        /// <summary>
        /// The has_ic routine is true if the terminal has insert- and
        /// delete- character capabilities.
        /// </summary>
        /// <returns>true if terminal supports insert/delete characters</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "has_ic")]
        public extern static bool has_ic();
        #endregion

        #region has_il
        /// <summary>
        /// The has_il routine is true if the terminal has insert- and
        /// delete-line capabilities, or  can simulate  them  using
        /// scrolling regions.This might be used to determine if  it
        /// would  be appropriate to turn on physical scrolling using
        /// scrollok.
        /// </summary>
        /// <returns>true if terminal supports insert/delete characters or can emulate them</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "has_il")]
        public extern static bool has_il();
        #endregion

        #region initscr
        /// <summary>
        /// see <see cref="initscr"/>
        /// </summary>
        /// <returns>returns a pointer to the stdscr on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "initscr")]
        internal extern static IntPtr ncurses_initscr();

        /// <summary>
        /// initscr is normally the first curses routine to call when
        /// initializing a program.A few special routines sometimes
        /// need to be called before it; these are slk_init,  filter,  ripoffline, use_env.
        /// For multiple-terminal applications, newterm may be called before initscr.
        /// This method also initializes Acs_Map, for usage in <see cref="Acs"/> and Wacs_Map, for usage in <see cref="Wacs"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <returns>returns a pointer to the stdscr on success</returns>
        public static IntPtr initscr()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                NativeLinux.setlocale(6, "");

            IntPtr stdScr = NativeNCurses.VerifyNCursesMethod(() => ncurses_initscr(), "initscr");

            try
            {
                Action<IntPtr> loadAcs = (IntPtr acsPtr) =>
                {
                    for (int i = 0; i < Acs_Map.Length; i++)
                        Acs_Map[i] = Marshal.PtrToStructure<uint>(acsPtr + (i * Marshal.SizeOf<uint>()));
                };
                NativeNCurses.LoadProperty("acs_map", loadAcs);
            }
            finally { }

            try
            {
                Action<IntPtr> loadWacs = (IntPtr wacsPtr) =>
                {
                    IntPtr real_wacs = Marshal.ReadIntPtr(wacsPtr);
                    for (int i = 0; i < Wacs_Map.Length; i++)
                        Wacs_Map[i] = (char)Marshal.ReadInt16(real_wacs + (i * Marshal.SizeOf<NCURSES_CH_T>()) + Marshal.SizeOf<uint>());
                };
                NativeNCurses.LoadProperty("_nc_wacs", loadWacs);
            }
            finally { }

            return stdScr;
        }
        #endregion

        #region init_color
        /// <summary>
        /// see <see cref="init_color"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "init_color")]
        internal extern static int ncurses_init_color(short color, short r, short g, short b);

        /// <summary>
        /// The init_color routine changes the definition of a  color.
        /// It takes  four arguments:  the number of the color to be
        /// changed followed by three RGB values(for the amounts  of
        /// red, green, and blue components).  The first argument must
        /// be a legal color value; default  colors are  not allowed
        /// here.   Each of the last three arguments must be a value in
        /// the range  0  through 1000.  When init_color is used, all
        /// occurrences of that color on the screen immediately change
        /// to the new definition.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="color">The number of the color to change (can't be 0-7)</param>
        /// <param name="r">The amount of red in the range 0 through 1000</param>
        /// <param name="g">The amount of green in the range 0 through 1000</param>
        /// <param name="b">The amount of blue in the range 0 through 1000</param>
        public static void init_color(short color, short r, short g, short b)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_init_color(color, r, g, b), "init_color");
        }
        #endregion

        #region init_pair
        /// <summary>
        /// see <see cref="init_pair"/>
        /// </param>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "init_pair")]
        internal extern static int ncurses_init_pair(short pair, short f, short b);

        /// <summary>
        /// The init_pair  routine changes the definition of a color-
        /// pair.It takes three arguments: the number of the  color-
        /// pair to  be changed, the foreground color number, and the
        /// background color number.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="pair">
        /// A legal color  pair  value.
        /// If  default  colors are used(see use_default_colors)
        /// the upper limit is adjusted to allow for  extra pairs
        /// which use  a default color in foreground and/or back-
        /// ground.
        /// </param>
        /// <param name="f">A color value for the foreground</param>
        /// <param name="b">A color value for the background</param>
        public static void init_pair(short pair, short f, short b)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_init_pair(pair, f, b), "init_pair");
        }
        #endregion

        #region intrflush
        /// <summary>
        /// see <see cref="intrflush"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "intrflush")]
        internal extern static int ncurses_intrflush(IntPtr win, bool bf);

        /// <summary>
        /// If the  intrflush option is enabled(bf is TRUE), and an
        /// interrupt key  is  pressed on  the keyboard(interrupt,
        /// break, quit),  all output in the tty driver queue will be
        /// flushed, giving the effect of faster response to the  interrupt, but causing curses to have the wrong idea of what
        /// is on the screen.Disabling the option(bf is FALSE) prevents the flush.The default for the option is inherited
        /// from the tty driver settings.The window argument is  ignored.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="win">ignored</param>
        /// <param name="bf">enbale/disable interrupt</param>
        public static void intrflush(IntPtr win, bool bf)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_intrflush(win, bf), "intrflush");
        }
        #endregion

        #region isendwin
        /// <summary>
        /// The isendwin routine returns  TRUE  if  endwin has  been
        /// called without any subsequent calls to wrefresh, and FALSE
        /// otherwise.
        /// </summary>
        /// <returns>if endwin has been called</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "isendwin")]
        public extern static bool isendwin();
        #endregion

        #region keyname
        /// <summary>
        /// see <see cref="keyname"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "keyname")]
        internal extern static IntPtr ncurses_keyname(int c);

        /// <summary>
        /// The keyname routine returns a character string corresponding to the key <paramref name="c"/>
        /// </summary>
        /// <param name="c">code of the key</param>
        /// <returns>a string representing the key code</returns>
        public static string keyname(int c)
        {
            //using own marshalling, because .net can't free the allocated memory
            IntPtr keyNamePtr = ncurses_keyname(c);
            return Marshal.PtrToStringAnsi(keyNamePtr);
        }
        #endregion

        #region killchar
        /// <summary>
        /// returns the user's current line  kill character.
        /// </summary>
        /// <returns>the kill character</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "killchar")]
        public extern static char killchar();
        #endregion

        #region longname
        /// <summary>
        /// The longname routine returns a pointer to a  static area
        /// containing  a verbose description of the current terminal.
        /// The maximum length of a verbose description is 128 charac-
        /// ters.It  is  defined only after the call to initscr or
        /// newterm.The area is overwritten by each call to newterm
        /// and  is  not restored by set_term, so the value should be
        /// saved between calls to newterm if longname is going to  be
        /// used with multiple terminals.
        /// </summary>
        /// <returns>the current terminal verbose description</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "longname")]
        public extern static string longname();
        #endregion

        #region meta
        /// <summary>
        /// see <see cref="meta"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "meta")]
        internal extern static int ncurses_meta(IntPtr win, bool bf);

        /// <summary>
        /// Initially, whether the terminal returns 7 or 8 significant
        /// bits on input depends on the control mode of the tty driver[see  termio(7)].  To force 8 bits to be returned,
        /// invoke meta(win, TRUE); this is equivalent, under POSIX, to
        /// setting the CS8 flag on the terminal.To force 7 bits to
        /// be returned, invoke meta(win, FALSE); this is  equivalent,
        /// under POSIX, to setting the CS7 flag on the terminal.The
        /// window argument, win, is always ignored.If the terminfo
        /// capabilities smm (meta_on) and rmm(meta_off) are defined
        /// for the terminal, smm  is  sent to  the terminal  when
        /// meta(win, TRUE)  is called and rmm is sent when meta(win,
        /// FALSE) is called.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="win">ignored</param>
        /// <param name="bf">enable/disable</param>
        public static void meta(IntPtr win, bool bf)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_meta(win, bf), "meta");
        }
        #endregion

        #region mvcur
        /// <summary>
        /// see <see cref="mvcur(int, int, int, int)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "mvcur")]
        internal extern static int ncurses_mvcur(int oldrow, int oldcol, int newrow, int newcol);

        /// <summary>
        /// The mvcur routine provides low-level cursor  motion.It
        /// takes  effect immediately(rather than  at the next refresh).
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        /// <param name="oldrow">old line number</param>
        /// <param name="oldcol">old column number</param>
        /// <param name="newrow">new row number</param>
        /// <param name="newcol">new column number</param>
        public static void mvcur(int oldrow, int oldcol, int newrow, int newcol)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_mvcur(oldrow, oldcol, newrow, newcol), "mvcur");
        }
        #endregion

        #region mvderwin
        /// <summary>
        /// see <see cref="mvderwin(IntPtr, int, int)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "mvderwin")]
        internal extern static int ncurses_mvderwin(IntPtr window, int par_y, int par_x);

        /// <summary>
        /// Calling mvderwin moves a derived window(or subwindow) inside its parent window.The screen-relative parameters of
        /// the  window are not changed.  This routine is used to display different parts of the parent  window at  the same
        /// physical position on the screen.
        /// <para />native method wrapped with verification and thread safety.
        /// </summary>
        /// <param name="window">A pointer to the window</param>
        /// <param name="par_y">Line number in the parent window</param>
        /// <param name="par_x">Column number in the parent window</param>
        public static void mvderwin(IntPtr window, int par_y, int par_x)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_mvderwin(window, par_y, par_x), "derwin");
        }
        #endregion

        #region mvwin
        /// <summary>
        /// see <see cref="mvwin(IntPtr, int, int)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "mvwin")]
        internal extern static int ncurses_mvwin(IntPtr win, int y, int x);

        /// <summary>
        /// Calling mvwin moves the window so that the upper left-hand
        /// corner is at position(<paramref name="x"/>, <paramref name="y"/>).  If the move would cause the
        /// window to be off the screen, it is an error and the window
        /// is not moved.Moving subwindows is allowed, but should be
        /// avoided.
        /// </summary>
        /// <param name="win">pointer to a window to move</param>
        /// <param name="y">line number of the new upper left corner</param>
        /// <param name="x">column number of the new upper left corner</param>
        public static void mvwin(IntPtr win, int y, int x)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_mvwin(win, y, x), "mvwin");
        }
        #endregion

        #region napms
        /// <summary>
        /// see <see cref="napms(int)"/>
        /// </summary>
        /// <param name="ms"></param>
        /// <returns>always returns Constants.OK</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "napms")]
        internal extern static int ncurses_napms(int ms);

        /// <summary>
        /// The napms routine is used to sleep for <paramref name="ms"/> milliseconds.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ms">The amount of milliseconds to sleep for</param>
        public static void napms(int ms)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_napms(ms), "napms");
        }
        #endregion

        #region newpad
        /// <summary>
        /// see <see cref="newpad(int, int)"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "newpad")]
        internal extern static IntPtr ncurses_newpad(int nlines, int ncols);

        /// <summary>
        /// The newpad routine creates and returns a pointer to a  new
        /// pad data structure with the given number of lines, nlines,
        /// and columns, ncols.  A pad is like a window,  except that
        /// it is not restricted by the screen size, and is not necessarily associated with a particular part  of the  screen.
        /// Pads can be used when a large window is needed, and only a
        /// part of the window will be on the screen at one time.Automatic refreshes of pads(e.g., from scrolling or echoing
        /// of input) do not occur.It is not legal to call  wrefresh
        /// with  a pad  as  an argument; the routines prefresh or
        /// pnoutrefresh should be called instead.Note that  these
        /// routines require additional parameters to specify the part
        /// of the pad to be displayed and the location on the  screen
        /// to be used for the display.
        /// </summary>
        /// <param name="nlines">number of lines</param>
        /// <param name="ncols">number of columns</param>
        /// <returns>A pointer to a new pad (window) on success</returns>
        public static IntPtr newpad(int nlines, int ncols)
        {
            return NativeNCurses.VerifyNCursesMethod(() => ncurses_newpad(nlines, ncols), "newpad");
        }
        #endregion

        /* TODO
         * SCREEN *newterm(char *type, FILE *outfd, FILE *infd);
         * needs MSVCRT.dll (Microsoft C/C++ Language and Standard Libraries)
        */

        #region newwin
        /// <summary>
        /// see <see cref="newwin(int, int, int, int)"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "newwin")]
        internal extern static IntPtr ncurses_newwin(int nlines, int ncols, int begin_y, int begin_x);

        /// <summary>
        /// Calling newwin creates and returns a pointer to a new window with the given number of lines and columns.The upper
        /// left-hand corner of the window is at line <paramref name="begin_y"/>, column <paramref name="begin_x"/>
        /// If either nlines or ncols is zero, they default to LINES - begin_y and COLS - begin_x.
        /// A new full-screen window   is   created by  calling newwin(0,0,0,0).
        /// </summary>
        /// <param name="nlines">number of lines</param>
        /// <param name="ncols">number of columns</param>
        /// <param name="begin_y">number of lines of upper left corner</param>
        /// <param name="begin_x">number of columns of upper left corner</param>
        /// <returns>A pointer to a new window</returns>
        public static IntPtr newwin(int nlines, int ncols, int begin_y, int begin_x)
        {
            return NativeNCurses.VerifyNCursesMethod(() => ncurses_newwin(nlines, ncols, begin_y, begin_x), "newwin");
        }
        #endregion

        #region nl
        /// <summary>
        /// see <see cref="nl"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "nl")]
        internal extern static int ncurses_nl();

        /// <summary>
        /// The  nl  and  nonl routines control whether the underlying
        /// display device translates the return key into  newline on
        /// input,  and whether it translates newline into return and
        /// line-feed on output(in either case, the call  addch('\n')
        /// does the equivalent of return and line feed on the virtual
        /// screen).  Initially, these translations do occur.If you
        /// disable them using nonl, curses will be able to make better use of the line-feed capability, resulting  in  faster
        /// cursor  motion.Also, curses will then be able to detect
        /// the return key.
        /// </summary>
        public static void nl()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_nl(), "nl");
        }
        #endregion

        #region noecho
        /// <summary>
        /// see <see cref="noecho"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "noecho")]
        internal extern static int ncurses_noecho();

        /// <summary>
        /// see <see cref="echo"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void noecho()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_noecho(), "noecho");
        }
        #endregion

        #region nonl
        /// <summary>
        /// see <see cref="nonl"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "nonl")]
        internal extern static int ncurses_nonl();

        /// <summary>
        /// see <see cref="nl"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void nonl()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_nonl(), "nonl");
        }
        #endregion

        #region noqiflush
        /// <summary>
        /// see <see cref="qiflush"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "noqiflush")]
        internal extern static void noqiflush();
        #endregion

        #region noraw
        /// <summary>
        /// see <see cref="noraw"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "noraw")]
        internal extern static int ncurses_noraw();

        /// <summary>
        /// see <see cref="nl"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void noraw()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_noraw(), "noraw");
        }
        #endregion

        #region overlay
        /// <summary>
        /// see <see cref="overlay(IntPtr, IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "overlay")]
        internal extern static int ncurses_overlay(IntPtr srcWin, IntPtr destWin);

        /// <summary>
        /// The overlay  and overwrite routines overlay srcwin on top
        /// of dstwin.scrwin and dstwin are not required to be  the
        /// same  size;  only text  where the two windows overlap is
        /// copied.The difference is that overlay is non-destructive
        /// (blanks are not copied) whereas overwrite is destructive.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="srcWin">pointer to the source window</param>
        /// <param name="destWin">pointer to the destination window</param>
        public static void overlay(IntPtr srcWin, IntPtr destWin)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_overlay(srcWin, destWin), "overlay");
        }
        #endregion

        #region overwrite
        /// <summary>
        /// see <see cref="overwrite(IntPtr, IntPtr)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "overwrite")]
        internal extern static int ncurses_overwrite(IntPtr srcWin, IntPtr destWin);

        /// <summary>
        /// see <see cref="overlay(IntPtr, IntPtr)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void overwrite(IntPtr srcWin, IntPtr destWin)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_overwrite(srcWin, destWin), "overwrite");
        }
        #endregion

        #region pair_content
        /// <summary>
        /// see <see cref="pair_content(short, ref short, ref short)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "pair_content")]
        internal extern static int ncurses_pair_content(short pair, out short fg, out short bg);

        /// <summary>
        /// The pair_content routine allows programmers to  find  out
        /// what colors  a given color-pair consists of.It requires
        /// three arguments: the color-pair number, and two addresses
        /// of shorts  for  storing the foreground and the background
        /// color numbers.The first argument must be a legal  color
        /// value, i.e., in the range 1 through COLOR_PAIRS-1, inclusive.The values that are stored at the addresses pointed
        /// to  by the  second and third arguments are in the range 0
        /// through COLORS, inclusive.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="pair">the number of the pair you want to know the content of</param>
        /// <param name="fg">a reference to the foreground color</param>
        /// <param name="bg">a reference to the background color</param>
        public static void pair_content(short pair, out short fg, out short bg)
        {
            NCursesException.Verify(ncurses_pair_content(pair, out fg, out bg), "pair_content");
        }
        #endregion

        #region PAIR_NUMBER
        /// <summary>
        /// PAIR_NUMBER(attrs) extracts the color value from its attrs
        /// parameter  and returns it as a color pair number.
        /// Its inverse COLOR_PAIR(n) converts a color pair number to an attribute.Attributes can hold color pairs in the range 0
        /// to 255.  If you need a color pair larger than  that,  you
        /// must  use functions such as attr_set(which pass the color
        /// pair as a separate parameter) rather than the legacy functions such as attrset.
        /// </summary>
        /// <returns>The color defined in the attribute</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "PAIR_NUMBER")]
        public extern static int PAIR_NUMBER(uint attrs);
        #endregion

        /* TODO
         * int putwin(WINDOW *win, FILE *filep);
        */

        #region qiflush
        /// <summary>
        /// When the noqiflush routine is used, normal flush of input
        /// and output queues associated with the INTR, QUIT and SUSP
        /// characters will not be done[see termio(7)].  When qiflush
        /// is  called,  the queues will be flushed when these control
        /// characters are read.You may want to call noqiflush in  a
        /// signal  handler  if  you want output to continue as though
        /// the interrupt had not occurred, after the handler exits.
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "qiflush")]
        internal extern static void qiflush();
        #endregion

        #region raw
        /// <summary>
        /// see <see cref="raw"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "raw")]
        internal extern static int ncurses_raw();

        /// <summary>
        /// The raw and noraw routines place the terminal into or out
        /// of raw mode.Raw mode is similar to cbreak mode, in  that
        /// characters typed are immediately passed through to the user program.The differences are that in raw mode,
        /// the interrupt, quit, suspend, and flow control characters are
        /// all passed through uninterpreted, instead of generating a
        /// signal.The behavior  of the BREAK key depends on other
        /// bits in the tty driver that are not set by curses.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void raw()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_raw(), "raw");
        }
        #endregion

        #region resetty
        /// <summary>
        /// see <see cref="resetty"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "resetty")]
        internal extern static int ncurses_resetty();

        /// <summary>
        /// The resetty and savetty  routines save  and restore  the
        /// state  of the  terminal modes.savetty saves the current
        /// state in a buffer and resetty restores the state to  what
        /// it was at the last call to savetty.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void resetty()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_resetty(), "resetty");
        }
        #endregion

        #region reset_prog_mode
        /// <summary>
        /// see <see cref="reset_prog_mode"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "reset_prog_mode")]
        internal extern static int ncurses_reset_prog_mode();

        /// <summary>
        /// The reset_prog_mode and reset_shell_mode routines restore
        /// the terminal  to "program" (in curses) or "shell" (out of
        /// curses) state.These are done automatically by endwin(3x)
        /// and,  after an  endwin, by doupdate, so they normally are
        /// not called.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void reset_prog_mode()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_reset_prog_mode(), "reset_prog_mode");
        }
        #endregion

        #region reset_shell_mode
        /// <summary>
        /// see <see cref="reset_shell_mode"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "reset_shell_mode")]
        internal extern static int ncurses_reset_shell_mode();

        /// <summary>
        /// see <see cref="reset_prog_mode"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void reset_shell_mode()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_reset_prog_mode(), "reset_shell_mode");
        }
        #endregion

        #region ripoffline
        /// <summary>
        /// see <see cref="ripoffline"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "ripoffline")]
        internal extern static int ncurses_ripoffline(int line, IntPtr method);

        private delegate int ripoffDelegate(IntPtr win, int cols);

        /// <summary>
        /// The ripoffline routine provides access to the same facility that  slk_init[see  curs_slk(3x)] uses to reduce the
        /// size of the screen.ripoffline must  be called  before
        /// initscr or newterm is called
        /// http://invisible-island.net/ncurses/man/curs_kernel.3x.html
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="line">a positive or negative integer</param>
        /// <param name="init">a method to be called on initscr (a window pointer and number of columns gets passed)</param>
        public static void ripoffline(int line, Func<IntPtr, int, IntPtr, int> init)
        {
            IntPtr func = IntPtr.Zero;
            Func<IntPtr, int, int> initCallback = (IntPtr win, int cols) => init(win, cols, func);
            func = Marshal.GetFunctionPointerForDelegate(new ripoffDelegate(initCallback));

            NativeNCurses.VerifyNCursesMethod(() => ncurses_ripoffline(line, func), "ripoffline");
        }
        #endregion

        #region savetty
        /// <summary>
        /// see <see cref="savetty"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "savetty")]
        internal extern static int ncurses_savetty();

        /// <summary>
        /// see <see cref="resetty"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void savetty()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_resetty(), "savetty");
        }
        #endregion

        #region scr_dump
        /// <summary>
        /// see <see cref="scr_dump(string)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "scr_dump")]
        internal extern static int ncurses_scr_dump(string filename);

        /// <summary>
        /// see <see cref="src_init(string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void scr_dump(string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_scr_dump(filename), "scr_dump");
        }
        #endregion

        #region scr_init
        /// <summary>
        /// see <see cref="scr_init(string)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "scr_init")]
        internal extern static int ncurses_scr_init(string filename);

        /// <summary>
        /// The scr_init routine reads in the contents of filename and
        /// uses  them to initialize the curses data structures about
        /// what the terminal currently has on its screen.If the data is determined to be valid, curses bases its next update
        /// of the screen on this information rather than clearing the
        /// screen  and starting from scratch.  scr_init is used after
        /// initscr or a system call to share the screen with  another
        /// process  which has  done a scr_dump after its endwin(3x)
        /// call.The data is declared invalid if the terminfo  capabilities rmcup  and nrrmc exist; also if the terminal has
        /// been written to since the preceding scr_dump call.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void scr_init(string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_scr_init(filename), "scr_init");
        }
        #endregion


        #region scr_restore
        /// <summary>
        /// see <see cref="scr_restore(string)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "scr_restore")]
        internal extern static int ncurses_scr_restore(string filename);

        /// <summary>
        /// The scr_restore  routine sets  the virtual screen to the
        /// contents of filename, which must have been written  using
        /// scr_dump.   The  next call to doupdate restores the screen
        /// to the way it looked in the dump file.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void scr_restore(string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_scr_restore(filename), "scr_restore");
        }
        #endregion

        #region scr_set
        /// <summary>
        /// see <see cref="src_set"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "scr_set")]
        internal extern static int ncurses_scr_set(string filename);

        /// <summary>
        /// The scr_set routine is a combination  of scr_restore  and
        /// scr_init.It  tells the program that the information in
        /// filename is what is currently on the screen, and also what
        /// the  program wants on the screen.This can be thought of
        /// as a screen inheritance function.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void scr_set(string filename)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_scr_set(filename), "scr_set");
        }
        #endregion

        #region set_term
        /// <summary>
        /// see <see cref="set_term(IntPtr)"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "set_term")]
        internal extern static IntPtr ncurses_set_term(IntPtr newScr);

        /// <summary>
        /// The set_term routine is used to switch  between different
        /// terminals.The screen reference new becomes the new current terminal.The previous terminal is returned by  the
        /// routine.   This  is  the only  routine which manipulates
        /// SCREEN pointers; all other routines affect only the  cur-
        /// rent terminal.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <returns>pointer to the previous screen</returns>
        public static IntPtr set_term(IntPtr newScr)
        {
            return NativeNCurses.VerifyNCursesMethod(() => ncurses_set_term(newScr), "set_term");
        }
        #endregion

        #region slk_attroff
        /// <summary>
        /// see <see cref="slk_attroff(uint)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attroff")]
        internal extern static int ncurses_slk_attroff(uint attrs);

        /// <summary>
        /// The slk_attron, slk_attrset, slk_attroff and slk_attr routines correspond to
        /// <see cref="NativeStdScr.attron(int)"/> , <see cref="NativeStdScr.attrset(int)"/>, <see cref="NativeStdScr.attroff(int)"/>
        /// and <see cref="NativeStdScr.attr_get(IntPtr, IntPtr)"/>.
        /// They  have an effect only if soft labels are simulated on
        /// the bottom line of the screen.The default highlight  for
        /// soft keys is A_STANDOUT (as in System V curses, which does
        /// not document this fact).
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attr"></param>
        public static void slk_attroff(uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attroff(attrs), "slk_attroff");
        }
        #endregion

        #region slk_attr_off
        /// <summary>
        /// see <see cref="slk_attr_off(uint)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attr_off")]
        internal extern static int ncurses_slk_attr_off(uint attrs, IntPtr opts);

        /// <summary>
        /// see <see cref="NativeStdScr.attr_off(uint)"/>  (for soft function keys)
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_attr_off(uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attr_off(attrs, IntPtr.Zero), "slk_attr_off");
        }
        #endregion

        #region slk_attron
        /// <summary>
        /// see <see cref="slk_attron(uint)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attron")]
        internal extern static int ncurses_slk_attron(uint attrs);

        /// <summary>
        /// see <see cref="slk_attroff(uint)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attr"></param>
        public static void slk_attron(uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attron(attrs), "slk_attron");
        }
        #endregion

        #region slk_attr_on
        /// <summary>
        /// see <see cref="slk_attr_on(uint)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attr_on")]
        internal extern static int ncurses_slk_attr_on(uint attrs, IntPtr opts);

        /// <summary>
        /// see <see cref="NativeStdScr.attr_on(uint)"/> (for soft function keys)
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_attr_on(uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attr_on(attrs, IntPtr.Zero), "slk_attr_on");
        }
        #endregion

        #region slk_attrset
        /// <summary>
        /// see <see cref="slk_attrset(uint, short)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attrset")]
        internal extern static int ncurses_slk_attrset(uint attrs, short color_pair, IntPtr opts);

        /// <summary>
        /// see <see cref="slk_attroff(uint)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attr"></param>
        public static void slk_attrset(uint attrs, short color_pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attrset(attrs, color_pair, IntPtr.Zero), "slk_attrset");
        }
        #endregion

        #region slk_attr
        /// <summary>
        /// returns the the attribute used for the soft keys
        /// </summary>
        /// <returns>an attribute</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attr")]
        public extern static uint slk_attr();
        #endregion

        #region slk_attr_set
        /// <summary>
        /// see <see cref="slk_attr_set(uint, short)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_attr_set")]
        internal extern static int ncurses_slk_attr_set(uint attrs,  short color_pair, IntPtr opts);

        /// <summary>
        /// see <see cref="NativeStdScr.attr_set(uint, short)"/> (for soft function keys)
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_attr_set(uint attrs, short color_pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_attr_set(attrs, color_pair, IntPtr.Zero), "slk_attr_set");
        }
        #endregion

        #region slk_clear
        /// <summary>
        /// see <see cref="slk_clear"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_clear")]
        internal extern static int ncurses_slk_clear();

        /// <summary>
        /// The slk_clear routine clears  the soft  labels from  the screen.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_clear()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_clear(), "slk_clear");
        }
        #endregion

        #region slk_color
        /// <summary>
        /// see <see cref="slk_color(short)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_color")]
        internal extern static int ncurses_slk_color(short color_pair);

        /// <summary>
        /// The slk_color routine corresponds to color_set.It has an
        /// effect only  if  soft labels are simulated on the bottom
        /// line of the screen.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_color(short color_pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_color(color_pair), "slk_color");
        }
        #endregion

        #region slk_init
        /// <summary>
        /// see <see cref="slk_init(int)"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_init")]
        internal extern static int ncurses_slk_init(int fmt);

        /// <summary>
        /// The slk_init  routine must  be called before initscr or
        /// newterm is called.If initscr eventually uses a line from
        /// stdscr to emulate the soft labels, then fmt determines how
        /// the labels are arranged on the screen:
        /// <para />0  indicates a 3-2-3 arrangement of the labels.
        /// <para />1  indicates a 4-4 arrangement
        /// <para />2  indicates the PC-like 4-4-4 mode.
        /// <para />3  is again the PC-like 4-4-4 mode, but in addition
        ///   an  index line is generated, helping the user to
        ///   identify the key numbers easily.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="fmt">the format to init the soft labels with</param>
        public static void slk_init(int fmt)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_init(fmt), "slk_init");
        }
        #endregion

        #region slk_label
        /// <summary>
        /// The  slk_label routine returns the current label for label
        /// number <paramref name="labnum"/>, with leading and trailing blanks stripped.
        /// </summary>
        /// <param name="labnum">number of the label for which you want to return the label</param>
        /// <returns>label</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_label")]
        public extern static string slk_label(int labnum);
        #endregion

        #region slk_noutrefresh
        /// <summary>
        /// see <see cref="slk_noutrefresh"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_noutrefresh")]
        internal extern static int ncurses_slk_noutrefresh();

        /// <summary>
        /// The slk_refresh and slk_noutrefresh routines correspond to
        /// the <see cref="NativeWindow.wrefresh(IntPtr)"/>  and <see cref="NativeWindow.wnoutrefresh"/>  routines.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_noutrefresh()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_noutrefresh(), "slk_noutrefresh");
        }
        #endregion

        #region slk_refresh
        /// <summary>
        /// see <see cref="slk_refresh"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_refresh")]
        internal extern static int ncurses_slk_refresh();

        /// <summary>
        /// see <see cref="slk_noutrefresh"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_refresh()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_refresh(), "slk_refresh");
        }
        #endregion

        #region slk_restore
        /// <summary>
        /// see <see cref="slk_restore"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_restore")]
        internal extern static int ncurses_slk_restore();

        /// <summary>
        /// The slk_restore  routine restores the soft labels to the
        /// screen after a slk_clear has been performed.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_restore()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_restore(), "slk_restore");
        }
        #endregion

        #region slk_set
        /// <summary>
        /// see <see cref="slk_set"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_set")]
        internal extern static int ncurses_slk_set(int labnum, string label, int fmt);

        /// <summary>
        /// label the soft keys
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="labnum">is  the  label number, from 1 to 8 (12 for fmt in slk_init is 2 or 3)</param>
        /// <param name="label">is be the string to put on the  label,  up  to
        /// eight(five  for  fmt  in slk_init is 2 or 3)
        /// characters in length.A null string or a null
        /// pointer sets up a blank label</param>
        /// <param name="fmt">is  either  0, 1, or 2, indicating whether the
        /// label is to be  left-justified,  centered,  or
        /// right-justified,  respectively, within the label.</param>
        public static void slk_set(int labnum, string label, int fmt)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_set(labnum, label, fmt), "slk_set");
        }
        #endregion

        #region slk_touch
        /// <summary>
        /// see <see cref="slk_touch"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_touch")]
        internal extern static int ncurses_slk_touch();

        /// <summary>
        /// The slk_touch routine forces all the  soft labels  to be
        /// output the next time a slk_noutrefresh is performed.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_touch()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_touch(), "slk_touch");
        }
        #endregion

        #region start_color
        /// <summary>
        /// see <see cref="start_color"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "start_color")]
        internal extern static int ncurses_start_color();

        /// <summary>
        /// The start_color routine requires no arguments.It must be
        /// called if the programmer wants to use colors, and before
        /// any other  color manipulation  routine is called.It is
        /// good practice to call this routine right  after <see cref="initscr"/> .
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void start_color()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_start_color(), "start_color");
        }
        #endregion

        #region subpad
        /// <summary>
        /// see <see cref="subpad"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "subpad")]
        internal extern static IntPtr ncurses_subpad(IntPtr orig, int nlines, int ncols, int begin_y, int begin_x);

        /// <summary>
        /// The subpad routine creates and returns a pointer to a subwindow within a pad  with the  given number  of lines,
        /// <paramref name="nlines"/>, and  <paramref name="ncols"/>,  ncols.Unlike subwin, which uses
        /// screen coordinates, the window is  at position(begin_x,
        /// begin_y)  on the pad.The window is made in the middle of
        /// the window orig, so that changes made to one window affect
        /// both windows.During the use of this routine,
        /// it will often be necessary to call touchwin or touchline on orig before calling prefresh.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="orig">the pad for which you want to create a subpad</param>
        /// <param name="nlines">number of lines for the new pad</param>
        /// <param name="ncols">number of columns for the new pad</param>
        /// <param name="begin_y">the y position of the window on the pad</param>
        /// <param name="begin_x">the x positoni of the window on the pad</param>
        /// <returns>a pointer to the new pad (window)</returns>
        public static IntPtr subpad(IntPtr orig, int nlines, int ncols, int begin_y, int begin_x)
        {
            return NativeNCurses.VerifyNCursesMethod(() => ncurses_subpad(orig, nlines, ncols, begin_y, begin_x), "subpad");
        }
        #endregion

        #region subwin
        /// <summary>
        /// see <see cref="subwin"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "subpad")]
        internal extern static IntPtr ncurses_subwin(IntPtr orig, int nlines, int ncols, int begin_y, int begin_x);

        /// <summary>
        /// Calling subwin creates and returns a pointer to a new window with the given number of lines, <paramref name="nlines"/>,  and columns,
        /// <paramref name="ncols"/>.   The window  is at position(<paramref name="begin_y"/>, <paramref name="begin_x"/>) on
        /// the screen.The subwindow shares memory with the  window
        /// orig, so that changes made to one window will affect both
        /// windows.When using this routine, it is necessary to call
        /// touchwin  or touchline on orig before calling wrefresh on
        /// the subwindow.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="orig">the window for which you want to create a subwindow</param>
        /// <param name="nlines">number of lines for the new window</param>
        /// <param name="ncols">nubmer of columns for the new window</param>
        /// <param name="begin_y">line number where the new subwindow should start on the screen</param>
        /// <param name="begin_x">column number where the new subwindow should start on the screen</param>
        /// <returns>a pointer to the new window</returns>
        public static IntPtr subwin(IntPtr orig, int nlines, int ncols, int begin_y, int begin_x)
        {
            return NativeNCurses.VerifyNCursesMethod(() => ncurses_subpad(orig, nlines, ncols, begin_y, begin_x), "subpad");
        }
        #endregion

        #region termattrs
        /// <summary>
        /// If a  given terminal  does not support a video attribute
        /// that an application program is trying to use,  curses may
        /// substitute a  different video attribute for it.The ter-
        /// mattrs and term_attrs functions return a logical OR of all
        /// video  attributes supported  by the terminal using A_ and
        /// WA_ constants respectively.This  information  is  useful
        /// when  a curses  program needs  complete control over the
        /// appearance of the screen.
        /// </summary>
        /// <returns>the supported attributes OR'd together</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "termattrs")]
        public extern static uint termattrs();
        #endregion

        #region termname
        /// <summary>
        /// The termname routine returns the terminal  name used  by setupterm.
        /// </summary>
        /// <returns>the terminal name</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "termname")]
        public extern static string termname();
        #endregion

        #region typeahead
        /// <summary>
        /// see <see cref="typeahead"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "typeahead")]
        internal extern static int ncurses_typeahead(int fd);

        /// <summary>
        /// The curses  library does "line-breakout optimization" by
        /// looking for  typeahead periodically  while  updating the
        /// screen.If input is found, and it is coming from a tty,
        /// the current update is postponed until refresh(3x) or doup-
        /// date is called again.This allows faster response to com-
        /// mands typed in advance.Normally, the input FILE pointer
        /// passed to  newterm, or stdin in the case that initscr was
        /// used, will be used to do this typeahead checking.  The ty-
        /// peahead routine  specifies that the file descriptor fd is
        /// to be used to check for typeahead instead.  If fd  is  -1,
        /// then no typeahead checking is done.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="fd">file descriptor to use for typeahead</param>
        public static void typeahead(int fd)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_typeahead(fd), "typeahead");
        }
        #endregion

        #region ungetch
        /// <summary>
        /// see <see cref="ungetch"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "ungetch")]
        internal extern static int ncurses_ungetch(int ch);

        /// <summary>
        /// The ungetch routine places ch back onto the input queue to
        /// be returned by the next call to wgetch.There is just one
        /// input queue for all windows.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ch">character to place into the input queue</param>
        public static void ungetch(int ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_ungetch(ch), "ungetch");
        }
        #endregion

        #region use_env
        /// <summary>
        /// The  use_env  routine,  if  used,  should be called before
        /// initscr or newterm are called(because those  compute the
        /// screen size).  It modifies the way ncurses treats environ-
        /// ment variables when determining the screen size.
        /// </summary>
        /// <param name="f">enable/disable</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_env")]
        public extern static void use_env(bool f);
        #endregion

        #region use_tioctl
        /// <summary>
        /// The use_tioctl  routine, if used, should be called before
        /// initscr or newterm are called(because those  compute the
        /// screen size).  After use_tioctl is called with TRUE as an
        /// argument, ncurses modifies the last step in  its computation of screen size as follows:
        /// <para>checks  if the LINES and COLUMNS environment variables are set to a number greater than zero.</para>
        /// <para>for each, ncurses updates the corresponding  environment variable with the value that it has obtained via
        /// operating system call or from the terminal database.</para>
        /// <para>ncurses re-fetches the value of the environment  variables so  that it is still the environment variables
        /// which set the screen size.</para>
        /// </summary>
        /// <param name="f">enable/disable</param>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_tioctl")]
        public extern static void use_tioctl(bool f);
        #endregion

        #region vidattr
        /// <summary>
        /// see <see cref="vidattr"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "vidattr")]
        internal extern static int ncurses_vidattr(uint attrs);

        /// <summary>
        /// The  vidattr  routine  is like the vidputs routine, except that it outputs through putchar.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attributes to show</param>
        public static void vidattr(uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_vidattr(attrs), "vidattr");
        }
        #endregion

        #region vidputs
        /// <summary>
        /// see <see cref="vidputs"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "vidputs")]
        internal extern static int ncurses_vidputs(uint attrs, IntPtr NCURSES_OUTC);

        /// <summary>
        /// The vidputs routine displays the string on the terminal in
        /// the video  attribute mode attrs, which is any combination
        /// of the attributes listed in  curses(3x).   The characters
        /// are passed to the putchar-like routine putc.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attributes to show</param>
        public static void vidputs(uint attrs, Func<int, int> NCURSES_OUTC)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_vidputs(attrs, Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC)), "vidputs");
        }
        #endregion

        #region tigetflag
        /// <summary>
        /// The tigetflag, tigetnum and tigetstr routines  return  the
        /// value of the capability corresponding to the terminfo cap-
        /// name passed to them, such as xenl.The capname  for  each
        /// capability  is  given in the table column entitled capname
        /// code in the capabilities section of terminfo(5).
        /// </summary>
        /// <param name="capname">name of the capability</param>
        /// <returns>
        /// <para>-1     if capname is not a boolean capability</para>
        /// <para>0      if it is canceled or absent from the terminal  description.</para>
        /// </returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "tigetflag")]
        public extern static int tigetflag(string capname);
        #endregion

        #region tigetnum
        /// <summary>
        /// see <see cref="tigetflag"/>
        /// </summary>
        /// <param name="capname">name of the capability</param>
        /// <returns>
        /// <para>-2     if capname is not a numeric capability</para>
        /// <para>-1     if  it  is canceled or absent from the terminal description.</para>
        /// </returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "tigetnum")]
        public extern static int tigetnum(string capname);
        #endregion

        #region tigetstr
        /// <summary>
        /// see <see cref="tigetflag"/>
        /// </summary>
        /// <param name="capname">name of the capability</param>
        /// <returns>
        /// <para>(char*)-1              if capname is not a string capability</para>
        /// <para>0      if it is canceled or absent from the terminal  description.</para>
        /// </returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "tigetstr")]
        public extern static string tigetstr(string capname);
        #endregion

        #region putp
        /// <summary>
        /// see <see cref="putp"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "putp")]
        internal extern static int ncurses_putp(string str);

        /// <summary>
        /// The putp routine calls tputs(str, 1, putchar).  Note that
        /// the output  of putp  always goes  to stdout, not to the
        /// fildes specified in setupterm.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attributes to show</param>
        public static void putp(string str)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_putp(str), "putp");
        }
        #endregion

        #region is_term_resized
        /// <summary>
        /// A support  function is_term_resized  is provided so that
        /// applications can check if the resize_term  function would
        /// modify the window structures.It returns TRUE if the win-
        /// dows would be modified, and FALSE otherwise.
        /// </summary>
        /// <param name="lines">new number of lines</param>
        /// <param name="columns">new number of columns</param>
        /// <returns>if term has been resized</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "is_term_resized")]
        internal extern static bool is_term_resized(int lines, int columns);
        #endregion

        #region keybound
        /// <summary>
        /// This is an extension to the curses library.It permits an
        /// application to determine the string which  is  defined  in
        /// the terminfo for specific keycodes.
        /// </summary>
        /// <param name="keycode">keycode to check</param>
        /// <param name="count">iterate for</param>
        /// <returns>the name of the keycode</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "keybound")]
        public extern static string keybound(int keycode, int count);
        #endregion

        #region curses_version
        /// <summary>
        /// Use  curses_version  to  get the version number, including  patch level of the library, e.g., 5.0.19991023
        /// </summary>
        /// <returns>version string</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "curses_version")]
        internal extern static string curses_version();
        #endregion

        #region assume_default_colors
        /// <summary>
        /// see <see cref="assume_default_colors"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "assume_default_colors")]
        internal extern static int ncurses_assume_default_colors(int fg, int bg);

        /// <summary>
        /// The other, assume_default_colors  is  a refinement which
        /// tells which colors to paint for color pair 0.  This function recognizes  a special color number -1, which denotes
        /// the default terminal color.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="fg">the default foreground color</param>
        /// <param name="bg">the default background color</param>
        public static void assume_default_colors(int fg, int bg)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_assume_default_colors(fg, bg), "assume_default_colors");
        }
        #endregion

        #region define_key
        /// <summary>
        /// see <see cref="define_key"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "define_key")]
        internal extern static int ncurses_define_key(string definition, int keycode);

        /// <summary>
        /// This is an extension to the curses library.It permits an
        /// application to define keycodes  with their  corresponding
        /// control  strings, so that the ncurses library will interpret them just as it would the predefined  codes  in  the
        /// terminfo database.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void define_key(string definition, int keycode)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_define_key(definition, keycode), "define_key");
        }
        #endregion

        #region get_escdelay
        /// <summary>
        /// The get_escdelay function returns the value for ESCDELAY.
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "get_escdelay")]
        public extern static int get_escdelay();
        #endregion

        #region key_defined
        /// <summary>
        /// This is an extension to the curses library.It permits an
        /// application to determine if a string is currently bound to
        /// any keycode.
        /// </summary>
        /// <returns>
        /// If  the  string  is bound to a keycode, its value (greater than zero) is returned.If no keycode is bound, zero is returned.
        /// If  the  string  conflicts with longer strings which are bound to keys, -1 is returned
        /// </returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "key_defined")]
        public extern static int key_defined(string definition);
        #endregion

        #region keyok
        /// <summary>
        /// see <see cref="keyok"/>
        /// </summary>
        /// <returns>Constants.ERR on error</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "keyok")]
        internal extern static int ncurses_keyok(int keycode, bool enable);

        /// <summary>
        /// This is an extension to the curses library.It permits an
        /// application to disable specific keycodes, rather than  use
        /// the  keypad function  to disable all keycodes.  Keys that
        /// have been disabled can be re-enabled.
        /// </summary>
        /// <param name="keycode">keycode to enable/disable</param>
        /// <param name="enable">enable/disable</param>
        public static void keyok(int keycode, bool enable)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_keyok(keycode, enable), "keyok");
        }
        #endregion

        #region resize_term
        /// <summary>
        /// see <see cref="resize_term"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "resize_term")]
        internal extern static int ncurses_resize_term(int lines, int columns);

        /// <summary>
        /// Most of   the work  is  done by  the inner  function
        /// resize_term.The outer function resizeterm adds bookkeeping  for the SIGWINCH handler.When resizing the windows,
        /// resize_term blank-fills the areas that are extended.   The
        /// calling application should fill in these areas with appropriate data.  The resize_term function attempts to resize
        /// all windows.   However, due to the calling convention of
        /// pads, it is not possible to resize  these without  additional interaction with the application.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void resize_term(int lines, int columns)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_resize_term(lines, columns), "resize_term");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NativeWindows.NativeWindowsConsoleResize(lines, columns);
        }
        #endregion

        #region resizeterm
        /// <summary>
        /// see <see cref="resizeterm"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "resizeterm")]
        internal extern static int ncurses_resizeterm(int lines, int columns);

        /// <summary>
        /// The function  resizeterm resizes the standard and current
        /// windows to the specified  dimensions,  and adjusts  other
        /// bookkeeping  data used by the ncurses library that record
        /// the window dimensions such as the LINES  and COLS  variables.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void resizeterm(int lines, int columns)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_resizeterm(lines, columns), "resizeterm");
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                NativeWindows.NativeWindowsConsoleResize(lines, columns);
        }
        #endregion

        #region set_escdelay
        /// <summary>
        /// see <see cref="set_escdelay"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "set_escdelay")]
        internal extern static int ncurses_set_escdelay(int size);

        /// <summary>
        /// The ESCDELAY and TABSIZE global variables are modified by
        /// some applications.To modify them in  any configuration,
        /// use  the set_escdelay  or set_tabsize  functions.Other
        /// global variables are not modifiable.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void set_escdelay(int size)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_set_escdelay(size), "set_escdelay");
        }
        #endregion

        #region set_tabsize
        /// <summary>
        /// see <see cref="set_tabsize"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "set_tabsize")]
        internal extern static int ncurses_set_tabsize(int size);

        /// <summary>
        /// The ESCDELAY and TABSIZE global variables are modified by
        /// some applications.To modify them in  any configuration,
        /// use  the set_escdelay  or set_tabsize  functions.Other
        /// global variables are not modifiable.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void set_tabsize(int size)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_set_tabsize(size), "set_tabsize");
        }
        #endregion

        #region use_default_colors
        /// <summary>
        /// see <see cref="use_default_colors"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_default_colors")]
        internal extern static int ncurses_use_default_colors();

        /// <summary>
        /// The first function, use_default_colors tells  the curses
        /// library to  assign terminal default foreground/background
        /// colors to color number  -1.  So init_pair(x, COLOR_RED,-1)
        /// will initialize  pair x as red on default background and
        /// init_pair(x,-1, COLOR_BLUE)  will initialize  pair x   as
        /// default foreground on blue.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void use_default_colors()
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_use_default_colors(), "use_default_colors");
        }
        #endregion

        #region use_extended_names
        /// <summary>
        /// see <see cref="use_extended_names"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_extended_names")]
        internal extern static int ncurses_use_extended_names(bool enable);

        /// <summary>
        /// The use_extended_names function controls whether the calling application is able to use user-defined or nonstandard
        /// names which may be compiled into the terminfo description,
        /// i.e.,  via the  terminfo or termcap interfaces.Normally
        /// these names are available for  use, since the  essential
        /// decision  is made by using the -x option of tic to compile
        /// extended terminal definitions.However you  can disable
        /// this  feature to ensure compatibility with other implementations of curses.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static int use_extended_names(bool enable)
        {
            return ncurses_use_extended_names(enable);
        }
        #endregion

        #region use_legacy_coding
        /// <summary>
        /// The use_legacy_coding  function  is  an extension to the
        /// curses library.It allows the caller to change the result
        /// of  unctrl, and suppress related checks within the library
        /// that would normally cause nonprinting  characters to  be
        /// rendered in visible form.  This affects only 8-bit characters.
        /// </summary>
        /// <param name="level">
        /// <para>0    the library functions normally, rendering non-printing characters as described in unctrl.</para>
        /// <para>1    the  library ignores isprintf for codes in the range 160-255.</para>
        /// <para>2    the library ignores isprintf for codes in  the range 128-255.
        /// It also modifies the output of unctrl, showing codes in the range 128-159  as is.</para>
        /// </param>
        /// <returns>
        /// If  the  screen  has  not  been  initialized, or the level
        /// parameter is out of range, the function returns ERR.Otherwise, it returns the previous level: 0, 1 or 2.
        /// </returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "use_legacy_coding")]
        public extern static int use_legacy_coding(int level);
        #endregion

        #region nofilter
        /// <summary>
        /// The nofilter  routine cancels  the effect of a preceding
        /// filter call.That allows  the caller  to initialize  a
        /// screen  on a different device, using a different value of
        /// $TERM.The limitation arises because the filter  routine
        /// modifies the in-memory copy of the terminal information.
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "nofilter")]
        public extern static void nofilter();
        #endregion

        #region exported properties
        /// <summary>
        /// used for certain low-level operations like
        /// clearing and redrawing a screen containing  garbage.The
        /// curscr can be used in only a few routines.
        /// </summary>
        /// <returns>pointer to a window</returns>
        public static IntPtr curscr()
        {
            IntPtr window = IntPtr.Zero;
            Action<IntPtr> act = (win) => window = win;

            NativeNCurses.LoadProperty("curscr", act);
            return window;
        }

        /// <summary>
        /// virtual screen to be updated to
        /// </summary>
        /// <returns>pointer to a window</returns>
        public static IntPtr newscr()
        {
            IntPtr window = IntPtr.Zero;
            Action<IntPtr> act = (win) => window = win;

            NativeNCurses.LoadProperty("newscr", act);
            return window;
        }

        /// <summary>
        /// screen's full-window context, used in all NativeStdScr methods
        /// </summary>
        /// <returns>pointer to a window</returns>
        public static IntPtr stdscr()
        {
            IntPtr window = IntPtr.Zero;
            Action<IntPtr> act = (win) => window = win;

            NativeNCurses.LoadProperty("stdscr", act);
            return window;
        }

        public static string ttytype()
        {
            string name = String.Empty;
            Action<IntPtr> act = (ptr) => name = Marshal.PtrToStringUni(ptr);

            NativeNCurses.LoadProperty("ttytype", act);
            return name;
        }

        /// <summary>
        /// COLORS corresponds to the terminal database's max_colors capability,
        /// which is typically a signed 16-bit integer(see terminfo(5)).
        /// </summary>
        /// <returns>number of supported colors</returns>
        public static int COLORS()
        {
            int colors = 0;
            Action<IntPtr> act = (ptr) => colors = Marshal.ReadInt32(ptr);

            NativeNCurses.LoadProperty("COLORS", act);
            return colors;
        }

        /// <summary>
        /// COLOR_PAIRS corresponds  to the  terminal database's max_pairs capability, which  is  typically a  signed 16-bit integer(see terminfo(5)).
        /// </summary>
        /// <returns>number of color pairs</returns>
        public static int COLOR_PAIRS()
        {
            int color_pairs = 0;
            Action<IntPtr> act = (ptr) => color_pairs = Marshal.ReadInt32(ptr);

            NativeNCurses.LoadProperty("COLOR_PAIRS", act);
            return color_pairs;
        }

        /// <summary>
        /// number of columns set on initscr
        /// use <see cref="NativeWindow.wresize"/> to resize a window
        /// </summary>
        /// <returns>number of columns</returns>
        public static int COLS()
        {
            int cols = 0;
            Action<IntPtr> act = (ptr) => cols = Marshal.ReadInt32(ptr);

            NativeNCurses.LoadProperty("COLS", act);
            return cols;
        }

        /// <summary>
        /// Specifies the  total time,  in  milliseconds,  for which
        /// ncurses will await a character sequence, e.g., a function
        /// key.
        /// use <see cref="set_escdelay"/> for thread safety
        /// </summary>
        /// <returns>delay in milliseconds</returns>
        public static int ESCDELAY()
        {
            int escDelay = 0;
            Action<IntPtr> act = (ptr) => escDelay = Marshal.ReadInt32(ptr);

            NativeNCurses.LoadProperty("ESCDELAY", act);
            return escDelay;
        }

        /// <summary>
        /// number of lines set on initscr
        /// use <see cref="NativeWindow.wresize"/> to resize a window
        /// </summary>
        /// <returns>number of columns</returns>
        public static int LINES()
        {
            int lines = 0;
            Action<IntPtr> act = (ptr) => lines = Marshal.ReadInt32(ptr);

            NativeNCurses.LoadProperty("LINES", act);
            return lines;
        }

        /// <summary>
        /// size of the tabs
        /// use <see cref="set_tabsize"/> for thread safety
        /// </summary>
        /// <returns>size of the tabs</returns>
        public static int TABSIZE()
        {
            int tabSize = 0;
            Action<IntPtr> act = (ptr) => tabSize = Marshal.ReadInt32(ptr);

            NativeNCurses.LoadProperty("TABSIZE", act);
            return tabSize;
        }
        #endregion

        #region erasewchar
        /// <summary>
        /// see <see cref="erasewchar"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "erasewchar")]
        internal extern static int ncurses_erasewcharr(IntPtr wch);

        /// <summary>
        /// The erasewchar routine stores the current erase character
        /// in  the location referenced by ch.If no erase character
        /// has been defined, the routine fails and the location  referenced by ch is not changed.
        /// see <see cref="erasechar"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">a reference to store the erase char</param>
        public static void erasewchar(ref NCURSES_CH_T wch)
        {
            IntPtr wPtr = Marshal.AllocHGlobal(Marshal.SizeOf(wch));
            Marshal.StructureToPtr(wch, wPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(wch));

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => ncurses_erasewcharr(wPtr), "erasewchar");
            }
            finally
            {
                Marshal.FreeHGlobal(wPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(wch));
            }
        }
        #endregion

        #region getcchar
        /// <summary>
        /// see <see cref="getcchar"/>
        /// </summary>
        /// <param name="opts">an application must provide a null pointer as opts</param>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "getcchar", CharSet = CharSet.Unicode)]
        internal extern static int ncurses_getcchar(NCURSES_CH_T wcval, StringBuilder wch, out uint attrs, out short color_pair, IntPtr opts);

        /// <summary>
        /// The getcchar  function gets  a wide-character string and
        /// rendition from a cchar_t argument.When wch is not a null
        /// pointer, the getcchar function does the following:
        /// <para>o Extracts information from a cchar_t value wcval</para>
        /// <para>o Stores   the character  attributes  in  the location pointed to by attrs</para>
        /// <para>o Stores the color-pair in the location  pointed to  by color_pair</para>
        /// <para>o Stores  the wide-character string, characters referenced by wcval, into the array pointed to by wch.</para>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">the NCURSES_CH_T to get all properties from</param>
        /// <param name="wch">a reference to store the string (initialized as StringBuilder(5))</param>
        /// <param name="attrs">a reference to store the attributes in</param>
        /// <param name="color_pair">a reference to store the color pair in</param>
        public static void getcchar(NCURSES_CH_T wcval, StringBuilder wch, out uint attrs, out short color_pair)
        {
            NCursesException.Verify(ncurses_getcchar(wcval, wch, out attrs, out color_pair, IntPtr.Zero), "getcchar");
        }
        #endregion

        #region key_name
        /// <summary>
        /// The keyname routine returns a character string corresponding to the key <paramref name="c"/>
        /// </summary>
        /// <param name="c">code of the key</param>
        /// <returns>a string representing the key code</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "key_name")]
        internal extern static IntPtr ncurses_key_name(char c);

        public static string key_name(char c)
        {
            //using own marshalling, because .net can't free the allocated memory
            IntPtr keyNamePtr = ncurses_key_name(c);
            return Marshal.PtrToStringAnsi(keyNamePtr);
        }
        #endregion

        #region killwchar
        /// <summary>
        /// see <see cref="killwchar"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "killwchar")]
        internal extern static int ncurses_killwchar(ref char wch);

        /// <summary>
        /// The killwchar routine stores the current line-kill character in the location referenced by  ch.If no  line-kill
        /// character  has been  defined,  the routine fails and the
        /// location referenced by ch is not changed.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">a reference to store the kill char</param>
        public static void killwchar(ref char wch)
        {
            NCursesException.Verify(ncurses_killwchar(ref wch), "killwchar");
        }
        #endregion

        #region setcchar
        /// <summary>
        /// see <see cref="setcchar"/>
        /// </summary>
        /// <param name="opts">an application must provide a null pointer as opts</param>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "setcchar", CharSet = CharSet.Unicode)]
        internal extern static int ncurses_setcchar(out NCURSES_CH_T wcval, string wch, uint attrs, short color_pair, IntPtr opts);

        /// <summary>
        /// The setcchar function initializes the location pointed to
        /// by wcval by using:
        /// <para>o The character attributes in attrs</para>
        /// <para>o The color pair in color_pair</para>
        /// <para>o The wide-character string pointed  to by  wch.The
        /// string must  be L'\0' terminated, contain at most one
        /// spacing character, which must be the first.</para>
        /// <para>Up to CCHARW_MAX-1 nonspacing characters  may follow.
        /// Additional nonspacing characters are ignored.</para>
        /// <para>The string may  contain a  single control character
        /// instead.  In that case, no nonspacing  characters are
        /// allowed. Color attributes will be OR'd into <see cref="NCURSES_CH_T.attr"/> .
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">the NCURSES_CH_T to get all properties from</param>
        /// <param name="wch">a reference to store the string</param>
        /// <param name="attrs">a reference to store the attributes in</param>
        /// <param name="color_pair">a reference to store the color pair in</param>
        public static void setcchar(out NCURSES_CH_T wcval, string wch, uint attrs, short color_pair)
        {
            NCursesException.Verify(ncurses_setcchar(out wcval, wch, attrs, color_pair, IntPtr.Zero), "setcchar");
        }
        #endregion

        #region slk_wset
        /// <summary>
        /// see <see cref="slk_wset"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "slk_set")]
        internal extern static int ncurses_slk_wset(int labnum, string label, int fmt);

        /// <summary>
        /// <see cref="slk_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_wset(int labnum, string label, int fmt)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_slk_wset(labnum, label, fmt), "slk_wset");
        }
        #endregion

        #region term_attrs
        /// <summary>
        /// see <see cref="termattrs"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "term_attrs")]
        public extern static uint term_attrs();
        #endregion

        #region unget_wch
        /// <summary>
        /// see <see cref="unget_wch"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "unget_wch")]
        internal extern static int ncurses_unget_wch(char wch);

        /// <summary>
        /// The unget_wch function pushes the wide character wch back
        /// onto the head of the input queue, so the wide character is
        /// returned by the next call to get_wch.The pushback of one
        /// character is guaranteed.If the program calls  unget_wch
        /// too many times without an intervening call to get_wch, the
        /// operation may fail.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">a reference to store the kill char</param>
        public static void unget_wch(char wch)
        {
            NCursesException.Verify(ncurses_unget_wch(wch), "unget_wch");
        }
        #endregion

        #region vid_attr
        /// <summary>
        /// see <see cref="vid_attr"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "vid_attr")]
        internal extern static int ncurses_vid_attr(uint attrs, short pair, IntPtr opts);

        /// <summary>
        /// The vid_attr and vid_puts routines correspond  to vidattr
        /// and vidputs, respectively.   They use a set of arguments
        /// for representing the video attributes  plus color, i.e.,
        /// one of type attr_t for the attributes and one of short for
        /// the color_pair number.The vid_attr and vid_puts routines
        /// are  designed to use the attribute constants with the WA_
        /// prefix.The opts argument is  reserved  for  future use.
        /// Currently, applications must  provide a null pointer for
        /// that argument.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attributes to show</param>
        /// <param name="pair">color pair index</param>
        public static void vid_attr(uint attrs, short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_vid_attr(attrs, pair, IntPtr.Zero), "vid_attr");
        }
        #endregion

        #region vid_puts
        /// <summary>
        /// see <see cref="vid_puts"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "vid_puts")]
        internal extern static int ncurses_vid_puts(uint attrs, short pair, IntPtr opts, IntPtr NCURSES_OUTC);

        /// <summary>
        /// see <see cref="vid_attr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void vid_puts(uint attrs, short pair, Func<int, int> NCURSES_OUTC)
        {
            NativeNCurses.VerifyNCursesMethod(() => ncurses_vid_puts(attrs, pair, IntPtr.Zero, Marshal.GetFunctionPointerForDelegate(NCURSES_OUTC)), "vid_puts");
        }
        #endregion

        #region unctrl
        /// <summary>
        /// The unctrl  routine returns a character string which is a
        /// printable representation of the character c, ignoring attributes.Control characters are displayed in the ^X no-
        /// tation.Printing characters are displayed  as  is.   The
        /// corresponding  wunctrl returns a printable representation
        /// of a wide character.
        /// </summary>
        /// <returns>printable representation of the character</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "unctrl")]
        internal extern static string unctrl(uint ch);
        #endregion

        #region wunctrl
        /// <summary>
        /// see <see cref="unctrl"/>
        /// </summary>
        /// <returns>printable representation of the character</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "wunctrl")]
        internal extern static string wunctrl(NCURSES_CH_T ch);
        #endregion

        #region has_mouse
        /// <summary>
        /// The has_mouse  function returns TRUE if the mouse driver has been successfully initialized.
        /// </summary>
        /// <returns>true if terminal supports insert/delete characters</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "has_mouse")]
        public extern static bool has_mouse();
        #endregion

        #region getmouse
        /// <summary>
        /// see <see cref="getmouse"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "getmouse")]
        internal extern static int ncurses_getmouse(out MEVENT ev);

        /// <summary>
        /// Once a class of mouse events has been made visible  in  a
        /// window, calling the wgetch function on that window may return KEY_MOUSE as an indicator that a mouse event has been
        /// queued.To read the event data and pop the event off the
        /// queue, call getmouse.This function will return OK  if  a
        /// mouse  event  is actually visible in the given window, ERR
        /// otherwise.  When getmouse returns OK, the data deposited
        /// as  y and x  in  the event structure coordinates will be
        /// screen-relative character-cell coordinates.The  returned
        /// state  mask will have exactly one bit set to indicate the
        /// event type.The corresponding data in the queue is marked
        /// invalid.A subsequent call to getmouse will retrieve the
        /// next older item from the queue.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void getmouse(out MEVENT ev)
        {
            //if(UseWindowsInputOverride)
            //{
            //    ev = default(MEVENT);
            //    //if (NativeWindows.MouseEvent.bstate == 0)
            //    //{
            //    //    ev = default(MEVENT);
            //    //    NCursesException.Verify(Constants.ERR, "getmouse");
            //    //}
            //    //ev = NativeWindows.MouseEvent;
            //}
            //else
                NCursesException.Verify(ncurses_getmouse(out ev), "getmouse");
        }
        #endregion

        #region ungetmouse
        /// <summary>
        /// see <see cref="ungetmouse"/>
        /// </summary>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "ungetmouse")]
        internal extern static int ncurses_ungetmouse(MEVENT ev);

        /// <summary>
        /// The ungetmouse function behaves  analogously to  ungetch.
        /// It pushes a KEY_MOUSE event onto the input queue, and associates with that event the given state data and
        /// screen-relative character-cell coordinates.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void ungetmouse(MEVENT ev)
        {
            NCursesException.Verify(ncurses_ungetmouse(ev), "ungetmouse");
        }
        #endregion

        #region mousemask
        /// <summary>
        /// To make mouse events visible, use the mousemask function.
        /// This will set the mouse events to  be reported.   By de-
        /// fault, no mouse  events are reported.The function will
        /// return a mask to indicate which of  the specified  mouse
        /// events  can be reported; on complete failure it returns 0.
        /// If oldmask is non-NULL, this function fills the indicated
        /// location with  the previous  value of the given window's
        /// mouse event mask.
        /// </summary>
        /// <returns>the supported mousemasks</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "mousemask")]
        public extern static uint mousemask(uint newmask, out uint oldmask);
        #endregion

        #region mouseinterval
        /// <summary>
        /// The mouseinterval function sets the maximum time(in thousands of a second)
        /// that can elapse between press and  release events  for  them to be recognized as a click.Use
        /// mouseinterval(0) to disable click resolution.This  function returns the previous interval value.
        /// Use mouseinterval(-1) to obtain the interval without altering  it.The
        /// default is one sixth of a second.
        /// </summary>
        /// <returns>the previous mouse interval</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "mouseinterval")]
        public extern static int mouseinterval(int erval);
        #endregion

        #region mouse_trafo
        /// <summary>
        /// see <see cref="NativeWindow.wmouse_trafo"/>
        /// </summary>
        [DllImport(Constants.DLLNAME, EntryPoint = "wmouse_trafo")]
        public extern static bool mouse_trafo(ref int pY, ref int pX, bool to_screen);
        #endregion

        #region has_key
        /// <summary>
        /// The has_key routine takes a key-code value from the above
        /// list, and returns TRUE or FALSE according to  whether the
        /// current terminal type recognizes a key with that value.
        /// </summary>
        /// <param name="ch">the key code you want to test</param>
        /// <returns>true or false</returns>
        [DllImport(Constants.DLLNAME, EntryPoint = "has_key")]
        public extern static int has_key(int ch);
        #endregion

        #region setfont
        /// <summary>
        /// Change the font of the terminal (windows only)
        /// <para>Do this before resizing, as it changes the max possible window size</para>
        /// </summary>
        /// <param name="font">The font you want to change to</param>
        public static void SetConsoleFont(WindowsConsoleFont font)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new ArgumentException("Changing the font not available on this platform");

            string fontName = string.Empty;
            switch (font)
            {
                case WindowsConsoleFont.TERMINAL:
                    fontName = "Terminal";
                    break;
                case WindowsConsoleFont.LUCIDA:
                    fontName = "Lucida Console";
                    break;
                case WindowsConsoleFont.CONSOLAS:
                    fontName = "Consolas";
                    break;
            }

            //address of the handle created by CreateConsoleScreenBuffer in NCurses
            //https://github.com/rprichard/win32-console-docs -> src/harness/WorkerProgram.cc scanForConsoleHandles
            //TODO: win7 only?
            IntPtr bufferHandle = new IntPtr(19);

            CONSOLE_FONT_INFO_EX newFont = new CONSOLE_FONT_INFO_EX();
            newFont.cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFO_EX>();
            newFont.FaceName = fontName;

            if (!NativeWindows.SetCurrentConsoleFontEx(bufferHandle, false, newFont))
                throw new ArgumentException("Couldn't set the font");
        }
        #endregion

        /// <summary>
        /// check if the current terminal is unicode-able
        /// </summary>
        /// <returns>true or false</returns>
        [DllImport(Constants.DLLNAME)]
        public extern static bool _nc_unicode_locale();

        /// <summary>
        /// returns the screen of the window
        /// </summary>
        /// <param name="window">pointer to a window</param>
        /// <returns>reference to a screen</returns>
        [DllImport(Constants.DLLNAME)]
        public extern static IntPtr _nc_screen_of(IntPtr window);

        [DllImport(Constants.DLLNAME)]
        public extern static int _nc_mingw_console_read(IntPtr screen, IntPtr intput, out int key);
    }
}
