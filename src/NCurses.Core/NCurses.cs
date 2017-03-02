using System;
using System.Runtime.InteropServices;
using System.Text;
using NCurses.Core.Interop;

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
            Func<IntPtr, int, IntPtr, int> initCallback = (IntPtr win, int cols, IntPtr func) =>
            {
                assignWindow(new Window(win, true), cols);
                if(func != IntPtr.Zero)
                    Marshal.FreeHGlobal(func);
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
        public static uint ColorPair(short pairIndex)
        {
            return (uint)NativeNCurses.COLOR_PAIR(pairIndex);
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
        public static string GetStringFromWideChar(NCURSES_CH_T wch, out uint attrs, out short pairIndex)
        {
            StringBuilder builder = new StringBuilder(5);
            NativeNCurses.getcchar(wch, builder, out attrs, out pairIndex);
            return builder.ToString();
        }

        /// <summary>
        /// get the wide char representation of a string with attributes and color pair index
        /// </summary>
        /// <param name="wStr">the string you want to convert</param>
        /// <param name="attrs">the attributes you want to apply to the string</param>
        /// <param name="pairIndex">the color pair index you want to apply to the string</param>
        /// <returns>the wide char represenation of the string</returns>
        public static NCURSES_CH_T GetWideCharFromString(string wStr, uint attrs, short pairIndex)
        {
            NCURSES_CH_T wch;
            NativeNCurses.setcchar(out wch, wStr, Attrs.BOLD, 4);
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
        public static uint EnableMouseMask(uint mouseMask, out uint oldMouseMask)
        {
            return NativeNCurses.mousemask(mouseMask, out oldMouseMask);
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
        public static bool GetKeyName(int ch, out Key key)
        {
            string keyName = NativeNCurses.keyname(ch);
            if (Enum.TryParse(keyName, out key))
                return true;
            return false;
        }
        #endregion
    }
}
