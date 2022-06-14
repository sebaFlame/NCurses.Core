using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;

using NippyWard.NCurses.Interop;
using NippyWard.NCurses.Interop.MultiByte;

namespace NippyWard.NCurses.Tests.MultiByte
{
    public class CharacterCreationTest : TestBase, IClassFixture<MultiByteStdScrState>
    {
        private char TestChar => '\u263A';

        public CharacterCreationTest(ITestOutputHelper testOutputHelper, MultiByteStdScrState multiByteStdScrState)
            : base(testOutputHelper, multiByteStdScrState)
        {

        }

        //TODO: sometimes crashes test host process on windows
        [Fact]
        public void SetCharacterTest()
        {
            NativeNCurses.NCurses.setcchar(out IMultiByteNCursesChar nativeWch, this.TestChar, 0, 0);
            INCursesChar managedWch = this.Window.CreateChar(this.TestChar, 0, 0);

            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeTest()
        {
            NativeNCurses.NCurses.setcchar(out IMultiByteNCursesChar nativeWch, this.TestChar, Attrs.BOLD | Attrs.ITALIC, 0);
            INCursesChar managedWch = this.Window.CreateChar(this.TestChar, Attrs.BOLD | Attrs.ITALIC);

            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            NativeNCurses.NCurses.setcchar(out IMultiByteNCursesChar nativeWch, this.TestChar, Attrs.BOLD | Attrs.ITALIC, 4);
            INCursesChar managedWch = this.Window.CreateChar(this.TestChar, Attrs.BOLD | Attrs.ITALIC, 4);

            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void GetCharacterTest()
        {
            INCursesChar managedWch = this.Window.CreateChar(this.TestChar, 0, 0);
            IMultiByteNCursesChar multibyteWch = Assert.IsAssignableFrom<IMultiByteNCursesChar>(managedWch);

            NativeNCurses.NCurses.getcchar(multibyteWch, out char resChar, out ulong resAttrs, out ushort resPair);

            Assert.Equal(this.TestChar, resChar);
            Assert.Equal((ulong)0, resAttrs);
            Assert.Equal(0, resPair);
        }

        [Fact]
        public void GetCharacterAttributeTest()
        {
            ulong attrs = Attrs.BOLD | Attrs.ITALIC;
            INCursesChar managedWch = this.Window.CreateChar(this.TestChar, attrs, 0);
            IMultiByteNCursesChar multibyteWch = Assert.IsAssignableFrom<IMultiByteNCursesChar>(managedWch);

            NativeNCurses.NCurses.getcchar(multibyteWch, out char resChar, out ulong resAttrs, out ushort resPair);

            Assert.Equal(this.TestChar, resChar);
            Assert.Equal(attrs, resAttrs);
            Assert.Equal(0, resPair);
        }

        [Fact]
        public void GetCharacterAttributeColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            ulong attrs = Attrs.BOLD | Attrs.ITALIC;
            ushort pair = 4;
            INCursesChar managedWch = this.Window.CreateChar(this.TestChar, attrs, pair);
            IMultiByteNCursesChar multibyteWch = Assert.IsAssignableFrom<IMultiByteNCursesChar>(managedWch);

            NativeNCurses.NCurses.getcchar(multibyteWch, out char resChar, out ulong resAttrs, out ushort resPair);

            Assert.Equal(this.TestChar, resChar);
            Assert.Equal(attrs, resAttrs);
            Assert.Equal(pair, resPair);
        }
    }
}
