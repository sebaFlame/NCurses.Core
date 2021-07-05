using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;

using NCurses.Core.Interop.Platform;

namespace NCurses.Core.Interop.Dynamic
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct wchar_t : IMultiByteChar, IEquatable<wchar_t> //wchar_t & wint_t
    {
        private const int _WcharTSize = 2; //Constants.SIZEOF_WCHAR_T

        public unsafe fixed byte @char[_WcharTSize];

        public int Char
        {
            get
            {
                int ret = 0;

                unsafe
                {
                    int* retPtr = &ret;

                    fixed (byte* b = @char)
                    {
                        Unsafe.CopyBlock(retPtr, b, _WcharTSize);
                    }
                }

                return ret;
            }
        }

        public wchar_t(Span<byte> encodedBytesChar)
        {
            if (encodedBytesChar.Length > _WcharTSize)
            {
                throw new ArgumentOutOfRangeException($"Wide char can only contain {_WcharTSize} bytes");
            }

            unsafe
            {
                fixed (byte* b = this.@char)
                {
                    Span<byte> @char = new Span<byte>(b, _WcharTSize);
                    encodedBytesChar.CopyTo(@char);
                }
            }
        }

        public wchar_t(ArraySegment<byte> encodedBytesChar)
        {
            if (encodedBytesChar.Count > _WcharTSize)
            {
                throw new ArgumentOutOfRangeException($"Wide char can only contain {_WcharTSize} bytes");
            }

            unsafe
            {
                fixed (byte* b = this.@char)
                {
                    Span<byte> @char = new Span<byte>(b, _WcharTSize);
                    ((Span<byte>)encodedBytesChar).CopyTo(@char);
                }
            }
        }

        public static bool operator ==(in wchar_t wchLeft, in wchar_t wchRight)
        {
            unsafe
            {
                fixed (byte* l = wchLeft.@char, r = wchRight.@char)
                {
                    Span<byte> leftSpan = new Span<byte>(l, _WcharTSize);
                    Span<byte> rightSpan = new Span<byte>(r, _WcharTSize);

                    return leftSpan.SequenceEqual(rightSpan);
                }
            }
        }

        public static bool operator !=(in wchar_t wchLeft, in wchar_t wchRight)
        {
            unsafe
            {
                fixed (byte* l = wchLeft.@char, r = wchRight.@char)
                {
                    Span<byte> leftSpan = new Span<byte>(l, _WcharTSize);
                    Span<byte> rightSpan = new Span<byte>(r, _WcharTSize);

                    return !leftSpan.SequenceEqual(rightSpan);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is wchar_t wch)
            {
                return this.Equals(wch);
            }
            return false;
        }

        public bool Equals(IChar other)
        {
            if (other is wchar_t wch)
            {
                return this.Equals(wch);
            }
            return false;
        }

        public bool Equals(IMultiByteChar other)
        {
            if (other is wchar_t wch)
            {
                return this.Equals(wch);
            }
            return false;
        }

        public bool Equals(wchar_t other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            int hashCode = -355065691;
            int @char = 0;

            unsafe
            {
                fixed (byte* b = this.@char)
                {
                    byte* bOffset;

                    for (int i = _WcharTSize - 1; i >= 0; i--)
                    {
                        bOffset = b + i;
                        @char = @char | (*bOffset << (i * 8));
                    }
                }
            }

            hashCode = hashCode * -1521134295 + @char;
            return hashCode;
        }
    }
}
