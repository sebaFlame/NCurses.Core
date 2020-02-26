using System;
using System.Text;
using System.Buffers;

using NCurses.Core.Interop;

namespace NCurses.Core
{
    public interface IWindow : IEquatable<IWindow>, IDisposable
    {
        INCursesChar BackGround { get; set; }
        bool Blocking { get; set; }
        int CursorColumn { get; }
        int CursorLine { get; }
        INCursesChar InsertBackGround { get; set; }
        bool KeyPad { get; set; }
        bool Meta { set; }
        int MaxColumn { get; }
        int MaxLine { get; }
        bool NoTimeout { get; set; }
        bool Scroll { get; set; }
        bool UseHwInsDelLine { get; set; }
        bool HasUnicodeSupport { get; }

        void AttributesOn(ulong attrs);
        void AttributesOff(ulong attrs);
        void Border();
        void Border(in INCursesChar l, in INCursesChar rs, in INCursesChar ts, in INCursesChar bs, in INCursesChar tl, in INCursesChar tr, in INCursesChar bl, in INCursesChar br);
        void Clear();
        void ClearToBottom();
        void ClearToEol();

        IWindow SubWindow(int nlines, int ncols, int begin_y, int begin_x);
        IWindow DerWindow(int nlines, int ncols, int begin_y, int begin_x);

        IWindow ToSingleByteWindow();
        IWindow ToMultiByteWindow();

        void ResizeWindow(int nlines, int ncols);
        void MoveWindow(int y, int x);

        INCursesChar CreateChar(char ch);
        INCursesChar CreateChar(char ch, ulong attrs);
        INCursesChar CreateChar(char ch, ulong attrs, short pair);
        INCursesCharString CreateString(string str);
        INCursesCharString CreateString(ReadOnlySpan<char> str);
        INCursesCharString CreateString(string str, ulong attrs);
        INCursesCharString CreateString(ReadOnlySpan<char> str, ulong attrs);
        INCursesCharString CreateString(string str, ulong attrs, short pair);
        INCursesCharString CreateString(ReadOnlySpan<char> str, ulong attrs, short pair);

        void CurrentAttributesAndColor(out ulong attrs, out short colorPair);
        void EnableAttributesAndColor(ulong attrs, short colorPair);
        void EnableColor(short colorPair);
        void Erase();
        char ExtractChar();
        void ExtractChar(out INCursesChar ch);
        char ExtractChar(int nline, int ncol);
        void ExtractChar(int nline, int ncol, out INCursesChar ch);
        char ExtractChar(int nline, int ncol, out ulong attrs, out short pair);
        char ExtractChar(out ulong attrs, out short pair);
        string ExtractString();
        string ExtractString(int maxChars, out int read);
        string ExtractString(int nline, int ncol);
        string ExtractString(int nline, int ncol, int maxChars, out int read);
        void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes);
        void ExtractString(int nline, int ncol, out INCursesCharString charsWithAttributes, int maxChars);
        void ExtractString(out INCursesCharString charsWithAttributes);
        void ExtractString(out INCursesCharString charsWithAttributes, int maxChars);

        //void ExtractString(int nline, int ncol, ArrayPool<byte> arrayPool, out byte[] buffer, out INCursesCharString charsWithAttributes);
        //void ExtractString(int nline, int ncol, ArrayPool<byte> arrayPool, out byte[] buffer, out INCursesCharString charsWithAttributes, int maxChars);
        //void ExtractString(ArrayPool<byte> arrayPool, out byte[] buffer, out INCursesCharString charsWithAttributes);
        //void ExtractString(ArrayPool<byte> arrayPool, out byte[] buffer, out INCursesCharString charsWithAttributes, int maxChars);

        void HorizontalLine(in INCursesChar lineChar, int length);
        void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length);
        void Insert(char ch);
        void Insert(in INCursesChar ch);
        void Insert(int nline, int ncol, in INCursesChar ch);
        void Insert(char ch, ulong attrs, short pair);
        void Insert(int nline, int ncol, char ch);
        void Insert(int nline, int ncol, char ch, ulong attrs, short pair);
        void Insert(int nline, int ncol, string str);
        void Insert(int nline, int ncol, ReadOnlySpan<char> str);
        void Insert(string str);
        void Insert(ReadOnlySpan<char> str);
        void Insert(string str, ulong attrs, short pair);
        void Insert(ReadOnlySpan<char> str, ulong attrs, short pair);
        void MoveCursor(int lineNumber, int columnNumber);
        void NoOutRefresh();
        bool ReadKey(out char ch, out Key key);
        bool ReadKey(int nline, int ncol, out char ch, out Key key);
        string ReadLine();
        string ReadLine(int nline, int ncol);
        string ReadLine(int length);
        string ReadLine(int nline, int ncol, int length);
        void Refresh();
        void ScrollWindow(int lines);
        void VerticalLine(in INCursesChar lineChar, int length);
        void VerticalLine(int nline, int ncol, in INCursesChar lineChar, int length);
        void Write(byte[] str, Encoding encoding);
        void Write(byte[] str, Encoding encoding, ulong attrs, short pair);
        void Write(char ch);
        void Write(char ch, ulong attrs, short pair);
        void Write(in INCursesChar ch);
        void Write(in INCursesCharString str);
        void Write(int nline, int ncol, byte[] str, Encoding encoding);
        void Write(int nline, int ncol, byte[] str, Encoding encoding, ulong attrs, short pair);
        void Write(int nline, int ncol, char ch);
        void Write(int nline, int ncol, char ch, ulong attrs, short pair);
        void Write(int nline, int ncol, string str);
        void Write(int nline, int ncol, ReadOnlySpan<char> str);
        void Write(int nline, int ncol, string str, ulong attrs, short pair);
        void Write(int nline, int ncol, ReadOnlySpan<char> str, ulong attrs, short pair);
        void Write(string str);
        void Write(ReadOnlySpan<char> str);
        void Write(string str, ulong attrs, short pair);
        void Write(ReadOnlySpan<char> str, ulong attrs, short pair);
        void Write(int nline, int ncol, in INCursesChar ch);
        void Write(int nline, int ncol, in INCursesCharString str);
        void Put(char ch);
        void Put(Key key);
        void Delete();
        void Delete(int nline, int ncol);
    }
}