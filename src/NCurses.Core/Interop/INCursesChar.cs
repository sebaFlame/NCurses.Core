using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface INCursesChar : IChar, IEquatable<INCursesChar>
    {
        ulong Attributes { get; }
        short Color { get; }
    }
}
