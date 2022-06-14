using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses
{
    public interface IChar : IEquatable<IChar>
    {
        int Char { get; }
    }
}
