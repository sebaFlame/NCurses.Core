using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.WideChar
{
    public interface INativeNCursesWideChar<TChar, TCharString>
        where TChar : IChar
        where TCharString : ICharString
    {
        void erasewchar(out TChar wch);
        string key_name(in TChar ch);
        void killwchar(out TChar wch);
        void slk_wset(int labnum, in TCharString label, int fmt);
        void unget_wch(in TChar wch);
    }
}
