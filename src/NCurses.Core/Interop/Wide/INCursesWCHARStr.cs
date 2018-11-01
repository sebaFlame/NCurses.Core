using System.Collections.Generic;

namespace NCurses.Core.Interop.Wide
{
    public interface INCursesWCHARStr : INCursesCharStr, IEnumerable<INCursesWCHAR>, IEnumerator<INCursesWCHAR>
    {
    }
}
