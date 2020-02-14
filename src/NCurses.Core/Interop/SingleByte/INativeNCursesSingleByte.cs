using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INativeNCursesSingleByte<TChar, TCharString, TMouseEvent>
        where TChar : ISingleByteNCursesChar
        where TCharString : ISingleByteNCursesCharString
        where TMouseEvent : IMEVENT
    {
        void getmouse(out TMouseEvent ev);
        ulong mousemask(ulong newmask, out ulong oldmask);
        ulong slk_attr();
        void slk_attr_off(ulong attrs);
        void slk_attr_on(ulong attrs);
        void slk_attr_set(ulong attrs, short color_pair);
        void slk_attroff(ulong attrs);
        void slk_attron(ulong attrs);
        void slk_attrset(ulong attrs);
        ulong term_attrs();
        ulong termattrs();
        string unctrl(in TChar ch);
        void ungetmouse(in TMouseEvent ev);
        void vid_attr(ulong attrs, short pair);
        void vid_puts(ulong attrs, short pair, Func<int, int> putc);
        void vidattr(ulong attrs);
        void vidputs(ulong attrs, Func<int, int> putc);
    }
}
