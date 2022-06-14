using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses
{
    public interface IPad : IWindow
    {
        void NoOutRefresh(int pminrow, int pmincol, int sminrow, int smincol, int smaxrow, int smaxcol);

        IWindow SubPad(int nlines, int ncols, int begin_y, int begin_x);

        void Echo(char ch);
    }
}
