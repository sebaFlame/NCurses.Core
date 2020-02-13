using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core
{
    public interface ISingleByteCharString : ICharString, IEquatable<ISingleByteCharString>, IEnumerable<ISingleByteChar>
    {
    }
}
