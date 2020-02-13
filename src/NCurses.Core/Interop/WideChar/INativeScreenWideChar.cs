using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.WideChar
{
    public interface INativeScreenWideChar<TChar, TCharString>
        where TChar : IChar
        where TCharString : ICharString
    {
        void unget_wch_sp(IntPtr screen, in TChar wch);
    }
}
