using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests.SingleByte
{
    public class SingleByteReadTest : ReadTest, IClassFixture<SingleByteStdScrState>
    {
        protected override char TestChar => 'a';

        public SingleByteReadTest(ITestOutputHelper testOutputHelper, SingleByteStdScrState singleByteStdScrState)
            : base(testOutputHelper, singleByteStdScrState)
        {

        }
    }
}
