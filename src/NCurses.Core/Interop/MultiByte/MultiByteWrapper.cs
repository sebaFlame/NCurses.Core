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
        : INativeCharWrapper<IMultiByteNCursesChar, TMultiByte, IMultiByteNCursesCharString, MultiByteCharString<TMultiByte, TWideChar, TSingleByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> Wrapper { get; }

        internal MultiByteWrapper(IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> wrapper)
        {
            this.Wrapper = wrapper;
        }

        public MultiByteCharString<TMultiByte, TWideChar, TSingleByte> CastString(in IMultiByteNCursesCharString wCharStr)
        {
            if (!(wCharStr is MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wCasted))
            {
                throw new InvalidCastException("MultiByte character is in incorrect format");
            }

            return wCasted;
        }

        public TMultiByte CastChar(in IMultiByteNCursesChar wChar)
        {
            if (!(wChar is TMultiByte wCasted))
            {
                throw new InvalidCastException("MultiByte character is in incorrect format");
            }

            return wCasted;
        }
    }
}
