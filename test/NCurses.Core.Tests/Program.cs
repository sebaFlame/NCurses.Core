using System;
using System.Text;
using System.Collections.Generic;
using NCurses.Core.Interop;

namespace NCurses.Core.Tests
{
    public class Program
    {
        private static uint UnicodeCharToUTF8Uint(char c)
        {
            byte[] arrow = Encoding.Unicode.GetBytes(c.ToString());
            byte[] arrowUint = new byte[4];
            Array.Copy(arrow, arrowUint, arrow.Length);
            return BitConverter.ToUInt32(arrowUint, 0);
        }

        public static void Main(string[] args)
        {
            Window stdScr = null;

            //testRipoffLine(ref stdScr);
            //testPad(ref stdScr);
            //testColor(ref stdScr);
            //testWrite(ref stdScr);
            //testReadFromOutput(ref stdScr);
            //testInsert(ref stdScr);
            //testASC(ref stdScr);
            testRead(ref stdScr);
            //testWindowMemLeak(ref stdScr);

            Console.ReadKey();

            NCurses.End();
        }

        //TODO: doesn't run on windows
        private static void testRipoffLine(ref Window stdScr)
        {
            Window input = null;
            Action<Window, int> assignInput = (Window win, int cols) => input = win;
            NCurses.RipOffLine(-1, assignInput);

            stdScr = NCurses.Start();
        }

        private static void testPad(ref Window stdScr)
        {
            Pad newPad = new Pad(200, 200);
            newPad.Scroll = true;
            newPad.KeyPad = true;

            for (int i = 0; i < 200; i++)
                newPad.WriteLine("{0}Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", i.ToString().PadLeft(3, ' '));

            NCurses.Resize(50, 120);
            int currentLine = 50;
            newPad.NoOutRefresh(currentLine, 0, 0, 0, 49, 119);
            NCurses.Update();

            int c;
            while ((c = newPad.ReadKey()) != Constants.ERR)
            {
                if (c == (int)Key.PPAGE) // page up
                {
                    if ((currentLine -= 50) < 0)
                        currentLine = 0;
                }
                else if (c == (int)Key.NPAGE)
                {
                    if ((currentLine += 50) > (newPad.MaxLine - 50))
                        currentLine = newPad.MaxLine - 50;
                }
                else if (c == 'q')
                    break;

                newPad.NoOutRefresh(currentLine, 0, 0, 0, 49, 119);
                NCurses.Update();
            }

            NCurses.Resize(50, 100);
            newPad.WriteLine(NCurses.stdscr.MaxColumn.ToString());
            newPad.NoOutRefresh(currentLine = 150, 0, 0, 0, 49, 99);
            NCurses.Update();
        }

        private static void testColor(ref Window stdScr)
        {
            stdScr = NCurses.Start();

            if (NCurses.HasColor)
            {
                NCurses.StartColor();
                NCurses.InitDefaultPairs();
            }

            for (int i = 0; i < NCurses.ColorPairs; i++)
            {
                stdScr.Color = (short)i;
                stdScr.Write(i.ToString().PadLeft(3));
            }

            stdScr.Refresh();
        }

