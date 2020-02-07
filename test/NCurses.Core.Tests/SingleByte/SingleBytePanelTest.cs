using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests.SingleByte
{
    public class SingleBytePanelTest : PanelTest, IClassFixture<SingleByteStdScrState>
    {
        protected override Func<int, int, int, int, Window> CreateWindow =>
            (int nlines, int ncols, int begy, int begx) => Core.Window.CreateSingleByteWindow(nlines, ncols, begy, begx);

        public SingleBytePanelTest(ITestOutputHelper testOutputHelper, SingleByteStdScrState singleByteStdScrState)
            : base(testOutputHelper, singleByteStdScrState)
        {

        }
    }
}
