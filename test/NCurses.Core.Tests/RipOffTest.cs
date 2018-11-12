using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace NCurses.Core.Tests
{
    public class RipOffTest : IDisposable
    {
        protected readonly ITestOutputHelper OutputHelper;
        private Window ripoffExecuted;

        public RipOffTest(ITestOutputHelper outputHelper)
        {
            this.OutputHelper = outputHelper;
        }

        [Fact]
        public void TestRipOff()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.OutputHelper.WriteLine("Skipping TestReadCharMultiByte on Windows (multibyte unget does not work).");
                return;
            }

            NCurses.RipOffLine(-1, this.ripoffAssign);
            NCurses.Start();

            Assert.NotNull(ripoffExecuted);
            ripoffExecuted.Write("test1");

            //TODO: crashes when it tries to extract "test" (4 chars)
            string resultString = ripoffExecuted.ExtractString(0, 0, 5, out int read);
            Assert.Equal(5, read);
            Assert.Equal("test1", resultString);
        }

        private void ripoffAssign(Window window, int columns)
        {
            this.ripoffExecuted = window;
        }

        public void Dispose()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;

            NCurses.End();
        }
    }
}
