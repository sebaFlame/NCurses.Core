using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCurses.Core.Interop;

namespace NCurses.Core
{
    public abstract class WindowBase : IDisposable
    {
        protected static Dictionary<IntPtr, WindowBase> DictPtrWindows = new Dictionary<IntPtr, WindowBase>();
        public static IEnumerable<WindowBase> LstWindows
        {
            get { return DictPtrWindows.Values.Where(x => x.OwnsHandle) ; }
        }

        //TODO: change back to protected
        protected IntPtr WindowPtr { get; set; }
        protected bool OwnsHandle;

        internal WindowBase()
        {
            this.initialize();
            this.OwnsHandle = true;
        }

        internal WindowBase(IntPtr ptr, bool ownsHandle)
        {
            this.WindowPtr = ptr;
            this.OwnsHandle = ownsHandle;
            DictPtrWindows.Add(ptr, this);
        }

        ~WindowBase()
        {
            this.Dispose(true);
        }

        private void initialize()
        {
            if (NCurses.stdscr == null)
                NCurses.Start();
        }

        #region properties
        /// <summary>
        /// return the current column number of the cursor
        /// </summary>
        public int CursorColumn
        {
            get { return NativeWindow.getcurx(this.WindowPtr); }
        }

        /// <summary>
        /// return the current line number of the cursor
        /// </summary>
        public int CursorLine
        {
            get { return NativeWindow.getcury(this.WindowPtr); }
        }

        /// <summary>
        /// return the maximum column number
        /// </summary>
        public int MaxColumn
        {
            get { return NativeWindow.getmaxx(this.WindowPtr); }
        }

        /// <summary>
        /// return the maximum line number
        /// </summary>
        public int MaxLine
        {
            get { return NativeWindow.getmaxy(this.WindowPtr); }
        }

        /// <summary>
        /// set/get the current active color pair index
        /// </summary>
        public short Color
        {
            get
            {
                uint attrs;
                short color_pair;
                NativeWindow.wattr_get(this.WindowPtr, out attrs, out color_pair);
                return color_pair;
            }
            set
            {
                NativeWindow.wcolor_set(this.WindowPtr, value);
            }
        }

        /// <summary>
        /// set/get the current attribute/color attribute.
        /// get the enabled attributes by using AND on the corresponding <see cref="Attrs"/> field.
        /// </summary>
        public uint Attribute
        {
            get
            {
                uint attr;
                short color_pair;
                NativeWindow.wattr_get(this.WindowPtr, out attr, out color_pair);
                return attr;
            }
            set
            {
                NativeWindow.wattr_set(this.WindowPtr, value, 0);
            }
        }

        /// <summary>
        /// set/gets the current window background with a character with attributes applied
        /// </summary>
        public uint BackGround
        {
            get { return NativeWindow.getbkgd(this.WindowPtr); }
            set { NativeWindow.wbkgd(this.WindowPtr, value); }
        }

        //public Color BackgroundColor
        //{
        //    get
        //    {
        //        uint bgAttr = 0;
        //        if (NCurses.UnicodeSupported)
        //        {
        //            NCURSES_CH_T bkgd;
        //            NativeWindow.wgetbkgrnd(this.WindowPtr, out bkgd);
        //            bgAttr = bkgd.attr & Attrs.COLOR;
        //        }
        //        else
        //        {
        //            uint bkgd = NativeWindow.getbkgd(this.WindowPtr);
        //            bgAttr = bkgd & Attrs.COLOR;
        //        }

        //        short fg, bg;
        //        if (bgAttr > 0)
        //        {
        //            int color_pair = NativeNCurses.PAIR_NUMBER(bgAttr);
        //            NativeNCurses.pair_content((short)color_pair, out fg, out bg);
        //            return (Color)bg;
        //        }

