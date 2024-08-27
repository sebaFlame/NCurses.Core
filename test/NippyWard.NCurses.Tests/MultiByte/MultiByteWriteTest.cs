using System;
using System.Collections.Generic;
using System.Text;
using System.Buffers;

using Xunit;
using Xunit.Abstractions;

using NippyWard.NCurses.Tests.Model;
using NippyWard.NCurses.Interop;

namespace NippyWard.NCurses.Tests.MultiByte
{
    /* TODO:
     * Optimized ASCII path on Windows */
    [Collection("Default")]
    public class MultiByteWriteTest : WriteTest
    {
        protected override char TestChar => '\u263A';
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
                '\u049F',
                '\uF2DD', //4 byte char
                '\u0CD8'
                
            });

        private ReadOnlyMemory<byte> _utf8Memory;
        private ReadOnlySequence<byte> _utf8Sequence;

        public MultiByteWriteTest(ITestOutputHelper testOutputHelper, StdScrState stdScrState)
            :base(testOutputHelper, stdScrState)
        {
            TestSequenceSegment startSegment = null, endSegment = null;

            Encoder encoder = Encoding.UTF8.GetEncoder();

            int totalByteCount = encoder.GetByteCount(this.TestString, true);
            byte[] bytes = new byte[totalByteCount];

            char[] charSegment;
            byte[] bytesSegment;
            int byteCount, byteIndex = 0;

            for (int i = 0; i < this.TestString.Length - 1; i++)
            {
                if (i == this.TestString.Length - 2) //4 byte char
                {
                    charSegment = new char[2];
                    charSegment[0] = this.TestString[i];
                    charSegment[1] = this.TestString[i + 1];
                }
                else
                {
                    charSegment = new char[1];
                    charSegment[0] = this.TestString[i];
                }

                byteCount = encoder.GetByteCount(charSegment, true);
                bytesSegment = new byte[byteCount];

                encoder.GetBytes(charSegment, bytesSegment, true);

                if (startSegment is null)
                {
                    startSegment = new TestSequenceSegment(bytesSegment);
                    endSegment = startSegment;
                }
                else
                {
                    endSegment = endSegment.SetNext(new TestSequenceSegment(bytesSegment));
                }

                bytesSegment.CopyTo(bytes, byteIndex);

                byteIndex += byteCount;
            }

            this._utf8Memory = new Memory<byte>(bytes);
            this._utf8Sequence = new ReadOnlySequence<byte>(startSegment, 0, endSegment, endSegment.Memory.Length);
        }

        protected override IWindow GenerateWindow(IWindow window)
        {
            return window.ToMultiByteWindow();
        }

        [Fact]
        public void WriteUTF8SpanTest()
        {
            this.Window.Write(this._utf8Memory.Span, Encoding.UTF8);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            string resultString = this.Window.ExtractString(0, 0, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void WriteUTF8SpanWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this._utf8Memory.Span, Encoding.UTF8, 0, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(0, (int)resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }

        [Fact]
        public void WriteUTF8SpanWithAttributesTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this._utf8Memory.Span, Encoding.UTF8, Attrs.BOLD | Attrs.ITALIC, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }

        [Fact]
        public void WriteUTF8SequenceTest()
        {
            this.Window.Write(this._utf8Sequence, Encoding.UTF8);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            string resultString = this.Window.ExtractString(0, 0, this.TestString.Length, out int read);
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(this.TestString.Length, read);
        }

        [Fact]
        public void WriteUTF8SequenceWithColorTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this._utf8Sequence, Encoding.UTF8, 0, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(0, (int)resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }

        [Fact]
        public void WriteUTF8SequnceWithAttributesTest()
        {
            Assert.True(this.StdScrState.SupportsColor);

            this.Window.Write(this._utf8Sequence, Encoding.UTF8, Attrs.BOLD | Attrs.ITALIC, 4);

            Assert.Equal(0, this.Window.CursorLine);
            Assert.Equal(this.TestString.Length, this.Window.CursorColumn);

            this.Window.ExtractString(0, 0, out INCursesCharString resultNCursesString, this.TestString.Length);
            Assert.Equal(this.TestString.Length, resultNCursesString.Length);
            string resultString = resultNCursesString.ToString();
            Assert.Equal(this.TestString, resultString);
            Assert.Equal(Attrs.BOLD | Attrs.ITALIC, resultNCursesString[0].Attributes);
            Assert.Equal(4, resultNCursesString[0].ColorPair);
        }
    }
}
