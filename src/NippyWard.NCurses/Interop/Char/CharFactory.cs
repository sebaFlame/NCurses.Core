using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Buffers;

namespace NippyWard.NCurses.Interop.Char
{
    internal class CharFactory<TChar> : ICharFactory<TChar, CharString<TChar>>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
    {
        internal static CharFactory<TChar> _Instance;
        internal static CreateBuffer<TChar> _CreatePooledBuffer;
        internal static CreateBuffer<TChar> _CreateArrayBuffer;

        private static Func<byte, TChar> _CreateCharFromByte;

        static CharFactory()
        {
            _Instance = new CharFactory<TChar>();

            _CreatePooledBuffer = NativeNCurses.CreatePoolBuffer<TChar>;
            _CreateArrayBuffer = NativeNCurses.CreateArrayBuffer<TChar>;

            ConstructorInfo ctor;
            ParameterExpression par1;

            par1 = Expression.Parameter(typeof(byte));
            ctor = typeof(TChar).GetConstructor(new Type[] { typeof(byte) });
            _CreateCharFromByte =
                Expression.Lambda<Func<byte, TChar>>(Expression.New(ctor, par1), par1).Compile();
        }

        public TChar GetNativeChar(byte @char)
            => _CreateCharFromByte(@char);

        public byte GetByte(TChar @char)
        {
            byte res;

            unsafe
            {
                byte* b = (byte*)(&@char);
                res = *b;
            }

            return res;
        }

        #region Helper methods
        private static unsafe void CreateSingleByteStringFromCharString(
            ReadOnlySpan<char> str,
            Memory<TChar> buffer)
        {
            byte* ch;

            fixed (char* chars = str)
            {
                fixed (TChar* b = buffer.Span)
                {
                    for (int i = 0; i < str.Length; i++)
                    {
                        ch = ((byte*)(chars + i));
                        Unsafe.Write(b + i, *ch);
                    }
                }
            }
        }

        private static void CreateSingleByteStringFromCharString(
            ReadOnlySpan<byte> str,
            Memory<TChar> buffer)
        {
            unsafe
            {
                byte* ch;

                fixed (byte* chars = str)
                {
                    fixed (TChar* b = buffer.Span)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {
                            ch = chars + i;
                            Unsafe.Write(b + i, *ch);
                        }
                    }
                }
            }
        }

        private static void CreateSingleByteStringFromCharString(
            ReadOnlySequence<byte> str,
            Memory<TChar> buffer)
        {
            int offset = 0;

            foreach (ReadOnlyMemory<byte> memory in str)
            {
                if (memory.IsEmpty)
                {
                    continue;
                }

                unsafe
                {
                    byte* ch;
                    TChar* bOffset;

                    fixed (byte* chars = memory.Span)
                    {
                        fixed (TChar* b = buffer.Span)
                        {
                            bOffset = b + offset;

                            for (int i = 0; i < memory.Length; i++)
                            {
                                ch = chars + i;
                                Unsafe.Write(bOffset + i, *ch);
                            }
                        }
                    }
                }

                offset += memory.Length;
            }
        }

        internal static string GenerateString(Memory<TChar> buffer)
        {
            char[] chars = new char[buffer.Length - 1];

            unsafe
            {
                byte* bCh, cCh;

                fixed (TChar* b = buffer.Span)
                {
                    fixed (char* c = chars)
                    {
                        for (int i = 0; i < buffer.Length - 1; i++)
                        {
                            bCh = (byte*)(b + i);
                            cCh = (byte*)(c + i);

                            Unsafe.Write(cCh, *bCh);
                        }
                    }
                }
            }

            return new string(chars);
        }
        #endregion

        public BufferState<TChar> GetNativeEmptyString(CreateBuffer<TChar> createBuffer, int length, out CharString<TChar> @string)
        {
            BufferState<TChar> bufferState = createBuffer((CharEncoderState)length);
            bufferState.Memory.Span.Clear();
            @string = new CharString<TChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ReadOnlySpan<char> str, out CharString<TChar> @string)
        {
            BufferState<TChar> bufferState = createBuffer((CharEncoderState)str.Length);
            CreateSingleByteStringFromCharString(str, bufferState.Memory);
            @string = new CharString<TChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ReadOnlySpan<byte> str, out CharString<TChar> @string)
        {
            BufferState<TChar> bufferState = createBuffer((CharEncoderState)str.Length);
            CreateSingleByteStringFromCharString(str, bufferState.Memory);
            @string = new CharString<TChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, in ReadOnlySequence<byte> str, out CharString<TChar> @string)
        {
            BufferState<TChar> bufferState = createBuffer((CharEncoderState)str.Length);
            CreateSingleByteStringFromCharString(str, bufferState.Memory);
            @string = new CharString<TChar>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ref TChar strRef, out CharString<TChar> @string)
        {
            int length = NativeNCurses.FindStringLength(ref strRef);
            BufferState<TChar> bufferState = createBuffer((CharEncoderState)length);

            ref TChar bufferRef = ref bufferState.Memory.Span.GetPinnableReference();

            Unsafe.CopyBlock
            (
                ref Unsafe.As<TChar, byte>(ref bufferRef),
                ref Unsafe.As<TChar, byte>(ref strRef),
                (uint)(bufferState.EncoderState.BufferLength * Marshal.SizeOf<TChar>())
            );

            @string = new CharString<TChar>(bufferState.Memory);
            return bufferState;
        }
    }
}
