using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Tests.Model
{
    public class SingleByteStdScrState : StdScrState
    {
        public override IWindow CurrentStandardScreen { get; }

        public SingleByteStdScrState()
            : base()
        {
            this.CurrentStandardScreen = this.StdScr.ToSingleByteWindow();
        }
    }
}
