﻿using System;
using System.Text;
using NCurses.Core.Interop;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.MultiByte;

namespace NCurses.Core.StdScr
{
    internal class MultiByteStdScr : StdScrBase
    {
        public override INCursesChar BackGround
        {
            get
            {
                NativeStdScr.getbkgrnd(out INCursesWCHAR wch);
                return wch;
            }
            set
            {
                NativeStdScr.bkgrnd(VerifyChar(value));
            }
        }

        public override INCursesChar InsertBackGround
        {
            get
            {
                NativeStdScr.getbkgrnd(out INCursesWCHAR wch);
                return wch;
            }
            set
            {
                NativeStdScr.bkgrndset(VerifyChar(value));
            }
        }

        public MultiByteStdScr(IntPtr stdScr)
            : base(stdScr) { }

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
            NativeStdScr.border_set(this.VerifyChar(ls), this.VerifyChar(rs), this.VerifyChar(ts), this.VerifyChar(bs),
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

            NativeStdScr.border_set(ls, rs, ts, bs, tl, tr, bl, br);
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

        public override void ExtractChar(out INCursesChar ch)
        {
            NativeStdScr.in_wch(out INCursesWCHAR sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            NativeStdScr.mvin_wch(nline, ncol, out INCursesWCHAR sch);
            ch = sch;
        }

        public override char ExtractChar()
        {
            NativeStdScr.in_wch(out INCursesWCHAR ch);
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            NativeStdScr.mvin_wch(nline, ncol, out INCursesWCHAR ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            NativeStdScr.in_wch(out INCursesWCHAR ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            NativeStdScr.mvin_wch(nline, ncol, out INCursesWCHAR ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override string ExtractString()
        {
            NativeStdScr.inwstr(out string str);
            return str;
        }

        public override string ExtractString(int maxChars, out int read)
        {
            NativeStdScr.innwstr(out string str, maxChars, out read);
            return str;
        }

        public override string ExtractString(int nline, int ncol)
        {
            NativeStdScr.mvinwstr(nline, ncol, out string str);
            return str;
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            NativeStdScr.mvinnwstr(nline, ncol, out string str, maxChars, out read);
            return str;
        }

        public override void ExtractString(out INCursesCharStr charsWithAttributes)
        {
            NativeStdScr.in_wchstr(out INCursesWCHARStr str);
            charsWithAttributes = str;
        }

        public override void ExtractString(out INCursesCharStr charsWithAttributes, int maxChars)
        {
            NativeStdScr.in_wchnstr(out INCursesWCHARStr str, maxChars);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharStr charsWithAttributes)
        {
            NativeStdScr.mvin_wchstr(nline, ncol, out INCursesWCHARStr str);
            charsWithAttributes = str;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharStr charsWithAttributes, int maxChars)
        {
            NativeStdScr.mvin_wchnstr(nline, ncol, out INCursesWCHARStr str, maxChars);
            charsWithAttributes = str;
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            NativeStdScr.hline_set(VerifyChar(lineChar), length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            NativeStdScr.mvhline_set(nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Insert(char ch)
        {
            WideCharFactory.Instance.GetNativeChar(ch, out INCursesWCHAR res);
            NativeStdScr.ins_wch(res);
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            WideCharFactory.Instance.GetNativeChar(ch, out INCursesWCHAR res);
            NativeStdScr.mvins_wch(nline, ncol, res);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesWCHAR res);
            NativeStdScr.ins_wch(res);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesWCHAR res);
            NativeStdScr.mvins_wch(nline, ncol, res);
        }

        public override void Insert(string str)
        {
            NativeStdScr.ins_nwstr(str, str.Length);
        }

        public override void Insert(int nline, int ncol, string str)
        {
            NativeStdScr.mvins_nwstr(nline, ncol, str, str.Length);
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            return NativeStdScr.get_wch(out ch, out key);
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            return NativeStdScr.mvget_wch(nline, ncol, out ch, out key);
        }

        public override string ReadLine()
        {
            NativeStdScr.get_wstr(out string str);
            return str;
        }

        public override string ReadLine(int nline, int ncol)
        {
            NativeStdScr.mvget_wstr(nline, ncol, out string str);
            return str;
        }

        public override string ReadLine(int length)
        {
            NativeStdScr.getn_wstr(out string str, length);
            return str;
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            NativeStdScr.mvgetn_wstr(nline, ncol, out string str, length);
            return str;
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            NativeStdScr.vline_set(VerifyChar(lineChar), length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            NativeStdScr.mvvline_set(nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Write(string str)
        {
            NativeStdScr.addnwstr(str, str.Length);
        }

        public override void Write(string str, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeString(str, attrs, pair, out INCursesWCHARStr res);
            NativeStdScr.add_wchnstr(res, str.Length);
        }

        public override void Write(int nline, int ncol, string str)
        {
            NativeStdScr.mvaddnwstr(nline, ncol, str, str.Length);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeString(str, attrs, pair, out INCursesWCHARStr res);
            NativeStdScr.mvadd_wchnstr(nline, ncol, res, str.Length);
        }

        public override void Write(in INCursesChar ch)
        {
            if (ch is INCursesWCHAR wchar)
                NativeStdScr.add_wch(wchar);
            else if (ch is INCursesSCHAR schar)
                NCurses.SingleByteStdScr.Write(schar);
            else
                throw new InvalidOperationException("Unsupported character");
        }

        public override void Write(in INCursesCharStr str)
        {
            if (str is INCursesWCHARStr wcharStr)
                NativeStdScr.add_wchstr(wcharStr);
            else if (str is INCursesSCHARStr scharStr)
                NCurses.SingleByteStdScr.Write(scharStr);
            else
                throw new InvalidOperationException("Unsupported string");
        }

        //TODO: don't convert?
        public override void Write(char ch)
        {
            NativeStdScr.addnwstr(ch.ToString(), 1);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesWCHAR res);
            NativeStdScr.add_wch(res);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            WideCharFactory.Instance.GetNativeChar(ch, out INCursesWCHAR res);
            NativeStdScr.mvadd_wch(nline, ncol, res);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeChar(ch, attrs, pair, out INCursesWCHAR res);
            NativeStdScr.mvadd_wch(nline, ncol, res);
        }

        public override void Write(byte[] str, Encoding encoding)
        {
            WideCharFactory.Instance.GetNativeString(str, encoding, out INCursesWCHARStr res);
            NativeStdScr.add_wchnstr(res, res.Length);
        }

        public override void Write(byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeString(str, encoding, attrs, pair, out INCursesWCHARStr res);
            NativeStdScr.add_wchnstr(res, res.Length);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding)
        {
            WideCharFactory.Instance.GetNativeString(str, encoding, out INCursesWCHARStr res);
            NativeStdScr.mvadd_wchnstr(nline, ncol, res, res.Length);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            WideCharFactory.Instance.GetNativeString(str, encoding, attrs, pair, out INCursesWCHARStr res);
            NativeStdScr.mvadd_wchnstr(nline, ncol, res, res.Length);
        }
    }
}
