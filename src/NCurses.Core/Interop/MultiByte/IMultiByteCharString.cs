using System;
using System.Collections.Generic;

namespace NCurses.Core.Interop.MultiByte
{
    public interface IMultiByteCharString : INCursesCharString, IEquatable<IMultiByteCharString>, IEnumerable<IMultiByteChar>
    {
    }
}
