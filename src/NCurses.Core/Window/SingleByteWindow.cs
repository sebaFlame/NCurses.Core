using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Buffers;

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
            TSingleByte ls = default;
            TSingleByte rs = default;
            TSingleByte ts = default;
            TSingleByte bs = default;
            TSingleByte tl = default;
            TSingleByte tr = default;
            TSingleByte bl = default;
            TSingleByte br = default;

            Window.wborder(this.WindowBaseSafeHandle, ls, rs, ts, bs, tl, tr, bl, br);
        }

        public override void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar)
        {
            Window.box(this.WindowBaseSafeHandle, this.VerifyChar(verticalChar), this.VerifyChar(horizontalChar));
        }

        public override void Box()
        {
            TSingleByte verch = default;
            TSingleByte horch = default;

            Window.box(this.WindowBaseSafeHandle, verch, horch);
        }

        //TODO: use native override?
        public override IChar CreateChar(char ch)
        {
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            return CharFactory<TChar>._Instance.GetNativeChar((byte)ch);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs)
        {
            return this.CreateChar(ch, attrs, 0);
        }

        public override INCursesChar CreateChar(char ch, ulong attrs, ushort pair)
        {
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            return SingleByteCharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch, attrs, pair);
        }

        public override ICharString CreateString(string str)
        {
            return this.CreateString(str.AsSpan());
        }

        public override ICharString CreateString(ReadOnlySpan<char> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                str,
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public override ICharString CreateString(ReadOnlySpan<byte> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                str,
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public override ICharString CreateString(in ReadOnlySequence<byte> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                in str,
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public override INCursesCharString CreateString(string str, ulong attrs)
        {
            return this.CreateString(str.AsSpan(), attrs, 0);
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
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreateArrayBuffer,
                str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                return @string;
            }
        }

        public override INCursesCharString CreateString(ReadOnlySpan<byte> str, ulong attrs)
        {
            return this.CreateString(str, attrs, 0);
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, ulong attrs)
        {
            return this.CreateString(in str, attrs, 0);
        }

        public override INCursesCharString CreateString(ReadOnlySpan<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreateArrayBuffer,
                str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                return @string;
            }
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreateArrayBuffer,
                in str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                return @string;
            }
        }

        public override char ExtractChar()
        {
            Window.winch(this.WindowBaseSafeHandle, out TSingleByte ch);

            return (char)SingleByteCharFactory<TSingleByte>._Instance.GetByte(ch);
        }

        public override char ExtractChar(INCursesChar ch)
        {
            return (char)SingleByteCharFactory<TSingleByte>._Instance.GetByte(VerifyChar(ch));
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

            return (char)SingleByteCharFactory<TSingleByte>._Instance.GetByte(ch);
        }

        public override char ExtractChar(out ulong attrs, out ushort pair)
        {
            Window.winch(this.WindowBaseSafeHandle, out TSingleByte ch);

            attrs = ch.Attributes;
            pair = ch.ColorPair;

            return (char)SingleByteCharFactory<TSingleByte>._Instance.GetByte(ch);
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out ushort pair)
        {
            Window.mvwinch(this.WindowBaseSafeHandle, nline, ncol, out TSingleByte ch);

            attrs = ch.Attributes;
            pair = ch.ColorPair;

            return (char)SingleByteCharFactory<TSingleByte>._Instance.GetByte(ch);
        }

        public override string ExtractString()
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeEmptyString(
                CharFactory<TChar>._CreatePooledBuffer,
                Constants.MAX_STRING_LENGTH,
                out CharString<TChar> @string))
            {
                Window.winnstr(this.WindowBaseSafeHandle, ref @string, @string.CharLength, out int read);
                return @string.ToString();
            }
        }

        public override string ExtractString(int maxChars, out int read)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeEmptyString(
                CharFactory<TChar>._CreatePooledBuffer,
                maxChars,
                out CharString<TChar> @string))
            {
                Window.winnstr(this.WindowBaseSafeHandle, ref @string, maxChars, out read);
                return @string.ToString();
            }
        }

        public override string ExtractString(int nline, int ncol)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeEmptyString(
                CharFactory<TChar>._CreatePooledBuffer,
                Constants.MAX_STRING_LENGTH,
                out CharString<TChar> @string))
            {
                Window.mvwinnstr(this.WindowBaseSafeHandle, nline, ncol, ref @string, @string.CharLength, out int read);
                return @string.ToString();
            }
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeEmptyString(
                CharFactory<TChar>._CreatePooledBuffer,
                maxChars,
                out CharString<TChar> @string))
            {
                Window.mvwinnstr(this.WindowBaseSafeHandle, nline, ncol, ref @string, maxChars, out read);
                return @string.ToString();
            }
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeEmptyString(
                SingleByteCharFactory<TSingleByte>._CreateArrayBuffer,
                Constants.MAX_STRING_LENGTH,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.winchstr(this.WindowBaseSafeHandle, ref @string, out int read);
                charsWithAttributes = @string;
            }
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeEmptyString(
                SingleByteCharFactory<TSingleByte>._CreateArrayBuffer,
                maxChars,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.winchnstr(this.WindowBaseSafeHandle, ref @string, maxChars, out int read);
                charsWithAttributes = @string;
            }
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeEmptyString(
                SingleByteCharFactory<TSingleByte>._CreateArrayBuffer,
                Constants.MAX_STRING_LENGTH,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.mvwinchstr(this.WindowBaseSafeHandle, nline, ncol, ref @string, out int read);
                charsWithAttributes = @string;
            }
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeEmptyString(
                SingleByteCharFactory<TSingleByte>._CreateArrayBuffer,
                maxChars,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.mvwinchnstr(this.WindowBaseSafeHandle, nline, ncol, ref @string, maxChars, out int read);
                charsWithAttributes = @string;
            }
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
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            TSingleByte sch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch);
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
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            TSingleByte sch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch);
            Window.mvwinsch(this.WindowBaseSafeHandle, nline, ncol, in sch);
        }

        public override void Insert(char ch, ulong attrs, ushort pair)
        {
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            TSingleByte sch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch, attrs, pair);
            Window.winsch(this.WindowBaseSafeHandle, in sch);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, ushort pair)
        {
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            TSingleByte sch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch, attrs, pair);
            Window.mvwinsch(this.WindowBaseSafeHandle, nline, ncol, in sch);
        }

        public override void Insert(string str)
        {
            this.Insert(str.AsSpan());
        }

        public override void Insert(ReadOnlySpan<char> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                str,
                out CharString<TChar> @string))
            {
                Window.winsnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Insert(int nline, int ncol, string str)
        {
            this.Insert(nline, ncol, str.AsSpan());
        }

        public override void Insert(int nline, int ncol, ReadOnlySpan<char> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                str,
                out CharString<TChar> @string))
            {
                Window.mvwinsnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Insert(string str, ulong attrs, ushort pair)
        {
            this.Insert(str.AsSpan(), attrs, pair);
        }

        public override void Insert(ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                Span<TSingleByte> buffer = bufferState.Memory.Span;

                for(int i = buffer.Length - 2; i >= 0; i--) // -2 for null termination
                {
                    Window.winsch(this.WindowBaseSafeHandle, buffer[i]);
                }
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
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeEmptyString(
                CharFactory<TChar>._CreatePooledBuffer,
                Constants.MAX_STRING_LENGTH,
                out CharString<TChar> @string))
            {
                Window.wgetstr(this.WindowBaseSafeHandle, ref @string);
                return @string.ToString();
            }
        }

        public override string ReadLine(int nline, int ncol)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeEmptyString(
                CharFactory<TChar>._CreatePooledBuffer,
                Constants.MAX_STRING_LENGTH,
                out CharString<TChar> @string))
            {
                Window.mvwgetstr(this.WindowBaseSafeHandle, nline, ncol, ref @string);
                return @string.ToString();
            }
        }

        public override string ReadLine(int length)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeEmptyString(
                CharFactory<TChar>._CreatePooledBuffer,
                length,
                out CharString<TChar> @string))
            {
                Window.wgetnstr(this.WindowBaseSafeHandle, ref @string, length);
                return @string.ToString();
            }
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeEmptyString(
                CharFactory<TChar>._CreatePooledBuffer,
                length,
                out CharString<TChar> @string))
            {
                Window.mvwgetnstr(this.WindowBaseSafeHandle, nline, ncol, ref @string, length);
                return @string.ToString();
            }
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

            this.Advance(str);
        }

        public override void Write(string str)
        {
            this.Write(str.AsSpan());
        }

        public override void Write(ReadOnlySpan<char> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                str,
                out CharString<TChar> @string))
            {
                Window.waddnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Write(ReadOnlySpan<byte> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                str,
                out CharString<TChar> @string))
            {
                Window.waddnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Write(in ReadOnlySequence<byte> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                in str,
                out CharString<TChar> @string))
            {
                Window.waddnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);
            }
        }

        public override void Write(string str, ulong attrs, ushort pair)
        {
            this.Write(str.AsSpan(), attrs, pair);
        }

        public override void Write(ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.waddchnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(ReadOnlySpan<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.waddchnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(in ReadOnlySequence<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreatePooledBuffer,
                in str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.waddchnstr(this.WindowBaseSafeHandle, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(int nline, int ncol, string str)
        {
            this.Write(nline, ncol, str.AsSpan());
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<char> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                str,
                out CharString<TChar> @string))
            {
                Window.mvwaddnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                str,
                out CharString<TChar> @string))
            {
                Window.mvwaddnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                in str,
                out CharString<TChar> @string))
            {
                Window.mvwaddnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);
            }
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, ushort pair)
        {
            this.Write(nline, ncol, str.AsSpan(), attrs, pair);
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<char> str, ulong attrs, ushort pair)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.mvwaddchnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreatePooledBuffer,
                str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.mvwaddchnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str, ulong attrs, ushort pair)
        {
            using (BufferState<TSingleByte> bufferState = SingleByteCharFactory<TSingleByte>._Instance.GetNativeString(
                SingleByteCharFactory<TSingleByte>._CreatePooledBuffer,
                in str,
                attrs,
                pair,
                out SingleByteCharString<TSingleByte> @string))
            {
                Window.mvwaddchnstr(this.WindowBaseSafeHandle, nline, ncol, in @string, @string.CharLength);

                this.Advance(@string);
            }
        }

        public override void Write(char ch)
        {
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            TSingleByte sch = CharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch);
            Window.waddch(this.WindowBaseSafeHandle, in sch);
        }

        public override void Write(char ch, ulong attrs, ushort pair)
        {
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            TSingleByte sch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch, attrs, pair);
            Window.waddch(this.WindowBaseSafeHandle, in sch);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            TSingleByte sch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch);
            Window.mvwaddch(this.WindowBaseSafeHandle, nline, ncol, in sch);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, ushort pair)
        {
            if (ch > sbyte.MaxValue)
            {
                throw _RangeException;
            }

            TSingleByte sch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeChar((byte)ch, attrs, pair);
            Window.mvwaddch(this.WindowBaseSafeHandle, nline, ncol, in sch);
        }

        public override void Write(ReadOnlySpan<byte> str, Encoding encoding)
        {
            throw _MultiByteException;
        }

        public override void Write(in ReadOnlySequence<byte> str, Encoding encoding)
        {
            throw _MultiByteException;
        }

        public override void Write(ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            throw _MultiByteException;
        }

        public override void Write(in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            throw _MultiByteException;
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str, Encoding encoding)
        {
            throw _MultiByteException;
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str, Encoding encoding)
        {
            throw _MultiByteException;
        }

        public override void Write(int nline, int ncol, ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            throw _MultiByteException;
        }

        public override void Write(int nline, int ncol, in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            throw _MultiByteException;
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

        public override void Write(string str, int maxLength)
        {
            this.Write(str.AsSpan(), maxLength);
        }

        public override void Write(ReadOnlySpan<char> str, int maxLength)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                str,
                out CharString<TChar> @string))
            {
                Window.waddnstr(this.WindowBaseSafeHandle, in @string, maxLength);
            }
        }

        public override void Write(ReadOnlySpan<byte> str, int maxLength)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                str,
                out CharString<TChar> @string))
            {
                Window.waddnstr(this.WindowBaseSafeHandle, in @string, maxLength);
            }
        }

        public override void Write(in ReadOnlySequence<byte> str, int maxLength)
        {
            using (BufferState<TChar> bufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                in str,
                out CharString<TChar> @string))
            {
                Window.waddnstr(this.WindowBaseSafeHandle, in @string, maxLength);
            }
        }

        public override void Write(ReadOnlySpan<byte> str, int maxLength, Encoding encoding)
        {
            throw _MultiByteException;
        }

        public override void Write(in ReadOnlySequence<byte> str, int maxLength, Encoding encoding)
        {
            throw _MultiByteException;
        }

        public override ICharString CreateString(in ReadOnlySequence<byte> str, Encoding encoding)
        {
            throw _MultiByteException;
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs)
        {
            throw _MultiByteException;
        }

        public override INCursesCharString CreateString(in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort pair)
        {
            throw _MultiByteException;
        }
    }
}
