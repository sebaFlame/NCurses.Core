using System;
using System.Collections.Generic;

namespace NCurses.Core.Interop.MultiByte
{
    public interface IMultiByteCharString : INCursesCharString, IEnumerable<IMultiByteChar>, IEnumerator<IMultiByteChar>, IEquatable<IMultiByteCharString>
    {
    }
}
