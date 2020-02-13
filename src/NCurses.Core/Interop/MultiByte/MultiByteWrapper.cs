using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.SingleByte;

using wchar_t = System.Char;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.MultiByte
{
    internal abstract class MultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> // handles cchar_t
        : INativeCharWrapper<IMultiByteChar, TMultiByte, IMultiByteCharString, MultiByteCharString<TMultiByte>>
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TChar : unmanaged, IChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> Wrapper { get; }

        internal MultiByteWrapper(IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> wrapper)
        {
            this.Wrapper = wrapper;
        }

        public MultiByteCharString<TMultiByte> CastString(in IMultiByteCharString wCharStr)
        {
            if (!(wCharStr is MultiByteCharString<TMultiByte> wCasted))
            {
                throw new InvalidCastException("MultiByte character is in incorrect format");
            }

            return wCasted;
        }

        public TMultiByte CastChar(in IMultiByteChar wChar)
        {
            if (!(wChar is TMultiByte wCasted))
            {
                throw new InvalidCastException("MultiByte character is in incorrect format");
            }

            return wCasted;
        }
    }
}
