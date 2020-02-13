using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using NCurses.Core.Window;
using NCurses.Core.Interop;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Wrappers;

namespace NCurses.Core.StdScr
{
    internal class MultiByteStdScr<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : StdScrBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TChar : unmanaged, IChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        public override INCursesChar BackGround
        {
            get
            {
                StdScr.getbkgrnd(out TMultiByte wch);
                return wch;
            }
            set
            {
                StdScr.bkgrnd(VerifyChar(value));
            }
        }

        public override INCursesChar InsertBackGround
        {
            get
            {
                StdScr.getbkgrnd(out TMultiByte wch);
                return wch;
            }
            set
            {
                StdScr.bkgrndset(VerifyChar(value));
            }
        }

        public override bool HasUnicodeSupport => true;

        public MultiByteStdScr(WindowBaseSafeHandle stdScr)
            : base(stdScr) { }

        public MultiByteStdScr(StdScrBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> stdScr)
            : base(stdScr) { }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowBaseSafeHandle windowBaseSafeHandle,
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow)
        {
            return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle, parentWindow);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
        {
            throw new NotSupportedException("Can not create a new StdScr from an existing window");
        }

        private TMultiByte VerifyChar(in INCursesChar ch)
        {
            if (!(ch is TMultiByte tmp))
            {
                throw new InvalidCastException("Character is in incorrect format");
            }

            return tmp;
        }

        private MultiByteCharString<TMultiByte> VerifyString(in INCursesCharString str)
        {
            if (!(str is MultiByteCharString<TMultiByte> tmp))
            {
                throw new InvalidCastException("String is in incorrect format");
            }

            return tmp;
        }

        public override void Border(in INCursesChar ls, in INCursesChar rs, in INCursesChar ts, in INCursesChar bs,
            in INCursesChar tl, in INCursesChar tr, in INCursesChar bl, in INCursesChar br)
        {
            StdScr.border_set(this.VerifyChar(ls), this.VerifyChar(rs), this.VerifyChar(ts), this.VerifyChar(bs),
                this.VerifyChar(tl), this.VerifyChar(tr), this.VerifyChar(bl), this.VerifyChar(br));
        }

        public override void Border()
        {
            TMultiByte ls = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            TMultiByte rs = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            TMultiByte ts = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            TMultiByte bs = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            TMultiByte tl = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            TMultiByte tr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            TMultiByte bl = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            TMultiByte br = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();

            StdScr.border_set(in ls, in rs, in ts, in bs, in tl, in tr, in bl, in br);
        }

        public override void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar)
        {
            Window.box_set(this.WindowBaseSafeHandle, this.VerifyChar(verticalChar), this.VerifyChar(horizontalChar));
        }

        public override void Box()
        {
            TMultiByte verch = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            TMultiByte horch = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();

            Window.box_set(this.WindowBaseSafeHandle, in verch, in horch);
        }

        //TODO: use native override?
        public override INCursesChar CreateChar(char ch)
        {
            return MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs)
        {
            return MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs, short pair)
        {
            return MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
        }

        public override INCursesCharString CreateString(string str)
        {
            byte[] buffer = new byte[MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, false)];
            return MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, str);
        }

        public override INCursesCharString CreateString(string str, ulong attrs)
        {
            byte[] buffer = new byte[MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, false)];
            return MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, str, attrs);
        }

        public override INCursesCharString CreateString(string str, ulong attrs, short pair)
        {
            byte[] buffer = new byte[MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, false)];
            return MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, str, attrs, pair);
        }

        public override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Duplicate()
        {
            return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(NCurses.dupwin(this.WindowBaseSafeHandle));
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            StdScr.in_wch(out TMultiByte sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            StdScr.mvin_wch(nline, ncol, out TMultiByte sch);
            ch = sch;
        }

        public override char ExtractChar()
        {
            StdScr.in_wch(out TMultiByte ch);
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            StdScr.mvin_wch(nline, ncol, out TMultiByte ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            StdScr.in_wch(out TMultiByte ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            StdScr.mvin_wch(nline, ncol, out TMultiByte ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override string ExtractString()
        {
            unsafe
            {
                int bufferLength = Constants.MAX_STRING_LENGTH * WideCharFactoryInternal<TWideChar>.Instance.GetCharLength();
                byte* buffer = stackalloc byte[bufferLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength);
                StdScr.inwstr(ref wChStr);
                return wChStr.ToString();
            }
        }

        public override string ExtractString(int maxChars, out int read)
        {
            unsafe
            {
                int bufferLength = maxChars * WideCharFactoryInternal<TWideChar>.Instance.GetCharLength();
                byte* buffer = stackalloc byte[bufferLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength);
                StdScr.innwstr(ref wChStr, maxChars, out read);
                return wChStr.ToString();
            }
        }

        public override string ExtractString(int nline, int ncol)
        {
            unsafe
            {
                int bufferLength = Constants.MAX_STRING_LENGTH * WideCharFactoryInternal<TWideChar>.Instance.GetCharLength();
                byte* buffer = stackalloc byte[bufferLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength);
                StdScr.mvinwstr(nline, ncol, ref wChStr);
                return wChStr.ToString();
            }
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            unsafe
            {
                int bufferLength = maxChars * WideCharFactoryInternal<TWideChar>.Instance.GetCharLength();
                byte* buffer = stackalloc byte[bufferLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength);
                StdScr.mvinnwstr(nline, ncol, ref wChStr, maxChars, out read);
                return wChStr.ToString();
            }
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            int bufferLength = Constants.MAX_STRING_LENGTH * MultiByteCharFactoryInternal<TMultiByte>.Instance.GetCharLength();
            byte[] buffer = new byte[bufferLength];
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyStringInternal(buffer);
            StdScr.in_wchstr(ref wChStr);
            charsWithAttributes = wChStr;
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            int bufferLength = maxChars * MultiByteCharFactoryInternal<TMultiByte>.Instance.GetCharLength();
            byte[] buffer = new byte[bufferLength];
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyStringInternal(buffer);
            StdScr.in_wchnstr(ref wChStr, maxChars);
            charsWithAttributes = wChStr;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            int bufferLength = Constants.MAX_STRING_LENGTH * MultiByteCharFactoryInternal<TMultiByte>.Instance.GetCharLength();
            byte[] buffer = new byte[bufferLength];
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyStringInternal(buffer);
            StdScr.mvin_wchstr(nline, ncol, ref wChStr);
            charsWithAttributes = wChStr;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            int bufferLength = maxChars * MultiByteCharFactoryInternal<TMultiByte>.Instance.GetCharLength();
            byte[] buffer = new byte[bufferLength];
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyStringInternal(buffer);
            StdScr.mvin_wchnstr(nline, ncol, ref wChStr, maxChars);
            charsWithAttributes = wChStr;
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            StdScr.hline_set(VerifyChar(lineChar), length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            StdScr.mvhline_set(nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Insert(char ch)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
            StdScr.ins_wch(in wCh);
        }

        public override void Insert(in INCursesChar ch)
        {
            StdScr.ins_wch(VerifyChar(ch));
        }

        public override void Insert(int nline, int ncol, in INCursesChar ch)
        {
            StdScr.mvins_wch(nline, ncol, VerifyChar(ch));
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
            StdScr.mvins_wch(nline, ncol, in wCh);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            StdScr.ins_wch(in wCh);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            StdScr.mvins_wch(nline, ncol, in wCh);
        }

        public override void Insert(string str)
        {
            unsafe
            {
                int byteLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(str);
                byte* buffer = stackalloc byte[byteLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeStringInternal(buffer, byteLength, str);
                StdScr.ins_nwstr(in wChStr, wChStr.Length);
            }
        }

        public override void Insert(int nline, int ncol, string str)
        {
            unsafe
            {
                int byteLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(str);
                byte* buffer = stackalloc byte[byteLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeStringInternal(buffer, byteLength, str);
                StdScr.mvins_nwstr(nline, ncol, in wChStr, wChStr.Length);
            }
        }

        public override void Insert(string str, ulong attrs, short pair)
        {
            unsafe
            {
                int byteLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str);
                byte* buffer = stackalloc byte[byteLength];
                MultiByteCharString<TMultiByte> mbStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, byteLength, str, attrs, pair);

                IEnumerable<IMultiByteChar> wchars = mbStr;
                foreach (IMultiByteChar wch in wchars.Reverse())
                {
                    StdScr.ins_wch(VerifyChar(wch));
                }
            }
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            bool ret = StdScr.get_wch(out TWideChar wch, out key);
            ch = wch.Char;
            return ret;
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            bool ret = StdScr.mvget_wch(nline, ncol, out TWideChar wch, out key);
            ch = wch.Char;
            return ret;
        }

        public override string ReadLine()
        {
            unsafe
            {
                int bufferLength = Constants.MAX_STRING_LENGTH * WideCharFactoryInternal<TWideChar>.Instance.GetCharLength();
                byte* buffer = stackalloc byte[bufferLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength);
                StdScr.get_wstr(ref wChStr);
                return wChStr.ToString();
            }
        }

        public override string ReadLine(int nline, int ncol)
        {
            unsafe
            {
                int bufferLength = Constants.MAX_STRING_LENGTH * WideCharFactoryInternal<TWideChar>.Instance.GetCharLength();
                byte* buffer = stackalloc byte[bufferLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength);
                StdScr.mvget_wstr(nline, ncol, ref wChStr);
                return wChStr.ToString();
            }
        }

        public override string ReadLine(int length)
        {
            unsafe
            {
                int bufferLength = length * WideCharFactoryInternal<TWideChar>.Instance.GetCharLength();
                byte* buffer = stackalloc byte[bufferLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength);
                StdScr.getn_wstr(ref wChStr, length);
                return wChStr.ToString();
            }
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            unsafe
            {
                int bufferLength = length * WideCharFactoryInternal<TWideChar>.Instance.GetCharLength();
                byte* buffer = stackalloc byte[bufferLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength);
                StdScr.mvgetn_wstr(nline, ncol, ref wChStr, length);
                return wChStr.ToString();
            }
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            StdScr.vline_set(VerifyChar(lineChar), length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            StdScr.mvvline_set(nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Write(in INCursesChar ch)
        {
            StdScr.add_wch(VerifyChar(ch));
        }

        public override void Write(in INCursesCharString str)
        {
            StdScr.add_wchstr(VerifyString(str));
        }

        public override void Write(string str)
        {
            unsafe
            {
                int byteLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(str);
                byte* buffer = stackalloc byte[byteLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeStringInternal(buffer, byteLength, str);
                StdScr.addnwstr(in wChStr, wChStr.Length);
            }
        }

        public override void Write(string str, ulong attrs, short pair)
        {
            unsafe
            {
                int byteLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str);
                byte* buffer = stackalloc byte[byteLength];
                MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, byteLength, str, attrs, pair);
                StdScr.add_wchnstr(in wChStr, wChStr.Length);
            }
        }

        public override void Write(int nline, int ncol, string str)
        {
            unsafe
            {
                int byteLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(str);
                byte* buffer = stackalloc byte[byteLength];
                WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeStringInternal(buffer, byteLength, str);
                StdScr.mvaddnwstr(nline, ncol, in wChStr, wChStr.Length);
            }
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            unsafe
            {
                int byteLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str);
                byte* buffer = stackalloc byte[byteLength];
                MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, byteLength, str, attrs, pair);
                StdScr.mvadd_wchnstr(nline, ncol, in wChStr, wChStr.Length);
            }
        }

        //TODO: don't convert?
        public override void Write(char ch)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
            StdScr.add_wch(in wCh);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            StdScr.add_wch(in wCh);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
            StdScr.mvadd_wch(nline, ncol, in wCh);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            StdScr.mvadd_wch(nline, ncol, in wCh);
        }

        public override void Write(byte[] str, Encoding encoding)
        {
            unsafe
            {
                int byteLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, encoding);
                byte* buffer = stackalloc byte[byteLength];
                MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, byteLength, str, encoding);
                StdScr.add_wchnstr(in wChStr, wChStr.Length);
            }
        }

        public override void Write(byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            unsafe
            {
                int byteLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, encoding);
                byte* buffer = stackalloc byte[byteLength];
                MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, byteLength, str, encoding, attrs, pair);
                StdScr.add_wchnstr(in wChStr, wChStr.Length);
            }
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding)
        {
            unsafe
            {
                int byteLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, encoding);
                byte* buffer = stackalloc byte[byteLength];
                MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, byteLength, str, encoding);
                StdScr.mvadd_wchnstr(nline, ncol, in wChStr, wChStr.Length);
            }
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            unsafe
            {
                int byteLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, encoding);
                byte* buffer = stackalloc byte[byteLength];
                MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, byteLength, str, encoding, attrs, pair);
                StdScr.mvadd_wchnstr(nline, ncol, in wChStr, wChStr.Length);
            }
        }

        public override void Write(int nline, int ncol, in INCursesChar ch)
        {
            StdScr.mvadd_wch(nline, ncol, VerifyChar(ch));
        }

        public override void Write(int nline, int ncol, in INCursesCharString str)
        {
            StdScr.mvadd_wchnstr(nline, ncol, VerifyString(str), str.Length);
        }

        public override void Put(char ch)
        {
            NCurses.unget_wch(WideCharFactoryInternal<TWideChar>.Instance.GetNativeCharInternal(ch));
        }

        public override void Put(Key key)
        {
            NCurses.ungetch((int)key);
        }
    }
}
