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

namespace NCurses.Core.Interop.Char
{
    internal struct CharString<TChar> : ISingleByteCharString, IEquatable<CharString<TChar>>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
    {
        public Span<byte> ByteSpan => MemoryMarshal.AsBytes(this._buffer.Span);
        public Span<TChar> CharSpan => this._buffer.Span;

        public ref TChar GetPinnableReference() => ref this._buffer.Span.GetPinnableReference();
        public int Length => this._buffer.Length;

        public int CharLength => this._buffer.Length - 1; // actual character length
        int ICharString.Length => this._buffer.Length - 1; // actual character length

        private Memory<TChar> _buffer;

        public CharString(Memory<TChar> buffer)
        {
            this._buffer = buffer;
        }

        IEnumerator IEnumerable.GetEnumerator() => new CharStringEnumerator(this);

        IEnumerator<IChar> IEnumerable<IChar>.GetEnumerator() => new CharStringEnumerator(this);

        IEnumerator<ISingleByteChar> IEnumerable<ISingleByteChar>.GetEnumerator() => new CharStringEnumerator(this);

        public static explicit operator string(CharString<TChar> wStr) => wStr.ToString();

        public override bool Equals(object obj)
        {
            if (obj is CharString<TChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if(obj is CharString<TChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ISingleByteCharString obj)
        {
            if (obj is CharString<TChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(CharString<TChar> other)
        {
            //remove null terminator
            Span<byte> thisSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TChar>());
            Span<byte> otherSpan = other.ByteSpan.Slice(0, other.Length * Marshal.SizeOf<TChar>());

            return thisSpan.SequenceEqual(otherSpan);
        }

        public override string ToString()
            => CharFactory<TChar>.GenerateString(this._buffer);

        public override int GetHashCode()
        {
            return this.CharSpan.GetHashCode();
        }

        private class CharStringEnumerator : IEnumerator<ISingleByteChar>, IEnumerator<IChar>
        {
            ISingleByteChar IEnumerator<ISingleByteChar>.Current => this.CharString.CharSpan[this.Position];

            IChar IEnumerator<IChar>.Current => this.CharString.CharSpan[this.Position];

            object IEnumerator.Current => this.CharString.CharSpan[this.Position];

            private CharString<TChar> CharString;
            private int Position;

            public CharStringEnumerator(CharString<TChar> charString)
            {
                this.CharString = charString;
                this.Reset();
            }

            public bool MoveNext() => ++this.Position < this.CharString.Length;

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
