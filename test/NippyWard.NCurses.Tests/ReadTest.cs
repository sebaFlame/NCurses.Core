using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

using NippyWard.NCurses.Interop;

namespace NippyWard.NCurses.Tests
{
    public abstract class ReadTest : TestBase
    {
        protected abstract char TestChar { get; }

        public ReadTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {
            NCurses.Echo = false;
        }

        [Fact]
        public void ReadCharTest()
        {
            char resultChar;
            using (NCurses.CreateThreadSafeDisposable())
            {
                this.Window.Put(this.TestChar);
                Assert.False(this.Window.ReadKey(out resultChar, out Key resultKey));
            }
            Assert.Equal(this.TestChar, resultChar);
        }

        [Fact]
        public void ReadFunctionKeyTest()
        {
            this.Window.KeyPad = true;

            char resultChar;
            Key resultKey;
            using (NCurses.CreateThreadSafeDisposable())
            {
                this.Window.Put(Key.F1);
                Assert.True(this.Window.ReadKey(out resultChar, out resultKey));
            }
            Assert.Equal(Key.F1, resultKey);
        }
    }
}
