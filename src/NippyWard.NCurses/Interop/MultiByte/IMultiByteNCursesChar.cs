using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.MultiByte
{
    public interface IMultiByteNCursesChar : INCursesChar, IMultiByteChar, IEquatable<IMultiByteNCursesChar>
    { }
}
