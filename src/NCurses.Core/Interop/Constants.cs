using System;
using System.Text;

//several constants taken from ncurses.h
namespace NCurses.Core.Interop
{
    public static class Constants
    {
        private static int NCURSES_ATTR_SHIFT = 8;

        //TODO: expose these?
        internal static string DLLNAME { get; private set; }
        internal static string DLLPANELNAME { get; private set; }
        internal static int SIZEOF_WCHAR_T { get; private set; }
        internal static Type CHTYPE_TYPE { get; private set; }

        //TODO: implement these
        internal static int SIZEOF_WINT_T { get; private set; }
        internal static int SIZEOF_ATTR_T { get; private set; }
        internal static int SIZEOF_MMASK_T { get; private set; }

        internal const int CCHARW_MAX = 5;
        //TODO: all references to this value can overflow
        internal const int MAX_STRING_LENGTH = 1024;

        internal const string TypeGenerationExceptionMessage = "Custom types haven't been generated yet, please run NCurses.Start, NativeNCurses.initscr or create a window with Window.CreateWindow";
        internal const string NoUnicodeExceptionMessage = "Unicode not supported";

        public const int ERR = -1;
        public const int OK = 0;

        //TODO: get WCHAR_T size at runtime (through libc?)
        static Constants()
        {
            //register code pages (for CP 437)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string identifier;
            switch ((identifier = Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier()))
            {
                case "win7-x64":
                case "win8-x64":
                case "win81-x64":
                case "win10-x64":
                case "win7-x86":
                case "win8-x86":
                case "win81-x86":
                case "win10-x86":
                    DLLNAME = "libncursesw6";
                    DLLPANELNAME = "libpanelw6";
                    SIZEOF_WCHAR_T = 2;
                    CHTYPE_TYPE = typeof(UInt32);
                    break;
                case "ubuntu.16.04-x64":
                case "ubuntu.18.04-x64":
                case "debian.8-x64":
                case "debian.9-x64":
				case "debian.10-x64":
                    DLLNAME = "libncursesw.so.5.9";
                    DLLPANELNAME = "libpanelw.so.5.9";
                    SIZEOF_WCHAR_T = 4;
                    CHTYPE_TYPE = typeof(UInt64);
                    break;
                default:
                    DLLNAME = "libncursesw.so.6";
                    DLLPANELNAME = "libpanelw.so.6";
                    SIZEOF_WCHAR_T = 4;
                    CHTYPE_TYPE = typeof(UInt32);
                    break;
            }
        }

        internal static ulong NCURSES_BITS(ulong mask, int shift)
        {
            return mask << (shift + NCURSES_ATTR_SHIFT);
        }

        internal static ulong NCURSES_MOUSE_MASK(int button, ulong mask)
        {
            return mask << (((button) - 1) * 5);
        }

        public static ulong COLOR_PAIR(ulong number)
        {
            if (number > 255)
            {
                return 255 << 8;
            }

            return (NCURSES_BITS(number, 0) & Attrs.COLOR);
        }

        public static int PAIR_NUMBER(ulong attr)
        {
            return (int)((attr & Attrs.COLOR) >> NCURSES_ATTR_SHIFT);
        }
    }

    /// <summary>
    /// Character attributes
    /// </summary>
    public static class Attrs
    {
        public static ulong NORMAL = (1U - 1U);
        public static ulong ATTRIBUTES = Constants.NCURSES_BITS(~(1U - 1U), 0);
        public static ulong CHARTEXT = Constants.NCURSES_BITS(1U, 0) - 1U;
        public static ulong COLOR = Constants.NCURSES_BITS(((1U) << 8) - 1U, 0);
        public static ulong STANDOUT = Constants.NCURSES_BITS(1U, 8);
        public static ulong UNDERLINE = Constants.NCURSES_BITS(1U, 9);
        public static ulong REVERSE = Constants.NCURSES_BITS(1U, 10);
        public static ulong BLINK = Constants.NCURSES_BITS(1U, 11);
        public static ulong DIM = Constants.NCURSES_BITS(1U, 12);
        public static ulong BOLD = Constants.NCURSES_BITS(1U, 13);
        public static ulong ALTCHARSET = Constants.NCURSES_BITS(1U, 14);
        public static ulong INVIS = Constants.NCURSES_BITS(1U, 15);
        public static ulong PROTECT = Constants.NCURSES_BITS(1U, 16);
        public static ulong HORIZONTAL = Constants.NCURSES_BITS(1U, 17);
        public static ulong LEFT = Constants.NCURSES_BITS(1U, 18);
        public static ulong LOW = Constants.NCURSES_BITS(1U, 19);
        public static ulong RIGHT = Constants.NCURSES_BITS(1U, 20);
        public static ulong TOP = Constants.NCURSES_BITS(1U, 21);
        public static ulong VERTICAL = Constants.NCURSES_BITS(1U, 22);
        public static ulong ITALIC = Constants.NCURSES_BITS(1U, 23);
    }

