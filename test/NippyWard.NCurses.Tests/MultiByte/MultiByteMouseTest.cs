using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

namespace NippyWard.NCurses.Tests.MultiByte
{
    [Collection("Default")]
    public class MultiByteMouseTest : MouseTest
    {
        public MultiByteMouseTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {

        }

        protected override IWindow GenerateWindow(IWindow window)
        {
            return window.ToMultiByteWindow();
        }
    }
}
