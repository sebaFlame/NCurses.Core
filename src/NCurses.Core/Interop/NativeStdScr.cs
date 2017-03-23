using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop
{
    /// <summary>
    /// native stdscr methods (which is a window).
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    internal static class NativeStdScr
    {
        #region addch
        /// <summary>
        /// The addch, waddch, mvaddch and mvwaddch routines put the
        /// character ch into the given window at its current  window
        /// position,  which  is then advanced.They are analogous to
        /// putchar in stdio(3).
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ch">The character you want to add</param>
        public static void addch(uint ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.addch(ch), "addch");
        }
        #endregion

        #region addchnstr
        /// <summary>
        /// These functions copy the(null-terminated) chstr array into the window image structure starting
        /// at the current cursor position.The four functions with n as the last argument copy at most n elements, but no more than will fit on
        /// the  line.If n = -1 then the whole array is copied, to the
        /// maximum number of characters that will fit on the line.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="txt">the string you want to add</param>
        /// <param name="number">number of elements to copy</param>
        public static void addchnstr(uint[] txt, int number)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.addchnstr(arrayPtr, number), "addchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region addchstr
        /// <summary>
        /// see <see cref="addchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="txt">the string you want to add</param>
        public static void addchstr(uint[] txt)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.addchstr(arrayPtr), "addchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region addnstr
        /// <summary>
        /// These functions  write the(null-terminated)  character
        /// string str on the given window.It is similar to  calling
        /// waddch once for each character in the string.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="txt">string to add</param>
        /// <param name="number">number of elements to copy</param>
        public static void addnstr(string txt, int number)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.addnstr(txt, number), "addnstr");
        }
        #endregion

        #region addstr
        /// <summary>
        /// see <see cref="addnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="txt">string to add</param>
        public static void addstr(string txt)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.addstr(txt), "addstr");
        }
        #endregion

        #region attroff
        
        /// <summary>
        ///  The remaining  attr* functions  operate exactly like the
        /// corresponding attr_* functions, except that they take  arguments of type int rather than attr_t.
        /// see <see cref="attr_off"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attribute(s) to disable</param>
        public static void attroff(int attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.attroff(attrs), "attroff");
        }
        #endregion

        #region attron
        
        /// <summary>
        /// see <see cref="attroff"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attribute(s) to enable</param>
        public static void attron(int attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.attron(attrs), "attron");
        }
        #endregion

        #region attrset
        /// <summary>
        /// The attr_set and wattr_set functions set the current  attributes of
        /// the given window to attrs, with color specified by pair.X/Open specified  an additional  parameter
        /// opts which is unused in all implementations.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attribute(s) to enable</param>
        public static void attrset(int attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.attrset(attrs), "attrset");
        }
        #endregion

        #region attr_on
        /// <summary>
        /// Use attr_on and wattr_on to turn  on window  attributes,
        /// i.e., values OR'd together in attr, without affecting other attributes.Use attr_off and  wattr_off to  turn off
        /// window attributes, again values  OR'd together in attr,
        /// without affecting other attributes.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attribute(s) to enable</param>
        public static void attr_on(uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.attr_on(attrs, IntPtr.Zero), "attr_on");
        }
        #endregion

        #region attr_off
        /// <summary>
        /// see <see cref="attr_on"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attribute(s) to disable</param>
        public static void attr_off(uint attrs)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.attr_off(attrs, IntPtr.Zero), "attr_off");
        }
        #endregion

        #region attr_set
        /// <summary>
        /// The attr_set and wattr_set functions set the current  attributes of  the given window to <paramref name="attrs"/>,
        /// with color specified by <paramref name="pair"/>.X/Open specified  an additional  parameter
        /// opts which is unused in all implementations.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attribute(s) to enable</param>
        /// <param name="pair">color pair to enable</param>
        public static void attr_set(uint attrs, short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.attr_set(attrs, pair, IntPtr.Zero), "attr_set");
        }
        #endregion

        #region attr_get
        /// <summary>
        /// see <see cref="attr_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">Pointer to an uint to retrieve current attributes</param>
        /// <param name="pair">Pointer to a short to retrieve current color pair</param>
        public static void attr_get(ref uint attrs, ref short pair)
        {
            IntPtr aPtr = Marshal.AllocHGlobal(Marshal.SizeOf(attrs));
            Marshal.StructureToPtr(attrs, aPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(attrs));

            IntPtr pPtr = Marshal.AllocHGlobal(Marshal.SizeOf(pair));
            Marshal.StructureToPtr(pair, pPtr, true);
            GC.AddMemoryPressure(Marshal.SizeOf(pair));

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.attr_get(aPtr, pPtr, IntPtr.Zero), "attr_get");
            }
            finally
            {
                Marshal.FreeHGlobal(aPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(attrs));

                Marshal.FreeHGlobal(pPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(pair));
            }
        }
        #endregion

        #region bkgd
        /// <summary>
        /// The bkgd and wbkgd functions set the background  property
        /// of  the current  or specified window and then apply this
        /// setting to every character position in that window:
        /// <para>The rendition of every character  on the  screen  is changed to the new background rendition.</para>
        /// <para>Wherever the  former background character appears, it is changed to the new background character.</para>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="bkgd">a character to change the background to</param>
        public static void bkgd(uint bkgd)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.bkgd(bkgd), "bkgd");
        }
        #endregion

        #region bkgdset
        /// <summary>
        /// The bkgdset and wbkgdset  routines manipulate  the back-
        /// ground of  the named window.The window background is a
        /// chtype consisting of any combination of attributes(i.e.,
        /// rendition)  and a  character.The attribute part of the
        /// background is combined(OR'ed) with all non-blank  characters that  are written into the window with waddch.Both
        /// the character and attribute parts of the  background are
        /// combined with  the blank  characters.The background
        /// becomes a property of the character  and moves  with the
        /// character through   any scrolling   and insert/delete
        /// line/character operations.
        /// </summary>
        /// <param name="bkgd">a character to change the background to</param>
        public static void bkgdset(uint bkgd)
        {
            NativeNCurses.NCursesWrapper.bkgdset(bkgd);
        }
        #endregion

        #region border
        /// <summary>
        /// draw a box around the edges of a window. If  any of these arguments is zero, then the corresponding ACS_* values are used
        /// native method wrapped with verification.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ls">left side</param>
        /// <param name="rs">right side</param>
        /// <param name="ts">top side</param>
        /// <param name="bs">bottom side</param>
        /// <param name="tl">top left-hand corner</param>
        /// <param name="tr">top right-hand corner</param>
        /// <param name="bl">bottom left-hand corner</param>
        /// <param name="br">bottom right-hand corner</param>
        public static void border(uint ls, uint rs, uint ts, uint bs, uint tl, uint tr, uint bl, uint br)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.border(ls, rs, ts, bs, tl, tr, bl, br), "border");
        }
        #endregion

        #region chgat
        /// <summary>
        /// The routine chgat changes the attributes of a given number
        /// of characters starting at the current cursor location  of
        /// stdscr.It  does not update the cursor and does not perform wrapping.  A character count of -1  or greater  than
        /// the  remaining window width means to change attributes all
        /// the way to the end of the current line.The wchgat  function generalizes this to any window; the mvwchgat function
        /// does a cursor move before acting.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="number">number of characters to apply the new attrs to</param>
        /// <param name="attrs">attribute(s) to enable</param>
        /// <param name="pair">color pair to enable</param>
        public static void chgat(int number, uint attrs, short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.chgat(number, attrs, pair, IntPtr.Zero), "chgat");
        }
        #endregion

        #region clear
        /// <summary>
        /// The  clear  and wclear routines are like erase and werase,
        /// but they also call clearok, so that the screen is  cleared
        /// completely  on the  next call to wrefresh for that window
        /// and repainted from scratch.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void clear()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.clear(), "clear");
        }
        #endregion

        #region clrtobot
        /// <summary>
        /// The clrtobot and wclrtobot routines erase from the  cursor
        /// to the end of screen.That is, they erase all lines below
        /// the cursor in the window.  Also, the current line to  the
        /// right of the cursor, inclusive, is erased.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void clrtobot()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.clrtobot(), "clrtobot");
        }
        #endregion

        #region clrtoeol
        /// <summary>
        /// The clrtoeol and wclrtoeol routines erase the current line
        /// to the right of the cursor, inclusive, to the end of  the current line.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void clrtoeol()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.clrtoeol(), "clrtoeol");
        }
        #endregion

        #region color_set
        /// <summary>
        /// The routine color_set sets the current color of the given
        /// window to the foreground/background combination  described
        /// by  the color  pair parameter.The parameter opts is reserved for future use; applications must  supply a  null pointer.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="pair">A color pair index</param>
        public static void color_set(short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.color_set(pair, IntPtr.Zero), "color_set");
        }
        #endregion

        #region delch
        /// <summary>
        /// These routines delete the character under the cursor;  all
        /// characters to the right of the cursor on the same line are
        /// moved to the left one position and the last character  on
        /// the line is filled with a blank.The cursor position does
        /// not change (after moving to y, x,  if  specified).   (This
        /// does  not imply use of the hardware delete character feature.)
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void delch()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.delch(), "delch");
        }
        #endregion

        #region deleteln
        /// <summary>
        /// The deleteln and wdeleteln routines delete the line  under
        /// the cursor in the window; all lines below the current line
        /// are moved up one line.The bottom line of the window  is
        /// cleared.The cursor position does not change.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void deleteln()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.deleteln(), "deleteln");
        }
        #endregion

        #region echochar
        /// <summary>
        /// The echochar  and wechochar routines are equivalent to a
        /// call to addch followed by a call to refresh(3x), or a call
        /// to  waddch followed by a call to wrefresh.The knowledge
        /// that only a single character is being output is used and,
        /// for  non-control characters, a  considerable performance
        /// gain may be seen by using these routines instead of their
        /// equivalents.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ch">the character you want to add</param>
        public static void echochar(uint ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.echochar(ch), "echochar");
        }
        #endregion

        #region erase
        /// <summary>
        /// The erase and werase routines copy blanks to  every position in the window, clearing the screen.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void erase()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.erase(), "erase");
        }
        #endregion

        #region getch
        /// <summary>
        /// The getch, wgetch, mvgetch and mvwgetch, routines  read a
        /// character from the window.In no-delay mode, if no input
        /// is waiting, the value ERR is returned.In delay mode, the
        /// program  waits until the system passes text through to the
        /// program.  Depending on the setting of cbreak, this is  after one  character (cbreak mode),
        /// or after the first newline(nocbreak mode).  In half-delay mode, the  program
        /// waits  until a character is typed or the specified timeout has been reached.
        /// refreshes the window when the window has been moved/modified.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static int getch()
        {
            return NativeNCurses.NCursesWrapper.getch();
        }
        #endregion

        #region getnstr
        /// <summary>
        /// wgetnstr reads  at most  n characters, thus preventing a
        /// possible overflow of the input buffer.Any attempt to enter more characters (other than the terminating newline or
        /// carriage return) causes a beep.Function keys also cause
        /// a beep  and are ignored.The getnstr function reads from
        /// the stdscr default window.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="builder">string reference to read the char(s) into</param>
        /// <param name="count">max number or characters to read</param>
        public static void getnstr(StringBuilder builder, int count)
        {
            NativeNCurses.NCursesWrapper.getnstr(builder, count);
        }
        #endregion

        #region getstr
        /// <summary>
        /// <summary>
        /// The function getstr is equivalent to a series of calls to
        /// getch, until a newline or carriage return is received(the
        /// terminating  character  is  not included  in the returned
        /// string).  The resulting value is placed in the area pointed to by the character pointer str.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="builder">string reference to read the char(s) into</param>
        public static void getstr(StringBuilder builder)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.getstr(builder), "getstr");
        }
        #endregion

        #region hline
        /// <summary>
        /// The hline and whline functions draw a horizontal(left to
        /// right)  line using <paramref name="ch"/> starting at the current cursor position in the window.The current cursor  position  is  not
        /// changed.   The line  is  at most <paramref name="count"/> characters long, or as many as fit into the window.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ch">the character to use as a horizontal line</param>
        /// <param name="count">maximum length of the line</param>
        public static void hline(uint ch, int count)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.hline(ch, count), "hline");
        }
        #endregion

        #region inch
        /// <summary>
        /// These routines return the character, of  type chtype, at
        /// the current  position  in  the named  window.If any
        /// attributes are set for that position, their  values are
        /// OR'ed  into  the  value  returned.   Constants  defined in
        /// <curses.h> can be used with the & (logical AND)  operator
        /// to extract the character or attributes alone.
        /// </summary>
        /// <returns>characther with attributes at current position</returns>
        public static uint inch()
        {
            return NativeNCurses.NCursesWrapper.inch();
        }
        #endregion

        #region inchnstr
        /// <summary>
        /// These routines return a NULL-terminated array  of chtype
        /// quantities, starting at the current cursor position in the
        /// named window and ending at the right margin of the window.
        /// The  four functions with n as the last argument, return a
        /// leading substring at most n characters long (exclusive of
        /// the trailing (chtype)0).  Constants defined in <curses.h>
        /// can be used with the & (logical AND) operator  to extract
        /// the character or the attribute alone from any position in
        /// the chstr[see curs_inch(3x)].
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="txt">the array to copy the chars into</param>
        /// <param name="count">number of chars/attributes to copy</param>
        public static void inchnstr(ref uint[] txt, int count)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.inchnstr(arrayPtr, count), "inchnstr");
                for (int i = 0; i < txt.Length; i++)
                    txt[i] = (uint)Marshal.ReadInt32(arrayPtr + (i * sizeof(uint)));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region inchstr
        /// <summary>
        /// see <see cref="inchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="txt">the array to copy the chars into</param>
        public static void inchstr(ref uint[] txt)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(txt, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.inchstr(arrayPtr), "inchstr");
                for (int i = 0; i < txt.Length; i++)
                    txt[i] = (uint)Marshal.ReadInt32(arrayPtr + (i * sizeof(uint)));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region innstr
        /// <summary>
        /// These routines return  a string of  characters  in  <paramref name="str"/>,
        /// extracted starting  at the current cursor position in the
        /// named window.Attributes are stripped from  the characters.The four  functions with  n as the last argument
        /// return a leading  substring at  most <paramref name="n"/>  characters long
        /// (exclusive of the trailing NUL).
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">A reference to the string you want to extract</param>
        /// <param name="n">max length of the leading substring</param>
        public static void innstr(StringBuilder str, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.innstr(str, n), "innstr");
        }
        #endregion

        #region insch
        /// <summary>
        /// These routines return the character, of  type chtype, at
        /// the current  position  in  the named  window.If any
        /// attributes are set for that position, their  values are
        /// OR'ed  into  the  value  returned.   Constants  defined in
        /// <curses.h> can be used with the & (logical AND)  operator
        /// to extract the character or attributes alone.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ch">The character to insert</param>
        public static void insch(uint ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.insch(ch), "insch");
        }
        #endregion

        #region insdelln
        /// <summary>
        /// for  positive <paramref name="n"/>,
        /// insert n lines into the specified window above the current
        /// line.The  <paramref name="n"/> bottom  lines are  lost.For negative <paramref name="n"/>,
        /// delete n lines (starting with the one under the  cursor),
        /// and move  the remaining lines up.The bottom n lines are
        /// cleared.The current cursor position remains the same.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="n">The number of lines to insert</param>
        public static void insdelln(int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.insdelln(n), "insdelln");
        }
        #endregion

        #region insertln
        /// <summary>
        /// insert  a  blank  line above the current line and the bottom line is lost.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void insertln()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.insertln(), "insertln");
        }
        #endregion

        #region insnstr
        /// <summary>
        /// These routines insert a character string (as many characters  as  will fit on the line) before the character under
        /// the cursor.All characters to the right of the cursor are
        /// shifted right with the possibility of the rightmost characters on the line being lost.  The cursor  position does
        /// not change  (after moving  to y, x, if specified).  The
        /// functions with n as the last  argument insert  a leading
        /// substring of  at most  n characters.If n&lt;=0, then the entire string is inserted.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">The string to insert</param>
        /// <param name="n">The nubmer of characters to insert</param>
        public static void insnstr(string str, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.insnstr(str, n), "insnstr");
        }
        #endregion

        #region insstr
        /// <summary>
        /// see <see cref="insnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">The string to insert</param>
        public static void insstr(string str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.insstr(str), "insstr");
        }
        #endregion

        #region instr
        /// <summary>
        /// see <see cref="innstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">Reference to the string you want extracted</param>
        public static void instr(StringBuilder str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.instr(str), "instr");
        }
        #endregion

        #region move
        /// <summary>
        /// These routines move the cursor associated with the window
        /// to line y and column x.This routine does not  move the
        /// physical cursor  of the  terminal until  refresh(3x) is
        /// called.The position specified is relative to  the upper
        /// left-hand corner of the window, which is (0,0).
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void move(int y, int x)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.move(y, x), "move");
        }
        #endregion

        #region mvaddch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addch(uint)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// <param name="ch">the character to add</param>
        public static void mvaddch(int y, int x, uint ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvaddch(y, x, ch), "mvaddch");
        }
        #endregion

        #region mvaddchnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addchnstr(IntPtr, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// <param name="chstr">pointer to a null terminated array of uint</param>
        /// <param name="n">number of elements to copy</param>
        public static void mvaddchnstr(int y, int x, uint[] chstr, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(chstr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvaddchnstr(y, x, arrayPtr, n), "mvaddchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region mvaddchstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addchstr(IntPtr)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// <param name="chstr">pointer to a null terminated array of uint</param>
        public static void mvaddchstr(int y, int x, uint[] chstr)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(chstr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvaddchstr(y, x, arrayPtr), "mvaddchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region mvaddnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addnstr(string, int)"/>
        /// <para />native method wrapped with verification.
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// </summary>
        public static void mvaddnstr(int y, int x, string txt, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvaddnstr(y, x, txt, n), "mvaddnstr");
        }
        #endregion

        #region mvaddstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addstr(string)"/>
        /// <para />native method wrapped with verification.
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// </summary>
        public static void mvaddstr(int y, int x, string txt)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvaddstr(y, x, txt), "mvaddstr");
        }
        #endregion

        #region mvchgat
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="chgat(int, uint, short)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvchgat(int y, int x, int number, uint attrs, short pair)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvchgat(y, x, number, attrs, pair), "mvchgat");
        }
        #endregion

        #region mvdelch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="delch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvdelch(int y, int x)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvdelch(y, x), "mvdelch");
        }
        #endregion

        #region mvgetch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="getch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvgetch(int y, int x)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvgetch(y, x), "mvgetch");
        }
        #endregion

        #region mvgetnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="getnstr(StringBuilder, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvgetnstr(int y, int x, StringBuilder str, int count)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvgetnstr(y, x, str, count), "mvgetnstr");
        }
        #endregion

        #region mvgetstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="getstr(StringBuilder)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvgetstr(int y, int x, StringBuilder str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvgetstr(y, x, str), "mvgetstr");
        }
        #endregion

        #region mvhline
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="hline(uint, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvhline(int y, int x, uint ch, int count)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvhline(y, x, ch, count), "mvhline");
        }
        #endregion

        #region mvinch
        /// <summary>
        /// see <see cref="inch()"/>
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        public static uint mvinch(int y, int x)
        {
            return NativeNCurses.NCursesWrapper.mvinch(y, x);
        }
        #endregion

        #region mvinchnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="inchnstr(IntPtr, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinchnstr(int y, int x, IntPtr txt, int count)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvinchnstr(y, x, txt, count), "mvinchnstr");
        }
        #endregion

        #region mvinchstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="inchstr(IntPtr)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinchstr(int y, int x, IntPtr txt)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvinchstr(y, x, txt), "mvinchstr");
        }
        #endregion

        #region mvinnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="innstr(StringBuilder, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinnstr(int y, int x, StringBuilder str, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvinnstr(y, x, str, n), "mvinnstr");
        }
        #endregion

        #region mvinsch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="insch(uint)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinsch(int y, int x, uint ch)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvinsch(y, x, ch), "mvinsch");
        }
        #endregion

        #region mvinsnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="insnstr(string, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinsnstr(int y, int x, string str, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvinsnstr(y, x, str, n), "mvinsnstr");
        }
        #endregion

        #region mvinsstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="insstr(string)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinsstr(int y, int x, string str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvinsstr(y, x, str), "mvinsstr");
        }
        #endregion

        #region mvinstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="instr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinstr(int y, int x, StringBuilder str)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvinstr(y, x, str), "mvinstr");
        }
        #endregion

        #region mvprintw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="printw(string, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvprintw(int y, int x, string format, params object[] var)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvprintw(y, x, format, var), "mvprintw");
        }
        #endregion

        #region mvscanw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="scanw(StringBuilder, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvscanw(int y, int x, StringBuilder format, params object[] var)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvscanw(y, x, format, var), "mvscanw");
        }
        #endregion

        #region mvvline
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="vline(uint, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvvline(int y, int x, uint ch, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvvline(y, x, ch, n), "mvvline");
        }
        #endregion

        #region printw
        /// <summary>
        /// The printw, wprintw, mvprintw and mvwprintw routines are
        /// analogous to  printf[see  printf(3)].   In effect, the
        /// string that would be output by printf is output instead as
        /// though waddstr were used on the given window.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="var">the variables</param>
        public static void printw(string format, params object[] var)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.printw(format, var), "printw");
        }
        #endregion

        #region redrawwin
        /// <summary>
        /// The wredrawln routine indicates to curses that some screen
        /// lines are corrupted and should be thrown away before  anything  is  written over  them.It touches the indicated
        /// lines(marking them  changed).   The routine  redrawwin
        /// touches the entire window.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="window">A pointer to a window</param>
        public static void redrawwin()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.redrawwin(), "redrawwin");
        }
        #endregion

        #region refresh
        /// <summary>
        /// The refresh and wrefresh  routines(or wnoutrefresh  and
        /// doupdate)  must be called to get actual output to the terminal,
        /// as other routines  merely manipulate  data structures.The routine  wrefresh copies the named window to
        /// the physical terminal screen, taking into account what  is
        /// already there to do optimizations.The refresh routine is
        /// the same, using stdscr  as  the  default  window.Unless
        /// leaveok  has been enabled, the physical cursor of the terminal is left at the location of the cursor for that window.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void refresh()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.refresh(), "refresh");
        }
        #endregion

        #region scanw
        /// <summary>
        /// The scanw, wscanw and mvscanw routines are  analogous to
        /// scanf[see scanf(3)].  The effect of these routines is as
        /// though wgetstr were called on the window, and the  result-
        /// ing line used as input for sscanf(3).  Fields which do not
        /// map to a variable in the fmt field are lost.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="format">the format string</param>
        /// <param name="var">the variables</param>
        public static void scanw(StringBuilder format, params object[] var)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.scanw(format, var), "scanw");
        }
        #endregion

        #region scrl
        /// <summary>
        /// For positive  <paramref name="n"/>,  the scrl and wscrl routines scroll the
        /// window up n lines(line i+n becomes i); otherwise scroll
        /// the window  down n lines.This involves moving the lines
        /// in the window character image structure.The current cursor position is not changed.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="n">the number of lines to scroll</param>
        public static void scrl(int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.scrl(n), "scrl");
        }
        #endregion

        #region setscrreg
        /// <summary>
        /// The setscrreg and wsetscrreg routines allow  the applica-
        /// tion programmer  to set a software scrolling region in a
        /// window.The top and bot parameters are the  line numbers
        /// of the  top and  bottom margin of the scrolling region.
        /// (Line 0 is the top line of the window.)   If this  option
        /// and  scrollok are enabled, an attempt to move off the bottom margin line causes all lines in the scrolling  region
        /// to  scroll one  line  in the direction of the first line.
        /// Only the text of the window is scrolled.  (Note that  this
        /// has nothing to do with the use of a physical scrolling region capability in the terminal, like that in  the VT100.
        /// If idlok  is  enabled and  the terminal  has either  a
        /// scrolling region or insert/delete line  capability, they
        /// will probably be used by the output routines.)
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="top">top line of the scrolling region</param>
        /// <param name="bot">bottom line of the scrolling region</param>
        public static void setscrreg(int top, int bot)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.setscrreg(top, bot), "setscrreg");
        }
        #endregion

        #region standout
        /// <summary>
        /// The routine  standout  is the same as attron(<see cref="Attrs.STANDOUT"/>).
        /// The routine standend is the same as  attrset(<see cref="Attrs.NORMAL"/> )  or
        /// attrset(0), that is, it turns off all attributes.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void standout()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.standout(), "standout");
        }
        #endregion

        #region standend
        /// <summary>
        /// see <see cref="standout"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void standend()
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.standend(), "standend");
        }
        #endregion

        #region timeout
        /// <summary>
        /// The timeout and wtimeout routines set  blocking or  non-blocking read  for a given window.If delay is negative,
        /// blocking read is used(i.e., waits indefinitely  for  input).   If delay  is zero, then non-blocking read is used
        /// (i.e., read returns ERR if no input is waiting).  If delay
        /// is  positive, then read blocks for delay milliseconds, and
        /// returns ERR if there is still no input.Hence, these rou-
        /// tines provide the same functionality as nodelay, plus the
        /// additional capability of being able to block for only delay milliseconds(where delay is positive).
        /// </summary>
        public static void timeout(int delay)
        {
            NativeNCurses.NCursesWrapper.timeout(delay);
        }
        #endregion

        #region vline
        /// <summary>
        /// The hline and whline functions draw a horizontal (left  to
        /// right)  line using ch starting at the current cursor position in the window.  The current cursor  position  is  not
        /// changed.   The  line  is  at most n characters long, or as
        /// many as fit into the window.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ch">the character to use as a horizontal line</param>
        /// <param name="count">maximum length of the line</param>
        public static void vline(uint ch, int n)
        {
            NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.vline(ch, n), "vline");
        }
        #endregion

        #region add_wch
        /// <summary>
        /// The add_wch, wadd_wch, mvadd_wch, and mvwadd_wch functions
        /// put the complex character wch into the given window at its
        /// current position, which is then advanced.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">The wide character to add</param>
        public static void add_wch(NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using(wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.add_wch(wPtr), "add_wch");
            }
        }
        #endregion

        #region add_wchnstr
        /// <summary>
        /// These functions copy the(null-terminated) array of  complex characters  wchstr into  the window image structure
        /// starting at the current cursor position.The  four functions with n as the last argument copy at most n elements,
        /// but no more than will fit on the line.If n = -1  then the
        /// whole array is copied, to the maximum number of characters
        /// that will fit on the line. The window cursor is not advanced. These  functions  work
        /// faster than waddnstr.On the other hand:
        /// <para>o they do not perform checking(such as for the newline, backspace, or carriage return characters)</para>
        /// <para>o they do not advance the current cursor position</para>
        /// <para>o they do not expand other control characters to  ^-escapes</para>
        /// <para>o they truncate the string if it crosses the right margin, rather than wrapping it around to the new line.</para>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wchStr">The string of complex characters you want to add</param>
        /// <param name="n">number of elements to copy</param>
        public static void add_wchnstr(NCursesWCHAR[] wchStr, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.add_wchnstr(arrayPtr, n), "add_wchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region add_wchstr
        /// <summary>
        /// see <see cref="add_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wchStr">The string of complex characters you want to add</param>
        /// <param name="n">number of elements to copy</param>
        public static void add_wchstr(NCursesWCHAR[] wchStr)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.add_wchstr(arrayPtr), "add_wchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region addnwstr
        /// <summary>
        /// These functions  write the characters of the(null-termitated) wchar_t character string wstr on the given window.
        /// It  is  similar to constructing a cchar_t for each wchar_t
        /// in the string, then calling  wadd_wch  for  the resulting cchar_t.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wstr">the string to add</param>
        /// <param name="n">number of elements to copy</param>
        public static void addnwstr(string wstr, int n)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.addnwstr(ptr, n), "addnwstr"),
                wstr);
        }
        #endregion

        #region addwstr
        /// <summary>
        /// see <see cref="addnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wstr">the string to add</param>
        public static void addwstr(string wstr)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.addwstr(wstr), "addwstr"),
                wstr, true);
        }
        #endregion

        #region bkgrnd
        /// <summary>
        /// The bkgrnd and wbkgrnd functions set the background property of the current or specified  window and  then apply
        /// this setting to every character position in that window:
        /// <para>o The  rendition of  every character  on the screen is
        /// changed to the new background rendition.</para>
        /// <para>o Wherever the former background character appears, it is changed to the new background character.</para>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wstr">the string to add</param>
        public static void bkgrnd(NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using(wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.bkgrnd(wPtr), "bkgrnd");
            }
        }
        #endregion

        #region bkgrndset
        
        /// <summary>
        /// The bkgrndset and wbkgrndset routines manipulate the background of the named window.The window  background  is  a
        /// cchar_t consisting of any combination of attributes(i.e.,
        /// rendition) and a complex character.The attribute part of
        /// the  background  is  combined  (OR'ed)  with all non-blank
        /// characters that are written into the window with  waddch.
        /// Both the  character and attribute parts of the background
        /// are combined with the blank  characters.The background
        /// becomes a  property of  the character and moves with the
        /// character through   any scrolling   and insert/delete
        /// line/character operations.
        /// </summary>
        /// <param name="wstr">the string to add</param>
        public static void bkgrndset(NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.NCursesWrapper.bkgrndset(wPtr);
            }
        }
        #endregion

        #region border_set
        /// <summary>
        /// The border_set and wborder_set  functions draw  a border
        /// around the  edges of  the current  or specified window.
        /// These functions do not change the cursor position, and  do
        /// not wrap.
        /// </summary>
        /// <param name="ls">left side</param>
        /// <param name="rs">right side</param>
        /// <param name="ts">top side</param>
        /// <param name="bs">bottom side</param>
        /// <param name="tl">top left-hand corner</param>
        /// <param name="tr">top right-hand corner</param>
        /// <param name="bl">bottom left-hand corner</param>
        /// <param name="br">bottom right-hand corner</param>
        public static void border_set(NCursesWCHAR ls, NCursesWCHAR rs, NCursesWCHAR ts, NCursesWCHAR bs, NCursesWCHAR tl, NCursesWCHAR tr,
            NCursesWCHAR bl, NCursesWCHAR br)
        {
            IntPtr lsPtr = ls.ToPointer();
            IntPtr rsPtr = rs.ToPointer();
            IntPtr tsPtr = ts.ToPointer();
            IntPtr bsPtr = bs.ToPointer();
            IntPtr tlPtr = tl.ToPointer();
            IntPtr trPtr = tr.ToPointer();
            IntPtr blPtr = bl.ToPointer();
            IntPtr brPtr = br.ToPointer();

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.border_set(lsPtr, rsPtr, tsPtr, bsPtr, tlPtr, trPtr, blPtr, brPtr), "add_wch");
            }
            finally
            {
                Marshal.FreeHGlobal(lsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(ls));

                Marshal.FreeHGlobal(rsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(rs));

                Marshal.FreeHGlobal(tsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(ts));

                Marshal.FreeHGlobal(bsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(bs));

                Marshal.FreeHGlobal(tlPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(tl));

                Marshal.FreeHGlobal(trPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(tr));

                Marshal.FreeHGlobal(blPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(bl));

                Marshal.FreeHGlobal(lsPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf(br));
            }
        }
        #endregion

        #region echo_wchar
        /// <summary>
        /// The echo_wchar function is functionally equivalent  to a
        /// call to add_wch followed by a call to refresh(3x).  Similarly, the wecho_wchar is  functionally equivalent  to a
        /// call to  wadd_wch followed  by a call to wrefresh.The
        /// knowledge that only a single character is being output  is
        /// taken into consideration and, for non-control characters,
        /// a considerable performance gain might be seen by using the
        /// *echo* functions instead of their equivalents.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">the character to echo</param>
        public static void echo_wchar(NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.echo_wchar(wPtr), "echo_wchar");
            }
        }
        #endregion

        #region get_wch
        /// <summary>
        /// The get_wch, wget_wch, mvget_wch, and mvwget_wch functions
        /// read a character from the terminal  associated with  the
        /// current  or specified window.In no-delay mode, if no input is waiting, the value ERR is returned.In delay mode,
        /// the program waits until the system passes text through to
        /// the program.Depending on the setting of cbreak, this  is
        /// after one character (cbreak mode), or after the first new-
        /// line(nocbreak mode).  In half-delay mode, the  program
        /// waits  until the  user types a character or the specified
        /// timeout interval has elapsed.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">a reference to store the returned wide char in</param>
        public static void get_wch(out char wch)
        {
            //TODO: returns KEY_CODE_YES if a function key gets pressed
            IntPtr chPtr = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());
            GC.AddMemoryPressure(Marshal.SizeOf<uint>());

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.get_wch(chPtr), "get_wch");

                byte[] arr = new byte[Marshal.SizeOf<uint>()];
                Marshal.Copy(chPtr, arr, 0, Marshal.SizeOf<uint>());

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    wch = Encoding.Unicode.GetChars(arr)[0];
                else
                    wch = Encoding.UTF32.GetChars(arr)[0];
            }
            finally
            {
                Marshal.FreeHGlobal(chPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<uint>());
            }
        }
        #endregion

        #region get_wstr
        /// <summary>
        /// The effect  of get_wstr is as though a series of calls to
        /// get_wch(3x) were made, until a newline, other end-of-line,
        /// or end-of-file condition  is  processed.An end-of-file
        /// condition is represented by WEOF, as defined in <wchar.h>.
        /// The newline and end-of-line conditions are represented by
        /// the \n wchar_t value.In all instances, the end  of the
        /// string  is  terminated by  a  null  wchar_t.The routine
        /// places resulting values in the area pointed to by wstr.
        /// refreshes the window when the window has been moved/modified.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wstr">a reference to store the returned wide string in</param>
        public static void get_wstr(StringBuilder wstr)
        {
            int size = Marshal.SizeOf<uint>() * wstr.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.get_wstr(strPtr), "get_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, wstr.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<uint>());
            }
        }
        #endregion

        #region getbkgrnd
        /// <summary>
        /// The getbkgrnd function returns the given window's current
        /// background character/attribute pair via the wch pointer.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">a reference to store the returned background char in</param>
        public static void getbkgrnd(out NCursesWCHAR wch)
        {
            IntPtr wPtr;
            wch = new NCursesWCHAR();
            using(wch.ToPointer(out wPtr))
            {
                NCursesException.Verify(NativeNCurses.NCursesWrapper.getbkgrnd(wPtr), "getbkgrnd");
            }
        }
        #endregion

        #region getn_wstr
        /// <summary>
        /// see <see cref="get_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="n">the number of wide characters to get</param>
        public static void getn_wstr(StringBuilder wstr, int n)
        {
            int size = Marshal.SizeOf<uint>() * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.getn_wstr(strPtr, n), "getn_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
        #endregion

        #region hline_set
        /// <summary>
        /// The* line_set functions use wch to draw a line starting at
        /// the current cursor position in the window.The line is at
        /// most n characters long or as many as fit into the window.
        /// The current cursor position is not changed.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">the character to use as line</param>
        /// <param name="n">the lenght of the line</param>
        public static void hline_set(NCursesWCHAR wch, int n)
        {
            IntPtr wPtr;
            using(wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.hline_set(wPtr, n), "hline_set");
            }
        }
        #endregion

        #region in_wch
        /// <summary>
        /// These functions extract the complex character  and rendition from  the current position in the named window into
        /// the cchar_t object referenced by wcval.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">a reference to store the complex character in</param>
        public static void in_wch(out NCursesWCHAR wcval)
        {
            IntPtr wPtr;
            wcval = new NCursesWCHAR();
            using (wcval.ToPointer(out wPtr))
            {
                NCursesException.Verify(NativeNCurses.NCursesWrapper.in_wch(wPtr), "in_wch");
            }
        }
        #endregion

        #region in_wchnstr
        /// <summary>
        /// These functions return an array of complex  characters  in
        /// wchstr,  starting at  the current cursor position in the
        /// named window.Attributes (rendition) are stored with the
        /// characters.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">array reference to store the complex characters in</param>
        public static void in_wchnstr(ref NCursesWCHAR[] wcval, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wcval, false, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.in_wchnstr(arrayPtr, n), "in_wchnstr");
                for (int i = 0; i < wcval.Length; i++)
                    wcval[i].ToStructure(arrayPtr + (i * wcval[i].Size));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region in_wchstr
        /// <summary>
        /// see <see cref="in_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">array reference to store the complex characters in</param>
        public static void in_wchstr(ref NCursesWCHAR[] wcval)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wcval, false, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.in_wchstrr(arrayPtr), "in_wchstr");
                for (int i = 0; i < wcval.Length; i++)
                    wcval[i].ToStructure(arrayPtr + (i * wcval[i].Size));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region innwstr
        /// <summary>
        /// These routines  return  a string of wchar_t characters in
        /// wstr, extracted starting at the current cursor position in
        /// the named window.Attributes are stripped from the characters.The four functions with n as  the last  argument
        /// return a leading substring at most n bytes long (exclusive
        /// of the trailing NUL).  Transfer stops at the  end of  the
        /// current  line,  or when  n bytes have been stored at the
        /// location referenced by wstr.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">a reference to store the string in</param>
        /// <param name="n">the number of characters to store</param>
        public static void innwstr(StringBuilder str, int n)
        {
            int size = Constants.SIZEOF_WCHAR_T * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.innwstr(strPtr, n), "innwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
        #endregion

        #region ins_nwstr
        /// <summary>
        /// These routines insert a wchar_t character string (as many
        /// characters as will fit on the line) before the  character
        /// under the cursor.All characters to the right of the cursor are shifted right, with the possibility of the right-
        /// most characters  on the line being lost.No wrapping is
        /// performed.The cursor position does  not change  (after
        /// moving  to y, x, if specified).  The four routines with n
        /// as the last argument insert a leading substring of at most
        /// n wchar_t  characters.If n is less than 1, the entire
        /// string is inserted.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">the string to insert</param>
        /// <param name="n">the number of characters to insert</param>
        public static void ins_nwstr(string str, int n)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((strPtr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.ins_nwstr(strPtr, n), "ins_nwstr"),
                str);
        }
        #endregion

        #region ins_wch
        /// <summary>
        /// These routines, insert the complex character wch with rendition before the character under the cursor.All charac-
        /// ters to the right of the cursor are moved one space to the
        /// right, with the possibility of the rightmost character  on
        /// the  line being  lost.The insertion operation does not
        /// change the cursor position.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">the complex character to insert</param>
        public static void ins_wch(NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using(wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.ins_wch(wPtr), "ins_wch");
            }
        }
        #endregion

        #region ins_wstr
        /// <summary>
        /// see <see cref="ins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">the string to insert</param>
        public static void ins_wstr(string str)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((strPtr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.ins_wstr(strPtr), "ins_wstr"),
                str, true);
        }
        #endregion

        #region inwstr
        /// <summary>
        /// see <see cref="innwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">the string to insert</param>
        public static void inwstr(StringBuilder str)
        {
            int size = Constants.SIZEOF_WCHAR_T * str.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.inwstr(strPtr), "inwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, str.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
        #endregion

        #region mvadd_wch
        /// <summary>
        /// see <see cref="add_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvadd_wch(int y, int x, NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvadd_wch(y, x, wPtr), "mvadd_wch");
            }
        }
        #endregion

        #region mvadd_wchnstr
        /// <summary>
        /// see <see cref="add_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvadd_wchnstr(int y, int x, NCursesWCHAR[] wchStr, int n)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvadd_wchnstr(y, x, arrayPtr, n), "mvadd_wchnstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region mvadd_wchstr
        /// <summary>
        /// see <see cref="add_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvadd_wchstr(int y, int x, NCursesWCHAR[] wchStr)
        {
            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wchStr, true, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvadd_wchstr(y, x, arrayPtr), "mvadd_wchstr");
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        #region mvaddnwstr
        /// <summary>
        /// see <see cref="addnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvaddnwstr(int y, int x, string wstr, int n)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvaddnwstr(y, x, ptr, n), "mvaddnwstr"),
                wstr);
        }
        #endregion

        #region mvaddwstr
        /// <summary>
        /// see <see cref="addwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvaddwstr(int y, int x, string wstr)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((ptr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvaddwstr(y, x, ptr), "mvaddwstr"),
                wstr, true);
        }
        #endregion

        #region mvget_wch
        /// <summary>
        /// see <see cref="get_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvget_wch(int y, int x, out char wch)
        {
            IntPtr chPtr = Marshal.AllocHGlobal(Marshal.SizeOf<uint>());
            GC.AddMemoryPressure(Marshal.SizeOf<uint>());

            try
            {
                NCursesException.Verify(NativeNCurses.NCursesWrapper.mvget_wch(y, x, chPtr), "mvget_wch");

                byte[] arr = new byte[Marshal.SizeOf<uint>()];
                Marshal.Copy(chPtr, arr, 0, Marshal.SizeOf<uint>());

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    wch = Encoding.Unicode.GetChars(arr)[0];
                else
                    wch = Encoding.UTF32.GetChars(arr)[0];
            }
            finally
            {
                Marshal.FreeHGlobal(chPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<uint>());
            }
        }
        #endregion

        #region mvget_wstr
        /// <summary>
        /// see <see cref="get_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvget_wstr(int y, int x, StringBuilder wstr)
        {
            int size = Marshal.SizeOf<uint>() * wstr.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvget_wstr(y, x, strPtr), "mvget_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, wstr.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(Marshal.SizeOf<uint>());
            }
        }
        #endregion

        #region mvgetn_wstr
        /// <summary>
        /// see <see cref="getn_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvgetn_wstr(int y, int x, StringBuilder wstr, int n)
        {
            int size = Marshal.SizeOf<uint>() * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvgetn_wstr(y, x, strPtr, n), "mvgetn_wstr");
                wstr.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
        #endregion

        #region mvhline_set
        /// <summary>
        /// see <see cref="hline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvhline_set(int y, int x, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvhline_set(y, x, wPtr, n), "mvhline_set");
            }
        }
        #endregion

        #region mvin_wch
        /// <summary>
        /// see <see cref="in_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvin_wch(int y, int x, ref NCursesWCHAR wcval)
        {
            IntPtr wPtr;
            using (wcval.ToPointer(out wPtr))
            {
                NCursesException.Verify(NativeNCurses.NCursesWrapper.mvin_wch(y, x, wPtr), "mvin_wch");
            }
        }
        #endregion

        #region mvin_wchnstr
        /// <summary>
        /// see <see cref="in_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvin_wchnstr(int y, int x, ref NCursesWCHAR[] wcval, int n)
        {
            if (n != wcval.Length)
                throw new ArgumentException("lenght of the array and n should be the same");

            int totalSize = 0;
            IntPtr arrayPtr = NativeNCurses.MarshallArray(wcval, false, out totalSize);
            GC.AddMemoryPressure(totalSize);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvin_wchnstr(y, x, arrayPtr, totalSize), "mvin_wchnstr");
                for (int i = 0; i < wcval.Length; i++)
                    wcval[i].ToStructure(arrayPtr + (i * wcval[i].Size));
            }
            finally
            {
                Marshal.FreeHGlobal(arrayPtr);
                GC.RemoveMemoryPressure(totalSize);
            }
        }
        #endregion

        /* can not implement mvin_wchstr, returns ERR in ncurses implementation */

        #region mvinnwstr
        /// <summary>
        /// see <see cref="innwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinnwstr(int y, int x, StringBuilder str, int n)
        {
            int size = Constants.SIZEOF_WCHAR_T * n;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvinnwstr(y, x, strPtr, n), "mvinnwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, n));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
        #endregion

        #region mvins_nwstr
        /// <summary>
        /// see <see cref="ins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvins_nwstr(int y, int x, string str, int n)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((strPtr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvins_nwstr(y, x, strPtr, n), "mvins_nwstr"),
                str);
        }
        #endregion

        #region mvins_wch
        /// <summary>
        /// see <see cref="ins_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvins_wch(int y, int x, NCursesWCHAR wch)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvins_wch(y, x, wPtr), "mvins_wch");
            }
        }
        #endregion

        #region mvins_wstr
        /// <summary>
        /// see <see cref="ins_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvins_wstr(int y, int x, string str)
        {
            NativeNCurses.MarshalNativeWideStringAndExecuteAction((strPtr) =>
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvins_wstr(y, x, strPtr), "mvins_wstr"),
                str, true);
        }
        #endregion

        #region mvinwstr
        /// <summary>
        /// see <see cref="inwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinwstr(int y, int x, StringBuilder str)
        {
            int size = Constants.SIZEOF_WCHAR_T * str.Capacity;
            IntPtr strPtr = Marshal.AllocHGlobal(size);
            GC.AddMemoryPressure(size);

            try
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvinwstr(y, x, strPtr), "mvinwstr");
                str.Append(NativeNCurses.MarshalStringFromNativeWideString(strPtr, str.Capacity));
            }
            finally
            {
                Marshal.FreeHGlobal(strPtr);
                GC.RemoveMemoryPressure(size);
            }
        }
        #endregion

        #region mvvline_set
        /// <summary>
        /// see <see cref="vline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvvline_set(int y, int x, NCursesWCHAR wch, int n)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.mvvline_set(y, x, wPtr, n), "mvvline_set");
            }
        }
        #endregion

        #region vline_set
        /// <summary>
        /// see <see cref="hline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void vline_set(NCursesWCHAR wch, int n)
        {
            IntPtr wPtr;
            using (wch.ToPointer(out wPtr))
            {
                NativeNCurses.VerifyNCursesMethod(() => NativeNCurses.NCursesWrapper.vline_set(wPtr, n), "vline_set");
            }
        }
        #endregion
    }
}
