using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

using NippyWard.NCurses.Interop;

namespace NippyWard.NCurses.Tests
{
    public abstract class ACSTest : TestBase
    {
        public ACSTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {

        }
    }
}
