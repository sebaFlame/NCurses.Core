using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace NippyWard.NCurses.Tests.Model
{
    public class MultiByteStdScrState : StdScrState
    {
        public override IWindow CurrentStandardScreen { get; }

        public MultiByteStdScrState()
            : base()
        {
            Assert.True(this.SupportsUnicode);

            this.CurrentStandardScreen = this.StdScr.ToMultiByteWindow();
        }
    }
}
