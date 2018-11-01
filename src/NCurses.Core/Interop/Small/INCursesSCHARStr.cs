using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.Small
{
    public interface INCursesSCHARStr : INCursesCharStr, IEnumerable<INCursesSCHAR>, IEnumerator<INCursesSCHAR>
    {
    }
}
