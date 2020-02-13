using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.Char
{
    public interface INativeNCursesChar<TChar, TCharString>
        where TChar : IChar
        where TCharString : ICharString
    {
        TCharString curses_version();
        void define_key(in TCharString definition, int keycode);
        TChar erasechar();
        int key_defined(in TCharString definition);
        TCharString keybound(int keycode, int count);
        TCharString keyname(int c);
        TChar killchar();
        TCharString longname();
        void putp(in TCharString str);
        void scr_dump(in TCharString filename);
        void scr_init(in TCharString filename);
        void scr_restore(in TCharString filename);
        void scr_set(in TCharString filename);
        TCharString slk_label(int labnum);
        void slk_set(int labnum, in TCharString label, int fmt);
        TCharString termname();
        int tigetflag(in TCharString capname);
        int tigetnum(in TCharString capname);
        int tigetstr(in TCharString capname);
    }
}
