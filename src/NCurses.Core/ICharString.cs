using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core
{
    public interface ICharString : IEnumerable<IChar>, IEquatable<ICharString>
    {
        int Length { get; }
    }
}
