using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

#if NCURSES_VERSION_6
using chtype = System.UInt32;
#elif NCURSES_VERSION_5
using chtype = System.UInt64;
#endif

namespace NCurses.Core.Interop
{
    internal enum Handles
    {
        STD_INPUT = -10,
        STD_OUTPUT = -11,
        STD_ERROR = -12
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct KeyEventRecord
    {
        [FieldOffset(0)]
        internal bool bKeyDown;
        [FieldOffset(4)]
        internal short wRepeatCount;
        [FieldOffset(6)]
        internal short wVirtualKeyCode;
        [FieldOffset(8)]
        internal short wVirtualScanCode;
        [FieldOffset(10)]
        internal char uChar; // Union between WCHAR and ASCII char
        [FieldOffset(12)]
        internal int dwControlKeyState;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct MouseEventRecord
    {
        [FieldOffset(0)]
        internal Coord dwMousePosition;
        [FieldOffset(4)]
        internal int dwButtonState;
        [FieldOffset(8)]
        internal int dwControlKeyState;
        [FieldOffset(12)]
        internal int dwEventFlags;
    }

    // Really, this is a union of KeyEventRecords and other types.
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    internal struct InputRecord
    {
        [FieldOffset(0)]
        internal short EventType;
        [FieldOffset(4)]
        internal KeyEventRecord KeyEvent;
        [FieldOffset(4)]
        internal MouseEventRecord MouseEvent;
        // This struct is a union!  Word alighment should take care of padding!
    }

    #region structs for windows overrides
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct CONSOLE_FONT_INFO_EX
    {
        public uint cbSize;
        public uint nFont;
        public Coord dwFontSize;
        public int FontFamily;
        public int FontWeight;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FaceName;
    }

    internal struct SmallRect
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;

        internal SmallRect(int left, int top, int right, int bottom)
        {
            Left = (short)left;
            Top = (short)top;
            Right = (short)right;
            Bottom = (short)bottom;
        }
    }

    internal struct Coord
    {
        public short X;
        public short Y;

        public Coord(int x, int y)
        {
            X = (short)x;
            Y = (short)y;
        }
    }
    #endregion

    internal enum ControlKeyState : short
    {
        RIGHT_ALT_PRESSED = 0x0001,
        LEFT_ALT_PRESSED = 0x0002,
        RIGHT_CTRL_PRESSED = 0x0004,
        LEFT_CTRL_PRESSED = 0x0008,
        SHIFT_PRESSED = 0x0010,
        NUMLOCK_ON = 0x0020,
        SCROLLLOCK_ON = 0x0040,
        CAPSLOCK_ON = 0x0080,
        ENHANCED_KEY = 0x0100
    }

    internal enum MouseButtonState : short
    {
        FROM_LEFT_1ST_BUTTON_PRESSED = 1,
        FROM_LEFT_2ND_BUTTON_PRESSED = 4,
        FROM_LEFT_3RD_BUTTON_PRESSED = 8,
        FROM_LEFT_4TH_BUTTON_PRESSED = 16,
        RIGHTMOST_BUTTON_PRESSED = 2
    }

    internal enum MouseEventFlags : short
    {
        MOUSE_MOVED = 0x0001,
        DOUBLE_CLICK = 0x0002,
        MOUSE_WHEELED = 0x0004,
        MOUSE_HWHEELED = 0x0008
    }

    internal enum VirtualKey : byte
    {
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_DELETE = 0x2E,
        VK_INSERT = 0x2D,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_F1 = 0x70,
        VK_F12 = 0x7B
    }

    public enum WindowsConsoleFont
    {
        TERMINAL,
        LUCIDA,
        CONSOLAS
    }

    internal static class NativeWindows
    {
        internal const short KEY_EVENT = 0X0001;
        internal const short MOUSE_EVENT = 0X0002;
        internal const short WINDOW_BUFFER_SIZE_EVENT = 0x0004;
        internal const short MENU_EVENT = 0X0008;
        internal const short FOCUS_EVENT = 0X0010;

