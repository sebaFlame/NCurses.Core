using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Buffers;

namespace NippyWard.NCurses.Interop.SingleByte
{
    internal class SingleByteCharFactory<TSingleByte> :
        INCursesCharFactory<TSingleByte, SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
    {
        internal static Func<byte, ulong, ushort, TSingleByte> _CreateCharWithAttributeAndColorPairFromByte;
        internal static Func<ulong, TSingleByte> _CreateCharFromAttribute;

        internal static SingleByteCharFactory<TSingleByte> _Instance;
        internal static CreateBuffer<TSingleByte> _CreatePooledBuffer;
        internal static CreateBuffer<TSingleByte> _CreateArrayBuffer;

        static SingleByteCharFactory()
        {
            _Instance = new SingleByteCharFactory<TSingleByte>();

            _CreatePooledBuffer = NativeNCurses.CreatePoolBuffer<TSingleByte>;
            _CreateArrayBuffer = NativeNCurses.CreateArrayBuffer<TSingleByte>;

            ConstructorInfo ctor;
            ParameterExpression par1, par2, par3;

            par1 = Expression.Parameter(typeof(byte));
            par2 = Expression.Parameter(typeof(ulong));
            par3 = Expression.Parameter(typeof(ushort));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(byte), typeof(ulong), typeof(ushort) });
            _CreateCharWithAttributeAndColorPairFromByte =
                Expression.Lambda<Func<byte, ulong, ushort, TSingleByte>>(Expression.New(ctor, par1, par2, par3), par1, par2, par3).Compile();

