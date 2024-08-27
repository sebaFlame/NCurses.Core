using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

namespace NippyWard.NCurses.Tests
{
    [Collection("Default")]
    public class InitTest : TestBase
    {
        public InitTest(ITestOutputHelper testOutputHelper, StdScrState state)
            : base(testOutputHelper, state)
        { }

        protected override IWindow GenerateWindow(IWindow window)
        {
            return window;
        }

        [Fact]
        public void SimpleInitTestTest()
        {
            this.Window.Write("Hello world!");
        }
    }
}