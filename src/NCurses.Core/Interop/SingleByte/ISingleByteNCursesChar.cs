using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SingleByte
{
    public interface ISingleByteNCursesChar : INCursesChar, ISingleByteChar, IEquatable<ISingleByteNCursesChar>
    { }
}
