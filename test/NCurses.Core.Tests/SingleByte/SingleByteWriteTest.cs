using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests.SingleByte
{
    public class SingleByteWriteTest : WriteTest, IClassFixture<SingleByteStdScrState>
    {
        protected override char TestChar => 'a';

        protected override string TestString => "test";

        public SingleByteWriteTest(ITestOutputHelper testOutputHelper, SingleByteStdScrState singleByteStdScrState)
            : base(testOutputHelper, singleByteStdScrState)
        {

        }
    }
}
