using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests
{
    public abstract class WindowTest : TestBase
    {
        protected abstract string TestString { get; }

        public WindowTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        { }

        [Fact]
        public void Window_Creation_Write_Clear_Test()
        {
            IWindow win1 = NCurses.CreateWindow(20, 20, 0, 0);

            win1.Write(this.TestString);
            win1.Clear();
            win1.Dispose();
        }

        [Fact]
        public void MultipleWindow_Creation_Write_Clear_Test()
        {
            IWindow win1 = NCurses.CreateWindow(20, 20, 0, 0);
            IWindow win2 = NCurses.CreateWindow(20, 20, 0, 0);
            IWindow win3 = NCurses.CreateWindow(20, 20, 0, 0);

            win1.Write(this.TestString);
            win1.Clear();

            win2.Write(this.TestString);
            win2.Clear();

            win3.Write(this.TestString);
            win3.Clear();

            win1.Dispose();
            win2.Dispose();
            win3.Dispose();
        }

        [Fact]
        public void SubWindow_Creation_Write_Clear_Test()
        {
            IWindow win1 = this.Window.SubWindow(20, 20, 0, 0);

            win1.Write(this.TestString);
            win1.Clear();
            win1.Dispose();
        }

        [Fact]
        public void MultipleSubWindow_Creation_Write_Clear_Test()
        {
            IWindow win1 = this.Window.SubWindow(20, 20, 0, 0);
            IWindow win2 = this.Window.SubWindow(20, 20, 0, 0);
            IWindow win3 = this.Window.SubWindow(20, 20, 0, 0);

            win1.Write(this.TestString);
            win1.Clear();

            win2.Write(this.TestString);
            win2.Clear();

            win3.Write(this.TestString);
            win3.Clear();

            win1.Dispose();
            win2.Dispose();
            win3.Dispose();
        }

        [Fact]
        public void SubSubWindow_Creation_Write_Clear_Test()
        {
            IWindow win1 = this.Window.SubWindow(20, 20, 0, 0);

            IWindow subWindow = win1.SubWindow(20, 20, 0, 0);

            subWindow.Write(this.TestString);
            subWindow.Clear();
            subWindow.Dispose();

            win1.Dispose();
        }
    }
}
