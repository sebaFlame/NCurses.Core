using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop
{
    public interface IACSMap
    {
        INCursesChar this[char index] { get; }
    }
}
