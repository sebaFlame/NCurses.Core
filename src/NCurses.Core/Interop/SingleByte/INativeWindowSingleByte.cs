using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INativeWindowSingleByte<TChar, TCharString>
        where TChar : ISingleByteNCursesChar
        where TCharString : ISingleByteNCursesCharString
    {
        void box(WindowBaseSafeHandle window, in TChar verch, in TChar horch);
        TChar getbkgd(WindowBaseSafeHandle window);
        void mvwaddch(WindowBaseSafeHandle window, int y, int x, in TChar ch);
        void mvwaddchnstr(WindowBaseSafeHandle window, int y, int x, in TCharString chstr, int n);
        void mvwaddchstr(WindowBaseSafeHandle window, int y, int x, in TCharString chstr);
        void mvwchgat(WindowBaseSafeHandle window, int y, int x, int number, ulong attrs, short pair);
        void mvwhline(WindowBaseSafeHandle window, int y, int x, in TChar ch, int count);
        void mvwinch(WindowBaseSafeHandle window, int y, int x, out TChar ch);
        void mvwinchnstr(WindowBaseSafeHandle window, int y, int x, ref TCharString chStr, int count, out int read);
        void mvwinchstr(WindowBaseSafeHandle window, int y, int x, ref TCharString chStr, out int read);
        void mvwinsch(WindowBaseSafeHandle window, int y, int x, in TChar ch);
        void mvwvline(WindowBaseSafeHandle window, int y, int x, in TChar ch, int n);
        void waddch(WindowBaseSafeHandle window, in TChar ch);
        void waddchnstr(WindowBaseSafeHandle window, in TCharString chstr, int number);
        void waddchstr(WindowBaseSafeHandle window, in TCharString chstr);
        void wattr_get(WindowBaseSafeHandle window, out ulong attrs, out short pair);
        void wattr_off(WindowBaseSafeHandle window, ulong attrs);
        void wattr_on(WindowBaseSafeHandle window, ulong attrs);
        void wattr_set(WindowBaseSafeHandle window, ulong attrs, short pair);
        void wbkgd(WindowBaseSafeHandle window, in TChar bkgd);
        void wbkgdset(WindowBaseSafeHandle window, in TChar bkgd);
        void wborder(WindowBaseSafeHandle window, in TChar ls, in TChar rs, in TChar ts, in TChar bs, in TChar tl, in TChar tr, in TChar bl, in TChar br);
        void wchgat(WindowBaseSafeHandle window, int number, ulong attrs, short pair);
        void wechochar(WindowBaseSafeHandle window, in TChar ch);
        void whline(WindowBaseSafeHandle window, in TChar ch, int count);
        void winch(WindowBaseSafeHandle window, out TChar ch);
        void winchnstr(WindowBaseSafeHandle window, ref TCharString txt, int count, out int read);
        void winchstr(WindowBaseSafeHandle window, ref TCharString txt, out int read);
        void winsch(WindowBaseSafeHandle window, in TChar ch);
        void wvline(WindowBaseSafeHandle window, in TChar ch, int n);
    }
}
