using System;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
{
    internal interface IMultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString> : INativeWrapper
        where TMultiByte : unmanaged, IMultiByteChar
        where TMultiByteString : unmanaged
        where TSingleByte: unmanaged, ISingleByteChar
        where TSingleByteString : unmanaged
    {
        //int add_wch( const cchar_t *wch );
        int add_wch(in TMultiByte wch);
        //int add_wchnstr(const cchar_t *wchstr, int n);
        int add_wchnstr(in TMultiByte wchStr, int n);
        //int add_wchstr(const cchar_t *wchstr);
        int add_wchstr(in TMultiByte wchStr);
        //int bkgrnd( const cchar_t *wch);
        int bkgrnd(in TMultiByte wch);
        //void bkgrndset(const cchar_t *wch );
        void bkgrndset(in TMultiByte wch);
        //int border_set(const cchar_t* ls, const cchar_t* rs, const cchar_t* ts, const cchar_t* bs, const cchar_t* tl, const cchar_t* tr, const cchar_t* bl, const cchar_t* br );
        int border_set(in TMultiByte ls, in TMultiByte rs, in TMultiByte ts, in TMultiByte bs, in TMultiByte tl, in TMultiByte tr, in TMultiByte bl, in TMultiByte br);
        //int box_set(WINDOW* win, const cchar_t* verch, const cchar_t* horch);
        int box_set(IntPtr window, in TMultiByte verch, in TMultiByte horch);
        //int echo_wchar( const cchar_t *wch );
        int echo_wchar(in TMultiByte wch);
        //int getbkgrnd(cchar_t *wch);
        int getbkgrnd(ref TMultiByte wch);
        //int getcchar(const cchar_t* wcval, wchar_t *wch, attr_t* attrs, short* color_pair, void* opts );
        int getcchar(in TMultiByte wcval, ref TMultiByteString wch, ref TSingleByte attrs, ref short color_pair, IntPtr opts);
        //int hline_set(const cchar_t* wch, int n);
        int hline_set(in TMultiByte wch, int n);
        //int in_wch(cchar_t *wcval);
        int in_wch(ref TMultiByte wch);
        //int in_wchnstr(cchar_t *wchstr, int n);
        int in_wchnstr(ref TMultiByte wch, int n);
        //int in_wchstr(cchar_t *wchstr);
        int in_wchstr(ref TMultiByte wch);
        //int ins_wch(const cchar_t *wch);
        int ins_wch(in TMultiByte wch);
        //int mvadd_wch( int y, int x, const cchar_t *wch );
        int mvadd_wch(int y, int x, in TMultiByte wch);
        //int mvadd_wchnstr(int y, int x, const cchar_t *wchstr, int n);
        int mvadd_wchnstr(int y, int x, in TMultiByte wchStr, int n);
        //int mvadd_wchstr(int y, int x, const cchar_t *wchstr);
        int mvadd_wchstr(int y, int x, in TMultiByte wchStr);
        //int mvhline_set(int y, int x, const cchar_t* wch, int n);
        int mvhline_set(int y, int x, in TMultiByte wch, int n);
        //int mvin_wch(int y, int x, cchar_t *wcval);
        int mvin_wch(int y, int x, ref TMultiByte wch);
        //int mvin_wchnstr(int y, int x, cchar_t *wchstr, int n);
        int mvin_wchnstr(int y, int x, ref TMultiByte wch, int n);
        //int mvin_wchstr(int y, int x, cchar_t *wchstr);
        int mvin_wchstr(int y, int x, ref TMultiByte wch);
        //int mvins_wch(int y, int x, const cchar_t *wch);
        int mvins_wch(int y, int x, in TMultiByte wch);
        //int mvvline_set(int y, int x, const cchar_t* wch, int n);
        int mvvline_set(int y, int x, in TMultiByte wch, int n);
        //int mvwadd_wch( WINDOW *win, int y, int x, const cchar_t *wch );
        int mvwadd_wch(IntPtr window, int y, int x, in TMultiByte wch);
        //int mvwadd_wchnstr(WINDOW *win, int y, int x, const cchar_t *wchstr, int n);
        int mvwadd_wchnstr(IntPtr window, int y, int x, in TMultiByte wchStr, int n);
        //int mvwadd_wchstr(WINDOW *win, int y, int x, const cchar_t *wchstr);
        int mvwadd_wchstr(IntPtr window, int y, int x, in TMultiByte wchStr);
        //int mvwhline_set(WINDOW* win, int y, int x, const cchar_t* wch, int n);
        int mvwhline_set(IntPtr window, int y, int x, in TMultiByte wch, int n);
        //int mvwin_wch(WINDOW *win, int y, int x, cchar_t *wcval);
        int mvwin_wch(IntPtr window, int y, int x, ref TMultiByte wch);
        //int mvwin_wchnstr(WINDOW *win, int y, int x, cchar_t *wchstr, int n);
        int mvwin_wchnstr(IntPtr window, int y, int x, ref TMultiByte wch, int n);
        //int mvwin_wchnstr(WINDOW *win, int y, int x, cchar_t *wchstr);
        int mvwin_wchstr(IntPtr window, int y, int x, ref TMultiByte wch);
        //int mvwins_wch(WINDOW *win, int y, int x, const cchar_t *wch);
        int mvwins_wch(IntPtr window, int y, int x, in TMultiByte wch);
        //int mvwvline_set(WINDOW* win, int y, int x, const cchar_t* wch, int n);
        int mvwvline_set(IntPtr window, int y, int x, in TMultiByte wch, int n);
        //int pecho_wchar(WINDOW *pad, const cchar_t *wch);
        int pecho_wchar(IntPtr pad, in TMultiByte wch);
        //int setcchar(cchar_t* wcval, const wchar_t* wch, const attr_t attrs, short color_pair, void* opts );
        int setcchar(ref TMultiByte wcval, in TMultiByteString wch, TSingleByte attrs, short color_pair, IntPtr opts);
        //int vline_set(const cchar_t* wch, int n);
        int vline_set(in TMultiByte wch, int n);
        //int wadd_wch( WINDOW *win, const cchar_t *wch );
        int wadd_wch(IntPtr window, in TMultiByte wch);
        //int wadd_wchnstr(WINDOW * win, const cchar_t *wchstr, int n);
        int wadd_wchnstr(IntPtr window, in TMultiByte wchStr, int n);
        //int mvwadd_wchstr(WINDOW *win, int y, int x, const cchar_t *wchstr);
        int wadd_wchstr(IntPtr window, in TMultiByte wchStr);
        //int wbkgrnd( WINDOW *win, const cchar_t *wch);
        int wbkgrnd(IntPtr window, in TMultiByte wch);
        //void wbkgrndset(WINDOW *win, const cchar_t *wch);
        void wbkgrndset(IntPtr window, in TMultiByte wch);
        //int wborder_set(WINDOW* win, const cchar_t* ls, const cchar_t* rs, const cchar_t* ts, const cchar_t* bs, const cchar_t* tl, const cchar_t* tr, const cchar_t* bl, const cchar_t* br);
        int wborder_set(IntPtr window, in TMultiByte ls, in TMultiByte rs, in TMultiByte ts, in TMultiByte bs, in TMultiByte tl, in TMultiByte tr, in TMultiByte bl, in TMultiByte br);
        //int wecho_wchar( WINDOW *win, const cchar_t *wch );
        int wecho_wchar(IntPtr window, in TMultiByte wch);
        //int wgetbkgrnd(WINDOW *win, cchar_t *wch);
        int wgetbkgrnd(IntPtr window, ref TMultiByte wch);
        //int whline_set(WINDOW* win, const cchar_t* wch, int n);
        int whline_set(IntPtr window, in TMultiByte wch, int n);
        //int win_wch(WINDOW* win, cchar_t* wcval);
        int win_wch(IntPtr window, ref TMultiByte wch);
        //int win_wchnstr(WINDOW *win, cchar_t *wchstr, int n);
        int win_wchnstr(IntPtr window, ref TMultiByte wchstr, int n);
        //int win_wchstr(WINDOW *win, cchar_t *wchstr);
        int win_wchstr(IntPtr window, ref TMultiByte wchstr);
        //int wins_wch(WINDOW *win, const cchar_t *wch);
        int wins_wch(IntPtr window, in TMultiByte wch);
        //wchar_t* wunctrl(cchar_t* c);
        ref TMultiByteString wunctrl(in TMultiByte wch);
        //wchar_t *wunctrl_sp(SCREEN* sp, cchar_t *c);
        ref TMultiByteString wunctrl_sp(IntPtr screen, in TMultiByte wch);
        //int wvline_set(WINDOW* win, const cchar_t* wch, int n);
        int wvline_set(IntPtr window, in TMultiByte wch, int n);
    }
}
