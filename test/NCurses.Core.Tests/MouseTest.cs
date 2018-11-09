using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop;
using System.Reflection;

namespace NCurses.Core.Tests
{
    public class MouseTest : TestBase
    {
        public MouseTest(ITestOutputHelper outputHelper)
            : base(outputHelper) { }

        [Fact]
        public void TestMouseEvent()
        {
            ulong newMouseState = NCurses.EnableMouseMask(MouseState.ALL_MOUSE_EVENTS, out ulong oldMouseState);

            if (this.TestMouse())
                return;

            Assert.Equal((ulong)0, oldMouseState);
            Assert.NotEqual((ulong)0, newMouseState);

            MouseEventFactory.Instance.GetMouseEvent(1, 1, 1, 1, MouseState.BUTTON1_CLICKED, out IMEVENT mouseEvent);
            NativeNCurses.ungetmouse(mouseEvent);

            Assert.True(this.SingleByteStdScr.ReadKey(out char resultChar, out Key resultKey));
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