    /// <summary>
    /// Default colors, some terminals support colors > 8
    /// </summary>
    public enum Color : short
    {
        BLACK =	0,
        RED = 1,
        GREEN =	2,
        YELLOW = 3,
        BLUE = 4,
        MAGENTA = 5,
        CYAN = 6,
        WHITE = 7
    }

    //TODO: add fkeys 13-24
    /// <summary>
    /// Key codes, these are octals in ncurses.h
    /// </summary>
    public enum Key : short
    {
        /// <summary>
        /// A wchar_t contains a key code
        /// </summary>
        CODE_YES = 256,
        /// <summary>
        /// Minimum curses key
        /// </summary>
        MIN = 257,
        /// <summary>
        /// Break key (unreliable)
        /// </summary>
        BREAK = 257,
        /// <summary>
        /// Soft (partial) reset (unreliable)
        /// </summary>
        SRESET = 344,
        /// <summary>
        /// Reset or hard reset (unreliable)
        /// </summary>
        RESET = 345,
        /// <summary>
        /// down-arrow key
        /// </summary>
        DOWN = 258,
        /// <summary>
        /// up-arrow key
        /// </summary>
        UP = 259,
        /// <summary>
        /// left-arrow key
        /// </summary>
        LEFT = 260,
        /// <summary>
        /// right-arrow key
        /// </summary>
        RIGHT = 261,
        /// <summary>
        /// home key
        /// </summary>
        HOME = 262,
        /// <summary>
        /// backspace key
        /// </summary>
        BACKSPACE = 263,
        /// <summary>
        /// Function keys.  Space for = 4
        /// </summary>
        F0 = 264,
        /// <summary>
        /// Function key 1
        /// </summary>
        F1 = 265,
        /// <summary>
        /// Function key 2
        /// </summary>
        F2 = 266,
        /// <summary>
        /// Function key 3
        /// </summary>
        F3 = 267,
        /// <summary>
        /// Function key 4
        /// </summary>
        F4 = 268,
        /// <summary>
        /// Function key 5
        /// </summary>
        F5 = 269,
        /// <summary>
        /// Function key 6
        /// </summary>
        F6 = 270,
        /// <summary>
        /// Function key 7
        /// </summary>
        F7 = 271,
        /// <summary>
        /// Function key 8
        /// </summary>
        F8 = 272,
        /// <summary>
        /// Function key 9
        /// </summary>
        F9 = 273,
        /// <summary>
        /// Function key 10
        /// </summary>
        F10 = 274,
        /// <summary>
        /// Function key 11
        /// </summary>
        F11 = 275,
        /// <summary>
        /// Function key 12
        /// </summary>
        F12 = 276,
        /// <summary>
        /// Function key 13
        /// </summary>
        F13 = 277,
        /// <summary>
        /// Function key 14
        /// </summary>
        F14 = 278,
        /// <summary>
        /// Function key 15
        /// </summary>
        F15 = 279,
        /// <summary>
        /// Function key 16
        /// </summary>
        F16 = 280,
        /// <summary>
        /// Function key 17
        /// </summary>
        F17 = 281,
        /// <summary>
        /// Function key 18
        /// </summary>
        F18 = 282,
        /// <summary>
        /// Function key 19
        /// </summary>
        F19 = 283,
        /// <summary>
        /// Function key 20
        /// </summary>
        F20 = 284,
        /// <summary>
        /// Function key 21
        /// </summary>
        F21 = 285,
        /// <summary>
        /// Function key 22
        /// </summary>
        F22 = 286,
        /// <summary>
        /// Function key 23
        /// </summary>
        F23 = 287,
        /// <summary>
        /// Function key 24
        /// </summary>
        F24 = 288,
        /// <summary>
        /// delete-line key
        /// </summary>
        DL = 328,
        /// <summary>
        /// insert-line key
        /// </summary>
        IL = 329,
        /// <summary>
        /// delete-character key
        /// </summary>
        DC = 330,
        /// <summary>
        /// insert-character key
        /// </summary>
        IC = 331,
        /// <summary>
        /// sent by rmir or smir in insert mode
        /// </summary>
        EIC = 332,
        /// <summary>
        /// clear-screen or erase key
        /// </summary>
        CLEAR = 333,
        /// <summary>
        /// clear-to-end-of-screen key
        /// </summary>
        EOS = 334,
        /// <summary>
        /// clear-to-end-of-line key
        /// </summary>
        EOL = 335,
        /// <summary>
        /// scroll-forward key
        /// </summary>
        SF = 336,
        /// <summary>
        /// scroll-backward key
        /// </summary>
        SR = 337,
        /// <summary>
        /// next-page key
        /// </summary>
        NPAGE = 338,
        /// <summary>
        /// previous-page key
        /// </summary>
        PPAGE = 339,
        /// <summary>
        /// set-tab key
        /// </summary>
        STAB = 340,
        /// <summary>
        /// clear-tab key
        /// </summary>
        CTAB = 341,
        /// <summary>
        /// clear-all-tabs key
        /// </summary>
        CATAB = 342,
        /// <summary>
        /// enter/send key
        /// </summary>
        ENTER = 343,
        /// <summary>
        /// print key
        /// </summary>
        PRINT = 346,
        /// <summary>
        /// lower-left key (home down)
        /// </summary>
        LL = 347,
        /// <summary>
        /// upper left of keypad
        /// </summary>
        A1 = 348,
        /// <summary>
        /// upper right of keypad
        /// </summary>
        A3 = 349,
        /// <summary>
        /// center of keypad
        /// </summary>
        B2 = 350,
        /// <summary>
        /// lower left of keypad
        /// </summary>
        C1 = 351,
        /// <summary>
        /// lower right of keypad
        /// </summary>
        C3 = 352,
        /// <summary>
        /// back-tab key
        /// </summary>
        BTAB = 353,
        /// <summary>
        /// begin key
        /// </summary>
        BEG = 354,
        /// <summary>
        /// cancel key
        /// </summary>
        CANCEL = 355,
        /// <summary>
        /// close key
        /// </summary>
        CLOSE = 356,
        /// <summary>
        /// command key
        /// </summary>
        COMMAND = 357,
        /// <summary>
        /// copy key
        /// </summary>
        COPY = 358,
        /// <summary>
        /// create key
        /// </summary>
        CREATE = 359,
        /// <summary>
        /// end key
        /// </summary>
        END = 360,
        /// <summary>
        /// exit key
        /// </summary>
        EXIT = 361,
        /// <summary>
        /// find key
        /// </summary>
        FIND = 362,
        /// <summary>
        /// help key
        /// </summary>
        HELP = 363,
        /// <summary>
        /// mark key
        /// </summary>
        MARK = 364,
        /// <summary>
        /// message key
        /// </summary>
        MESSAGE = 365,
        /// <summary>
        /// move key
        /// </summary>
        MOVE = 366,
        /// <summary>
        /// next key
        /// </summary>
        NEXT = 367,
        /// <summary>
        /// open key
        /// </summary>
        OPEN = 368,
        /// <summary>
        /// options key
        /// </summary>
        OPTIONS = 369,
        /// <summary>
        /// previous key
        /// </summary>
        PREVIOUS = 370,
        /// <summary>
        /// redo key
        /// </summary>
        REDO = 371,
        /// <summary>
        /// reference key
        /// </summary>
        REFERENCE = 372,
        /// <summary>
        /// refresh key
        /// </summary>
        REFRESH = 373,
        /// <summary>
        /// replace key
        /// </summary>
        REPLACE = 374,
        /// <summary>
        /// restart key
        /// </summary>
        RESTART = 375,
        /// <summary>
        /// resume key
        /// </summary>
        RESUME = 376,
        /// <summary>
        /// save key
        /// </summary>
        SAVE = 377,
        /// <summary>
        /// shifted begin key
        /// </summary>
        SBEG = 378,
        /// <summary>
        /// shifted cancel key
        /// </summary>
        SCANCEL = 379,
        /// <summary>
        /// shifted command key
        /// </summary>
        SCOMMAND = 380,
        /// <summary>
        /// shifted copy key
        /// </summary>
        SCOPY = 381,
        /// <summary>
        /// shifted create key
        /// </summary>
        SCREATE = 382,
        /// <summary>
        /// shifted delete-character key
        /// </summary>
        SDC = 383,
        /// <summary>
        /// shifted delete-line key
        /// </summary>
        SDL = 384,
        /// <summary>
        /// select key
        /// </summary>
        SELECT = 385,
        /// <summary>
        /// shifted end key
        /// </summary>
        SEND = 386,
        /// <summary>
        /// shifted clear-to-end-of-line key
        /// </summary>
        SEOL = 387,
        /// <summary>
        /// shifted exit key
        /// </summary>
        SEXIT = 388,
        /// <summary>
        /// shifted find key
        /// </summary>
        SFIND = 389,
        /// <summary>
        /// shifted help key
        /// </summary>
        SHELP = 390,
        /// <summary>
        /// shifted home key
        /// </summary>
        SHOME = 391,
        /// <summary>
        /// shifted insert-character key
        /// </summary>
        SIC = 392,
        /// <summary>
        /// shifted left-arrow key
        /// </summary>
        SLEFT = 393,
        /// <summary>
        /// shifted message key
        /// </summary>
        SMESSAGE = 394,
        /// <summary>
        /// shifted move key
        /// </summary>
        SMOVE = 395,
        /// <summary>
        /// shifted next key
        /// </summary>
        SNEXT = 396,
        /// <summary>
        /// shifted options key
        /// </summary>
        SOPTIONS = 397,
        /// <summary>
        /// shifted previous key
        /// </summary>
        SPREVIOUS = 398,
        /// <summary>
        /// shifted print key
        /// </summary>
        SPRINT = 399,
        /// <summary>
        /// shifted redo key
        /// </summary>
        SREDO = 400,
        /// <summary>
        /// shifted replace key
        /// </summary>
        SREPLACE = 401,
        /// <summary>
        /// shifted right-arrow key
        /// </summary>
        SRIGHT = 402,
        /// <summary>
        /// shifted resume key
        /// </summary>
        SRSUME = 403,
        /// <summary>
        /// shifted save key
        /// </summary>
        SSAVE = 404,
        /// <summary>
        /// shifted suspend key
        /// </summary>
        SSUSPEND = 405,
        /// <summary>
        /// shifted undo key
        /// </summary>
        SUNDO = 406,
        /// <summary>
        /// suspend key
        /// </summary>
        SUSPEND = 407,
        /// <summary>
        /// undo key
        /// </summary>
        UNDO = 408,
        /// <summary>
        /// Mouse event has occurred
        /// </summary>
        MOUSE = 409,
        /// <summary>
        /// Terminal resize event
        /// </summary>
        RESIZE = 410,
        /// <summary>
        /// We were interrupted by an event
        /// </summary>
        EVENT = 411,

