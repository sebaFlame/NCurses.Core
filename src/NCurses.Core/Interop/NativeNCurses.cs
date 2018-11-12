using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.MultiByteString;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.SingleByteString;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.Platform;
using NCurses.Core.Interop.Dynamic;
using System.Threading;

[assembly: InternalsVisibleTo("NCurses.Core.Interop.Dynamic.Generated")]
[assembly: InternalsVisibleTo("NCurses.Core.Tests")]

namespace NCurses.Core.Interop
{
    public delegate int NCURSES_WINDOW_CB(IntPtr window, IntPtr args);
    public delegate int NCURSES_SCREEN_CB(IntPtr screen, IntPtr args);

    /// <summary>
    /// native curses methods.
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    internal static class NativeNCurses
    {
        private static INCursesWrapper wrapper;
        public static INCursesWrapper NCursesWrapper
        {
            get
            {
                if (wrapper is null)
                    wrapper = (INCursesWrapper)Activator.CreateInstance(DynamicTypeBuilder.CreateNCursesWrapper(Constants.DLLNAME));
                return wrapper;
            }
        }

        private static bool? hasUnicodeSupport;
        internal static bool HasUnicodeSupport => hasUnicodeSupport.HasValue ? hasUnicodeSupport.Value : throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);

        internal static INativeWrapper NCursesCustomTypeWrapper { get; private set; }
        internal static INativeWrapper NCursesCharTypeWrapper { get; private set; }

        /// <summary>
        /// These only return a number representation of the symbol, which NCurses can handle internally
        /// </summary>
        internal static IACSMap Acs_Map;
        internal static IACSMap Wacs_Map;

        private static Encoding encoding;
        internal static Encoding Encoding
        {
            get
            {
                return (encoding is null)
                    ? RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        ? (encoding = Encoding.Unicode)
                        : (encoding = Encoding.UTF32)
                    : encoding;
            }
        }

        private static INativeLoader nativeLoader;
        private static INativeLoader NativeLoader
        {
            get
            {
                return (nativeLoader is null)
                    ? RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                        ? nativeLoader = new WindowsLoader()
                        : nativeLoader = new LinuxLoader()
                    : nativeLoader;
            }
        }