        //        NativeNCurses.pair_content(0, out fg, out bg);
        //        return (Color)bg;
        //    }
        //    set
        //    {
        //        if (NCurses.UnicodeSupported)
        //        {
        //            NCURSES_CH_T bkgd = new NCURSES_CH_T(' ');
        //            bkgd.attr = (uint)NativeNCurses.COLOR_PAIR((int)value);
        //            NativeWindow.wbkgrnd(this.WindowPtr, bkgd);
        //        }
        //        else
        //        {
        //            uint bkgd = ' ';
        //            bkgd |= (uint)NativeNCurses.COLOR_PAIR((int)value);
        //            NativeWindow.wbkgd(this.WindowPtr, bkgd);
        //        }
        //    }
        //}

        //public Color ForegroundColor
        //{
        //    get
        //    {
        //        short fg, bg;
        //        NativeNCurses.pair_content(0, out fg, out bg);
        //        return (Color)fg;
        //    }
        //    set
        //    {
        //        NativeNCurses.assume_default_colors((int)value, -1);
        //    }
        //}

        /// <summary>
        /// enable/disable returning function keys on <see cref="ReadKey"/>  (Key.* defined in constants).
        /// disabled by default
        /// </summary>
        public bool KeyPad
        {
            get { return NativeWindow.is_keypad(this.WindowPtr); }
            set { NativeWindow.keypad(this.WindowPtr, value); }
        }

        /// <summary>
        /// enable/disable scrolling on the current window
        /// disabled by default
        /// </summary>
        public bool Scroll
        {
            get { return NativeWindow.is_scrollok(this.WindowPtr); }
            set { NativeWindow.scrollok(this.WindowPtr, value); }
        }

        public bool UseHwInsDelLine
        {
            get { return NativeWindow.is_idlok(this.WindowPtr); }
            set { NativeWindow.idlok(this.WindowPtr, value); }
        }

        //TODO: gives read error on windows when true
        public bool Blocking
        {
            get { return NativeWindow.is_nodelay(this.WindowPtr); }
            set { NativeWindow.nodelay(this.WindowPtr, value); }
        }

        public bool NoTimeout
        {
            set { NativeWindow.notimeout(this.WindowPtr, value); }
            get { return NativeWindow.is_notimeout(this.WindowPtr); }
        }
        #endregion

        #region attributes
        /// <summary>
        /// enable attribute(s) for the current window. Attributes can be OR'd together.
        /// </summary>
        /// <param name="attr">attribute(s) to enable</param>
        public void AttrOn(uint attr)
        {
            NativeWindow.wattr_on(this.WindowPtr, attr);
        }

        /// <summary>
        /// disable attribute(s) for the current window. Attaributes can be OR'd together.
        /// </summary>
        /// <param name="attr">attribute(s) to disable</param>
        public void AttrOff(uint attr)
        {
            NativeWindow.wattr_off(this.WindowPtr, attr);
        }
        #endregion

        #region Cursor
        /// <summary>
        /// move the cursor to position <paramref name="lineNumber"/> and <paramref name="columnNumber"/>
        /// </summary>
        /// <param name="lineNumber">the line number to move the cursor to</param>
        /// <param name="columnNumber">the column number to move the cursor to</param>
        public void MoveCursor(int lineNumber, int columnNumber)
        {
            NativeWindow.wmove(this.WindowPtr, lineNumber, columnNumber);
        }
        #endregion

        #region Write
        /// <summary>
        /// write string <paramref name="str"/> to the window.
        /// <paramref name="str"/> can be unicode if your terminal supports it.
        /// the cursor advances the length of the string.
        /// </summary>
        /// <param name="str">the string to write</param>
        public void Write(string str)
        {
            if (NCurses.UnicodeSupported)
                NativeWindow.waddwstr(this.WindowPtr, str);
            else
                NativeWindow.waddstr(this.WindowPtr, str);
        }

