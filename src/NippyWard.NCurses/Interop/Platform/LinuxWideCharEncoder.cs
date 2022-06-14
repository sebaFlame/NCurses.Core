using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NippyWard.NCurses.Interop.Platform
{
    internal class LinuxWideCharEncoder<TWideChar> : PlatformWideCharEncoder<TWideChar>
        where TWideChar : unmanaged, IMultiByteChar
    {
        internal override Encoding PlatformEncoding => UTF8;

        static LinuxWideCharEncoder()
        { }

        internal override CharEncoderState GetBufferSize(ReadOnlySpan<byte> @string, Encoding encoding)
        {
            CharEncoderState encoderState;
            ReadOnlySpan<byte> strSpan;

            if (encoding.CodePage == this.PlatformEncoding.CodePage
                || encoding.CodePage == ASCII.CodePage)
            {
                //guarantee the buffer is null terminated
                encoderState = new CharEncoderState(@string.Length + 1);
                Span<byte> strNullTerminated = new Span<byte>(encoderState.IntermediateBuffer, 0, @string.Length + 1);
                @string.CopyTo(strNullTerminated);
                //buffer could not be cleared
                strNullTerminated[strNullTerminated.Length - 1] = 0;

                strSpan = strNullTerminated;
                encoderState.IntermadiateBufferLength = strNullTerminated.Length;
            }
            else
            {
                /* create a buffer atleast @string.Length (byte length, ASCII length is always max!) long
                 * and add room for a null terminator */
                encoderState = new CharEncoderState((int)@string.Length + 1);

                char[] charArr = null;
                ReadOnlySpan<char> charSpan;
                int charsUsed = 0, bytesUsed = 0;
                bool completed = false;

                if (encoding.CodePage == UTF16.CodePage)
                {
                    charSpan = MemoryMarshal.Cast<byte, char>(@string);
                }
                else
                { 
                    //create a decoder
                    Decoder decoder = GetDecoder(encoding);
                    decoder.Reset();

                    charArr = ArrayPool<char>.Shared.Rent(@string.Length);

                    try
                    {
                        unsafe
                        {
                            fixed (byte* buffer = @string)
                            {
                                fixed (char* chars = charArr)
                                {
                                    decoder.Convert
                                    (
                                        buffer,
                                        @string.Length,
                                        chars,
                                        charArr.Length,
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

                        charSpan = new ReadOnlySpan<char>(charArr, 0, charsUsed);
                    }
                    catch (Exception)
                    {
                        ArrayPool<char>.Shared.Return(charArr);
                        throw;
                    }
                }

                try
                {
                    Encoder encoder = GetEncoder(this.PlatformEncoding);
                    encoder.Reset();

                    unsafe
                    {
                        fixed (char* chars = charSpan)
                        {
                            fixed (byte* bytes = encoderState.IntermediateBuffer)
                            {
                                encoder.Convert
                                (
                                    chars,
                                    charSpan.Length,
                                    bytes,
                                    encoderState.IntermediateBuffer.Length,
                                    false,
                                    out charsUsed,
                                    out bytesUsed,
                                    out completed
                                );
                            }
                        }
                    }

                    if (!completed)
                    {
                        throw new InvalidOperationException("Encoding conversion did not succeed");
                    }

                    encoderState.IntermediateBuffer[bytesUsed] = 0;
                    strSpan = new ReadOnlySpan<byte>(encoderState.IntermediateBuffer, 0, bytesUsed + 1); //add null terminator
                    encoderState.IntermadiateBufferLength = strSpan.Length;
                }
                finally
                {
                    if (!(charArr is null))
                    {
                        ArrayPool<char>.Shared.Return(charArr);
                    }
                }
            }

            mbstate mbstate = new mbstate();
            IntPtr chPtr, ptrToPtr;
            int res;

            ptrToPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());

            try
            {
                ref byte chRef = ref Unsafe.AsRef(in strSpan.GetPinnableReference());

                unsafe
                {
                    void* pointer = Unsafe.AsPointer(ref chRef);
                    chPtr = new IntPtr(pointer);
                }

                Marshal.WriteIntPtr(ptrToPtr, chPtr);

                if ((res = LinuxLoader.mbsrtowcs(IntPtr.Zero, ptrToPtr, 0, ref mbstate)) < 0)
                {
                    throw new InvalidOperationException("Conversion failed");
                }

                encoderState.BufferLength += res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrToPtr);
            }

            return encoderState;
        }

        internal override CharEncoderState GetBufferSize(in ReadOnlySequence<byte> @string, Encoding encoding)
        {
            CharEncoderState encoderState;
            ReadOnlySpan<byte> strSpan;

            if (encoding.CodePage == this.PlatformEncoding.CodePage
                || encoding.CodePage == ASCII.CodePage)
            {
                //guarantee the buffer is null terminated
                encoderState = new CharEncoderState((int)@string.Length + 1);
                Span<byte> strNullTerminated = new Span<byte>(encoderState.IntermediateBuffer, 0, (int)@string.Length + 1);
                @string.CopyTo(strNullTerminated);

                //buffer could not be cleared
                strNullTerminated[strNullTerminated.Length - 1] = 0;
                strSpan = strNullTerminated;
                encoderState.IntermadiateBufferLength = strNullTerminated.Length;
            }
            else
            {
                //create a buffer atleast @string.Length (byte length, ASCII length is always max!) long
                encoderState = new CharEncoderState((int)@string.Length + 1);
                int charsUsed = 0, bytesUsed = 0;
                bool completed = false;
                int bytesIndex = 0;

                if (encoding.CodePage == UTF16.CodePage)
                {
                    ReadOnlySpan<char> charSpan;

                    Encoder encoder = GetEncoder(this.PlatformEncoding);
                    encoder.Reset();

                    foreach (ReadOnlyMemory<byte> memory in @string)
                    {
                        if (memory.IsEmpty)
                        {
                            continue;
                        }

                        charSpan = MemoryMarshal.Cast<byte, char>(memory.Span);

                        unsafe
                        {
                            fixed (char* chars = charSpan)
                            {
                                fixed (byte* bytes = encoderState.IntermediateBuffer)
                                {
                                    encoder.Convert
                                    (
                                        chars,
                                        charSpan.Length,
                                        bytes + bytesIndex,
                                        encoderState.IntermediateBuffer.Length,
                                        false,
                                        out charsUsed,
                                        out bytesUsed,
                                        out completed
                                    );
                                }
                            }
                        }

                        if (!completed)
                        {
                            throw new InvalidOperationException("Encoding conversion did not succeed");
                        }

                        bytesIndex += bytesUsed;
                    }
                }
                else
                {
                    int charsIndex = 0;

                    Decoder decoder = GetDecoder(encoding);
                    decoder.Reset();

                    char[] charArr = ArrayPool<char>.Shared.Rent((int)@string.Length);

                    try
                    {
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
                                    fixed (char* chars = charArr)
                                    {
                                        decoder.Convert
                                        (
                                            buffer,
                                            (int)@string.Length,
                                            chars + charsUsed,
                                            charArr.Length,
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

                            charsIndex += charsUsed;
                        }

                        Encoder encoder = GetEncoder(this.PlatformEncoding);
                        encoder.Reset();

                        unsafe
                        {
                            fixed (char* chars = charArr)
                            {
                                fixed (byte* bytes = encoderState.IntermediateBuffer)
                                {
                                    encoder.Convert
                                    (
                                        chars,
                                        charsIndex,
                                        bytes,
                                        encoderState.IntermediateBuffer.Length,
                                        false,
                                        out charsUsed,
                                        out bytesUsed,
                                        out completed
                                    );
                                }
                            }
                        }

                        if (!completed)
                        {
                            throw new InvalidOperationException("Encoding conversion did not succeed");
                        }

                        bytesIndex = bytesUsed;
                    }
                    finally
                    {
                        ArrayPool<char>.Shared.Return(charArr);
                    }
                }

                encoderState.IntermediateBuffer[bytesIndex] = 0;
                strSpan = new ReadOnlySpan<byte>(encoderState.IntermediateBuffer, 0, bytesIndex + 1); //add null terminator
                encoderState.IntermadiateBufferLength = strSpan.Length;
            }

            mbstate mbstate = new mbstate();
            IntPtr chPtr, ptrToPtr;
            int res;

            ptrToPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());

            try
            {
                ref byte chRef = ref Unsafe.AsRef(in strSpan.GetPinnableReference());

                unsafe
                {
                    void* pointer = Unsafe.AsPointer(ref chRef);
                    chPtr = new IntPtr(pointer);
                }

                Marshal.WriteIntPtr(ptrToPtr, chPtr);

                if ((res = LinuxLoader.mbsrtowcs(IntPtr.Zero, ptrToPtr, 0, ref mbstate)) < 0)
                {
                    throw new InvalidOperationException("Conversion failed");
                }

                encoderState.BufferLength = res;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrToPtr);
            }

            return encoderState;
        }

        private static void Encode(in BufferState<TWideChar> bufferState)
        {
            if(bufferState.EncoderState.IntermediateBuffer is null)
            {
                throw new NullReferenceException("Intermediate buffer can not be null");
            }

            mbstate mbstate = new mbstate();
            IntPtr mbPtr, ptrToPtr, wcPtr;
            int res;

            ReadOnlySpan<byte> @string = new ReadOnlySpan<byte>(bufferState.EncoderState.IntermediateBuffer, 0, bufferState.EncoderState.IntermadiateBufferLength);

            ptrToPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());
            try
            {
                ref byte chRef = ref Unsafe.AsRef(in @string.GetPinnableReference());
                ref TWideChar wcRef = ref bufferState.Memory.Span.GetPinnableReference();

                unsafe
                {
                    mbPtr = new IntPtr(Unsafe.AsPointer(ref chRef));
                    wcPtr = new IntPtr(((TWideChar*)Unsafe.AsPointer(ref wcRef)));
                }

                Marshal.WriteIntPtr(ptrToPtr, mbPtr);

                if ((res = LinuxLoader.mbsrtowcs(wcPtr, ptrToPtr, bufferState.Memory.Length, ref mbstate)) < 0)
                {
                    throw new InvalidOperationException("Conversion failed");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptrToPtr);
            }
        }

        internal override void Encode(
            in ReadOnlySequence<byte> @string, 
            Encoding encoding, 
            in BufferState<TWideChar> bufferState)
        {
            Encode(bufferState);
        }

        internal override void Encode(
            ReadOnlySpan<byte> @string,
            Encoding encoding,
            in BufferState<TWideChar> bufferState)
        {
            Encode(bufferState);
        }

        internal override string GenerateString(ReadOnlyMemory<TWideChar> buffer)
        {
            mbstate mbstate;
            IntPtr mbPtr, ptrToPtr, wcPtr;
            int res;

            ptrToPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IntPtr>());

            try
            {
                ReadOnlySpan<byte> byteBuffer = MemoryMarshal.Cast<TWideChar, byte>(buffer.Span);
                ref byte wcRef = ref Unsafe.AsRef(in byteBuffer.GetPinnableReference());

                unsafe
                {
                    wcPtr = new IntPtr(((TWideChar*)Unsafe.AsPointer(ref wcRef)));
                }

                Marshal.WriteIntPtr(ptrToPtr, wcPtr);

                mbstate = new mbstate();
                int byteCount = LinuxLoader.wcsrtombs(IntPtr.Zero, ptrToPtr, 0, ref mbstate);

                byte[] pooledByteBuffer = ArrayPool<byte>.Shared.Rent(byteCount);
                try
                {
                    Span<byte> mbString = new Span<byte>(pooledByteBuffer);

                    ref byte mbRef = ref mbString.GetPinnableReference();

                    unsafe
                    {
                        mbPtr = new IntPtr(Unsafe.AsPointer(ref mbRef));
                    }

                    mbstate = new mbstate();
                    int resByteCount = LinuxLoader.wcsrtombs(mbPtr, ptrToPtr, byteCount, ref mbstate);

                    if (byteCount != resByteCount)
                    {
                        throw new InvalidOperationException("Conversion failed");
                    }

                    int bytesUsed = 0, charsUsed = 0;
                    bool completed = false;

                    char[] charArrary = new char[byteCount];

                    Decoder decoder = GetDecoder(this.PlatformEncoding);
                    decoder.Reset();

                    unsafe
                    {
                        fixed (char* chars = charArrary)
                        {
                            byte* b = (byte*)Unsafe.AsPointer(ref mbRef);

                            decoder.Convert
                            (
                                b,
                                resByteCount,
                                chars,
                                charArrary.Length,
                                false,
                                out bytesUsed,
                                out charsUsed,
                                out completed
                            );
                        }
                    }

                    if (!completed)
                    {
                        throw new InvalidOperationException("Conversion did not complete!");
                    }

                    Span<char> charSpan = new Span<char>(charArrary, 0, charsUsed);
                    return charSpan.ToString();
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(pooledByteBuffer);
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptrToPtr);
            }
        }

        internal override TWideChar Encode(int @char)
        {
            ReadOnlySpan<char> chSpan;
            Span<byte> mbSpan;
            Span<TWideChar> wchSpan;

            unsafe
            {
                int* charArr = stackalloc int[1];
                charArr[0] = @char;
                chSpan = new Span<char>(charArr, 1);

                byte* bArr = stackalloc byte[4];
                mbSpan = new Span<byte>(bArr, 4);

                TWideChar* wCharArr = stackalloc TWideChar[1];
                wchSpan = new Span<TWideChar>(wCharArr, 1);
            }

            Encoder encoder = GetEncoder(this.PlatformEncoding);
            encoder.Reset();

            int charsUsed, bytesUsed;
            bool completed;

            unsafe
            {
                fixed (char* chars = chSpan)
                {
                    fixed (byte* bytes = mbSpan)
                    {
                        encoder.Convert
                        (
                            chars,
                            1,
                            bytes,
                            4,
                            true,
                            out charsUsed,
                            out bytesUsed,
                            out completed
                        );
                    }
                }
            }

            if (!completed)
            {
                throw new InvalidOperationException("Conversion did not complete!");
            }

            IntPtr wcPtr, mbPtr;
            mbstate mbstate = new mbstate();
            int res;

            unsafe
            {
                wcPtr = new IntPtr(Unsafe.AsPointer(ref wchSpan.GetPinnableReference()));
                mbPtr = new IntPtr(Unsafe.AsPointer(ref mbSpan.GetPinnableReference()));
            }

            if ((res = LinuxLoader.mbrtowc(wcPtr, mbPtr, bytesUsed, ref mbstate)) < 0)
            {
                throw new InvalidOperationException("Conversion failed");
            }

            return wchSpan[0];
        }
    }
}
