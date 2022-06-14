using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.WideChar
{
    public interface INativeStdScrWideChar<TChar, TCharString>
        where TChar : IMultiByteChar
        where TCharString : IMultiByteCharString
    {
        void addnwstr(in TCharString wstr, int n);
        void addwstr(in TCharString wstr);
        bool get_wch(out TChar wch, out Key key);
        void get_wstr(ref TCharString wstr);
        void getn_wstr(ref TCharString wstr, int n);
        void innwstr(ref TCharString wstr, int n, out int read);
        void ins_nwstr(in TCharString wstr, int n);
        void ins_wstr(in TCharString wstr);
        void inwstr(ref TCharString wstr);
        void mvaddnwstr(int y, int x, in TCharString wstr, int n);
        void mvaddwstr(int y, int x, in TCharString wstr);
        bool mvget_wch(int y, int x, out TChar wch, out Key key);
        void mvget_wstr(int y, int x, ref TCharString wstr);
        void mvgetn_wstr(int y, int x, ref TCharString wstr, int n);
        void mvinnwstr(int y, int x, ref TCharString wstr, int n, out int read);
        void mvins_nwstr(int y, int x, in TCharString wstr, int n);
        void mvins_wstr(int y, int x, in TCharString wstr);
        void mvinwstr(int y, int x, ref TCharString wstr);
    }
}