        private static void testWrite(ref Window stdScr)
        {
            stdScr = NCurses.Start();

            //supports more unicode characters
            NCurses.SetFont(WindowsConsoleFont.CONSOLAS);
            NCurses.Resize(50, 120);

            NCurses.Echo = false;

            ////test add ASCII char with attributes
            ulong c = 'a';
            c |= Attrs.BOLD;
            stdScr.Write(c);
            stdScr.Refresh();

            //test add unicode char
            stdScr.Write('\u263A');
            stdScr.Refresh();

            if (NCurses.HasColor)
            {
                NCurses.StartColor();
                NCurses.InitDefaultPairs();
            }

            ////test get correct "string" from cchar_t
            short colorPair;
            ulong attrs;
            NCursesWCHAR wch = new NCursesWCHAR('\u263A');
            wch.attr = Attrs.BOLD;
            wch.ext_color = 4;
            string wStr = NCurses.GetStringFromWideChar(wch, out attrs, out colorPair);

            //test get correct cchar_t from string
            string str = '\u263A'.ToString();
            NCursesWCHAR wch1 = NCurses.GetWideCharFromString(str, Attrs.BOLD, 4);

            //test add ASCII char with attributes and color
            c = 'b';
            c |= Attrs.BOLD;
            c |= NCurses.ColorPair(3);
            stdScr.Write(c);
            stdScr.Refresh();

            //test add unicode char with attributes and color
            stdScr.Write('\u263A', Attrs.BOLD, 4);
            stdScr.Refresh();

            //test add unicode string
            stdScr.Write("bleh\u263A");
            stdScr.Refresh();

            //test add ascii character array
            ulong[] chars1 = new ulong[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };
            stdScr.Write(chars1, Attrs.BOLD, 4);
            stdScr.Refresh();

            //test add unicode character array
            char[] chars = new char[] { '\u0490', '\u0491', '\u0492', '\u0493', '\u0494', '\u0495', '\u0496', '\u0497', '\u0498', '\u0499'
                , '\u049A', '\u049B', '\u049C', '\u049D', '\u049E', '\u049F' };
            stdScr.Write(chars, Attrs.BOLD, 4);
            stdScr.Refresh();
        }

        private static void testReadFromOutput(ref Window stdScr)
        {
            stdScr = NCurses.Start();

            //supports more unicode characters
            NCurses.SetFont(WindowsConsoleFont.CONSOLAS);
            NCurses.Resize(50, 120);

            //test add ascii character array
            ulong[] chars1 = new ulong[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };
            stdScr.Write(chars1, Attrs.BOLD, 4);

            //ASCII char test
            char ch = stdScr.GetChar();
            ulong attrs;
            short pair;
            ch = stdScr.GetChar(out attrs, out pair);
            string str = stdScr.GetString();

            //test add unicode character array
            char[] chars = new char[] { '\u0490', '\u0491', '\u0492', '\u0493', '\u0494', '\u0495', '\u0496', '\u0497', '\u0498', '\u0499'
                , '\u049A', '\u049B', '\u049C', '\u049D', '\u049E', '\u049F' };
            stdScr.Write(chars, Attrs.BOLD, 4);

            //unicode char test
            ch = stdScr.GetChar();
            str = stdScr.GetString();
        }

        private static void testInsert(ref Window stdScr)
        {
            stdScr = NCurses.Start();

            //supports more unicode characters
            NCurses.SetFont(WindowsConsoleFont.CONSOLAS);
            NCurses.Resize(50, 120);

            stdScr.Write("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
            stdScr.Refresh();

            stdScr.MoveCursor(0, 0);
            //test unicode char insert
            stdScr.Insert('\u263A');
            //test ASCII char insert
            stdScr.Insert('b');
            stdScr.Refresh();

            if (NCurses.HasColor)
            {
                NCurses.StartColor();
                NCurses.InitDefaultPairs();
            }

            stdScr.MoveCursor(0, 0);
            //test unicode char insert
            stdScr.Insert('\u263A', Attrs.BOLD, 4);
            //test ASCII char insert
            stdScr.Insert('b', Attrs.BOLD, 4);
            stdScr.Refresh();

            char[] chars = new char[] { '\u0490', '\u0491', '\u0492', '\u0493', '\u0494', '\u0495', '\u0496', '\u0497', '\u0498', '\u0499'
                , '\u049A', '\u049B', '\u049C', '\u049D', '\u049E', '\u049F' };
            char[] chars1 = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };

            //test unicode string insert
            stdScr.Insert(new string(chars));
            //test ASCII string insert
            stdScr.Insert(new string(chars1));
            stdScr.Refresh();

            stdScr.Color = 4;
            stdScr.AttrOn(Attrs.BOLD);
            stdScr.Insert("IS THIS COLORED?"); //yes
            stdScr.Refresh();
        }

