using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

namespace NippyWard.NCurses.Tests.SingleByte
{
    [Collection("Default")]
    public class SingleByteReadTest : ReadTest
    {
        protected override char TestChar => 'a';

        public SingleByteReadTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {

        }

        protected override IWindow GenerateWindow(IWindow window)
        {
            return window.ToSingleByteWindow();
        }
    }
}
