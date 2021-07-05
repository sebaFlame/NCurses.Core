using System;
using System.Collections.Generic;
using System.Text;
using System.Buffers;

using NCurses.Core.Interop.Platform;

namespace NCurses.Core.Interop
{
    internal struct BufferState<TChar> : IDisposable
        where TChar : unmanaged, IChar
    {
        internal Memory<TChar> Memory;
        internal CharEncoderState EncoderState;

        private TChar[] _pooledBuffer;

        internal BufferState(CharEncoderState encoderState, TChar[] buffer)
        {
            this.Memory = new Memory<TChar>(buffer);
            this.EncoderState = encoderState;

            this._pooledBuffer = null;
        }

        internal BufferState(CharEncoderState encoderState, bool addNullTerminator = true)
        {
            this._pooledBuffer = ArrayPool<TChar>.Shared.Rent(encoderState.BufferLength + (addNullTerminator ? 1 : 0));
            this.Memory = new Memory<TChar>(this._pooledBuffer, 0, encoderState.BufferLength + (addNullTerminator ? 1 : 0));
            this.EncoderState = encoderState;
        }

        public void Dispose()
        {
            if (!(this._pooledBuffer is null))
            {
                ArrayPool<TChar>.Shared.Return(this._pooledBuffer);
            }

            this.EncoderState.Dispose();
        }
    }
}
