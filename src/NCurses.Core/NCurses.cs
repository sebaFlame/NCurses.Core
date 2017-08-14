using System;
using System.Runtime.InteropServices;
using System.Text;
using NCurses.Core.Interop;

#if NCURSES_VERSION_6
using chtype = System.UInt32;
#elif NCURSES_VERSION_5
using chtype = System.UInt64;
#endif

namespace NCurses.Core
{
    public static class NCurses
    {
        public static Window stdscr = null;
        internal static bool UnicodeSupported;

        #region init/end
        /// <summary>
        /// start the NCurses subsystem (auto starts if you're using Windows/Pads)
        /// </summary>
        /// <returns>the standard screen</returns>
        public static Window Start()
        {
            if (stdscr != null)
                throw new InvalidOperationException("NCurses was already initialized");

            stdscr = new Window(NativeNCurses.initscr(), false);
            UnicodeSupported = NativeNCurses._nc_unicode_locale();

            return stdscr;
        }

        /// <summary>
        /// End the NCurses subsystem.
        /// It keeps current windows/screens for your next call to <see cref="Start"/>.
        /// </summary>
        public static void End()
        {
            if(stdscr == null)
                throw new InvalidOperationException("NCurses not initialized yet");

            stdscr.Dispose();
            NativeNCurses.endwin();
            stdscr = null;
        }
        #endregion

        /// <summary>
        /// rip off a line from the bottom (-1) or top (1) of the screen
        /// call this method before <see cref="Start"/>
        /// </summary>
        /// <param name="direction">-1 for bottom, 1 for top</param>
        /// <param name="assignWindow">a method to assign the ripped off line during <see cref="Start"/>, also gets passed the amount of columns</param>
        public static void RipOffLine(int direction, Action<Window, int> assignWindow)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new InvalidOperationException("RipOffLine is not supported on windows.");

            Func<IntPtr, int, IntPtr, int> initCallback = (IntPtr win, int cols, IntPtr func) =>
            {
                assignWindow(new Window(win, true), cols);
                return Constants.OK;
            };
            NativeNCurses.ripoffline(direction, initCallback);
        }

        /// <summary>
        /// Update the current screen after (a) call(s) to <see cref="WindowBase.NoOutRefresh"/> to prevent flicker.
        /// </summary>
        public static void Update()
        {
            NativeNCurses.doupdate();
        }

        /// <summary>
        /// resize the screen to <paramref name="lines"/> and <paramref name="columns"/>
        /// </summary>
        /// <param name="lines">number of lines to resize to</param>
        /// <param name="columns">number of columns to resize to</param>
        public static void Resize(int lines, int columns)
        {
            NativeNCurses.resizeterm(lines, columns);
        }

        /// <summary>
        /// set the font for the current screen (only available on windows)
        /// </summary>
        /// <param name="font">the font to use</param>
        public static void SetFont(WindowsConsoleFont font)
        {
            NativeNCurses.SetConsoleFont(font);
        }

        /// <summary>
        /// enable/disable echoing read character to console
        /// </summary>
        public static bool Echo
        {
            set
            {
                if (value)
                    NativeNCurses.echo();
                else
                    NativeNCurses.noecho();
            }
        }

        /// <summary>
        /// disable/enable character buffering by TTY
        /// </summary>
        public static bool CBreak
        {
            set
            {
                if (value)
                    NativeNCurses.cbreak();
                else
                    NativeNCurses.nocbreak();
            }
        }

        /// <summary>
        /// enable/disable translating return key into newline on input
        /// </summary>
        public static bool NewLine
        {
            set
            {
                if (value)
                    NativeNCurses.nl();
                else
                    NativeNCurses.nonl();
            }
        }

        public static bool Meta
        {
            set
            {
                NativeNCurses.meta(IntPtr.Zero, value);
            }
        }

        public static bool Raw
        {
            set
            {
                if (value)
                    NativeNCurses.raw();
                else
                    NativeNCurses.noraw();
            }
        }

