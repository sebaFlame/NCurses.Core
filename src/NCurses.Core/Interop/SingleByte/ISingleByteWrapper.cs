﻿using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    internal interface ISingleByteWrapper<TSingleByte, TChar, TMouseEvent>
        where TSingleByte : unmanaged, ISingleByteNCursesChar
        where TChar : unmanaged, IChar
        where TMouseEvent : unmanaged, IMEVENT
    {
        //int addch(const chtype ch);
        int addch(TSingleByte ch);
        //int addchnstr(const chtype *chstr, int n);
        int addchnstr(in TSingleByte txt, int number);
        //int addchstr(const chtype *chstr);
        int addchstr(in TSingleByte txt);
        //int attr_get(attr_t* attrs, short* pair, void* opts);
        int attr_get(ref TSingleByte attrs, ref short pair, IntPtr opts);
        //int attr_off(attr_t attrs, void *opts);
        int attr_off(TSingleByte attrs, IntPtr opts);
        //int attr_on(attr_t attrs, void *opts);
        int attr_on(TSingleByte attrs, IntPtr opts);
        //int attr_set(attr_t attrs, short pair, void *opts);
        int attr_set(TSingleByte attrs, short pair, IntPtr opts);
        //int bkgd(chtype ch);
        int bkgd(TSingleByte bkgd);
        //void bkgdset(chtype ch);
        void bkgdset(TSingleByte bkgd);
        //int border(chtype ls, chtype rs, chtype ts, chtype bs, chtype tl, chtype tr, chtype bl, chtype br);
        int border(TSingleByte ls, TSingleByte rs, TSingleByte ts, TSingleByte bs, TSingleByte tl, TSingleByte tr, TSingleByte bl, TSingleByte br);
        //int box(WINDOW *win, chtype verch, chtype horch);
        int box(WindowBaseSafeHandle win, TSingleByte verch, TSingleByte horch);
        //int chgat(int n, attr_t attr, short pair, const void *opts);
        int chgat(int number, TSingleByte attrs, short pair, IntPtr opts);
        //int echochar(const chtype ch);
        int echochar(TSingleByte ch);
        // chtype getbkgd(WINDOW *win);
        TSingleByte getbkgd(WindowBaseSafeHandle win);
        //int getmouse(MEVENT *event);
        int getmouse(ref TMouseEvent ev);
        //int getmouse_sp(SCREEN* sp, MEVENT *event);
        int getmouse_sp(IntPtr screen, ref TMouseEvent ev);
        //int hline(chtype ch, int n);
        int hline(TSingleByte ch, int count);
        //chtype inch(void);
        TSingleByte inch();
        //int inchnstr(chtype *chstr, int n);
        int inchnstr(ref TSingleByte txt, int count);
        //int inchstr(chtype *chstr);
        int inchstr(ref TSingleByte txt);
        //int insch(chtype ch);
        int insch(TSingleByte ch);
        //mmask_t mousemask(mmask_t newmask, mmask_t *oldmask);
        TSingleByte mousemask(TSingleByte newmask, ref TSingleByte oldmask);
        //mmask_t mousemask_sp(SCREEN* sp, mmask_t newmask, mmask_t *oldmask);
        TSingleByte mousemask_sp(IntPtr screen, TSingleByte newmask, ref TSingleByte oldmask);
        //int mvaddch(int y, int x, const chtype ch);
        int mvaddch(int y, int x, TSingleByte ch);
        //int mvaddchnstr(int y, int x, const chtype *chstr, int n);
        int mvaddchnstr(int y, int x, in TSingleByte chstr, int n);
        //int mvaddchstr(int y, int x, const chtype *chstr);
        int mvaddchstr(int y, int x, in TSingleByte chStr);
        //int mvchgat(int y, int x, int n, attr_t attr, short pair, const void* opts);
        int mvchgat(int y, int x, int number, TSingleByte attrs, short pair, IntPtr opts);
        //int mvhline(int y, int x, chtype ch, int n);
        int mvhline(int y, int x, TSingleByte ch, int count);
        //chtype mvinch(int y, int x);
        TSingleByte mvinch(int y, int x);
        //int mvinchnstr(int y, int x, chtype *chstr, int n);
        int mvinchnstr(int y, int x, ref TSingleByte txt, int count);
        //int mvinchstr(int y, int x, chtype *chstr);
        int mvinchstr(int y, int x, ref TSingleByte txt);
        //int mvinsch(int y, int x, chtype ch);
        int mvinsch(int y, int x, TSingleByte ch);
        //int mvvline(int y, int x, chtype ch, int n);
        int mvvline(int y, int x, TSingleByte ch, int n);
        //int mvwaddch(WINDOW *win, int y, int x, const chtype ch);
        int mvwaddch(WindowBaseSafeHandle win, int y, int x, TSingleByte ch);
        //int mvwaddchnstr(WINDOW *win, int y, int x, const chtype *chstr, int n);
        int mvwaddchnstr(WindowBaseSafeHandle win, int y, int x, in TSingleByte chstr, int n);
        //int mvwaddchstr(WINDOW *win, int y, int x, const chtype *chstr);
        int mvwaddchstr(WindowBaseSafeHandle win, int y, int x, in TSingleByte chstr);
        //int mvwchgat(WINDOW *win, int y, int x, int n, attr_t attr, short pair, const void* opts);
        int mvwchgat(WindowBaseSafeHandle win, int y, int x, int number, TSingleByte attrs, short pair, IntPtr opts);
        //int mvwhline(WINDOW *, int y, int x, chtype ch, int n);
        int mvwhline(WindowBaseSafeHandle win, int y, int x, TSingleByte ch, int count);
        //chtype mvwinch(WINDOW *win, int y, int x);
        TSingleByte mvwinch(WindowBaseSafeHandle win, int y, int x);
        //int mvwinchnstr(WINDOW *win, int y, int x, chtype *chstr, int n);
        int mvwinchnstr(WindowBaseSafeHandle win, int y, int x, ref TSingleByte chStr, int count);
        //int mvwinchstr(WINDOW *win, int y, int x, chtype *chstr);
        int mvwinchstr(WindowBaseSafeHandle win, int y, int x, ref TSingleByte chStr);
        //int mvwinsch(WINDOW *win, int y, int x, chtype ch);
        int mvwinsch(WindowBaseSafeHandle win, int y, int x, TSingleByte ch);
        //int mvwvline(WINDOW *, int y, int x, chtype ch, int n);
        int mvwvline(WindowBaseSafeHandle win, int y, int x, TSingleByte ch, int n);
        //int pechochar(WINDOW *pad, chtype ch);
        int pechochar(WindowBaseSafeHandle pad, TSingleByte ch);
        //attr_t slk_attr(void);
        TSingleByte slk_attr();
        //attr_t slk_attr(SCREEN* sp);
        TSingleByte slk_attr_sp(IntPtr screen);
        //int slk_attr_off(const attr_t attrs, void * opts);
        int slk_attr_off(TSingleByte attrs, IntPtr opts);
        //int slk_attr_on(attr_t attrs, void* opts);
        int slk_attr_on(TSingleByte attrs, IntPtr opts);
        //int slk_attr_set(const attr_t attrs, short pair, void* opts);
        int slk_attr_set(TSingleByte attrs, short color_pair, IntPtr opts);
        //int slk_attr_set_sp(SCREEN* sp, const attr_t attrs, short pair, void* opts);
        int slk_attr_set_sp(IntPtr screen, TSingleByte attrs, short color_pair, IntPtr opts);
        //int slk_attroff(const chtype attrs);
        int slk_attroff(TSingleByte attrs);
        //int slk_attroff_sp(SCREEN* sp, const chtype attrs);
        int slk_attroff_sp(IntPtr screen, TSingleByte attrs);
        //int slk_attron(const chtype attrs);
        int slk_attron(TSingleByte attrs);
        //int slk_attron_sp(SCREEN* sp, const chtype attrs);
        int slk_attron_sp(IntPtr screen, TSingleByte attrs);
        //int slk_attrset(const chtype attrs);
        int slk_attrset(TSingleByte attrs);
        //int slk_attrset_sp(SCREEN* sp, const chtype attrs);
        int slk_attrset_sp(IntPtr screen, TSingleByte attrs);
        //attr_t term_attrs(void);
        TSingleByte term_attrs();
        //attr_t term_attrs_sp(SCREEN* sp);
        TSingleByte term_attrs_sp(IntPtr screen);
        //chtype termattrs(void);
        TSingleByte termattrs();
        //chtype termattrs_sp(SCREEN* sp);
        TSingleByte termattrs_sp(IntPtr screen);
        //char *unctrl(chtype c);
        ref TChar unctrl(TSingleByte ch);
        //int ungetmouse(MEVENT *event);
        int ungetmouse(in TMouseEvent ev);
        //int ungetmouse_sp(SCREEN* sp, MEVENT *event);
        int ungetmouse_sp(IntPtr screen, in TMouseEvent ev);
        //int vid_attr(attr_t attrs, short pair, void *opts);
        int vid_attr(TSingleByte attrs, short pair, IntPtr opts);
        //int vid_attr_sp(SCREEN* sp, attr_t attrs, short pair, void *opts);
        int vid_attr_sp(IntPtr screen, TSingleByte attrs, short pair, IntPtr opts);
        //int vid_puts(attr_t attrs, short pair, void *opts, int (*putc)(int));
        int vid_puts(TSingleByte attrs, short pair, IntPtr opts, IntPtr OUTC);
        //int vid_puts_sp(SCREEN* sp, attr_t attrs, short pair, void *opts, int (*putc)(int));
        int vid_puts_sp(IntPtr screen, TSingleByte attrs, short pair, IntPtr opts, IntPtr OUTC);
        //int vidattr(chtype attrs);
        int vidattr(TSingleByte attrs);
        //int vidattr_sp(SCREEN* sp, chtype attrs);
        int vidattr_sp(IntPtr screen, TSingleByte attrs);
        //int vidputs(chtype attrs, int (*putc)(int));
        int vidputs(TSingleByte attrs, IntPtr OUTC);
        //int vidputs_sp(SCREEN* sp, chtype attrs, int (*putc)(int));
        int vidputs_sp(IntPtr screen, TSingleByte attrs, IntPtr OUTC);
        //int vline(chtype ch, int n);
        int vline(TSingleByte ch, int n);
        //int waddch(WINDOW *win, const chtype ch);
        int waddch(WindowBaseSafeHandle win, TSingleByte ch);
        //int waddchnstr(WINDOW *win, const chtype *chstr, int n);
        int waddchnstr(WindowBaseSafeHandle win, in TSingleByte chstr, int number);
        //int mvaddchstr(int y, int x, const chtype *chstr);
        int waddchstr(WindowBaseSafeHandle win, in TSingleByte chstr);
        //int wattr_get(WINDOW *win, attr_t *attrs, short *pair, void *opts);
        int wattr_get(WindowBaseSafeHandle win, ref TSingleByte attrs, ref short pair, IntPtr opts);
        //int wattr_off(WINDOW *win, attr_t attrs, void *opts);
        int wattr_off(WindowBaseSafeHandle win, TSingleByte attrs, IntPtr opts);
        //int wattr_on(WINDOW *win, attr_t attrs, void *opts);
        int wattr_on(WindowBaseSafeHandle win, TSingleByte attrs, IntPtr opts);
        //int wattr_set(WINDOW *win, attr_t attrs, short pair, void *opts);
        int wattr_set(WindowBaseSafeHandle win, TSingleByte attrs, short pair, IntPtr opts);
        //int wbkgd(WINDOW *win, chtype ch);
        int wbkgd(WindowBaseSafeHandle win, TSingleByte bkgd);
        //void wbkgdset(WINDOW *win, chtype ch);
        void wbkgdset(WindowBaseSafeHandle win, TSingleByte bkgd);
        //int wborder(WINDOW *win, chtype ls, chtype rs, chtype ts, chtype bs, chtype tl, chtype tr, chtype bl, chtype br);
        int wborder(WindowBaseSafeHandle win, TSingleByte ls, TSingleByte rs, TSingleByte ts, TSingleByte bs, TSingleByte tl, TSingleByte tr, TSingleByte bl, TSingleByte br);
        //int wchgat(WINDOW *win, int n, attr_t attr, short pair, const void* opts);
        int wchgat(WindowBaseSafeHandle win, int number, TSingleByte attrs, short pair, IntPtr opts);
        //int wechochar(WINDOW *win, const chtype ch);
        int wechochar(WindowBaseSafeHandle win, TSingleByte ch);
        //int whline(WINDOW *win, chtype ch, int n);
        int whline(WindowBaseSafeHandle win, TSingleByte ch, int count);
        //chtype winch(WINDOW *win);
        TSingleByte winch(WindowBaseSafeHandle win);
        //int winchnstr(WINDOW *win, chtype *chstr, int n);
        int winchnstr(WindowBaseSafeHandle win, ref TSingleByte chstr, int count);
        //int winchstr(WINDOW *win, chtype *chstr);
        int winchstr(WindowBaseSafeHandle win, ref TSingleByte chstr);
        //int winsch(WINDOW *win, chtype ch);
        int winsch(WindowBaseSafeHandle win, TSingleByte ch);
        //int wvline(WINDOW *win, chtype ch, int n);
        int wvline(WindowBaseSafeHandle win, TSingleByte ch, int n);
    }
}
