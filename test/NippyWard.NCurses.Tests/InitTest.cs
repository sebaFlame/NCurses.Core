using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

namespace NippyWard.NCurses.Tests
{
    public class InitTest : IDisposable
    {
        public IWindow Window { get; }

        public InitTest()
        {
            this.Window  = NCurses.Start();
        }

        [Fact]
        public void SimpleInitTestTest()
        {
            this.Window.Dispose();
        }

        public void Dispose()
        {
            NCurses.End();
        }
    }
}