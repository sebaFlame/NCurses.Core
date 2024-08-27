using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;
using NippyWard.NCurses.Tests.MultiByte;

namespace NippyWard.NCurses.Tests
{
    public abstract class TestBase : IDisposable
    {
        public ITestOutputHelper TestOutputHelper { get; }
        public StdScrState StdScrState { get; }

        public IWindow Window { get; }

        private IWindow _originalWindow;

        protected TestBase(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
        {
            this._originalWindow = NCurses.CreateWindow();
            this.Window = this.GenerateWindow(this._originalWindow);

            this.StdScrState = stdScrState;
            this.TestOutputHelper = testOutputHelper;
        }

        protected abstract IWindow GenerateWindow(IWindow window);

        public void Dispose()
        {
            this._originalWindow.Dispose();

            if(!object.ReferenceEquals(this._originalWindow, this.Window))
            {
                this.Window.Dispose();
            }
        }
    }
}
