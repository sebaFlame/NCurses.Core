using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Buffers;
using System.Threading;

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

        private MultiByteCharString<TMultiByte, TWideChar, TSingleByte> VerifyString(in INCursesCharString str)
        {
            if (!(str is MultiByteCharString<TMultiByte, TWideChar, TSingleByte> tmp))
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
            TMultiByte ls = default;
            TMultiByte rs = default;
            TMultiByte ts = default;
            TMultiByte bs = default;
            TMultiByte tl = default;
            TMultiByte tr = default;
            TMultiByte bl = default;
            TMultiByte br = default;

            Window.wborder_set(this.WindowBaseSafeHandle, in ls, in rs, in ts, in bs, in tl, in tr, in bl, in br);
        }

        public override void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar)
        {
            Window.box_set(this.WindowBaseSafeHandle, this.VerifyChar(verticalChar), this.VerifyChar(horizontalChar));
        }

        public override void Box()
        {
            TMultiByte verch = default;
            TMultiByte horch = default;

            Window.box_set(this.WindowBaseSafeHandle, in verch, in horch);
        }

        //TODO: use native override?
        public override IChar CreateChar(char ch)
        {
            return WideCharFactory<TWideChar>._Instance.GetNativeChar(ch);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs)
        {
            return MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch, attrs, 0);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs, ushort pair)
        {
            return MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch, attrs, pair);
        }

        public override ICharString CreateString(string str)
        {
            return this.CreateString(str.AsSpan());
        }

        public override ICharString CreateString(ReadOnlySpan<char> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreateArrayBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                return @string;
            }
        }

        public override ICharString CreateString(ReadOnlySpan<byte> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreateArrayBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                return @string;
            }
        }

        public override ICharString CreateString(in ReadOnlySequence<byte> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreateArrayBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                return @string;
            }
        }

        public override ICharString CreateString(in ReadOnlySequence<byte> str, Encoding encoding)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreateArrayBuffer,
                in str,
                encoding,
                out WideCharString<TWideChar> @string))
            {
                return @string;
            }
        }

        public override INCursesCharString CreateString(string str, ulong attrs)
        {
            return this.CreateString(str.AsSpan(), attrs);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<char> str, ulong attrs)
        {
            return this.CreateString(str, attrs, 0);
        }

        public override INCursesCharString CreateString(string str, ulong attrs, ushort pair)
        {
            return this.CreateString(str.AsSpan(), attrs, pair);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreateArrayBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                return @string;
            }
        }

        public override INCursesCharString CreateString(ReadOnlySpan<byte> str, ulong attrs)
        {
            return this.CreateString(str, attrs, 0);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreateArrayBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                return @string;
            }
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, ulong attrs)
        {
            return this.CreateString(str, attrs, 0);
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreateArrayBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                return @string;
            }
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs)
        {
            return this.CreateString(in str, encoding, attrs, 0);
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreateArrayBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                return @string;
            }
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
            return (char)MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetChar(ch);
        }

        public override char ExtractChar(INCursesChar ch)
        {
            return (char)MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetChar(VerifyChar(ch));
        }

        public override char ExtractChar(int nline, int ncol)
        {
            Window.mvwin_wch(this.WindowBaseSafeHandle, nline, ncol, out TMultiByte ch);
            return (char)MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetChar(ch);
        }

        public override char ExtractChar(out ulong attrs, out ushort pair)
        {
            Window.win_wch(this.WindowBaseSafeHandle, out TMultiByte ch);

            attrs = ch.Attributes;
            pair = ch.ColorPair;
            return (char)MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetChar(ch);
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out ushort pair)
        {
            Window.mvwin_wch(this.WindowBaseSafeHandle, nline, ncol, out TMultiByte ch);

            attrs = ch.Attributes;
            pair = ch.ColorPair;
            return (char)MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetChar(ch);
        }

        public override string ExtractString()
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeEmptyString(
                WideCharFactory<TWideChar>._CreatePooledBuffer, 
                Constants.MAX_STRING_LENGTH, 
                out WideCharString<TWideChar> @string))
            {
                Window.winnwstr(this.WindowBaseSafeHandle, ref @string, bufferState.EncoderState.BufferLength, out int read);

                return @string.ToString();
            }
        }

        public override string ExtractString(int maxChars, out int read)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeEmptyString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                maxChars,
                out WideCharString<TWideChar> @string))
            {
                Window.winnwstr(this.WindowBaseSafeHandle, ref @string, maxChars, out read);

                return @string.ToString();
            }
        }

        public override string ExtractString(int nline, int ncol)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeEmptyString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                Constants.MAX_STRING_LENGTH,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwinnwstr(this.WindowBaseSafeHandle, nline, ncol, ref @string, @string.CharLength, out int read);

                return @string.ToString();
            }
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeEmptyString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                maxChars,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwinnwstr(this.WindowBaseSafeHandle, nline, ncol, ref @string, maxChars, out read);

                return @string.ToString();
            }
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeEmptyString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreateArrayBuffer,
                Constants.MAX_STRING_LENGTH,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.win_wchstr(this.WindowBaseSafeHandle, ref @string);
                charsWithAttributes = @string;
            }
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeEmptyString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreateArrayBuffer,
                maxChars,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.win_wchnstr(this.WindowBaseSafeHandle, ref @string, maxChars);
                charsWithAttributes = @string;
            }
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeEmptyString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreateArrayBuffer,
                Constants.MAX_STRING_LENGTH,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.mvwin_wchstr(this.WindowBaseSafeHandle, nline, ncol, ref @string);
                charsWithAttributes = @string;
            }
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeEmptyString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreateArrayBuffer,
                maxChars,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.mvwin_wchnstr(this.WindowBaseSafeHandle, nline, ncol, ref @string, maxChars);
                charsWithAttributes = @string;
            }
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
            TMultiByte wCh = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch);
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
            TMultiByte wCh = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch);
            Window.mvwins_wch(this.WindowBaseSafeHandle, nline, ncol, in wCh);
        }

        public override void Insert(char ch, ulong attrs, ushort pair)
        {
            TMultiByte wCh = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch, attrs, pair);
            Window.wins_wch(this.WindowBaseSafeHandle, in wCh);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, ushort pair)
        {
            TMultiByte wCh = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch, attrs, pair);
            Window.mvwins_wch(this.WindowBaseSafeHandle, nline, ncol, in wCh);
        }

        public override void Insert(string str)
        {
            this.Insert(str.AsSpan());
        }

        public override void Insert(ReadOnlySpan<char> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.wins_nwstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Insert(int nline, int ncol, string str)
        {
            this.Insert(nline, ncol, str.AsSpan());
        }

        public override void Insert(int nline, int ncol, ReadOnlySpan<char> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwins_nwstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Insert(string str, ulong attrs, ushort pair)
        {
            this.Insert(str.AsSpan(), attrs, pair);
        }

        public override void Insert(ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Span<TMultiByte> buffer = bufferState.Memory.Span;
                for (int i = buffer.Length - 2; i >= 0; i--) //-2 because of null terminator
                {
                    Window.wins_wch(this.WindowBaseSafeHandle, in buffer[i]);
                }
            }
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            bool ret = Window.wget_wch(this.WindowBaseSafeHandle, out TWideChar wch, out key);

            ch = (char)WideCharFactory<TWideChar>._Instance.GetChar(wch);

            return ret;
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            bool ret = Window.mvwget_wch(this.WindowBaseSafeHandle, nline, ncol, out TWideChar wch, out key);

            ch = (char)WideCharFactory<TWideChar>._Instance.GetChar(wch);

            return ret;
        }

        public override string ReadLine()
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeEmptyString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                Constants.MAX_STRING_LENGTH,
                out WideCharString<TWideChar> @string))
            {
                Window.wget_wstr(this.WindowBaseSafeHandle, ref @string);
                return @string.ToString();
            }
        }

        public override string ReadLine(int nline, int ncol)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeEmptyString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                Constants.MAX_STRING_LENGTH,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwget_wstr(this.WindowBaseSafeHandle, nline, ncol, ref @string);
                return @string.ToString();
            }
        }

        public override string ReadLine(int length)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeEmptyString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                length,
                out WideCharString<TWideChar> @string))
            {
                Window.wgetn_wstr(this.WindowBaseSafeHandle, ref @string, length);
                return @string.ToString();
            }
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeEmptyString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                length,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwgetn_wstr(this.WindowBaseSafeHandle, nline, ncol, ref @string, length);
                return @string.ToString();
            }
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

            this.Advance(str);
        }

        public override void Write(string str)
        {
            this.Write(str.AsSpan());
        }

        public override void Write(ReadOnlySpan<char> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Write(ReadOnlySpan<byte> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Write(in ReadOnlySequence<byte> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                in str,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Write(string str, ulong attrs, ushort pair)
        {
            this.Write(str.AsSpan(), attrs, pair);
        }

        public override void Write(ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.wadd_wchnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(ReadOnlySpan<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.wadd_wchnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(in ReadOnlySequence<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.wadd_wchnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(int nline, int ncol, string str)
        {
            this.Write(nline, ncol, str.AsSpan());
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<char> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwaddnwstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwaddnwstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwaddnwstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, ushort pair)
        {
            this.Write(nline, ncol, str.AsSpan(), attrs, pair);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        //TODO: don't convert?
        public override void Write(char ch)
        {
            TMultiByte wCh = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch);
            Window.wadd_wch(this.WindowBaseSafeHandle, in wCh);
        }

        public override void Write(char ch, ulong attrs, ushort pair)
        {
            TMultiByte wCh = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch, attrs, pair);
            Window.wadd_wch(this.WindowBaseSafeHandle, in wCh);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            TMultiByte wCh = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch);
            Window.mvwadd_wch(this.WindowBaseSafeHandle, nline, ncol, in wCh);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, ushort pair)
        {
            TMultiByte wCh = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeChar(ch, attrs, pair);
            Window.mvwadd_wch(this.WindowBaseSafeHandle, nline, ncol, in wCh);
        }

        public override void Write(ReadOnlySpan<byte> str, Encoding encoding)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                encoding,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Write(in ReadOnlySequence<byte> str, Encoding encoding)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                in str,
                encoding,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str, Encoding encoding)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                encoding,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwaddnwstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str, Encoding encoding)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                encoding,
                out WideCharString<TWideChar> @string))
            {
                Window.mvwaddnwstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Write(ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                encoding,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.wadd_wchnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                encoding,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.wadd_wchnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                encoding,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            using (BufferState<TMultiByte> bufferState = MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._Instance.GetNativeString(
                MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>._CreatePooledBuffer,
                str,
                encoding,
                attrs,
                pair,
                out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string))
            {
                Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(int nline, int ncol, in INCursesChar ch)
        {
            Window.mvwadd_wch(this.WindowBaseSafeHandle, nline, ncol, VerifyChar(ch));
        }

        public override void Write(int nline, int ncol, in INCursesCharString str)
        {
            Window.mvwadd_wchnstr(this.WindowBaseSafeHandle, nline, ncol, VerifyString(str), str.Length);
            this.Advance(str);
        }

        public override void Put(char ch)
        {
            NCurses.unget_wch(WideCharFactory<TWideChar>._Instance.GetNativeChar(ch));
        }

        public override void Put(Key key)
        {
            NCurses.ungetch((int)key);
        }

        public override void Write(string str, int maxLength)
        {
            this.Write(str.AsSpan(), maxLength);
        }

        public override void Write(ReadOnlySpan<char> str, int maxLength)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, maxLength);
            }
        }

        public override void Write(ReadOnlySpan<byte> str, int maxLength)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, maxLength);
            }
        }

        public override void Write(in ReadOnlySequence<byte> str, int maxLength)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, maxLength);
            }
        }

        public override void Write(ReadOnlySpan<byte> str, int maxLength, Encoding encoding)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                encoding,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, maxLength);
            }
        }

        public override void Write(in ReadOnlySequence<byte> str, int maxLength, Encoding encoding)
        {
            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(
                WideCharFactory<TWideChar>._CreatePooledBuffer,
                str,
                encoding,
                out WideCharString<TWideChar> @string))
            {
                Window.waddnwstr(this.WindowBaseSafeHandle, in @string, maxLength);
            }
        }
    }
}
