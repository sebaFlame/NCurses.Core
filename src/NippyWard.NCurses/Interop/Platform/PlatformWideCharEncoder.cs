using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NippyWard.NCurses.Interop.Platform
{
    internal abstract class PlatformWideCharEncoder<TWideChar>
        where TWideChar : unmanaged, IMultiByteChar
    {
        protected static Encoding UTF16 { get; }
        protected static Encoding UTF8 { get; }
        protected static Encoding ASCII { get; }

        internal abstract Encoding PlatformEncoding { get; }
        internal static PlatformWideCharEncoder<TWideChar> _Instance { get; set; }

        //private const int _BufferSize = 32;
        private static Dictionary<Encoding, ThreadLocal<Decoder>> _Decoders;
        private static Dictionary<Encoding, ThreadLocal<Encoder>> _Encoders;

        static PlatformWideCharEncoder()
        {
            UTF16 = Encoding.Unicode;
            UTF8 = Encoding.UTF8;
            ASCII = Encoding.ASCII;

            _Decoders = new Dictionary<Encoding, ThreadLocal<Decoder>>();
            _Encoders = new Dictionary<Encoding, ThreadLocal<Encoder>>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _Instance = new WindowsWideCharEncoder<TWideChar>();
            }
            else
            {
                _Instance = new LinuxWideCharEncoder<TWideChar>();
            }
        }

        internal static Decoder GetDecoder(Encoding encoding)
        {
            Decoder decoder = null;
            if (!_Decoders.TryGetValue(encoding, out ThreadLocal<Decoder> threadDecoder))
            {
                _Decoders.Add(encoding, threadDecoder = new ThreadLocal<Decoder>());
            }

            if (threadDecoder.IsValueCreated)
            {
                decoder = threadDecoder.Value;
            }
            else
            {
                threadDecoder.Value = decoder = encoding.GetDecoder();
            }

            return decoder;
        }

        internal static Encoder GetEncoder(Encoding encoding)
        {
            Encoder encoder = null;
            if (!_Encoders.TryGetValue(encoding, out ThreadLocal<Encoder>  threadEncoder))
            {
                _Encoders.Add(encoding, threadEncoder = new ThreadLocal<Encoder>());
            }

            if (threadEncoder.IsValueCreated)
            {
                encoder = threadEncoder.Value;
            }
            else
            {
                threadEncoder.Value = encoder = encoding.GetEncoder();
            }

            return encoder;
        }

        /// <summary>
        /// Get the correct buffer size for <typeparamref name="TWideChar"/> of string <paramref name="string"/>
        /// </summary>
        /// <param name="string">The string to get the buffer size for</param>
        /// <returns>An <see cref="CharEncoderState"/> object containing all info for correct encoding</returns>
        internal CharEncoderState GetBufferSize(ReadOnlySpan<char> @string)
            => this.GetBufferSize(MemoryMarshal.AsBytes(@string), UTF16);

        /// <summary>
        /// Get the correct buffer size for <typeparamref name="TWideChar"/> of string <paramref name="string"/>
        /// </summary>
        /// <param name="string">The string to get the buffer size for</param>
        /// <param name="encoding">The encoding of the string</param>
        /// <returns>An <see cref="CharEncoderState"/> object containing all info for correct encoding</returns>
        internal CharEncoderState GetBufferSize(
            ReadOnlyMemory<byte> @string,
            Encoding encoding)
            => this.GetBufferSize(@string.Span, encoding);

        /// <summary>
        /// Get the correct buffer size for <typeparamref name="TWideChar"/> of string <paramref name="string"/>
        /// </summary>
        /// <param name="string">The string to get the buffer size for</param>
        /// <param name="encoding">The encoding of the string</param>
        /// <returns>An <see cref="CharEncoderState"/> object containing all info for correct encoding</returns>
        internal abstract CharEncoderState GetBufferSize(
            ReadOnlySpan<byte> @string,
            Encoding encoding);

        /// <summary>
        /// Get the correct buffer size for <typeparamref name="TWideChar"/> of string <paramref name="string"/>
        /// </summary>
        /// <param name="string">The string to get the buffer size for</param>
        /// <param name="encoding">The encoding of the string</param>
        /// <returns>An <see cref="CharEncoderState"/> object containing all info for correct encoding</returns>
        internal abstract CharEncoderState GetBufferSize(
            in ReadOnlySequence<byte> @string, 
            Encoding encoding);

        /// <summary>
        /// Encode char <paramref name="char"/> into <see cref="PlatformEncoding"/>
        /// </summary>
        /// <param name="char">The char</param>
        internal abstract TWideChar Encode(int @char);

        /// <summary>
        /// Encode string <paramref name="string"/> from <paramref name="encoding"/> into <see cref="PlatformEncoding"/>
        /// </summary>
        /// <param name="string">The string to encode</param>
        /// <param name="encoding">The encoding of <paramref name="string"/></param>
        /// <param name="encoderState">The <see cref="CharEncoderState"/> object containing encoding info</param>
        /// <param name="buffer">A buffer large enough to contain the <see cref="PlatformEncoding"/> encoded <paramref name="string"/></param>
        internal void Encode(
            ReadOnlyMemory<byte> @string,
            Encoding encoding,
            in BufferState<TWideChar> bufferState)
        {
            this.Encode(@string.Span, encoding, bufferState);
        }

        /// <summary>
        /// Encode string <paramref name="string"/> from <paramref name="encoding"/> into <see cref="PlatformEncoding"/>
        /// </summary>
        /// <param name="string">The string to encode</param>
        /// <param name="encoding">The encoding of <paramref name="string"/></param>
        /// <param name="encoderState">The <see cref="CharEncoderState"/> object containing encoding info</param>
        /// <param name="buffer">A buffer large enough to contain the <see cref="PlatformEncoding"/> encoded <paramref name="string"/></param>
        internal void Encode(
            ReadOnlySpan<char> @string,
            in BufferState<TWideChar> bufferState)
            => this.Encode(MemoryMarshal.AsBytes(@string), UTF16, bufferState);

        /// <summary>
        /// Encode string <paramref name="string"/> from <paramref name="encoding"/> into <see cref="PlatformEncoding"/>
        /// </summary>
        /// <param name="string">The string to encode</param>
        /// <param name="encoding">The encoding of <paramref name="string"/></param>
        /// <param name="encoderState">The <see cref="CharEncoderState"/> object containing encoding info</param>
        /// <param name="buffer">A buffer large enough to contain the <see cref="PlatformEncoding"/> encoded <paramref name="string"/></param>
        internal abstract void Encode(
            ReadOnlySpan<byte> @string,
            Encoding encoding,
            in BufferState<TWideChar> bufferState);

        /// <summary>
        /// Encode string <paramref name="string"/> from <paramref name="encoding"/> into <see cref="PlatformEncoding"/>
        /// </summary>
        /// <param name="string">The string to encode</param>
        /// <param name="encoding">The encoding of <paramref name="string"/></param>
        /// <param name="encoderState">The <see cref="CharEncoderState"/> object containing encoding info</param>
        /// <param name="buffer">A buffer large enough to contain the <see cref="PlatformEncoding"/> encoded <paramref name="string"/></param>
        internal abstract void Encode(
            in ReadOnlySequence<byte> @string,
            Encoding encoding,
            in BufferState<TWideChar> bufferState);

        /// <summary>
        /// Generate a UTF-16 encoded string from a <see cref="PlatformEncoding"/> encoded buffer
        /// </summary>
        /// <param name="buffer">The buffer to get the string from</param>
        /// <returns>An UTF-16 encoded string</returns>
        internal abstract string GenerateString(ReadOnlyMemory<TWideChar> buffer);
    }
}