        internal const short AltVKCode = 0x12;
        internal const int BUTTON_MASK = (int)MouseButtonState.FROM_LEFT_1ST_BUTTON_PRESSED |
            (int)MouseButtonState.FROM_LEFT_2ND_BUTTON_PRESSED | (int)MouseButtonState.FROM_LEFT_3RD_BUTTON_PRESSED |
            (int)MouseButtonState.FROM_LEFT_4TH_BUTTON_PRESSED | (int)MouseButtonState.RIGHTMOST_BUTTON_PRESSED;

        private static IntPtr stdInput;

        //TODO: use hashmaps
        private static long[] keyList =
        {
                makeLong((short)VirtualKey.VK_PRIOR, (short)Key.PPAGE),
                makeLong((short)VirtualKey.VK_NEXT, (short)Key.NPAGE),
                makeLong((short)VirtualKey.VK_END, (short)Key.END),
                makeLong((short)VirtualKey.VK_HOME, (short)Key.HOME),
                makeLong((short)VirtualKey.VK_LEFT, (short)Key.LEFT),
                makeLong((short)VirtualKey.VK_UP, (short)Key.UP),
                makeLong((short)VirtualKey.VK_RIGHT, (short)Key.RIGHT),
                makeLong((short)VirtualKey.VK_DOWN, (short)Key.DOWN),
                makeLong((short)VirtualKey.VK_DELETE, (short)Key.DC),
                makeLong((short)VirtualKey.VK_INSERT, (short)Key.IC)
        };

        private static long[] ansi_keys =
        {
            makeLong((short)VirtualKey.VK_PRIOR, (short)'I'),
            makeLong((short)VirtualKey.VK_NEXT, (short)'Q'),
            makeLong((short)VirtualKey.VK_END, (short)'O'),
            makeLong((short)VirtualKey.VK_HOME, (short)'H'),
            makeLong((short)VirtualKey.VK_LEFT, (short)'K'),
            makeLong((short)VirtualKey.VK_UP, (short)'H'),
            makeLong((short)VirtualKey.VK_RIGHT, (short)'M'),
            makeLong((short)VirtualKey.VK_DOWN, (short)'P'),
            makeLong((short)VirtualKey.VK_DELETE, (short)'S'),
            makeLong((short)VirtualKey.VK_INSERT, (short)'R')
        };

        private static long[] map;
        private static long[] ansi_map;

        private static int N_INI = keyList.Length;
        private static int FKEYS = 24;
        private static int MAPSIZE = FKEYS + N_INI;
        private static int NUMPAIRS = 64;

        internal static bool IsKeyDownEvent(InputRecord ir)
        {
            return (ir.EventType == KEY_EVENT && ir.KeyEvent.bKeyDown);
        }

        internal static bool IsModKey(InputRecord ir)
        {
            // We should also skip over Shift, Control, and Alt, as well as caps lock.
            // Apparently we don't need to check for 0xA0 through 0xA5, which are keys like
            // Left Control & Right Control. See the ConsoleKey enum for these values.
            short keyCode = ir.KeyEvent.wVirtualKeyCode;
            return ((keyCode >= 0x10 && keyCode <= 0x12)
                    || keyCode == 0x14 || keyCode == 0x90 || keyCode == 0x91);
        }

        [DllImport("kernel32.dll")]
        internal extern static IntPtr LoadLibrary(string libToLoad);

        [DllImport("kernel32.dll")]
        internal extern static IntPtr GetProcAddress(IntPtr libHandle, string symbol);

        [DllImport("kernel32.dll")]
        internal extern static bool FreeLibrary(IntPtr libHandle);

        [DllImport("kernel32.dll")]
        internal extern static Coord GetLargestConsoleWindowSize(IntPtr outputHandle);

        [DllImport("kernel32.dll")]
        internal extern static bool SetConsoleScreenBufferSize(IntPtr outputHandle, Coord newSize);

        [DllImport("kernel32.dll")]
        internal extern static bool SetConsoleWindowInfo(IntPtr outputHandle, bool absolute, ref SmallRect rect);

        [DllImport("kernel32.dll")]
        internal extern static bool SetCurrentConsoleFontEx(IntPtr outputHandle, bool maximumWindow, CONSOLE_FONT_INFO_EX fontInfo);

        //TODO: remove trial and error
        [DllImport("kernel32.dll")]
        public extern static IntPtr GetStdHandle(Handles handle);

