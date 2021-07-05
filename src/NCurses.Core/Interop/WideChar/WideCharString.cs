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
using NCurses.Core.Interop.Platform;

namespace NCurses.Core.Interop.WideChar
{
    internal struct WideCharString<TWideChar> : IMultiByteCharString, IEquatable<WideCharString<TWideChar>>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
    {
        public Span<byte> ByteSpan => MemoryMarshal.AsBytes(this._buffer.Span);
        public Span<TWideChar> CharSpan => this._buffer.Span;

        public ref TWideChar GetPinnableReference() => ref this._buffer.Span.GetPinnableReference();
        public int Length => this._buffer.Length;

        public int CharLength => this._buffer.Length - 1; // actual character length
        int ICharString.Length => this._buffer.Length - 1; // actual character length

        private Memory<TWideChar> _buffer;

        public WideCharString(Memory<TWideChar> buffer)
        {
            this._buffer = buffer;
        }

        IEnumerator IEnumerable.GetEnumerator() => new WideCharStringEnumerator(this);

        IEnumerator<IChar> IEnumerable<IChar>.GetEnumerator() => new WideCharStringEnumerator(this);

        IEnumerator<IMultiByteChar> IEnumerable<IMultiByteChar>.GetEnumerator() => new WideCharStringEnumerator(this);

        public static explicit operator string(WideCharString<TWideChar> wStr) => wStr.ToString();

        public override bool Equals(object obj)
        {
            if (obj is WideCharString<TWideChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(ICharString obj)
        {
            if (obj is WideCharString<TWideChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IMultiByteCharString obj)
        {
            if (obj is WideCharString<TWideChar> other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(WideCharString<TWideChar> other)
        {
            //remove null terminator
            Span<byte> thisSpan = this.ByteSpan.Slice(0, this.Length * Marshal.SizeOf<TWideChar>());
            Span<byte> otherSpan = other.ByteSpan.Slice(0, other.Length * Marshal.SizeOf<TWideChar>());

            return thisSpan.SequenceEqual(otherSpan);
        }

        public override string ToString()
            => WideCharFactory<TWideChar>.GenerateString(this._buffer);

        public override int GetHashCode()
        {
            return this._buffer.GetHashCode();
        }

        private class WideCharStringEnumerator : IEnumerator<IMultiByteChar>, IEnumerator<IChar>
        {
            IMultiByteChar IEnumerator<IMultiByteChar>.Current => this.WideCharString.CharSpan[this.Position];

            IChar IEnumerator<IChar>.Current => this.WideCharString.CharSpan[this.Position];

            object IEnumerator.Current => this.WideCharString.CharSpan[this.Position];

            private WideCharString<TWideChar> WideCharString;
            private int Position;

            public WideCharStringEnumerator(WideCharString<TWideChar> wideCharString)
            {
                this.WideCharString = wideCharString;
                this.Reset();
            }

            public bool MoveNext() => ++this.Position < this.WideCharString.Length;

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
