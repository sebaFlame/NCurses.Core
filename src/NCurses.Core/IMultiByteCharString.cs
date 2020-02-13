using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core
{
    public interface IMultiByteCharString : ICharString, IEquatable<IMultiByteCharString>, IEnumerable<IMultiByteChar>
    {

    }
}