        /// <summary>
        /// Maximum key value is 0633
        /// </summary>
        MAX = 511
    }

    //TODO: verify
    public static class MouseState
    {
        public static ulong BUTTON_RELEASED = 1U;
        public static ulong BUTTON_PRESSED = 2U;
        public static ulong BUTTON_CLICKED = 4U;
        public static ulong DOUBLE_CLICKED = 10U;
        public static ulong TRIPLE_CLICKED = 20U;
        public static ulong RESERVED_EVENT = 40U;

        /* event masks */
        public static ulong BUTTON1_RELEASED = Constants.NCURSES_MOUSE_MASK(1, BUTTON_RELEASED);
        public static ulong BUTTON1_PRESSED = Constants.NCURSES_MOUSE_MASK(1, BUTTON_PRESSED);
        public static ulong BUTTON1_CLICKED = Constants.NCURSES_MOUSE_MASK(1, BUTTON_CLICKED);
        public static ulong BUTTON1_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(1, DOUBLE_CLICKED);
        public static ulong BUTTON1_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(1, TRIPLE_CLICKED);

        public static ulong BUTTON2_RELEASED = Constants.NCURSES_MOUSE_MASK(2, BUTTON_RELEASED);
        public static ulong BUTTON2_PRESSED = Constants.NCURSES_MOUSE_MASK(2, BUTTON_PRESSED);
        public static ulong BUTTON2_CLICKED = Constants.NCURSES_MOUSE_MASK(2, BUTTON_CLICKED);
        public static ulong BUTTON2_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(2, DOUBLE_CLICKED);
        public static ulong BUTTON2_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(2, TRIPLE_CLICKED);

