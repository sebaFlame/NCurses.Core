using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface INCursesCharString : IEnumerable<INCursesChar>, IEnumerator<INCursesChar>, IEquatable<INCursesCharString>
    {
        int Length { get; }
        INCursesChar this[int index] { get; }
    }
}
