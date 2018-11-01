using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using NCurses.Core.Interop;
using NCurses.Core.Interop.MultiByte;

namespace NCurses.Core.Tests
{
    public class WriteTestMultiByte : TestBase
    {
        public WriteTestMultiByte(ITestOutputHelper outputHelper)
            :base(outputHelper) { }

        [Fact]
        public void TestWriteCharMultiByte()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            this.MultiByteStdScr.Write(testChar);
            char resultChar = this.MultiByteStdScr.ExtractChar(0, 0);
            Assert.Equal(testChar, resultChar);
        }

        [Fact]
        public void TestMoveWriteCharMultiByte()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            this.MultiByteStdScr.Write(1, 1, testChar);
            char resultChar = this.MultiByteStdScr.ExtractChar(1, 1);
            Assert.Equal(testChar, resultChar);
        }

        [Fact]
        public void TestWriteCharColorMultiByte()
        {
            if (this.TestUnicode() || this.TestColor())
                return;

            char testChar = '\u263A';
            this.MultiByteStdScr.Write(testChar, Attrs.BOLD, 4);
            this.MultiByteStdScr.Write(testChar);
            this.MultiByteStdScr.ExtractChar(0, 0, out INCursesChar resultChar);

            Assert.Equal(testChar, resultChar.Char);
            Assert.Equal(Attrs.BOLD, resultChar.Attributes);
            Assert.Equal(4, resultChar.Color);
        }

        [Fact]
        public void TestWriteStringMultiByte()
        {
            if (this.TestUnicode())
                return;

            string testString = new string(new char[] { '\u0490', '\u0491', '\u0492', '\u0493', '\u0494', '\u0495', '\u0496', '\u0497', '\u0498', '\u0499'
                , '\u049A', '\u049B', '\u049C', '\u049D', '\u049E', '\u049F' });
            this.MultiByteStdScr.Write(testString);

            string resultString = this.MultiByteStdScr.ExtractString(0, 0, testString.Length, out int read);
            Assert.Equal(testString, resultString);
            Assert.Equal(testString.Length, read);
        }

        [Fact]
        public void TestMoveWriteStringMultiByte()
        {
            if (this.TestUnicode())
                return;

            string testString = new string(new char[] { '\u0490', '\u0491', '\u0492', '\u0493', '\u0494', '\u0495', '\u0496', '\u0497', '\u0498', '\u0499'
                , '\u049A', '\u049B', '\u049C', '\u049D', '\u049E', '\u049F' });
            this.MultiByteStdScr.Write(1, 1, testString);

            string resultString = this.MultiByteStdScr.ExtractString(1, 1, testString.Length, out int read);
            Assert.Equal(testString, resultString);
            Assert.Equal(testString.Length, read);
        }

        [Fact]
        public void TestWriteStringColorMultiByte()
        {
            if (this.TestUnicode())
                return;

            string testString = new string(new char[] { '\u0490', '\u0491', '\u0492', '\u0493', '\u0494', '\u0495', '\u0496', '\u0497', '\u0498', '\u0499'
                , '\u049A', '\u049B', '\u049C', '\u049D', '\u049E', '\u049F' });
            this.MultiByteStdScr.Write(testString, Attrs.BOLD | Attrs.ITALIC, 4);

            this.MultiByteStdScr.ExtractString(0, 0, out INCursesCharStr resultNCursesString, testString.Length);
            Assert.Equal(testString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(testString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].Color);
        }
    }
}
