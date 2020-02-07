using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

using NCurses.Core.Interop;
using NCurses.Core.Interop.MultiByte;

namespace NCurses.Core.Tests.MultiByte
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
            NativeNCurses.setcchar(out IMultiByteChar nativeWch, this.TestChar, 0, 0);
            MultiByteCharFactory.Instance.GetNativeChar(this.TestChar, out IMultiByteChar managedWch);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeTest()
        {
            NativeNCurses.setcchar(out IMultiByteChar nativeWch, this.TestChar, Attrs.BOLD | Attrs.ITALIC, 0);
            MultiByteCharFactory.Instance.GetNativeChar(this.TestChar, Attrs.BOLD | Attrs.ITALIC, out IMultiByteChar managedWch);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            NativeNCurses.setcchar(out IMultiByteChar nativeWch, this.TestChar, Attrs.BOLD | Attrs.ITALIC, 4);
            MultiByteCharFactory.Instance.GetNativeChar(this.TestChar, Attrs.BOLD | Attrs.ITALIC, 4, out IMultiByteChar managedWch);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void GetCharacterTest()
        {
            MultiByteCharFactory.Instance.GetNativeChar(this.TestChar, out IMultiByteChar managedWch);
            NativeNCurses.getcchar(managedWch, out char resChar, out ulong resAttrs, out short resPair);
            Assert.Equal(this.TestChar, resChar);
            Assert.Equal((ulong)0, resAttrs);
            Assert.Equal(0, resPair);
        }

        [Fact]
        public void GetCharacterAttributeTest()
        {
            ulong attrs = Attrs.BOLD | Attrs.ITALIC;
            MultiByteCharFactory.Instance.GetNativeChar(this.TestChar, attrs, out IMultiByteChar managedWch);
            NativeNCurses.getcchar(managedWch, out char resChar, out ulong resAttrs, out short resPair);
            Assert.Equal(this.TestChar, resChar);
            Assert.Equal(attrs, resAttrs);
            Assert.Equal(0, resPair);
        }

        [Fact]
        public void GetCharacterAttributeColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            ulong attrs = Attrs.BOLD | Attrs.ITALIC;
            short pair = 4;
            MultiByteCharFactory.Instance.GetNativeChar(this.TestChar, attrs, pair, out IMultiByteChar managedWch);
            NativeNCurses.getcchar(managedWch, out char resChar, out ulong resAttrs, out short resPair);
            Assert.Equal(this.TestChar, resChar);
            Assert.Equal(attrs, resAttrs);
            Assert.Equal(pair, resPair);
        }
    }
}
