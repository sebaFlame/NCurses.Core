using System;
using System.Text;
using NCurses.Core.Interop;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core
{
    internal class SingleByteWindow : Window
    {
        public override INCursesChar BackGround
        {
            get
            {
                return NativeWindow.getbkgd(this.WindowPtr);
            }
            set
            {
                NativeWindow.wbkgd(this.WindowPtr, VerifyChar(value));
            }
        }

        public override INCursesChar InsertBackGround
        {
            get
            {
                return NativeWindow.getbkgd(this.WindowPtr);
            }
            set
            {
                NativeWindow.wbkgdset(this.WindowPtr, VerifyChar(value));
            }
        }

        internal SingleByteWindow(IntPtr windowPtr, bool ownsHandle = true)
            : base(windowPtr, ownsHandle)
        { }

        ///// <summary>
        ///// create a new window
        ///// </summary>
        ///// <param name="nlines">number of lines of the new window</param>
        ///// <param name="ncols">number of columns of the new window</param>
        ///// <param name="begy">line of the upper left corner of the new window</param>
        ///// <param name="begx">column of the upper left corent of the new window</param>
        public SingleByteWindow(int nlines, int ncols, int begy, int begx)
            : base()
        {
            if (!NCurses.UnicodeSupported)
                throw new NotSupportedException("Unicode not supported");

            DictPtrWindows.Add(this, this.WindowPtr = NativeNCurses.newwin(nlines, ncols, begy, begx));
        }

        public SingleByteWindow(int nlines, int ncols)
            : this(nlines, ncols, 0, 0)
        { }

        public SingleByteWindow()
            : this(0, 0, 0, 0)
        { }

        //TODO: same reference????
        private INCursesSCHAR VerifyChar(in INCursesChar ch)
        {
            if (!(ch is INCursesSCHAR tmp))
                throw new InvalidCastException("Character is in incorrect format");
            return tmp;
        }

        public override void Border(in INCursesChar ls, in INCursesChar rs, in INCursesChar ts, in INCursesChar bs, 
            in INCursesChar tl, in INCursesChar tr, in INCursesChar bl, in INCursesChar br)
        {
            NativeWindow.wborder(this.WindowPtr, this.VerifyChar(ls), this.VerifyChar(rs), this.VerifyChar(ts), this.VerifyChar(bs), 
                this.VerifyChar(tl), this.VerifyChar(tr), this.VerifyChar(bl), this.VerifyChar(br));
        }

        public override void Border()
        {
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR ls);
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR rs);
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR ts);
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR bs);
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR tl);
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR tr);
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR bl);
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR br);

            NativeWindow.wborder(this.WindowPtr, ls, rs, ts, bs, tl, tr, bl, br);
        }

        public override void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar)
        {
            NativeWindow.box(this.WindowPtr, this.VerifyChar(verticalChar), this.VerifyChar(horizontalChar));
        }

        public override void Box()
        {
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR verch);
            SmallCharFactory.Instance.GetNativeEmptyChar(out INCursesSCHAR horch);

            NativeWindow.box(this.WindowPtr, verch, horch);
        }

        //TODO: use native override?
        public override void CreateChar(char ch, out INCursesChar chRet)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, out INCursesSCHAR res);
            chRet = res;
        }

        public override void CreateChar(char ch, ulong attrs, out INCursesChar chRet)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, attrs, out INCursesSCHAR res);
            chRet = res;
        }

        public override void CreateChar(char ch, ulong attrs, short pair, out INCursesChar chRet)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesSCHAR res);
            chRet = res;
        }

        public override void CreateString(string str, out INCursesCharStr chStr)
        {
            SmallCharFactory.Instance.GetNativeString(str, out INCursesSCHARStr res);
            chStr = res;
        }

        public override void CreateString(string str, ulong attrs, out INCursesCharStr chStr)
        {
            SmallCharFactory.Instance.GetNativeString(str, attrs, out INCursesSCHARStr res);
            chStr = res;
        }

        public override void CreateString(string str, ulong attrs, short pair, out INCursesCharStr chStr)
        {
            SmallCharFactory.Instance.GetNativeString(str, attrs, pair, out INCursesSCHARStr res);
            chStr = res;
        }

        public override Window Duplicate()
        {
            return new SingleByteWindow(NativeNCurses.dupwin(this.WindowPtr));
        }

        public override char ExtractChar()
        {
            NativeWindow.winch(this.WindowPtr, out INCursesSCHAR ch);
            return ch.Char;
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            NativeWindow.winch(this.WindowPtr, out INCursesSCHAR sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            NativeWindow.mvwinch(this.WindowPtr, nline, ncol, out INCursesSCHAR sch);
            ch = sch;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            NativeWindow.mvwinch(this.WindowPtr, nline, ncol, out INCursesSCHAR ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            NativeWindow.winch(this.WindowPtr, out INCursesSCHAR ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            NativeWindow.mvwinch(this.WindowPtr, nline, ncol, out INCursesSCHAR ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override string ExtractString()
        {
            NativeWindow.winstr(this.WindowPtr, out string str, out int read);
            return str;
        }

        public override string ExtractString(int maxChars, out int read)
        {
            NativeWindow.winnstr(this.WindowPtr, out string str, maxChars, out read);
            return str;
        }

        public override string ExtractString(int nline, int ncol)
        {
            NativeWindow.mvwinstr(this.WindowPtr, nline, ncol, out string str, out int read);
            return str;
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            NativeWindow.mvwinnstr(this.WindowPtr, nline, ncol, out string str, maxChars, out read);
            return str;
        }

        public override void ExtractString(out INCursesCharStr charsWithAttributes)
        {
            NativeWindow.winchstr(this.WindowPtr, out INCursesSCHARStr str, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(out INCursesCharStr charsWithAttributes, int maxChars)
        {
            NativeWindow.winchnstr(this.WindowPtr, out INCursesSCHARStr str, maxChars, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharStr charsWithAttributes)
        {
            NativeWindow.mvwinchstr(this.WindowPtr, nline, ncol, out INCursesSCHARStr str, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharStr charsWithAttributes, int maxChars)
        {
            NativeWindow.mvwinchnstr(this.WindowPtr, nline, ncol, out INCursesSCHARStr str, maxChars, out int read);
            charsWithAttributes = str;
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            NativeWindow.whline(this.WindowPtr, VerifyChar(lineChar), length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            NativeWindow.mvwhline(this.WindowPtr, nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Insert(char ch)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, out INCursesSCHAR res);
            NativeWindow.winsch(this.WindowPtr, res);
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, out INCursesSCHAR res);
            NativeWindow.mvwinsch(this.WindowPtr, nline, ncol, res);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesSCHAR res);
            NativeWindow.winsch(this.WindowPtr, res);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesSCHAR res);
            NativeWindow.mvwinsch(this.WindowPtr, nline, ncol, res);
        }

        public override void Insert(string str)
        {
            NativeWindow.winsnstr(this.WindowPtr, str, str.Length);
        }

        public override void Insert(int nline, int ncol, string str)
        {
            NativeWindow.mvwinsnstr(this.WindowPtr, nline, ncol, str, str.Length);
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            return NativeWindow.wgetch(this.WindowPtr, out ch, out key);
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            return NativeWindow.mvwgetch(this.WindowPtr, nline, ncol, out ch, out key);
        }

        public override string ReadLine()
        {
            NativeWindow.wgetstr(this.WindowPtr, out string str);
            return str;
        }

        public override string ReadLine(int nline, int ncol)
        {
            NativeWindow.mvwgetstr(this.WindowPtr, nline, ncol, out string str);
            return str;
        }

        public override string ReadLine(int length)
        {
            NativeWindow.wgetnstr(this.WindowPtr, out string str, length);
            return str;
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            NativeWindow.mvwgetnstr(this.WindowPtr, nline, ncol, out string str, length);
            return str;
        }

        public override Window ToSingleByteWindow()
        {
            return this;
        }

        //TODO: save window
        public override Window ToMultiByteWindow()
        {
            if (!NCurses.UnicodeSupported)
                throw new NotSupportedException("Unicode not supported");

            return new MultiByteWindow(this.WindowPtr, false);
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            NativeWindow.wvline(this.WindowPtr, VerifyChar(lineChar), length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            NativeWindow.mvwvline(this.WindowPtr, nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Write(in INCursesChar ch)
        {
            if (ch is INCursesSCHAR schar)
                NativeWindow.waddch(this.WindowPtr, schar);
            else
                throw new InvalidOperationException("Unsupported character");
        }

        public override void Write(in INCursesCharStr str)
        {
            if (str is INCursesSCHARStr scharStr)
                NativeWindow.waddchstr(this.WindowPtr, scharStr);
            else
                throw new InvalidOperationException("Unsupported string");
        }

        public override void Write(string str)
        {
            NativeWindow.waddnstr(this.WindowPtr, str, str.Length);
        }

        public override void Write(string str, ulong attrs, short pair)
        {
            SmallCharFactory.Instance.GetNativeString(str, attrs, pair, out INCursesSCHARStr res);
            NativeWindow.waddchnstr(this.WindowPtr, res, str.Length);
        }

        public override void Write(int nline, int ncol, string str)
        {
            NativeWindow.mvwaddnstr(this.WindowPtr, nline, ncol, str, str.Length);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            SmallCharFactory.Instance.GetNativeString(str, attrs, pair, out INCursesSCHARStr res);
            NativeWindow.mvwaddchnstr(this.WindowPtr, nline, ncol, res, str.Length);
        }

        public override void Write(char ch)
        {
            NativeWindow.waddnstr(this.WindowPtr, ch.ToString(), 1);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesSCHAR res);
            NativeWindow.waddch(this.WindowPtr, res);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, out INCursesSCHAR res);
            NativeWindow.mvwaddch(this.WindowPtr, nline, ncol, res);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            SmallCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesSCHAR res);
            NativeWindow.mvwaddch(this.WindowPtr, nline, ncol, res);
        }

        public override void Write(byte[] str, Encoding encoding)
        {
            throw new NotImplementedException("Only useful in multibyte mode");
        }

        public override void Write(byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            throw new NotImplementedException("Only useful in multibyte mode");
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding)
        {
            throw new NotImplementedException("Only useful in multibyte mode");
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            throw new NotImplementedException("Only useful in multibyte mode");
        }
    }
}