        public static bool FlushOnInterrupt
        {
            set
            {
                NativeNCurses.intrflush(IntPtr.Zero, value);
            }
        }

        public static int CursorVisibility
        {
            set
            {
                NativeNCurses.curs_set(value);
            }
        }

        #region color
        /// <summary>
        /// check if the console supports colors
        /// </summary>
        public static bool HasColor
        {
            get { return NativeNCurses.has_colors(); }
        }

        /// <summary>
        /// Initialize NCurses colors
        /// </summary>
        public static void StartColor()
        {
            NativeNCurses.start_color();
        }

        /// <summary>
        /// Assigns the default terminal colors to pair -1.
        /// </summary>
        public static void UseDefaultColors()
        {
            NativeNCurses.use_default_colors();
        }

        /// <summary>
        /// Redefines colors at pair index 0. Use -1 to use default terminal colors.
        /// </summary>
        /// <param name="foreGround">the foreground color to assign</param>
        /// <param name="backGround">the background color to assign</param>
        public static void AssumeDefaultColors(short foreGround, short backGround)
        {
            NativeNCurses.assume_default_colors(foreGround, backGround);
        }

        /// <summary>
        /// see <see cref="AssumeDefaultColors(short, short)"/> 
        /// </summary>
        public static void AssumeDefaultColors(Color foreGround, Color backGround)
        {
            AssumeDefaultColors((short)foreGround, (short)backGround);
        }

        /// <summary>
        /// Initialize a <paramref name="pairIndex"/> to a combination of <paramref name="foreGround"/> and <paramref name="backGround"/> color
        /// </summary>
        /// <param name="pairIndex">the pair index to initialize</param>
        /// <param name="foreGround">the foreground color to use</param>
        /// <param name="backGround">the background color to use</param>
        public static void InitPair(short pairIndex, short foreGround, short backGround)
        {
            NativeNCurses.init_pair(pairIndex, foreGround, backGround);
        }

        /// <summary>
        /// see <see cref="InitPair(short, short, short)"/> 
        /// </summary>
        public static void InitPair(short pairIndex, Color foreGround, Color backGround)
        {
            InitPair(pairIndex, (short)foreGround, (short)backGround);
        }

        /// <summary>
        /// if you want to use default colors, this method initializes default pairs with all default color combinations
        /// </summary>
        public static void InitDefaultPairs()
        {
            int colorCount = NativeNCurses.COLORS();
            int pairCount = NativeNCurses.COLOR_PAIRS();

            short pairIndex = 0;
            for (short i = 0; i < colorCount; i++)
            {
                for (short j = (short)(colorCount - 1); j >= 0; j--)
                {
                    if (pairIndex == 0)
                    {
                        pairIndex++;
                        continue;
                    }
                    NativeNCurses.init_pair(pairIndex++, j, i);
                }
            }
        }

        /// <summary>
        /// Initialize a color with a RGB value
        /// </summary>
        /// <param name="color">The number of the color to change (can't be 0-7)</param>
        /// <param name="red">The amount of red in the range 0 through 1000</param>
        /// <param name="green">The amount of green in the range 0 through 1000</param>
        /// <param name="blue">The amount of blue in the range 0 through 1000</param>
        public static void InitColor(short color, short red, short green, short blue)
        {
            NativeNCurses.init_color(color, red, green, blue);
        }

        /// <summary>
        /// see <see cref="InitColor(short, short, short, short)"/> 
        /// </summary>
        public static void InitColor(Color color, short red, short green, short blue)
        {
            NativeNCurses.init_color((short)color, red, green, blue);
        }

        /// <summary>
        /// Returns the number of available color pairs
        /// </summary>
        public static int ColorPairs
        {
           get { return NativeNCurses.COLOR_PAIRS(); }
        }

        /// <summary>
        /// returns the color attribute by pair index
        /// </summary>
        /// <param name="pairIndex">the pair index for which you want the attribute</param>
        /// <returns>attribute of the color</returns>
        public static ulong ColorPair(short pairIndex)
        {
            return (chtype)NativeNCurses.COLOR_PAIR(pairIndex);
        }

