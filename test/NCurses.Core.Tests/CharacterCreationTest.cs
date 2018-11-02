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
            WideCharFactory.Instance.GetNativeChar(testChar, out INCursesWCHAR managedWch);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeTest()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            NativeNCurses.setcchar(out INCursesWCHAR nativeWch, testChar, Attrs.BOLD | Attrs.ITALIC, 0);
            WideCharFactory.Instance.GetNativeChar(testChar, Attrs.BOLD | Attrs.ITALIC, out INCursesWCHAR managedWch);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeColorTest()
        {
            if (this.TestUnicode() || this.TestColor())
                return;

            char testChar = '\u263A';
            NativeNCurses.setcchar(out INCursesWCHAR nativeWch, testChar, Attrs.BOLD | Attrs.ITALIC, 4);
            WideCharFactory.Instance.GetNativeChar(testChar, Attrs.BOLD | Attrs.ITALIC, 4, out INCursesWCHAR managedWch);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void GetCharacterTest()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            WideCharFactory.Instance.GetNativeChar(testChar, out INCursesWCHAR managedWch);
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
            WideCharFactory.Instance.GetNativeChar(testChar, attrs, out INCursesWCHAR managedWch);
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
            WideCharFactory.Instance.GetNativeChar(testChar, attrs, pair, out INCursesWCHAR managedWch);
            NativeNCurses.getcchar(managedWch, out char resChar, out ulong resAttrs, out short resPair);
            Assert.Equal(testChar, resChar);
            Assert.Equal(attrs, resAttrs);
            Assert.Equal(pair, resPair);
        }
    }
}
