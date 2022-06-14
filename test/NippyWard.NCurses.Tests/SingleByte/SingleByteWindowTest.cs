using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

namespace NippyWard.NCurses.Tests.SingleByte
{
    public class SingleByteWindowTest : WindowTest, IClassFixture<SingleByteStdScrState>
    {
        protected override string TestString => "test";

        public SingleByteWindowTest(ITestOutputHelper testOutputHelper, SingleByteStdScrState singleByteStdScrState)
            : base(testOutputHelper, singleByteStdScrState)
        {

        }
    }
}
