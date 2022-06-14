using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop;

namespace NippyWard.NCurses.Tests
{
    public abstract class MouseTest : TestBase
    {
        public MouseTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            : base(testOutputHelper, stdScrState)
        {
            this.Window.KeyPad = true;
        }

        [Fact]
        public void TestMouseEvent()
        {
            ulong newMouseState = NCurses.EnableMouseMask(MouseState.ALL_MOUSE_EVENTS, out ulong oldMouseState);

            Assert.True(this.StdScrState.SupportsMouse);

            //Assert.NotEqual(oldMouseState, newMouseState);

            MouseEventFactory.Instance.GetMouseEvent(1, 1, 1, 1, MouseState.BUTTON1_CLICKED, out IMEVENT mouseEvent);
            NativeNCurses.NCurses.ungetmouse(mouseEvent);

            Assert.True(this.Window.ReadKey(out char resultChar, out Key resultKey));
            Assert.Equal(Key.MOUSE, resultKey);

            NCurses.GetMouseEvent(out IMEVENT resultMouseEvent);

            Assert.Equal(mouseEvent.ID, resultMouseEvent.ID);
            Assert.Equal(mouseEvent.X, resultMouseEvent.X);
            Assert.Equal(mouseEvent.Y, resultMouseEvent.Y);
            Assert.Equal(mouseEvent.Z, resultMouseEvent.Z);
            Assert.Equal(mouseEvent.BState, resultMouseEvent.BState);

            Assert.StrictEqual(mouseEvent, resultMouseEvent);
        }
    }
}
