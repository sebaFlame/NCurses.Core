using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface INCursesChar : IEquatable<INCursesChar>
    {
        char Char { get; }
        ulong Attributes { get; }
        short Color { get; }
    }
}
