using System.Text;
using NCurses.Core.Interop;

namespace NCurses.Core
{
    public interface IWindow
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

        void AttributesOn(ulong attrs);
        void AttributesOff(ulong attrs);
        void Border();
        void Border(in INCursesChar l, in INCursesChar rs, in INCursesChar ts, in INCursesChar bs, in INCursesChar tl, in INCursesChar tr, in INCursesChar bl, in INCursesChar br);
        void Clear();
        void ClearToBottom();
        void ClearToEol();
        void CreateChar(char ch, out INCursesChar chRet);
        void CreateChar(char ch, ulong attrs, out INCursesChar chRet);
        void CreateChar(char ch, ulong attrs, short pair, out INCursesChar chRet);
        void CreateString(string str, out INCursesCharString chStr);
        void CreateString(string str, ulong attrs, out INCursesCharString chStr);
        void CreateString(string str, ulong attrs, short pair, out INCursesCharString chStr);
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
        void HorizontalLine(in INCursesChar lineChar, int length);
        void HorizontalLine(int nline, int ncol, in INCursesChar lineChar, int length);
        void Insert(char ch);
        void Insert(char ch, ulong attrs, short pair);
        void Insert(int nline, int ncol, char ch);
        void Insert(int nline, int ncol, char ch, ulong attrs, short pair);
        void Insert(int nline, int ncol, string str);
        void Insert(string str);
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
        void Write(int nline, int ncol, string str, ulong attrs, short pair);
        void Write(string str);
        void Write(string str, ulong attrs, short pair);
    }
}