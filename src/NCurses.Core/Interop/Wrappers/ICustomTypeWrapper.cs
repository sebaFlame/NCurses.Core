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
        INativeNCursesWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> NCurses { get; }
        INativeWindowWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> Window { get; }
        INativeStdScrWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> StdScr { get; }
        INativeScreenWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> Screen { get; }
        INativePadWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> Pad { get; }
        IWindowFactory WindowFactory { get; }
    }
}
