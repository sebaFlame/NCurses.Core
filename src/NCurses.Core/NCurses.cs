using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using NCurses.Core.Interop;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Wrappers;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.Panel;
using NCurses.Core.StdScr;

namespace NCurses.Core
{
    //TODO: create shared lock (dictionary?)
    public static class NCurses
    {
        public static bool UnicodeSupported => NativeNCurses.HasUnicodeSupport;

        public static IWindow StdScr { get; private set; }

        internal static INativeNCursesWrapper<
            IMultiByteNCursesChar,
            IMultiByteNCursesCharString,
            IMultiByteChar,
            IMultiByteCharString,
            ISingleByteNCursesChar,
            ISingleByteNCursesCharString,
            ISingleByteChar,
            ISingleByteCharString,
            IMEVENT> NCursesWrapper => NativeNCurses.NCurses;

        internal static IWindowFactory WindowFactory => NativeNCurses.WindowFactory;

        private static int _ColorBitShift;
        private static byte _ColorBitMask;
        private static int _BgBitShift;

        #region init/end
        /// <summary>
        /// start the NCurses subsystem (auto starts if you're using Windows/Pads)
        /// </summary>
        /// <returns>the standard screen</returns>
        public static IWindow Start()
        {
            if (!(StdScr is null))
            {
                return StdScr;
            }

            StdScrSafeHandle stdScrSafehandle = NCursesWrapper.initscr();

            StdScr = CreateStdScr(stdScrSafehandle);

            return StdScr;
        }

        /// <summary>
        /// End the NCurses subsystem.
        /// It keeps current windows/screens for your next call to <see cref="Start"/>.
        /// </summary>
        public static void End()
        {
            if (StdScr is null)
            {
                return;
            }

            if (WindowFactory.HasPanels)
            {
                throw new NotSupportedException("Dispose of all panels before calling End()");
            }

            if (WindowFactory.HasWindows)
            {
                throw new NotSupportedException("Dispose of all windows before calling End()");
            }

            NCursesWrapper.endwin();

            StdScr = null;
        }
        #endregion

        #region StdScr Creation
        private static IWindow CreateStdScr(StdScrSafeHandle stdScrSafeHandle) => WindowFactory.CreateStdScr(stdScrSafeHandle);

        private static IWindow CreateStdScr(IWindow existingStdScr) => WindowFactory.CreateStdScr(existingStdScr);
        #endregion

        #region Window Creation
        public static IWindow CreateWindow(int nlines, int ncols, int begy, int begx)
        {
            Start();
            return WindowFactory.CreateWindow(nlines, ncols, begy, begx);
        }

        public static IWindow CreateWindow(int nlines, int ncols)
        {
            Start();
            return WindowFactory.CreateWindow(nlines, ncols);
        }

        public static IWindow CreateWindow()
        {
            Start();
            return WindowFactory.CreateWindow();
        }

        private static IWindow CreateWindow(WindowBaseSafeHandle windowBaseSafeHandle)
        {
            return WindowFactory.CreateWindow(windowBaseSafeHandle);
        }
        #endregion

        #region Pad Creation
        /// <summary>
        /// Create a new pad according to the current window Unicode support
        /// </summary>
        /// <param name="window"></param>
        /// <param name="nlines"></param>
        /// <param name="ncols"></param>
        /// <returns></returns>
        public static IPad CreatePad(IWindow window, int nlines, int ncols)
        {
            return WindowFactory.CreatePad(window, nlines, ncols);
        }

        public static IPad CreatePad(int nlines, int ncols)
        {
            Start();
            return WindowFactory.CreatePad(nlines, ncols);
        }
        #endregion

        #region Panel Creation
        public static IPanel CreatePanel(IWindow window)
        {
            return WindowFactory.CreatePanel(window);
        }

        public static void UpdatePanels()
        {
            NativePanel.update_panels();
        }
        #endregion

