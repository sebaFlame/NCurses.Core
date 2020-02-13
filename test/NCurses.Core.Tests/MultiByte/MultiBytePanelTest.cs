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
        public MultiBytePanelTest(ITestOutputHelper testOutputHelper, MultiByteStdScrState multiByteStdScrState)
            : base(testOutputHelper, multiByteStdScrState)
        {

        }
    }
}
