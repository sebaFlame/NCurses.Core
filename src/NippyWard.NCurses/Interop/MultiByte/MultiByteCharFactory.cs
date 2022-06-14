using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Buffers;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Platform;

namespace NippyWard.NCurses.Interop.MultiByte
{
    internal delegate TMultiByte CreateMultiByteCharWithAttributeAndColorPairFromSpan<TMultiByte>(Span<byte> byteSpan, ulong attrs, ushort colorPair)
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>;

    internal class MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte> :
        IMultiByteNCursesCharFactory<TMultiByte, MultiByteCharString<TMultiByte, TWideChar, TSingleByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
    {
        internal static MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte> _Instance;
        internal static CreateBuffer<TMultiByte> _CreatePooledBuffer;
        internal static CreateBuffer<TMultiByte> _CreateArrayBuffer;

        private static CreateMultiByteCharWithAttributeAndColorPairFromSpan<TMultiByte> _CreateCharWithAttributeAndColorPairFromSpan;
        private static int _WideCharOffset;
        private static int _ExtColorOffset;

        static MultiByteCharFactory()
        {
            _Instance = new MultiByteCharFactory<TMultiByte, TWideChar, TSingleByte>();

            _CreatePooledBuffer = NativeNCurses.CreatePoolBuffer<TMultiByte>;
            _CreateArrayBuffer = NativeNCurses.CreateArrayBuffer<TMultiByte>;

            Type mbType = typeof(TMultiByte);

            _WideCharOffset = Marshal.SizeOf<TSingleByte>();
            _ExtColorOffset = Marshal.SizeOf<TSingleByte>() + (Marshal.SizeOf<TWideChar>() * Constants.CCHARW_MAX);

            /* padding should always be after the chars field */
            _ExtColorOffset = (_ExtColorOffset + mbType.StructLayoutAttribute.Pack / 2) / mbType.StructLayoutAttribute.Pack * mbType.StructLayoutAttribute.Pack;

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(Span<byte>));
            par2 = Expression.Parameter(typeof(ulong));
            par3 = Expression.Parameter(typeof(ushort));

            ctor = typeof(TMultiByte).GetConstructor(new Type[] { typeof(Span<byte>), typeof(ulong), typeof(ushort) });
            _CreateCharWithAttributeAndColorPairFromSpan =
                Expression.Lambda<CreateMultiByteCharWithAttributeAndColorPairFromSpan<TMultiByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();
        }

        internal static TMultiByte CreateMultiByteChar(int @char, ulong attrs, ushort colorPair)
        {
            int wideCharSize = Unsafe.SizeOf<TWideChar>();
            TWideChar wCh = PlatformWideCharEncoder<TWideChar>._Instance.Encode(@char);
            Span<byte> wChBuffer;

            unsafe
            {
                wChBuffer = new Span<byte>(&wCh, wideCharSize);
            }

            return _CreateCharWithAttributeAndColorPairFromSpan(wChBuffer, attrs, colorPair);
        }

        internal static void CreateMultiByteStringFromWideCharString(
            Memory<TMultiByte> buffer,
            Memory<TWideChar> wchBuffer, 
            ulong attrs, 
            ushort colorPair)
        {
            if (colorPair > 0)
            {
                attrs |= (uint)NativeNCurses.COLOR_PAIR(colorPair);
            }

            uint multiByteSize = (uint)Unsafe.SizeOf<TMultiByte>();
            uint singleByteSize = (uint)Unsafe.SizeOf<TSingleByte>();
            uint ulongByteSize = (uint)Unsafe.SizeOf<ulong>();

            unsafe
            {
                byte* bOffset, b;
                byte* a = ((byte*)&attrs);

                fixed (TMultiByte* mb = buffer.Span)
                {
                    fixed (TWideChar* wch = wchBuffer.Span)
                    {
                        for (int i = 0; i < wchBuffer.Length - 1; i++) //-1 -> ensure null termintor stays null
                        {
                            b = (byte*)(mb + i);

                            if (attrs > 0)
                            {
                                bOffset = b;

                                Unsafe.CopyBlock(bOffset, a, singleByteSize);
                            }

                            bOffset = b + _WideCharOffset;
                            Unsafe.Write(bOffset, *(wch + i));

                            if (colorPair > 0)
                            {
                                bOffset = b + _ExtColorOffset;
                                Unsafe.Write(bOffset, colorPair);
                            }
                        }
                    }
                }
            }
        }

        internal static string GenerateString(Memory<TMultiByte> buffer)
        {
            uint wideCharSize = (uint)Unsafe.SizeOf<TWideChar>();

            using (BufferState<TWideChar> wchBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer((CharEncoderState)buffer.Length, false))
            {
                Memory<TWideChar> wchBuffer = wchBufferState.Memory;

                unsafe
                {
                    byte* bOffset, b;

                    fixed (TMultiByte* mb = buffer.Span)
                    {
                        fixed (TWideChar* wch = wchBuffer.Span)
                        {
                            for (int i = 0; i < buffer.Length; i++)
                            {
                                b = (byte*)(mb + i);
                                bOffset = b + _WideCharOffset;

                                Unsafe.CopyBlock(wch + i, bOffset, wideCharSize);
                            }
                        }
                    }
                }

                return WideCharFactory<TWideChar>.GenerateString(wchBuffer);
            }
        }

