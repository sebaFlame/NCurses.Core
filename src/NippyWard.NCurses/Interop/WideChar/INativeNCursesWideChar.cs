﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.WideChar
{
    public interface INativeNCursesWideChar<TChar, TCharString>
        where TChar : IMultiByteChar
        where TCharString : IMultiByteCharString
    {
        void erasewchar(out TChar wch);
        string key_name(in TChar ch);
        void killwchar(out TChar wch);
        void slk_wset(int labnum, in TCharString label, int fmt);
        void unget_wch(in TChar wch);
    }
}
