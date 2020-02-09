using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.MultiByteString
{
    internal interface IMultiByteStringWrapper<TMultiByteString, TSingleByteString> : INativeWrapper
        where TMultiByteString : unmanaged
        where TSingleByteString : unmanaged
    {
        //int addnwstr(const wchar_t *wstr, int n);
        int addnwstr(in TMultiByteString wstr, int n);
        //int addwstr(const wchar_t *wstr);
        int addwstr(in TMultiByteString wstr);
        //int erasewchar(wchar_t *ch);
        int erasewchar(ref TMultiByteString wch);
        //int get_wch(wint_t *wch);
        int get_wch(ref int wch); //max 4 bytes (gets casted from an integer)
        //int get_wstr(wint_t *wstr);
        int get_wstr(ref int wstr);
        //int getn_wstr(wint_t *wstr, int n);
        int getn_wstr(ref int wstr, int n);
        //int innwstr(wchar_t *wstr, int n);
        int innwstr(ref TMultiByteString wstr, int n);
        //int ins_nwstr(const wchar_t *wstr, int n);
        int ins_nwstr(in TMultiByteString wstr, int n);
        //int ins_wstr(const wchar_t *wstr);
        int ins_wstr(in TMultiByteString wstr);
        //int inwstr(wchar_t *wstr);
        int inwstr(ref TMultiByteString wstr);
        //char *key_name(wchar_t w);
        ref TSingleByteString key_name(TMultiByteString c);
        //int killwchar(wchar_t *ch);
        int killwchar(ref TMultiByteString wch);
        //int mvaddnwstr(int y, int x, const wchar_t *wstr, int n);
        int mvaddnwstr(int y, int x, in TMultiByteString wstr, int n);
        //int mvaddwstr(int y, int x, const wchar_t *wstr);
        int mvaddwstr(int y, int x, in TMultiByteString wstr);
        //int mvwget_wch(WINDOW *win, int y, int x, wint_t *wch);
        int mvget_wch(int y, int x, ref int wch);
        //int mvget_wstr(int y, int x, wint_t *wstr);
        int mvget_wstr(int y, int x, ref int wstr);
        //int mvgetn_wstr(int y, int x, wint_t *wstr, int n);
        int mvgetn_wstr(int y, int x, ref int wstr, int n);
        //int mvinnwstr(int y, int x, wchar_t *wstr, int n);
        int mvinnwstr(int y, int x, ref TMultiByteString wstr, int n);
        //int mvins_nwstr(int y, int x, const wchar_t *wstr, int n);
        int mvins_nwstr(int y, int x, in TMultiByteString wstr, int n);
        //int mvins_wstr(int y, int x, const wchar_t *wstr);
        int mvins_wstr(int y, int x, in TMultiByteString wstr);
        //int mvinwstr(int y, int x, wchar_t *wstr);
        int mvinwstr(int y, int x, ref TMultiByteString wstr);
        //int mvwaddnwstr(WINDOW *win, int y, int x, const wchar_t *wstr, int n);
        int mvwaddnwstr(IntPtr window, int y, int x, in TMultiByteString wstr, int n);
        //int mvwaddwstr(WINDOW *win, int y, int x, const wchar_t *wstr);
        int mvwaddwstr(IntPtr window, int y, int x, in TMultiByteString wstr);
        //int mvwget_wch(WINDOW *win, int y, int x, wint_t *wch);
        int mvwget_wch(IntPtr window, int y, int x, ref int wch);
        //int mvwget_wstr(WINDOW *win, int y, int x, wint_t *wstr);
        int mvwget_wstr(IntPtr window, int y, int x, ref int wstr);
        //int mvwgetn_wstr(WINDOW *win, int y, int x, wint_t *wstr, int n);
        int mvwgetn_wstr(IntPtr window, int y, int x, ref int wstr, int n);
        //int mvwinnwstr(WINDOW *win, int y, int x, wchar_t *wstr, int n);
        int mvwinnwstr(IntPtr window, int y, int x, ref TMultiByteString wstr, int n);
        //int mvwins_nwstr(WINDOW *win, int y, int x, const wchar_t *wstr, int n);
        int mvwins_nwstr(IntPtr window, int y, int x, in TMultiByteString wstr, int n);
        //int mvwins_wstr(WINDOW *win, int y, int x, const wchar_t *wstr);
        int mvwins_wstr(IntPtr window, int y, int x, in TMultiByteString str);
        //int mvwinwstr(WINDOW *win, int y, int x, wchar_t *wstr);
        int mvwinwstr(IntPtr window, int y, int x, ref TMultiByteString wstr);
        //int slk_wset(int labnum, const wchar_t *label, int fmt);
        int slk_wset(int labnum, in TMultiByteString label, int fmt);
        //int unget_wch(const wchar_t wch);
        int unget_wch(TMultiByteString wch);
        //int unget_wch_sp(SCREEN* sp, const wchar_t wch);
        int unget_wch_sp(IntPtr screen, TMultiByteString wch);
        //int waddnwstr(WINDOW *win, const wchar_t *wstr, int n);
        int waddnwstr(IntPtr window, in TMultiByteString wstr, int n);
        //int waddwstr(WINDOW *win, const wchar_t *wstr);
        int waddwstr(IntPtr window, in TMultiByteString wstr);
        //int wget_wch(WINDOW *win, wint_t *wch);
        int wget_wch(IntPtr window, ref int wch);
        //int wget_wstr(WINDOW *win, wint_t *wstr);
        int wget_wstr(IntPtr window, ref int wstr);
        //int wgetn_wstr(WINDOW *win, wint_t *wstr, int n);
        int wgetn_wstr(IntPtr window, ref int wstr, int n);
        //int winnwstr(WINDOW *win, wchar_t *wstr, int n);
        int winnwstr(IntPtr window, ref TMultiByteString str, int n);
        //int winwstr(WINDOW *win, wchar_t *wstr);
        int winwstr(IntPtr window, ref TMultiByteString str);
        //int wins_nwstr(WINDOW *win, const wchar_t *wstr, int n);
        int wins_nwstr(IntPtr window, in TMultiByteString wstr, int n);
        //int wins_wstr(WINDOW *win, const wchar_t *wstr);
        int wins_wstr(IntPtr window, in TMultiByteString wstr);
    }
}
