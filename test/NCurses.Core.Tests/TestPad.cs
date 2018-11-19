using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace NCurses.Core.Tests
{
    public class TestPad : TestBase
    {
        private const string loremString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

        public TestPad(ITestOutputHelper outputHelper)
            : base(outputHelper) { }

        [Fact]
        public void TestPadSingleByte()
        {
            string testString = loremString.Substring(0, this.StdScr.MaxColumn - 3);
            Pad pad = new SingleBytePad(200, 200);

            for (int i = 0; i < 200; i++)
                pad.Write($"{i.ToString().PadLeft(3, ' ')}{testString}{(i < 199 ? "\n" : "")}");

            pad.Refresh();

            string resString = pad.ExtractString(0, 0, 3, out int read);
            if (int.TryParse(resString.Trim(' '), out int lineNumber))
                Assert.Equal(0, lineNumber);

            resString = pad.ExtractString(150, 0, 3, out read);
            if (int.TryParse(resString.Trim(' '), out lineNumber))
                Assert.Equal(150, lineNumber);
        }

        [Fact]
        public void TestPadMultiByte()
        {
            if (this.TestUnicode())
                return;

            string testString = loremString.Substring(0, this.StdScr.MaxColumn - 3);
            Pad pad = Pad.CreatePad(200, 200);

            Assert.IsType<MultiBytePad>(pad);

            for (int i = 0; i < 200; i++)
                pad.Write($"{i.ToString().PadLeft(3, ' ')}{testString}{(i < 199 ? "\n" : "")}");

            string resString = pad.ExtractString(0, 0, 3, out int read);
            if (int.TryParse(resString.Trim(' '), out int lineNumber))
                Assert.Equal(0, lineNumber);

            resString = pad.ExtractString(150, 0, 3, out read);
            if (int.TryParse(resString.Trim(' '), out lineNumber))
                Assert.Equal(150, lineNumber);
        }
    }
}
