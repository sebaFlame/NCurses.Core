using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.WideChar
{
    internal interface IWideCharWrapper<TWideChar, TChar>
        where TWideChar : unmanaged, IChar
        where TChar : unmanaged, IChar
    {
        //int addnwstr(const wchar_t *wstr, int n);
        int addnwstr(in TWideChar wstr, int n);
        //int addwstr(const wchar_t *wstr);
        int addwstr(in TWideChar wstr);
        //int erasewchar(wchar_t *ch);
        int erasewchar(ref TWideChar wch);
        //int get_wch(wint_t *wch);
        int get_wch(ref int wch); //max 4 bytes (gets casted from an integer)
        //int get_wstr(wint_t *wstr);
        int get_wstr(ref int wstr);
        //int getn_wstr(wint_t *wstr, int n);
        int getn_wstr(ref int wstr, int n);
        //int innwstr(wchar_t *wstr, int n);
        int innwstr(ref TWideChar wstr, int n);
        //int ins_nwstr(const wchar_t *wstr, int n);
        int ins_nwstr(in TWideChar wstr, int n);
        //int ins_wstr(const wchar_t *wstr);
        int ins_wstr(in TWideChar wstr);
        //int inwstr(wchar_t *wstr);
        int inwstr(ref TWideChar wstr);
        //char *key_name(wchar_t w);
        ref TChar key_name(TWideChar c);
        //int killwchar(wchar_t *ch);
        int killwchar(ref TWideChar wch);
        //int mvaddnwstr(int y, int x, const wchar_t *wstr, int n);
        int mvaddnwstr(int y, int x, in TWideChar wstr, int n);
        //int mvaddwstr(int y, int x, const wchar_t *wstr);
        int mvaddwstr(int y, int x, in TWideChar wstr);
        //int mvwget_wch(WINDOW *win, int y, int x, wint_t *wch);
        int mvget_wch(int y, int x, ref int wch);
        //int mvget_wstr(int y, int x, wint_t *wstr);
        int mvget_wstr(int y, int x, ref int wstr);
        //int mvgetn_wstr(int y, int x, wint_t *wstr, int n);
        int mvgetn_wstr(int y, int x, ref int wstr, int n);
        //int mvinnwstr(int y, int x, wchar_t *wstr, int n);
        int mvinnwstr(int y, int x, ref TWideChar wstr, int n);
        //int mvins_nwstr(int y, int x, const wchar_t *wstr, int n);
        int mvins_nwstr(int y, int x, in TWideChar wstr, int n);
        //int mvins_wstr(int y, int x, const wchar_t *wstr);
        int mvins_wstr(int y, int x, in TWideChar wstr);
        //int mvinwstr(int y, int x, wchar_t *wstr);
        int mvinwstr(int y, int x, ref TWideChar wstr);
        //int mvwaddnwstr(WINDOW *win, int y, int x, const wchar_t *wstr, int n);
        int mvwaddnwstr(WindowBaseSafeHandle win, int y, int x, in TWideChar wstr, int n);
        //int mvwaddwstr(WINDOW *win, int y, int x, const wchar_t *wstr);
        int mvwaddwstr(WindowBaseSafeHandle win, int y, int x, in TWideChar wstr);
        //int mvwget_wch(WINDOW *win, int y, int x, wint_t *wch);
        int mvwget_wch(WindowBaseSafeHandle win, int y, int x, ref int wch);
        //int mvwget_wstr(WINDOW *win, int y, int x, wint_t *wstr);
        int mvwget_wstr(WindowBaseSafeHandle win, int y, int x, ref int wstr);
        //int mvwgetn_wstr(WINDOW *win, int y, int x, wint_t *wstr, int n);
        int mvwgetn_wstr(WindowBaseSafeHandle win, int y, int x, ref int wstr, int n);
        //int mvwinnwstr(WINDOW *win, int y, int x, wchar_t *wstr, int n);
        int mvwinnwstr(WindowBaseSafeHandle win, int y, int x, ref TWideChar wstr, int n);
        //int mvwins_nwstr(WINDOW *win, int y, int x, const wchar_t *wstr, int n);
        int mvwins_nwstr(WindowBaseSafeHandle win, int y, int x, in TWideChar wstr, int n);
        //int mvwins_wstr(WINDOW *win, int y, int x, const wchar_t *wstr);
        int mvwins_wstr(WindowBaseSafeHandle win, int y, int x, in TWideChar str);
        //int mvwinwstr(WINDOW *win, int y, int x, wchar_t *wstr);
        int mvwinwstr(WindowBaseSafeHandle win, int y, int x, ref TWideChar wstr);
        //int slk_wset(int labnum, const wchar_t *label, int fmt);
        int slk_wset(int labnum, in TWideChar label, int fmt);
        //int unget_wch(const wchar_t wch);
        int unget_wch(TWideChar wch);
        //int unget_wch_sp(SCREEN* sp, const wchar_t wch);
        int unget_wch_sp(IntPtr screen, TWideChar wch);
        //int waddnwstr(WINDOW *win, const wchar_t *wstr, int n);
        int waddnwstr(WindowBaseSafeHandle win, in TWideChar wstr, int n);
        //int waddwstr(WINDOW *win, const wchar_t *wstr);
        int waddwstr(WindowBaseSafeHandle win, in TWideChar wstr);
        //int wget_wch(WINDOW *win, wint_t *wch);
        int wget_wch(WindowBaseSafeHandle win, ref int wch);
        //int wget_wstr(WINDOW *win, wint_t *wstr);
        int wget_wstr(WindowBaseSafeHandle win, ref int wstr);
        //int wgetn_wstr(WINDOW *win, wint_t *wstr, int n);
        int wgetn_wstr(WindowBaseSafeHandle win, ref int wstr, int n);
        //int winnwstr(WINDOW *win, wchar_t *wstr, int n);
        int winnwstr(WindowBaseSafeHandle win, ref TWideChar str, int n);
        //int winwstr(WINDOW *win, wchar_t *wstr);
        int winwstr(WindowBaseSafeHandle win, ref TWideChar str);
        //int wins_nwstr(WINDOW *win, const wchar_t *wstr, int n);
        int wins_nwstr(WindowBaseSafeHandle win, in TWideChar wstr, int n);
        //int wins_wstr(WINDOW *win, const wchar_t *wstr);
        int wins_wstr(WindowBaseSafeHandle win, in TWideChar wstr);
    }
}
