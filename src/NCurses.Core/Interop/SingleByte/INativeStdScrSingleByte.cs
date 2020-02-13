using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INativeStdScrSingleByte<TChar, TCharString>
        where TChar : ISingleByteNCursesChar
        where TCharString : ISingleByteNCursesCharString
    {
        void addch(in TChar ch);
        void addchnstr(in TCharString txt, int number);
        void addchstr(in TCharString txt);
        void attr_get(out ulong attrs, out short pair);
        void attr_off(ulong attrs);
        void attr_on(ulong attrs);
        void attr_set(ulong attrs, short pair);
        void bkgd(in TChar bkgd);
        void bkgdset(in TChar bkgd);
        void border(
            in TChar ls, 
            in TChar rs, 
            in TChar ts, 
            in TChar bs, 
            in TChar tl, 
            in TChar tr, 
            in TChar bl, 
            in TChar br);
        void chgat(int number, ulong attrs, short pair);
        void echochar(in TChar ch);
        void hline(in TChar ch, int count);
        void inch(out TChar ch);
        void inchnstr(ref TCharString txt, int count, out int read);
        void inchstr(ref TCharString txt, out int read);
        void insch(in TChar ch);
        void mvaddch(int y, int x, in TChar ch);
        void mvaddchnstr(int y, int x, in TCharString chstr, int n);
        void mvaddchstr(int y, int x, in TCharString chStr);
        void mvchgat(int y, int x, int number, ulong attrs, short pair);
        void mvhline(int y, int x, in TChar ch, int count);
        void mvinch(int y, int x, out TChar ch);
        void mvinchnstr(int y, int x, ref TCharString chstr, int count, out int read);
        void mvinchstr(int y, int x, ref TCharString chstr, out int read);
        void mvinsch(int y, int x, in TChar ch);
        void mvvline(int y, int x, in TChar ch, int n);
        void vline(in TChar ch, int n);
    }
}
