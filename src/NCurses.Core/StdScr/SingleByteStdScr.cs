using System;
using System.Text;
using NCurses.Core.Interop;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.StdScr
{
    internal class SingleByteStdScr : StdScrBase
    {
        public override INCursesChar BackGround
        {
            get
            {
                return NativeWindow.getbkgd(this.WindowPtr);
            }
            set
            {
                NativeStdScr.bkgd(VerifyChar(value));
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
                NativeStdScr.bkgdset(VerifyChar(value));
            }
        }

        public SingleByteStdScr(IntPtr stdScr)
            : base(stdScr) { }

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
            NativeStdScr.border(this.VerifyChar(ls), this.VerifyChar(rs), this.VerifyChar(ts), this.VerifyChar(bs), 
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

            NativeStdScr.border(ls, rs, ts, bs, tl, tr, bl, br);
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

        public override void ExtractChar(out INCursesChar ch)
        {
            NativeStdScr.inch(out ISingleByteChar sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            NativeStdScr.mvinch(nline, ncol, out ISingleByteChar sch);
            ch = sch;
        }

        public override char ExtractChar()
        {
            NativeStdScr.inch(out ISingleByteChar ch);
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            NativeStdScr.mvinch(nline, ncol, out ISingleByteChar ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            NativeStdScr.inch(out ISingleByteChar ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            NativeStdScr.mvinch(nline, ncol, out ISingleByteChar ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override string ExtractString()
        {
            NativeStdScr.instr(out string str, out int read);
            return str;
        }

        public override string ExtractString(int maxChars, out int read)
        {
            NativeStdScr.innstr(out string str, maxChars, out read);
            return str;
        }

        public override string ExtractString(int nline, int ncol)
        {
            NativeStdScr.mvinstr(nline, ncol, out string str, out int read);
            return str;
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            NativeStdScr.mvinnstr(nline, ncol, out string str, maxChars, out read);
            return str;
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            NativeStdScr.inchstr(out ISingleByteCharString str, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            NativeStdScr.inchnstr(out ISingleByteCharString str, maxChars, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            NativeStdScr.mvinchstr(nline, ncol, out ISingleByteCharString str, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            NativeStdScr.mvinchnstr(nline, ncol, out ISingleByteCharString str, maxChars, out int read);
            charsWithAttributes = str;
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            NativeStdScr.hline(VerifyChar(lineChar), length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            NativeStdScr.mvhline(nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Insert(char ch)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, out ISingleByteChar res);
            NativeStdScr.insch(res);
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, out ISingleByteChar res);
            NativeStdScr.mvinsch(nline, ncol, res);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, pair, out ISingleByteChar res);
            NativeStdScr.insch(res);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, pair, out ISingleByteChar res);
            NativeStdScr.mvinsch(nline, ncol, res);
        }

        public override void Insert(string str)
        {
            NativeStdScr.insnstr(str, str.Length);
        }

        public override void Insert(int nline, int ncol, string str)
        {
            NativeStdScr.mvinsnstr(nline, ncol, str, str.Length);
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            return NativeStdScr.getch(out ch, out key);
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            return NativeStdScr.mvgetch(nline, ncol, out ch, out key);
        }

        public override string ReadLine()
        {
            NativeStdScr.getstr(out string str);
            return str;
        }

        public override string ReadLine(int nline, int ncol)
        {
            NativeStdScr.mvgetstr(nline, ncol, out string str);
            return str;
        }

        public override string ReadLine(int length)
        {
            NativeStdScr.getnstr(out string str, length);
            return str;
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            NativeStdScr.mvgetnstr(nline, ncol, out string str, length);
            return str;
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            NativeStdScr.vline(VerifyChar(lineChar), length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            NativeStdScr.mvvline(nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Write(in INCursesChar ch)
        {
            if (ch is ISingleByteChar schar)
                NativeStdScr.addch(schar);
            else
                throw new InvalidOperationException("Unsupported character");
        }

        public override void Write(in INCursesCharString str)
        {
            if (str is ISingleByteCharString scharStr)
                NativeStdScr.addchstr(scharStr);
            else
                throw new InvalidOperationException("Unsupported string");
        }

        public override void Write(string str)
        {
            NativeStdScr.addnstr(str, str.Length);
        }

        public override void Write(string str, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeString(str, attrs, pair, out ISingleByteCharString res);
            NativeStdScr.addchnstr(res, str.Length);
        }

        public override void Write(int nline, int ncol, string str)
        {
            NativeStdScr.mvaddnstr(nline, ncol, str, str.Length);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeString(str, attrs, pair, out ISingleByteCharString res);
            NativeStdScr.mvaddchnstr(nline, ncol, res, str.Length);
        }

        public override void Write(char ch)
        {
            NativeStdScr.addnstr(ch.ToString(), 1);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, pair, out ISingleByteChar res);
            NativeStdScr.addch(res);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, out ISingleByteChar res);
            NativeStdScr.mvaddch(nline, ncol, res);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, attrs, pair, out ISingleByteChar res);
            NativeStdScr.mvaddch(nline, ncol, res);
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
