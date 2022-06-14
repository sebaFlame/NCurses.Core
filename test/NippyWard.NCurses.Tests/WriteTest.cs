using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;
using NippyWard.NCurses.Interop;

namespace NippyWard.NCurses.Tests
{
    public abstract class WriteTest : TestBase
    {
        protected abstract char TestChar { get; }
        protected abstract string TestString { get; }

        public WriteTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {
            
        }

        [Fact]
        public void WriteCharTest()
        {
            this.Window.Write(this.TestChar);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(1, this.Window.CursorColumn);

            char resultChar = this.Window.ExtractChar(0, 0);
            Assert.Equal(this.TestChar, resultChar);
        }

        [Fact]
        public void MoveAndWriteCharTest()
        {
            this.Window.Write(1, 1, this.TestChar);

            Assert.Equal(1, this.Window.CursorLine);
            Assert.Equal(1 + 1, this.Window.CursorColumn);

            char resultChar = this.Window.ExtractChar(1, 1);
            Assert.Equal(this.TestChar, resultChar);
        }

        [Fact]
        public void WriteCharWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this.TestChar, 0, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(1, this.Window.CursorColumn);

            this.Window.ExtractChar(0, 0, out INCursesChar resultChar);
            Assert.Equal(this.TestChar, this.Window.ExtractChar(resultChar));
            Assert.Equal(0, (int)resultChar.Attributes);
            Assert.Equal(4, resultChar.ColorPair);
        }

        [Fact]
        public void WriteCharWithAttributesTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this.TestChar, Attrs.BOLD, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(1, this.Window.CursorColumn);

            this.Window.ExtractChar(0, 0, out INCursesChar resultChar);
            Assert.Equal(this.TestChar, this.Window.ExtractChar(resultChar));
            Assert.Equal(Attrs.BOLD, resultChar.Attributes);
            Assert.Equal(4, resultChar.ColorPair);
        }

        [Fact]
        public void WriteStringTest()
        {
            this.Window.Write(this.TestString);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            string resultString = this.Window.ExtractString(0, 0, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void MoveAndWriteStringTest()
        {
            this.Window.Write(1, 1, this.TestString);

            Assert.Equal(1, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length + 1, this.Window.CursorColumn);

            string resultString = this.Window.ExtractString(1, 1, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void WriteStringWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this.TestString, 0, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(0, (int)resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }

        [Fact]
        public void WriteStringWithAttributesTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this.TestString, Attrs.BOLD | Attrs.ITALIC, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }

        [Fact]
        public void WriteStringWithColorAndVerifyEquality()
        {
            Assert.True(this.StdScrState.SupportsColor);

            INCursesCharString managedString = this.Window.CreateString(this.TestString, Attrs.BOLD | Attrs.ITALIC, 4);

            this.Window.Write(managedString);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(managedString.Length, this.Window.CursorColumn);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);

            Assert.StrictEqual(resultNCursesString, managedString);
        }
    }
}
