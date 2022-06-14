using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.MultiByte
{
    public interface INativeNCursesMultiByte<TChar, TCharString>
        where TChar : IMultiByteNCursesChar
        where TCharString : IMultiByteNCursesCharString
    {
        void getcchar(in TChar wcval, out char wch, out ulong attrs, out ushort color_pair);
        void setcchar(out TChar wcval, in char wch, ulong attrs, ushort color_pair);
        void wunctrl(in TChar wch, out string str);
    }
}
