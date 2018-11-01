using System;
using System.Text;
using NCurses.Core.Interop;
using NCurses.Core.Interop.Small;

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
        private INCursesSCHAR VerifyChar(in INCursesChar ch)
        {
            if (!(ch is INCursesSCHAR tmp))
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
            NativeStdScr.border(SmallCharFactory.GetSmallChar(), SmallCharFactory.GetSmallChar(), SmallCharFactory.GetSmallChar(), SmallCharFactory.GetSmallChar(),
                SmallCharFactory.GetSmallChar(), SmallCharFactory.GetSmallChar(), SmallCharFactory.GetSmallChar(), SmallCharFactory.GetSmallChar());
        }

        //TODO: use native override?
        public override void CreateChar(char ch, out INCursesChar chRet)
        {
            chRet = SmallCharFactory.GetSmallChar(ch);
        }

        public override void CreateChar(char ch, ulong attrs, out INCursesChar chRet)
        {
            chRet = SmallCharFactory.GetSmallChar(ch, attrs);
        }

        public override void CreateChar(char ch, ulong attrs, short pair, out INCursesChar chRet)
        {
            chRet = SmallCharFactory.GetSmallChar(ch, attrs, pair);
        }

        public override void CreateString(string str, out INCursesCharStr chStr)
        {
            chStr = SmallCharFactory.GetSmallString(str);
        }

        public override void CreateString(string str, ulong attrs, out INCursesCharStr chStr)
        {
            chStr = SmallCharFactory.GetSmallString(str, attrs);
        }

        public override void CreateString(string str, ulong attrs, short pair, out INCursesCharStr chStr)
        {
            chStr = SmallCharFactory.GetSmallString(str, attrs, pair);
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            NativeStdScr.inch(out INCursesSCHAR sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            NativeStdScr.mvinch(nline, ncol, out INCursesSCHAR sch);
            ch = sch;
        }

        public override char ExtractChar()
        {
            NativeStdScr.inch(out INCursesSCHAR ch);
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            NativeStdScr.mvinch(nline, ncol, out INCursesSCHAR ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            NativeStdScr.inch(out INCursesSCHAR ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            NativeStdScr.mvinch(nline, ncol, out INCursesSCHAR ch);
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

        public override void ExtractString(out INCursesCharStr charsWithAttributes)
        {
            NativeStdScr.inchstr(out INCursesSCHARStr str, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(out INCursesCharStr charsWithAttributes, int maxChars)
        {
            NativeStdScr.inchnstr(out INCursesSCHARStr str, maxChars, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharStr charsWithAttributes)
        {
            NativeStdScr.mvinchstr(nline, ncol, out INCursesSCHARStr str, out int read);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharStr charsWithAttributes, int maxChars)
        {
            NativeStdScr.mvinchnstr(nline, ncol, out INCursesSCHARStr str, maxChars, out int read);
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
            NativeStdScr.insch(SmallCharFactory.GetSmallChar(ch));
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            NativeStdScr.mvinsch(nline, ncol, SmallCharFactory.GetSmallChar(ch));
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            NativeStdScr.insch(SmallCharFactory.GetSmallChar(ch, attrs, pair));
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            NativeStdScr.mvinsch(nline, ncol, SmallCharFactory.GetSmallChar(ch, attrs, pair));
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
            if (ch is INCursesSCHAR schar)
                NativeStdScr.addch(schar);
            else
                throw new InvalidOperationException("Unsupported character");
        }

        public override void Write(in INCursesCharStr str)
        {
            if (str is INCursesSCHARStr scharStr)
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
            NativeStdScr.addchnstr(SmallCharFactory.GetSmallString(str, attrs, pair), str.Length);
        }

        public override void Write(int nline, int ncol, string str)
        {
            NativeStdScr.mvaddnstr(nline, ncol, str, str.Length);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            NativeStdScr.mvaddchnstr(nline, ncol, SmallCharFactory.GetSmallString(str, attrs, pair), str.Length);
        }

        public override void Write(char ch)
        {
            NativeStdScr.addnstr(ch.ToString(), 1);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            NativeStdScr.addch(SmallCharFactory.GetSmallChar(ch, attrs, pair));
        }

        public override void Write(int nline, int ncol, char ch)
        {
            NativeStdScr.mvaddch(nline, ncol, SmallCharFactory.GetSmallChar(ch));
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            NativeStdScr.mvaddch(nline, ncol, SmallCharFactory.GetSmallChar(ch, attrs, pair));
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
