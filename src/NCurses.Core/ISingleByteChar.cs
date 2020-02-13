using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core
{
    public interface ISingleByteChar : IChar, IEquatable<ISingleByteChar>
    {
        byte EncodedChar { get; }
    }
}