        [DllImport("kernel32.dll")]
        internal static extern bool ReadConsoleInput(IntPtr hConsoleInput, out InputRecord buffer, int numInputRecords_UseOne, out int numEventsRead);

        [DllImport("kernel32.dll")]
        internal static extern bool PeekConsoleInput(IntPtr hConsoleInput, out InputRecord buffer, int numInputRecords_UseOne, out int numEventsRead);

        [DllImport("kernel32.dll")]
        internal static extern bool WriteConsoleInput(IntPtr hConsoleInput, InputRecord buffer, int numInputRecords_UseOne, out int numEventsRead);

        [DllImport("kernel32.dll")]
        internal static extern bool GetNumberOfConsoleMouseButtons(out int lpNumberOfMouseButtons);

        [DllImport("kernel32.dll")]
        internal static extern bool GetNumberOfConsoleInputEvents(IntPtr hConsoleInput, out int numEvents);

        [DllImport("kernel32.dll")]
        internal static extern bool SetConsoleCP(uint wCodePageID);

        /// <summary>
        /// windows implementation for resize
        /// </summary>
        /// <param name="lines">new number of lines</param>
        /// <param name="columns">new number of columns</param>
        internal static void NativeWindowsConsoleResize(int lines, int columns)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new InvalidOperationException("Native console resize only available on Windows OS");

            //address of the handle created by CreateConsoleScreenBuffer in NCurses
            //https://github.com/rprichard/win32-console-docs -> src/harness/WorkerProgram.cc scanForConsoleHandles
            //TODO: win7 only?
            IntPtr bufferHandle = new IntPtr(19);

            Coord max, newSize;
            SmallRect rect;

            max = NativeWindows.GetLargestConsoleWindowSize(bufferHandle);
            rect.Left = rect.Top = 0;
            rect.Right = (short)(columns - 1);

            if (rect.Right > max.X)
                rect.Right = max.X;

            rect.Bottom = (short)(lines - 1);

            if (rect.Bottom > max.Y)
                rect.Bottom = max.Y;

            newSize.X = (short)(rect.Right + 1);
            newSize.Y = (short)(rect.Bottom + 1);

            if ((!NativeWindows.SetConsoleScreenBufferSize(bufferHandle, newSize) | !NativeWindows.SetConsoleWindowInfo(bufferHandle, true, ref rect)) &&
                !NativeWindows.SetConsoleScreenBufferSize(bufferHandle, newSize))
                throw new ArgumentException("Resize failed");
        }

        private static long makeLong(short highPart, short lowPart)
        {
            return (((ushort)lowPart) | (uint)(highPart << 16));
        }

        private static short hiWord(long dword)
        {
            return (short)(dword >> 16);
        }

        private static short loWord(long dword)
        {
            return (short)dword;
        }

        private static int keycompare(long el1, long el2)
        {
            short key1 = hiWord(el1);
            short key2 = hiWord(el2);

            return ((key1 < key2) ? -1 : ((key1 == key2) ? 0 : 1));
        }

        private static void initMap()
        {
            if (map != null)
                return;

            map = new long[MAPSIZE];
            ansi_map = new long[MAPSIZE];

            for (int i = 0; i < MAPSIZE; i++)
            {
                if (i < N_INI)
                {
                    map[i] = keyList[i];
                    ansi_map[i] = ansi_keys[i];
                }

                else
                {
                    map[i] = makeLong((short)((short)VirtualKey.VK_F1 + (i - N_INI)), (short)((short)Key.F1 + (i - N_INI)));
                    ansi_map[i] = makeLong((short)((short)VirtualKey.VK_F1 + (i - N_INI)), (short)(';' + (i - N_INI)));
                }
            }

            //TODO: sort map?
        }

        private static int mapKey(long[] keyArray, short vKey)
        {
            short nKey = 0;
            long res = 0;
            long key = makeLong(vKey, 0);
            int code = -1;

            for(int i = 0; i < keyArray.Length; i++)
            {
                if (keycompare(keyArray[i], key) == 0)
                {
                    res = keyArray[i];
                    break;
                }
            }

            if (res > 0)
            {
                key = res;
                nKey = loWord(key);
                code = (nKey & 0x7fff);
                if ((nKey & 0x8000) != 0)
                    code = -code;
            }

            return code;
        }

