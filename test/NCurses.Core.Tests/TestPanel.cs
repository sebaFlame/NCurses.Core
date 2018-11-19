using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace NCurses.Core.Tests
{
    public class TestPanel : TestBase
    {
        public TestPanel(ITestOutputHelper outputHelper)
            : base(outputHelper) { }

        [Fact]
        public void TestPanelOrder()
        {
            Window win1, win2, win3, win4;
            win1 = Window.CreateWindow(20, 20, 0, 0);
            win2 = Window.CreateWindow(20, 20, 0, 0);
            win3 = Window.CreateWindow(20, 20, 0, 0);
            win4 = Window.CreateWindow(20, 20, 0, 0);

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
