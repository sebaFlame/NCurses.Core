using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

using NCurses.Core.Interop;

namespace NCurses.Core.Tests
{
    public abstract class PanelTest : TestBase
    {
        public PanelTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        { }

        [Fact]
        public void TestPanelOrder()
        {
            IWindow win1, win2, win3, win4;
            win1 = this.Window.SubWindow(20, 20, 0, 0);
            win2 = this.Window.SubWindow(20, 20, 0, 0);
            win3 = this.Window.SubWindow(20, 20, 0, 0);
            win4 = this.Window.SubWindow(20, 20, 0, 0);

            IPanel panel1, panel2, panel3, resultPanel;
            panel1 = NCurses.CreatePanel(win1);
            panel2 = NCurses.CreatePanel(win2);

            resultPanel = panel2.Below();
            Assert.Equal(panel1, resultPanel);

            resultPanel = panel1.Above();
            Assert.Equal(panel2, resultPanel);

            panel3 = NCurses.CreatePanel(win3);

            panel1.Top();
            resultPanel = panel3.Above();
            Assert.Equal(panel1, resultPanel);

            panel2.WrappedWindow = win4;
            Assert.Equal(win4, panel2.WrappedWindow);

            panel3.Hide();
            Assert.True(panel3.Hidden);

            resultPanel = panel2.Above();
            Assert.Equal(panel1, resultPanel);

            panel1.Dispose();
            panel2.Dispose();
            panel3.Dispose();

            win1.Dispose();
            win2.Dispose();
            win3.Dispose();
            win4.Dispose();
        }
    }
}
