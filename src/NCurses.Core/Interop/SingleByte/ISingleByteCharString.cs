using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SingleByte
{
    public interface ISingleByteCharString : INCursesCharString, IEnumerable<ISingleByteChar>, IEnumerator<ISingleByteChar>, IEquatable<ISingleByteCharString>
    {
    }
}
