using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INCursesSCHARStr : INCursesCharStr, IEnumerable<INCursesSCHAR>, IEnumerator<INCursesSCHAR>
    {
    }
}