        /// <summary>
        /// write string <paramref name="str"/> to the window on line <paramref name="nline"/> and column <paramref name="ncol"/>.
        /// <paramref name="str"/> can be unicode if your terminal supports it.
        /// the cursor advances the length of the string
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="str">the string to add</param>
        public void Write(int nline, int ncol, string str)
        {
            if (NCurses.UnicodeSupported)
                NativeWindow.mvwaddwstr(this.WindowPtr, nline, ncol, str);
            else
                NativeWindow.mvwaddstr(this.WindowPtr, nline, ncol, str);
        }

        /// <summary>
        /// add a string with formatting. see <see cref="Write(string)"/>
        /// </summary>
        /// <param name="format">the string to format</param>
        /// <param name="arg">arguments to format with</param>
        public void Write(string format, params object[] arg)
        {
            this.Write(string.Format(format, arg));
        }

        /// <summary>
        /// add character OR'd together with attributes/color attributes.
        /// no unicode supported.
        /// the cursor advances.
        /// </summary>
        /// <param name="ch">character/attributes you want to add</param>
        public void Write(uint ch)
        {
            //if (NCurses.UnicodeSupported)
            //{
            //    uint c = ch & Attrs.CHARTEXT;
            //    uint a = ch & Attrs.ATTRIBUTES;
            //    uint col = ch & Attrs.COLOR;

            //    int pair = Constants.PAIR_NUMBER(col);
            //    NCURSES_CH_T wch = new NCURSES_CH_T(c);
            //    wch.attr = a;
            //    wch.ext_color = pair;
            //    NativeWindow.wadd_wch(this.WindowPtr, wch);
            //}
            //else
                NativeWindow.waddch(this.WindowPtr, ch);
        }

        /// <summary>
        /// write character <paramref name="ch"/> to the window with defined attributes/color pair.
        /// no unicode support.
        /// the cursor advances.
        /// </summary>
        /// <param name="ch">the character to add</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public void Write(uint ch, uint attrs, short pair)
        {
            ch |= attrs;
            ch |= (uint)NativeNCurses.COLOR_PAIR(pair);
            NativeWindow.waddch(this.WindowPtr, ch);
        }

        /// <summary>
        /// write character <paramref name="ch"/> to the window.
        /// <paramref name="ch"/> can be unicode if your terminal supports it.
        /// the cursor advances.
        /// </summary>
        /// <param name="ch">the character to add</param>
        public void Write(char ch)
        {
            if (NCurses.UnicodeSupported)
                NativeWindow.wadd_wch(this.WindowPtr, new NCursesWCHAR(ch));
            else
                NativeWindow.waddch(this.WindowPtr, (byte)ch);
        }

        /// <summary>
        /// write character <paramref name="ch"/> to the window with defined attributes/color pair.
        /// <paramref name="ch"/> can be unicode if your terminal supports it.
        /// the cursor advances
        /// </summary>
        /// <param name="ch">the character to add</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public void Write(char ch, uint attrs, short pair)
        {
            if (NCurses.UnicodeSupported)
            {
                NCursesWCHAR wch = new NCursesWCHAR(ch);
                wch.attr = attrs;
                wch.ext_color = pair;
                NativeWindow.wadd_wch(this.WindowPtr, wch);
            }
            else
            {
                uint c = ch;
                c |= attrs;
                //TODO: replace with managed variant
                c |= (uint)NativeNCurses.COLOR_PAIR(pair);
                NativeWindow.waddch(this.WindowPtr, c);
            }
        }

        /// <summary>
        /// write the character/attributes <paramref name="ch"/> to line <paramref name="nline"/> and column <paramref name="ncol"/>.
        /// see <see cref="Write(uint)"/>
        /// </summary>
        /// <param name="nline">the line number to add the char to</param>
        /// <param name="ncol">the column number to add the char to</param>
        /// <param name="ch">the character/attributes to add</param>
        public void Write(int nline, int ncol, uint ch)
        {
            //if (NCurses.UnicodeSupported)
            //{
            //    uint c = ch & Attrs.CHARTEXT;
            //    uint a = ch & Attrs.ATTRIBUTES;
            //    NCURSES_CH_T wch = new NCURSES_CH_T(c);
            //    wch.attr = a;
            //    NativeWindow.mvwadd_wch(this.WindowPtr, nline, ncol, wch);
            //}
            //else
                NativeWindow.mvwaddch(this.WindowPtr, nline, ncol, ch);
        }

