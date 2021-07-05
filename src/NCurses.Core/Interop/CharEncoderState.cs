using System;
using System.Collections.Generic;
using System.Text;
using System.Buffers;

namespace NCurses.Core.Interop
{
    internal struct CharEncoderState : IDisposable
    {
        //intermediate buffer in platform encoding
        internal byte[] IntermediateBuffer;
        //the lenght in bytes (!)
        internal int IntermadiateBufferLength;

        /* on linux platform: number of code points
         * on windows: number of chars */
        internal int BufferLength;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intermediateBufferLength">Length in bytes (!)</param>
        internal CharEncoderState(int intermediateBufferLength)
        {
            this.IntermediateBuffer = ArrayPool<byte>.Shared.Rent(intermediateBufferLength);

            this.BufferLength = 0;
            this.IntermadiateBufferLength = 0;
        }

        public void Dispose()
        {
            if (this.IntermediateBuffer is null)
            {
                return;
            }

            ArrayPool<byte>.Shared.Return(this.IntermediateBuffer);
        }

        public static explicit operator CharEncoderState(int bufferLength)
        {
            CharEncoderState encoderState = default;
            encoderState.BufferLength = bufferLength;
            return encoderState;
        }
    }
}
