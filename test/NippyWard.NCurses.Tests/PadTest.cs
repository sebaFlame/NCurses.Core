using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

namespace NippyWard.NCurses.Tests
{
    public abstract class PadTest : TestBase
    {
        private const string loremString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

        public PadTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {
            
        }

        [Fact]
        public void PadCreationTest()
        {
            string testString = (this.Window.MaxColumn < loremString.Length + 3) ? loremString.Substring(0, this.Window.MaxColumn - 3) : loremString;
            IPad pad = NCurses.CreatePad(this.Window, 200, 200);

            for (int i = 1; i <= 200; i++)
            {
                pad.Write($"{i.ToString().PadLeft(3, ' ')}{testString}");

                if(i < 200)
                {
                    pad.MoveCursor(pad.CursorLine + 1, 0);
                }
            }

            string resString = pad.ExtractString(0, 0, 3, out int read);
            if (int.TryParse(resString.Trim(' '), out int lineNumber))
            {
                Assert.Equal(1, lineNumber);
            }

            resString = pad.ExtractString(150, 0, 3, out read);
            if (int.TryParse(resString.Trim(' '), out lineNumber))
            {
                Assert.Equal(151, lineNumber);
            }

            pad.Dispose();
        }
    }
}
