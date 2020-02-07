using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests
{
    //TODO: incorrect -> pass the current stdscr to pad creation
    public abstract class PadTest<TPad> : TestBase
        where TPad : Pad
    {
        private const string loremString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

        protected abstract Func<int, int, Pad> CreatePad { get; }

        public PadTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {

        }

        [Fact]
        public void PadCreationTest()
        {
            string testString = (this.Window.MaxColumn < loremString.Length + 3) ? loremString.Substring(0, this.Window.MaxColumn - 3) : loremString;
            Pad pad = this.CreatePad(200, 200);

            Assert.IsType<TPad>(pad);

            for (int i = 0; i < 200; i++)
            {
                pad.Write($"{i.ToString().PadLeft(3, ' ')}{testString}{(i < 199 ? "\n" : "")}");
            }

            string resString = pad.ExtractString(0, 0, 3, out int read);
            if (int.TryParse(resString.Trim(' '), out int lineNumber))
            {
                Assert.Equal(0, lineNumber);
            }

            resString = pad.ExtractString(150, 0, 3, out read);
            if (int.TryParse(resString.Trim(' '), out lineNumber))
            {
                Assert.Equal(150, lineNumber);
            }
        }
    }
}