        public static ulong BUTTON3_RELEASED = Constants.NCURSES_MOUSE_MASK(3, BUTTON_RELEASED);
        public static ulong BUTTON3_PRESSED = Constants.NCURSES_MOUSE_MASK(3, BUTTON_PRESSED);
        public static ulong BUTTON3_CLICKED = Constants.NCURSES_MOUSE_MASK(3, BUTTON_CLICKED);
        public static ulong BUTTON3_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(3, DOUBLE_CLICKED);
        public static ulong BUTTON3_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(3, TRIPLE_CLICKED);

        public static ulong BUTTON4_RELEASED = Constants.NCURSES_MOUSE_MASK(4, BUTTON_RELEASED);
        public static ulong BUTTON4_PRESSED = Constants.NCURSES_MOUSE_MASK(4, BUTTON_PRESSED);
        public static ulong BUTTON4_CLICKED = Constants.NCURSES_MOUSE_MASK(4, BUTTON_CLICKED);
        public static ulong BUTTON4_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(4, DOUBLE_CLICKED);
        public static ulong BUTTON4_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(4, TRIPLE_CLICKED);

        //TODO: everything below could be different on x86
        public static ulong BUTTON5_RELEASED = Constants.NCURSES_MOUSE_MASK(5, BUTTON_RELEASED);
        public static ulong BUTTON5_PRESSED = Constants.NCURSES_MOUSE_MASK(5, BUTTON_PRESSED);
        public static ulong BUTTON5_CLICKED = Constants.NCURSES_MOUSE_MASK(5, BUTTON_CLICKED);
        public static ulong BUTTON5_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(5, DOUBLE_CLICKED);
        public static ulong BUTTON5_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(5, TRIPLE_CLICKED);

