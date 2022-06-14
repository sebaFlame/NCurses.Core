using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

using NippyWard.NCurses.Interop;

namespace NippyWard.NCurses.Tests.SingleByte
{
    public class SingleByteACSTest : ACSTest, IClassFixture<SingleByteStdScrState>
    {
        public SingleByteACSTest(ITestOutputHelper testOutputHelper, SingleByteStdScrState singleByteStdScrState)
            : base(testOutputHelper, singleByteStdScrState)
        {

        }

        [Fact]
        public void WriteACSTest()
        {
            this.Window.Write(Acs.ULCORNER);
            char resultChar = this.Window.ExtractChar(0, 0);

            //does not equal what's rendered
            Assert.Equal('l', resultChar);
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
            {
                lstChar.Add(this.Window.ExtractChar((INCursesChar)prop.GetValue(null)));
            }

            Assert.Equal(verifyList.Count, lstChar.Count);
            Assert.Equal<char>(verifyList, lstChar);
        }
    }
}
