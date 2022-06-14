using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

namespace NippyWard.NCurses.Tests.SingleByte
{
    public class SingleByteInsertTest : InsertTest, IClassFixture<SingleByteStdScrState>
    {
        protected override char TestChar => 'a';

        protected override string TestString => "test";

        public SingleByteInsertTest(ITestOutputHelper testOutputHelper, SingleByteStdScrState singleByteStdScrState)
            : base(testOutputHelper, singleByteStdScrState)
        {

        }
    }
}
