using System;
using System.Collections.Generic;
using System.Text;
using System.Buffers;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;
using NippyWard.NCurses.Interop;

namespace NippyWard.NCurses.Tests.SingleByte
{
    public class SingleByteWriteTest : WriteTest, IClassFixture<SingleByteStdScrState>
    {
        protected override char TestChar => 'a';
        protected override string TestString => "test";

        private ReadOnlyMemory<byte> _singleByteMemory;
        private ReadOnlySequence<byte> _singleByteSequence;

        public SingleByteWriteTest(ITestOutputHelper testOutputHelper, SingleByteStdScrState singleByteStdScrState)
            : base(testOutputHelper, singleByteStdScrState)
        {
            TestSequenceSegment startSegment = null, endSegment = null;

            byte[] bytes = new byte[this.TestString.Length];
            for (int i = 0; i < this.TestString.Length; i++)
            {
                bytes[i] = (byte)this.TestString[i];

                if (startSegment is null)
                {
                    startSegment = new TestSequenceSegment(bytes[i]);
                    endSegment = startSegment;
                }
                else
                {
                    endSegment = endSegment.SetNext(new TestSequenceSegment(bytes[i]));
                }
            }

            this._singleByteMemory = new Memory<byte>(bytes);
            this._singleByteSequence = new ReadOnlySequence<byte>(startSegment, 0, endSegment, endSegment.Memory.Length);
        }

        [Fact]
        public void WriteASCIISpanTest()
        {
            this.Window.Write(this._singleByteMemory.Span);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            string resultString = this.Window.ExtractString(0, 0, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void WriteASCIISpanWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this._singleByteMemory.Span, 0, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn); //does not get advanced

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(0, (int)resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }

        [Fact]
        public void WriteASCIISpanWithAttributesTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this._singleByteMemory.Span, Attrs.BOLD | Attrs.ITALIC, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn); //does get advanced

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }

        [Fact]
        public void WriteASCIISequenceTest()
        {
            this.Window.Write(this._singleByteSequence);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            string resultString = this.Window.ExtractString(0, 0, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void WriteASCIISequenceWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this._singleByteSequence, 0, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn); //does not get advanced

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(0, (int)resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }

        [Fact]
        public void WriteASCIISequnceWithAttributesTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this._singleByteSequence, Attrs.BOLD | Attrs.ITALIC, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn); //does get advanced

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }
    }
}
