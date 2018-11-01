using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using Xunit;
using NCurses.Core.Interop;

namespace NCurses.Core.Tests
{
    public abstract class TestBase : IDisposable
    {
        private IWindow _SingleByteStdScr;
        public IWindow SingleByteStdScr => _SingleByteStdScr ?? (_SingleByteStdScr = NCurses.SingleByteStdScr);

        private IWindow _MultiByteStdScr;
        protected IWindow MultiByteStdScr => _MultiByteStdScr ?? (_MultiByteStdScr = NCurses.MultiByteStdScr);

        protected readonly ITestOutputHelper OutputHelper;

        protected TestBase(ITestOutputHelper outputHelper)
        {
            this.OutputHelper = outputHelper;
            IWindow stdScr = NCurses.Start();

            //default options
            NCurses.CBreak = true;
            NCurses.Echo = false;
            stdScr.KeyPad = true;
            stdScr.Meta = true;
        }

        protected bool TestUnicode()
        {
            if (!NCurses.UnicodeSupported)
            {
                this.OutputHelper.WriteLine("Unicode not supported on this machine.");
                Assert.Throws<NotSupportedException>(() => this.MultiByteStdScr);
                return true;
            }
            return false;
        }

        protected bool TestColor()
        {
            if (!NCurses.HasColor)
            {
                this.OutputHelper.WriteLine("Color not supported on this machine.");
                Assert.Throws<NCursesException>(() => NCurses.StartColor());
                return true;
            }

            NCurses.StartColor();
            NCurses.InitDefaultPairs();
            return false;
        }

        public void Dispose()
        {
            this.SingleByteStdScr.Clear();
            NCurses.End();
        }
    }
}
