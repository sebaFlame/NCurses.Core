using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace NippyWard.NCurses.Tests.Model
{
    public abstract class StdScrState : IDisposable
    {
        public abstract IWindow CurrentStandardScreen { get; }

        //works without NCurses intialization
        public bool SupportsUnicode => NCurses.UnicodeSupported;

        //CAN work without NCurses initialization
        public bool SupportsColor => NCurses.HasColor;

        //DOES NOT work without NCurses initialization
        public bool SupportsMouse => NCurses.HasMouse;

        public int ColorCount { get; private set; }

        internal IWindow StdScr { get; }

        public StdScrState()
        {
            this.StdScr = NCurses.Start();

            if (this.SupportsColor)
            {
                this.InitializeColor();
            }
        }

        public void InitializeColor()
        {
            NCurses.StartColor();
            this.ColorCount = NCurses.InitDefaultPairs();
        }

        public void Dispose()
        {
            this.CurrentStandardScreen.Dispose();

            this.StdScr.Dispose();

            NCurses.End();
        }
    }
}
