using System;
using System.Collections.Generic;

namespace NCurses.Core.Interop.MultiByte
{
    public interface IMultiByteNCursesCharString : INCursesCharString, IMultiByteCharString, IEquatable<IMultiByteNCursesCharString>, IEnumerable<IMultiByteNCursesChar>
    {
    }
}
