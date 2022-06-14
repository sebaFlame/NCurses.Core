using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.SingleByte
{
    public interface ISingleByteNCursesCharString : INCursesCharString, ISingleByteCharString, IEquatable<ISingleByteNCursesCharString>, IEnumerable<ISingleByteNCursesChar>
    {
    }
}
