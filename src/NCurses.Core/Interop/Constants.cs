using System;
using System.Runtime.InteropServices;

//several constants taken from ncurses.h
namespace NCurses.Core.Interop
{
    public static class Constants
    {
        private static int NCURSES_ATTR_SHIFT = 8;

        internal static string DLLNAME { get; private set; }
        internal static int SIZEOF_WCHAR_T { get; private set; }
        internal const int CCHARW_MAX = 5;

        public const int ERR = -1;
        public const int OK = 0;

        //TODO: get WCHAR_T size at runtime (through libc?)
        static Constants()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                SIZEOF_WCHAR_T = 2;
                DLLNAME = "libncursesw6";
            }
            else
            {
                SIZEOF_WCHAR_T = 4;
                DLLNAME = "libncursesw.so.6";
            }
        }

        internal static uint NCURSES_BITS(uint mask, int shift)
        {
            return mask << (shift + NCURSES_ATTR_SHIFT);
        }

        internal static uint NCURSES_MOUSE_MASK(int button, uint mask)
        {
            return mask << (((button) - 1) * 5);
        }

        public static uint COLOR_PAIR(uint number)
        {
            return (NCURSES_BITS(number, 0) & Attrs.COLOR);
        }

        public static int PAIR_NUMBER(uint attr)
        {
            return (int)((attr & Attrs.COLOR) >> NCURSES_ATTR_SHIFT);
        }
    }

    /// <summary>
    /// Character attributes
    /// </summary>
    public static class Attrs
    {
        public static uint NORMAL = (1U - 1U);
        public static uint ATTRIBUTES = Constants.NCURSES_BITS(~(1U - 1U), 0);
        public static uint CHARTEXT = Constants.NCURSES_BITS(1U, 0) - 1U;
        public static uint COLOR = Constants.NCURSES_BITS(((1U) << 8) - 1U, 0);
        public static uint STANDOUT = Constants.NCURSES_BITS(1U, 8);
        public static uint UNDERLINE = Constants.NCURSES_BITS(1U, 9);
        public static uint REVERSE = Constants.NCURSES_BITS(1U, 10);
        public static uint BLINK = Constants.NCURSES_BITS(1U, 11);
        public static uint DIM = Constants.NCURSES_BITS(1U, 12);
        public static uint BOLD = Constants.NCURSES_BITS(1U, 13);
        public static uint ALTCHARSET = Constants.NCURSES_BITS(1U, 14);
        public static uint INVIS = Constants.NCURSES_BITS(1U, 15);
        public static uint PROTECT = Constants.NCURSES_BITS(1U, 16);
        public static uint HORIZONTAL = Constants.NCURSES_BITS(1U, 17);
        public static uint LEFT = Constants.NCURSES_BITS(1U, 18);
        public static uint LOW = Constants.NCURSES_BITS(1U, 19);
        public static uint RIGHT = Constants.NCURSES_BITS(1U, 20);
        public static uint TOP = Constants.NCURSES_BITS(1U, 21);
        public static uint VERTICAL = Constants.NCURSES_BITS(1U, 22);
        public static uint ITALIC = Constants.NCURSES_BITS(1U, 23);
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
        public static uint BUTTON_RELEASED = 1U;
        public static uint BUTTON_PRESSED = 2U;
        public static uint BUTTON_CLICKED = 4U;
        public static uint DOUBLE_CLICKED = 10U;
        public static uint TRIPLE_CLICKED = 20U;
        public static uint RESERVED_EVENT = 40U;

        /* event masks */
        public static uint BUTTON1_RELEASED = Constants.NCURSES_MOUSE_MASK(1, BUTTON_RELEASED);
        public static uint BUTTON1_PRESSED = Constants.NCURSES_MOUSE_MASK(1, BUTTON_PRESSED);
        public static uint BUTTON1_CLICKED = Constants.NCURSES_MOUSE_MASK(1, BUTTON_CLICKED);
        public static uint BUTTON1_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(1, DOUBLE_CLICKED);
        public static uint BUTTON1_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(1, TRIPLE_CLICKED);

        public static uint BUTTON2_RELEASED = Constants.NCURSES_MOUSE_MASK(2, BUTTON_RELEASED);
        public static uint BUTTON2_PRESSED = Constants.NCURSES_MOUSE_MASK(2, BUTTON_PRESSED);
        public static uint BUTTON2_CLICKED = Constants.NCURSES_MOUSE_MASK(2, BUTTON_CLICKED);
        public static uint BUTTON2_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(2, DOUBLE_CLICKED);
        public static uint BUTTON2_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(2, TRIPLE_CLICKED);

        public static uint BUTTON3_RELEASED = Constants.NCURSES_MOUSE_MASK(3, BUTTON_RELEASED);
        public static uint BUTTON3_PRESSED = Constants.NCURSES_MOUSE_MASK(3, BUTTON_PRESSED);
        public static uint BUTTON3_CLICKED = Constants.NCURSES_MOUSE_MASK(3, BUTTON_CLICKED);
        public static uint BUTTON3_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(3, DOUBLE_CLICKED);
        public static uint BUTTON3_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(3, TRIPLE_CLICKED);

        public static uint BUTTON4_RELEASED = Constants.NCURSES_MOUSE_MASK(4, BUTTON_RELEASED);
        public static uint BUTTON4_PRESSED = Constants.NCURSES_MOUSE_MASK(4, BUTTON_PRESSED);
        public static uint BUTTON4_CLICKED = Constants.NCURSES_MOUSE_MASK(4, BUTTON_CLICKED);
        public static uint BUTTON4_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(4, DOUBLE_CLICKED);
        public static uint BUTTON4_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(4, TRIPLE_CLICKED);

        //TODO: everything below could be different on x86
        public static uint BUTTON5_RELEASED = Constants.NCURSES_MOUSE_MASK(5, BUTTON_RELEASED);
        public static uint BUTTON5_PRESSED = Constants.NCURSES_MOUSE_MASK(5, BUTTON_PRESSED);
        public static uint BUTTON5_CLICKED = Constants.NCURSES_MOUSE_MASK(5, BUTTON_CLICKED);
        public static uint BUTTON5_DOUBLE_CLICKED = Constants.NCURSES_MOUSE_MASK(5, DOUBLE_CLICKED);
        public static uint BUTTON5_TRIPLE_CLICKED = Constants.NCURSES_MOUSE_MASK(5, TRIPLE_CLICKED);

        public static uint BUTTON_CTRL = Constants.NCURSES_MOUSE_MASK(6, 1U);
        public static uint BUTTON_SHIFT = Constants.NCURSES_MOUSE_MASK(6, 2U);
        public static uint BUTTON_ALT = Constants.NCURSES_MOUSE_MASK(6, 4U);
        public static uint REPORT_MOUSE_POSITION = Constants.NCURSES_MOUSE_MASK(6, 10U);

        public static uint ALL_MOUSE_EVENTS = REPORT_MOUSE_POSITION - 1;
    }

    /// <summary>
    /// Line drawing characters
    /// </summary>
    public static class Acs
    {
        /// <summary>
        /// upper left corner
        /// </summary>
        public static uint ULCORNER = NativeNCurses.Acs_Map['l'];     /* NCURSES_ACS('l') */
        /// <summary>
        /// lower left corner
        /// </summary>
        public static uint LLCORNER = NativeNCurses.Acs_Map['m'];     /* NCURSES_ACS('m') */
        /// <summary>
        /// upper right corner
        /// </summary>
        public static uint URCORNER = NativeNCurses.Acs_Map['k'];     /* NCURSES_ACS('k') */
        /// <summary>
        /// lower right corner
        /// </summary>
        public static uint LRCORNER = NativeNCurses.Acs_Map['j'];     /* NCURSES_ACS('j') */
        /// <summary>
        /// tee pointing right
        /// </summary>
        public static uint LTEE = NativeNCurses.Acs_Map['t'];     /* NCURSES_ACS('t') */
        /// <summary>
        /// tee pointing left
        /// </summary>
        public static uint RTEE = NativeNCurses.Acs_Map['u'];     /* NCURSES_ACS('u') */
        /// <summary>
        /// tee pointing up
        /// </summary>
        public static uint BTEE = NativeNCurses.Acs_Map['v'];     /* NCURSES_ACS('v') */
        /// <summary>
        /// tee pointing down
        /// </summary>
        public static uint TTEE = NativeNCurses.Acs_Map['w'];     /* NCURSES_ACS('w') */
        /// <summary>
        /// horizontal line
        /// </summary>
        public static uint HLINE = NativeNCurses.Acs_Map['q'];    /* NCURSES_ACS('q') */
        /// <summary>
        /// vertical line
        /// </summary>
        public static uint VLINE = NativeNCurses.Acs_Map['x'];    /* NCURSES_ACS('x') */
        /// <summary>
        /// large plus or crossover
        /// </summary>
        public static uint PLUS = NativeNCurses.Acs_Map['n'];     /* NCURSES_ACS('n') */
        /// <summary>
        /// scan line 1
        /// </summary>
        public static uint S1 = NativeNCurses.Acs_Map['o'];       /* NCURSES_ACS('o') */
        /// <summary>
        /// scan line 9
        /// </summary>
        public static uint S9 = NativeNCurses.Acs_Map['s'];       /* NCURSES_ACS('s') */
        /// <summary>
        /// diamond
        /// </summary>
        public static uint DIAMOND = NativeNCurses.Acs_Map['`'];  /* NCURSES_ACS('`') */
        /// <summary>
        /// checker board (stipple)
        /// </summary>
        public static uint CKBOARD = NativeNCurses.Acs_Map['a'];  /* NCURSES_ACS('a') */
        /// <summary>
        /// degree symbol
        /// </summary>
        public static uint DEGREE = NativeNCurses.Acs_Map['f'];   /* NCURSES_ACS('f') */
        /// <summary>
        /// plus/minus
        /// </summary>
        public static uint PLMINUS = NativeNCurses.Acs_Map['g'];  /* NCURSES_ACS('g') */
        /// <summary>
        /// bullet
        /// </summary>
        public static uint BULLET = NativeNCurses.Acs_Map['~'];   /* NCURSES_ACS('~') */

        #region Teletype 5410v1
        /// <summary>
        /// arrow pointing left
        /// </summary>
        public static uint LARROW = NativeNCurses.Acs_Map[','];   /* NCURSES_ACS(',') */
        /// <summary>
        /// arrow pointing right
        /// </summary>
        public static uint RARROW = NativeNCurses.Acs_Map['+'];   /* NCURSES_ACS('+') */
        /// <summary>
        /// arrow pointing down
        /// </summary>
        public static uint DARROW = NativeNCurses.Acs_Map['.'];  /* NCURSES_ACS('.')  */
        /// <summary>
        /// arrow pointing up
        /// </summary>
        public static uint UARROW = NativeNCurses.Acs_Map['-'];   /* NCURSES_ACS('-') */
        /// <summary>
        /// board of squares
        /// </summary>
        public static uint BOARD = NativeNCurses.Acs_Map['h'];    /* NCURSES_ACS('h') */
        /// <summary>
        /// lantern symbol
        /// </summary>
        public static uint LANTERN = NativeNCurses.Acs_Map['i'];  /* NCURSES_ACS('i') */
        /// <summary>
        /// solid square block
        /// </summary>
        public static uint BLOCK = NativeNCurses.Acs_Map['0'];    /* NCURSES_ACS('0') */
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
        public static uint S3 = NativeNCurses.Acs_Map['p'];       /* NCURSES_ACS('p') */
        /// <summary>
        /// scan line 7
        /// </summary>
        public static uint S7 = NativeNCurses.Acs_Map['r'];       /* NCURSES_ACS('r') */
        /// <summary>
        /// less/equal
        /// </summary>
        public static uint LEQUAL = NativeNCurses.Acs_Map['y'];   /* NCURSES_ACS('y') */
        /// <summary>
        /// greater/equal
        /// </summary>
        public static uint GEQUAL = NativeNCurses.Acs_Map['z'];   /* NCURSES_ACS('z') */
        /// <summary>
        /// Pi
        /// </summary>
        public static uint PI = NativeNCurses.Acs_Map['{'];       /* NCURSES_ACS('{') */
        /// <summary>
        /// not equal
        /// </summary>
        public static uint NEQUAL = NativeNCurses.Acs_Map['|'];   /* NCURSES_ACS('|') */
        /// <summary>
        /// UK pound sign
        /// </summary>
        public static uint STERLING = NativeNCurses.Acs_Map['}'];     /* NCURSES_ACS('}') */
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
        public static char ULCORNER = NativeNCurses.Wacs_Map['l'];     /* NCURSES_WACS('l') */
        /// <summary>
        /// lower left corner
        /// </summary>
        public static char LLCORNER = NativeNCurses.Wacs_Map['m'];     /* NCURSES_WACS('m') */
        /// <summary>
        /// upper right corner
        /// </summary>
        public static char URCORNER = NativeNCurses.Wacs_Map['k'];     /* NCURSES_WACS('k') */
        /// <summary>
        /// lower right corner
        /// </summary>
        public static char LRCORNER = NativeNCurses.Wacs_Map['j'];     /* NCURSES_WACS('j') */
        /// <summary>
        /// tee pointing right
        /// </summary>
        public static char LTEE = NativeNCurses.Wacs_Map['t'];     /* NCURSES_WACS('t') */
        /// <summary>
        /// tee pointing left
        /// </summary>
        public static char RTEE = NativeNCurses.Wacs_Map['u'];     /* NCURSES_WACS('u') */
        /// <summary>
        /// tee pointing up
        /// </summary>
        public static char BTEE = NativeNCurses.Wacs_Map['v'];     /* NCURSES_WACS('v') */
        /// <summary>
        /// tee pointing down
        /// </summary>
        public static char TTEE = NativeNCurses.Wacs_Map['w'];     /* NCURSES_WACS('w') */
        /// <summary>
        /// horizontal line
        /// </summary>
        public static char HLINE = NativeNCurses.Wacs_Map['q'];    /* NCURSES_WACS('q') */
        /// <summary>
        /// vertical line
        /// </summary>
        public static char VLINE = NativeNCurses.Wacs_Map['x'];    /* NCURSES_WACS('x') */
        /// <summary>
        /// large plus or crossover
        /// </summary>
        public static char PLUS = NativeNCurses.Wacs_Map['n'];     /* NCURSES_WACS('n') */
        /// <summary>
        /// scan line 1
        /// </summary>
        public static char S1 = NativeNCurses.Wacs_Map['o'];       /* NCURSES_WACS('o') */
        /// <summary>
        /// scan line 9
        /// </summary>
        public static char S9 = NativeNCurses.Wacs_Map['s'];   /* NCURSES_WACS('s') */
        /// <summary>
        /// diamond
        /// </summary>
        public static char DIAMOND = NativeNCurses.Wacs_Map['`'];  /* NCURSES_WACS('`') */
        /// <summary>
        /// checker board
        /// </summary>
        public static char CKBOARD = NativeNCurses.Wacs_Map['a'];  /* NCURSES_WACS('a') */
        /// <summary>
        /// degree symbol
        /// </summary>
        public static char DEGREE = NativeNCurses.Wacs_Map['f'];   /* NCURSES_WACS('f') */
        /// <summary>
        /// plus/minus
        /// </summary>
        public static char PLMINUS = NativeNCurses.Wacs_Map['g'];  /* NCURSES_WACS('g') */
        /// <summary>
        /// bullet
        /// </summary>
        public static char BULLET = NativeNCurses.Wacs_Map['~'];   /* NCURSES_WACS('~') */

        #region Teletype 5410v1 symbols
        /// <summary>
        /// arrow left
        /// </summary>
        public static char LARROW = NativeNCurses.Wacs_Map[','];   /* NCURSES_WACS(',') */
        /// <summary>
        /// arrow right
        /// </summary>
        public static char RARROW = NativeNCurses.Wacs_Map['+'];   /* NCURSES_WACS('+') */
        /// <summary>
        /// arrow down
        /// </summary>
        public static char DARROW = NativeNCurses.Wacs_Map['.'];   /* NCURSES_WACS('.') */
        /// <summary>
        /// arrow up
        /// </summary>
        public static char UARROW = NativeNCurses.Wacs_Map['-'];   /* NCURSES_WACS('-') */
        /// <summary>
        /// board of squares
        /// </summary>
        public static char BOARD = NativeNCurses.Wacs_Map['h'];    /* NCURSES_WACS('h') */
        /// <summary>
        /// lantern symbol
        /// </summary>
        public static char LANTERN = NativeNCurses.Wacs_Map['i'];  /* NCURSES_WACS('i') */
        /// <summary>
        /// solid square block
        /// </summary>
        public static char BLOCK = NativeNCurses.Wacs_Map['0'];    /* NCURSES_WACS('0') */
        #endregion

        #region ncurses extensions
        /// <summary>
        /// scan line 3
        /// </summary>
        public static char S3 = NativeNCurses.Wacs_Map['p'];       /* NCURSES_WACS('p') */
        /// <summary>
        /// scan line 7
        /// </summary>
        public static char S7 = NativeNCurses.Wacs_Map['r'];       /* NCURSES_WACS('r') */
        /// <summary>
        /// less/equal
        /// </summary>
        public static char LEQUAL = NativeNCurses.Wacs_Map['y'];   /* NCURSES_WACS('y') */
        /// <summary>
        /// greater/equal
        /// </summary>
        public static char GEQUAL = NativeNCurses.Wacs_Map['z'];   /* NCURSES_WACS('z') */
        /// <summary>
        /// Pi
        /// </summary>
        public static char PI = NativeNCurses.Wacs_Map['{'];       /* NCURSES_WACS('{') */
        /// <summary>
        /// not equal
        /// </summary>
        public static char NEQUAL = NativeNCurses.Wacs_Map['|'];   /* NCURSES_WACS('|') */
        /// <summary>
        /// UK pound sign
        /// </summary>
        public static char STERLING = NativeNCurses.Wacs_Map['}'];     /* NCURSES_WACS('}') */
        #endregion

        #region double lines
        public static char D_ULCORNER = NativeNCurses.Wacs_Map['C'];  /* NCURSES_WACS('C') */
        public static char D_LLCORNER = NativeNCurses.Wacs_Map['D'];  /* NCURSES_WACS('D') */
        public static char D_URCORNER = NativeNCurses.Wacs_Map['B'];  /* NCURSES_WACS('B') */
        public static char D_LRCORNER = NativeNCurses.Wacs_Map['A'];  /* NCURSES_WACS('A') */
        public static char D_RTEE = NativeNCurses.Wacs_Map['G'];  /* NCURSES_WACS('G') */
        public static char D_LTEE = NativeNCurses.Wacs_Map['F'];  /* NCURSES_WACS('F') */
        public static char D_BTEE = NativeNCurses.Wacs_Map['H'];  /* NCURSES_WACS('H') */
        public static char D_TTEE = NativeNCurses.Wacs_Map['I'];  /* NCURSES_WACS('I') */
        public static char D_HLINE = NativeNCurses.Wacs_Map['R']; /* NCURSES_WACS('R') */
        public static char D_VLINE = NativeNCurses.Wacs_Map['Y']; /* NCURSES_WACS('Y') */
        public static char D_PLUS = NativeNCurses.Wacs_Map['E'];  /* NCURSES_WACS('E') */
        #endregion

        #region thick lines
        public static char T_ULCORNER = NativeNCurses.Wacs_Map['L'];  /* NCURSES_WACS('L') */
        public static char T_LLCORNER = NativeNCurses.Wacs_Map['M'];  /* NCURSES_WACS('M') */
        public static char T_URCORNER = NativeNCurses.Wacs_Map['K'];  /* NCURSES_WACS('K') */
        public static char T_LRCORNER = NativeNCurses.Wacs_Map['J'];  /* NCURSES_WACS('J') */
        public static char T_RTEE = NativeNCurses.Wacs_Map['U'];  /* NCURSES_WACS('U') */
        public static char T_LTEE = NativeNCurses.Wacs_Map['T'];  /* NCURSES_WACS('T') */
        public static char T_BTEE = NativeNCurses.Wacs_Map['V'];  /* NCURSES_WACS('V') */
        public static char T_TTEE = NativeNCurses.Wacs_Map['W'];  /* NCURSES_WACS('W') */
        public static char T_HLINE = NativeNCurses.Wacs_Map['Q']; /* NCURSES_WACS('Q') */
        public static char T_VLINE = NativeNCurses.Wacs_Map['X']; /* NCURSES_WACS('X') */
        public static char T_PLUS = NativeNCurses.Wacs_Map['N'];	/* NCURSES_WACS('N') */
        #endregion
    }
}
