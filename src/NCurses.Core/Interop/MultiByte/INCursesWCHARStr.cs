using System;
using System.Collections.Generic;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INCursesWCHARStr : INCursesCharStr, IEnumerable<INCursesWCHAR>, IEnumerator<INCursesWCHAR>, IEquatable<INCursesWCHARStr>
    {
    }
}
