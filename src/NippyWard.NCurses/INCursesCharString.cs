﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses
{
    public interface INCursesCharString : ICharString, IEnumerable<INCursesChar>, IEquatable<INCursesCharString>
    {
        INCursesChar this[int index] { get; }
    }
}
