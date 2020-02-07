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
            char resultChar = this.Window.ExtractChar(0, 0);
            Assert.Equal(this.TestChar, resultChar);
        }

        [Fact]
        public void MoveAndWriteCharTest()
        {
            this.Window.Write(1, 1, this.TestChar);
            char resultChar = this.Window.ExtractChar(1, 1);
            Assert.Equal(this.TestChar, resultChar);
        }

        [Fact]
        public void WriteCharWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this.TestChar, Attrs.BOLD, 4);
            this.Window.Write(this.TestChar);
            this.Window.ExtractChar(0, 0, out INCursesChar resultChar);

            Assert.Equal(this.TestChar, resultChar.Char);
            Assert.Equal(Attrs.BOLD, resultChar.Attributes);
            Assert.Equal(4, resultChar.Color);
        }

        [Fact]
        public void WriteStringTest()
        {
            this.Window.Write(this.TestString);

            string resultString = this.Window.ExtractString(0, 0, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void MoveAndWriteStringTest()
        {
            this.Window.Write(1, 1, this.TestString);

            string resultString = this.Window.ExtractString(1, 1, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void WriteStringWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this.TestString, Attrs.BOLD | Attrs.ITALIC, 4);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].Color);
        }

        [Fact]
        public void WriteStringWithColorAndVerifyEquality()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.CreateString(this.TestString, Attrs.BOLD | Attrs.ITALIC, 4, out INCursesCharString managedString);

            this.Window.Write(managedString);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);

            Assert.StrictEqual(resultNCursesString, managedString);
        }
    }
}
