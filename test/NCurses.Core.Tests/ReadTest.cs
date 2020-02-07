using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

using NCurses.Core.Interop;

namespace NCurses.Core.Tests
{
    public abstract class ReadTest : TestBase
    {
        protected abstract char TestChar { get; }

        public ReadTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {

        }

        [Fact]
        public void ReadCharTest()
        {
            char resultChar;
            using (NCurses.CreateThreadSafeDisposable())
            {
                Assert.True(NativeNCurses.EnableLocking);
                this.Window.Put(this.TestChar);
                Assert.False(this.Window.ReadKey(out resultChar, out Key resultKey));
            }
            Assert.Equal(this.TestChar, resultChar);
        }

        [Fact]
        public void ReadFunctionKeyTest()
        {
            char resultChar;
            Key resultKey;
            using (NCurses.CreateThreadSafeDisposable())
            {
                Assert.True(NativeNCurses.EnableLocking);
                this.Window.Put((int)Key.F1);
                Assert.True(this.Window.ReadKey(out resultChar, out resultKey));
            }
            Assert.Equal(Key.F1, resultKey);
        }
    }
}
