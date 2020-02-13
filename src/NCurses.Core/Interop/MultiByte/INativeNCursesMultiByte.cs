using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativeNCursesMultiByte<TChar, TCharString>
        where TChar : IMultiByteChar
        where TCharString : IMultiByteCharString
    {
        void getcchar(in TChar wcval, out char wch, out ulong attrs, out short color_pair);
        void setcchar(out TChar wcval, in char wch, ulong attrs, short color_pair);
        void wunctrl(in TChar wch, out string str);
    }
}
