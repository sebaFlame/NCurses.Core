using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Tests.Model
{
    public class SingleByteStdScrState : StdScrState
    {
        public override IWindow CurrentStandardScreen { get; }

        public SingleByteStdScrState()
            : base()
        {
            this.CurrentStandardScreen = NCurses.SingleByteStdScr;
        }
    }
}
