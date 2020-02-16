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
    internal class MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
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
                Window.wgetbkgrnd(this.WindowBaseSafeHandle, out TMultiByte wch);
                return wch;
            }
            set
            {
                Window.wbkgrnd(this.WindowBaseSafeHandle, VerifyChar(value));
            }
        }

        public override INCursesChar InsertBackGround
        {
            get
            {
                Window.wgetbkgrnd(this.WindowBaseSafeHandle, out TMultiByte wch);
                return wch;
            }
            set
            {
                Window.wbkgrndset(this.WindowBaseSafeHandle, VerifyChar(value));
            }
        }

        public override bool HasUnicodeSupport => true;

        public MultiByteWindow(WindowBaseSafeHandle windowBaseSafeHandle)
            : base(windowBaseSafeHandle)
        { }

        public MultiByteWindow(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
            : base(existingWindow)
        { }

        public MultiByteWindow(
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
        public MultiByteWindow(int nlines, int ncols, int begy, int begx)
            : base
            (
                 NativeNCurses.HasUnicodeSupport ? NCurses.newwin(nlines, ncols, begy, begx) : throw new NotSupportedException("Unicode not supported")
            )
        {  }

        public MultiByteWindow(int nlines, int ncols)
            : this(nlines, ncols, 0, 0)
        { }

        public MultiByteWindow()
            : this(0, 0, 0, 0)
        { }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowBaseSafeHandle windowBaseSafeHandle,
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow)
        {
            return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle, parentWindow);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
        {
            return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(existingWindow);
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
            Window.wborder_set(this.WindowBaseSafeHandle, this.VerifyChar(ls), this.VerifyChar(rs), this.VerifyChar(ts), this.VerifyChar(bs),
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

            Window.wborder_set(this.WindowBaseSafeHandle, in ls, in rs, in ts, in bs, in tl, in tr, in bl, in br);
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
            byte[] buffer = new byte[MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str)];
            return MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str);
        }

        public override INCursesCharString CreateString(string str, ulong attrs)
        {
            byte[] buffer = new byte[MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str)];
            return MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str, attrs);
        }

        public override INCursesCharString CreateString(string str, ulong attrs, short pair)
        {
            byte[] buffer = new byte[MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str)];
            return MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, buffer.Length, str, attrs, pair);
        }

        public override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Duplicate()
        {
            return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(NCurses.dupwin(this.WindowBaseSafeHandle));
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            Window.win_wch(this.WindowBaseSafeHandle, out TMultiByte sch);
            ch = sch;
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            Window.mvwin_wch(this.WindowBaseSafeHandle, nline, ncol, out TMultiByte sch);
            ch = sch;
        }

        public override char ExtractChar()
        {
            Window.win_wch(this.WindowBaseSafeHandle, out TMultiByte ch);
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol)
        {
            Window.mvwin_wch(this.WindowBaseSafeHandle, nline, ncol, out TMultiByte ch);
            return ch.Char;
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            Window.win_wch(this.WindowBaseSafeHandle, out TMultiByte ch);
            attrs = ch.Attributes;
            pair = ch.ColorPair;
            return ch.Char;
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            Window.mvwin_wch(this.WindowBaseSafeHandle, nline, ncol, out TMultiByte ch);
            attrs = ch.Attributes;
            pair = ch.ColorPair;
            return ch.Char;
        }

        public override string ExtractString()
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / WideCharFactoryInternal<TWideChar>.Instance.GetCharLength());
            Window.winwstr(this.WindowBaseSafeHandle, ref wChStr);
            return wChStr.ToString();
        }

        public override string ExtractString(int maxChars, out int read)
        {
            int bufferLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(maxChars);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, maxChars);
            Window.winnwstr(this.WindowBaseSafeHandle, ref wChStr, maxChars, out read);
            return wChStr.ToString();
        }

        public override string ExtractString(int nline, int ncol)
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / WideCharFactoryInternal<TWideChar>.Instance.GetCharLength());
            Window.mvwinwstr(this.WindowBaseSafeHandle, nline, ncol, ref wChStr);
            return wChStr.ToString();
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            int bufferLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(maxChars);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, maxChars);
            Window.mvwinnwstr(this.WindowBaseSafeHandle, nline, ncol, ref wChStr, maxChars, out read);
            return wChStr.ToString();
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(Constants.MAX_STRING_LENGTH);
            byte[] buffer = new byte[bufferLength];
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyStringInternal(
                buffer, 
                buffer.Length, 
                Constants.MAX_STRING_LENGTH);
            Window.win_wchstr(this.WindowBaseSafeHandle, ref wChStr);
            charsWithAttributes = wChStr;
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(maxChars);
            byte[] buffer = new byte[bufferLength];
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, maxChars);
            Window.win_wchnstr(this.WindowBaseSafeHandle, ref wChStr, maxChars);
            charsWithAttributes = wChStr;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(Constants.MAX_STRING_LENGTH);
            byte[] buffer = new byte[bufferLength];
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyStringInternal(
                buffer, 
                buffer.Length, 
                Constants.MAX_STRING_LENGTH);
            Window.mvwin_wchstr(this.WindowBaseSafeHandle, nline, ncol, ref wChStr);
            charsWithAttributes = wChStr;
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(maxChars);
            byte[] buffer = new byte[bufferLength];
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyStringInternal(buffer, buffer.Length, maxChars);
            Window.mvwin_wchnstr(this.WindowBaseSafeHandle, nline, ncol, ref wChStr, maxChars);
            charsWithAttributes = wChStr;
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            Window.whline_set(this.WindowBaseSafeHandle, VerifyChar(lineChar), length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            Window.mvwhline_set(this.WindowBaseSafeHandle, nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Insert(char ch)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
            Window.wins_wch(this.WindowBaseSafeHandle, in wCh);
        }

        public override void Insert(in INCursesChar ch)
        {
            Window.wins_wch(this.WindowBaseSafeHandle, VerifyChar(ch));
        }

        public override void Insert(int nline, int ncol, in INCursesChar ch)
        {
            Window.mvwins_wch(this.WindowBaseSafeHandle, nline, ncol, VerifyChar(ch));
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
            Window.mvwins_wch(this.WindowBaseSafeHandle, nline, ncol, in wCh);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            Window.wins_wch(this.WindowBaseSafeHandle, in wCh);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            Window.mvwins_wch(this.WindowBaseSafeHandle, nline, ncol, in wCh);
        }

        public override void Insert(string str)
        {
            int bufferLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            Window.wins_nwstr(this.WindowBaseSafeHandle, in wChStr, wChStr.Length);
        }

        public override void Insert(int nline, int ncol, string str)
        {
            int bufferLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            Window.mvwins_nwstr(this.WindowBaseSafeHandle, nline, ncol, in wChStr, wChStr.Length);
        }

        public override void Insert(string str, ulong attrs, short pair)
        {
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            MultiByteCharString<TMultiByte> mbStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);

            IEnumerable<IMultiByteNCursesChar> wchars = mbStr;
            foreach (IMultiByteNCursesChar wch in wchars.Reverse())
            {
                Window.wins_wch(this.WindowBaseSafeHandle, VerifyChar(wch));
            }
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            bool ret = Window.wget_wch(this.WindowBaseSafeHandle, out TWideChar wch, out key);
            ch = wch.Char;
            return ret;
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            bool ret = Window.mvwget_wch(this.WindowBaseSafeHandle, nline, ncol, out TWideChar wch, out key);
            ch = wch.Char;
            return ret;
        }

        public override string ReadLine()
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / WideCharFactoryInternal<TWideChar>.Instance.GetCharLength());
            Window.wget_wstr(this.WindowBaseSafeHandle, ref wChStr);
            return wChStr.ToString();
        }

        public override string ReadLine(int nline, int ncol)
        {
            byte[] buffer = NativeNCurses.GetBuffer();
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(
                buffer,
                buffer.Length,
                buffer.Length / WideCharFactoryInternal<TWideChar>.Instance.GetCharLength());
            Window.mvwget_wstr(this.WindowBaseSafeHandle, nline, ncol, ref wChStr);
            return wChStr.ToString();
        }

        public override string ReadLine(int length)
        {
            int bufferLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(length);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, length);
            Window.wgetn_wstr(this.WindowBaseSafeHandle, ref wChStr, length);
            return wChStr.ToString();
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            int bufferLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(length);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyStringInternal(buffer, bufferLength, length);
            Window.mvwgetn_wstr(this.WindowBaseSafeHandle, nline, ncol, ref wChStr, length);
            return wChStr.ToString();
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            Window.wvline_set(this.WindowBaseSafeHandle, VerifyChar(lineChar), length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            Window.mvwvline_set(this.WindowBaseSafeHandle, nline, ncol, VerifyChar(lineChar), length);
        }

        public override void Write(in INCursesChar ch)
        {
            Window.wadd_wch(this.WindowBaseSafeHandle, VerifyChar(ch));
        }

        public override void Write(in INCursesCharString str)
        {
            Window.wadd_wchstr(this.WindowBaseSafeHandle, VerifyString(str));
        }

        public override void Write(string str)
        {
            int bufferLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            Window.waddnwstr(this.WindowBaseSafeHandle, in wChStr, wChStr.Length);
        }

        public override void Write(string str, ulong attrs, short pair)
        {
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);
            Window.wadd_wchnstr(this.WindowBaseSafeHandle, in wChStr, wChStr.Length);
        }

        public override void Write(int nline, int ncol, string str)
        {
            int bufferLength = WideCharFactoryInternal<TWideChar>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            WideCharString<TWideChar> wChStr = WideCharFactoryInternal<TWideChar>.Instance.GetNativeStringInternal(buffer, bufferLength, str);
            Window.mvwaddnwstr(this.WindowBaseSafeHandle, nline, ncol, in wChStr, wChStr.Length);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, attrs, pair);
            Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol, in wChStr, wChStr.Length);
        }

        //TODO: don't convert?
        public override void Write(char ch)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
            Window.wadd_wch(this.WindowBaseSafeHandle, in wCh);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            Window.wadd_wch(this.WindowBaseSafeHandle, in wCh);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
            Window.mvwadd_wch(this.WindowBaseSafeHandle, nline, ncol, in wCh);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch, attrs, pair);
            Window.mvwadd_wch(this.WindowBaseSafeHandle, nline, ncol, in wCh);
        }

        public override void Write(byte[] str, Encoding encoding)
        {
            int charLength = encoding.GetCharCount(str);
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, encoding);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding);
            Window.wadd_wchnstr(this.WindowBaseSafeHandle, in wChStr, wChStr.Length);
        }

        public override void Write(byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            int charLength = encoding.GetCharCount(str);
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, encoding);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding, attrs, pair);
            Window.wadd_wchnstr(this.WindowBaseSafeHandle, in wChStr, wChStr.Length);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding)
        {
            int charLength = encoding.GetCharCount(str);
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, encoding);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding);
            Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol,  in wChStr, wChStr.Length);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            int charLength = encoding.GetCharCount(str);
            int bufferLength = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetByteCount(str, encoding);
            byte[] buffer = NativeNCurses.GetBuffer(bufferLength);
            MultiByteCharString<TMultiByte> wChStr = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeStringInternal(buffer, bufferLength, str, charLength, encoding, attrs, pair);
            Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol, in wChStr, wChStr.Length);
        }

        public override void Write(int nline, int ncol, in INCursesChar ch)
        {
            Window.mvwadd_wch(this.WindowBaseSafeHandle, nline, ncol, VerifyChar(ch));
        }

        public override void Write(int nline, int ncol, in INCursesCharString str)
        {
            Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol, VerifyString(str), str.Length);
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
