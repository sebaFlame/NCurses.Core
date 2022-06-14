using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses
{
    public interface IMultiByteCharString : ICharString, IEquatable<IMultiByteCharString>, IEnumerable<IMultiByteChar>
    {

    }
}
