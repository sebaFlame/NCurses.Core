using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

using NCurses.Core.Interop;

namespace NCurses.Core.Tests
{
    public abstract class ACSTest : TestBase
    {
        public ACSTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {

        }
    }
}
