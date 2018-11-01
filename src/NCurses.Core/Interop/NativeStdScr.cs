﻿using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Buffers;
using NCurses.Core.Interop.Wide;
using NCurses.Core.Interop.Small;
using NCurses.Core.Interop.WideStr;
using NCurses.Core.Interop.SmallStr;
using NCurses.Core.Interop.Dynamic;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop
{
    /// <summary>
    /// native stdscr methods (which is a window).
    /// all methods can be found at http://invisible-island.net/ncurses/man/ncurses.3x.html#h3-Routine-Name-Index
    /// </summary>
    internal static class NativeStdScr
    {
        #region Custom type wrapper fields
        private static INativeStdScrWide wideStdScrWrapper;
        private static INativeStdScrWide WideStdScrWrapper => NativeNCurses.HasUnicodeSupport
              ? wideStdScrWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage)
              : throw new InvalidOperationException(Constants.NoUnicodeExceptionMessage);
        private static INativeStdScrWideStr wideStrStdScrWrapper;
        private static INativeStdScrWideStr WideStrStdScrWrapper => NativeNCurses.HasUnicodeSupport
              ? wideStrStdScrWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage)
              : throw new InvalidOperationException(Constants.NoUnicodeExceptionMessage);

        private static INativeStdScrSmall smallStdScrWrapper;
        private static INativeStdScrSmall SmallStdScrWrapper => smallStdScrWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);
        private static INativeStdScrSmallStr smallStrStdScrWrapper;
        private static INativeStdScrSmallStr SmallStrStdScrWrapper => smallStrStdScrWrapper ?? throw new InvalidOperationException(Constants.TypeGenerationExceptionMessage);
        #endregion

        #region custom type initialization
        internal static void CreateCharCustomWrappers()
        {
            if (DynamicTypeBuilder.schar is null)
                throw new InvalidOperationException("Custom types haven't been generated yet.");

            Type customType;
            if (smallStrStdScrWrapper is null)
            {
                customType = typeof(NativeStdScrSmallStr<>).MakeGenericType(DynamicTypeBuilder.schar);
                smallStrStdScrWrapper = (INativeStdScrSmallStr)Activator.CreateInstance(customType);
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
                if (wideStdScrWrapper is null)
                {
                    customType = typeof(NativeStdScrWide<,,,>).MakeGenericType(DynamicTypeBuilder.cchar_t, DynamicTypeBuilder.wchar_t, DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar);
                    wideStdScrWrapper = (INativeStdScrWide)Activator.CreateInstance(customType);
                }

                if (wideStrStdScrWrapper is null)
                {
                    customType = typeof(NativeStdScrWideStr<,>).MakeGenericType(DynamicTypeBuilder.wchar_t, DynamicTypeBuilder.schar);
                    wideStrStdScrWrapper = (INativeStdScrWideStr)Activator.CreateInstance(customType);
                }
            }

            if (smallStdScrWrapper is null)
            {
                customType = typeof(NativeStdScrSmall<,>).MakeGenericType(DynamicTypeBuilder.chtype, DynamicTypeBuilder.schar);
                smallStdScrWrapper = (INativeStdScrSmall)Activator.CreateInstance(customType);
            }
        }
        #endregion

        #region addch
        /// <summary>
        /// The addch, waddch, mvaddch and mvwaddch routines put the
        /// character ch into the given window at its current  window
        /// position,  which  is then advanced.They are analogous to
        /// putchar in stdio(3).
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="ch">The character you want to add</param>
        public static void addch(INCursesSCHAR ch)
        {
            SmallStdScrWrapper.addch(ch);
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
        public static void addchnstr(INCursesSCHARStr txt, int number)
        {
            SmallStdScrWrapper.addchnstr(txt, number);
        }
        #endregion

        #region addchstr
        /// <summary>
        /// see <see cref="addchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="txt">the string you want to add</param>
        public static void addchstr(INCursesSCHARStr txt)
        {
            SmallStdScrWrapper.addchstr(txt);
        }
        #endregion

        #region addnstr
        /// <summary>
        /// These functions  write the(null-terminated)  character
        /// string str on the given window.It is similar to  calling
        /// waddch once for each character in the string. It gets wrapped
        /// into wide overloads if supported
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="txt">string to add</param>
        /// <param name="number">number of elements to copy</param>
        public static void addnstr(string txt, int number)
        {
            SmallStrStdScrWrapper.addnstr(txt, number);
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
            SmallStrStdScrWrapper.addstr(txt);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.attroff(attrs), "attroff");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.attron(attrs), "attron");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.attrset(attrs), "attrset");
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
        public static void attr_on(in ulong attrs)
        {
            SmallStdScrWrapper.attr_on(attrs);
        }
        #endregion

        #region attr_off
        /// <summary>
        /// see <see cref="attr_on"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">attribute(s) to disable</param>
        public static void attr_off(in ulong attrs)
        {
            SmallStdScrWrapper.attr_off(attrs);
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
        public static void attr_set(ulong attrs, short pair)
        {
            SmallStdScrWrapper.attr_set(attrs, pair);
        }
        #endregion

        #region attr_get
        /// <summary>
        /// see <see cref="attr_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="attrs">Pointer to an chtype to retrieve current attributes</param>
        /// <param name="pair">Pointer to a short to retrieve current color pair</param>
        public static void attr_get(out ulong attrs, out short pair)
        {
            SmallStdScrWrapper.attr_get(out attrs, out pair);
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
        public static void bkgd(in INCursesSCHAR bkgd)
        {
            SmallStdScrWrapper.bkgd(bkgd);
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
        public static void bkgdset(in INCursesSCHAR bkgd)
        {
            SmallStdScrWrapper.bkgdset(bkgd);
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
        public static void border(in INCursesSCHAR ls, in INCursesSCHAR rs, in INCursesSCHAR ts, in INCursesSCHAR bs, in INCursesSCHAR tl, in INCursesSCHAR tr, in INCursesSCHAR bl, in INCursesSCHAR br)
        {
            SmallStdScrWrapper.border(ls, rs, ts, bs, tl, tr, bl, br);
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
        public static void chgat(int number, ulong attrs, short pair)
        {
            SmallStdScrWrapper.chgat(number, attrs, pair);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.clear(), "clear");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.clrtobot(), "clrtobot");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.clrtoeol(), "clrtoeol");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.color_set(pair, IntPtr.Zero), "color_set");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.delch(), "delch");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.deleteln(), "deleteln");
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
        public static void echochar(in INCursesSCHAR ch)
        {
            SmallStdScrWrapper.echochar(ch);
        }
        #endregion

        #region erase
        /// <summary>
        /// The erase and werase routines copy blanks to  every position in the window, clearing the screen.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void erase()
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.erase(), "erase");
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
            int ret = 0;
            NCursesException.Verify(ret = NativeNCurses.NCursesWrapper.getch(), "getch");
            return ret;
        }

        /// <summary>
        /// <see cref="NativeStdScr.getch"/>
        /// </summary>
        public static bool getch(out char ch, out Key key)
        {
            return NativeNCurses.VerifyInput("getch", NativeNCurses.NCursesWrapper.getch(), out ch, out key);
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
        public static void getnstr(out string str, int count)
        {
            SmallStrStdScrWrapper.getnstr(out str, count);
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
        public static void getstr(out string str)
        {
            SmallStrStdScrWrapper.getstr(out str);
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
        public static void hline(INCursesSCHAR ch, int count)
        {
            SmallStdScrWrapper.hline(ch, count);
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
        public static void inch(out INCursesSCHAR ch)
        {
            SmallStdScrWrapper.inch(out ch);
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
        public static void inchnstr(out INCursesSCHARStr txt, int count, out int read)
        {
            SmallStdScrWrapper.inchnstr(out txt, count, out read);
        }
        #endregion

        #region inchstr
        /// <summary>
        /// see <see cref="inchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="txt">the array to copy the chars into</param>
        public static void inchstr(out INCursesSCHARStr str, out int read)
        {
            SmallStdScrWrapper.inchstr(out str, out read);
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
        public static void innstr(out string str, int n, out int read)
        {
            SmallStrStdScrWrapper.innstr(out str, n, out read);
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
        public static void insch(in INCursesSCHAR ch)
        {
            SmallStdScrWrapper.insch(ch);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.insdelln(n), "insdelln");
        }
        #endregion

        #region insertln
        /// <summary>
        /// insert  a  blank  line above the current line and the bottom line is lost.
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void insertln()
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.insertln(), "insertln");
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
        public static void insnstr(in string str, int n)
        {
            SmallStrStdScrWrapper.insnstr(str, n);
        }
        #endregion

        #region insstr
        /// <summary>
        /// see <see cref="insnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">The string to insert</param>
        public static void insstr(in string str)
        {
            SmallStrStdScrWrapper.insstr(str);
        }
        #endregion

        #region instr
        /// <summary>
        /// see <see cref="innstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">Reference to the string you want extracted</param>
        public static void instr(out string str, out int read)
        {
            SmallStrStdScrWrapper.instr(out str, out read);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.move(y, x), "move");
        }
        #endregion

        #region mvaddch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addch(chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// <param name="ch">the character to add</param>
        public static void mvaddch(int y, int x, in INCursesSCHAR ch)
        {
            SmallStdScrWrapper.mvaddch(y, x, ch);
        }
        #endregion

        #region mvaddchnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addchnstr(IntPtr, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// <param name="chstr">pointer to a null terminated array of chtype</param>
        /// <param name="n">number of elements to copy</param>
        public static void mvaddchnstr(int y, int x, in INCursesSCHARStr chstr, int n)
        {
            SmallStdScrWrapper.mvaddchnstr(y, x, chstr, n);
        }
        #endregion

        #region mvaddchstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addchstr(IntPtr)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// <param name="chstr">pointer to a null terminated array of chtype</param>
        public static void mvaddchstr(int y, int x, in INCursesSCHARStr chstr)
        {
            SmallStdScrWrapper.mvaddchstr(y, x, chstr);
        }
        #endregion

        #region mvaddnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addnstr(string, int)"/>
        /// <para />native method wrapped with verification.
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// </summary>
        public static void mvaddnstr(int y, int x, in string txt, int n)
        {
            SmallStrStdScrWrapper.mvaddnstr(y, x, txt, n);
        }
        #endregion

        #region mvaddstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="addstr(string)"/>
        /// <para />native method wrapped with verification.
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// </summary>
        public static void mvaddstr(int y, int x, in string txt)
        {
            SmallStrStdScrWrapper.mvaddstr(y, x, txt);
        }
        #endregion

        #region mvchgat
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="chgat(int, chtype, short)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvchgat(int y, int x, int number, ulong attrs, short pair)
        {
            SmallStdScrWrapper.mvchgat(y, x, number, attrs, pair);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.mvdelch(y, x), "mvdelch");
        }
        #endregion

        #region mvgetch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="getch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static int mvgetch(int y, int x)
        {
            int ret = 0;
            NCursesException.Verify(NativeNCurses.NCursesWrapper.mvgetch(y, x), "mvgetch");
            return ret;
        }

        /// <summary>
        /// <see cref="NativeStdScr.mvgetch(int, int)"/>
        /// </summary>
        public static bool mvgetch(int y, int x, out char ch, out Key key)
        {
            return NativeNCurses.VerifyInput("mvgetch", NativeNCurses.NCursesWrapper.mvgetch(y, x), out ch, out key);
        }
        #endregion

        #region mvgetnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="getnstr(StringBuilder, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvgetnstr(int y, int x, out string str, int count)
        {
            SmallStrStdScrWrapper.mvgetnstr(y, x, out str, count);
        }
        #endregion

        #region mvgetstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="getstr(StringBuilder)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvgetstr(int y, int x, out string str)
        {
            SmallStrStdScrWrapper.mvgetstr(y, x, out str);
        }
        #endregion

        #region mvhline
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="hline(chtype, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvhline(int y, int x, in INCursesSCHAR ch, int count)
        {
            SmallStdScrWrapper.mvhline(y, x, ch, count);
        }
        #endregion

        #region mvinch
        /// <summary>
        /// see <see cref="inch()"/>
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        /// <returns>Constants.ERR on error or Constants.OK on success</returns>
        public static void mvinch(int y, int x, out INCursesSCHAR ch)
        {
            SmallStdScrWrapper.mvinch(y, x, out ch);
        }
        #endregion

        #region mvinchnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="inchnstr(IntPtr, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinchnstr(int y, int x, out INCursesSCHARStr txt, int count, out int read)
        {
            SmallStdScrWrapper.mvinchnstr(y, x, out txt, count, out read);
        }
        #endregion

        #region mvinchstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="inchstr(IntPtr)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinchstr(int y, int x, out INCursesSCHARStr chstr, out int read)
        {
            SmallStdScrWrapper.mvinchstr(y, x, out chstr, out read);
        }
        #endregion

        #region mvinnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="innstr(StringBuilder, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinnstr(int y, int x, out string str, int n, out int read)
        {
            SmallStrStdScrWrapper.mvinnstr(y, x, out str, n, out read);
        }
        #endregion

        #region mvinsch
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="insch(chtype)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinsch(int y, int x, in INCursesSCHAR ch)
        {
            SmallStdScrWrapper.mvinsch(y, x, ch);
        }
        #endregion

        #region mvinsnstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="insnstr(string, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinsnstr(int y, int x, in string str, int n)
        {
            SmallStrStdScrWrapper.mvinsnstr(y, x, str, n);
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
            SmallStrStdScrWrapper.mvinsstr(y, x, str);
        }
        #endregion

        #region mvinstr
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="instr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinstr(int y, int x, out string str, out int read)
        {
            SmallStrStdScrWrapper.mvinstr(y, x, out str, out read);
        }
        #endregion

        #region mvprintw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="printw(string, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvprintw(int y, int x, in string format, params string[] varArg)
        {
            SmallStrStdScrWrapper.mvprintw(y, x, format, varArg);
        }
        #endregion

        #region mvscanw
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="scanw(StringBuilder, object[])"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvscanw(int y, int x, out string format, params string[] argList)
        {
            SmallStrStdScrWrapper.mvscanw(y, x, out format, argList);
        }
        #endregion

        #region mvvline
        /// <summary>
        /// move cursor position to line <paramref name="y"/> and column <paramref name="x"/> and see <see cref="vline(chtype, int)"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvvline(int y, int x, in INCursesSCHAR ch, int n)
        {
            SmallStdScrWrapper.mvvline(y, x, ch, n);
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
        public static void printw(string format, params string[] argList)
        {
            SmallStrStdScrWrapper.printw(format, argList);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.refresh(), "refresh");
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
        public static void scanw(out string format, params string[] argList)
        {
            SmallStrStdScrWrapper.scanw(out format, argList);
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.scrl(n), "scrl");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.setscrreg(top, bot), "setscrreg");
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
            NCursesException.Verify(NativeNCurses.NCursesWrapper.standout(), "standout");
        }
        #endregion

        #region standend
        /// <summary>
        /// see <see cref="standout"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void standend()
        {
            NCursesException.Verify(NativeNCurses.NCursesWrapper.standend(), "standend");
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
        public static void vline(in INCursesSCHAR ch, int n)
        {
            SmallStdScrWrapper.vline(ch, n);
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
        public static void add_wch(INCursesWCHAR wch)
        {
            WideStdScrWrapper.add_wch(wch);
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
        public static void add_wchnstr(INCursesWCHARStr wchStr, int n)
        {
            WideStdScrWrapper.add_wchnstr(wchStr, n);
        }
        #endregion

        #region add_wchstr
        /// <summary>
        /// see <see cref="add_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wchStr">The string of complex characters you want to add</param>
        /// <param name="n">number of elements to copy</param>
        public static void add_wchstr(INCursesWCHARStr wchStr)
        {
            WideStdScrWrapper.add_wchstr(wchStr);
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
            WideStrStdScrWrapper.addnwstr(wstr, n);
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
            WideStrStdScrWrapper.addwstr(wstr);
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
        public static void bkgrnd(in INCursesWCHAR wch)
        {
            WideStdScrWrapper.bkgrnd(wch);
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
        public static void bkgrndset(in INCursesWCHAR wch)
        {
            WideStdScrWrapper.bkgrndset(wch);
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
        public static void border_set(in INCursesWCHAR ls, in INCursesWCHAR rs, in INCursesWCHAR ts, in INCursesWCHAR bs, in INCursesWCHAR tl, 
            in INCursesWCHAR tr, in INCursesWCHAR bl, in INCursesWCHAR br)
        {
            WideStdScrWrapper.border_set(ls, rs, ts, bs, tl, tr, bl, br);
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
        public static void echo_wchar(in INCursesWCHAR wch)
        {
            WideStdScrWrapper.echo_wchar(wch);
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
        public static bool get_wch(out char wch, out Key key)
        {
            return WideStrStdScrWrapper.get_wch(out wch, out key);
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
        public static void get_wstr(out string wstr)
        {
            WideStrStdScrWrapper.get_wstr(out wstr);
        }
        #endregion

        #region getbkgrnd
        /// <summary>
        /// The getbkgrnd function returns the given window's current
        /// background character/attribute pair via the wch pointer.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wch">a reference to store the returned background char in</param>
        public static void getbkgrnd(out INCursesWCHAR wch)
        {
            WideStdScrWrapper.getbkgrnd(out wch);
        }
        #endregion

        #region getn_wstr
        /// <summary>
        /// see <see cref="get_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="n">the number of wide characters to get</param>
        public static void getn_wstr(out string wstr, int n)
        {
            WideStrStdScrWrapper.getn_wstr(out wstr, n);
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
        public static void hline_set(INCursesWCHAR wch, int n)
        {
            WideStdScrWrapper.hline_set(wch, n);
        }
        #endregion

        #region in_wch
        /// <summary>
        /// These functions extract the complex character  and rendition from  the current position in the named window into
        /// the cchar_t object referenced by wcval.
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">a reference to store the complex character in</param>
        public static void in_wch(out INCursesWCHAR wcval)
        {
            WideStdScrWrapper.in_wch(out wcval);
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
        public static void in_wchnstr(out INCursesWCHARStr wcval, int n)
        {
            WideStdScrWrapper.in_wchnstr(out wcval, n);
        }
        #endregion

        #region in_wchstr
        /// <summary>
        /// see <see cref="in_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="wcval">array reference to store the complex characters in</param>
        public static void in_wchstr(out INCursesWCHARStr wcval)
        {
            WideStdScrWrapper.in_wchstr(out wcval);
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
        public static void innwstr(out string str, int n, out int read)
        {
            WideStrStdScrWrapper.innwstr(out str, n, out read);
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
        public static void ins_nwstr(in string str, int n)
        {
            WideStrStdScrWrapper.ins_nwstr(in str, n);
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
        public static void ins_wch(in INCursesWCHAR wch)
        {
            WideStdScrWrapper.ins_wch(wch);
        }
        #endregion

        #region ins_wstr
        /// <summary>
        /// see <see cref="ins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">the string to insert</param>
        public static void ins_wstr(in string str)
        {
            WideStrStdScrWrapper.ins_wstr(str);
        }
        #endregion

        #region inwstr
        /// <summary>
        /// see <see cref="innwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="str">the string to insert</param>
        public static void inwstr(out string str)
        {
            WideStrStdScrWrapper.inwstr(out str);
        }
        #endregion

        #region mvadd_wch
        /// <summary>
        /// see <see cref="add_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvadd_wch(int y, int x, in INCursesWCHAR wch)
        {
            WideStdScrWrapper.mvadd_wch(y, x, wch);
        }
        #endregion

        #region mvadd_wchnstr
        /// <summary>
        /// see <see cref="add_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvadd_wchnstr(int y, int x, in INCursesWCHARStr wchStr, int n)
        {
            WideStdScrWrapper.mvadd_wchnstr(y, x, wchStr, n);
        }
        #endregion

        #region mvadd_wchstr
        /// <summary>
        /// see <see cref="add_wchstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvadd_wchstr(int y, int x, in INCursesWCHARStr wchStr)
        {
            WideStdScrWrapper.mvadd_wchstr(y, x, wchStr);
        }
        #endregion

        #region mvaddnwstr
        /// <summary>
        /// see <see cref="addnwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvaddnwstr(int y, int x, in string wstr, int n)
        {
            WideStrStdScrWrapper.mvaddnwstr(y, x, wstr, n);
        }
        #endregion

        #region mvaddwstr
        /// <summary>
        /// see <see cref="addwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvaddwstr(int y, int x, in string wstr)
        {
            WideStrStdScrWrapper.mvaddwstr(y, x, wstr);
        }
        #endregion

        #region mvget_wch
        /// <summary>
        /// see <see cref="get_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static bool mvget_wch(int y, int x, out char wch, out Key key)
        {
            return WideStrStdScrWrapper.mvget_wch(y, x, out wch, out key);
        }
        #endregion

        #region mvget_wstr
        /// <summary>
        /// see <see cref="get_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvget_wstr(int y, int x, out string wstr)
        {
            WideStrStdScrWrapper.mvget_wstr(y, x, out wstr);
        }
        #endregion

        #region mvgetn_wstr
        /// <summary>
        /// see <see cref="getn_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvgetn_wstr(int y, int x, out string wstr, int n)
        {
            WideStrStdScrWrapper.mvgetn_wstr(y, x, out wstr, n);
        }
        #endregion

        #region mvhline_set
        /// <summary>
        /// see <see cref="hline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvhline_set(int y, int x, in INCursesWCHAR wch, int n)
        {
            WideStdScrWrapper.mvhline_set(y, x, wch, n);
        }
        #endregion

        #region mvin_wch
        /// <summary>
        /// see <see cref="in_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvin_wch(int y, int x, out INCursesWCHAR wcval)
        {
            WideStdScrWrapper.mvin_wch(y, x, out wcval);
        }
        #endregion

        #region mvin_wchnstr
        /// <summary>
        /// see <see cref="in_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvin_wchnstr(int y, int x, out INCursesWCHARStr wchStr, int n)
        {
            WideStdScrWrapper.mvin_wchnstr(y, x, out wchStr, n);
        }
        #endregion

        #region mvin_wchstr
        /// <summary>
        /// see <see cref="in_wchnstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvin_wchstr(int y, int x, out INCursesWCHARStr wchStr)
        {
            WideStdScrWrapper.mvin_wchstr(y, x, out wchStr);
        }
        #endregion

        #region mvinnwstr
        /// <summary>
        /// see <see cref="innwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinnwstr(int y, int x, out string str, int n, out int read)
        {
            WideStrStdScrWrapper.mvinnwstr(y, x, out str, n, out read);
        }
        #endregion

        #region mvins_nwstr
        /// <summary>
        /// see <see cref="ins_nwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvins_nwstr(int y, int x, in string str, int n)
        {
            WideStrStdScrWrapper.mvins_nwstr(y, x, str, n);
        }
        #endregion

        #region mvins_wch
        /// <summary>
        /// see <see cref="ins_wch"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvins_wch(int y, int x, in INCursesWCHAR wch)
        {
            WideStdScrWrapper.mvins_wch(y, x, wch);
        }
        #endregion

        #region mvins_wstr
        /// <summary>
        /// see <see cref="ins_wstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvins_wstr(int y, int x, in string str)
        {
            WideStrStdScrWrapper.mvins_wstr(y, x, str);
        }
        #endregion

        #region mvinwstr
        /// <summary>
        /// see <see cref="inwstr"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvinwstr(int y, int x, out string wstr)
        {
            WideStrStdScrWrapper.mvinwstr(y, x, out wstr);
        }
        #endregion

        #region mvvline_set
        /// <summary>
        /// see <see cref="vline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        /// <param name="y">the line number to move to</param>
        /// <param name="x">the column number to move to</param>
        public static void mvvline_set(int y, int x, in INCursesWCHAR wch, int n)
        {
            WideStdScrWrapper.mvvline_set(y, x, wch, n);
        }
        #endregion

        #region vline_set
        /// <summary>
        /// see <see cref="hline_set"/>
        /// <para />native method wrapped with verification.
        /// </summary>
        public static void vline_set(in INCursesWCHAR wch, int n)
        {
            WideStdScrWrapper.vline_set(wch, n);
        }
        #endregion
    }
}