        #region IMultiByteCharFactory
        public TMultiByte GetNativeChar(int ch)
            => this.GetNativeChar(ch, 0, 0);

        public TMultiByte GetNativeChar(byte ch)
            => this.GetNativeChar(ch, 0, 0);

        public byte GetByte(TMultiByte @char)
        {
            byte res;

            unsafe
            {
                byte* b = ((byte*)(&@char)) + _WideCharOffset;
                res = *b;
            }

            return res;
        }

        public int GetChar(TMultiByte @char)
        {
            TWideChar wCh = default;

            unsafe
            {
                byte* b = ((byte*)(&@char)) + _WideCharOffset;
                wCh = *((TWideChar*)b);
            }

            using (BufferState<TWideChar> wchBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer((CharEncoderState)1))
            {
                Memory<TWideChar> wchBuffer = wchBufferState.Memory;
                wchBuffer.Span[0] = wCh;

                return (WideCharFactory<TWideChar>.GenerateString(wchBuffer))[0];
            }
        }

        public BufferState<TMultiByte> GetNativeEmptyString(CreateBuffer<TMultiByte> createBuffer, int length, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            BufferState<TMultiByte> bufferState = createBuffer((CharEncoderState)length);
            bufferState.Memory.Span.Clear();
            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, ReadOnlySpan<char> str, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory.Slice(0, str.Length),
                    0,
                    0
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, ReadOnlySpan<byte> str, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, Encoding.ASCII);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, Encoding.ASCII, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory,
                    0,
                    0
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, in ReadOnlySequence<byte> str, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, Encoding.ASCII);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, Encoding.ASCII, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory,
                    0,
                    0
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, ReadOnlySpan<byte> str, Encoding encoding, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, encoding);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, encoding, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory,
                    0,
                    0
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, in ReadOnlySequence<byte> str, Encoding encoding, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, encoding);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, encoding, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory,
                    0,
                    0
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, ref TMultiByte strRef, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            int length = NativeNCurses.FindStringLength(ref strRef);
            BufferState<TMultiByte> bufferState = createBuffer((CharEncoderState)length);

            ref TMultiByte bufferRef = ref bufferState.Memory.Span.GetPinnableReference();

            Unsafe.CopyBlock
            (
                ref Unsafe.As<TMultiByte, byte>(ref bufferRef),
                ref Unsafe.As<TMultiByte, byte>(ref strRef),
                (uint)(bufferState.EncoderState.BufferLength * Marshal.SizeOf<TMultiByte>())
            );

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(bufferState.Memory);

            return bufferState;
        }
        #endregion

        #region IMultiByteNCursesCharFactory
        public TMultiByte GetNativeChar(byte ch, ulong attrs, ushort colorPair)
            => CreateMultiByteChar((char)ch, attrs, colorPair);


        public TMultiByte GetNativeChar(int ch, ulong attrs, ushort colorPair)
            => CreateMultiByteChar(ch, attrs, colorPair);

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, ReadOnlySpan<char> str, ulong attrs, ushort colorPair, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory,
                    attrs,
                    colorPair
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, ReadOnlySpan<byte> str, ulong attrs, ushort colorPair, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, Encoding.ASCII);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, Encoding.ASCII, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory,
                    attrs,
                    colorPair
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, in ReadOnlySequence<byte> str, ulong attrs, ushort colorPair, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, Encoding.ASCII);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, Encoding.ASCII, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory,
                    attrs,
                    colorPair
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, ushort colorPair, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, encoding);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str,  encoding, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory,
                    attrs,
                    colorPair
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }

        public BufferState<TMultiByte> GetNativeString(CreateBuffer<TMultiByte> createBuffer, in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort colorPair, out MultiByteCharString<TMultiByte, TWideChar, TSingleByte> @string)
        {
            CharEncoderState encoderState = PlatformWideCharEncoder<TWideChar>._Instance.GetBufferSize(str, encoding);
            BufferState<TMultiByte> multiBufferState = createBuffer((CharEncoderState)encoderState.BufferLength);

            using (BufferState<TWideChar> wideBufferState = WideCharFactory<TWideChar>._CreatePooledBuffer(encoderState))
            {
                PlatformWideCharEncoder<TWideChar>._Instance.Encode(str, encoding, wideBufferState);

                CreateMultiByteStringFromWideCharString
                (
                    multiBufferState.Memory,
                    wideBufferState.Memory,
                    attrs,
                    colorPair
                );
            }

            @string = new MultiByteCharString<TMultiByte, TWideChar, TSingleByte>(multiBufferState.Memory);
            return multiBufferState;
        }
        #endregion
    }
}
