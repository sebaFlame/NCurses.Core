using System;
using System.Collections;
using System.Collections.Generic;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop.SingleByte
{
    internal struct SingleByteCharString<TSingleByte> : ISingleByteNCursesCharString, IEquatable<SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
    {
        public Span<byte> ByteSpan => MemoryMarshal.AsBytes(this._buffer.Span);
        public Span<TSingleByte> CharSpan => this._buffer.Span;

        public ref TSingleByte GetPinnableReference() => ref this._buffer.Span.GetPinnableReference();
        public int Length => this._buffer.Length;

        public int CharLength => this._buffer.Length - 1; // actual character length
        int ICharString.Length => this._buffer.Length - 1; // actual character length

        public INCursesChar this[int index] => this.CharSpan[index];

        private Memory<TSingleByte> _buffer;

        public SingleByteCharString(Memory<TSingleByte> buffer)
        {
            this._buffer = buffer;
        }

        IEnumerator IEnumerable.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        IEnumerator<IChar> IEnumerable<IChar>.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        IEnumerator<ISingleByteChar> IEnumerable<ISingleByteChar>.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        IEnumerator<ISingleByteNCursesChar> IEnumerable<ISingleByteNCursesChar>.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        IEnumerator<INCursesChar> IEnumerable<INCursesChar>.GetEnumerator() => new SingleByteCharStringEnumerator(this);

        //TODO: crashes Visual Studio debugger when using netcoreapp2.0
        public static explicit operator string(SingleByteCharString<TSingleByte> str)
        {
            return str.ToString();
        }

        public static bool operator ==(in SingleByteCharString<TSingleByte> chStrLeft, in SingleByteCharString<TSingleByte> chStrRight)
        {
            return chStrLeft.Equals(chStrRight);
        }

        public static bool operator !=(in SingleByteCharString<TSingleByte> chStrLeft, in SingleByteCharString<TSingleByte> chStrRight)
        {
            return chStrLeft.Equals(chStrRight);
        }

        public override bool Equals(object obj)
        {
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(ISingleByteCharString obj)
        {
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(INCursesCharString obj)
        {
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(ISingleByteNCursesCharString obj)
        {
            if (obj is SingleByteCharString<TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(SingleByteCharString<TSingleByte> other)
        {
            //remove null terminator
            Span<byte> thisSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TSingleByte>());
            Span<byte> otherSpan = other.ByteSpan.Slice(0, other.Length * Marshal.SizeOf<TSingleByte>());

            return thisSpan.SequenceEqual(otherSpan);
        }

        public override string ToString()
            => SingleByteCharFactory<TSingleByte>.GenerateString(this._buffer);

        public override int GetHashCode()
        {
            return this.CharSpan.GetHashCode();
        }

        private class SingleByteCharStringEnumerator : IEnumerator<ISingleByteNCursesChar>, IEnumerator<ISingleByteChar>, IEnumerator<IChar>
        {
            ISingleByteNCursesChar IEnumerator<ISingleByteNCursesChar>.Current => this.SingleByteCharString.CharSpan[this.Position];

            ISingleByteChar IEnumerator<ISingleByteChar>.Current => this.SingleByteCharString.CharSpan[this.Position];

            IChar IEnumerator<IChar>.Current => this.SingleByteCharString.CharSpan[this.Position];

            object IEnumerator.Current => this.SingleByteCharString.CharSpan[this.Position];

            private SingleByteCharString<TSingleByte> SingleByteCharString;
            private int Position;

            public SingleByteCharStringEnumerator(SingleByteCharString<TSingleByte> singleByteCharString)
            {
                this.SingleByteCharString = singleByteCharString;
                this.Position = -1;
            }

            public bool MoveNext() => ++this.Position < this.SingleByteCharString.Length;

            public void Reset()
            {
                this.Position = -1;
            }

            public void Dispose()
            {
                //NOP
            }
        }
    }
}
