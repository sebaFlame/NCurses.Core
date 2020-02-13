using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.Char
{
    public interface INativeStdScrChar<TChar, TCharString>
        where TChar : IChar
        where TCharString : ICharString
    {
        void addnstr(in TCharString txt, int number);
        void addstr(in TCharString txt);
        void getnstr(ref TCharString str, int count);
        void getstr(ref TCharString txt);
        void innstr(ref TCharString str, int n, out int read);
        void insnstr(in TCharString str, int n);
        void insstr(in TCharString str);
        void instr(ref TCharString str, out int read);
        void mvaddnstr(int y, int x, in TCharString txt, int n);
        void mvaddstr(int y, int x, in TCharString txt);
        void mvgetnstr(int y, int x, ref TCharString str, int count);
        void mvgetstr(int y, int x, ref TCharString str);
        void mvinnstr(int y, int x, ref TCharString str, int n, out int read);
        void mvinsnstr(int y, int x, in TCharString str, int n);
        void mvinsstr(int y, int x, in TCharString str);
        void mvinstr(int y, int x, ref TCharString str, out int read);
        void mvprintw(int y, int x, in TCharString str, params TCharString[] argList);
        void mvscanw(int y, int x, ref TCharString str, params TCharString[] argList);
        void printw(in TCharString format, params TCharString[] argList);
        void scanw(ref TCharString str, params TCharString[] argList);
    }
}
