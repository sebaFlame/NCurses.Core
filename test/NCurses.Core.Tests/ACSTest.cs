using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using System.Reflection;

using NCurses.Core.Interop;

namespace NCurses.Core.Tests
{
    public class ACSTest : TestBase
    {
        public ACSTest(ITestOutputHelper outputHelper)
            : base(outputHelper) { }

        [Fact]
        public void TestWriteACSSingleByte()
        {
            this.SingleByteStdScr.Write(Acs.ULCORNER);
            char resultChar = this.SingleByteStdScr.ExtractChar(0, 0);

            //does not equal what's rendered
            Assert.Equal('l', resultChar);
        }

        [Fact]
        public void TestWriteACSMultiByte()
        {
            if (this.TestUnicode())
                return;

            this.MultiByteStdScr.Write(Acs.ULCORNER);
            char resultChar = this.SingleByteStdScr.ExtractChar(0, 0);

            //does not equal what's rendered
            Assert.Equal('l', resultChar);
        }

        [Fact]
        public void TestWriteWACS()
        {
            if (this.TestUnicode())
                return;

            this.MultiByteStdScr.Write(Wacs.ULCORNER);
            char resultChar = this.MultiByteStdScr.ExtractChar(0, 0);

            Assert.Equal('┌', resultChar);
        }

        [Fact]
        public void TestEnumerateACS()
        {
            List<char> lstChar = new List<char>();
            List<char> verifyList = new List<char>
            {
                (char)108,
                (char)109,
                (char)107,
                (char)106,
                (char)116,
                (char)117,
                (char)118,
                (char)119,
                (char)113,
                (char)120,
                (char)110,
                (char)111,
                (char)115,
                (char)96,
                (char)97,
                (char)102,
                (char)103,
                (char)126,
                (char)44,
                (char)43,
                (char)46,
                (char)45,
                (char)104,
                (char)105,
                (char)48,
                (char)112,
                (char)114,
                (char)121,
                (char)122,
                (char)123,
                (char)124,
                (char)125
            };

            foreach (PropertyInfo prop in typeof(Acs).GetProperties())
                lstChar.Add(((INCursesChar)prop.GetValue(null)).Char);

            Assert.Equal(verifyList.Count, lstChar.Count);
            Assert.Equal<char>(verifyList, lstChar);
        }

        [Fact]
        public void TestEnumerateWACS()
        {
            if (this.TestUnicode())
                return;

            List<char> lstChar = new List<char>();
            List<char> verifyList = new List<char>
            {
                (char)9484,
                (char)9492,
                (char)9488,
                (char)9496,
                (char)9500,
                (char)9508,
                (char)9524,
                (char)9516,
                (char)9472,
                (char)9474,
                (char)9532,
                (char)9146,
                (char)9149,
                (char)9670,
                (char)9618,
                (char)176,
                (char)177,
                (char)183,
                (char)8592,
                (char)8594,
                (char)8595,
                (char)8593,
                (char)9618,
                (char)9731,
                (char)9646,
                (char)9147,
                (char)9148,
                (char)8804,
                (char)8805,
                (char)960,
                (char)8800,
                (char)163,
                (char)9556,
                (char)9562,
                (char)9559,
                (char)9565,
                (char)9571,
                (char)9568,
                (char)9577,
                (char)9574,
                (char)9552,
                (char)9553,
                (char)9580,
                (char)9487,
                (char)9495,
                (char)9491,
                (char)9499,
                (char)9515,
                (char)9507,
                (char)9531,
                (char)9523,
                (char)9473,
                (char)9475,
                (char)9547
            };

            foreach (PropertyInfo prop in typeof(Wacs).GetProperties())
                lstChar.Add(((INCursesChar)prop.GetValue(null)).Char);

            Assert.Equal(verifyList.Count, lstChar.Count);
            Assert.Equal<char>(verifyList, lstChar);
        }
    }
}
