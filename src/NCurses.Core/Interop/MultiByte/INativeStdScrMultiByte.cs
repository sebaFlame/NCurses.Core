using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativeStdScrMultiByte<TChar, TCharString>
        where TChar : IMultiByteNCursesChar
        where TCharString : IMultiByteNCursesCharString
    {
        void add_wch(in TChar wch);
        void add_wchnstr(in TCharString wchStr, int n);
        void add_wchstr(in TCharString wchStr);
        void bkgrnd(in TChar wch);
        void bkgrndset(in TChar wch);
        void border_set(
            in TChar ls,
            in TChar rs,
            in TChar ts,
            in TChar bs,
            in TChar tl,
            in TChar tr,
            in TChar bl,
            in TChar br);
        void echo_wchar(in TChar wch);
        void getbkgrnd(out TChar wch);
        void hline_set(in TChar wch, int n);
        void in_wch(out TChar wch);
        void in_wchnstr(ref TCharString wchStr, int n);
        void in_wchstr(ref TCharString wch);
        void ins_wch(in TChar wch);
        void mvadd_wch(int y, int x, in TChar wch);
        void mvadd_wchnstr(int y, int x, in TCharString wchStr, int n);
        void mvadd_wchstr(int y, int x, in TCharString wchStr);
        void mvhline_set(int y, int x, in TChar wch, int n);
        void mvin_wch(int y, int x, out TChar wch);
        void mvin_wchnstr(int y, int x, ref TCharString wchStr, int n);
        void mvin_wchstr(int y, int x, ref TCharString wchStr);
        void mvins_wch(int y, int x, in TChar wch);
        void mvvline_set(int y, int x, in TChar wch, int n);
        void vline_set(in TChar wch, int n);
    }
}
