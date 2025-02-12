﻿using System;
using System.Text;
using System.Buffers;

using NippyWard.NCurses.Interop;
using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.SafeHandles;
using NippyWard.NCurses.Interop.Wrappers;

namespace NippyWard.NCurses.Pad
{
    public abstract class PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>, IPad
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        public override INCursesChar BackGround { get => this.WrappedWindow.BackGround; set => this.WrappedWindow.BackGround = value; }
        public override INCursesChar InsertBackGround { get => this.WrappedWindow.InsertBackGround; set => this.WrappedWindow.InsertBackGround = value; }

        protected WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> WrappedWindow { get; private set; }

        private PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> ParentPad;

        private bool CanDisposeWindow;

        internal PadBase(WindowBaseSafeHandle windowBaseSafeHandle)
            : base(windowBaseSafeHandle)
        {
            this.WrappedWindow = this.CreateWindow(this);
            this.CanDisposeWindow = true;
        }

        internal PadBase(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window)
            : base(window)
        {
            this.WrappedWindow = window;
        }

        /// <summary>
        /// create a subwindow with the current pad as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        public WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> SubPad(int nlines, int ncols, int begin_y, int begin_x)
        {
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> subWindow = 
                this.CreateWindow(NCurses.subpad(this.WindowBaseSafeHandle, nlines, ncols, begin_y, begin_x), this);

            this.SubWindows.Add(subWindow);

            return subWindow;
        }

        IWindow IPad.SubPad(int nlines, int ncols, int begin_y, int begin_x)
            => this.SubPad(nlines, ncols, begin_y, begin_x);

        #region NoOutRefresh
        /// <summary>
        /// efficient non-instant pad update. follow up with <see cref="NCurses.Update"/> to render to the console.
        /// the area to render gets computed from the screen rectangle
        /// </summary>
        /// <param name="pminrow">the line of the top left corner to refresh</param>
        /// <param name="pmincol">the column of the top left corner to refresh</param>
        /// <param name="sminrow">the line of the top left corner of the screen to start the refresh (0)</param>
        /// <param name="smincol">the column of the top left corner of the screen to start the refresh (0)</param>
        /// <param name="smaxrow">the line of the bottom right corner of the screen to end the refresh (MaxLine - 1)</param>
        /// <param name="smaxcol">the column of the bottom right corner of the screen to end the refresh (MaxColumn - 1)</param>
        public void NoOutRefresh(int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            Pad.pnoutrefresh(this.WindowBaseSafeHandle, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol);
        }

        /// <summary>
        /// not thread-safe
        /// see <see cref="NoOutRefresh(int, int, int, int, int, int)"/>
        /// </summary>
        public override void NoOutRefresh()
        {
            this.NoOutRefresh(0, 0, 0, 0, NippyWard.NCurses.NCurses.StdScr.MaxLine - 1, NippyWard.NCurses.NCurses.StdScr.MaxColumn - 1);
        }
        #endregion

        #region Refresh
        /// <summary>
        /// refresh the pad. the area to render gets computed from the screen rectangle
        /// </summary>
        /// <param name="pminrow">the line of the top left corner to refresh</param>
        /// <param name="pmincol">the column of the top left corner to refresh</param>
        /// <param name="sminrow">the line of the top left corner of the screen to start the refresh (0)</param>
        /// <param name="smincol">the column of the top left corner of the screen to start the refresh (0)</param>
        /// <param name="smaxrow">the line of the bottom right corner of the screen to end the refresh (MaxLine - 1)</param>
        /// <param name="smaxcol">the column of the bottom right corner of the screen to end the refresh (MaxColumn - 1)</param>
        public void Refresh(int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol)
        {
            Pad.prefresh(this.WindowBaseSafeHandle, pminrow, pmincol, sminrow, smincol, smaxrow, smaxcol);
        }

        /// <summary>
        /// not thread-safe
        /// see <see cref="Refresh(int, int, int, int, int, int)"/>
        /// </summary>
        public override void Refresh()
        {
            this.Refresh(0, 0, 0, 0, NippyWard.NCurses.NCurses.StdScr.MaxLine - 1, NippyWard.NCurses.NCurses.StdScr.MaxColumn - 1);
        }
        #endregion

        #region echo
        public abstract void Echo(char ch);
        #endregion

        public override void Put(char ch)
        {
            this.WrappedWindow.Put(ch);
        }

        public override void Put(Key key)
        {
            this.WrappedWindow.Put(key);
        }

        public override void Write(int nline, int ncol, in INCursesChar ch)
        {
            this.WrappedWindow.Write(nline, ncol, in ch);
        }

        public override void Write(int nline, int ncol, in INCursesCharString str)
        {
            this.WrappedWindow.Write(nline, ncol, in str);
        }

        public override void Insert(in INCursesChar ch)
        {
            this.WrappedWindow.Insert(in ch);
        }

