using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

using NCurses.Core.Interop;

namespace NCurses.Core.Tests
{
    public abstract class InsertTest : TestBase
    {
        protected abstract char TestChar { get; }
        protected abstract string TestString { get; }

        public InsertTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {
            
        }

        [Fact]
        public void InsertCharTest()
        {
            this.Window.Insert(this.TestChar);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(0, this.Window.CursorColumn);

            char resultChar = this.Window.ExtractChar(0, 0);
            Assert.Equal(this.TestChar, resultChar);
        }

        [Fact]
        public void MoveAndInsertCharTest()
        {
            this.Window.Insert(1, 1, this.TestChar);

            Assert.Equal(1, this.Window.CursorLine);
            Assert.Equal(1, this.Window.CursorColumn);

            char resultChar = this.Window.ExtractChar(1, 1);
            Assert.Equal(this.TestChar, resultChar);
        }

        [Fact]
        public void InsertCharWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Insert(this.TestChar, Attrs.BOLD, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(0, this.Window.CursorColumn);

            this.Window.ExtractChar(0, 0, out INCursesChar resultChar);
            Assert.Equal(this.TestChar, resultChar.Char);
            Assert.Equal(Attrs.BOLD, resultChar.Attributes);
            Assert.Equal(4, resultChar.ColorPair);
        }

        [Fact]
        public void InsertCharWithAttributesTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.AttributesOn(Attrs.BOLD | Constants.COLOR_PAIR(4));

            this.Window.Insert(this.TestChar);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(0, this.Window.CursorColumn);

            this.Window.ExtractChar(0, 0, out INCursesChar resultChar);
            Assert.Equal(this.TestChar, resultChar.Char);
            Assert.Equal(Attrs.BOLD, resultChar.Attributes);
            Assert.Equal(4, resultChar.ColorPair);

            this.Window.AttributesOff(Attrs.BOLD | Constants.COLOR_PAIR(4));
        }

        [Fact]
        public void InsertStringTest()
        {
            this.Window.Insert(this.TestString);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(0, this.Window.CursorColumn);

            string resultString = this.Window.ExtractString(0, 0, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void MoveAndInsertStringTest()
        {
            this.Window.Insert(1, 1, this.TestString);

            Assert.Equal(1, this.Window.CursorLine);
            Assert.Equal(1, this.Window.CursorColumn);

            string resultString = this.Window.ExtractString(1, 1, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void InsertStringWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Insert(this.TestString, Attrs.BOLD | Attrs.ITALIC, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(0, this.Window.CursorColumn);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }

        [Fact]
        public void InsertStringWithAttributesTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.AttributesOn(Attrs.BOLD | Attrs.ITALIC | Constants.COLOR_PAIR(4));

            this.Window.Insert(this.TestString);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(0, this.Window.CursorColumn);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);

            this.Window.AttributesOff(Attrs.BOLD | Attrs.ITALIC | Constants.COLOR_PAIR(4));
        }
    }
}
