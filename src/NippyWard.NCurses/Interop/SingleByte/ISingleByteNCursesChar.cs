using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.SingleByte
{
    public interface ISingleByteNCursesChar : INCursesChar, ISingleByteChar, IEquatable<ISingleByteNCursesChar>
    { }
}
