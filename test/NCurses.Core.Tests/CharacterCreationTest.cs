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
            NativeNCurses.setcchar(out IMultiByteChar nativeWch, testChar, 0, 0);
            MultiByteCharFactory.Instance.GetNativeChar(testChar, out IMultiByteChar managedWch);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeTest()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            NativeNCurses.setcchar(out IMultiByteChar nativeWch, testChar, Attrs.BOLD | Attrs.ITALIC, 0);
            MultiByteCharFactory.Instance.GetNativeChar(testChar, Attrs.BOLD | Attrs.ITALIC, out IMultiByteChar managedWch);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void SetCharacterAttributeColorTest()
        {
            if (this.TestUnicode() || this.TestColor())
                return;

            char testChar = '\u263A';
            NativeNCurses.setcchar(out IMultiByteChar nativeWch, testChar, Attrs.BOLD | Attrs.ITALIC, 4);
            MultiByteCharFactory.Instance.GetNativeChar(testChar, Attrs.BOLD | Attrs.ITALIC, 4, out IMultiByteChar managedWch);
            Assert.StrictEqual(nativeWch, managedWch);
        }

        [Fact]
        public void GetCharacterTest()
        {
            if (this.TestUnicode())
                return;

            char testChar = '\u263A';
            MultiByteCharFactory.Instance.GetNativeChar(testChar, out IMultiByteChar managedWch);
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
            MultiByteCharFactory.Instance.GetNativeChar(testChar, attrs, out IMultiByteChar managedWch);
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
            MultiByteCharFactory.Instance.GetNativeChar(testChar, attrs, pair, out IMultiByteChar managedWch);
            NativeNCurses.getcchar(managedWch, out char resChar, out ulong resAttrs, out short resPair);
            Assert.Equal(testChar, resChar);
            Assert.Equal(attrs, resAttrs);
            Assert.Equal(pair, resPair);
        }
    }
}
