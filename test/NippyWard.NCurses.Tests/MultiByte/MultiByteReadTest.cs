using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

namespace NippyWard.NCurses.Tests.MultiByte
{
    public class MultiByteReadTest : ReadTest, IClassFixture<MultiByteStdScrState>
    {
        protected override char TestChar => '\u263A';

        public MultiByteReadTest(ITestOutputHelper testOutputHelper, MultiByteStdScrState multiByteStdScrState)
            : base(testOutputHelper, multiByteStdScrState)
        {

        }
    }
}
