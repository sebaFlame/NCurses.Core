using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.Wrappers
{
    public interface INativeStdScrWrapper<TMultiByte, TMultiByteString, TWideChar, TWideCharString, TSingleByte, TSingleByteString, TChar, TCharString, TMouseEvent>
            : INativeStdScrMultiByte<TMultiByte, TMultiByteString>,
            INativeStdScrWideChar<TWideChar, TWideCharString>,
            INativeStdScrSingleByte<TSingleByte, TSingleByteString>,
            INativeStdScrChar<TChar, TCharString>
        where TMultiByte : IMultiByteNCursesChar
        where TMultiByteString : IMultiByteNCursesCharString
        where TWideChar : IMultiByteChar
        where TWideCharString : IMultiByteCharString
        where TSingleByte : ISingleByteNCursesChar
        where TSingleByteString : ISingleByteNCursesCharString
        where TChar : ISingleByteChar
        where TCharString : ISingleByteCharString
        where TMouseEvent : IMEVENT
    {
        void attroff(int attrs);
        void attron(int attrs);
        void attrset(int attrs);
        void clear();
        void clrtobot();
        void clrtoeol();
        void color_set(short pair);
        void delch();
        void deleteln();
        void erase();
        int getch();
        bool getch(out char ch, out Key key);
        void insdelln(int n);
        void insertln();
        void move(int y, int x);
        void mvdelch(int y, int x);
        int mvgetch(int y, int x);
        bool mvgetch(int y, int x, out char ch, out Key key);
        void refresh();
        void scrl(int n);
        void setscrreg(int top, int bot);
        void standend();
        void standout();
        void timeout(int delay);
    }
}
