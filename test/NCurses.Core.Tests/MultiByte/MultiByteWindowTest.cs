using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Xunit.Abstractions;

using NCurses.Core.Tests.Model;

namespace NCurses.Core.Tests.MultiByte
{
    public class MultiByteWindowTest : WindowTest, IClassFixture<MultiByteStdScrState>
    {
        protected override string TestString => new string(
            new char[]
            {
                        '\u0490',
                        '\u0491',
                        '\u0492',
                        '\u0493',
                        '\u0494',
                        '\u0495',
                        '\u0496',
                        '\u0497',
                        '\u0498',
                        '\u0499',
                        '\u049A',
                        '\u049B',
                        '\u049C',
                        '\u049D',
                        '\u049E',
                        '\u049F'
            });


        public MultiByteWindowTest(ITestOutputHelper testOutputHelper, MultiByteStdScrState multiByteStdScrState)
            : base(testOutputHelper, multiByteStdScrState)
        {

        }
    }
}
