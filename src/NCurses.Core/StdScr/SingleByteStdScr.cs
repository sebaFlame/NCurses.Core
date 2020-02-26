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
    internal class SingleByteStdScr<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : StdScrBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        public override INCursesChar BackGround
        {
            get
            {
                return Window.getbkgd(this.WindowBaseSafeHandle);
            }
            set
            {
                StdScr.bkgd(VerifyChar(value));
            }
        }

        public override INCursesChar InsertBackGround
        {
            get
            {
                return Window.getbkgd(this.WindowBaseSafeHandle);
            }
            set
            {
                StdScr.bkgdset(VerifyChar(value));
            }
        }

        public override bool HasUnicodeSupport => false;

        public SingleByteStdScr(WindowBaseSafeHandle stdScr)
            : base(stdScr) { }

        public SingleByteStdScr(StdScrBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> stdScr)
            : base(stdScr) { }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowBaseSafeHandle windowBaseSafeHandle,
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow)
        {
            return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle, parentWindow);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
        {
            throw new NotSupportedException("Can not create a new StdScr from an existing window");
        }

        private TSingleByte VerifyChar(in INCursesChar ch)
        {
            if (!(ch is TSingleByte tmp))
            {
                throw new InvalidCastException("Character is in incorrect format");
            }

            return tmp;
        }

        private SingleByteCharString<TSingleByte> VerifyString(in INCursesCharString str)
        {
            if (!(str is SingleByteCharString<TSingleByte> tmp))
            {
                throw new InvalidCastException("String is in incorrect format");
            }

            return tmp;
        }

        public override void Border(in INCursesChar ls, in INCursesChar rs, in INCursesChar ts, in INCursesChar bs,
            in INCursesChar tl, in INCursesChar tr, in INCursesChar bl, in INCursesChar br)
        {
            StdScr.border(this.VerifyChar(ls), this.VerifyChar(rs), this.VerifyChar(ts), this.VerifyChar(bs),
                this.VerifyChar(tl), this.VerifyChar(tr), this.VerifyChar(bl), this.VerifyChar(br));
        }

        public override void Border()
        {
            TSingleByte ls = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();
            TSingleByte rs = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();
            TSingleByte ts = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();
            TSingleByte bs = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();
            TSingleByte tl = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();
            TSingleByte tr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();
            TSingleByte bl = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();
            TSingleByte br = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();

            StdScr.border(ls, rs, ts, bs, tl, tr, bl, br);
        }

        public override void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar)
        {
            Window.box(this.WindowBaseSafeHandle, this.VerifyChar(verticalChar), this.VerifyChar(horizontalChar));
        }

        public override void Box()
        {
            TSingleByte verch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();
            TSingleByte horch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();

            Window.box(this.WindowBaseSafeHandle, verch, horch);
        }

        //TODO: use native override?
        public override INCursesChar CreateChar(char ch)
        {
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs)
        {
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch, attrs);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs, short pair)
        {
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
        }

        public override INCursesCharString CreateString(string str)
        {
            byte[] buffer = new byte[SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str)];
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<char> str)
        {
            byte[] buffer = new byte[SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str)];
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str);
        }

        public override INCursesCharString CreateString(string str, ulong attrs)
        {
            byte[] buffer = new byte[SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str)];
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str, attrs);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<char> str, ulong attrs)
        {
            byte[] buffer = new byte[SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str)];
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str, attrs);
        }

        public override INCursesCharString CreateString(string str, ulong attrs, short pair)
        {
            byte[] buffer = new byte[SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str)];
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str, attrs, pair);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<char> str, ulong attrs, short pair)
        {
            byte[] buffer = new byte[SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str)];
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str, attrs, pair);
        }

        public override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Duplicate()
        {
            return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(NCurses.dupwin(this.WindowBaseSafeHandle));
        }

        public override char ExtractChar()
        {
            StdScr.inch(out TSingleByte ch);
            return ch.Char;
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            StdScr.inch(out TSingleByte sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            StdScr.mvinch(nline, ncol, out TSingleByte sch);
            ch = sch;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            StdScr.mvinch(nline, ncol, out TSingleByte ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            StdScr.inch(out TSingleByte ch);
            attrs = ch.Attributes;
            pair = ch.ColorPair;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            StdScr.mvinch(nline, ncol, out TSingleByte ch);
            attrs = ch.Attributes;
            pair = ch.ColorPair;
            return ch.Char;
        }

        public override string ExtractString()
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / CharFactoryInternal<TChar>.Instance.GetCharLength());
            StdScr.instr(ref chStr, out int read);
            return chStr.ToString();
        }

        public override string ExtractString(int maxChars, out int read)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(maxChars);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, maxChars);
            StdScr.innstr(ref chStr, maxChars, out read);
            return chStr.ToString();
        }

        public override string ExtractString(int nline, int ncol)
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / CharFactoryInternal<TChar>.Instance.GetCharLength());
            StdScr.mvinstr(nline, ncol, ref chStr, out int read);
            return chStr.ToString();
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(maxChars);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, maxChars);
            StdScr.mvinnstr(nline, ncol, ref chStr, maxChars, out read);
            return chStr.ToString();
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(Constants.MAX_STRING_LENGTH);
            byte[] buffer = new byte[bufferLength];
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, Constants.MAX_STRING_LENGTH);
            StdScr.inchstr(ref chStr, out int read);
            charsWithAttributes = chStr;
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(maxChars);
            byte[] buffer = new byte[bufferLength];
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, maxChars);
            StdScr.inchnstr(ref chStr, maxChars, out int read);
            charsWithAttributes = chStr;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(Constants.MAX_STRING_LENGTH);
            byte[] buffer = new byte[bufferLength];
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, Constants.MAX_STRING_LENGTH);
            StdScr.mvinchstr(nline, ncol, ref chStr, out int read);
            charsWithAttributes = chStr;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(maxChars);
            byte[] buffer = new byte[bufferLength];
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, maxChars);
            StdScr.mvinchnstr(nline, ncol, ref chStr, maxChars, out int read);
            charsWithAttributes = chStr;
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            StdScr.hline(VerifyChar(lineChar), length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            StdScr.mvhline(nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Insert(char ch)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            StdScr.insch(in sch);
        }

        public override void Insert(in INCursesChar ch)
        {
            StdScr.insch(VerifyChar(ch));
        }

        public override void Insert(int nline, int ncol, in INCursesChar ch)
        {
            StdScr.mvinsch(nline, ncol, VerifyChar(ch));
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            StdScr.mvinsch(nline, ncol, in sch);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            StdScr.insch(in sch);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            StdScr.mvinsch(nline, ncol, in sch);
        }

        public override void Insert(string str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            StdScr.insnstr(in chStr, chStr.Length);
        }

        public override void Insert(ReadOnlySpan<char> str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            StdScr.insnstr(in chStr, chStr.Length);
        }

        public override void Insert(int nline, int ncol, string str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            StdScr.mvinsnstr(nline, ncol, in chStr, chStr.Length);
        }

        public override void Insert(int nline, int ncol, ReadOnlySpan<char> str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            StdScr.mvinsnstr(nline, ncol, in chStr, chStr.Length);
        }

        public override void Insert(string str, ulong attrs, short pair)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            SingleByteCharString<TSingleByte> sStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);

            IEnumerable<ISingleByteNCursesChar> wchars = sStr;
            foreach (ISingleByteNCursesChar wch in wchars.Reverse())
            {
                StdScr.insch(VerifyChar(wch));
            }
        }

        public override void Insert(ReadOnlySpan<char> str, ulong attrs, short pair)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            SingleByteCharString<TSingleByte> sStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);

            IEnumerable<ISingleByteNCursesChar> wchars = sStr;
            foreach (ISingleByteNCursesChar wch in wchars.Reverse())
            {
                StdScr.insch(VerifyChar(wch));
            }
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            return StdScr.getch(out ch, out key);
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            return StdScr.mvgetch(nline, ncol, out ch, out key);
        }

        public override string ReadLine()
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / CharFactoryInternal<TChar>.Instance.GetCharLength());
            StdScr.getstr(ref chStr);
            return chStr.ToString();
        }

        public override string ReadLine(int nline, int ncol)
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / CharFactoryInternal<TChar>.Instance.GetCharLength());
            StdScr.mvgetstr(nline, ncol, ref chStr);
            return chStr.ToString();
        }

        public override string ReadLine(int length)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(length);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, length);
            StdScr.getnstr(ref chStr, length);
            return chStr.ToString();
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(length);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, length);
            StdScr.mvgetnstr(nline, ncol, ref chStr, length);
            return chStr.ToString();
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            StdScr.vline(VerifyChar(lineChar), length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            StdScr.mvvline(nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Write(in INCursesChar ch)
        {
            StdScr.addch(VerifyChar(ch));
        }

        public override void Write(in INCursesCharString str)
        {
            StdScr.addchstr(VerifyString(str));
        }

        public override void Write(string str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            StdScr.addnstr(in chStr, chStr.Length);
        }

        public override void Write(ReadOnlySpan<char> str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            StdScr.addnstr(in chStr, chStr.Length);
        }

        public override void Write(string str, ulong attrs, short pair)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);
            StdScr.addchnstr(in chStr, chStr.Length);
        }

        public override void Write(ReadOnlySpan<char> str, ulong attrs, short pair)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);
            StdScr.addchnstr(in chStr, chStr.Length);
        }

        public override void Write(int nline, int ncol, string str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            StdScr.mvaddnstr(nline, ncol, in chStr, chStr.Length);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<char> str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            StdScr.mvaddnstr(nline, ncol, in chStr, chStr.Length);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);
            StdScr.mvaddchnstr(nline, ncol, in chStr, chStr.Length);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<char> str, ulong attrs, short pair)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);
            StdScr.mvaddchnstr(nline, ncol, in chStr, chStr.Length);
        }

        public override void Write(char ch)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            StdScr.addch(in sch);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            StdScr.addch(in sch);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            StdScr.mvaddch(nline, ncol, in sch);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            StdScr.mvaddch(nline, ncol, in sch);
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
            StdScr.mvaddch(nline, ncol, VerifyChar(ch));
        }

        public override void Write(int nline, int ncol, in INCursesCharString str)
        {
            StdScr.mvaddchnstr(nline, ncol, VerifyString(str), str.Length);
        }

        public override void Put(char ch)
        {
            NCurses.ungetch(ch);
        }

        public override void Put(Key key)
        {
            NCurses.ungetch((int)key);
        }
    }
}
