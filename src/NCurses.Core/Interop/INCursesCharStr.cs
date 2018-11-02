using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface INCursesCharStr : IEnumerable<INCursesChar>, IEnumerator<INCursesChar>, IEquatable<INCursesCharStr>
    {
        int Length { get; }
        INCursesChar this[int index] { get; }
    }
}