        private static void testASC(ref Window stdScr)
        {
            stdScr = NCurses.Start();

            //supports more unicode characters
            NCurses.SetFont(WindowsConsoleFont.LUCIDA);
            NCurses.Resize(50, 120);

            //regular supported ACS chars
            stdScr.Write("Regular ACS: ");
            stdScr.Write(Acs.ULCORNER);

            //wide ACS chars
            stdScr.Write("\nWide ACS: ");
            stdScr.Write(Wacs.D_ULCORNER);
            stdScr.Write(Wacs.ULCORNER);

            stdScr.Refresh();
        }

        //keypad and keyname as ansi string == actual key names (except backspace/enter)
        private static void testRead(ref Window stdScr)
        {
            stdScr = NCurses.Start();
            NCurses.Echo = true;
            //Managed.NCurses.CBreak = true;
            stdScr.KeyPad = true;
            //Managed.NCurses.Meta = true;
            //stdScr.UseHwInsDelLine = true;
            //Managed.NCurses.FlushOnInterrupt = false;
            //stdScr.Blocking = true;
            //stdScr.NoTimeout = true;
            //Managed.NCurses.Raw = true;

            int ch;
            Key key;
            MEVENT mouseEvent = default(MEVENT);
            ulong oldMask;
            ulong newMask = NCurses.EnableMouseMask(MouseState.BUTTON1_CLICKED |
                MouseState.BUTTON1_DOUBLE_CLICKED | MouseState.BUTTON1_TRIPLE_CLICKED, out oldMask);

            while ((ch = stdScr.ReadKey()) != 'q')
            {
                if (NCurses.GetKey(ch, out key) && key == Key.MOUSE)
                    mouseEvent = NCurses.GetMouseEvent();
            }
        }

        //upto 420MB, back down to 24MB -> FINE
        private static void testWindowMemLeak(ref Window stdScr)
        {
            List<Window> lstWindow = new List<Window>();
            Window win;

            if (NCurses.HasColor)
            {
                NCurses.StartColor();
                NCurses.InitDefaultPairs();
            }

            for (int i = 0; i < 100; i++)
            {
                lstWindow.Add(win = new Window());
                win.BackGround = 'g' | Attrs.BOLD | NCurses.ColorPair(3);
                win.NoOutRefresh();
            }

            NCurses.Update();
            stdScr = NCurses.stdscr;

            stdScr.ReadKey();

            foreach(Window w in lstWindow)
                w.Dispose();

            lstWindow.Clear();
            NCurses.Update();

            stdScr.ReadKey();
        }

        //    public static void TestMain(string[] args)
        //    {
        //        //Console.OutputEncoding = Encoding.Unicode;

        //        //CultureInfo culture = new CultureInfo("nl-be");
        //        //CultureInfo.CurrentCulture = culture;
        //        //CultureInfo.CurrentUICulture = culture;

        //        //Environment.SetEnvironmentVariable("LC_CTYPE", "nl_BE.UTF-8");
        //        //Environment.SetEnvironmentVariable("LC_ALL", "6");

        //        IntPtr mainWindowPtr = Curses.initscr();
        //        IntPtr mainScreenPtr = Curses._nc_screen_of(mainWindowPtr);

        //        bool hasColors = Curses.has_colors();
        //        bool hasUnicode = Curses._nc_unicode_locale();

        //        int ret;

        //        if (hasColors)
        //        {

        //            ret = Curses.start_color();
        //            //    ret = Curses.use_default_colors();
        //        }

        //        ret = Curses.keypad(mainWindowPtr, true);

        //        //int length = Marshal.ReadInt32(mainScreen._acs_map);
        //        //long[] result = new long[length];
        //        //Marshal.Copy(mainScreen._acs_map + sizeof(int), result, 0, length);

        //        //ret = Curses.nocbreak();

        //        int newLines = 50;
        //        int newColumns = 120;
        //        ret = Curses.resize_term(newLines, newColumns);

