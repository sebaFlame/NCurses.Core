using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;
using System.Buffers;
using System.Runtime.CompilerServices;

using NCurses.Core.Interop.Platform;

namespace NCurses.Core.Interop.WideChar
{
    internal class WideCharFactory<TWideChar> : IMultiByteCharFactory<TWideChar, WideCharString<TWideChar>>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
    {
        internal static WideCharFactory<TWideChar> _Instance;
        internal static CreateBuffer<TWideChar> _CreatePooledBuffer;
        internal static CreateBuffer<TWideChar> _CreateArrayBuffer;

        static WideCharFactory()
        {
            _Instance = new WideCharFactory<TWideChar>();

            _CreatePooledBuffer = NativeNCurses.CreatePoolBuffer<TWideChar>;
            _CreateArrayBuffer = NativeNCurses.CreateArrayBuffer<TWideChar>;
        }

        internal static string GenerateString(Memory<TWideChar> buffer)
            => PlatformWideCharEncoder<TWideChar>._Instance.GenerateString
            (
                buffer
            );

        public TWideChar GetNativeChar(int ch)
            => PlatformWideCharEncoder<TWideChar>._Instance.Encode(ch);

        public TWideChar GetNativeChar(byte ch)
            => PlatformWideCharEncoder<TWideChar>._Instance.Encode((char)ch);

        public byte GetByte(TWideChar @char)
        {
            byte res;

            unsafe
            {
                byte* b = (byte*)(&@char);
                res = *b;
            }

            return res;
        }

        public int GetChar(TWideChar @char)
        {
            if (@char.Equals(default))
            {
                return 0;
            }

            using (BufferState<TWideChar> wchBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer((CharEncoderState)1))
            {
                Memory<TWideChar> wchBuffer = wchBufferState.Memory;
                wchBuffer.Span[0] = @char;

                return (WideCharFactory<TWideChar>.GenerateString(wchBuffer))[0];
            }
        }

        public BufferState<TWideChar> GetNativeEmptyString(CreateBuffer<TWideChar> createBuffer, int length, out WideCharString<TWideChar> @string)
        {
            BufferState<TWideChar> bufferState = createBuffer((CharEncoderState)length);
            bufferState.Memory.Span.Clear();
            @string = new WideCharString<TWideChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TWideChar> GetNativeString(CreateBuffer<TWideChar> createBuffer, ReadOnlySpan<char> str, out WideCharString<TWideChar> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str);
            BufferState<TWideChar> bufferState = createBuffer(encoderState);
            PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, bufferState);
            @string = new WideCharString<TWideChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TWideChar> GetNativeString(CreateBuffer<TWideChar> createBuffer, ReadOnlySpan<byte> str, out WideCharString<TWideChar> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, Encoding.ASCII);
            BufferState<TWideChar> bufferState = createBuffer(encoderState);
            PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, Encoding.ASCII, bufferState);
            @string = new WideCharString<TWideChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TWideChar> GetNativeString(CreateBuffer<TWideChar> createBuffer, in ReadOnlySequence<byte> str, out WideCharString<TWideChar> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, Encoding.ASCII);
            BufferState<TWideChar> bufferState = createBuffer(encoderState);
            PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, Encoding.ASCII, bufferState);
            @string = new WideCharString<TWideChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TWideChar> GetNativeString(CreateBuffer<TWideChar> createBuffer, ReadOnlySpan<byte> str, Encoding encoding, out WideCharString<TWideChar> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, encoding);
            BufferState<TWideChar> bufferState = createBuffer(encoderState);
            PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, encoding, bufferState);
            @string = new WideCharString<TWideChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TWideChar> GetNativeString(CreateBuffer<TWideChar> createBuffer, in ReadOnlySequence<byte> str, Encoding encoding, out WideCharString<TWideChar> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, encoding);
            BufferState<TWideChar> bufferState = createBuffer(encoderState);
            PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, encoding, bufferState);
            @string = new WideCharString<TWideChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TWideChar> GetNativeString(CreateBuffer<TWideChar> createBuffer, ref TWideChar strRef, out WideCharString<TWideChar> @string)
        {
            int length = NativeNCurses.FindStringLength(ref strRef);
            BufferState<TWideChar> bufferState = createBuffer((CharEncoderState)length);

            ref TWideChar bufferRef = ref bufferState.Memory.Span.GetPinnableReference();

            Unsafe.CopyBlock
            (
                ref Unsafe.As<TWideChar, byte>(ref bufferRef),
                ref Unsafe.As<TWideChar, byte>(ref strRef),
                (uint)(bufferState.EncoderState.BufferLength * Marshal.SizeOf<TWideChar>())
            );

            @string = new WideCharString<TWideChar>(bufferState.Memory);

            return bufferState;
        }
    }
}