        private static int numButtons;
        private static chtype decode_mouse(int mask)
        {
            if (numButtons == 0)
                NativeWindows.GetNumberOfConsoleMouseButtons(out numButtons);

            chtype result = 0;

            if ((mask & (int)MouseButtonState.FROM_LEFT_1ST_BUTTON_PRESSED) != 0)
                result |= MouseState.BUTTON1_PRESSED;
            if ((mask & (int)MouseButtonState.FROM_LEFT_2ND_BUTTON_PRESSED) != 0)
                result |= MouseState.BUTTON2_PRESSED;
            if ((mask & (int)MouseButtonState.FROM_LEFT_3RD_BUTTON_PRESSED) != 0)
                result |= MouseState.BUTTON3_PRESSED;
            if ((mask & (int)MouseButtonState.FROM_LEFT_4TH_BUTTON_PRESSED) != 0)
                result |= MouseState.BUTTON4_PRESSED;

            if ((mask & (int)MouseButtonState.RIGHTMOST_BUTTON_PRESSED) != 0)
            {
                switch (numButtons)
                {
                    case 1:
                        result |= MouseState.BUTTON1_PRESSED;
                        break;
                    case 2:
                        result |= MouseState.BUTTON2_PRESSED;
                        break;
                    case 3:
                        result |= MouseState.BUTTON3_PRESSED;
                        break;
                    case 4:
                        result |= MouseState.BUTTON4_PRESSED;
                        break;
                }
            }

            return result;
        }

        private static Queue<MEVENT> qMEvent;
        private static int _drv_mouse_old_buttons;
        private static int _drv_mouse_new_buttons;
        private static bool handle_mouse(MouseEventRecord mer)
        {
            bool result = false;

            if (qMEvent == null)
                qMEvent = new Queue<MEVENT>();


            MEVENT mouseEvent = default(MEVENT);

            //if ((mer.dwButtonState & (int)MouseButtonState.FROM_LEFT_1ST_BUTTON_PRESSED) != 0)
            //    mouseEvent.bstate |= MouseState.BUTTON1_PRESSED;
            //if ((mer.dwButtonState & (int)MouseButtonState.FROM_LEFT_2ND_BUTTON_PRESSED) != 0)
            //    mouseEvent.bstate |= MouseState.BUTTON2_PRESSED;
            //if ((mer.dwButtonState & (int)MouseButtonState.FROM_LEFT_3RD_BUTTON_PRESSED) != 0)
            //    mouseEvent.bstate |= MouseState.BUTTON3_PRESSED;
            //if ((mer.dwButtonState & (int)MouseButtonState.FROM_LEFT_4TH_BUTTON_PRESSED) != 0)
            //    mouseEvent.bstate |= MouseState.BUTTON4_PRESSED;

            //if ((mer.dwEventFlags & (int)MouseEventFlags.DOUBLE_CLICK) != 0)
            //    mouseEvent.bstate |= MouseState.BUTTON1_DOUBLE_CLICKED;

            //if (mouseEvent.bstate != 0)
            //{
            //    mouseEvent.x = mer.dwMousePosition.X;
            //    mouseEvent.y = mer.dwMousePosition.Y;
            //    qMEvent.Enqueue(mouseEvent);
            //    result = true;
            //}

            _drv_mouse_old_buttons = _drv_mouse_new_buttons;
            _drv_mouse_new_buttons = mer.dwButtonState & BUTTON_MASK;

            if (_drv_mouse_new_buttons != _drv_mouse_old_buttons)
            {
                if (_drv_mouse_new_buttons != 0)
                    mouseEvent.bstate |= decode_mouse(_drv_mouse_new_buttons);
                else
                {
                    /* cf: BUTTON_PRESSED, BUTTON_RELEASED */
                    mouseEvent.bstate |= decode_mouse(_drv_mouse_old_buttons >> 1);
                    result = true;
                }

                mouseEvent.x = mer.dwMousePosition.X;
                mouseEvent.y = mer.dwMousePosition.Y; // - AdjustY(); //TODO: get CON.SBI.srWindow.Top

                qMEvent.Enqueue(mouseEvent);

                //TODO: let ncurses handle it?
                //sp->_drv_mouse_fifo[sp->_drv_mouse_tail] = work;
                //sp->_drv_mouse_tail += 1;
            }

            return result;
        }