        public static ulong BUTTON_CTRL = Constants.NCURSES_MOUSE_MASK(6, 1U);
        public static ulong BUTTON_SHIFT = Constants.NCURSES_MOUSE_MASK(6, 2U);
        public static ulong BUTTON_ALT = Constants.NCURSES_MOUSE_MASK(6, 4U);

        public static ulong REPORT_MOUSE_POSITION = Constants.NCURSES_MOUSE_MASK(6, 10U);
        public static ulong ALL_MOUSE_EVENTS = REPORT_MOUSE_POSITION - 1;
    }

    /// <summary>
    /// Line drawing characters, these can only be drawn (presented) within NCurses
    /// </summary>
    public static class Acs
    {
        /// <summary>
        /// upper left corner
        /// </summary>
        public static INCursesChar ULCORNER => NativeNCurses.NCurses.ACSMap['l'];     /* NCURSES_ACS('l') */
        /// <summary>
        /// lower left corner
        /// </summary>
        public static INCursesChar LLCORNER => NativeNCurses.NCurses.ACSMap['m'];     /* NCURSES_ACS('m') */
        /// <summary>
        /// upper right corner
        /// </summary>
        public static INCursesChar URCORNER => NativeNCurses.NCurses.ACSMap['k'];     /* NCURSES_ACS('k') */
        /// <summary>
        /// lower right corner
        /// </summary>
        public static INCursesChar LRCORNER => NativeNCurses.NCurses.ACSMap['j'];     /* NCURSES_ACS('j') */
        /// <summary>
        /// tee pointing right
        /// </summary>
        public static INCursesChar LTEE => NativeNCurses.NCurses.ACSMap['t'];     /* NCURSES_ACS('t') */
        /// <summary>
        /// tee pointing left
        /// </summary>
        public static INCursesChar RTEE => NativeNCurses.NCurses.ACSMap['u'];     /* NCURSES_ACS('u') */
        /// <summary>
        /// tee pointing up
        /// </summary>
        public static INCursesChar BTEE => NativeNCurses.NCurses.ACSMap['v'];     /* NCURSES_ACS('v') */
        /// <summary>
        /// tee pointing down
        /// </summary>
        public static INCursesChar TTEE => NativeNCurses.NCurses.ACSMap['w'];     /* NCURSES_ACS('w') */
        /// <summary>
        /// horizontal line
        /// </summary>
        public static INCursesChar HLINE => NativeNCurses.NCurses.ACSMap['q'];    /* NCURSES_ACS('q') */
        /// <summary>
        /// vertical line
        /// </summary>
        public static INCursesChar VLINE => NativeNCurses.NCurses.ACSMap['x'];    /* NCURSES_ACS('x') */
        /// <summary>
        /// large plus or crossover
        /// </summary>
        public static INCursesChar PLUS => NativeNCurses.NCurses.ACSMap['n'];     /* NCURSES_ACS('n') */
        /// <summary>
        /// scan line 1
        /// </summary>
        public static INCursesChar S1 => NativeNCurses.NCurses.ACSMap['o'];       /* NCURSES_ACS('o') */
        /// <summary>
        /// scan line 9
        /// </summary>
        public static INCursesChar S9 => NativeNCurses.NCurses.ACSMap['s'];       /* NCURSES_ACS('s') */
        /// <summary>
        /// diamond
        /// </summary>
        public static INCursesChar DIAMOND => NativeNCurses.NCurses.ACSMap['`'];  /* NCURSES_ACS('`') */
        /// <summary>
        /// checker board (stipple)
        /// </summary>
        public static INCursesChar CKBOARD => NativeNCurses.NCurses.ACSMap['a'];  /* NCURSES_ACS('a') */
        /// <summary>
        /// degree symbol
        /// </summary>
        public static INCursesChar DEGREE => NativeNCurses.NCurses.ACSMap['f'];   /* NCURSES_ACS('f') */
        /// <summary>
        /// plus/minus
        /// </summary>
        public static INCursesChar PLMINUS => NativeNCurses.NCurses.ACSMap['g'];  /* NCURSES_ACS('g') */
        /// <summary>
        /// bullet
        /// </summary>
        public static INCursesChar BULLET => NativeNCurses.NCurses.ACSMap['~'];   /* NCURSES_ACS('~') */

#region Teletype 5410v1
        /// <summary>
        /// arrow pointing left
        /// </summary>
        public static INCursesChar LARROW => NativeNCurses.NCurses.ACSMap[','];   /* NCURSES_ACS(',') */
        /// <summary>
        /// arrow pointing right
        /// </summary>
        public static INCursesChar RARROW => NativeNCurses.NCurses.ACSMap['+'];   /* NCURSES_ACS('+') */
        /// <summary>
        /// arrow pointing down
        /// </summary>
        public static INCursesChar DARROW => NativeNCurses.NCurses.ACSMap['.'];  /* NCURSES_ACS('.')  */
        /// <summary>
        /// arrow pointing up
        /// </summary>
        public static INCursesChar UARROW => NativeNCurses.NCurses.ACSMap['-'];   /* NCURSES_ACS('-') */
        /// <summary>
        /// board of squares
        /// </summary>
        public static INCursesChar BOARD => NativeNCurses.NCurses.ACSMap['h'];    /* NCURSES_ACS('h') */
        /// <summary>
        /// lantern symbol
        /// </summary>
        public static INCursesChar LANTERN => NativeNCurses.NCurses.ACSMap['i'];  /* NCURSES_ACS('i') */
        /// <summary>
        /// solid square block
        /// </summary>
        public static INCursesChar BLOCK => NativeNCurses.NCurses.ACSMap['0'];    /* NCURSES_ACS('0') */
#endregion

