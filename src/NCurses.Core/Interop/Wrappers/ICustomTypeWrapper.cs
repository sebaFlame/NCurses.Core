using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.Wrappers
{
    internal interface ICustomTypeWrapper
    {
        INativeNCursesWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> NCurses { get; }
        INativeWindowWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> Window { get; }
        INativeStdScrWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> StdScr { get; }
        INativeScreenWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> Screen { get; }
        INativePadWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> Pad { get; }
        IWindowFactory WindowFactory { get; }
    }
}