        internal static void GetWindowsConsoleMouseEvent(out MEVENT mEvent)
        {
            if (qMEvent == null)
                throw new ArgumentNullException("Mouse queue hasn't been initialized yet.");

            if (qMEvent.Count == 0)
                throw new ArgumentException("No mouse events have been queued yet.");

            mEvent = qMEvent.Dequeue();
        }

        private static int prevBuf;
        /* TODO:
         * thread-safety (or consider as a stream)
         * check if echo is on and add to window?
        */
        /// <summary>
        /// windows implementation of input handling
        /// </summary>
        /// <param name="lines">new number of lines</param>
        /// <param name="columns">new number of columns</param>
        internal static int NativeWindowsConsoleRead(IntPtr window)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new InvalidOperationException("Native console read only available on Windows OS");

            initMap();

            if(stdInput == IntPtr.Zero)
                stdInput = NativeWindows.GetStdHandle(Handles.STD_INPUT);

            if(prevBuf != 0)
            {
                try
                {
                    return prevBuf;
                }
                finally
                {
                    prevBuf = 0;
                }
            }

            int buf = 0;
            int rc = -1;
            InputRecord inp_rec;
            bool b;
            int nRead;
            short vk;

            while ((b = ReadConsoleInput(stdInput, out inp_rec, 1, out nRead)))
            {
                if (b && nRead > 0)
                {
                    if (rc < 0)
                        rc = 0;
                    rc = rc + nRead;

                    if (inp_rec.EventType == KEY_EVENT)
                    {
                        if (!inp_rec.KeyEvent.bKeyDown)
                            continue;

                        buf = inp_rec.KeyEvent.uChar;
                        vk = inp_rec.KeyEvent.wVirtualKeyCode;

                        if (vk >= (int)VirtualKey.VK_F1 && vk <= (int)VirtualKey.VK_F12 &&
                            (inp_rec.KeyEvent.dwControlKeyState & (int)ControlKeyState.SHIFT_PRESSED) != 0)
                            vk = (short)(vk + 12);

                        if (buf == 0)
                        {
                            int key = mapKey(map, vk);
                            if (key < 0)
                                continue;

                            if (NativeWindow.is_keypad(window))
                                buf = key;
                            else
                                buf = mapKey(ansi_keys, vk);
                        }

                        //TODO: control keys only?
                        if ((inp_rec.KeyEvent.dwControlKeyState & ((int)ControlKeyState.LEFT_ALT_PRESSED | (int)ControlKeyState.RIGHT_ALT_PRESSED)) != 0)
                        {
                            prevBuf = buf;
                            buf = (int)VirtualKey.VK_MENU;
                        }
                        else if ((inp_rec.KeyEvent.dwControlKeyState & ((int)ControlKeyState.LEFT_CTRL_PRESSED | (int)ControlKeyState.RIGHT_CTRL_PRESSED)) != 0)
                        {
                            prevBuf = buf;
                            buf = (int)VirtualKey.VK_CONTROL;
                        }

                        break;
                    }
                    else if (inp_rec.EventType == MOUSE_EVENT && handle_mouse(inp_rec.MouseEvent))
                    {
                        buf = (int)Key.MOUSE;
                        break;
                    }
                }
            }

            if (rc > 0)
                return buf;
            else
                return Constants.ERR;

            //InputRecord ir;
            //int read;
            //short keyCode;
            //char ch;
            //IntPtr stdInput = NativeWindows.GetStdHandle(Handles.STD_INPUT);
            //while (NativeWindows.ReadConsoleInput(stdInput, out ir, 1, out read))
            //{
            //    if (read == 0)
            //        break;

            //    //if (ir.eventType == NativeWindows.MOUSE_EVENT &&
            //    //    (ir.mouseEvent.eventFlag == 0 || ir.mouseEvent.eventFlag == 2))
            //    //    break;

            //    keyCode = ir.keyEvent.virtualKeyCode;

            //    if (!NativeWindows.IsKeyDownEvent(ir) && keyCode != NativeWindows.AltVKCode)
            //        continue;

            //    ch = ir.keyEvent.uChar;

            //    if (ch == 0 && NativeWindows.IsModKey(ir))
            //        continue;

            //    break;
            //}

