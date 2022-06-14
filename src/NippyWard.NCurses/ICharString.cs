using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses
{
    public interface ICharString : IEnumerable<IChar>, IEquatable<ICharString>
    {
        int Length { get; }
    }
}
