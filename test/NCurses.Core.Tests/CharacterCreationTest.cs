using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using NCurses.Core.Interop;
using NCurses.Core.Interop.MultiByte;

namespace NCurses.Core.Tests
{
    public class CharacterCreationTest : TestBase
    {
        public CharacterCreationTest(ITestOutputHelper outputHelper)
            : base(outputHelper) { }

        [Fact]
        public void SetCharacterTest()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            NativeNCurses.setcchar(out INCursesWCHAR nativeWch, testChar, 0, 0);
            INCursesWCHAR managedWch = WideCharFactory.GetWideChar(testChar);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeTest()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            NativeNCurses.setcchar(out INCursesWCHAR nativeWch, testChar, Attrs.BOLD | Attrs.ITALIC, 0);
            INCursesWCHAR managedWch = WideCharFactory.GetWideChar(testChar, Attrs.BOLD | Attrs.ITALIC);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeColorTest()
        {
            if (this.TestUnicode() || this.TestColor())
                return;

            char testChar = '\u263A';
            NativeNCurses.setcchar(out INCursesWCHAR nativeWch, testChar, Attrs.BOLD | Attrs.ITALIC, 4);
            INCursesWCHAR managedWch = WideCharFactory.GetWideChar(testChar, Attrs.BOLD | Attrs.ITALIC, 4);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void GetCharacterTest()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            INCursesWCHAR managedWch = WideCharFactory.GetWideChar(testChar);
            NativeNCurses.getcchar(managedWch, out char resChar, out ulong resAttrs, out short resPair);
            Assert.Equal(testChar, resChar);
            Assert.Equal((ulong)0, resAttrs);
            Assert.Equal(0, resPair);
        }

        [Fact]
        public void GetCharacterAttributeTest()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            ulong attrs = Attrs.BOLD | Attrs.ITALIC;
            INCursesWCHAR managedWch = WideCharFactory.GetWideChar(testChar, attrs);
            NativeNCurses.getcchar(managedWch, out char resChar, out ulong resAttrs, out short resPair);
            Assert.Equal(testChar, resChar);
            Assert.Equal(attrs, resAttrs);
            Assert.Equal(0, resPair);
        }

        [Fact]
        public void GetCharacterAttributeColorTest()
        {
            if (this.TestUnicode() || this.TestColor())
                return;

            char testChar = '\u263A';
            ulong attrs = Attrs.BOLD | Attrs.ITALIC;
            short pair = 4;
            INCursesWCHAR managedWch = WideCharFactory.GetWideChar(testChar, attrs, pair);
            NativeNCurses.getcchar(managedWch, out char resChar, out ulong resAttrs, out short resPair);
            Assert.Equal(testChar, resChar);
            Assert.Equal(attrs, resAttrs);
            Assert.Equal(pair, resPair);
        }
    }
}