        /// <summary>
        /// rip off a line from the bottom (-1) or top (1) of the screen
        /// call this method before <see cref="Start"/>
        /// </summary>
        /// <param name="direction">-1 for bottom, 1 for top</param>
        /// <param name="assignWindowDelegate">a method to assign the ripped off line during <see cref="Start"/>, also gets passed the amount of columns</param>
        public static void RipOffLine(int direction, RipoffDelegate assignWindowDelegate)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new InvalidOperationException("RipOffLine is not supported on windows.");
            }

            NativeNCurses.ripoffline(direction, assignWindowDelegate);
        }

        /// <summary>
        /// Update the current screen after (a) call(s) to <see cref="WindowBase.NoOutRefresh"/> to prevent flicker.
        /// </summary>
        public static void Update()
        {
            NCursesWrapper.doupdate();
        }

        /// <summary>
        /// resize the screen to <paramref name="lines"/> and <paramref name="columns"/>
        /// </summary>
        /// <param name="lines">number of lines to resize to</param>
        /// <param name="columns">number of columns to resize to</param>
        public static void Resize(int lines, int columns)
        {
            NCursesWrapper.resizeterm(lines, columns);
        }

        /// <summary>
        /// enable/disable echoing read character to console
        /// </summary>
        public static bool Echo
        {
            set
            {
                if (value)
                {
                    NCursesWrapper.echo();
                }
                else
                {
                    NCursesWrapper.noecho();
                }
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
                {
                    NCursesWrapper.cbreak();
                }
                else
                {
                    NCursesWrapper.nocbreak();
                }
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
                {
                    NCursesWrapper.nl();
                }
                else
                {
                    NCursesWrapper.nonl();
                }
            }
        }

        public static bool Raw
        {
            set
            {
                if (value)
                {
                    NCursesWrapper.raw();
                }
                else
                {
                    NCursesWrapper.noraw();
                }
            }
        }

        public static bool FlushOnInterrupt
        {
            set
            {
                NCursesWrapper.intrflush(value);
            }
        }

        public static int CursorVisibility
        {
            set
            {
                NCursesWrapper.curs_set(value);
            }
        }

        #region thread safety
        /// <summary>
        /// enable/disable locking globally on the lowest level
        /// can be used by using <see cref="NCurses.CreateThreadSafeDisposable"/>
        /// </summary>
        public static bool EnableLocking
        {
            get
            {
                lock (NativeNCurses.SyncRoot)
                {
                    return NativeNCurses.EnableLocking;
                }
            }
            set
            {
                lock (NativeNCurses.SyncRoot)
                {
                    NativeNCurses.EnableLocking = value;
                }
            }
        }

        /// <summary>
        /// Create a disposable object to guarantee thread safety until disposal
        /// Can be used to group several actions as 1
        /// This will enable (and lock) <see cref="NCurses.EnableLocking"/> until disposal
        /// Be sure to dispose or you'll be locked out of all NCurses functions
        /// </summary>
        /// <example>
        /// <code>
        /// using(NCurses.GetThreadSafeDisposable()) //do something with NCurses
        /// </code>
        /// </example>
        /// <returns></returns>
        public static IDisposable CreateThreadSafeDisposable()
        {
            return new InternalLockDisposable(NativeNCurses.EnableLocking);
        }
        #endregion

        #region color
        /// <summary>
        /// check if the console supports colors
        /// </summary>
        public static bool HasColor
        {
            get { return NCursesWrapper.has_colors(); }
        }

        /// <summary>
        /// Initialize NCurses colors
        /// </summary>
        public static void StartColor()
        {
            NCursesWrapper.start_color();
        }

        /// <summary>
        /// Assigns the default terminal colors to pair -1.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">When not supported on the current platform</exception>
        public static void UseDefaultColors()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                throw new PlatformNotSupportedException("UseDefaultColors is not supported on windows.");
            }

            NCursesWrapper.use_default_colors();
        }

        /// <summary>
        /// Redefines colors at pair index 0. Use -1 to use default terminal colors.
        /// </summary>
        /// <param name="foreGround">the foreground color to assign</param>
        /// <param name="backGround">the background color to assign</param>
        public static void AssumeDefaultColors(short foreGround, short backGround)
        {
            NCursesWrapper.assume_default_colors(foreGround, backGround);
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
        public static void InitPair(ushort pairIndex, short foreGround, short backGround)
        {
            if (pairIndex > Int16.MaxValue)
            {
                NCursesWrapper.init_extended_pair(pairIndex, foreGround, backGround);
            }
            else
            {
                NCursesWrapper.init_pair((short)pairIndex, foreGround, backGround);
            }
        }

        /// <summary>
        /// see <see cref="InitPair(short, short, short)"/> 
        /// </summary>
        public static void InitPair(ushort pairIndex, Color foreGround, Color backGround)
        {
            InitPair(pairIndex, (short)foreGround, (short)backGround);
        }

        /// <summary>
        /// If you want to use default colors, this method initializes default pairs with all default color combinations
        /// </summary>
        /// <returns>The number of colors which got initialized</returns>
        public static int InitDefaultPairs()
        {
            /*
                Colors
                0 black     4 blue      8 dark gray         12 bright blue
                1 red       5 magenta   9 bright red        13 bright magenta
                2 green     6 cyan      10 bright green     14 bright cyan
                3 yellow    7 white     11 bright yellow    15 bright white

                Default color: -1
            */

            //initialize default colors
            short startFg = -1;
            short startBg = -1;
            try
            {
                UseDefaultColors();
            }
            catch (PlatformNotSupportedException)
            {
                //no default colors!
                startFg = 0;
                startBg = 0;
            }

            int colorCount = NCursesWrapper.COLORS();
            int pairCount = NCursesWrapper.COLOR_PAIRS();

            //only support upto 16 default colors
            if (colorCount > 16)
            {
                colorCount = 16;
            }

            if (pairCount < colorCount * colorCount)
            {
                colorCount = (int)Math.Sqrt(pairCount);
            }

            int colorMostSignificantBit = SetBitNumber(colorCount - 1);
            _ColorBitShift = FindPosition(colorMostSignificantBit);

            //set extra bit to account for default colors
            if (startFg < 0)
            {
                _ColorBitShift++;
            }
            else
            {
                /* if no default color support
                 * shift to left, so you get 0 (black) */
                _BgBitShift = _ColorBitShift;
            }

            _ColorBitMask = (byte)(byte.MaxValue >> (8 - _ColorBitShift));

            ushort pairIndex = 0;
            for (short bg = startBg ; bg < colorCount; bg++)
            {
                for (short fg = startFg ; fg < colorCount; fg++)
                {
                    pairIndex = ComputeDefaultColorPair(fg, bg);

                    InitPair
                    (
                        pairIndex,
                        fg,
                        bg
                    );
                }
            }

            return colorCount;
        }

        /// <summary>
        /// Compute the color pair referencing <paramref name="fg"/> and <paramref name="bg"/>
        /// Using this method upto 127 colors are supported (127 * 127 pairs)
        /// You can only use upto 256 pairs on single byte chars (windows)!
        /// </summary>
        /// <param name="fg">Foreground color (-1 for default)</param>
        /// <param name="bg">Background color (-1 for default)</param>
        /// <returns>The pair number of the combined foreground and background color</returns>
        public static ushort ComputeDefaultColorPair(short fg, short bg)
        {
            if (_ColorBitShift == 0)
            {
                throw new InvalidOperationException("Color bit shift has not been set!");
            }

            ushort pairIndex = 0;
            ushort cFg, cBg, mFg, mBg;

            unchecked
            {
                cFg = (ushort)fg;
                cBg = (ushort)bg;
            }

            mFg = (ushort)(cFg & _ColorBitMask);

            if (cBg - (mBg = (ushort)(cBg & _ColorBitMask)) > 0)
            {
                mBg = (ushort)(mBg >> _BgBitShift);
            }

            pairIndex = (ushort)(mFg | (mBg << _ColorBitShift));

            //account for reserved pair 0
            return ++pairIndex;
        }

        private static int SetBitNumber(int n)
        {
            n |= n >> 1;
            n |= n >> 2;

            n |= n >> 4;
            n |= n >> 8;
            n |= n >> 16;

            n = n + 1;

            return (n >> 1);
        }

        private static bool IsPowerOfTwo(int n)
        {
            return n > 0 && ((n & (n - 1)) == 0);
        }

        // Returns position of the only set bit in 'n'
        private static int FindPosition(int n)
        {
            if (!IsPowerOfTwo(n))
            {
                return -1;
            }

            int count = 0;

            // One by one move the only set bit
            // to right till it reaches end
            while (n > 0)
            {
                n = n >> 1;

                // increment count of shifts
                ++count;
            }

            return count;
        }
        public static bool CanChangeColor => NCursesWrapper.can_change_color();

        /// <summary>
        /// Initialize a color with a RGB value
        /// </summary>
        /// <param name="color">The number of the color to change (can't be 0-7)</param>
        /// <param name="red">The amount of red in the range 0 through 1000</param>
        /// <param name="green">The amount of green in the range 0 through 1000</param>
        /// <param name="blue">The amount of blue in the range 0 through 1000</param>
        public static void InitColor(short color, short red, short green, short blue)
        {
            NCursesWrapper.init_color(color, red, green, blue);
        }

        /// <summary>
        /// see <see cref="InitColor(short, short, short, short)"/> 
        /// </summary>
        public static void InitColor(Color color, short red, short green, short blue)
        {
            NCursesWrapper.init_color((short)color, red, green, blue);
        }

        /// <summary>
        /// Returns the number of available color pairs
        /// </summary>
        public static int ColorPairs
        {
           get { return NCursesWrapper.COLOR_PAIRS(); }
        }

        /// <summary>
        /// returns the color attribute by pair index
        /// </summary>
        /// <param name="pairIndex">the pair index for which you want the attribute</param>
        /// <returns>attribute of the color</returns>
        public static ulong ColorPair(short pairIndex)
        {
            return (ulong)NativeNCurses.COLOR_PAIR(pairIndex);
        }

        /// <summary>
        /// returns the pair index by (color) attribute
        /// </summary>
        /// <param name="attrs">the (color) attribute we want the pair index for</param>
        /// <returns>pair index of the color attribute</returns>
        public static int PairNumber(uint attrs)
        {
            return NativeNCurses.PAIR_NUMBER(attrs);
        }

        public static int Colors
        {
            get { return NCursesWrapper.COLORS(); }
        }
        #endregion

        #region mouse
        public static bool HasMouse => NCursesWrapper.has_mouse();

        /// <summary>
        /// enable the reporting of mouse events
        /// </summary>
        /// <param name="mouseMask">mouse events you want to enable</param>
        /// <param name="oldMouseMask">mouse event which were already enabled</param>
        /// <returns>the enabled mouse mask</returns>
        public static ulong EnableMouseMask(ulong mouseMask, out ulong oldMouseMask)
        {
            return NCursesWrapper.mousemask(mouseMask, out oldMouseMask);
        }

        /// <summary>
        /// gets the last mouse event
        /// </summary>
        /// <returns>the last mouse event</returns>
        public static void GetMouseEvent(out IMEVENT mouseEvent)
        {
            NCursesWrapper.getmouse(out mouseEvent);
        }
        #endregion

        /// <summary>
        /// Restores the terminal to the previous state
        /// </summary>
        public static void ResetProgramMode()
        {
            NCursesWrapper.reset_prog_mode();
        }

        /// <summary>
        /// Restores the terminal to the previous state
        /// </summary>
        public static void ResetShellMode()
        {
            NCursesWrapper.reset_shell_mode();
        }
    }
}
