using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using Xunit;
using Xunit.Abstractions;

namespace NippyWard.NCurses.Tests
{
    public class RipOffTest
    {
        protected readonly ITestOutputHelper OutputHelper;
        private IWindow ripoffExecuted;

        public RipOffTest(ITestOutputHelper outputHelper)
        {
            this.OutputHelper = outputHelper;
        }

        [Fact(Skip = "Can not be executed in batch testing?")]
        public void TestRipOff()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Assert.Throws<InvalidOperationException>(() => NCurses.RipOffLine(-1, this.ripoffAssign));
                return;
            }
                
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
