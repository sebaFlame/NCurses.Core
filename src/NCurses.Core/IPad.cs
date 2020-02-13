using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core
{
    public interface IPad : IWindow
    {
        IPad SubPad(int nlines, int ncols, int begin_y, int begin_x);

        void Echo(char ch);
    }
}
