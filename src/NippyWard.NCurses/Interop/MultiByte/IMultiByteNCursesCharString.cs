using System;
using System.Collections.Generic;

namespace NippyWard.NCurses.Interop.MultiByte
{
    public interface IMultiByteNCursesCharString : INCursesCharString, IMultiByteCharString, IEquatable<IMultiByteNCursesCharString>, IEnumerable<IMultiByteNCursesChar>
    {
    }
}
