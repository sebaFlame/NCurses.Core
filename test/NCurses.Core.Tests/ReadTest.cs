using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using NCurses.Core.Interop;

namespace NCurses.Core.Tests
{
    public class ReadTest : TestBase
    {
        public ReadTest(ITestOutputHelper outputHelper)
            : base(outputHelper) { }

        //TODO: doesn't work on windows (blocks on wget_wch)
        // [Fact]
        // public void TestReadCharMultiByte()
        // {
        //   if (this.TestUnicode())
        //       return;

        //   char testChar = '\u263A';
        //   int bleh = testChar;
        //   NativeNCurses.unget_wch(testChar);
        //   Assert.False(this.MultiByteStdScr.ReadKey(out char resultChar, out Key resultKey));
        //   Assert.Equal(testChar, resultChar);
        // }

        [Fact]
        public void TestReadCharSingleByte()
        {
            char testChar = 'a';
            char resultChar;
            using (NCurses.CreateThreadSafeDisposable())
            {
                Assert.True(NativeNCurses.EnableLocking);
                NativeNCurses.ungetch(testChar);
                Assert.False(this.SingleByteStdScr.ReadKey(out resultChar, out Key resultKey));
            }
            Assert.Equal(testChar, resultChar);
        }

        [Fact]
        public void TestReadFunctionKey()
        {
            char resultChar;
            Key resultKey;
            using (NCurses.CreateThreadSafeDisposable())
            {
                Assert.True(NativeNCurses.EnableLocking);
                NativeNCurses.ungetch((int)Key.F1);
                Assert.True(this.SingleByteStdScr.ReadKey(out resultChar, out resultKey));
            }
            Assert.Equal(Key.F1, resultKey);
        }
    }
}