        //        WINDOW mainWindow = Marshal.PtrToStructure<WINDOW>(mainWindowPtr);
        //        SCREEN mainScreen = Marshal.PtrToStructure<SCREEN>(mainScreenPtr);

        //        Coord max, newSize;
        //        SmallRect rect;
        //        bool success = false;

        //        IntPtr windowHandle = NativeWindowsMethods.GetConsoleWindow();
        //        IntPtr outputHandle = NativeWindowsMethods.GetStdHandle(Handles.STD_OUTPUT);

        //        //address of the handle created by CreateConsoleScreenBuffer in NCurses
        //        //https://github.com/rprichard/win32-console-docs -> src/harness/WorkerProgram.cc scanForConsoleHandles
        //        //TODO: win7 only?
        //        IntPtr bufferHandle = new IntPtr(19);

        //        max = NativeWindowsMethods.GetLargestConsoleWindowSize(bufferHandle);
        //        rect.Left = rect.Top = 0;
        //        rect.Right = (short)(newColumns - 1);

        //        if (rect.Right > max.X)
        //            rect.Right = max.X;

        //        rect.Bottom = (short)(newLines - 1);

        //        if (rect.Bottom > max.Y)
        //            rect.Bottom = max.Y;

        //        newSize.X = (short)(rect.Right + 1);
        //        newSize.Y = (short)(rect.Bottom + 1);

        //        success = NativeWindowsMethods.SetConsoleScreenBufferSize(bufferHandle, newSize);
        //        success = NativeWindowsMethods.SetConsoleWindowInfo(bufferHandle, true, ref rect);
        //        //success = NativeWindowsMethods.SetConsoleActiveScreenBuffer(bufferHandle);

        //        //ConsoleScreenBufferInfo info = new ConsoleScreenBufferInfo();
        //        //success = NativeWindowsMethods.GetConsoleScreenBufferInfo(outputHandle, out info);
        //        //rect = info.Window;
        //        //rect.Right = (short)(rect.Left + newColumns - 1);
        //        //rect.Bottom = (short)(rect.Top + newLines - 1);

        //        //newSize.X = (short)(rect.Right + 1);
        //        //newSize.Y = (short)(rect.Bottom + 1);

        //        //success = NativeWindowsMethods.SetConsoleScreenBufferSize(outputHandle, newSize);
        //        //success = NativeWindowsMethods.SetConsoleWindowInfo(outputHandle, true, ref rect);
        //        //success = NativeWindowsMethods.SetConsoleActiveScreenBuffer(outputHandle);

        //        uint downArrow = '↓';
        //        uint uSmth = 'ù';
        //        uint cSmth = 'ç';

        //        //StringBuilder correctChar = new StringBuilder(2);
        //        //ret = Curses._nc_mbtowc(correctChar, "↓", 2);
        //        //char[] arrowUint = new char[2];
        //        //correctChar.CopyTo(0, arrowUint, 0, correctChar.Length);
        //        //byte[] arrowUnicodeBytes = Encoding.Unicode.GetBytes(arrowUint);
        //        //uint arrowValue = BitConverter.ToUInt32(arrowUnicodeBytes, 0);

        //        //StringBuilder result = new StringBuilder(2);
        //        //ret = Curses._nc_wctomb(result, "↓");
        //        //arrowUint = new char[4];
        //        //result.CopyTo(0, arrowUint, 0, result.Length);
        //        //arrowUnicodeBytes = Encoding.UTF8.GetBytes(arrowUint);
        //        //arrowValue = BitConverter.ToUInt32(arrowUnicodeBytes, 0);

        //        //byte[] bytes = Encoding.UTF8.GetBytes("↓");
        //        //StringBuilder utf8Builder = new StringBuilder(4);
        //        //ret = Curses._nc_mbtowc(utf8Builder, new string(Encoding.UTF8.GetChars(bytes)), 4);

        //        CONSOLE_FONT_INFO_EX info = new CONSOLE_FONT_INFO_EX();
        //        info.cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFO_EX>();
        //        success = NativeWindowsMethods.GetCurrentConsoleFontEx(bufferHandle, false, ref info);

