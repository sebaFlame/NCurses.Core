using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core
{
    public interface IChar : IEquatable<IChar>
    {
        int Char { get; }
    }
}