        /*
         * These aren't documented, but a lot of System Vs have them anyway
         * (you can spot pprryyzz{{||}} in a lot of AT&T terminfo strings).
         * The ACS_names may not match AT&T's, our source didn't know them.
         */
#region AT&T
        /// <summary>
        /// scan line 3
        /// </summary>
        public static INCursesChar S3 => NativeNCurses.NCurses.ACSMap['p'];       /* NCURSES_ACS('p') */
        /// <summary>
        /// scan line 7
        /// </summary>
        public static INCursesChar S7 => NativeNCurses.NCurses.ACSMap['r'];       /* NCURSES_ACS('r') */
        /// <summary>
        /// less/equal
        /// </summary>
        public static INCursesChar LEQUAL => NativeNCurses.NCurses.ACSMap['y'];   /* NCURSES_ACS('y') */
        /// <summary>
        /// greater/equal
        /// </summary>
        public static INCursesChar GEQUAL => NativeNCurses.NCurses.ACSMap['z'];   /* NCURSES_ACS('z') */
        /// <summary>
        /// Pi
        /// </summary>
        public static INCursesChar PI => NativeNCurses.NCurses.ACSMap['{'];       /* NCURSES_ACS('{') */
        /// <summary>
        /// not equal
        /// </summary>
        public static INCursesChar NEQUAL => NativeNCurses.NCurses.ACSMap['|'];   /* NCURSES_ACS('|') */
        /// <summary>
        /// UK pound sign
        /// </summary>
        public static INCursesChar STERLING => NativeNCurses.NCurses.ACSMap['}'];     /* NCURSES_ACS('}') */
#endregion
    }

