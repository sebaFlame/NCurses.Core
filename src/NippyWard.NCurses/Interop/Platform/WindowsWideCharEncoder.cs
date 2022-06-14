using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NippyWard.NCurses.Interop.Platform
{
    internal class WindowsWideCharEncoder<TWideChar> : PlatformWideCharEncoder<TWideChar>
        where TWideChar : unmanaged, IMultiByteChar
    {
        internal override Encoding PlatformEncoding => UTF16;

        internal WindowsWideCharEncoder()
            : base()
        { }

        internal override CharEncoderState GetBufferSize(ReadOnlySpan<byte> @string, Encoding encoding)
        {
            CharEncoderState encoderState;

            if (encoding.CodePage == this.PlatformEncoding.CodePage)
            {
                encoderState = new CharEncoderState();
                ReadOnlySpan<char> chSpan = MemoryMarshal.Cast<byte, char>(@string);
                encoderState.BufferLength = chSpan.Length;
            }
            else
            {
                //create a buffer atleast @string.Length long
                encoderState = new CharEncoderState((int)@string.Length);

                //create a decoder
                Decoder decoder = GetDecoder(encoding);
                decoder.Reset();

                int charsUsed = 0, bytesUsed = 0;
                bool completed = false;

                unsafe
                {
                    fixed (byte* buffer = @string)
                    {
                        fixed (byte* b = encoderState.IntermediateBuffer)
                        {
                            char* chars = (char*)b;
                            decoder.Convert
                            (
                                buffer,
                                @string.Length,
                                chars,
                                encoderState.IntermediateBuffer.Length / sizeof(char),
                                false,
                                out bytesUsed,
                                out charsUsed,
                                out completed
                            );
                        }
                    }
                }

                if (!completed)
                {
                    throw new InvalidOperationException("Encoding conversion did not succeed");
                }

                encoderState.BufferLength = charsUsed;
                encoderState.IntermadiateBufferLength = (charsUsed * sizeof(char));
            }

            return encoderState;
        }

        internal override CharEncoderState GetBufferSize(in ReadOnlySequence<byte> @string, Encoding encoding)
        {
            CharEncoderState encoderState;

            if (encoding.CodePage == this.PlatformEncoding.CodePage)
            {
                encoderState = new CharEncoderState();
                ReadOnlySpan<char> chSpan;

                foreach (ReadOnlyMemory<byte> memory in @string)
                {
                    if (memory.IsEmpty)
                    {
                        continue;
                    }

                    chSpan = MemoryMarshal.Cast<byte, char>(memory.Span);

                    encoderState.BufferLength += chSpan.Length;
                }
            }
            else
            {
                //create a buffer atleast @string.Length long
                encoderState = new CharEncoderState((int)@string.Length);

                //create a decoder
                Decoder decoder = GetDecoder(encoding);
                decoder.Reset();

                int charsUsed = 0, bytesUsed = 0;
                bool completed = false;
                int charsIndex = 0;

                foreach (ReadOnlyMemory<byte> memory in @string)
                {
                    if (memory.IsEmpty)
                    {
                        continue;
                    }

                    unsafe
                    {
                        fixed (byte* buffer = memory.Span)
                        {
                            fixed (byte* b = encoderState.IntermediateBuffer)
                            {
                                char* chars = (char*)b;
                                decoder.Convert
                                (
                                    buffer,
                                    memory.Length,
                                    chars + charsIndex,
                                    encoderState.IntermediateBuffer.Length / sizeof(char),
                                    false,
                                    out bytesUsed,
                                    out charsUsed,
                                    out completed
                                );
                            }
                        }
                    }

                    charsIndex += charsUsed;
                }

                if (!completed)
                {
                    throw new InvalidOperationException("Encoding conversion did not succeed");
                }

                encoderState.BufferLength = charsIndex;
                encoderState.IntermadiateBufferLength = charsIndex * sizeof(char);
            }

            return encoderState;
        }

        internal override void Encode(
            in ReadOnlySequence<byte> @string, 
            Encoding encoding, 
            in BufferState<TWideChar> bufferState)
        {
            if (@string.IsSingleSegment)
            {
                this.Encode(@string.First.Span, encoding, bufferState);
                return;
            }

            Span<TWideChar> buffer = bufferState.Memory.Span;

            if (encoding.CodePage != this.PlatformEncoding.CodePage)
            {
                if (bufferState.EncoderState.IntermediateBuffer is null)
                {
                    throw new NullReferenceException("Intermediate buffer can not be null");
                }

                ReadOnlySpan<byte> intermediateSpan = new ReadOnlySpan<byte>(bufferState.EncoderState.IntermediateBuffer, 0, bufferState.EncoderState.IntermadiateBufferLength);
                ReadOnlySpan<TWideChar> charBuffer = MemoryMarshal.Cast<byte, TWideChar>(intermediateSpan);
                charBuffer.CopyTo(buffer);
            }
            else
            {
                Span<TWideChar> charBufferSpan = buffer;
                ReadOnlySpan<TWideChar> charSpan;

                foreach (ReadOnlyMemory<byte> memory in @string)
                {
                    if (memory.IsEmpty)
                    {
                        continue;
                    }

                    charSpan = MemoryMarshal.Cast<byte, TWideChar>(memory.Span);
                    charSpan.CopyTo(charBufferSpan);
                    charBufferSpan = charBufferSpan.Slice(charSpan.Length);
                }
            }
        }

        internal override void Encode(
            ReadOnlySpan<byte> @string,
            Encoding encoding,
            in BufferState<TWideChar> bufferState)
        {
            ReadOnlySpan<TWideChar> charBuffer;
            Span<TWideChar> buffer = bufferState.Memory.Span;

            if (encoding.CodePage != this.PlatformEncoding.CodePage)
            {
                if (bufferState.EncoderState.IntermediateBuffer is null)
                {
                    throw new NullReferenceException("Inermediate buffer can not be null");
                }

                ReadOnlySpan<byte> intermediateSpan = new ReadOnlySpan<byte>(bufferState.EncoderState.IntermediateBuffer, 0, bufferState.EncoderState.IntermadiateBufferLength);
                charBuffer = MemoryMarshal.Cast<byte, TWideChar>(intermediateSpan);
            }
            else
            {
                charBuffer = MemoryMarshal.Cast<byte, TWideChar>(@string);
            }

            charBuffer.CopyTo(buffer);
        }

        internal override string GenerateString(ReadOnlyMemory<TWideChar> buffer)
        {
            ReadOnlySpan<char> charBuffer = MemoryMarshal.Cast<TWideChar, char>(buffer.Slice(0, buffer.Length - 1).Span); //remove null terminator
            return charBuffer.ToString();
        }

        internal override TWideChar Encode(int @char)
        {
            TWideChar wch = default;

            unsafe
            {
                char ch = (char)@char;
                wch = *((TWideChar*)&ch);
            }

            return wch;
        }
    }
}
