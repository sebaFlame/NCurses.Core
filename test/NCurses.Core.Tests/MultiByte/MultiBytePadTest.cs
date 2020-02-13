using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests.MultiByte
{
    public class MultiBytePadTest : PadTest, IClassFixture<MultiByteStdScrState>
    {
        public MultiBytePadTest(ITestOutputHelper testOutputHelper, MultiByteStdScrState multiByteStdScrState)
            : base(testOutputHelper, multiByteStdScrState)
        {

        }
    }
}
