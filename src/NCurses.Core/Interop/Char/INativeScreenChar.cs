using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.Char
{
    public interface INativeScreenChar<TChar, TCharString>
        where TChar : ISingleByteChar
        where TCharString : ISingleByteCharString
    {
        void define_key_sp(IntPtr screen, in TCharString definition, int keycode);
        TChar erasechar_sp(IntPtr screen);
        int key_defined_sp(IntPtr screen, in TCharString definition);
        TCharString keybound_sp(IntPtr screen, int keycode, int count);
        TCharString keyname_sp(IntPtr screen, int c);
        TChar killchar_sp(IntPtr screen);
        TCharString longname_sp(IntPtr screen);
        void putp_sp(IntPtr screen, in TCharString str);
        void scr_dump_sp(IntPtr screen, in TCharString filename);
        void scr_init_sp(IntPtr screen, in TCharString filename);
        void scr_restore_sp(IntPtr screen, in TCharString filename);
        void scr_set_sp(IntPtr screen, in TCharString filename);
        TCharString slk_label_sp(IntPtr screen, int labnum);
        void slk_set_sp(IntPtr screen, int labnum, in TCharString label, int fmt);

        TCharString termname_sp(IntPtr screen);
        int tigetflag_sp(IntPtr screen, in TCharString capname);
        int tigetnum_sp(IntPtr screen, in TCharString capname);
        int tigetstr_sp(IntPtr screen, in TCharString capname);
    }
}
