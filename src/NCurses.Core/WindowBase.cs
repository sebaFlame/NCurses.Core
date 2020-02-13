using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop;
using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Wrappers;

namespace NCurses.Core
{
    /* TODO
     * methods using INCursesChar should allow other types than their own implementation
    */
    public abstract class WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> 
        : IWindow, IEquatable<WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>>, IDisposable
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal static HashSet<WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>> Windows;

        internal static NativeNCursesInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> NCurses =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.NCursesInternal;
        internal static NativeWindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Window =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal;
        internal static NativeStdScrInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> StdScr =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.StdScrInternal;
        internal static NativeScreenInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Screen =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.ScreenInternal;
        internal static NativePadInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Pad =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.PadInternal;

        internal WindowBaseSafeHandle WindowBaseSafeHandle { get; private set; }

        private bool HasAddedRefToExistingSafeHandle = false;

        static WindowBase()
        {
            Windows = new HashSet<WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>>();
        }

        internal WindowBase(
            WindowBaseSafeHandle windowBaseSafeHandle)
        {
            this.WindowBaseSafeHandle = windowBaseSafeHandle;

            //this is needed for extended ASCII keyboard input
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.Meta = true;
            }
        }

        internal WindowBase(
            WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
        {
            this.WindowBaseSafeHandle = existingWindow.WindowBaseSafeHandle;

            this.WindowBaseSafeHandle.DangerousAddRef(ref this.HasAddedRefToExistingSafeHandle);
        }

        ~WindowBase()
        {
            this.Dispose();
        }

        #region properties
        /// <summary>
        /// return the current column number of the cursor
        /// </summary>
        public int CursorColumn
        {
            get { return Window.getcurx(this.WindowBaseSafeHandle); }
        }

        /// <summary>
        /// return the current line number of the cursor
        /// </summary>
        public int CursorLine
        {
            get { return Window.getcury(this.WindowBaseSafeHandle); }
        }

        /// <summary>
        /// return the maximum column number
        /// </summary>
        public int MaxColumn
        {
            get { return Window.getmaxx(this.WindowBaseSafeHandle); }
        }

        /// <summary>
        /// return the maximum line number
        /// </summary>
        public int MaxLine
        {
            get { return Window.getmaxy(this.WindowBaseSafeHandle); }
        }

        /// <summary>
        /// set/gets the current window background with a character with attributes applied
        /// </summary>
        public abstract INCursesChar BackGround { get; set; }

        /// <summary>
        /// set/gets the background for all insterted characters (with waddch)
        /// </summary>
        public abstract INCursesChar InsertBackGround { get; set; }

        /// <summary>
        /// enable/disable returning function keys on <see cref="ReadKey"/>  (Key.* defined in constants).
        /// disabled by default
        /// </summary>
        public bool KeyPad
        {
            get { return Window.is_keypad(this.WindowBaseSafeHandle); }
            set { Window.keypad(this.WindowBaseSafeHandle, value); }
        }

        /// <summary>
        /// return 8 bits instead of 7 from console input (could enable alt key usage)
        /// disabled by default
        /// </summary>
        public bool Meta
        {
            set { NCurses.meta(this.WindowBaseSafeHandle, value); }
        }

        /// <summary>
        /// enable/disable scrolling on the current window
        /// disabled by default
        /// </summary>
        public bool Scroll
        {
            get { return Window.is_scrollok(this.WindowBaseSafeHandle); }
            set { Window.scrollok(this.WindowBaseSafeHandle, value); }
        }

        /// <summary>
        /// use the hardware  insert/delete line  feature of  terminals so equipped.
        /// </summary>
        public bool UseHwInsDelLine
        {
            get { return Window.is_idlok(this.WindowBaseSafeHandle); }
            set { Window.idlok(this.WindowBaseSafeHandle, value); }
        }

        //TODO: gives read error on windows when true
        /// <summary>
        /// The nodelay option causes getch to be a non-blocking call.
        /// </summary>
        public bool Blocking
        {
            get { return Window.is_nodelay(this.WindowBaseSafeHandle); }
            set { Window.nodelay(this.WindowBaseSafeHandle, !value); }
        }

        /// <summary>
        /// if set wgetch does not set a timer.  The purpose of  the  timeout
        /// is  to differentiate between sequences received from a function key and
        /// those typed by a user.
        /// </summary>
        public bool NoTimeout
        {
            set { Window.notimeout(this.WindowBaseSafeHandle, value); }
            get { return Window.is_notimeout(this.WindowBaseSafeHandle); }
        }
        #endregion

        public abstract bool HasUnicodeSupport { get; }

        #region Cursor
        /// <summary>
        /// move the cursor to position <paramref name="lineNumber"/> and <paramref name="columnNumber"/>
        /// </summary>
        /// <param name="lineNumber">the line number to move the cursor to</param>
        /// <param name="columnNumber">the column number to move the cursor to</param>
        public abstract void MoveCursor(int lineNumber, int columnNumber);
        #endregion

        #region Attributes and Color manipulation
        /// <summary>
        /// Turn  1 or multiple attributes OR'd together on
        /// Colors are NOT supported
        /// </summary>
        /// <param name="Attributes">Attributes to enable</param>
        public abstract void AttributesOn(ulong Attributes);

        /// <summary>
        /// Turn on 1 or multiple attributes
        /// Colors are NOT supported
        /// </summary>
        /// <param name="Attributes">Attributes to disable</param>
        public abstract void AttributesOff(ulong Attributes);

        /// <summary>
        /// Get current enables attributes and active color pair
        /// </summary>
        /// <param name="attrs">The current enabled attributes OR'd together</param>
        /// <param name="colorPair">The current active color pair</param>
        public abstract void CurrentAttributesAndColor(out ulong attrs, out short colorPair);

        /// <summary>
        /// Activate attributes OR's together and color
        /// </summary>
        /// <param name="attrs">Attributes to enable</param>
        /// <param name="colorPair">Color pair to use</param>
        public abstract void EnableAttributesAndColor(ulong attrs, short colorPair);

        /// <summary>
        /// Activate a color
        /// </summary>
        /// <param name="colorPair">The color pair to use</param>
        public abstract void EnableColor(short colorPair);
        #endregion

        #region Write
        /// <summary>
        /// write a (generated) NCurses type to the window
        /// can be useful for ACS symbols (single byte characters)
        /// </summary>
        /// <param name="ch">the character to write to the window</param>
        public abstract void Write(in INCursesChar ch);

        /// <summary>
        /// write a (generated) string of an NCurses type to the window
        /// </summary>
        /// <param name="str">the string to write to the window</param>
        public abstract void Write(in INCursesCharString str);

        /// <summary>
        /// write string <paramref name="str"/> to the window.
        /// </summary>
        /// <param name="str">the string to write</param>
        public abstract void Write(string str);

        /// <summary>
        /// write string <paramref name="str"/> to the window. with defined attributes/color pair.
        /// </summary>
        /// <param name="str">the string to write</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public abstract void Write(string str, ulong attrs, short pair);

        /// <summary>
        /// write string <paramref name="str"/> to the window on line <paramref name="nline"/> and column <paramref name="ncol"/>.
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="str">the string to add</param>
        public abstract void Write(int nline, int ncol, string str);

        /// <summary>
        /// write string <paramref name="str"/> to the window on line <paramref name="nline"/> and column <paramref name="ncol"/>.
        /// with defined attributes/color pair.
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="str">the string to add</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public abstract void Write(int nline, int ncol, string str, ulong attrs, short pair);

        /// <summary>
        /// write character <paramref name="ch"/> to the window.
        /// </summary>
        /// <param name="ch">the character to add</param>
        public abstract void Write(char ch);

        /// <summary>
        /// write character <paramref name="ch"/> to the window with defined attributes/color pair.
        /// </summary>
        /// <param name="ch">the character to add</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public abstract void Write(char ch, ulong attrs, short pair);

        /// <summary>
        /// write the character/attributes <paramref name="ch"/> to line <paramref name="nline"/> and column <paramref name="ncol"/>.
        /// see <see cref="Write(ulong)"/>
        /// </summary>
        /// <param name="nline">the line number to add the char to</param>
        /// <param name="ncol">the column number to add the char to</param>
        /// <param name="ch">the character/attributes to add</param>
        public abstract void Write(int nline, int ncol, char ch);

        /// <summary>
        /// write the character/attributes <paramref name="ch"/> to line <paramref name="nline"/> and column <paramref name="ncol"/>
        /// with defined attributes/color pair.
        /// </summary>
        /// <param name="nline">the line number to add the char to</param>
        /// <param name="ncol">the column number to add the char to</param>
        /// <param name="ch">the character/attributes to add</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public abstract void Write(int nline, int ncol, char ch, ulong attrs, short pair);

        /// <summary>
        /// write byte array <paramref name="str"/>  encoded in <paramref name="encoding"/> to the window.
        /// </summary>
        /// <param name="str">the string to write</param>
        /// <param name="encoding">encoding of <paramref name="str"/></param>
        public abstract void Write(byte[] str, Encoding encoding);

        /// <summary>
        /// write byte array <paramref name="str"/> encoded in <paramref name="encoding"/> to the window. with defined attributes/color pair.
        /// </summary>
        /// <param name="str">the string to write</param>
        /// <param name="encoding">encoding of <paramref name="str"/></param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public abstract void Write(byte[] str, Encoding encoding, ulong attrs, short pair);

        /// <summary>
        /// write byte array <paramref name="str"/> encoded in <paramref name="encoding"/> to the window on line <paramref name="nline"/> and column <paramref name="ncol"/>.
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="str">the string to add</param>
        /// <param name="encoding">encoding of <paramref name="str"/></param>
        public abstract void Write(int nline, int ncol, byte[] str, Encoding encoding);

        /// <summary>
        /// write byte array <paramref name="str"/> encoded in <paramref name="encoding"/> to the window on line <paramref name="nline"/> and column <paramref name="ncol"/>.
        /// with defined attributes/color pair.
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="str">the string to add</param>
        /// <param name="encoding">encoding of <paramref name="str"/></param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public abstract void Write(int nline, int ncol, byte[] str, Encoding encoding, ulong attrs, short pair);

        public abstract void Write(int nline, int ncol, in INCursesChar ch);
        public abstract void Write(int nline, int ncol, in INCursesCharString str);
        #endregion

        #region write input
        public abstract void Put(char ch);

        public abstract void Put(Key key);
        #endregion

        #region read input
        /// <summary>
        /// read a character from console input.
        /// can also be an escaped character/function key <see cref="NativeNCurses.IsCTRL(int)"/> and <see cref="NativeNCurses.IsALT(int, int)"/>
        /// if this returns a multibyte character, it's already decoded (incorrectly), so escaping information is lost
        /// use the appropriate static methods in that case
        /// </summary>
        /// <param name="ch">the read character</param>
        /// <param name="key">the read function key</param>
        /// <returns>TRUE if the read key is a function key</returns>
        public abstract bool ReadKey(out char ch, out Key key);

        /// <summary>
        /// read a character from console input after moving the cursor to <paramref name="nline"/> and <paramref name="ncol"/>
        /// can also be an escaped character/function key <see cref="NativeNCurses.IsCTRL(int)"/> and <see cref="NativeNCurses.IsALT(int, int)"/>
        /// if this returns a multibyte character, it's already decoded (incorrectly), so escaping information is lost
        /// use the appropriate static methods in that case
        /// </summary>
        /// <param name="nline">the line number to move to</param>
        /// <param name="ncol">the column number to move to</param>
        /// <param name="ch">the read character</param>
        /// <param name="key">the read function key</param>
        /// <returns>TRUE if the read key is a function key</returns>
        public abstract bool ReadKey(int nline, int ncol, out char ch, out Key key);

        /// <summary>
        /// read a string of atmost 1023 characters from the console input or until return
        /// Also refreshes the window if it hasn't been refreshed yet.
        /// </summary>
        /// <returns>the read string</returns>
        public abstract string ReadLine();

        /// <summary>
        /// read a string of atmost 1023 characters from the console input or until return
        /// Also refreshes the window if it hasn't been refreshed yet.
        /// </summary>
        /// <param name="nline">the line number to move to</param>
        /// <param name="ncol">the column number to move to</param>
        /// <returns>the read string</returns>
        public abstract string ReadLine(int nline, int ncol);

        /// <summary>
        /// read a string of a particular length from the console input or until return
        /// Also refreshes the window if it hasn't been refreshed yet.
        /// </summary>
        /// <param name="length">count of characters to read</param>
        /// <returns>the read string</returns>
        public abstract string ReadLine(int length);

        /// <summary>
        /// read a string of a particular length from the console input or until return
        /// Also refreshes the window if it hasn't been refreshed yet.
        /// </summary>
        /// <param name="nline">the line number to move to</param>
        /// <param name="ncol">the column number to move to</param>
        /// <param name="length">count of characters to read</param>
        /// <returns>the read string</returns>
        public abstract string ReadLine(int nline, int ncol, int length);
        #endregion

        #region insert
        /// <summary>
        /// insert a character on the current cursor position. all characaters on the right move 1 column. character might fall off at the end of the line.
        /// </summary>
        /// <param name="ch">the character to insert</param>
        public abstract void Insert(char ch);

        public abstract void Insert(in INCursesChar ch);
        public abstract void Insert(int nline, int ncol, in INCursesChar ch);

        /// <summary>
        /// insert a character on line <paramref name="nline"/> and column <paramref name="ncol"/>. all characaters on the right move 1 column. character might fall off at the end of the line.
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="ch">the character to insert</param>
        public abstract void Insert(int nline, int ncol, char ch);

        /// <summary>
        /// insert a character with attributes/color on the current cursor position. all characaters on the right move 1 column. character might fall off at the end of the line.
        /// </summary>
        /// <param name="ch">the character to insert</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public abstract void Insert(char ch, ulong attrs, short pair);

        /// <summary>
        /// insert a character with attributes/color on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// all characaters on the right move 1 column. character might fall off at the end of the line.
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="ch">the character to insert</param>
        /// <param name="attrs">the attributes you want to add (eg <see cref="Attrs.BOLD"/>)</param>
        /// <param name="pair">the color pair you want to use on this character</param>
        public abstract void Insert(int nline, int ncol, char ch, ulong attrs, short pair);

        /// <summary>
        /// insert a string on the current cursor position. all characaters on the right move the lenght of the string.
        /// character might fall off at the end of the line.
        /// </summary>
        /// <param name="str">the string to insert</param>
        public abstract void Insert(string str);

        /// <summary>
        /// insert a string on line <paramref name="nline"/> and column <paramref name="ncol"/>. all characaters on the right move 1 column. character might fall off at the end of the line.
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="str">the string to insert</param>
        public abstract void Insert(int nline, int ncol, string str);

        public abstract void Insert(string str, ulong attrs, short pair);
        #endregion

        #region extract output
        /// <summary>
        /// read a character from the console output at the current position
        /// </summary>
        /// <param name="charsWithAttributes">The returned charactes with attributes</param>
        public abstract void ExtractChar(out INCursesChar ch);

        /// <summary>
        /// read a character from the console output at the current position
        /// </summary>
        /// <returns>the read character</returns>
        public abstract char ExtractChar();

        /// <summary>
        /// read a character from the console output on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <returns>the read character</returns>
        public abstract char ExtractChar(int nline, int ncol);

        /// <summary>
        /// read a character from the console output on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="charsWithAttributes">The returned charactes with attributes</param>
        public abstract void ExtractChar(int nline, int ncol, out INCursesChar ch);

        /// <summary>
        /// read a character with all its attributes from the console output at the current position
        /// supports unicode.
        /// doesn't move the cursor
        /// </summary>
        /// <param name="attrs">attributes applied to the character</param>
        /// <param name="pair">pair number applied to the character</param>
        /// <returns>the read character</returns>
        public abstract char ExtractChar(out ulong attrs, out short pair);

        /// <summary>
        /// read a character with all its attributes from the console output on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// supports unicode.
        /// doesn't move the cursor
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="attrs">attributes applied to the character</param>
        /// <param name="pair">pair number applied to the character</param>
        /// <returns>the read character</returns>
        public abstract char ExtractChar(int nline, int ncol, out ulong attrs, out short pair);

        /// <summary>
        /// read a string of atmost <see cref="Constants.MAX_STRING_LENGTH"/> characters or until the right margin from the console output.
        /// </summary>
        /// <returns>the read string</returns>
        public abstract string ExtractString();

        /// <summary>
        /// read a string of atmost <paramref name="maxChars"/> characters.
        /// </summary>
        /// <param name="maxChars">The number of characters to extract</param>
        /// <param name="read">The number of character extracted</param>
        /// <returns>the read string</returns>
        public abstract string ExtractString(int maxChars, out int read);

        /// <summary>
        /// read a string atmost <see cref="Constants.MAX_STRING_LENGTH"/> charactersor until the right margin from the console output from the console output 
        /// on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <returns>the read character</returns>
        public abstract string ExtractString(int nline, int ncol);

        /// <summary>
        /// read a string atmost <see cref="Constants.MAX_STRING_LENGTH"/> charactersor until the right margin from the console output from the console output 
        /// on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="maxChars">The number of characters to extract</param>
        /// <param name="read">The number of character extracted</param>
        /// <returns>the read character</returns>
        public abstract string ExtractString(int nline, int ncol, int maxChars, out int read);

        /// <summary>
        /// read a string with all its attributes of atmost <see cref="Constants.MAX_STRING_LENGTH"/> characters or until the right margin from the console output.
        /// </summary>
        /// <param name="charsWithAttributes">The returned charactes with attributes</param>
        public abstract void ExtractString(out INCursesCharString charsWithAttributes);

        /// <summary>
        /// read a string with all its attributes of atmost paramref name="maxChars"/> characters or until the right margin from the console output.
        /// </summary>
        /// <param name="charsWithAttributes">The returned charactes with attributes</param>
        /// <param name="maxChars">The number of characters to extract</param>
        public abstract void ExtractString(out INCursesCharString charsWithAttributes, int maxChars);

        /// <summary>
        /// read a string with all its attributes of atmost <see cref="Constants.MAX_STRING_LENGTH"/> characters or until the right margin from the console output 
        /// on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="charsWithAttributes">The returned charactes with attributes</param>
        public abstract void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes);

        /// <summary>
        /// read a string with all its attributes of atmost <paramref name="maxChars"/> characters or until the right margin from the console output 
        /// on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="charsWithAttributes">The returned charactes with attributes</param>
        /// <param name="maxChars">The number of characters to extract</param>
        public abstract void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars);
        #endregion

        //TODO: strings
        #region character creation
        /// <summary>
        /// Get a native character representing <paramref name="ch"/>
        /// </summary>
        /// <param name="ch">The character to convert</param>
        /// <returns>The converted cahracter</returns>
        public abstract INCursesChar CreateChar(char ch);

        /// <summary>
        /// Get a native character representing <paramref name="ch"/>
        /// </summary>
        /// <param name="ch">The character to convert</param>
        /// <param name="attrs">attributes applied to the character</param>
        /// <returns>The converted cahracter</returns>
        public abstract INCursesChar CreateChar(char ch, ulong attrs);

        /// <summary>
        /// Get a native character representing <paramref name="ch"/> with attributes/color applied
        /// </summary>
        /// <param name="ch">The character to convert</param>
        /// <param name="attrs">attributes applied to the character</param>
        /// <param name="pair">pair number applied to the character</param>
        /// <returns>The converted cahracter</returns>
        public abstract INCursesChar CreateChar(char ch, ulong attrs, short pair);

        /// <summary>
        /// Get a native string representing <paramref name="str"/>
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <returns>The converted str</returns>
        public abstract INCursesCharString CreateString(string str);

        /// <summary>
        /// Get a native string representing <paramref name="str"/>
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <param name="attrs">attributes applied to the character</param>
        /// <returns>The converted str</returns>
        public abstract INCursesCharString CreateString(string str, ulong attrs);

        /// <summary>
        /// Get a native string representing <paramref name="str"/> with attributes/color applied
        /// </summary>
        /// <param name="str">The string to convert</param>
        /// <param name="attrs">attributes applied to the character</param>
        /// <param name="pair">pair number applied to the character</param>
        /// <returns>The converted cahracter</returns>
        public abstract INCursesCharString CreateString(string str, ulong attrs, short pair);
        #endregion

        #region border
        /// <summary>
        /// Draw a box around the edges of the window
        /// </summary>
        /// <param name="ls">left side</param>
        /// <param name="rs">right side</param>
        /// <param name="ts">top side</param>
        /// <param name="bs">bottom side</param>
        /// <param name="tl">top left-hand corner</param>
        /// <param name="tr">top right-hand corner</param>
        /// <param name="bl">bottom left-hand corner</param>
        /// <param name="br">bottom right-hand corner</param>
        public abstract void Border(in INCursesChar l, in INCursesChar rs, in INCursesChar ts, in INCursesChar bs, in INCursesChar tl, in INCursesChar tr, in INCursesChar bl, in INCursesChar br);

        /// <summary>
        /// Draw a default border around the edges of the window
        /// </summary>
        public abstract void Border();

        /// <summary>
        /// Draw a horizontal line using
        /// </summary>
        /// <param name="lineChar">The character to use for the horizontal line</param>
        /// <param name="length">Lenght of the line with character <paramref name="lineChar"/></param>
        public abstract void HorizontalLine(in INCursesChar lineChar, int length);

        /// <summary>
        /// Draw a horizontal line on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="lineChar">The character to use for the horizontal line</param>
        /// <param name="length">Lenght of the line with character <paramref name="lineChar"/></param>
        public abstract void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length);

        /// <summary>
        /// Draw a vertical line
        /// </summary>
        /// <param name="lineChar">The character to use for the vertical line</param>
        /// <param name="length">Lenght of the line with character <paramref name="lineChar"/></param>
        public abstract void VerticalLine(in INCursesChar lineChar, int length);

        /// <summary>
        /// Draw a vertical line on line <paramref name="nline"/> and column <paramref name="ncol"/>. 
        /// </summary>
        /// <param name="nline">the line number to start writing</param>
        /// <param name="ncol">the column number to start writing</param>
        /// <param name="lineChar">The character to use for the vertical line</param>
        /// <param name="length">Lenght of the line with character <paramref name="lineChar"/></param>
        public abstract void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length);
        #endregion

        /// <summary>
        /// Clear the window
        /// </summary>
        public abstract void Erase();

        /// <summary>
        /// Clear the window without committing to the screen
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Scroll the window <paramref name="lines"/> lines.
        /// </summary>
        /// <param name="lines">the number of lines to scroll, negative to scroll down.</param>
        public abstract void ScrollWindow(int lines);

        /// <summary>
        /// Clear to the end of the line
        /// </summary>
        public abstract void ClearToEol();

        /// <summary>
        /// Clear window to bottom starting from current cursor position
        /// </summary>
        public abstract void ClearToBottom();

        /// <summary>
        /// Refresh the entire screen
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// Refresh the changed portions of the screen, allowing to refresh multiple windows at once
        /// Remember to call <see cref="NCurses.DoUpdate"/> after all calls
        /// </summary>
        public abstract void NoOutRefresh();

        public abstract IWindow SubWindow(int nlines, int ncols, int begin_y, int begin_x);
        public abstract IWindow DerWindow(int nlines, int ncols, int begin_y, int begin_x);

        public abstract IWindow ToSingleByteWindow();
        public abstract IWindow ToMultiByteWindow();

        #region Equality
        public override bool Equals(object obj)
        {
            if (obj is WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window)
            {
                return this.Equals(window);
            }
            return false;
        }

        public bool Equals(IWindow other)
        {
            if(other is WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window)
            {
                return this.Equals(window);
            }
            return false;
        }

        public bool Equals(WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> other)
        {
            if (!(this.WindowBaseSafeHandle is null && other.WindowBaseSafeHandle is null))
            {
                return this.WindowBaseSafeHandle.Equals(other.WindowBaseSafeHandle);
            }
            else
            {
                return object.ReferenceEquals(this, other);
            }
        }

        public override int GetHashCode()
        {
            if (this.WindowBaseSafeHandle is null)
            {
                return base.GetHashCode();
            }

            return this.WindowBaseSafeHandle.GetHashCode();
        }
        #endregion

        #region IDisposable
        public virtual void Dispose()
        {
            if (Windows.Contains(this))
            {
                Windows.Remove(this);
            }

            if (this.HasAddedRefToExistingSafeHandle)
            {
                this.WindowBaseSafeHandle.DangerousRelease();
            }
            else
            {
                this.WindowBaseSafeHandle?.Dispose();
            }
            this.WindowBaseSafeHandle = null;

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
