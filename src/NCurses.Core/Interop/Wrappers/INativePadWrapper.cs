using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.Wrappers
{
    public interface INativePadWrapper<TMultiByte, TMultiByteString, TWideChar, TWideCharString, TSingleByte, TSingleByteString, TChar, TCharString, TMouseEvent>
            : INativePadMultiByte<TMultiByte, TMultiByteString>,
            INativePadSingleByte<TSingleByte, TSingleByteString>
        where TMultiByte : IMultiByteChar
        where TMultiByteString : IMultiByteCharString
        where TWideChar : IChar
        where TWideCharString : ICharString
        where TSingleByte : ISingleByteChar
        where TSingleByteString : ISingleByteCharString
        where TChar : IChar
        where TCharString : ICharString
        where TMouseEvent : IMEVENT
    {
        void pnoutrefresh(WindowBaseSafeHandle pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol);
        void prefresh(WindowBaseSafeHandle pad, int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol);
    }
}
