using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INativeScreenSingleByte<TChar, TCharString>
        where TChar : ISingleByteNCursesChar
        where TCharString : ISingleByteNCursesCharString
    {
        void getmouse_sp(IntPtr screen, out IMEVENT ev);
        ulong mousemask_sp(IntPtr screen, ulong newmask, out ulong oldmask);
        ulong slk_attr_sp(IntPtr screen);
        void slk_attr_set_sp(IntPtr screen, ulong attrs, short color_pair);
        void slk_attroff_sp(IntPtr screen, ulong attrs);
        void slk_attron_sp(IntPtr screen, ulong attrs);
        void slk_attrset_sp(IntPtr screen, ulong attrs);
        ulong term_attrs_sp(IntPtr screen);
        ulong termattrs_sp(IntPtr screen);
        void ungetmouse_sp(IntPtr screen, in IMEVENT ev);

        void vid_attr_sp(IntPtr screen, ulong attrs, short pair);
        void vid_puts_sp(IntPtr screen, ulong attrs, short pair, Func<int, int> putc);
        void vidattr_sp(IntPtr screen, ulong attrs);
        void vidputs_sp(IntPtr screen, ulong attrs, Func<int, int> putc);
    }
}
