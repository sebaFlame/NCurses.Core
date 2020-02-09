using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

        internal SingleByteWindow(IntPtr windowPtr, bool ownsHandle = true, bool initizalize = true)
            : base(windowPtr, ownsHandle, initizalize)
        { }

        ///// <summary>
        ///// create a new window
        ///// </summary>
        ///// <param name="nlines">number of lines of the new window</param>
        ///// <param name="ncols">number of columns of the new window</param>
        ///// <param name="begy">line of the upper left corner of the new window</param>
        ///// <param name="begx">column of the upper left corent of the new window</param>
        public SingleByteWindow(int nlines, int ncols, int begy, int begx)
            : base(NativeNCurses.newwin(nlines, ncols, begy, begx))
        {  }

        public SingleByteWindow(int nlines, int ncols)
            : this(nlines, ncols, 0, 0)
        { }

        public SingleByteWindow()
            : this(0, 0, 0, 0)
        { }

        //TODO: same reference????
        private ISingleByteChar VerifyChar(in INCursesChar ch)
        {
            if (!(ch is ISingleByteChar tmp))
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
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar ls);
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar rs);
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar ts);
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar bs);
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar tl);
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar tr);
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar bl);
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar br);

            NativeWindow.wborder(this.WindowPtr, ls, rs, ts, bs, tl, tr, bl, br);
        }

        public override void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar)
        {
            NativeWindow.box(this.WindowPtr, this.VerifyChar(verticalChar), this.VerifyChar(horizontalChar));
        }

        public override void Box()
        {
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar verch);
            SingleByteCharFactory.Instance.GetNativeEmptyChar(out ISingleByteChar horch);

            NativeWindow.box(this.WindowPtr, verch, horch);
        }

        //TODO: use native override?
        public override void CreateChar(char ch, out INCursesChar chRet)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, out ISingleByteChar res);
            chRet = res;
        }

        public override void CreateChar(char ch, ulong attrs, out INCursesChar chRet)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, out ISingleByteChar res);
            chRet = res;
        }

        public override void CreateChar(char ch, ulong attrs, short pair, out INCursesChar chRet)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, pair, out ISingleByteChar res);
            chRet = res;
        }

        public override void CreateString(string str, out INCursesCharString chStr)
        {
            SingleByteCharFactory.Instance.GetNativeString(str, out ISingleByteCharString res);
            chStr = res;
        }

        public override void CreateString(string str, ulong attrs, out INCursesCharString chStr)
        {
            SingleByteCharFactory.Instance.GetNativeString(str, attrs, out ISingleByteCharString res);
            chStr = res;
        }

        public override void CreateString(string str, ulong attrs, short pair, out INCursesCharString chStr)
        {
            SingleByteCharFactory.Instance.GetNativeString(str, attrs, pair, out ISingleByteCharString res);
            chStr = res;
        }

        public override Window Duplicate()
        {
            return new SingleByteWindow(NativeNCurses.dupwin(this.WindowPtr));
        }

        public override char ExtractChar()
        {
            NativeWindow.winch(this.WindowPtr, out ISingleByteChar ch);
            return ch.Char;
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            NativeWindow.winch(this.WindowPtr, out ISingleByteChar sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            NativeWindow.mvwinch(this.WindowPtr, nline, ncol, out ISingleByteChar sch);
            ch = sch;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            NativeWindow.mvwinch(this.WindowPtr, nline, ncol, out ISingleByteChar ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            NativeWindow.winch(this.WindowPtr, out ISingleByteChar ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            NativeWindow.mvwinch(this.WindowPtr, nline, ncol, out ISingleByteChar ch);
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

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            NativeWindow.winchstr(this.WindowPtr, out ISingleByteCharString str, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            NativeWindow.winchnstr(this.WindowPtr, out ISingleByteCharString str, maxChars, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            NativeWindow.mvwinchstr(this.WindowPtr, nline, ncol, out ISingleByteCharString str, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            NativeWindow.mvwinchnstr(this.WindowPtr, nline, ncol, out ISingleByteCharString str, maxChars, out int read);
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
            SingleByteCharFactory.Instance.GetNativeChar(ch, out ISingleByteChar res);
            NativeWindow.winsch(this.WindowPtr, res);
        }

        public override void Insert(in INCursesChar ch)
        {
            if (ch is ISingleByteChar sch)
            {
                NativeWindow.winsch(this.WindowPtr, in sch);
            }
            else
            {
                throw new InvalidOperationException("Unsupported string, try using a SingleByteWindow");
            }
        }

        public override void Insert(int nline, int ncol, in INCursesChar ch)
        {
            if (ch is ISingleByteChar sch)
            {
                NativeWindow.mvwinsch(this.WindowPtr, nline, ncol, in sch);
            }
            else
            {
                throw new InvalidOperationException("Unsupported string, try using a SingleByteWindow");
            }
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, out ISingleByteChar res);
            NativeWindow.mvwinsch(this.WindowPtr, nline, ncol, res);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, pair, out ISingleByteChar res);
            NativeWindow.winsch(this.WindowPtr, res);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, pair, out ISingleByteChar res);
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

        public override void Insert(string str, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeString(str, attrs, pair, out ISingleByteCharString res);

            IEnumerable<ISingleByteChar> schars = res;
            foreach (ISingleByteChar sch in schars.Reverse())
            {
                NativeWindow.winsch(this.WindowPtr, sch);
            }
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
            if (ch is ISingleByteChar schar)
                NativeWindow.waddch(this.WindowPtr, schar);
            else
                throw new InvalidOperationException("Unsupported character");
        }

        public override void Write(in INCursesCharString str)
        {
            if (str is ISingleByteCharString scharStr)
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
            SingleByteCharFactory.Instance.GetNativeString(str, attrs, pair, out ISingleByteCharString res);
            NativeWindow.waddchnstr(this.WindowPtr, res, str.Length);
        }

        public override void Write(int nline, int ncol, string str)
        {
            NativeWindow.mvwaddnstr(this.WindowPtr, nline, ncol, str, str.Length);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeString(str, attrs, pair, out ISingleByteCharString res);
            NativeWindow.mvwaddchnstr(this.WindowPtr, nline, ncol, res, str.Length);
        }

        public override void Write(char ch)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, out ISingleByteChar res);
            NativeWindow.waddch(this.WindowPtr, res);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, pair, out ISingleByteChar res);
            NativeWindow.waddch(this.WindowPtr, res);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, out ISingleByteChar res);
            NativeWindow.mvwaddch(this.WindowPtr, nline, ncol, res);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, pair, out ISingleByteChar res);
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

        public override void Write(int nline, int ncol, in INCursesChar ch)
        {
            if (ch is ISingleByteChar sch)
            {
                NativeWindow.mvwaddch(this.WindowPtr, nline, ncol, in sch);
            }
            else
            {
                throw new InvalidOperationException("Unsupported string, try using a SingleByteWindow");
            }
        }

        public override void Write(int nline, int ncol, in INCursesCharString str)
        {
            if (str is ISingleByteCharString cStr)
            {
                NativeWindow.mvwaddchnstr(this.WindowPtr, nline, ncol, in cStr, cStr.Length);
            }
            else
            {
                throw new InvalidOperationException("Unsupported string, try using a SingleByteWindow");
            }

            throw new NotImplementedException();
        }

        public override void Put(char ch)
        {
            NativeNCurses.ungetch(ch);
        }

        public override void Put(Key key)
        {
            NativeNCurses.ungetch((int)key);
        }
    }
}