            par1 = Expression.Parameter(typeof(ulong));
            ctor = typeof(TSingleByte).GetConstructor(new Type[] { typeof(ulong) });
            _CreateCharFromAttribute =
                Expression.Lambda<Func<ulong, TSingleByte>>(Expression.New(ctor, par1), par1).Compile();
        }

        #region Helper methods
        private static void CreateSingleByteStringFromCharString(
            ReadOnlySpan<char> str,
            Memory<TSingleByte> buffer,
            ulong attrs,
            ushort colorPair)
        {
            uint singleByteSize = (uint)Unsafe.SizeOf<TSingleByte>();

            if (colorPair > 0)
            {
                attrs |= (uint)NativeNCurses.COLOR_PAIR(colorPair);
            }

            unsafe
            {
                ulong ch;
                byte* chType;

                fixed(char* chars = str)
                {
                    fixed (TSingleByte* b = buffer.Span)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {
                            ch = (byte)*(chars + i);

                            if (attrs > 0)
                            {
                                ch |= attrs;
                            }

                            chType = ((byte*)&ch);
                            Unsafe.CopyBlock(b + i, chType, singleByteSize);
                        }
                    }
                }
            }
        }

        private static void CreateSingleByteStringFromCharString(
            ReadOnlySpan<byte> str,
            Memory<TSingleByte> buffer,
            ulong attrs,
            ushort colorPair)
        {
            uint singleByteSize = (uint)Unsafe.SizeOf<TSingleByte>();

            if (colorPair > 0)
            {
                attrs |= (uint)NativeNCurses.COLOR_PAIR(colorPair);
            }

            unsafe
            {
                ulong ch;
                byte* chType;

                fixed (byte* bytes = str)
                {
                    fixed (TSingleByte* b = buffer.Span)
                    {
                        for (int i = 0; i < str.Length; i++)
                        {
                            ch = *(bytes + i);

                            if (attrs > 0)
                            {
                                ch |= attrs;
                            }

                            chType = ((byte*)&ch);
                            Unsafe.CopyBlock(b + i, chType, singleByteSize);
                        }
                    }
                }
            }
        }

        private static void CreateSingleByteStringFromCharString(
            ReadOnlySequence<byte> str,
            Memory<TSingleByte> buffer,
            ulong attrs,
            ushort colorPair)
        {
            if (str.IsSingleSegment)
            {
                CreateSingleByteStringFromCharString(str.First.Span, buffer, attrs, colorPair);
                return;
            }

            int offset = 0;
            uint singleByteSize = (uint)Unsafe.SizeOf<TSingleByte>();

            if (colorPair > 0)
            {
                attrs |= (uint)NativeNCurses.COLOR_PAIR(colorPair);
            }

            foreach (ReadOnlyMemory<byte> memory in str)
            {
                if (memory.IsEmpty)
                {
                    continue;
                }

                unsafe
                {
                    ulong ch;
                    TSingleByte* chOffset;
                    byte* chType;

                    fixed (byte* bytes = memory.Span)
                    {
                        fixed (TSingleByte* b = buffer.Span)
                        {
                            chOffset = b + offset;

                            for (int i = 0; i < memory.Length; i++)
                            {
                                ch = *(bytes +  i);

                                if (attrs > 0)
                                {
                                    ch |= attrs;
                                }

                                chType = ((byte*)&ch);
                                Unsafe.CopyBlock(chOffset + i, chType, singleByteSize);
                            }
                        }
                    }
                }

                offset += memory.Length;
            }
        }

        internal static string GenerateString(Memory<TSingleByte> buffer)
        {
            char[] chars = new char[buffer.Length - 1];
            int singleByteSize = Unsafe.SizeOf<TSingleByte>();

            unsafe
            {
                byte* bCh, cCh;

                fixed (TSingleByte* b = buffer.Span)
                {
                    fixed (char* c = chars)
                    {
                        for (int i = 0; i < buffer.Length - 1; i++)
                        {
                            bCh = ((byte*)(b + i));
                            cCh = ((byte*)(c + i));

                            Unsafe.Write(cCh, *bCh);
                        }
                    }
                }
            }

            return new string(chars);
        }
        #endregion

        public TSingleByte GetNativeChar(byte @char)
            => this.GetNativeChar(@char, 0, 0);

        public TSingleByte GetNativeChar(byte ch, ulong attrs, ushort colorPair)
            => _CreateCharWithAttributeAndColorPairFromByte(ch, attrs, colorPair);

        public TSingleByte GetNativeAttribute(ulong attrs)
            => _CreateCharFromAttribute(attrs);

        public byte GetByte(TSingleByte @char)
        {
            byte res;
            int charSize = Unsafe.SizeOf<TSingleByte>();

            unsafe
            {
                byte* b = (byte*)(&@char);
                res = *b;
            }

            return res;
        }

        public BufferState<TSingleByte> GetNativeEmptyString(CreateBuffer<TSingleByte> createBuffer, int length, out SingleByteCharString<TSingleByte> @string)
        {
            BufferState<TSingleByte> bufferState = createBuffer((CharEncoderState)length);
            bufferState.Memory.Span.Clear();
            @string = new SingleByteCharString<TSingleByte>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TSingleByte> GetNativeString(CreateBuffer<TSingleByte> createBuffer, ReadOnlySpan<char> str, out SingleByteCharString<TSingleByte> @string)
        {
            BufferState<TSingleByte> bufferState = createBuffer((CharEncoderState)str.Length);
            CreateSingleByteStringFromCharString(str, bufferState.Memory, 0, 0);
            @string = new SingleByteCharString<TSingleByte>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TSingleByte> GetNativeString(CreateBuffer<TSingleByte> createBuffer, ReadOnlySpan<byte> str, out SingleByteCharString<TSingleByte> @string)
        {
            BufferState<TSingleByte> bufferState = createBuffer((CharEncoderState)str.Length);
            CreateSingleByteStringFromCharString(str, bufferState.Memory, 0, 0);
            @string = new SingleByteCharString<TSingleByte>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TSingleByte> GetNativeString(CreateBuffer<TSingleByte> createBuffer, in ReadOnlySequence<byte> str, out SingleByteCharString<TSingleByte> @string)
        {
            BufferState<TSingleByte> bufferState = createBuffer((CharEncoderState)str.Length);
            CreateSingleByteStringFromCharString(str, bufferState.Memory, 0, 0);
            @string = new SingleByteCharString<TSingleByte>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TSingleByte> GetNativeString(CreateBuffer<TSingleByte> createBuffer, ref TSingleByte strRef, out SingleByteCharString<TSingleByte> @string)
        {
            int length = NativeNCurses.FindStringLength(ref strRef);
            BufferState<TSingleByte> bufferState = createBuffer((CharEncoderState)length);

            ref TSingleByte bufferRef = ref bufferState.Memory.Span.GetPinnableReference();

            Unsafe.CopyBlock
            (
                ref Unsafe.As<TSingleByte, byte>(ref bufferRef),
                ref Unsafe.As<TSingleByte, byte>(ref strRef),
                (uint)(bufferState.EncoderState.BufferLength * Marshal.SizeOf<TSingleByte>())
            );

            @string = new SingleByteCharString<TSingleByte>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TSingleByte> GetNativeString(CreateBuffer<TSingleByte> createBuffer, ReadOnlySpan<char> str, ulong attrs, ushort colorPair, out SingleByteCharString<TSingleByte> @string)
        {
            BufferState<TSingleByte> bufferState = createBuffer((CharEncoderState)str.Length);
            CreateSingleByteStringFromCharString(str, bufferState.Memory, attrs, colorPair);
            @string = new SingleByteCharString<TSingleByte>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TSingleByte> GetNativeString(CreateBuffer<TSingleByte> createBuffer, ReadOnlySpan<byte> str, ulong attrs, ushort colorPair, out SingleByteCharString<TSingleByte> @string)
        {
            BufferState<TSingleByte> bufferState = createBuffer((CharEncoderState)str.Length);
            CreateSingleByteStringFromCharString(str, bufferState.Memory, attrs, colorPair);
            @string = new SingleByteCharString<TSingleByte>(bufferState.Memory);
            return bufferState;
        }

        public BufferState<TSingleByte> GetNativeString(CreateBuffer<TSingleByte> createBuffer, in ReadOnlySequence<byte> str, ulong attrs, ushort colorPair, out SingleByteCharString<TSingleByte> @string)
        {
            BufferState<TSingleByte> bufferState = createBuffer((CharEncoderState)str.Length);
            CreateSingleByteStringFromCharString(str, bufferState.Memory, attrs, colorPair);
            @string = new SingleByteCharString<TSingleByte>(bufferState.Memory);
            return bufferState;
        }
    }
}
