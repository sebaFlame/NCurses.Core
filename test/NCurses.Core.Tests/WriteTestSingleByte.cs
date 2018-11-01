using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using NCurses.Core.Interop;
using NCurses.Core.StdScr;

namespace NCurses.Core.Tests
{
    public class WriteTestSingleByte : TestBase
    {
        public WriteTestSingleByte(ITestOutputHelper outputHelper)
            :base(outputHelper) { }

        [Fact]
        public void TestWriteCharSingleByte()
        {
           char testChar = 'a';
           this.SingleByteStdScr.Write(testChar);
           char resultChar = this.SingleByteStdScr.ExtractChar(0, 0);
           Assert.Equal(testChar, resultChar);
        }

        [Fact]
        public void TestMoveWriteCharSingleByte()
        {
           char testChar = 'a';
           this.SingleByteStdScr.Write(1, 1, testChar);
           char resultChar = this.SingleByteStdScr.ExtractChar(1, 1);
           Assert.Equal(testChar, resultChar);
        }

        [Fact]
        public void TestWriteCharColorSingleByte()
        {
           if (this.TestColor())
               return;

           char testChar = 'a';
           this.SingleByteStdScr.Write(testChar, Attrs.BOLD, 4);
           this.SingleByteStdScr.ExtractChar(0, 0, out INCursesChar resultChar);

           Assert.Equal(testChar, resultChar.Char);
           Assert.Equal(4, resultChar.Color);
           Assert.Equal(Attrs.BOLD, resultChar.Attributes);
        }

        [Fact]
        public void TestWriteStringSingleByte()
        {
           string testString = "test";
           this.SingleByteStdScr.Write(testString);
           string resultString = this.SingleByteStdScr.ExtractString(0, 0, testString.Length, out int read);
           Assert.Equal(testString, resultString);
        }

        [Fact]
        public void TestMoveWriteStringSingleByte()
        {
           string testString = "test";
           this.SingleByteStdScr.Write(1, 1, testString);
           string resultString = this.SingleByteStdScr.ExtractString(1, 1, testString.Length, out int read);
           Assert.Equal(testString, resultString);
        }

        //TODO: crashes visual studio debugger in Windows (on NCursesSCHAR.ToString()) using netcoreapp2.0
        [Fact]
        public void TestWriteStringColorSingleByte()
        {
            string testString = "test";
            this.SingleByteStdScr.Write(testString, Attrs.BOLD | Attrs.ITALIC, 4);
            this.SingleByteStdScr.ExtractString(0, 0, out INCursesCharStr resultNCursesString, testString.Length);

            Assert.Equal(testString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(testString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].Color);
        }
    }
}
