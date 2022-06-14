using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.Mouse;

namespace NippyWard.NCurses.Interop.Wrappers
{
    internal interface ICustomTypeWrapper
    {
        INativeNCursesWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> NCurses { get; }
        INativeWindowWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> Window { get; }
        INativeStdScrWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> StdScr { get; }
        INativeScreenWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> Screen { get; }
        INativePadWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> Pad { get; }
        IWindowFactory WindowFactory { get; }
    }
}
