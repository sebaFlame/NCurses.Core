using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.MultiByte
{
    public interface IMultiByteNCursesChar : INCursesChar, IMultiByteChar, IEquatable<IMultiByteNCursesChar>
    { }
}
