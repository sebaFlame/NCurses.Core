using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests.SingleByte
{
    public class SingleBytePadTest : PadTest, IClassFixture<SingleByteStdScrState>
    {
        public SingleBytePadTest(ITestOutputHelper testOutputHelper, SingleByteStdScrState singleByteStdScrState)
            : base(testOutputHelper, singleByteStdScrState)
        {

        }
    }
}