            //if (!NativeWindows.WriteConsoleInput(stdInput, ir, 1, out read))
            //    throw new InvalidOperationException("Failed to place back into buffer");

            //if (NativeWindows.IsKeyDownEvent(ir))
            //{
            //    ControlKeyState state = (ControlKeyState)ir.keyEvent.controlKeyState;
            //    bool shift = (state & ControlKeyState.ShiftPressed) != 0;
            //    bool alt = (state & (ControlKeyState.LeftAltPressed | ControlKeyState.RightAltPressed)) != 0;
            //    bool control = (state & (ControlKeyState.LeftCtrlPressed | ControlKeyState.RightCtrlPressed)) != 0;
            //    if (alt || control)
            //    {
            //        receivedModkey = true;
            //        return alt ? 12 : control ? 11 : 0;
            //    }
            //}
        }

        private static int getModifierKey(int read)
        {
            if (previousButton.EventType == KEY_EVENT &&
                (previousButton.KeyEvent.dwControlKeyState & ((int)ControlKeyState.LEFT_ALT_PRESSED | (int)ControlKeyState.RIGHT_ALT_PRESSED)) != 0)
            {
                previousCharBuffer = read;
                return (int)VirtualKey.VK_MENU;
            }
            else if (previousButton.EventType == KEY_EVENT &&
                (previousButton.KeyEvent.dwControlKeyState & ((int)ControlKeyState.LEFT_CTRL_PRESSED | (int)ControlKeyState.RIGHT_CTRL_PRESSED)) != 0)
            {
                previousCharBuffer = read;
                return (int)VirtualKey.VK_CONTROL;
            }

            return 0;
        }

        private static int previousCharBuffer;
        //windows follows registered order?
        internal static int NativeWindowsConsoleCharInput(IntPtr window)
        {
            if (previousCharBuffer > 0)
            {
                try
                {
                    return previousCharBuffer;
                }
                finally
                {
                    previousCharBuffer = 0;
                }
            }

            CancellationTokenSource cancel = new CancellationTokenSource();
            Task peekTask = Task.Run(() => nativeWindowsConsoleInputPeek(cancel.Token));

            int read = NativeNCurses.NCursesWrapper.wgetch(window);
            cancel.Cancel();
            peekTask.Wait();

            int modifierKey;
            if ((modifierKey = getModifierKey(read)) > 0)
                return modifierKey;

            return read;
        }

        internal static char NativeWindowsConsoleWCharInput(IntPtr window)
        {
            if (previousCharBuffer > 0)
            {
                try
                {
                    return (char)previousCharBuffer;
                }
                finally
                {
                    previousCharBuffer = 0;
                }
            }

            CancellationTokenSource cancel = new CancellationTokenSource();
            Task peekTask = Task.Run(() => nativeWindowsConsoleInputPeek(cancel.Token));

            char read;
            //shouldn't be commented out
            //NativeWindow.ncurses_wget_wch(window, out read);
            //cancel.Cancel();
            //peekTask.Wait();

            //int modifierKey;
            //if ((modifierKey = getModifierKey(read)) > 0)
            //    return (char)modifierKey;

            //return read;
            return '0';
        }

        private static InputRecord previousButton;
        private static void nativeWindowsConsoleInputPeek(CancellationToken cancellationToken)
        {
            if (stdInput == IntPtr.Zero)
                stdInput = NativeWindows.GetStdHandle(Handles.STD_INPUT);

            InputRecord inp_rec = default(InputRecord);
            bool b;
            int nRead;

            while (!cancellationToken.IsCancellationRequested && (b = ReadConsoleInput(stdInput, out inp_rec, 1, out nRead)))
                WriteConsoleInput(stdInput, inp_rec, 1, out nRead);

            previousButton = inp_rec;
        }
    }

    internal static class NativeLinux
    {
        [DllImport("libdl.so.2")]
        internal static extern IntPtr dlopen(string dllToLoad, int flags);

        [DllImport("libdl.so.2")]
        internal static extern IntPtr dlsym(IntPtr libHandle, string symbol);

        [DllImport("libdl.so.2")]
        internal static extern int dlclose(IntPtr libHandle);

        [DllImport("libc.so.6")]
        internal static extern string setlocale(int category, string locale);
    }
}
