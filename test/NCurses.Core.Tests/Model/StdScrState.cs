using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace NCurses.Core.Tests.Model
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

        protected IWindow StdScr { get; }

        public StdScrState()
        {
            this.StdScr = NCurses.Start();

            NCurses.Echo = false;
            this.StdScr.KeyPad = true;
            this.StdScr.Meta = true;

            if (this.SupportsColor)
            {
                this.InitializeColor();
            }
        }



        public void InitializeColor()
        {
            NCurses.StartColor();
            NCurses.InitDefaultPairs();
        }

        public void Dispose()
        {
            NCurses.End();
        }
    }
}