        //        //change font size
        //        //info.dwFontSize.X = 4;
        //        //info.dwFontSize.Y = 6;
        //        //success = NativeWindowsMethods.SetCurrentConsoleFontEx(bufferHandle, false, info);

        //        //works
        //        CONSOLE_FONT_INFO_EX newFont = new CONSOLE_FONT_INFO_EX();
        //        newFont.cbSize = (uint)Marshal.SizeOf<CONSOLE_FONT_INFO_EX>();
        //        newFont.FaceName = "Lucida Console";
        //        success = NativeWindowsMethods.SetCurrentConsoleFontEx(bufferHandle, false, newFont);

        //        max = NativeWindowsMethods.GetLargestConsoleWindowSize(bufferHandle);

        //        //works (contains all Terminal fontSizes and Lucida inserted at random)
        //        //uint numberOfConsoleFonts = NativeWindowsMethods.GetNumberOfConsoleFonts();
        //        //success = NativeWindowsMethods.SetConsoleFont(bufferHandle, --numberOfConsoleFonts);

        //        StringBuilder text = new StringBuilder(1);
        //        text.Append("↓");
        //        ret = Curses.waddwstr(mainWindowPtr, text);

        //        text.Clear();
        //        text.Append("\u0442\u2020\u263A\u2473");
        //        ret = Curses.waddwstr(mainWindowPtr, text);

        //        NativeWindow.waddch(mainWindowPtr, uSmth);
        //        NativeWindow.waddch(mainWindowPtr, cSmth);
        //        ret = Curses.wnoutrefresh(mainWindowPtr);
        //        ret = Curses.doupdate();

        //        //Curses.COLORS(mainWindow);
        //        //uint corner = Acs.ULCORNER;

        //        //uint border = Constants.NCURSES_ACS(mainScreenPtr, 'l');
        //        //char b = (char)border;
        //        //NativeWindow.waddch(mainWindowPtr, border);
        //        //ret = Curses.wnoutrefresh(mainWindowPtr);
        //        //ret = Curses.doupdate();

        //        Curses.wattron(mainWindowPtr, Attrs.BOLD);
        //        NativeWindow.waddch(mainWindowPtr, 'b');
        //        NativeWindow.waddch(mainWindowPtr, 'o');
        //        NativeWindow.waddch(mainWindowPtr, 'l');
        //        NativeWindow.waddch(mainWindowPtr, 'd');
        //        Curses.wattroff(mainWindowPtr, Attrs.BOLD);
        //        NativeWindow.waddch(mainWindowPtr, ' ');
        //        NativeWindow.waddch(mainWindowPtr, 'n');
        //        NativeWindow.waddch(mainWindowPtr, 'o');
        //        ret = Curses.wnoutrefresh(mainWindowPtr);
        //        ret = Curses.doupdate();

        //        ret = Curses.init_pair(1, (short)Color.RED, (short)Color.BLACK);
        //        ret = Curses.wattron(mainWindowPtr, (uint)Curses.COLOR_PAIR(1));
        //        NativeWindow.waddch(mainWindowPtr, ' ');
        //        NativeWindow.waddch(mainWindowPtr, 'r');
        //        NativeWindow.waddch(mainWindowPtr, 'e');
        //        NativeWindow.waddch(mainWindowPtr, 'd');
        //        Curses.wattrset(mainWindowPtr, Attrs.NORMAL);
        //        NativeWindow.waddch(mainWindowPtr, ' ');
        //        NativeWindow.waddch(mainWindowPtr, 'n');
        //        NativeWindow.waddch(mainWindowPtr, 'o');
        //        ret = Curses.wnoutrefresh(mainWindowPtr);
        //        ret = Curses.doupdate();

        //        Curses._nc_init_wacs();

        //        IntPtr dllPtr = NativeWindowsMethods.LoadLibrary(Constants.DLLNAME);