        public static int Colors
        {
            get { return NativeNCurses.COLORS(); }
        }
        #endregion

        #region NCURSES_CH_T
        /// <summary>
        /// get the string representation of a wide char with its attributes and color pair index
        /// </summary>
        /// <param name="wch">the wide char you want to convert</param>
        /// <param name="attrs">will contain the attributes of the wide char</param>
        /// <param name="pairIndex">will contain the color pair index of the wide char</param>
        /// <returns>string containing the wide char</returns>
        public static string GetStringFromWideChar(NCursesWCHAR wch, out ulong attrs, out short pairIndex)
        {
            StringBuilder builder = new StringBuilder(Constants.CCHARW_MAX);
#if NCURSES_VERSION_5
            NativeNCurses.getcchar(wch, builder, out attrs, out pairIndex);
#elif NCURSES_VERSION_6
            chtype attrs_1;
            NativeNCurses.getcchar(wch, builder, out attrs_1, out pairIndex);
            attrs = (ulong)attrs_1;
#endif
            return builder.ToString();
        }

        /// <summary>
        /// get the wide char representation of a string with attributes and color pair index
        /// </summary>
        /// <param name="wStr">the string you want to convert</param>
        /// <param name="attrs">the attributes you want to apply to the string</param>
        /// <param name="pairIndex">the color pair index you want to apply to the string</param>
        /// <returns>the wide char represenation of the string</returns>
        public static NCursesWCHAR GetWideCharFromString(string wStr, ulong attrs, short pairIndex)
        {
            NCursesWCHAR wch;
#if NCURSES_VERSION_5
            NativeNCurses.setcchar(out wch, wStr, attrs, 4);
#elif NCURSES_VERSION_6
            NativeNCurses.setcchar(out wch, wStr, (chtype)attrs, 4);
#endif
            return wch;
        }
#endregion

#region mouse
        /// <summary>
        /// enable the reporting of mouse events
        /// </summary>
        /// <param name="mouseMask">mouse events you want to enable</param>
        /// <param name="oldMouseMask">mouse event which were already enabled</param>
        /// <returns>the enabled mouse mask</returns>
        public static ulong EnableMouseMask(ulong mouseMask, out ulong oldMouseMask)
        {
#if NCURSES_VERSION_5
            return NativeNCurses.mousemask(mouseMask, out oldMouseMask);
#elif NCURSES_VERSION_6
            chtype oldMouseMask_1;
            chtype ret = NativeNCurses.mousemask((chtype)mouseMask, out oldMouseMask_1);
            oldMouseMask = (ulong)oldMouseMask_1;
            return ret;
#endif
        }

        /// <summary>
        /// gets the last mouse event
        /// </summary>
        /// <returns>the last mouse event</returns>
        public static MEVENT GetMouseEvent()
        {
            MEVENT mouseEvent;
            NativeNCurses.getmouse(out mouseEvent);
            return mouseEvent;
        }
#endregion

#region keymap
        /// <summary>
        /// checks if a function key has been pressed
        /// </summary>
        /// <param name="ch">the character you want check</param>
        /// <param name="key">the returned key</param>
        /// <returns>true if a function key has been pressed</returns>
        public static bool GetKey(int ch, out Key key)
        {
            key = 0;
            if (Enum.IsDefined(typeof(Key), (short)ch))
            {
                key = (Key)ch;
                return true;
            }
            return false;
        }

        public static bool IsCtrlKey(int ch, out string keyName)
        {
            keyName = NativeNCurses.keyname(ch);
            if (keyName.StartsWith("^"))
                return true;
            return false;
        }
#endregion

        /// <summary>
        /// Restores the terminal to the previous state
        /// </summary>
        public static void ResetProgramMode()
        {
            NativeNCurses.reset_prog_mode();
        }

        /// <summary>
        /// Restores the terminal to the previous state
        /// </summary>
        public static void ResetShellMode()
        {
            NativeNCurses.reset_shell_mode();
        }
    }
}
