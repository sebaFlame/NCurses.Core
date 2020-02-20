using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core
{
    internal interface IWindowFactory
    {
        bool HasWindows { get; }
        bool HasPanels { get; }

        IWindow CreateStdScr(StdScrSafeHandle stdScrSafeHandle);
        IWindow CreateStdScr(IWindow existingStdScr);

        IWindow CreateWindow(int nlines, int ncols, int begy, int begx);
        IWindow CreateWindow(int nlines, int ncols);
        IWindow CreateWindow();
        IWindow CreateWindow(WindowBaseSafeHandle windowBaseSafeHandle);

        IPad CreatePad(IWindow window, int nlines, int ncols);
        IPad CreatePad(int nlines, int ncols);

        IPanel CreatePanel(IWindow window);
    }
}
