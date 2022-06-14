using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.SafeHandles;

namespace NippyWard.NCurses.Interop.Wrappers
{
    public interface INativePadWrapper<TMultiByte, TMultiByteString, TWideChar, TWideCharString, TSingleByte, TSingleByteString, TChar, TCharString, TMouseEvent>
            : INativePadMultiByte<TMultiByte, TMultiByteString>,
            INativePadSingleByte<TSingleByte, TSingleByteString>
        where TMultiByte : IMultiByteNCursesChar
        where TMultiByteString : IMultiByteNCursesCharString
        where TWideChar : IMultiByteChar
        where TWideCharString : IMultiByteCharString
        where TSingleByte : ISingleByteNCursesChar
        where TSingleByteString : ISingleByteNCursesCharString
        where TChar : ISingleByteChar
        where TCharString : ISingleByteCharString
        where TMouseEvent : IMEVENT
    {
        void pnoutrefresh(WindowBaseSafeHandle pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol);
        void prefresh(WindowBaseSafeHandle pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol);
    }
}
