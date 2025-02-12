﻿using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.SafeHandles;

namespace NippyWard.NCurses.Interop.Char
{
    internal interface ICharWrapper<TChar>
        where TChar : unmanaged, IChar
    {
        //int addnstr(const char *str, int n);
        int addnstr(in TChar txt, int number);
        //int addstr(const char *str);
        int addstr(in TChar txt);
        //const char * curses_version(void);
        ref TChar curses_version();
        //int define_key(const char *definition, int keycode);
        int define_key(in TChar definition, int keycode);
        //int define_key(SCREEN* sp, const char *definition, int keycode);
        int define_key_sp(IntPtr screen, in TChar definition, int keycode);
        //char erasechar(void);
        TChar erasechar();
        //char erasechar_sp(SCREEN* sp);
        TChar erasechar_sp(IntPtr screen);
        //int getnstr(char *str, int n);
        int getnstr(ref TChar txt, int count);
        //int getstr(char *str);
        int getstr(ref TChar txt);
        //int instr(char *str);
        int innstr(ref TChar str, int n);
        //int insnstr(const char *str, int n);
        int insnstr(in TChar str, int n);
        //int insstr(const char *str);
        int insstr(in TChar str);
        //int instr(char *str);
        int instr(ref TChar str);
        //int key_defined(const char *definition);
        int key_defined(in TChar definition);
        //int key_defined(const char *definition);
        int key_defined_sp(IntPtr screen, in TChar definition);
        //char * keybound(int keycode, int count);
        ref TChar keybound(int keycode, int count);
        //char * keybound_sp(SCREEN* sp, int keycode, int count);
        ref TChar keybound_sp(IntPtr screen, int keycode, int count);
        //char *keyname(int c);
        ref TChar keyname(int c);
        //char *keyname_sp(SCREEN* sp, int c);
        ref TChar keyname_sp(IntPtr screen, int c);
        //char killchar(void);
        TChar killchar();
        //char killchar_sp(SCREEN* sp);
        TChar killchar_sp(IntPtr screen);
        //char *longname(void);
        ref TChar longname();
        //char *longname_sp(SCREEN* sp);
        ref TChar longname_sp(IntPtr screen);
        //int mvaddnstr(int y, int x, const char *str, int n);
        int mvaddnstr(int y, int x, in TChar txt, int n);
        //int mvaddstr(int y, int x, const char *str);
        int mvaddstr(int y, int x, in TChar txt);
        //int mvgetnstr(int y, int x, char *str, int n);
        int mvgetnstr(int y, int x, ref TChar str, int count);
        //int mvgetstr(int y, int x, char *str);
        int mvgetstr(int y, int x, ref TChar str);
        //int mvinnstr(int y, int x, char *str, int n);
        int mvinnstr(int y, int x, ref TChar str, int n);
        //int mvinsnstr(int y, int x, const char *str, int n);
        int mvinsnstr(int y, int x, in TChar str, int n);
        //int mvinsstr(int y, int x, const char *str);
        int mvinsstr(int y, int x, in TChar str);
        //int mvinstr(int y, int x, char *str);
        int mvinstr(int y, int x, ref TChar str);
        //int mvprintw(int y, int x, const char *fmt, ...);
        int mvprintw(int y, int x, in TChar str, IntPtr argListPtr);
        //int mvscanw(int y, int x, const char *fmt, ...);
        int mvscanw(int y, int x, ref TChar format, IntPtr argListPtr);
        //int mvwaddnstr(WINDOW* win, int y, int x, const char* str, int n);
        int mvwaddnstr(WindowBaseSafeHandle win, int y, int x, in TChar str, int n);
        //int mvwaddstr(WINDOW *win, int y, int x, const char *str);
        int mvwaddstr(WindowBaseSafeHandle win, int y, int x, in TChar str);
        //int mvwgetnstr(WINDOW *, int y, int x, char *str, int n);
        int mvwgetnstr(WindowBaseSafeHandle win, int y, int x, ref TChar str, int count);
        //int mvwgetstr(WINDOW *win, int y, int x, char *str);
        int mvwgetstr(WindowBaseSafeHandle win, int y, int x, ref TChar str);
        //int mvwinnstr(WINDOW *win, int y, int x, char *str, int n);
        int mvwinnstr(WindowBaseSafeHandle win, int y, int x, ref TChar str, int n);
        //int mvwinsnstr(WINDOW *win, int y, int x, const char *str, int n);
        int mvwinsnstr(WindowBaseSafeHandle win, int y, int x, in TChar str, int n);
        //int mvwinsstr(WINDOW *win, int y, int x, const char *str);
        int mvwinsstr(WindowBaseSafeHandle win, int y, int x, in TChar str);
        //int mvwinstr(WINDOW *win, int y, int x, char *str);
        int mvwinstr(WindowBaseSafeHandle win, int y, int x, ref TChar str);
        //int mvwprintw(WINDOW *win, int y, int x, const char *fmt, ...);
        int mvwprintw(WindowBaseSafeHandle win, int y, int x, in TChar format, IntPtr varArgListPtr);
        //int mvwscanw(WINDOW *win, int y, int x, const char *fmt, ...);
        int mvwscanw(WindowBaseSafeHandle win, int y, int x, ref TChar format, IntPtr varArgListPtr);
        //int printw(const char *fmt, ...);
        int printw(in TChar format, IntPtr varArgListPtr);
        //int putp(const char *str);
        int putp(in TChar str);
        //int putp_sp(SCREEN* sp, const char *str);
        int putp_sp(IntPtr screen, in TChar str);
        //int scanw(const char *fmt, ...);
        int scanw(ref TChar format, IntPtr varArgListPtr);
        //int scr_dump(const char *filename);
        int scr_dump(in TChar filename);
        //int scr_dump_sp(SCREEN* sp, const char *filename);
        int scr_dump_sp(IntPtr screen, in TChar filename);
        //int scr_init(const char *filename);
        int scr_init(in TChar filename);
        //int scr_init_sp(SCREEN* sp, const char *filename);
        int scr_init_sp(IntPtr screen, in TChar filename);
        //int scr_restore(const char *filename);
        int scr_restore(in TChar filename);
        //int scr_restore_sp(SCREEN* sp, const char *filename);
        int scr_restore_sp(IntPtr screen, in TChar filename);
        //int scr_set(const char *filename);
        int scr_set(in TChar filename);
        //int scr_set_sp(SCREEN* sp, const char *filename);
        int scr_set_sp(IntPtr screen, in TChar filename);
        //char *slk_label(int labnum);
        ref TChar slk_label(int labnum);
        //char *slk_label_sp(SCREEN* sp, int labnum);
        ref TChar slk_label_sp(IntPtr screen, int labnum);
        //int slk_set(int labnum, const char *label, int fmt);
        int slk_set(int labnum, in TChar label, int fmt);
        //int slk_set_sp(SCREEN* sp, int labnum, const char *label, int fmt);
        int slk_set_sp(IntPtr screen, int labnum, in TChar label, int fmt);
        //char *termname(void);
        ref TChar termname();
        //char *termname_sp(SCREEN* sp);
        ref TChar termname_sp(IntPtr screen);
        //int tigetflag(const char *capname);
        int tigetflag(in TChar capname);
        //int tigetflag_sp(SCREEN* sp, const char *capname);
        int tigetflag_sp(IntPtr screen, in TChar capname);
        //int tigetnum(const char *capname);
        int tigetnum(in TChar capname);
        //int tigetnum_sp(SCREEN* sp, const char *capname);
        int tigetnum_sp(IntPtr screen, in TChar capname);
        //char *tigetstr(const char *capname);
        ref sbyte tigetstr(in TChar capname);
        //char *tigetstr_sp(SCREEN* sp, const char *capname);
        ref sbyte tigetstr_sp(IntPtr screen, in TChar capname);
        //int waddnstr(WINDOW *win, const char *str, int n);
        int waddnstr(WindowBaseSafeHandle win, in TChar str, int number);
        //int waddstr(WINDOW *win, const char *str);
        int waddstr(WindowBaseSafeHandle win, in TChar str);
        //int wgetnstr(WINDOW *win, char *str, int n);
        int wgetnstr(WindowBaseSafeHandle win, ref TChar str, int count);
        //int wgetstr(WINDOW *win, char *str);
        int wgetstr(WindowBaseSafeHandle win, ref TChar str);
        //int winnstr(WINDOW *win, char *str, int n);
        int winnstr(WindowBaseSafeHandle win, ref TChar str, int n);
        //int winstr(WINDOW *win, char *str);
        int winstr(WindowBaseSafeHandle win, ref TChar str);
        //int winsnstr(WINDOW *win, const char *str, int n);
        int winsnstr(WindowBaseSafeHandle win, in TChar str, int n);
        //int winsstr(WINDOW *win, const char *str);
        int winsstr(WindowBaseSafeHandle win, in TChar str);
        //int wprintw(WINDOW *win, const char *fmt, ...);
        int wprintw(WindowBaseSafeHandle win, in TChar format, IntPtr varArgListPtr);
        //int wscanw(WINDOW *win, const char *fmt, ...);
        int wscanw(WindowBaseSafeHandle win, ref TChar format, IntPtr varArgListPtr);
    }
}