        /// <summary>
        /// write the character <paramref name="ch"/> to line <paramref name="nline"/> and column <paramref name="ncol"/>.
        /// see <see cref="Write(char)"/>
        /// </summary>
        /// <param name="nline">the line number to add the char to</param>
        /// <param name="ncol">the column number to add the char to</param>
        /// <param name="ch">the character to add</param>
        public void Write(int nline, int ncol, char ch)
        {
            if (NCurses.UnicodeSupported)
                NativeWindow.mvwadd_wch(this.WindowPtr, nline, ncol, new NCursesWCHAR(ch));
            else
                NativeWindow.mvwaddch(this.WindowPtr, nline, ncol, (byte)ch);

        }

        /// <summary>
        /// write the array of characters.
        /// unicode not supported.
        /// the cursor doesn't advance, only writes until the end of the line.
        /// </summary>
        /// <param name="chars">the characters you wnat to add</param>
        public void Write(uint[] chars)
        {
            NativeWindow.waddchstr(this.WindowPtr, chars);
        }

        /// <summary>
        /// write the array of characters OR'd with attributes/coror attribute.
        /// unicode not supported.
        /// the cursor doesn't advance, only writes until the end of the line.
        /// </summary>
        /// <param name="chars">the characters you wnat to add</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public void Write(uint[] chars, uint attrs, short pair)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] |= attrs;
                //TODO: replace with managed variant
                chars[i] |= (uint)NativeNCurses.COLOR_PAIR(pair);
            }
            NativeWindow.waddchstr(this.WindowPtr, chars);
        }

        /// <summary>
        /// write the array of (unicode) characters.
        /// the cursor doesn't advance, only writes until the end of the line.
        /// </summary>
        /// <param name="chars">the characters you wnat to add</param>
        public void Write(char[] chars)
        {
            if (NCurses.UnicodeSupported)
            {
                NCursesWCHAR[] chArray = new NCursesWCHAR[chars.Length];
                for (int i = 0; i < chars.Length; i++)
                    chArray[i] = new NCursesWCHAR(chars[i]);
                NativeWindow.wadd_wchstr(this.WindowPtr, chArray);
            }
            else
            {
                //TODO: correct unicode char conversion to ASCII
                uint[] chArray = new uint[chars.Length];
                for (int i = 0; i < chars.Length; i++)
                    chArray[i] = chars[i];
                NativeWindow.waddchstr(this.WindowPtr, chArray);
            }
        }

        /// <summary>
        /// write the array of (unicode) characters with defined attributes/color pair
        /// the cursor doesn't advance, only writes until the end of the line.
        /// </summary>
        /// <param name="chars">the characters you wnat to add</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public void Write(char[] chars, uint attrs, short pair)
        {
            NCursesWCHAR[] chArray = new NCursesWCHAR[chars.Length];
            for (int i = 0; i < chars.Length; i++)
            {
                chArray[i] = new NCursesWCHAR(chars[i]);
                chArray[i].attr = attrs;
                chArray[i].ext_color = pair;
            }
            NativeWindow.wadd_wchstr(this.WindowPtr, chArray);
        }
        #endregion

        #region WriteLine
        /// <summary>
        /// write a new line to the window. see <see cref="Write(int, int, string)"/>
        /// </summary>
        /// <param name="str">the string to write to the window</param>
        public void WriteLine(string str)
        {
            this.Write(string.Format("{0}\n", str));
        }

        /// <summary>
        /// write a formatted string to the window. see <see cref="WriteLine(string)"/>
        /// </summary>
        /// <param name="format">the string to format</param>
        /// <param name="arg">arguments to format with</param>
        public void WriteLine(string format, params object[] arg)
        {
            this.WriteLine(string.Format(format, arg));
        }
        #endregion

        #region read input
        /// <summary>
        /// read a character from console input.
        /// Also refreshes the window if it hasn't been refreshed yet.
        /// </summary>
        /// <returns>the read character</returns>
        public int ReadKey()
        {
            if (NCurses.UnicodeSupported)
            {
                char ret;
                NativeWindow.wget_wch(this.WindowPtr, out ret);
                return ret;
            }
            else
                return NativeWindow.wgetch(this.WindowPtr);
        }

        /// <summary>
        /// read a string of atmost 1023 characters from the console input or until return
        /// Also refreshes the window if it hasn't been refreshed yet.
        /// </summary>
        /// <returns>the read string</returns>
        public string ReadLine()
        {
            StringBuilder builder = new StringBuilder(1024);
            if (NCurses.UnicodeSupported)
                NativeWindow.wget_wstr(this.WindowPtr, builder);
            else
                NativeWindow.wgetstr(this.WindowPtr, builder);
            return builder.ToString();
        }

        /// <summary>
        /// read a string of a particular length from the console input or until return
        /// Also refreshes the window if it hasn't been refreshed yet.
        /// </summary>
        /// <param name="length">count of characters to read</param>
        /// <returns>the read string</returns>
        public string ReadLine(int length)
        {
            StringBuilder builder = new StringBuilder(length + 1);
            if (NCurses.UnicodeSupported)
                NativeWindow.wgetn_wstr(this.WindowPtr, builder, length);
            else
                NativeWindow.wgetnstr(this.WindowPtr, builder, length);
            return builder.ToString();
        }
        #endregion


        #region insert
        /// <summary>
        /// insert a character on the current cursor position. all characaters on the right move 1 column. character might fall off at the end of the line.
        /// supports unicode.
        /// cursor doesn't move.
        /// </summary>
        /// <param name="ch">the character to insert</param>
        public void Insert(char ch)
        {
            if (NCurses.UnicodeSupported)
            {
                NCursesWCHAR wch = new NCursesWCHAR(ch);
                NativeWindow.wins_wch(this.WindowPtr, wch);
            }
            else
                NativeWindow.winsch(this.WindowPtr, ch);
        }

        /// <summary>
        /// insert a character with attributes/color on the current cursor position. all characaters on the right move 1 column. character might fall off at the end of the line.
        /// supports unicode.
        /// cursor doesn't move.
        /// </summary>
        /// <param name="ch">the character to insert</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public void Insert(char ch, uint attrs, short pair)
        {
            if (NCurses.UnicodeSupported)
            {
                NCursesWCHAR wch = new NCursesWCHAR(ch);
                wch.attr = attrs;
                wch.ext_color = pair;
                NativeWindow.wins_wch(this.WindowPtr, wch);
            }
            else
            {
                uint c = ch;
                c |= attrs;
                c |= (uint)NativeNCurses.COLOR_PAIR(pair);
                NativeWindow.winsch(this.WindowPtr, c);
            }
        }

        /// <summary>
        /// insert a string on the current cursor position. all characaters on the right move the lenght of the string.
        /// character might fall off at the end of the line.
        /// supports unicode
        /// cursor doesn't move
        /// </summary>
        /// <param name="str">the string to insert</param>
        public void Insert(string str)
        {
            if (NCurses.UnicodeSupported)
                NativeWindow.wins_wstr(this.WindowPtr, str);
            else
                NativeWindow.winsstr(this.WindowPtr, str);
        }

        /// <summary>
        /// insert a max of <paramref name="count"/> character of the string on the current cursor position.
        /// all characaters shift the length of the string. character might fall off at the end of the line.
        /// supports unicode
        /// cursor doesn't move
        /// </summary>
        /// <param name="str">the string to insert</param>
        /// <param name="count">the amount of characters to insert</param>
        public void Insert(string str, int count)
        {
            if (NCurses.UnicodeSupported)
                NativeWindow.wins_nwstr(this.WindowPtr, str, count);
            else
                NativeWindow.winsnstr(this.WindowPtr, str, count);
        }
        #endregion

        #region read output
        /// <summary>
        /// read a character from the console output at the current position
        /// supports unicode.
        /// doesn't move the cursor
        /// </summary>
        /// <returns>the read character</returns>
        public char ReadChar()
        {
            char ch;
            if (NCurses.UnicodeSupported)
            {
                NCursesWCHAR wch;
                NativeWindow.win_wch(this.WindowPtr, out wch);
                return wch.GetChar();
            }
            else
            {
                uint c = NativeWindow.winch(this.WindowPtr);
                ch = (char)(c & Attrs.CHARTEXT);
            }

            return ch;
        }

        /// <summary>
        /// read a character with all its attributes from the console output at the current position
        /// supports unicode.
        /// doesn't move the cursor
        /// </summary>
        /// <param name="attrs">attributes applied to the character</param>
        /// <param name="pair">pair number applied to the character</param>
        /// <returns>the read character</returns>
        public char ReadChar(out uint attrs, out short pair)
        {
            char ch;
            if (NCurses.UnicodeSupported)
            {
                NCursesWCHAR wch;
                NativeWindow.win_wch(this.WindowPtr, out wch);
                ch = wch.GetChar();
                attrs = wch.attr;
                pair = (short)wch.ext_color;
            }
            else
            {
                uint c = NativeWindow.winch(this.WindowPtr);
                ch = (char)(c & Attrs.CHARTEXT);
                attrs = c & Attrs.ATTRIBUTES;
                pair = (short)Constants.PAIR_NUMBER(c & Attrs.COLOR);
            }

            return ch;
        }

        /// <summary>
        /// read a string of atmost 1024 characters or until the right margin from the console output.
        /// supports unicode.
        /// doesn't move the cursor
        /// </summary>
        /// <returns>the read string</returns>
        public string ReadString()
        {
            StringBuilder builder = new StringBuilder(1024);
            if (NCurses.UnicodeSupported)
                NativeWindow.winwstr(this.WindowPtr, builder);
            else
                NativeWindow.winstr(this.WindowPtr, builder);
            return builder.ToString();
        }

        /// <summary>
        /// read a string of atmost <paramref name="count"/> characters or until the right margin from the console output.
        /// supports unicode.
        /// doesn't move the cursor
        /// </summary>
        /// <param name="count">the number of characters to read</param>
        /// <returns>the read string</returns>
        public string ReadString(int count)
        {
            StringBuilder builder = new StringBuilder(count);
            if (NCurses.UnicodeSupported)
                NativeWindow.winnwstr(this.WindowPtr, builder, count);
            else
                NativeWindow.winnstr(this.WindowPtr, builder, count);
            return builder.ToString();
        }

        /// <summary>
        ///  read a string with all of its attributes of atmost 1024 characters or until the right margin from the console output.
        ///  attribute indexes relate to the character index in the string.
        /// </summary>
        /// <param name="lstAttributes">a list to save the attributes in</param>
        /// <returns>the read string</returns>
        public string ReadString(IList<Tuple<uint, short>> lstAttributes)
        {
            if (lstAttributes == null)
                throw new ArgumentNullException("No list passed to save the attributes");

            StringBuilder builder = new StringBuilder(1024);
            if (NCurses.UnicodeSupported)
            {
                NCursesWCHAR[] wchArr = new NCursesWCHAR[1024];
                NativeWindow.win_wchstr(this.WindowPtr, ref wchArr);
                for (int i = 0; i < wchArr.Length; i++)
                {
                    builder.Append(wchArr[i].GetChar());
                    lstAttributes.Add(Tuple.Create(wchArr[i].attr, (short)wchArr[i].ext_color));
                }
            }
            else
            {
                uint[] chArr = new uint[1024];
                NativeWindow.winchstr(this.WindowPtr, ref chArr);
                for (int i = 0; i < chArr.Length; i++)
                {
                    builder.Append((char)(chArr[i] & Attrs.CHARTEXT));
                    lstAttributes.Add(Tuple.Create(chArr[i] & Attrs.ATTRIBUTES, (short)Constants.PAIR_NUMBER(chArr[i] & Attrs.COLOR)));
                }
            }
            return builder.ToString();
        }

        //TODO: return NCURSES_CH_T or uint[] ????
        /// <summary>
        ///  read a string with all of its attributes of atmost <paramref name="count"/> characters or until the right margin from the console output.
        ///  attribute indexes refer to the character index in the string.
        /// </summary>
        /// <param name="count">the number of characters to read</param>
        /// <param name="lstAttributes">a list to save the attributes in</param>
        /// <returns>the read string</returns>
        public string ReadString(int count, IList<Tuple<uint, short>> lstAttributes)
        {
            if (lstAttributes == null)
                throw new ArgumentNullException("No list passed to save the attributes");

            StringBuilder builder = new StringBuilder(count);
            if (NCurses.UnicodeSupported)
            {
                NCursesWCHAR[] wchArr = new NCursesWCHAR[count];
                NativeWindow.win_wchnstr(this.WindowPtr, ref wchArr, count);
                for (int i = 0; i < wchArr.Length; i++)
                {
                    builder.Append(wchArr[i].GetChar());
                    lstAttributes.Add(Tuple.Create(wchArr[i].attr, (short)wchArr[i].ext_color));
                }
            }
            else
            {
                uint[] chArr = new uint[count];
                NativeWindow.winchnstr(this.WindowPtr, ref chArr, count);
                for (int i = 0; i < chArr.Length; i++)
                {
                    builder.Append((char)(chArr[i] & Attrs.CHARTEXT));
                    lstAttributes.Add(Tuple.Create(chArr[i] & Attrs.ATTRIBUTES, (short)Constants.PAIR_NUMBER(chArr[i] & Attrs.COLOR)));
                }
            }
            return builder.ToString();
        }
        #endregion

        #region border
        /// <summary>
        /// draw the default borders for the current window.
        /// non-unicode supported
        /// </summary>
        public void Box()
        {
            NativeWindow.box(this.WindowPtr, 0, 0);
        }
        #endregion

        /// <summary>
        /// Clear the window
        /// </summary>
        public void Erase()
        {
            NativeWindow.werase(this.WindowPtr);
        }

        /// <summary>
        /// Clear the window without a call to refresh
        /// </summary>
        public void Clear()
        {
            NativeWindow.wclear(this.WindowPtr);
        }

        /// <summary>
        /// Scroll the window <paramref name="lines"/> lines.
        /// </summary>
        /// <param name="lines">the number of lines to scroll, negative to scroll down.</param>
        public void ScrollWindow(int lines)
        {
            NativeWindow.wscrl(this.WindowPtr, lines);
        }

        /// <summary>
        /// Clear to the end of the line
        /// </summary>
        public void ClearToEol()
        {
            NativeWindow.wclrtoeol(this.WindowPtr);
        }

        /// <summary>
        /// Clear window to bottom starting from current cursor position
        /// </summary>
        public void ClearToBottom()
        {
            NativeWindow.wclrtobot(this.WindowPtr);
        }

        public abstract void Refresh();
        public abstract void NoOutRefresh();
        public abstract WindowBase SubWindow();

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (this.WindowPtr != IntPtr.Zero)
                DictPtrWindows.Remove(this.WindowPtr);
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion
    }
}
