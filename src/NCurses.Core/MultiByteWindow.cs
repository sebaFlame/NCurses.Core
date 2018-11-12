using System;
using System.Text;
using NCurses.Core.Interop;
using NCurses.Core.Interop.MultiByte;

namespace NCurses.Core
{
    internal class MultiByteWindow : Window
    {
        public override INCursesChar BackGround
        {
            get
            {
                NativeWindow.wgetbkgrnd(this.WindowPtr, out INCursesWCHAR wch);
                return wch;
            }
            set
            {
                NativeWindow.wbkgrnd(this.WindowPtr, VerifyChar(value));
            }
        }

        public override INCursesChar InsertBackGround
        {
            get
            {
                NativeWindow.wgetbkgrnd(this.WindowPtr, out INCursesWCHAR wch);
                return wch;
            }
            set
            {
                NativeWindow.wbkgrndset(this.WindowPtr, VerifyChar(value));
            }
        }

        internal MultiByteWindow(IntPtr windowPtr, bool ownsHandle = true, bool initialize = true)
            : base(windowPtr, ownsHandle, initialize)
        { }

        ///// <summary>
        ///// create a new window
        ///// </summary>
        ///// <param name="nlines">number of lines of the new window</param>
        ///// <param name="ncols">number of columns of the new window</param>
        ///// <param name="begy">line of the upper left corner of the new window</param>
        ///// <param name="begx">column of the upper left corent of the new window</param>
        public MultiByteWindow(int nlines, int ncols, int begy, int begx)
            : base
            (
                 NCurses.UnicodeSupported ? NativeNCurses.newwin(nlines, ncols, begy, begx) : throw new NotSupportedException("Unicode not supported")
            )
        {  }

        public MultiByteWindow(int nlines, int ncols)
            : this(nlines, ncols, 0, 0)
        { }

        public MultiByteWindow()
            : this(0, 0, 0, 0)
        { }

        //TODO: same reference????
        private INCursesWCHAR VerifyChar(in INCursesChar ch)
        {
            if (!(ch is INCursesWCHAR tmp))
                throw new InvalidCastException("Character is in incorrect format");
            return tmp;
        }

        public override void Border(in INCursesChar ls, in INCursesChar rs, in INCursesChar ts, in INCursesChar bs,
            in INCursesChar tl, in INCursesChar tr, in INCursesChar bl, in INCursesChar br)
        {
            NativeWindow.wborder_set(this.WindowPtr, this.VerifyChar(ls), this.VerifyChar(rs), this.VerifyChar(ts), this.VerifyChar(bs),
                this.VerifyChar(tl), this.VerifyChar(tr), this.VerifyChar(bl), this.VerifyChar(br));
        }