        #region single byte custom type wrapper fields
        private static INativeNCursesSmallStr smallStrCursesWrapper;
        private static INativeNCursesSmallStr SmallStrCursesWrapper => smallStrCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);
        private static INativeNCursesWideStr wideStrCursesWrapper;
        private static INativeNCursesWideStr WideStrCursesWrapper => wideStrCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);
        #endregion

        #region multi byte custom type wrapper fields
        private static INativeNCursesWide wideNCursesWrapper;
        private static INativeNCursesWide WideNCursesWrapper => NativeNCurses.HasUnicodeSupport
              ? wideNCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage)
              : throw new InvalidOperationException(Constants.NoUnicodeExceptionMessage);
        private static INativeNCursesSmall smallCursesWrapper;
        private static INativeNCursesSmall SmallCursesWrapper => NativeNCurses.HasUnicodeSupport
              ? smallCursesWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage)
              : throw new InvalidOperationException(Constants.NoUnicodeExceptionMessage);
        #endregion

        #region locking
        internal static readonly object SyncRoot = new object();
        internal static bool EnableLocking { get; set; }
        #endregion

        #region custom type initialization
        internal static void CreateCharCustomWrappers()
        {
            if (DynamicTypeBuilder.schar is null)
                throw new InvalidOperationException("Custom types haven't been generated yet.");

            Type customType;
            if (smallStrCursesWrapper is null)
            {
                customType = typeof(NativeNCursesSmallStr<>).MakeGenericType(DynamicTypeBuilder.schar);
                smallStrCursesWrapper = (INativeNCursesSmallStr)Activator.CreateInstance(customType);
            }
        }

        internal static void CreateCustomTypeWrappers()
        {
            if ((DynamicTypeBuilder.chtype is null
                || DynamicTypeBuilder.schar is null)
                || (NativeNCurses.HasUnicodeSupport
                    && (DynamicTypeBuilder.cchar_t is null || DynamicTypeBuilder.wchar_t is null)))
                throw new InvalidOperationException("Custom types haven't been generated yet.");

            Type customType;
            if (NativeNCurses.HasUnicodeSupport)
            {
                if (wideNCursesWrapper is null)
                {
                    customType = typeof(NativeNCursesWide<,,,,>).MakeGenericType(DynamicTypeBuilder.cchar_t, DynamicTypeBuilder.wchar_t, 
                        DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar, DynamicTypeBuilder.MEVENT);
                    wideNCursesWrapper = (INativeNCursesWide)Activator.CreateInstance(customType);
                }

                if (wideStrCursesWrapper is null)
                {
                    customType = typeof(NativeNCursesWideStr<,>).MakeGenericType(DynamicTypeBuilder.wchar_t, DynamicTypeBuilder.schar);
                    wideStrCursesWrapper = (INativeNCursesWideStr)Activator.CreateInstance(customType);
                }
            }

            if (smallCursesWrapper is null)
            {
                customType = typeof(NativeNCursesSmall<,,>).MakeGenericType(DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar, DynamicTypeBuilder.MEVENT);
                smallCursesWrapper = (INativeNCursesSmall)Activator.CreateInstance(customType);
            }
        }
        #endregion

        #region native thread-safety
        /// <summary>
        /// Execute a thread-safe WINDOW method with verification.
        /// </summary>
        /// <param name="window">A pointer to the window</param>
        /// <param name="callback">A pointer to a callback function using NCURSES_WINDOW_CB</param>
        /// <param name="args">A pointer to the arguments</param>
        public static void use_window(IntPtr window, NCURSES_WINDOW_CB callback, IntPtr args)
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

        internal static bool VerifyInput(string method, int val, out char ch, out Key key)
        {
            //if(has_key(val)) //TODO: does not work on windows
            if (HasKey(val, out key))
            {
                ch = '\0';
                return true;
            }

            NCursesException.Verify(val, method);
            ch = (char)(sbyte)val;
            key = default(Key);
            return false;
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
                if (bytes1[length - 1 - i] != bytes2[length - 1 - i])
                    return false;

            return true;
        }
        #endregion

        #region baudrate
        /// <summary>
        /// returns the output speed of the terminal.
        /// </summary>
        /// <returns>The number returned is in  bits per  second,  for example 9600, and is an integer.</returns>
        public static int baudrate()
        {
            return NCursesWrapper.baudrate();
        }
        #endregion

        #region beep
        /// <summary>
        /// sounds an  audible  alarm  on  the terminal,  if  possible;
        /// native method wrapped with verification.
        /// </summary>
        public static void beep()
        {
            NCursesException.Verify(NCursesWrapper.beep(), "beep");
        }
        #endregion

        #region can_change_color
        /// <summary>
        /// check if the  terminal  supports  colors and can change their definitions
        /// </summary>
        /// <returns>returns true if the  terminal  supports  colors and can change their definitions</returns>
        public static bool can_change_color()
        {
            return NCursesWrapper.can_change_color();
        }
        #endregion

        #region cbreak
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
            NCursesException.Verify(NCursesWrapper.cbreak(), "cbreak");
        }
        #endregion

        #region nocbreak
        /// <summary>
        /// returns the terminal to normal(cooked) mode
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void nocbreak()
        {
            NCursesException.Verify(NCursesWrapper.nocbreak(), "nocbreak");
        }
        #endregion

        #region color_content
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
        public static unsafe void color_content(short color, ref short red, ref short green, ref short blue)
        {
            Span<short> spanRed = new Span<short>(Unsafe.AsPointer<short>(ref red), Marshal.SizeOf<short>());
            Span<short> spanGreen = new Span<short>(Unsafe.AsPointer<short>(ref green), Marshal.SizeOf<short>());
            Span<short> spanBlue = new Span<short>(Unsafe.AsPointer<short>(ref blue), Marshal.SizeOf<short>());

            ref short redRef = ref spanRed.GetPinnableReference();
            ref short greenRef = ref spanGreen.GetPinnableReference();
            ref short blueRef = ref spanBlue.GetPinnableReference();

            NCursesException.Verify(NCursesWrapper.color_content(color, ref redRef, ref greenRef, ref blueRef), "color_content");
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
            return NCursesWrapper.COLOR_PAIR(pair);
        }
        #endregion

        #region copywin
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
            NCursesException.Verify(NCursesWrapper.copywin(srcwin, dstwin, sminrow, smincol, dminrow, dmincol, dmaxrow, dmaxcol, overlay), "copywin");
        }
        #endregion

        #region curs_set
        /// <summary>
        /// sets the cursor visbility
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="visibility">invisible, normal, or very visible for visibility equal to 0, 1, or 2 respectively</param>
        public static void curs_set(int visibility)
        {
            NCursesException.Verify(NCursesWrapper.curs_set(visibility), "curs_set");
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
        public static int def_prog_mode()
        {
            return NCursesWrapper.def_prog_mode();
        }
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
        public static int def_shell_mode()
        {
            return NCursesWrapper.def_shell_mode();
        }
        #endregion

        #region delay_output
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
            NCursesException.Verify(NCursesWrapper.delay_output(ms), "delay_output");
        }
        #endregion

        #region delscreen
        /// <summary>
        /// The delscreen  routine frees storage associated with the
        /// SCREEN data structure.The endwin  routine does  not  do
        /// this, so delscreen should be called after endwin if a particular SCREEN is no longer needed.
        /// </summary>
        /// <param name="screen">A pointer to a screen</param>
        public static void delscreen_sp(IntPtr screen)
        {
            NCursesWrapper.delscreen(screen);
        }
        #endregion

        #region delwin
        /// <summary>
        /// Calling delwin deletes the named window,
        /// freeing all memory associated with it(it does not actually erase the window's screen image).
        /// Subwindows must  be  deleted  before the main window can be deleted.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void delwin(IntPtr window)
        {
            NCursesException.Verify(NCursesWrapper.delwin(window), "delwin");
        }
        #endregion

        #region derwin
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
            return NCursesException.Verify(NCursesWrapper.derwin(window, nlines, ncols, begin_y, begin_x), "derwin");
        }
        #endregion

        #region doupdate
        /// <summary>
        /// update the current screen
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void doupdate()
        {
            NCursesException.Verify(NCursesWrapper.doupdate(), "doupdate");
        }
        #endregion

        #region dupwin
        /// <summary>
        /// Calling  dupwin  creates  an exact duplicate of the window <paramref name="window"/>.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to the window to copy</param>
        /// <returns>A pointer to a new window</returns>
        public static IntPtr dupwin(IntPtr window)
        {
            return NCursesException.Verify(dupwin(window), "dupwin");
        }
        #endregion

        #region echo
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
            NCursesException.Verify(NCursesWrapper.echo(), "echo");
        }
        #endregion

        #region endwin
        /// <summary>
        /// The program must also call endwin for each terminal being
        /// used before exiting from curses. If  newterm  is  called
        /// more  than once for the same terminal, the first terminal
        /// referred to must be the  last one  for  which endwin  is  called.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void endwin()
        {
            NCursesException.Verify(NCursesWrapper.endwin(), "endwin");
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
        public static char erasechar()
        {
            return SmallStrCursesWrapper.erasechar();
        }
        #endregion

        #region filter
        /// <summary>
        /// The filter routine, if used, must be called before initscr
        /// or newterm are called.The effect is that, during those
        /// calls, LINES  is  set to 1; the capabilities clear, cup,
        /// cud, cud1, cuu1, cuu,  vpa are  disabled;  and the  home
        /// string is set to the value of cr.
        /// </summary>
        public static void filter()
        {
            NCursesWrapper.filter();
        }
        #endregion

        #region flash
        /// <summary>
        /// The routine flash flashes the screen, and
        /// if  that  is  not possible, sounds the alert.If neither
        /// alert is possible, nothing happens.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void flash()
        {
            NCursesException.Verify(NCursesWrapper.flash(), "flash");
        }
        #endregion

        #region flushinp
        /// <summary>
        /// The flushinp  routine throws away any typeahead that has
        /// been typed by the user and has not yet been read  by the
        /// program.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void flushinp()
        {
            NCursesException.Verify(NCursesWrapper.flushinp(), "flushinp");
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
            NCursesException.Verify(NCursesWrapper.halfdelay(tenths), "halfdelay");
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
        public static bool has_colors()
        {
            return NCursesWrapper.has_colors();
        }
        #endregion

        #region has_ic
        /// <summary>
        /// The has_ic routine is true if the terminal has insert- and
        /// delete- character capabilities.
        /// </summary>
        /// <returns>true if terminal supports insert/delete characters</returns>
        public static bool has_ic()
        {
            return NCursesWrapper.has_ic();
        }
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
        public static bool has_il()
        {
            return NCursesWrapper.has_il();
        }
        #endregion

        #region initscr
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
            //TODO: should only be called once
            if (NativeLoader is LinuxLoader linuxLoader)
                linuxLoader.SetLocale(6, ""); //6 = LC_ALL

            if (!hasUnicodeSupport.HasValue)
                hasUnicodeSupport = NativeNCurses._nc_unicode_locale();

            IntPtr stdScr = NCursesException.Verify(NCursesWrapper.initscr(), "initscr");

            if(NCursesCharTypeWrapper is null)
            {
                NCursesCharTypeWrapper = (INativeWrapper)Activator.CreateInstance(DynamicTypeBuilder.CreateCharTypeWrapper(Constants.DLLNAME));
                NativeNCurses.CreateCharCustomWrappers();
                NativeStdScr.CreateCharCustomWrappers();
                NativeWindow.CreateCharCustomWrappers();
                NativeScreen.CreateCharCustomWrappers();
                NativePad.CreateCharCustomWrappers();
            }

            /*TODO
             * version / platform customization
             * use correct chtype type
            */
            if (NCursesCustomTypeWrapper is null)
            {
                //chtype size calculation
                string version = NativeNCurses.curses_version();
                int major = (int)char.GetNumericValue(version, version.IndexOf(' ') + 1);

                Type chtypeType;
                if (major >= 6)
                    chtypeType = typeof(UInt32);
                else
                    chtypeType = typeof(UInt64);

                NCursesCustomTypeWrapper = (INativeWrapper)Activator.CreateInstance(DynamicTypeBuilder.CreateCustomTypeWrapper(Constants.DLLNAME, HasUnicodeSupport));
                NativeNCurses.CreateCustomTypeWrappers();
                NativeStdScr.CreateCustomTypeWrappers();
                NativeWindow.CreateCustomTypeWrappers();
                NativeScreen.CreateCustomTypeWrappers();
                NativePad.CreateCustomTypeWrappers();
            }

            //TODO: put in methods (if it fails and can't return stdScr)
            if (Acs_Map is null)
            {
                Action<IntPtr> loadAcs = (IntPtr acsPtr) =>
                {
                    Type acsMapType = typeof(ACSMap<>).MakeGenericType(DynamicTypeBuilder.chtype);
                    NativeNCurses.Acs_Map = (IACSMap)Activator.CreateInstance(acsMapType, new object[] { acsPtr });
                };
                NativeNCurses.LoadProperty("acs_map", loadAcs);
            }

            if (HasUnicodeSupport && Wacs_Map is null)
            {
                Action<IntPtr> loadWacs = (IntPtr wacsPtr) =>
                {
                    IntPtr real_wacsPtr = Marshal.ReadIntPtr(wacsPtr);
                    Type acsMapType = typeof(ACSMap<>).MakeGenericType(DynamicTypeBuilder.cchar_t);
                    NativeNCurses.Wacs_Map = (IACSMap)Activator.CreateInstance(acsMapType, new object[] { real_wacsPtr });
                };
                NativeNCurses.LoadProperty("_nc_wacs", loadWacs);
            }

            return stdScr;
        }
        #endregion

        #region init_color
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
            NCursesException.Verify(NCursesWrapper.init_color(color, r, g, b), "init_color");
        }
        #endregion

        #region init_pair
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
            NCursesException.Verify(NCursesWrapper.init_pair(pair, f, b), "init_pair");
        }
        #endregion

        #region intrflush
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
            NCursesException.Verify(NCursesWrapper.intrflush(win, bf), "intrflush");
        }
        #endregion

        #region isendwin
        /// <summary>
        /// The isendwin routine returns  TRUE  if  endwin has  been
        /// called without any subsequent calls to wrefresh, and FALSE
        /// otherwise.
        /// </summary>
        /// <returns>if endwin has been called</returns>
        public static bool isendwin()
        {
            return NCursesWrapper.isendwin();
        }
        #endregion

        #region keyname
        /// <summary>
        /// The keyname routine returns a character string corresponding to the key <paramref name="c"/>
        /// </summary>
        /// <param name="c">code of the key</param>
        /// <returns>a string representing the key code</returns>
        public static string keyname(int c)
        {
            return SmallStrCursesWrapper.keyname(c);
        }
        #endregion

        #region killchar
        /// <summary>
        /// returns the user's current line  kill character.
        /// </summary>
        /// <returns>the kill character</returns>
        public static char killchar()
        {
            return SmallStrCursesWrapper.killchar();
        }
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
        public static string longname()
        {
            return SmallStrCursesWrapper.longname();
        }
        #endregion

        #region meta
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
            NCursesException.Verify(NCursesWrapper.meta(win, bf), "meta");
        }
        #endregion

        #region mvcur
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
            NCursesException.Verify(NCursesWrapper.mvcur(oldrow, oldcol, newrow, newcol), "mvcur");
        }
        #endregion

        #region mvderwin
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
            NCursesException.Verify(NCursesWrapper.mvderwin(window, par_y, par_x), "derwin");
        }
        #endregion

        #region mvwin
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
            NCursesException.Verify(NCursesWrapper.mvwin(win, y, x), "mvwin");
        }
        #endregion

        #region napms
        /// <summary>
        /// The napms routine is used to sleep for <paramref name="ms"/> milliseconds.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ms">The amount of milliseconds to sleep for</param>
        public static void napms(int ms)
        {
            NCursesException.Verify(NCursesWrapper.napms(ms), "napms");
        }
        #endregion

        #region newpad
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
            return NCursesException.Verify(NCursesWrapper.newpad(nlines, ncols), "newpad");
        }
        #endregion

        /* TODO
         * SCREEN *newterm(char *type, FILE *outfd, FILE *infd);
         * needs MSVCRT.dll (Microsoft C/C++ Language and Standard Libraries)
        */

        #region newwin
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
            return NCursesException.Verify(NCursesWrapper.newwin(nlines, ncols, begin_y, begin_x), "newwin");
        }
        #endregion

        #region nl
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
            NCursesException.Verify(NCursesWrapper.nl(), "nl");
        }
        #endregion

        #region noecho
        /// <summary>
        /// see <see cref="echo"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void noecho()
        {
            NCursesException.Verify(NCursesWrapper.noecho(), "noecho");
        }
        #endregion

        #region nonl
        /// <summary>
        /// see <see cref="nl"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void nonl()
        {
            NCursesException.Verify(NCursesWrapper.nonl(), "nonl");
        }
        #endregion

        #region noqiflush
        /// <summary>
        /// see <see cref="qiflush"/>
        /// </summary>
        public static void noqiflush()
        {
            NCursesWrapper.noqiflush();
        }
        #endregion

        #region noraw
        /// <summary>
        /// see <see cref="nl"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void noraw()
        {
            NCursesException.Verify(NCursesWrapper.noraw(), "noraw");
        }
        #endregion

        #region overlay
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
            NCursesException.Verify(NCursesWrapper.overlay(srcWin, destWin), "overlay");
        }
        #endregion

        #region overwrite
        /// <summary>
        /// see <see cref="overlay(IntPtr, IntPtr)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void overwrite(IntPtr srcWin, IntPtr destWin)
        {
            NCursesException.Verify(NCursesWrapper.overwrite(srcWin, destWin), "overwrite");
        }
        #endregion

        #region pair_content
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
            NCursesException.Verify(NCursesWrapper.pair_content(pair, out fg, out bg), "pair_content");
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
        public static int PAIR_NUMBER(uint attrs)
        {
            return NCursesWrapper.PAIR_NUMBER(attrs);
        }
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
        public static void qiflush()
        {
            NCursesWrapper.qiflush();
        }
        #endregion

        #region raw
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
            NCursesException.Verify(NCursesWrapper.raw(), "raw");
        }
        #endregion

        #region resetty
        /// <summary>
        /// The resetty and savetty  routines save  and restore  the
        /// state  of the  terminal modes.savetty saves the current
        /// state in a buffer and resetty restores the state to  what
        /// it was at the last call to savetty.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void resetty()
        {
            NCursesException.Verify(NCursesWrapper.resetty(), "resetty");
        }
        #endregion

        #region reset_prog_mode
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
            NCursesException.Verify(NCursesWrapper.reset_prog_mode(), "reset_prog_mode");
        }
        #endregion

        #region reset_shell_mode
        /// <summary>
        /// see <see cref="reset_prog_mode"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void reset_shell_mode()
        {
            NCursesException.Verify(NCursesWrapper.reset_prog_mode(), "reset_shell_mode");
        }
        #endregion

        #region ripoffline
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
            func = Marshal.GetFunctionPointerForDelegate(new ripoffDelegate(initCallback)); //TODO: needs to be double wrapped as pointer?

            //TODO: register func for GC
            NCursesException.Verify(NCursesWrapper.ripoffline(line, func), "ripoffline");
        }
        #endregion

        #region savetty
        /// <summary>
        /// see <see cref="resetty"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void savetty()
        {
            NCursesException.Verify(NCursesWrapper.savetty(), "savetty");
        }
        #endregion

        #region scr_dump
        /// <summary>
        /// see <see cref="src_init(string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void scr_dump(in string filename)
        {
            SmallStrCursesWrapper.scr_dump(filename);
        }
        #endregion

        #region scr_init
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
        public static void scr_init(in string filename)
        {
            SmallStrCursesWrapper.scr_init(filename);
        }
        #endregion

        #region scr_restore
        /// <summary>
        /// The scr_restore  routine sets  the virtual screen to the
        /// contents of filename, which must have been written  using
        /// scr_dump.   The  next call to doupdate restores the screen
        /// to the way it looked in the dump file.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void scr_restore(in string filename)
        {
            SmallStrCursesWrapper.scr_restore(filename);
        }
        #endregion

        #region scr_set
        /// <summary>
        /// The scr_set routine is a combination  of scr_restore  and
        /// scr_init.It  tells the program that the information in
        /// filename is what is currently on the screen, and also what
        /// the  program wants on the screen.This can be thought of
        /// as a screen inheritance function.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void scr_set(in string filename)
        {
            SmallStrCursesWrapper.scr_set(filename);
        }
        #endregion

        #region set_term
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
            return NCursesException.Verify(NCursesWrapper.set_term(newScr), "set_term");
        }
        #endregion

        #region slk_attroff
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
        public static void slk_attroff(ulong attrs)
        {
            SmallCursesWrapper.slk_attroff(attrs);
        }
        #endregion

        #region slk_attr_off
        /// <summary>
        /// see <see cref="NativeStdScr.attr_off(chtype)"/>  (for soft function keys)
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_attr_off(ulong attrs)
        {
            SmallCursesWrapper.slk_attr_off(attrs);
        }
        #endregion

        #region slk_attron
        /// <summary>
        /// see <see cref="slk_attroff(chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attr"></param>
        public static void slk_attron(ulong attrs)
        {
            SmallCursesWrapper.slk_attron(attrs);
        }
        #endregion

        #region slk_attr_on
        /// <summary>
        /// see <see cref="NativeStdScr.attr_on(chtype)"/> (for soft function keys)
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_attr_on(ulong attrs)
        {
            SmallCursesWrapper.slk_attr_on(attrs);
        }
        #endregion

        #region slk_attrset
        /// <summary>
        /// see <see cref="slk_attroff(chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attr"></param>
        public static void slk_attrset(ulong attrs)
        {
            SmallCursesWrapper.slk_attrset(attrs);
        }
        #endregion

        #region slk_attr
        /// <summary>
        /// returns the the attribute used for the soft keys
        /// </summary>
        /// <returns>an attribute</returns>
        public static ulong slk_attr()
        {
            return SmallCursesWrapper.slk_attr();
        }
        #endregion

        #region slk_attr_set
        /// <summary>
        /// see <see cref="NativeStdScr.attr_set(chtype, short)"/> (for soft function keys)
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_attr_set(ulong attrs, short color_pair)
        {
            SmallCursesWrapper.slk_attr_set(attrs, color_pair);
        }
        #endregion

        #region slk_clear
        /// <summary>
        /// The slk_clear routine clears  the soft  labels from  the screen.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_clear()
        {
            NCursesException.Verify(NCursesWrapper.slk_clear(), "slk_clear");
        }
        #endregion

        #region slk_color
        /// <summary>
        /// The slk_color routine corresponds to color_set.It has an
        /// effect only  if  soft labels are simulated on the bottom
        /// line of the screen.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_color(short color_pair)
        {
            NCursesException.Verify(NCursesWrapper.slk_color(color_pair), "slk_color");
        }
        #endregion

        #region slk_init
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
            NCursesException.Verify(NCursesWrapper.slk_init(fmt), "slk_init");
        }
        #endregion

        #region slk_label
        /// <summary>
        /// The  slk_label routine returns the current label for label
        /// number <paramref name="labnum"/>, with leading and trailing blanks stripped.
        /// </summary>
        /// <param name="labnum">number of the label for which you want to return the label</param>
        /// <returns>label</returns>
        public static string slk_label(int labnum)
        {
            return SmallStrCursesWrapper.slk_label(labnum);
        }
        #endregion

        #region slk_noutrefresh
        /// <summary>
        /// The slk_refresh and slk_noutrefresh routines correspond to
        /// the <see cref="NativeWindow.wrefresh(IntPtr)"/>  and <see cref="NativeWindow.wnoutrefresh"/>  routines.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_noutrefresh()
        {
            NCursesException.Verify(NCursesWrapper.slk_noutrefresh(), "slk_noutrefresh");
        }
        #endregion

        #region slk_refresh
        /// <summary>
        /// see <see cref="slk_noutrefresh"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_refresh()
        {
            NCursesException.Verify(NCursesWrapper.slk_refresh(), "slk_refresh");
        }
        #endregion

        #region slk_restore
        /// <summary>
        /// The slk_restore  routine restores the soft labels to the
        /// screen after a slk_clear has been performed.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_restore()
        {
            NCursesException.Verify(NCursesWrapper.slk_restore(), "slk_restore");
        }
        #endregion

        #region slk_set
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
        public static void slk_set(int labnum, in string label, int fmt)
        {
            SmallStrCursesWrapper.slk_set(labnum, label, fmt);
        }
        #endregion

        #region slk_touch
        /// <summary>
        /// The slk_touch routine forces all the  soft labels  to be
        /// output the next time a slk_noutrefresh is performed.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_touch()
        {
            NCursesException.Verify(NCursesWrapper.slk_touch(), "slk_touch");
        }
        #endregion

        #region start_color
        /// <summary>
        /// The start_color routine requires no arguments.It must be
        /// called if the programmer wants to use colors, and before
        /// any other  color manipulation  routine is called.It is
        /// good practice to call this routine right  after <see cref="initscr"/> .
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void start_color()
        {
            NCursesException.Verify(NCursesWrapper.start_color(), "start_color");
        }
        #endregion

        #region subpad
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
            return NCursesException.Verify(NCursesWrapper.subpad(orig, nlines, ncols, begin_y, begin_x), "subpad");
        }
        #endregion

        #region subwin
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
            return NCursesException.Verify(NCursesWrapper.subwin(orig, nlines, ncols, begin_y, begin_x), "subpad");
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
        public static ulong termattrs()
        {
            return SmallCursesWrapper.termattrs();
        }
        #endregion

        #region termname
        /// <summary>
        /// The termname routine returns the terminal  name used  by setupterm.
        /// </summary>
        /// <returns>the terminal name</returns>
        public static string termname()
        {
            return SmallStrCursesWrapper.termname();
        }
        #endregion

        #region typeahead
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
            NCursesException.Verify(NCursesWrapper.typeahead(fd), "typeahead");
        }
        #endregion

        #region ungetch
        /// <summary>
        /// The ungetch routine places ch back onto the input queue to
        /// be returned by the next call to wgetch.There is just one
        /// input queue for all windows.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ch">character to place into the input queue</param>
        public static void ungetch(int ch)
        {
            NCursesException.Verify(NCursesWrapper.ungetch(ch), "ungetch");
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
        public static void use_env(bool f)
        {
            NCursesWrapper.use_env(f);
        }
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
        public static void use_tioctl(bool f)
        {
            NCursesWrapper.use_tioctl(f);
        }
        #endregion

        #region vidattr
        /// <summary>
        /// The  vidattr  routine  is like the vidputs routine, except that it outputs through putchar.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attributes to show</param>
        public static void vidattr(ulong attrs)
        {
            SmallCursesWrapper.vidattr(attrs);
        }
        #endregion

        #region vidputs
        /// <summary>
        /// The vidputs routine displays the string on the terminal in
        /// the video  attribute mode attrs, which is any combination
        /// of the attributes listed in  curses(3x).   The characters
        /// are passed to the putchar-like routine putc.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attributes to show</param>
        public static void vidputs(ulong attrs, Func<int, int> NCURSES_OUTC)
        {
            SmallCursesWrapper.vidputs(attrs, NCURSES_OUTC);
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
        public static int tigetflag(in string capName)
        {
            return SmallStrCursesWrapper.tigetflag(capName);
        }
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
        public static int tigetnum(in string capname)
        {
            return SmallStrCursesWrapper.tigetnum(capname);
        }
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
        public static int tigetstr(in string capname)
        {
            return SmallStrCursesWrapper.tigetstr(capname);
        }
        #endregion

        #region putp
        /// <summary>
        /// The putp routine calls tputs(str, 1, putchar).  Note that
        /// the output  of putp  always goes  to stdout, not to the
        /// fildes specified in setupterm.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attributes to show</param>
        public static void putp(in string str)
        {
            SmallStrCursesWrapper.putp(str);
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
        public static bool is_term_resized(int lines, int columns)
        {
            return NCursesWrapper.is_term_resized(lines, columns);
        }
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
        public static string keybound(int keycode, int count)
        {
            return SmallStrCursesWrapper.keybound(keycode, count);
        }
        #endregion

        #region curses_version
        /// <summary>
        /// Use  curses_version  to  get the version number, including  patch level of the library, e.g., 5.0.19991023
        /// </summary>
        /// <returns>version string</returns>
        public static string curses_version()
        {
            return SmallStrCursesWrapper.curses_version();
        }
        #endregion

        #region assume_default_colors
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
            NCursesException.Verify(NCursesWrapper.assume_default_colors(fg, bg), "assume_default_colors");
        }
        #endregion

        #region define_key
        /// <summary>
        /// This is an extension to the curses library.It permits an
        /// application to define keycodes  with their  corresponding
        /// control  strings, so that the ncurses library will interpret them just as it would the predefined  codes  in  the
        /// terminfo database.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void define_key(in string definition, int keycode)
        {
            SmallStrCursesWrapper.define_key(definition, keycode);
        }
        #endregion

        #region get_escdelay
        /// <summary>
        /// The get_escdelay function returns the value for ESCDELAY.
        /// </summary>
        public static int get_escdelay()
        {
            return NCursesWrapper.get_escdelay();
        }
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
        public static int key_defined(string definition)
        {
           return SmallStrCursesWrapper.key_defined(definition);
        }
        #endregion

        #region keyok
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
            NCursesException.Verify(NCursesWrapper.keyok(keycode, enable), "keyok");
        }
        #endregion

        #region resize_term
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
            NCursesException.Verify(NCursesWrapper.resize_term(lines, columns), "resize_term");
            //TODO: check for < win 10
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //    NativeWindows.NativeWindowsConsoleResize(lines, columns);
        }
        #endregion

        #region resizeterm
        /// <summary>
        /// The function  resizeterm resizes the standard and current
        /// windows to the specified  dimensions,  and adjusts  other
        /// bookkeeping  data used by the ncurses library that record
        /// the window dimensions such as the LINES  and COLS  variables.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void resizeterm(int lines, int columns)
        {
            NCursesException.Verify(NCursesWrapper.resizeterm(lines, columns), "resizeterm");
            //TODO:check for < win 10
            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //    NativeWindows.NativeWindowsConsoleResize(lines, columns);
        }
        #endregion

        #region set_escdelay
        /// <summary>
        /// The ESCDELAY and TABSIZE global variables are modified by
        /// some applications.To modify them in  any configuration,
        /// use  the set_escdelay  or set_tabsize  functions.Other
        /// global variables are not modifiable.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void set_escdelay(int size)
        {
            NCursesException.Verify(NCursesWrapper.set_escdelay(size), "set_escdelay");
        }
        #endregion

        #region set_tabsize
        /// <summary>
        /// The ESCDELAY and TABSIZE global variables are modified by
        /// some applications.To modify them in  any configuration,
        /// use  the set_escdelay  or set_tabsize  functions.Other
        /// global variables are not modifiable.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void set_tabsize(int size)
        {
            NCursesException.Verify(NCursesWrapper.set_tabsize(size), "set_tabsize");
        }
        #endregion

        #region use_default_colors
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
            NCursesException.Verify(NCursesWrapper.use_default_colors(), "use_default_colors");
        }
        #endregion

        #region use_extended_names
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
            return NCursesWrapper.use_extended_names(enable);
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
        public static int use_legacy_coding(int level)
        {
            return NCursesWrapper.use_legacy_coding(level);
        }
        #endregion

        #region nofilter
        /// <summary>
        /// The nofilter  routine cancels  the effect of a preceding
        /// filter call.That allows  the caller  to initialize  a
        /// screen  on a different device, using a different value of
        /// $TERM.The limitation arises because the filter  routine
        /// modifies the in-memory copy of the terminal information.
        /// </summary>
        public static void nofilter()
        {
            NCursesWrapper.nofilter();
        }
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
        /// The erasewchar routine stores the current erase character
        /// in  the location referenced by ch.If no erase character
        /// has been defined, the routine fails and the location  referenced by ch is not changed.
        /// see <see cref="erasechar"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">a reference to store the erase char</param>
        public static void erasewchar(out char wch)
        {
            WideStrCursesWrapper.erasewchar(out wch);
        }
        #endregion

        #region getcchar
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
        public static void getcchar(in INCursesWCHAR wcval, out char wch, out ulong attrs, out short color_pair)
        {
            WideNCursesWrapper.getcchar(wcval, out wch, out attrs, out color_pair);
        }
        #endregion

        #region key_name
        /// <summary>
        /// The keyname routine returns a character string corresponding to the key <paramref name="c"/>
        /// </summary>
        /// <param name="c">code of the key</param>
        /// <returns>a string representing the key code</returns>
        public static string key_name(in char c)
        {
            return WideStrCursesWrapper.key_name(c);
        }
        #endregion

        #region killwchar
        /// <summary>
        /// The killwchar routine stores the current line-kill character in the location referenced by  ch.If no  line-kill
        /// character  has been  defined,  the routine fails and the
        /// location referenced by ch is not changed.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">a reference to store the kill char</param>
        public static void killwchar(out char wch)
        {
            WideStrCursesWrapper.killwchar(out wch);
        }
        #endregion

        #region setcchar
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
        /// allowed. Color attributes will be OR'd into <see cref="NCursesWCHAR.attr"/> .
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">the NCURSES_CH_T to get all properties from</param>
        /// <param name="wch">a reference to store the string</param>
        /// <param name="attrs">a reference to store the attributes in</param>
        /// <param name="color_pair">a reference to store the color pair in</param>
        public static void setcchar(out INCursesWCHAR wcval, in char wch, ulong attrs, short color_pair)
        {
            WideNCursesWrapper.setcchar(out wcval, wch, attrs, color_pair);
        }
        #endregion

        #region slk_wset
        /// <summary>
        /// <see cref="slk_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void slk_wset(int labnum, in string label, int fmt)
        {
            WideStrCursesWrapper.slk_wset(labnum, label, fmt);
        }
        #endregion

        #region term_attrs
        /// <summary>
        /// see <see cref="termattrs"/>
        /// </summary>
        public static ulong term_attrs()
        {
            return SmallCursesWrapper.term_attrs();
        }
        #endregion

        #region unget_wch
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
        public static void unget_wch(in char wch)
        {
            WideStrCursesWrapper.unget_wch(wch);
        }
        #endregion

        #region vid_attr
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
        public static void vid_attr(ulong attrs, short pair)
        {
            SmallCursesWrapper.vid_attr(attrs, pair);
        }
        #endregion

        #region vid_puts
        /// <summary>
        /// see <see cref="vid_attr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void vid_puts(ulong attrs, short pair, Func<int, int> NCURSES_OUTC)
        {
            SmallCursesWrapper.vid_puts(attrs, pair, NCURSES_OUTC);
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
        public static void unctrl(in INCursesSCHAR ch, out string str)
        {
            SmallCursesWrapper.unctrl(ch, out str);
        }
        #endregion

        #region wunctrl
        /// <summary>
        /// see <see cref="unctrl"/>
        /// </summary>
        /// <returns>printable representation of the character</returns>
        public static void wunctrl(in INCursesWCHAR wch, out string str)
        {
            WideNCursesWrapper.wunctrl(wch, out str);
        }
        #endregion

        #region has_mouse
        /// <summary>
        /// The has_mouse  function returns TRUE if the mouse driver has been successfully initialized.
        /// </summary>
        /// <returns>true if terminal supports insert/delete characters</returns>
        public static bool has_mouse()
        {
            return NCursesWrapper.has_mouse();
        }
        #endregion

        #region getmouse
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
        public static void getmouse(out IMEVENT ev)
        {
            SmallCursesWrapper.getmouse(out ev);
        }
        #endregion

        #region ungetmouse        
        /// <summary>
        /// The ungetmouse function behaves  analogously to  ungetch.
        /// It pushes a KEY_MOUSE event onto the input queue, and associates with that event the given state data and
        /// screen-relative character-cell coordinates.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void ungetmouse(in IMEVENT ev)
        {
            SmallCursesWrapper.ungetmouse(ev);
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
        public static ulong mousemask(ulong newmask, out ulong oldmask)
        {
            return SmallCursesWrapper.mousemask(newmask, out oldmask);
        }
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
        public static int mouseinterval(int erval)
        {
            return NCursesWrapper.mouseinterval(erval);
        }
        #endregion

        #region mouse_trafo
        /// <summary>
        /// see <see cref="NativeWindow.wmouse_trafo"/>
        /// </summary>
        public static bool mouse_trafo(ref int pY, ref int pX, bool to_screen)
        {
            return NCursesWrapper.mouse_trafo(ref pY, ref pX, to_screen);
        }
        #endregion

        #region has_key
        /// <summary>
        /// The has_key routine takes a key-code value from the above
        /// list, and returns TRUE or FALSE according to  whether the
        /// current terminal type recognizes a key with that value.
        /// this function doesn't work on windows, use the managed alternative <see cref="HasKey(int, out Key)"/>
        /// </summary>
        /// <param name="ch">the key code you want to test</param>
        /// <returns>true or false</returns>
        public static bool has_key(int ch, out Key key)
        {
            if (NCursesWrapper.has_key(ch))
            {
                key = (Key)ch;
                return true;
            }

            key = 0;
            return false;
        }
        #endregion

        /// <summary>
        /// check if the current terminal is unicode-able
        /// </summary>
        /// <returns>true or false</returns>
        public static bool _nc_unicode_locale()
        {
            return NCursesWrapper._nc_unicode_locale();
        }

        /// <summary>
        /// returns the screen of the window
        /// </summary>
        /// <param name="window">pointer to a window</param>
        /// <returns>reference to a screen</returns>
        public static IntPtr _nc_screen_of(IntPtr window)
        {
            return NCursesWrapper._nc_screen_of(window);
        }
    }
}
