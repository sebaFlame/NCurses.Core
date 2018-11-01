using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface INCursesCharStr : IEnumerable<INCursesChar>, IEnumerator<INCursesChar>
    {
        int Length { get; }
        INCursesChar this[int index] { get; }
    }
}
