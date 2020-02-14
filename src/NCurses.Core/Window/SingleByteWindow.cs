using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using NCurses.Core.Interop;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Wrappers;

namespace NCurses.Core.Window
{
    internal class SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
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
                Window.wbkgd(this.WindowBaseSafeHandle, VerifyChar(value));
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
                Window.wbkgdset(this.WindowBaseSafeHandle, VerifyChar(value));
            }
        }

        public override bool HasUnicodeSupport => false;

        public SingleByteWindow(WindowBaseSafeHandle windowBaseSafeHandle)
            : base(windowBaseSafeHandle)
        { }

        public SingleByteWindow(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
            : base(existingWindow)
        { }

        public SingleByteWindow(
            WindowBaseSafeHandle windowBaseSafeHandle,
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow)
            : base(windowBaseSafeHandle, parentWindow) { }

        ///// <summary>
        ///// create a new window
        ///// </summary>
        ///// <param name="nlines">number of lines of the new window</param>
        ///// <param name="ncols">number of columns of the new window</param>
        ///// <param name="begy">line of the upper left corner of the new window</param>
        ///// <param name="begx">column of the upper left corent of the new window</param>
        public SingleByteWindow(int nlines, int ncols, int begy, int begx)
            : base(NCurses.newwin(nlines, ncols, begy, begx))
        {  }

        public SingleByteWindow(int nlines, int ncols)
            : this(nlines, ncols, 0, 0)
        { }

        public SingleByteWindow()
            : this(0, 0, 0, 0)
        { }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowBaseSafeHandle windowBaseSafeHandle,
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow)
        {
            return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle, parentWindow);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
        {
            return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(existingWindow);
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
            Window.wborder(this.WindowBaseSafeHandle, this.VerifyChar(ls), this.VerifyChar(rs), this.VerifyChar(ts), this.VerifyChar(bs), 
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

            Window.wborder(this.WindowBaseSafeHandle, ls, rs, ts, bs, tl, tr, bl, br);
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

        public override INCursesCharString CreateString(string str, ulong attrs)
        {
            byte[] buffer = new byte[SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str)];
            return SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str, attrs);
        }

        public override INCursesCharString CreateString(string str, ulong attrs, short pair)
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
            Window.winch(this.WindowBaseSafeHandle, out TSingleByte ch);
            return ch.Char;
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            Window.winch(this.WindowBaseSafeHandle, out TSingleByte sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            Window.mvwinch(this.WindowBaseSafeHandle, nline, ncol, out TSingleByte sch);
            ch = sch;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            Window.mvwinch(this.WindowBaseSafeHandle, nline, ncol, out TSingleByte ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            Window.winch(this.WindowBaseSafeHandle, out TSingleByte ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            Window.mvwinch(this.WindowBaseSafeHandle, nline, ncol, out TSingleByte ch);
            attrs = ch.Attributes;
            pair = ch.Color;
            return ch.Char;
        }

        public override string ExtractString()
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / CharFactoryInternal<TChar>.Instance.GetCharLength());
            Window.winstr(this.WindowBaseSafeHandle, ref chStr, out int read);
            return chStr.ToString();
        }

        public override string ExtractString(int maxChars, out int read)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(maxChars);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, maxChars);
            Window.winnstr(this.WindowBaseSafeHandle, ref chStr, maxChars, out read);
            return chStr.ToString();
        }

        public override string ExtractString(int nline, int ncol)
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / CharFactoryInternal<TChar>.Instance.GetCharLength());
            Window.mvwinstr(this.WindowBaseSafeHandle, nline, ncol, ref chStr, out int read);
            return chStr.ToString();
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(maxChars);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, maxChars);
            Window.mvwinnstr(this.WindowBaseSafeHandle, nline, ncol, ref chStr, maxChars, out read);
            return chStr.ToString();
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(Constants.MAX_STRING_LENGTH);
            byte[] buffer = new byte[bufferLength];
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, Constants.MAX_STRING_LENGTH);
            Window.winchstr(this.WindowBaseSafeHandle, ref chStr, out int read);
            charsWithAttributes = chStr;
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(maxChars);
            byte[] buffer = new byte[bufferLength];
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, maxChars);
            Window.winchnstr(this.WindowBaseSafeHandle, ref chStr, maxChars, out int read);
            charsWithAttributes = chStr;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(Constants.MAX_STRING_LENGTH);
            byte[] buffer = new byte[bufferLength];
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, Constants.MAX_STRING_LENGTH);
            Window.mvwinchstr(this.WindowBaseSafeHandle, nline, ncol, ref chStr, out int read);
            charsWithAttributes = chStr;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(maxChars);
            byte[] buffer = new byte[bufferLength];
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, maxChars);
            Window.mvwinchnstr(this.WindowBaseSafeHandle, nline, ncol, ref chStr, maxChars, out int read);
            charsWithAttributes = chStr;
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            Window.whline(this.WindowBaseSafeHandle, VerifyChar(lineChar), length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            Window.mvwhline(this.WindowBaseSafeHandle, nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Insert(char ch)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            Window.winsch(this.WindowBaseSafeHandle, in sch);
        }

        public override void Insert(in INCursesChar ch)
        {
            Window.winsch(this.WindowBaseSafeHandle, VerifyChar(ch));
        }

        public override void Insert(int nline, int ncol, in INCursesChar ch)
        {
            Window.mvwinsch(this.WindowBaseSafeHandle, nline, ncol, VerifyChar(ch));
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            Window.mvwinsch(this.WindowBaseSafeHandle, nline, ncol, in sch);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            Window.winsch(this.WindowBaseSafeHandle, in sch);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            Window.mvwinsch(this.WindowBaseSafeHandle, nline, ncol, in sch);
        }

        public override void Insert(string str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            Window.winsnstr(this.WindowBaseSafeHandle, in chStr, chStr.Length);
        }

        public override void Insert(int nline, int ncol, string str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            Window.mvwinsnstr(this.WindowBaseSafeHandle, nline, ncol, in chStr, chStr.Length);
        }

        public override void Insert(string str, ulong attrs, short pair)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            SingleByteCharString<TSingleByte> sStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);

            IEnumerable<ISingleByteNCursesChar> wchars = sStr;
            foreach (ISingleByteNCursesChar wch in wchars.Reverse())
            {
                Window.winsch(this.WindowBaseSafeHandle, VerifyChar(wch));
            }
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            return Window.wgetch(this.WindowBaseSafeHandle, out ch, out key);
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            return Window.mvwgetch(this.WindowBaseSafeHandle, nline, ncol, out ch, out key);
        }

        public override string ReadLine()
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / CharFactoryInternal<TChar>.Instance.GetCharLength());
            Window.wgetstr(this.WindowBaseSafeHandle, ref chStr);
            return chStr.ToString();
        }

        public override string ReadLine(int nline, int ncol)
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / CharFactoryInternal<TChar>.Instance.GetCharLength());
            Window.mvwgetstr(this.WindowBaseSafeHandle, nline, ncol, ref chStr);
            return chStr.ToString();
        }

        public override string ReadLine(int length)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(length);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, length);
            Window.wgetnstr(this.WindowBaseSafeHandle, ref chStr, length);
            return chStr.ToString();
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(length);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, length);
            Window.mvwgetnstr(this.WindowBaseSafeHandle, nline, ncol, ref chStr, length);
            return chStr.ToString();
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            Window.wvline(this.WindowBaseSafeHandle, VerifyChar(lineChar), length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            Window.mvwvline(this.WindowBaseSafeHandle, nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Write(in INCursesChar ch)
        {
            Window.waddch(this.WindowBaseSafeHandle, VerifyChar(ch));
        }

        public override void Write(in INCursesCharString str)
        {
            Window.waddchstr(this.WindowBaseSafeHandle, VerifyString(str));
        }

        public override void Write(string str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            Window.waddnstr(this.WindowBaseSafeHandle, in chStr, chStr.Length);
        }

        public override void Write(string str, ulong attrs, short pair)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);
            Window.waddchnstr(this.WindowBaseSafeHandle, in chStr, chStr.Length);
        }

        public override void Write(int nline, int ncol, string str)
        {
            int bufferLength = CharFactoryInternal<TChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            CharString<TChar> chStr = CharFactoryInternal<TChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            Window.mvwaddnstr(this.WindowBaseSafeHandle, nline, ncol, in chStr, chStr.Length);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            int bufferLength = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            SingleByteCharString<TSingleByte> chStr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);
            Window.mvwaddchnstr(this.WindowBaseSafeHandle, nline, ncol, in chStr, chStr.Length);
        }

        public override void Write(char ch)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            Window.waddch(this.WindowBaseSafeHandle, in sch);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            Window.waddch(this.WindowBaseSafeHandle, in sch);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            Window.mvwaddch(this.WindowBaseSafeHandle, nline, ncol, in sch);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            TSingleByte sch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeCharInternal(ch);
            Window.mvwaddch(this.WindowBaseSafeHandle, nline, ncol, in sch);
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
            Window.mvwaddch(this.WindowBaseSafeHandle, nline, ncol, VerifyChar(ch));
        }

        public override void Write(int nline, int ncol, in INCursesCharString str)
        {
            Window.mvwaddchnstr(this.WindowBaseSafeHandle, nline, ncol, VerifyString(str), str.Length);
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
