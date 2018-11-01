using System;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
{
    internal interface INCursesWrapperWide<TWide, TWideStr, TSMall, TSmallStr> : INativeWrapper
        where TWide : unmanaged, INCursesWCHAR
        where TWideStr : unmanaged
        where TSMall: unmanaged, INCursesSCHAR
        where TSmallStr : unmanaged
    {
        //int add_wch( const cchar_t *wch );
        int add_wch(in TWide wch);
        //int add_wchnstr(const cchar_t *wchstr, int n);
        int add_wchnstr(in TWide wchStr, int n);
        //int add_wchstr(const cchar_t *wchstr);
        int add_wchstr(in TWide wchStr);
        //int bkgrnd( const cchar_t *wch);
        int bkgrnd(in TWide wch);
        //void bkgrndset(const cchar_t *wch );
        void bkgrndset(in TWide wch);
        //int border_set(const cchar_t* ls, const cchar_t* rs, const cchar_t* ts, const cchar_t* bs, const cchar_t* tl, const cchar_t* tr, const cchar_t* bl, const cchar_t* br );
        int border_set(in TWide ls, in TWide rs, in TWide ts, in TWide bs, in TWide tl, in TWide tr, in TWide bl, in TWide br);
        //int box_set(WINDOW* win, const cchar_t* verch, const cchar_t* horch);
        int box_set(IntPtr window, in TWide verch, in TWide horch);
        //int echo_wchar( const cchar_t *wch );
        int echo_wchar(in TWide wch);
        //int getbkgrnd(cchar_t *wch);
        int getbkgrnd(ref TWide wch);
        //int getcchar(const cchar_t* wcval, wchar_t *wch, attr_t* attrs, short* color_pair, void* opts );
        int getcchar(in TWide wcval, ref TWideStr wch, ref TSMall attrs, ref short color_pair, IntPtr opts);
        //int hline_set(const cchar_t* wch, int n);
        int hline_set(in TWide wch, int n);
        //int in_wch(cchar_t *wcval);
        int in_wch(ref TWide wch);
        //int in_wchnstr(cchar_t *wchstr, int n);
        int in_wchnstr(ref TWide wch, int n);
        //int in_wchstr(cchar_t *wchstr);
        int in_wchstr(ref TWide wch);
        //int ins_wch(const cchar_t *wch);
        int ins_wch(in TWide wch);
        //int mvadd_wch( int y, int x, const cchar_t *wch );
        int mvadd_wch(int y, int x, in TWide wch);
        //int mvadd_wchnstr(int y, int x, const cchar_t *wchstr, int n);
        int mvadd_wchnstr(int y, int x, in TWide wchStr, int n);
        //int mvadd_wchstr(int y, int x, const cchar_t *wchstr);
        int mvadd_wchstr(int y, int x, in TWide wchStr);
        //int mvhline_set(int y, int x, const cchar_t* wch, int n);
        int mvhline_set(int y, int x, in TWide wch, int n);
        //int mvin_wch(int y, int x, cchar_t *wcval);
        int mvin_wch(int y, int x, ref TWide wch);
        //int mvin_wchnstr(int y, int x, cchar_t *wchstr, int n);
        int mvin_wchnstr(int y, int x, ref TWide wch, int n);
        //int mvin_wchstr(int y, int x, cchar_t *wchstr);
        int mvin_wchstr(int y, int x, ref TWide wch);
        //int mvins_wch(int y, int x, const cchar_t *wch);
        int mvins_wch(int y, int x, in TWide wch);
        //int mvvline_set(int y, int x, const cchar_t* wch, int n);
        int mvvline_set(int y, int x, in TWide wch, int n);
        //int mvwadd_wch( WINDOW *win, int y, int x, const cchar_t *wch );
        int mvwadd_wch(IntPtr window, int y, int x, in TWide wch);
        //int mvwadd_wchnstr(WINDOW *win, int y, int x, const cchar_t *wchstr, int n);
        int mvwadd_wchnstr(IntPtr window, int y, int x, in TWide wchStr, int n);
        //int mvwadd_wchstr(WINDOW *win, int y, int x, const cchar_t *wchstr);
        int mvwadd_wchstr(IntPtr window, int y, int x, in TWide wchStr);
        //int mvwhline_set(WINDOW* win, int y, int x, const cchar_t* wch, int n);
        int mvwhline_set(IntPtr window, int y, int x, in TWide wch, int n);
        //int mvwin_wch(WINDOW *win, int y, int x, cchar_t *wcval);
        int mvwin_wch(IntPtr window, int y, int x, ref TWide wch);
        //int mvwin_wchnstr(WINDOW *win, int y, int x, cchar_t *wchstr, int n);
        int mvwin_wchnstr(IntPtr window, int y, int x, ref TWide wch, int n);
        //int mvwin_wchnstr(WINDOW *win, int y, int x, cchar_t *wchstr);
        int mvwin_wchstr(IntPtr window, int y, int x, ref TWide wch);
        //int mvwins_wch(WINDOW *win, int y, int x, const cchar_t *wch);
        int mvwins_wch(IntPtr window, int y, int x, in TWide wch);
        //int mvwvline_set(WINDOW* win, int y, int x, const cchar_t* wch, int n);
        int mvwvline_set(IntPtr window, int y, int x, in TWide wch, int n);
        //int pecho_wchar(WINDOW *pad, const cchar_t *wch);
        int pecho_wchar(IntPtr pad, in TWide wch);
        //int setcchar(cchar_t* wcval, const wchar_t* wch, const attr_t attrs, short color_pair, void* opts );
        int setcchar(ref TWide wcval, in TWideStr wch, TSMall attrs, short color_pair, IntPtr opts);
        //int vline_set(const cchar_t* wch, int n);
        int vline_set(in TWide wch, int n);
        //int wadd_wch( WINDOW *win, const cchar_t *wch );
        int wadd_wch(IntPtr window, in TWide wch);
        //int wadd_wchnstr(WINDOW * win, const cchar_t *wchstr, int n);
        int wadd_wchnstr(IntPtr window, in TWide wchStr, int n);
        //int mvwadd_wchstr(WINDOW *win, int y, int x, const cchar_t *wchstr);
        int wadd_wchstr(IntPtr window, in TWide wchStr);
        //int wbkgrnd( WINDOW *win, const cchar_t *wch);
        int wbkgrnd(IntPtr window, in TWide wch);
        //void wbkgrndset(WINDOW *win, const cchar_t *wch);
        void wbkgrndset(IntPtr window, in TWide wch);
        //int wborder_set(WINDOW* win, const cchar_t* ls, const cchar_t* rs, const cchar_t* ts, const cchar_t* bs, const cchar_t* tl, const cchar_t* tr, const cchar_t* bl, const cchar_t* br);
        int wborder_set(IntPtr window, in TWide ls, in TWide rs, in TWide ts, in TWide bs, in TWide tl, in TWide tr, in TWide bl, in TWide br);
        //int wecho_wchar( WINDOW *win, const cchar_t *wch );
        int wecho_wchar(IntPtr window, in TWide wch);
        //int wgetbkgrnd(WINDOW *win, cchar_t *wch);
        int wgetbkgrnd(IntPtr window, ref TWide wch);
        //int whline_set(WINDOW* win, const cchar_t* wch, int n);
        int whline_set(IntPtr window, in TWide wch, int n);
        //int win_wch(WINDOW* win, cchar_t* wcval);
        int win_wch(IntPtr window, ref TWide wch);
        //int win_wchnstr(WINDOW *win, cchar_t *wchstr, int n);
        int win_wchnstr(IntPtr window, ref TWide wchstr, int n);
        //int win_wchstr(WINDOW *win, cchar_t *wchstr);
        int win_wchstr(IntPtr window, ref TWide wchstr);
        //int wins_wch(WINDOW *win, const cchar_t *wch);
        int wins_wch(IntPtr window, in TWide wch);
        //wchar_t* wunctrl(cchar_t* c);
        ref TWideStr wunctrl(in TWide wch);
        //wchar_t *wunctrl_sp(SCREEN* sp, cchar_t *c);
        ref TWideStr wunctrl_sp(IntPtr screen, in TWide wch);
        //int wvline_set(WINDOW* win, const cchar_t* wch, int n);
        int wvline_set(IntPtr window, in TWide wch, int n);
    }
}
