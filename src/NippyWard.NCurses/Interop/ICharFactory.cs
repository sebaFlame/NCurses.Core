using System;
using System.Collections.Generic;
using System.Text;
using System.Buffers;

using NippyWard.NCurses.Interop.Platform;

namespace NippyWard.NCurses.Interop
{
    internal delegate BufferState<TChar> CreateBuffer<TChar>(CharEncoderState encoderState, bool addNullTerminator = true)
        where TChar : unmanaged, IChar;

    internal interface ICharFactory<TChar, TString>
        where TChar : unmanaged, IChar
        where TString : ICharString
    {
        TChar GetNativeChar(byte @char);
        byte GetByte(TChar @char);

        BufferState<TChar> GetNativeEmptyString(CreateBuffer<TChar> createBuffer, int length, out TString @string);

        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ReadOnlySpan<char> str, out TString @string);
        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ReadOnlySpan<byte> str, out TString @string);
        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, in ReadOnlySequence<byte> str, out TString @string);

        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ref TChar strRef, out TString @string);
    }

    internal interface IMultiByteCharFactory<TChar, TString> : ICharFactory<TChar, TString>
        where TChar : unmanaged, IMultiByteChar, IEquatable<TChar>
        where TString : struct, ICharString
    {
        TChar GetNativeChar(int @char);
        int GetChar(TChar @char);

        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ReadOnlySpan<byte> str, Encoding encoding, out TString @string);
        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, in ReadOnlySequence<byte> str, Encoding encoding, out TString @string);
    }

    internal interface INCursesCharFactory<TChar, TString> : ICharFactory<TChar, TString>
        where TChar : unmanaged, INCursesChar
        where TString : INCursesCharString
    {
        TChar GetNativeChar(byte ch, ulong attrs, ushort colorPair);

        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ReadOnlySpan<char> str, ulong attrs, ushort colorPair, out TString @string);
        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ReadOnlySpan<byte> str, ulong attrs, ushort colorPair, out TString @string);
        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, in ReadOnlySequence<byte> str, ulong attrs, ushort colorPair, out TString @string);
    }

    internal interface IMultiByteNCursesCharFactory<TChar, TString> : IMultiByteCharFactory<TChar, TString>, INCursesCharFactory<TChar, TString>
        where TChar : unmanaged, IMultiByteChar, INCursesChar, IEquatable<TChar>
        where TString : struct, INCursesCharString
    {
        TChar GetNativeChar(int @char, ulong attrs, ushort colorPair);

        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, ReadOnlySpan<byte> str, Encoding encoding, ulong attrs, ushort colorPair, out TString @string);
        BufferState<TChar> GetNativeString(CreateBuffer<TChar> createBuffer, in ReadOnlySequence<byte> str, Encoding encoding, ulong attrs, ushort colorPair, out TString @string);
    }
}
