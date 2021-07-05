using System;
using System.Collections.Generic;
using System.Text;
using System.Buffers;

namespace NCurses.Core.Tests.Model
{
    internal class TestSequenceSegment : ReadOnlySequenceSegment<byte>
    {
        public TestSequenceSegment(params byte[] b)
        {
            this.Memory = new ReadOnlyMemory<byte>(b);
        }

        public TestSequenceSegment SetNext(TestSequenceSegment testSegment)
        {
            this.Next = testSegment;

            testSegment.RunningIndex = this.RunningIndex + this.Memory.Length;

            return testSegment;
        }
    }
}
