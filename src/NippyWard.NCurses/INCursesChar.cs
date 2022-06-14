using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses
{
    public interface INCursesChar : IChar, IEquatable<INCursesChar>
    {
        ulong Attributes { get; }
        ushort ColorPair { get; }
    }
}
