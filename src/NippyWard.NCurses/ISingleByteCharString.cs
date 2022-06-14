using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses
{
    public interface ISingleByteCharString : ICharString, IEquatable<ISingleByteCharString>, IEnumerable<ISingleByteChar>
    {
    }
}