        public override void Insert(int nline, int ncol, in INCursesChar ch)
        {
            this.WrappedWindow.Insert(nline, ncol, in ch);
        }

        public override void Insert(string str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Insert(str, attrs, pair);
        }

        public override void Insert(ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Insert(str, attrs, pair);
        }

        public override void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar)
        {
            this.WrappedWindow.Box(verticalChar, horizontalChar);
        }

        public override void Box()
        {
            this.WrappedWindow.Box();
        }

        public override void Write(in INCursesChar ch)
        {
            this.WrappedWindow.Write(ch);
        }

        public override void Write(in INCursesCharString str)
        {
            this.WrappedWindow.Write(str);
        }

        public override void Write(string str)
        {
            this.WrappedWindow.Write(str);
        }

        public override void Write(ReadOnlySpan<char> str)
        {
            this.WrappedWindow.Write(str);
        }

        public override void Write(string str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(str, attrs, pair);
        }

        public override void Write(ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(str, attrs, pair);
        }

        public override void Write(int nline, int ncol, string str)
        {
            this.WrappedWindow.Write(nline, ncol, str);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<char> str)
        {
            this.WrappedWindow.Write(nline, ncol, str);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(nline, ncol, str, attrs, pair);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(nline, ncol, str, attrs, pair);
        }

        public override void Write(char ch)
        {
            this.WrappedWindow.Write(ch);
        }

        public override void Write(char ch, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(ch, attrs, pair);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            this.WrappedWindow.Write(nline, ncol, ch);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(nline, ncol, ch, attrs, pair);
        }

        public override void Write(ReadOnlySpan<byte> str, Encoding encoding)
        {
            this.WrappedWindow.Write(str, encoding);
        }

        public override void Write(in ReadOnlySequence<byte> str, Encoding encoding)
        {
            this.WrappedWindow.Write(in str, encoding);
        }

        public override void Write(ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(str, encoding, attrs, pair);
        }

        public override void Write(in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(str, encoding, attrs, pair);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str, Encoding encoding)
        {
            this.WrappedWindow.Write(nline, ncol, str, encoding);
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str, Encoding encoding)
        {
            this.WrappedWindow.Write(nline, ncol, in str, encoding);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(nline, ncol, str, encoding, attrs, pair);
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(nline, ncol, in str, encoding, attrs, pair);
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            return this.WrappedWindow.ReadKey(out ch, out key);
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            return this.WrappedWindow.ReadKey(nline, ncol, out ch, out key);
        }

        public override string ReadLine()
        {
            return this.WrappedWindow.ReadLine();
        }

        public override string ReadLine(int nline, int ncol)
        {
            return this.WrappedWindow.ReadLine(nline, ncol);
        }

        public override string ReadLine(int length)
        {
            return this.WrappedWindow.ReadLine(length);
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            return this.WrappedWindow.ReadLine(nline, ncol, length);
        }

        public override void Insert(char ch)
        {
            this.WrappedWindow.Insert(ch);
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            this.WrappedWindow.Insert(nline, ncol, ch);
        }

        public override void Insert(char ch, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Insert(ch, attrs, pair);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Insert(nline, ncol, ch, attrs, pair);
        }

        public override void Insert(string str)
        {
            this.WrappedWindow.Insert(str);
        }

        public override void Insert(ReadOnlySpan<char> str)
        {
            this.WrappedWindow.Insert(str);
        }

        public override void Insert(int nline, int ncol, string str)
        {
            this.WrappedWindow.Insert(nline, ncol, str);
        }

        public override void Insert(int nline, int ncol, ReadOnlySpan<char> str)
        {
            this.WrappedWindow.Insert(nline, ncol, str);
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            this.WrappedWindow.ExtractChar(out ch);
        }

        public override char ExtractChar()
        {
            return this.WrappedWindow.ExtractChar();
        }

        public override char ExtractChar(INCursesChar @char)
        {
            return this.WrappedWindow.ExtractChar(@char);
        }

        public override char ExtractChar(int nline, int ncol)
        {
            return this.WrappedWindow.ExtractChar(nline, ncol);
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            this.WrappedWindow.ExtractChar(nline, ncol, out ch);
        }

        public override char ExtractChar(out ulong attrs, out ushort pair)
        {
            return this.WrappedWindow.ExtractChar(out attrs, out pair);
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out ushort pair)
        {
            return this.WrappedWindow.ExtractChar(nline, ncol, out attrs, out pair);
        }

        public override string ExtractString()
        {
            return this.WrappedWindow.ExtractString();
        }

        public override string ExtractString(int maxChars, out int read)
        {
            return this.WrappedWindow.ExtractString(maxChars, out read);
        }

        public override string ExtractString(int nline, int ncol)
        {
            return this.WrappedWindow.ExtractString(nline, ncol);
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            return this.WrappedWindow.ExtractString(nline, ncol, maxChars, out read);
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            this.WrappedWindow.ExtractString(out charsWithAttributes);
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            this.WrappedWindow.ExtractString(out charsWithAttributes, maxChars);
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            this.WrappedWindow.ExtractString(nline, ncol, out charsWithAttributes);
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            this.WrappedWindow.ExtractString(nline, ncol, out charsWithAttributes, maxChars);
        }

        public override IChar CreateChar(char ch)
        {
            return this.WrappedWindow.CreateChar(ch);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs)
        {
            return this.WrappedWindow.CreateChar(ch, attrs);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs, ushort pair)
        {
            return this.WrappedWindow.CreateChar(ch, attrs, pair);
        }

        public override ICharString CreateString(string str)
        {
            return this.WrappedWindow.CreateString(str);
        }

        public override ICharString CreateString(ReadOnlySpan<char> str)
        {
            return this.WrappedWindow.CreateString(str);
        }

        public override INCursesCharString CreateString(string str, ulong attrs)
        {
            return this.WrappedWindow.CreateString(str, attrs);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<char> str, ulong attrs)
        {
            return this.WrappedWindow.CreateString(str, attrs);
        }

        public override INCursesCharString CreateString(string str, ulong attrs, ushort pair)
        {
            return this.WrappedWindow.CreateString(str, attrs, pair);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            return this.WrappedWindow.CreateString(str, attrs, pair);
        }

        public override void Border(in INCursesChar l, in INCursesChar rs, in INCursesChar ts, in INCursesChar bs, in INCursesChar tl, in INCursesChar tr, in INCursesChar bl, in INCursesChar br)
        {
            this.WrappedWindow.Border(l, rs, ts, bs, tl, tr, bl, br);
        }

        public override void Border()
        {
            this.WrappedWindow.Border();
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            this.WrappedWindow.HorizontalLine(lineChar, length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            this.WrappedWindow.HorizontalLine(nline, ncol, lineChar, length);
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            this.WrappedWindow.VerticalLine(lineChar, length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            this.WrappedWindow.VerticalLine(nline, ncol, lineChar, length);
        }

        public override void Write(string str, int maxLength)
        {
            this.WrappedWindow.Write(str, maxLength);
        }

        public override void Write(ReadOnlySpan<char> str, int maxLength)
        {
            this.WrappedWindow.Write(str, maxLength);
        }

        public override void Write(ReadOnlySpan<byte> str, int maxLength, Encoding encoding)
        {
            this.WrappedWindow.Write(str, maxLength, encoding);
        }

        public override void Write(in ReadOnlySequence<byte> str, int maxLength, Encoding encoding)
        {
            this.WrappedWindow.Write(in str, maxLength, encoding);
        }

        public override void Write(ReadOnlySpan<byte> str)
        {
            this.WrappedWindow.Write(str);
        }

        public override void Write(in ReadOnlySequence<byte> str)
        {
            this.WrappedWindow.Write(in str);
        }

        public override void Write(ReadOnlySpan<byte> str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(str, attrs, pair);
        }

        public override void Write(in ReadOnlySequence<byte> str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(in str, attrs, pair);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str)
        {
            this.WrappedWindow.Write(nline, ncol, str);
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str)
        {
            this.WrappedWindow.Write(nline, ncol, in str);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(nline, ncol, str, attrs, pair);
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str, ulong attrs, ushort pair)
        {
            this.WrappedWindow.Write(nline, ncol, in str, attrs, pair);
        }

        public override ICharString CreateString(ReadOnlySpan<byte> str)
        {
            return this.WrappedWindow.CreateString(str);
        }

        public override ICharString CreateString(in ReadOnlySequence<byte> str)
        {
            return this.WrappedWindow.CreateString(in str);
        }

        public override ICharString CreateString(in ReadOnlySequence<byte> str, Encoding encoding)
        {
            return this.WrappedWindow.CreateString(in str);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<byte> str, ulong attrs)
        {
            return this.WrappedWindow.CreateString(str, attrs);
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, ulong attrs)
        {
            return this.WrappedWindow.CreateString(in str, attrs);
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs)
        {
            return this.WrappedWindow.CreateString(in str, encoding, attrs);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<byte> str, ulong attrs, ushort pair)
        {
            return this.WrappedWindow.CreateString(str, attrs, pair);
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, ulong attrs, ushort pair)
        {
            return this.WrappedWindow.CreateString(in str, attrs, pair);
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            return this.WrappedWindow.CreateString(in str, encoding, attrs, pair);
        }

        public override void Write(ReadOnlySpan<byte> str, int maxLength)
        {
            this.WrappedWindow.Write(str, maxLength);
        }

        public override void Write(in ReadOnlySequence<byte> str, int maxLength)
        {
            this.WrappedWindow.Write(in str, maxLength);
        }

        public override void Dispose()
        {
            if (this.CanDisposeWindow)
            {
                this.WrappedWindow?.Dispose();
            }

            base.Dispose();

            this.WrappedWindow = null;
        }
    }
}
