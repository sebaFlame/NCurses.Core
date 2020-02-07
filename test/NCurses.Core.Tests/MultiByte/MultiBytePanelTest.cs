using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests.MultiByte
{
    public class MultiBytePanelTest : PanelTest, IClassFixture<MultiByteStdScrState>
    {
        protected override Func<int, int, int, int, Window> CreateWindow => 
            (int nlines, int ncols, int begy, int begx) => Core.Window.CreateMultiByteWindow(nlines, ncols, begy, begx);

        public MultiBytePanelTest(ITestOutputHelper testOutputHelper, MultiByteStdScrState multiByteStdScrState)
            : base(testOutputHelper, multiByteStdScrState)
        {

        }
    }
}
