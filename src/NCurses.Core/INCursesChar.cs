using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core
{
    public interface INCursesChar : IChar, IEquatable<INCursesChar>
    {
        ulong Attributes { get; }
        short Color { get; }
    }
}
