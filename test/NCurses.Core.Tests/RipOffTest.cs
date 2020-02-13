using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace NCurses.Core.Tests
{
    public class RipOffTest
    {
        protected readonly ITestOutputHelper OutputHelper;
        private IWindow ripoffExecuted;

        public RipOffTest(ITestOutputHelper outputHelper)
        {
            this.OutputHelper = outputHelper;
        }

        [SkipWindowsFact("Skipping TestRipOff on Windows")]
        public void TestRipOff()
        {
            NCurses.RipOffLine(-1, this.ripoffAssign);
            NCurses.Start();

            Assert.NotNull(ripoffExecuted);
            ripoffExecuted.Write("test1");

            //TODO: crashes when it tries to extract "test" (4 chars)
            string resultString = ripoffExecuted.ExtractString(0, 0, 5, out int read);
            Assert.Equal(5, read);
            Assert.Equal("test1", resultString);

            NCurses.End();
        }

        private void ripoffAssign(IWindow window, int columns)
        {
            this.ripoffExecuted = window;
        }
    }
}