        //        IntPtr colPtr = NativeWindowsMethods.GetProcAddress(dllPtr, "COLORS");
        //        int colorCount = Marshal.ReadInt32(colPtr);

        //        IntPtr pairPtr = NativeWindowsMethods.GetProcAddress(dllPtr, "COLOR_PAIRS");
        //        int colorPairs = Marshal.ReadInt32(pairPtr);

        //        IntPtr columnPtr = NativeWindowsMethods.GetProcAddress(dllPtr, "COLS");
        //        int columns = Marshal.ReadInt32(columnPtr);

        //        IntPtr linePtr = NativeWindowsMethods.GetProcAddress(dllPtr, "LINES");
        //        int lines = Marshal.ReadInt32(linePtr);

        //        IntPtr tabPtr = NativeWindowsMethods.GetProcAddress(dllPtr, "TABSIZE");
        //        int tabs = Marshal.ReadInt32(tabPtr);

        //        IntPtr acsPtr = NativeWindowsMethods.GetProcAddress(dllPtr, "acs_map");
        //        int acsLength = Marshal.ReadInt32(acsPtr);
        //        uint ACS_ULCORNER = Marshal.PtrToStructure<uint>(acsPtr + ('l' * sizeof(uint)));
        //        char ulCorner = (char)ACS_ULCORNER;
        //        uint ACS_BULLET = Marshal.PtrToStructure<uint>(acsPtr + ('~' * sizeof(uint)));
        //        char bullet = (char)ACS_BULLET;
        //        int[] acs_map = new int[128];
        //        Marshal.Copy(acsPtr, acs_map, 0, 128);
        //        ACS_ULCORNER = (uint)acs_map['l'];

        //        IntPtr wacsPtr = NativeWindowsMethods.GetProcAddress(dllPtr, "_nc_wacs");
        //        //int wacsLength = Marshal.ReadInt32(wacsPtr);
        //        //IntPtr ulCornerPtr = Marshal.ReadIntPtr(wacsPtr + ('l' * sizeof(int)));
        //        //NCURSES_CH_T WACS_ULCORNER = Marshal.PtrToStructure<NCURSES_CH_T>(ulCornerPtr);
        //        int[] wacs_map = new int[128];
        //        Marshal.Copy(wacsPtr, wacs_map, 0, 128);

        //        IntPtr spPtr = NativeWindowsMethods.GetProcAddress(dllPtr, "SP");
        //        IntPtr spPtr1 = Marshal.ReadIntPtr(spPtr);
        //        SCREEN spScreen = Marshal.PtrToStructure<SCREEN>(spPtr1);

        //        NativeWindowsMethods.FreeLibrary(dllPtr);

        //        //multi-threading support!!!! (SLOW)
        //        NativeWindow.waddch_t(mainWindowPtr, 'a');
        //        ret = Curses.wnoutrefresh(mainWindowPtr);
        //        ret = Curses.doupdate();

        //        uint ACS_LLCORNER = Curses._nc_acs_char_sp(mainScreenPtr, 'm');
        //        char llcorner = (char)ACS_LLCORNER;

        //        NCURSES_CH_T wch = new NCURSES_CH_T('\u263A');
        //        wch.attr = Attrs.ITALIC;
        //        IntPtr wchPtr = Marshal.AllocHGlobal(Marshal.SizeOf(wch));
        //        Marshal.StructureToPtr(wch, wchPtr, true);

        //        ret = Curses.add_wch(wchPtr);
        //        ret = Curses.wnoutrefresh(mainWindowPtr);
        //        ret = Curses.doupdate();

        //        Marshal.FreeHGlobal(wchPtr);

        //        //Curses.VerifyNCursesMethod(new Func<IntPtr, uint, int>(Curses.waddch), "waddch", mainWindowPtr, 'd');
        //        //Curses.VerifyNCursesMethod(() => Curses.waddch(mainWindowPtr, 'd'), "waddch");

        //        Console.ReadKey();

        //        Curses.endwin();
        //    }
    }
}
