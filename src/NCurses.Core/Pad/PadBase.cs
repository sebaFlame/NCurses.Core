using System;
using System.Text;
using System.Collections.Generic;

using NCurses.Core.Interop;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Wrappers;

namespace NCurses.Core.Pad
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

        private HashSet<PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>> SubPads
            = new HashSet<PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>>();
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

        internal PadBase(
            WindowBaseSafeHandle windowBaseSafeHandle,
            PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentPad)
            : this(windowBaseSafeHandle)
        {
            this.ParentPad = parentPad;
        }

        internal void RemoveSubPad(PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> childPad)
        {
            this.SubPads.Remove(childPad);
        }

        /// <summary>
        /// create a subwindow with the current pad as parent
        /// </summary>
        /// <returns>the new subwindow</returns>
        public PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> SubPad(int nlines, int ncols, int begin_y, int begin_x)
        {
            PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> subPad = 
                this.CreatePad(NCurses.subpad(this.WindowBaseSafeHandle, nlines, ncols, begin_y, begin_x), this);
            this.SubPads.Add(subPad);
            return subPad;
        }

        IPad IPad.SubPad(int nlines, int ncols, int begin_y, int begin_x)
            => this.SubPad(nlines, ncols, begin_y, begin_x);

        internal abstract PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreatePad(
            WindowBaseSafeHandle windowBaseSafeHandle,
            PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentPad);

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
            this.NoOutRefresh(0, 0, 0, 0, Core.NCurses.StdScr.MaxLine - 1, Core.NCurses.StdScr.MaxColumn - 1);
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
            this.Refresh(0, 0, 0, 0, Core.NCurses.StdScr.MaxLine - 1, Core.NCurses.StdScr.MaxColumn - 1);
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

        public override void Insert(string str, ulong attrs, short pair)
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

        public override void Write(string str, ulong attrs, short pair)
        {
            this.WrappedWindow.Write(str, attrs, pair);
        }

        public override void Write(int nline, int ncol, string str)
        {
            this.WrappedWindow.Write(nline, ncol, str);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            this.WrappedWindow.Write(nline, ncol, str, attrs, pair);
        }

        public override void Write(char ch)
        {
            this.WrappedWindow.Write(ch);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            this.WrappedWindow.Write(ch, attrs, pair);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            this.WrappedWindow.Write(nline, ncol, ch);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            this.WrappedWindow.Write(nline, ncol, ch, attrs, pair);
        }

        public override void Write(byte[] str, Encoding encoding)
        {
            this.WrappedWindow.Write(str, encoding);
        }

        public override void Write(byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            this.WrappedWindow.Write(str, encoding, attrs, pair);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding)
        {
            this.WrappedWindow.Write(nline, ncol, str, encoding);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            this.WrappedWindow.Write(nline, ncol, str, encoding, attrs, pair);
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

        public override void Insert(char ch, ulong attrs, short pair)
        {
            this.WrappedWindow.Insert(ch, attrs, pair);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            this.WrappedWindow.Insert(nline, ncol, ch, attrs, pair);
        }

        public override void Insert(string str)
        {
            this.WrappedWindow.Insert(str);
        }

        public override void Insert(int nline, int ncol, string str)
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

        public override char ExtractChar(int nline, int ncol)
        {
            return this.WrappedWindow.ExtractChar(nline, ncol);
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            this.WrappedWindow.ExtractChar(nline, ncol, out ch);
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            return this.WrappedWindow.ExtractChar(out attrs, out pair);
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
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

        public override INCursesChar CreateChar(char ch)
        {
            return this.WrappedWindow.CreateChar(ch);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs)
        {
            return this.WrappedWindow.CreateChar(ch, attrs);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs, short pair)
        {
            return this.WrappedWindow.CreateChar(ch, attrs, pair);
        }

        public override INCursesCharString CreateString(string str)
        {
            return this.WrappedWindow.CreateString(str);
        }

        public override INCursesCharString CreateString(string str, ulong attrs)
        {
            return this.WrappedWindow.CreateString(str, attrs);
        }

        public override INCursesCharString CreateString(string str, ulong attrs, short pair)
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

        public override void Dispose()
        {
            this.ParentPad?.RemoveSubPad(this);

            if (this.SubPads.Count > 0)
            {
                throw new InvalidOperationException("Subwindows need to be disposed first");
            }

            if (this.CanDisposeWindow)
            {
                this.WrappedWindow.Dispose();
            }

            base.Dispose();

            this.WrappedWindow = null;
        }
    }
}
