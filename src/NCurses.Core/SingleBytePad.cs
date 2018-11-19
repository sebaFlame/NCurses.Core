using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core
{
    public class SingleBytePad : Pad
    {
        public override INCursesChar BackGround { get => this.Window.BackGround; set => this.Window.BackGround = value; }
        public override INCursesChar InsertBackGround { get => this.Window.InsertBackGround; set => this.Window.InsertBackGround = value; }

        internal SingleBytePad(IntPtr windowPtr, bool ownsHandle = true, bool initizalize = true)
            : base(windowPtr, ownsHandle, initizalize)
        {
            this.Window = new SingleByteWindow(this.WindowPtr, false);
        }

        internal SingleBytePad(SingleByteWindow window)
            : base(window.WindowPtr, true)
        {
            this.Window = window;
        }

        public SingleBytePad(int nlines, int ncols)
            : base(NativeNCurses.newpad(nlines, ncols))
        {
            this.Window = new SingleByteWindow(this.WindowPtr, false);
        }

        public override void Echo(char ch)
        {
            SingleByteCharFactory.Instance.GetNativeChar(ch, out ISingleByteChar sch);
            NativePad.pechochar(this.WindowPtr, sch);
        }

        public override Window Duplicate()
        {
            return new SingleBytePad(new SingleByteWindow(NativeNCurses.dupwin(this.WindowPtr), false));
        }

        public override void Box(in INCursesChar verticalChar, in INCursesChar horizontalChar)
        {
            this.Window.Box(verticalChar, horizontalChar);
        }

        public override void Box()
        {
            this.Window.Box();
        }

        public override Window ToSingleByteWindow()
        {
            return this;
        }

        public override Window ToMultiByteWindow()
        {
            if (!NCurses.UnicodeSupported)
                throw new NotSupportedException("Unicode not supported");

            return new MultiBytePad(this.WindowPtr, false);
        }

        public override void Write(in INCursesChar ch)
        {
            this.Window.Write(ch);
        }

        public override void Write(in INCursesCharString str)
        {
            this.Window.Write(str);
        }

        public override void Write(string str)
        {
            this.Window.Write(str);
        }

        public override void Write(string str, ulong attrs, short pair)
        {
            this.Window.Write(str, attrs, pair);
        }

        public override void Write(int nline, int ncol, string str)
        {
            this.Window.Write(nline, ncol, str);
        }

        public override void Write(int nline, int ncol, string str, ulong attrs, short pair)
        {
            this.Window.Write(nline, ncol, str, attrs, pair);
        }

        public override void Write(char ch)
        {
            this.Window.Write(ch);
        }

        public override void Write(char ch, ulong attrs, short pair)
        {
            this.Window.Write(ch, attrs, pair);
        }

        public override void Write(int nline, int ncol, char ch)
        {
            this.Window.Write(nline, ncol, ch);
        }

        public override void Write(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            this.Window.Write(nline, ncol, ch, attrs, pair);
        }

        public override void Write(byte[] str, Encoding encoding)
        {
            this.Window.Write(str, encoding);
        }

        public override void Write(byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            this.Window.Write(str, encoding, attrs, pair);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding)
        {
            this.Window.Write(nline, ncol, str, encoding);
        }

        public override void Write(int nline, int ncol, byte[] str, Encoding encoding, ulong attrs, short pair)
        {
            this.Window.Write(nline, ncol, str, encoding, attrs, pair);
        }

        public override bool ReadKey(out char ch, out Key key)
        {
            return this.Window.ReadKey(out ch, out key);
        }

        public override bool ReadKey(int nline, int ncol, out char ch, out Key key)
        {
            return this.Window.ReadKey(nline, ncol, out ch, out key);
        }

        public override string ReadLine()
        {
            return this.Window.ReadLine();
        }

        public override string ReadLine(int nline, int ncol)
        {
            return this.Window.ReadLine(nline, ncol);
        }

        public override string ReadLine(int length)
        {
            return this.Window.ReadLine(length);
        }

        public override string ReadLine(int nline, int ncol, int length)
        {
            return this.Window.ReadLine(nline, ncol, length);
        }

        public override void Insert(char ch)
        {
            this.Window.Insert(ch);
        }

        public override void Insert(int nline, int ncol, char ch)
        {
            this.Window.Insert(nline, ncol, ch);
        }

        public override void Insert(char ch, ulong attrs, short pair)
        {
            this.Window.Insert(ch, attrs, pair);
        }

        public override void Insert(int nline, int ncol, char ch, ulong attrs, short pair)
        {
            this.Window.Insert(nline, ncol, ch, attrs, pair);
        }

        public override void Insert(string str)
        {
            this.Window.Insert(str);
        }

        public override void Insert(int nline, int ncol, string str)
        {
            this.Window.Insert(nline, ncol, str);
        }

        public override void ExtractChar(out INCursesChar ch)
        {
            this.Window.ExtractChar(out ch);
        }

        public override char ExtractChar()
        {
            return this.Window.ExtractChar();
        }

        public override char ExtractChar(int nline, int ncol)
        {
            return this.Window.ExtractChar(nline, ncol);
        }

        public override void ExtractChar(int nline, int ncol, out INCursesChar ch)
        {
            this.Window.ExtractChar(nline, ncol, out ch);
        }

        public override char ExtractChar(out ulong attrs, out short pair)
        {
            return this.Window.ExtractChar(out attrs, out pair);
        }

        public override char ExtractChar(int nline, int ncol, out ulong attrs, out short pair)
        {
            return this.Window.ExtractChar(nline, ncol, out attrs, out pair);
        }

        public override string ExtractString()
        {
            return this.Window.ExtractString();
        }

        public override string ExtractString(int maxChars, out int read)
        {
            return this.Window.ExtractString(maxChars, out read);
        }

        public override string ExtractString(int nline, int ncol)
        {
            return this.Window.ExtractString(nline, ncol);
        }

        public override string ExtractString(int nline, int ncol, int maxChars, out int read)
        {
            return this.Window.ExtractString(nline, ncol, maxChars, out read);
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes)
        {
            this.Window.ExtractString(out charsWithAttributes);
        }

        public override void ExtractString(out INCursesCharString charsWithAttributes, int maxChars)
        {
            this.Window.ExtractString(out charsWithAttributes, maxChars);
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes)
        {
            this.Window.ExtractString(nline, ncol, out charsWithAttributes);
        }

        public override void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars)
        {
            this.Window.ExtractString(nline, ncol, out charsWithAttributes, maxChars);
        }

        public override void CreateChar(char ch, out INCursesChar chRet)
        {
            this.Window.CreateChar(ch, out chRet);
        }

        public override void CreateChar(char ch, ulong attrs, out INCursesChar chRet)
        {
            this.Window.CreateChar(ch, attrs, out chRet);
        }

        public override void CreateChar(char ch, ulong attrs, short pair, out INCursesChar chRet)
        {
            this.Window.CreateChar(ch, attrs, pair, out chRet);
        }

        public override void CreateString(string str, out INCursesCharString chStr)
        {
            this.Window.CreateString(str, out chStr);
        }

        public override void CreateString(string str, ulong attrs, out INCursesCharString chStr)
        {
            this.Window.CreateString(str, attrs, out chStr);
        }

        public override void CreateString(string str, ulong attrs, short pair, out INCursesCharString chStr)
        {
            this.Window.CreateString(str, attrs, pair, out chStr);
        }

        public override void Border(in INCursesChar l, in INCursesChar rs, in INCursesChar ts, in INCursesChar bs, in INCursesChar tl, in INCursesChar tr, in INCursesChar bl, in INCursesChar br)
        {
            this.Window.Border(l, rs, ts, bs, tl, tr, bl, br);
        }

        public override void Border()
        {
            this.Window.Border();
        }

        public override void HorizontalLine(in INCursesChar lineChar, int length)
        {
            this.Window.HorizontalLine(lineChar, length);
        }

        public override void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            this.Window.HorizontalLine(nline, ncol, lineChar, length);
        }

        public override void VerticalLine(in INCursesChar lineChar, int length)
        {
            this.Window.VerticalLine(lineChar, length);
        }

        public override void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length)
        {
            this.Window.VerticalLine(nline, ncol, lineChar, length);
        }
    }
}
