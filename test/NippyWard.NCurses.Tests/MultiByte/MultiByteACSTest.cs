﻿using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

using NippyWard.NCurses.Interop;

namespace NippyWard.NCurses.Tests.MultiByte
{
    [Collection("Default")]
    public class MultiByteACSTest : ACSTest
    {
        public MultiByteACSTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {

        }

        protected override IWindow GenerateWindow(IWindow window)
        {
            return window.ToMultiByteWindow();
        }

        [Fact]
        public void TestWriteWACS()
        {
            this.Window.Write(Wacs.ULCORNER);
            char resultChar = this.Window.ExtractChar(0, 0);

            Assert.Equal('┌', resultChar);
        }

        [Fact]
        public void TestEnumerateWACS()
        {
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
            {
                lstChar.Add(this.Window.ExtractChar((INCursesChar)prop.GetValue(null)));
            }

            Assert.Equal(verifyList.Count, lstChar.Count);
            Assert.Equal<char>(verifyList, lstChar);
        }
    }
}
