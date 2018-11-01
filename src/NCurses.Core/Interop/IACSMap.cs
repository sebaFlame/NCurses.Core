using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface IACSMap
    {
        INCursesChar this[char index] { get; }
    }
}
