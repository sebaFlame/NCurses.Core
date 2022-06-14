using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

namespace NippyWard.NCurses.Tests
{
    public abstract class TestBase : IDisposable
    {
        public ITestOutputHelper TestOutputHelper { get; }
        public StdScrState StdScrState { get; }

        public IWindow Window => this.StdScrState.CurrentStandardScreen;

        protected TestBase(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
        {
            this.StdScrState = stdScrState;
            this.TestOutputHelper = testOutputHelper;
        }

        public void Dispose()
        {
            this.Window.Clear();
        }
    }
}
