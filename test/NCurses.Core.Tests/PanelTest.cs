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
    //TODO: incorrect -> pass the current stdscr to panel/window creation
    public abstract class PanelTest : TestBase
    {
        protected abstract Func<int, int, int, int, Window> CreateWindow { get; }

        public PanelTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {

        }

        [Fact]
        public void TestPanelOrder()
        {
            Window win1, win2, win3, win4;
            win1 = this.CreateWindow(20, 20, 0, 0);
            win2 = this.CreateWindow(20, 20, 0, 0);
            win3 = this.CreateWindow(20, 20, 0, 0);
            win4 = this.CreateWindow(20, 20, 0, 0);

            Panel panel1, panel2, panel3, resultPanel;
            panel1 = new Panel(win1);
            panel2 = new Panel(win2);

            resultPanel = panel2.Below();
            Assert.Equal(panel1, resultPanel);

            resultPanel = panel1.Above();
            Assert.Equal(panel2, resultPanel);

            panel3 = new Panel(win3);

            panel1.Top();
            resultPanel = panel3.Above();
            Assert.Equal(panel1, resultPanel);

            panel2.Window = win4;
            Assert.Equal(win4, panel2.Window);

            panel3.Hide();
            Assert.True(panel3.Hidden);

            resultPanel = panel2.Above();
            Assert.Equal(panel1, resultPanel);
        }
    }
}
