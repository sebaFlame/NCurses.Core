using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using NCurses.Core.Interop.MultiByte;

namespace NCurses.Core.Interop.Dynamic
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct cchar_t : IMultiByteNCursesChar, IEquatable<cchar_t> //cchar_t
    {
        private const int _WcharTSize = 2;
        private const int _CharGlobalLength = _WcharTSize * Constants.CCHARW_MAX;

        public chtype attr;
        public unsafe fixed byte chars[_CharGlobalLength];
        public int ext_color;

        public ushort ColorPair => ext_color == 0 ? attr.ColorPair : (ushort)ext_color;
        public ulong Attributes => (ulong)(this.attr ^ (this.attr & Attrs.COLOR));

        public int Char
        {
            get
            {
                int ret = 0;

                unsafe
                {
                    int* retPtr = &ret;

                    fixed (byte* b = chars)
                    {
                        Unsafe.CopyBlock(retPtr, b, _WcharTSize);
                    }
                }

                return ret;
            }
        }

        public cchar_t(ArraySegment<byte> encodedBytesChar)
        {
            if (encodedBytesChar.Count > _WcharTSize)
            {
                throw new ArgumentOutOfRangeException($"Wide char can only contain {_WcharTSize} bytes");
            }

            unsafe
            {
                fixed (byte* bArr = chars)
                {
                    Span<byte> @char = new Span<byte>(bArr, _WcharTSize);
                    ((Span<byte>)encodedBytesChar).CopyTo(@char);
                }
            }

            this.attr = 0;
            this.ext_color = 0;
        }

        public cchar_t(ArraySegment<byte> encodedBytesChar, ulong attrs)
            : this(encodedBytesChar)
        {
            this.attr = attrs;
        }

        public cchar_t(ArraySegment<byte> encodedBytesChar, ulong attrs, ushort pair)
            : this(encodedBytesChar, attrs)
        {
            this.ext_color = pair;
            this.attr |= (ulong)NativeNCurses.COLOR_PAIR(pair);
        }

        public cchar_t(Span<byte> encodedBytesChar)
        {
            if (encodedBytesChar.Length > _WcharTSize)
            {
                throw new ArgumentOutOfRangeException($"Wide char can only contain {_WcharTSize} bytes");
            }

            unsafe
            {
                fixed (byte* bArr = chars)
                {
                    Span<byte> @char = new Span<byte>(bArr, _WcharTSize);
                    encodedBytesChar.CopyTo(@char);
                }
            }

            this.attr = 0;
            this.ext_color = 0;
        }

        public cchar_t(Span<byte> encodedBytesChar, ulong attrs)
            : this(encodedBytesChar)
        {
            this.attr = attrs;
        }

        public cchar_t(Span<byte> encodedBytesChar, ulong attrs, ushort pair)
            : this(encodedBytesChar, attrs)
        {
            this.ext_color = pair;
            this.attr |= (ulong)NativeNCurses.COLOR_PAIR(pair);
        }

        /* TODO
        * ReadOnlySpan.SequenceEqual returns false on linux (using netcoreapp2.0 or netcoreapp2.1)
        */
        public static bool operator ==(in cchar_t wchLeft, in cchar_t wchRight)
        {
            if( wchLeft.attr != wchRight.attr
                || wchLeft.ext_color != wchRight.ext_color)
            {
                return false;
            }

            unsafe
            {
                fixed (byte* l = wchLeft.chars, r = wchRight.chars)
                {
                    Span<byte> leftSpan = new Span<byte>(l, _WcharTSize);
                    Span<byte> rightSpan = new Span<byte>(r, _WcharTSize);

                    return leftSpan.SequenceEqual(rightSpan);
                }
            }
        }


        public static bool operator !=(in cchar_t wchLeft, in cchar_t wchRight)
        {
            if (wchLeft.attr != wchRight.attr
                || wchLeft.ext_color != wchRight.ext_color)
            {
                return true;
            }

            unsafe
            {
                fixed (byte* l = wchLeft.chars, r = wchRight.chars)
                {
                    Span<byte> leftSpan = new Span<byte>(l, _WcharTSize);
                    Span<byte> rightSpan = new Span<byte>(r, _WcharTSize);

                    return !leftSpan.SequenceEqual(rightSpan);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IChar obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IMultiByteChar obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(INCursesChar obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(IMultiByteNCursesChar obj)
        {
            if (obj is cchar_t other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(cchar_t other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            int hashCode = -946517152;
            hashCode = hashCode * -1521134295 + this.attr.GetHashCode();

            int @char = 0;

            unsafe
            {
                fixed (byte* b = this.chars)
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
            hashCode = hashCode * -1521134295 + this.ext_color.GetHashCode();

            return hashCode;
        }
    }
}
