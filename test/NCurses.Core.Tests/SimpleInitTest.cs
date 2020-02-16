using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests
{
    public class SimpleInitTest : IDisposable
    {
        public IWindow Window { get; }

        public SimpleInitTest()
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