        public override void Border()
        {
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR ls);
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR rs);
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR ts);
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR bs);
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR tl);
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR tr);
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR bl);
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR br);

            NativeWindow.wborder_set(this.WindowPtr, ls, rs, ts, bs, tl, tr, bl, br);
        }

        public override void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar)
        {
            NativeWindow.box_set(this.WindowPtr, this.VerifyChar(verticalChar), this.VerifyChar(horizontalChar));
        }

        public override void Box()
        {
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR verch);
            WideCharFactory.Instance.GetNativeEmptyChar(out INCursesWCHAR horch);

            NativeWindow.box_set(this.WindowPtr, verch, horch);
        }

        //TODO: use native override?
        public override void CreateChar(char ch, out INCursesChar chRet)
        {
            WideCharFactory.Instance.GetNativeChar(ch, out INCursesWCHAR res);
            chRet = res;
        }

        public override void CreateChar(char ch, ulong attrs, out INCursesChar chRet)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, out INCursesWCHAR res);
            chRet = res;
        }

        public override void CreateChar(char ch, ulong attrs, short pair, out INCursesChar chRet)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesWCHAR res);
            chRet = res;
        }

        public override void CreateString(string str, out INCursesCharStr chStr)
        {
            WideCharFactory.Instance.GetNativeString(str, out INCursesWCHARStr res);
            chStr = res;
        }

        public override void CreateString(string str, ulong attrs, out INCursesCharStr chStr)
        {
            WideCharFactory.Instance.GetNativeString(str, attrs, out INCursesWCHARStr res);
            chStr = res;
        }

        public override void CreateString(string str, ulong attrs, short pair, out INCursesCharStr chStr)
        {
            WideCharFactory.Instance.GetNativeString(str, attrs, pair, out INCursesWCHARStr res);
            chStr = res;
        }

        public override Window Duplicate()
        {
            return new MultiByteWindow(NativeNCurses.dupwin(this.WindowPtr));
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            NativeWindow.win_wch(this.WindowPtr, out INCursesWCHAR sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            NativeWindow.mvwin_wch(this.WindowPtr, nline, ncol, out INCursesWCHAR sch);
            ch = sch;
        }

        public override char ExtractChar()
        {
            NativeWindow.win_wch(this.WindowPtr, out INCursesWCHAR ch);
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            NativeWindow.mvwin_wch(this.WindowPtr, nline, ncol, out INCursesWCHAR ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            NativeWindow.win_wch(this.WindowPtr, out INCursesWCHAR ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            NativeWindow.mvwin_wch(this.WindowPtr, nline, ncol, out INCursesWCHAR ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override string ExtractString()
        {
            NativeWindow.winwstr(this.WindowPtr, out string str);
            return str;
        }

        public override string ExtractString(int maxChars, out int read)
        {
            NativeWindow.winnwstr(this.WindowPtr, out string str, maxChars, out read);
            return str;
        }

        public override string ExtractString(int nline, int ncol)
        {
            NativeWindow.mvwinwstr(this.WindowPtr, nline, ncol, out string str);
            return str;
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            NativeWindow.mvwinnwstr(this.WindowPtr, nline, ncol, out string str, maxChars, out read);
            return str;
        }

        public override void ExtractString(out INCursesCharStr charsWithAttributes)
        {
            NativeWindow.win_wchstr(this.WindowPtr, out INCursesWCHARStr str);
            charsWithAttributes = str;
        }

        public override void ExtractString(out INCursesCharStr charsWithAttributes, int maxChars)
        {
            NativeWindow.win_wchnstr(this.WindowPtr, out INCursesWCHARStr str, maxChars);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharStr charsWithAttributes)
        {
            NativeWindow.mvwin_wchstr(this.WindowPtr, nline, ncol, out INCursesWCHARStr str);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharStr charsWithAttributes, int maxChars)
        {
            NativeWindow.mvwin_wchnstr(this.WindowPtr, nline, ncol, out INCursesWCHARStr str, maxChars);
            charsWithAttributes = str;
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            NativeWindow.whline_set(this.WindowPtr, VerifyChar(lineChar), length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            NativeWindow.mvwhline_set(this.WindowPtr, nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Insert(char ch)
        {
            WideCharFactory.Instance.GetNativeChar(ch, out INCursesWCHAR res);
            NativeWindow.wins_wch(this.WindowPtr, res);
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            WideCharFactory.Instance.GetNativeChar(ch, out INCursesWCHAR res);
            NativeWindow.mvwins_wch(this.WindowPtr, nline, ncol, res);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesWCHAR res);
            NativeWindow.wins_wch(this.WindowPtr, res);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesWCHAR res);
            NativeWindow.mvwins_wch(this.WindowPtr, nline, ncol, res);
        }

        public override void Insert(string str)
        {
            NativeWindow.wins_nwstr(this.WindowPtr, str, str.Length);
        }

        public override void Insert(int nline, int ncol, string str)
        {
            NativeWindow.mvwins_nwstr(this.WindowPtr, nline, ncol, str, str.Length);
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            return NativeWindow.wget_wch(this.WindowPtr, out ch, out key);
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            return NativeWindow.mvwget_wch(this.WindowPtr, nline, ncol, out ch, out key);
        }

        public override string ReadLine()
        {
            NativeWindow.wget_wstr(this.WindowPtr, out string str);
            return str;
        }

        public override string ReadLine(int nline, int ncol)
        {
            NativeWindow.mvwget_wstr(this.WindowPtr, nline, ncol, out string str);
            return str;
        }

        public override string ReadLine(int length)
        {
            NativeWindow.wgetn_wstr(this.WindowPtr, out string str, length);
            return str;
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            NativeWindow.mvwgetn_wstr(this.WindowPtr, nline, ncol, out string str, length);
            return str;
        }

        //TODO: save window
        public override Window ToSingleByteWindow()
        {
            return new SingleByteWindow(this.WindowPtr, false);
        }

        public override Window ToMultiByteWindow()
        {
            return this;
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            NativeWindow.wvline_set(this.WindowPtr, VerifyChar(lineChar), length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            NativeWindow.mvwvline_set(this.WindowPtr, nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Write(in INCursesChar ch)
        {
            if (ch is INCursesWCHAR wchar)
                NativeWindow.wadd_wch(this.WindowPtr, wchar);
            else
                throw new InvalidOperationException("Unsupported character, try using a SingleByteWindow");
        }

        public override void Write(in INCursesCharStr str)
        {
            if (str is INCursesWCHARStr wcharStr)
                NativeWindow.wadd_wchstr(this.WindowPtr, wcharStr);
            else
                throw new InvalidOperationException("Unsupported string, try using a SingleByteWindow");
        }

        public override void Write(string str)
        {
            NativeWindow.waddnwstr(this.WindowPtr, str, str.Length);
        }

        public override void Write(string str, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeString(str, attrs, pair, out INCursesWCHARStr res);
            NativeWindow.wadd_wchnstr(this.WindowPtr, res, str.Length);
        }

        public override void Write(int nline, int ncol, string str)
        {
            NativeWindow.mvwaddnwstr(this.WindowPtr, nline, ncol, str, str.Length);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeString(str, attrs, pair, out INCursesWCHARStr res);
            NativeWindow.mvwadd_wchnstr(this.WindowPtr, nline, ncol, res, str.Length);
        }

        //TODO: don't convert?
        public override void Write(char ch)
        {
            NativeWindow.waddnwstr(this.WindowPtr, ch.ToString(), 1);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesWCHAR res);
            NativeWindow.wadd_wch(this.WindowPtr, res);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            WideCharFactory.Instance.GetNativeChar(ch, out INCursesWCHAR res);
            NativeWindow.mvwadd_wch(this.WindowPtr, nline, ncol, res);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesWCHAR res);
            NativeWindow.mvwadd_wch(this.WindowPtr, nline, ncol, res);
        }

        public override void Write(byte[] str, Encoding encoding)
        {
            WideCharFactory.Instance.GetNativeString(str, encoding, out INCursesWCHARStr res);
            NativeWindow.wadd_wchnstr(this.WindowPtr, res, res.Length);
        }

        public override void Write(byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeString(str, encoding, attrs, pair, out INCursesWCHARStr res);
            NativeWindow.wadd_wchnstr(this.WindowPtr, res, res.Length);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding)
        {
            WideCharFactory.Instance.GetNativeString(str, encoding, out INCursesWCHARStr res);
            NativeWindow.mvwadd_wchnstr(this.WindowPtr, nline, ncol, res, res.Length);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeString(str, encoding, attrs, pair, out INCursesWCHARStr res);
            NativeWindow.mvwadd_wchnstr(this.WindowPtr, nline, ncol, res, res.Length);
        }
    }
}
