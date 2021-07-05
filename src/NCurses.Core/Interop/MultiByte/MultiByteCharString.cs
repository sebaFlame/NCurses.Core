using System;
using System.Collections.Generic;
using System.Linq;
using System.Buffers;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
{
    internal struct MultiByteCharString<TMultiByte, TWideChar, TSingleByte> : IMultiByteNCursesCharString, IEquatable<MultiByteCharString<TMultiByte, TWideChar, TSingleByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
    {
        public Span<byte> ByteSpan => MemoryMarshal.AsBytes(this._buffer.Span);
        public Span<TMultiByte> CharSpan => this._buffer.Span;

        public ref TMultiByte GetPinnableReference() => ref this._buffer.Span.GetPinnableReference();
        public int Length => this._buffer.Length;

        public int CharLength => this._buffer.Length - 1; // actual character length
        int ICharString.Length => this._buffer.Length - 1; // actual character length

        public INCursesChar this[int index] => this.CharSpan[index];

        private Memory<TMultiByte> _buffer;

        public MultiByteCharString(Memory<TMultiByte> buffer)
        {
            this._buffer = buffer;
        }

        IEnumerator IEnumerable.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        IEnumerator<IChar> IEnumerable<IChar>.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        IEnumerator<IMultiByteChar> IEnumerable<IMultiByteChar>.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        IEnumerator<IMultiByteNCursesChar> IEnumerable<IMultiByteNCursesChar>.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        IEnumerator<INCursesChar> IEnumerable<INCursesChar>.GetEnumerator() => new MultiByteCharStringEnumerator(this);

        public static explicit operator string(MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wStr)
        {
            return wStr.ToString();
        }

        public static bool operator ==(in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStrLeft, in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public static bool operator !=(in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStrLeft, in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStrRight)
        {
            return wchStrLeft.Equals(wchStrRight);
        }

        public override bool Equals(object obj)
        {
            if (obj is MultiByteCharString<TMultiByte, TWideChar, TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if (obj is MultiByteCharString<TMultiByte, TWideChar, TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(IMultiByteCharString obj)
        {
            if (obj is MultiByteCharString<TMultiByte, TWideChar, TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(INCursesCharString obj)
        {
            if (obj is MultiByteCharString<TMultiByte, TWideChar, TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(IMultiByteNCursesCharString obj)
        {
            if (obj is MultiByteCharString<TMultiByte, TWideChar, TSingleByte> charString)
            {
                return this.Equals(charString);
            }
            return false;
        }

        public bool Equals(MultiByteCharString<TMultiByte, TWideChar, TSingleByte> other)
        {
            //remove null terminator
            Span<byte> thisSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TMultiByte>());
            Span<byte> otherSpan = other.ByteSpan.Slice(0, other.Length * Marshal.SizeOf<TMultiByte>());

            return thisSpan.SequenceEqual(otherSpan);
        }

        public override string ToString()
            => MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>.GenerateString(this._buffer);

        public override int GetHashCode()
        {
            return this.CharSpan.GetHashCode();
        }

        private class MultiByteCharStringEnumerator : IEnumerator<IMultiByteNCursesChar>, IEnumerator<IMultiByteChar>, IEnumerator<IChar>
        {
            IMultiByteNCursesChar IEnumerator<IMultiByteNCursesChar>.Current => this.MultiByteCharString.CharSpan[this.Position];

            IMultiByteChar IEnumerator<IMultiByteChar>.Current => this.MultiByteCharString.CharSpan[this.Position];

            IChar IEnumerator<IChar>.Current => this.MultiByteCharString.CharSpan[this.Position];

            object IEnumerator.Current => this.MultiByteCharString.CharSpan[this.Position];

            private MultiByteCharString<TMultiByte, TWideChar, TSingleByte> MultiByteCharString;
            private int Position;

            public MultiByteCharStringEnumerator(MultiByteCharString<TMultiByte, TWideChar, TSingleByte> multiByteCharString)
            {
                this.MultiByteCharString = multiByteCharString;
                this.Reset();
            }

            public bool MoveNext() => ++this.Position < this.MultiByteCharString.Length;

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