    /// <summary>
    /// Wide line drawing characters
    /// </summary>
    public static class Wacs
    {
        /// <summary>
        /// upper left corner
        /// </summary>
        public static INCursesChar ULCORNER => NativeNCurses.NCurses.WACSMap['l'];     /* NCURSES_WACS('l') */
        /// <summary>
        /// lower left corner
        /// </summary>
        public static INCursesChar LLCORNER => NativeNCurses.NCurses.WACSMap['m'];     /* NCURSES_WACS('m') */
        /// <summary>
        /// upper right corner
        /// </summary>
        public static INCursesChar URCORNER => NativeNCurses.NCurses.WACSMap['k'];     /* NCURSES_WACS('k') */
        /// <summary>
        /// lower right corner
        /// </summary>
        public static INCursesChar LRCORNER => NativeNCurses.NCurses.WACSMap['j'];     /* NCURSES_WACS('j') */
        /// <summary>
        /// tee pointing right
        /// </summary>
        public static INCursesChar LTEE => NativeNCurses.NCurses.WACSMap['t'];     /* NCURSES_WACS('t') */
        /// <summary>
        /// tee pointing left
        /// </summary>
        public static INCursesChar RTEE => NativeNCurses.NCurses.WACSMap['u'];     /* NCURSES_WACS('u') */
        /// <summary>
        /// tee pointing up
        /// </summary>
        public static INCursesChar BTEE => NativeNCurses.NCurses.WACSMap['v'];     /* NCURSES_WACS('v') */
        /// <summary>
        /// tee pointing down
        /// </summary>
        public static INCursesChar TTEE => NativeNCurses.NCurses.WACSMap['w'];     /* NCURSES_WACS('w') */
        /// <summary>
        /// horizontal line
        /// </summary>
        public static INCursesChar HLINE => NativeNCurses.NCurses.WACSMap['q'];    /* NCURSES_WACS('q') */
        /// <summary>
        /// vertical line
        /// </summary>
        public static INCursesChar VLINE => NativeNCurses.NCurses.WACSMap['x'];    /* NCURSES_WACS('x') */
        /// <summary>
        /// large plus or crossover
        /// </summary>
        public static INCursesChar PLUS => NativeNCurses.NCurses.WACSMap['n'];     /* NCURSES_WACS('n') */
        /// <summary>
        /// scan line 1
        /// </summary>
        public static INCursesChar S1 => NativeNCurses.NCurses.WACSMap['o'];       /* NCURSES_WACS('o') */
        /// <summary>
        /// scan line 9
        /// </summary>
        public static INCursesChar S9 => NativeNCurses.NCurses.WACSMap['s'];   /* NCURSES_WACS('s') */
        /// <summary>
        /// diamond
        /// </summary>
        public static INCursesChar DIAMOND => NativeNCurses.NCurses.WACSMap['`'];  /* NCURSES_WACS('`') */
        /// <summary>
        /// checker board
        /// </summary>
        public static INCursesChar CKBOARD => NativeNCurses.NCurses.WACSMap['a'];  /* NCURSES_WACS('a') */
        /// <summary>
        /// degree symbol
        /// </summary>
        public static INCursesChar DEGREE => NativeNCurses.NCurses.WACSMap['f'];   /* NCURSES_WACS('f') */
        /// <summary>
        /// plus/minus
        /// </summary>
        public static INCursesChar PLMINUS => NativeNCurses.NCurses.WACSMap['g'];  /* NCURSES_WACS('g') */
        /// <summary>
        /// bullet
        /// </summary>
        public static INCursesChar BULLET => NativeNCurses.NCurses.WACSMap['~'];   /* NCURSES_WACS('~') */

#region Teletype 5410v1 symbols
        /// <summary>
        /// arrow left
        /// </summary>
        public static INCursesChar LARROW => NativeNCurses.NCurses.WACSMap[','];   /* NCURSES_WACS(',') */
        /// <summary>
        /// arrow right
        /// </summary>
        public static INCursesChar RARROW => NativeNCurses.NCurses.WACSMap['+'];   /* NCURSES_WACS('+') */
        /// <summary>
        /// arrow down
        /// </summary>
        public static INCursesChar DARROW => NativeNCurses.NCurses.WACSMap['.'];   /* NCURSES_WACS('.') */
        /// <summary>
        /// arrow up
        /// </summary>
        public static INCursesChar UARROW => NativeNCurses.NCurses.WACSMap['-'];   /* NCURSES_WACS('-') */
        /// <summary>
        /// board of squares
        /// </summary>
        public static INCursesChar BOARD => NativeNCurses.NCurses.WACSMap['h'];    /* NCURSES_WACS('h') */
        /// <summary>
        /// lantern symbol
        /// </summary>
        public static INCursesChar LANTERN => NativeNCurses.NCurses.WACSMap['i'];  /* NCURSES_WACS('i') */
        /// <summary>
        /// solid square block
        /// </summary>
        public static INCursesChar BLOCK => NativeNCurses.NCurses.WACSMap['0'];    /* NCURSES_WACS('0') */
#endregion

#region ncurses extensions
        /// <summary>
        /// scan line 3
        /// </summary>
        public static INCursesChar S3 => NativeNCurses.NCurses.WACSMap['p'];       /* NCURSES_WACS('p') */
        /// <summary>
        /// scan line 7
        /// </summary>
        public static INCursesChar S7 => NativeNCurses.NCurses.WACSMap['r'];       /* NCURSES_WACS('r') */
        /// <summary>
        /// less/equal
        /// </summary>
        public static INCursesChar LEQUAL => NativeNCurses.NCurses.WACSMap['y'];   /* NCURSES_WACS('y') */
        /// <summary>
        /// greater/equal
        /// </summary>
        public static INCursesChar GEQUAL => NativeNCurses.NCurses.WACSMap['z'];   /* NCURSES_WACS('z') */
        /// <summary>
        /// Pi
        /// </summary>
        public static INCursesChar PI => NativeNCurses.NCurses.WACSMap['{'];       /* NCURSES_WACS('{') */
        /// <summary>
        /// not equal
        /// </summary>
        public static INCursesChar NEQUAL => NativeNCurses.NCurses.WACSMap['|'];   /* NCURSES_WACS('|') */
        /// <summary>
        /// UK pound sign
        /// </summary>
        public static INCursesChar STERLING => NativeNCurses.NCurses.WACSMap['}'];     /* NCURSES_WACS('}') */
#endregion

#region double lines
        public static INCursesChar D_ULCORNER => NativeNCurses.NCurses.WACSMap['C'];  /* NCURSES_WACS('C') */
        public static INCursesChar D_LLCORNER => NativeNCurses.NCurses.WACSMap['D'];  /* NCURSES_WACS('D') */
        public static INCursesChar D_URCORNER => NativeNCurses.NCurses.WACSMap['B'];  /* NCURSES_WACS('B') */
        public static INCursesChar D_LRCORNER => NativeNCurses.NCurses.WACSMap['A'];  /* NCURSES_WACS('A') */
        public static INCursesChar D_RTEE => NativeNCurses.NCurses.WACSMap['G'];  /* NCURSES_WACS('G') */
        public static INCursesChar D_LTEE => NativeNCurses.NCurses.WACSMap['F'];  /* NCURSES_WACS('F') */
        public static INCursesChar D_BTEE => NativeNCurses.NCurses.WACSMap['H'];  /* NCURSES_WACS('H') */
        public static INCursesChar D_TTEE => NativeNCurses.NCurses.WACSMap['I'];  /* NCURSES_WACS('I') */
        public static INCursesChar D_HLINE => NativeNCurses.NCurses.WACSMap['R']; /* NCURSES_WACS('R') */
        public static INCursesChar D_VLINE => NativeNCurses.NCurses.WACSMap['Y']; /* NCURSES_WACS('Y') */
        public static INCursesChar D_PLUS => NativeNCurses.NCurses.WACSMap['E'];  /* NCURSES_WACS('E') */
#endregion

#region thick lines
        public static INCursesChar T_ULCORNER => NativeNCurses.NCurses.WACSMap['L'];  /* NCURSES_WACS('L') */
        public static INCursesChar T_LLCORNER => NativeNCurses.NCurses.WACSMap['M'];  /* NCURSES_WACS('M') */
        public static INCursesChar T_URCORNER => NativeNCurses.NCurses.WACSMap['K'];  /* NCURSES_WACS('K') */
        public static INCursesChar T_LRCORNER => NativeNCurses.NCurses.WACSMap['J'];  /* NCURSES_WACS('J') */
        public static INCursesChar T_RTEE => NativeNCurses.NCurses.WACSMap['U'];  /* NCURSES_WACS('U') */
        public static INCursesChar T_LTEE => NativeNCurses.NCurses.WACSMap['T'];  /* NCURSES_WACS('T') */
        public static INCursesChar T_BTEE => NativeNCurses.NCurses.WACSMap['V'];  /* NCURSES_WACS('V') */
        public static INCursesChar T_TTEE => NativeNCurses.NCurses.WACSMap['W'];  /* NCURSES_WACS('W') */
        public static INCursesChar T_HLINE => NativeNCurses.NCurses.WACSMap['Q']; /* NCURSES_WACS('Q') */
        public static INCursesChar T_VLINE => NativeNCurses.NCurses.WACSMap['X']; /* NCURSES_WACS('X') */
        public static INCursesChar T_PLUS => NativeNCurses.NCurses.WACSMap['N'];	/* NCURSES_WACS('N') */
#endregion
    }
}
