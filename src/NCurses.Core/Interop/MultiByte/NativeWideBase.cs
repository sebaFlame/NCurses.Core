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
    public abstract class NativeWideBase<TWide, TWideStr, TSMall, TSmallStr, TMouseEvent> // handles cchar_t
        where TWide : unmanaged, INCursesWCHAR, IEquatable<TWide>
        where TWideStr : unmanaged
        where TSMall : unmanaged, INCursesSCHAR, IEquatable<TSMall>
        where TSmallStr : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        private INCursesWrapperWide<TWide, TWideStr, TSMall, TSmallStr> wrapper;
        internal INCursesWrapperWide<TWide, TWideStr, TSMall, TSmallStr> Wrapper => 
            wrapper ?? (wrapper = NativeNCurses.NCursesCustomTypeWrapper as INCursesWrapperWide<TWide, TWideStr, TSMall, TSmallStr>);

        internal static ref readonly TWide MarshallArrayReadonly(in INCursesWCHAR wChar)
        {
            if (!(wChar is NCursesWCHAR<TWide> wCasted))
                throw new InvalidCastException("Wide character is in incorrect format");

            //Span<TWide> chSpan;
            //unsafe
            //{
            //    TWide* arr = stackalloc TWide[1];
            //    chSpan = new Span<TWide>(arr, 1);
            //}

            //chSpan[0] = wCasted.GetCharReference();
            //ReadOnlySpan<TWide> span = chSpan;
            //return ref span.GetPinnableReference();
            return ref wCasted.WCHAR.Span.GetPinnableReference();
        }

        internal static ref readonly TWide MarshallArrayReadonly(in INCursesWCHARStr wCharStr)
        {
            if (!(wCharStr is NCursesWCHARStr<TWide> wCasted))
                throw new InvalidCastException("Wide character is in incorrect format");

            ReadOnlySpan<TWide> span = wCasted.WCHAR.Span;
            return ref span.GetPinnableReference();
        }

        internal static ref TWide MarshallArray(ref INCursesWCHAR wChar)
        {
            if (!(wChar is NCursesWCHAR<TWide> wCasted))
                throw new InvalidCastException("Wide character is in incorrect format");

            //Span<TWide> chSpan;
            //unsafe
            //{
            //    TWide* arr = stackalloc TWide[1];
            //    chSpan = new Span<TWide>(arr, 1);
            //}

            //chSpan[0] = wCasted.GetCharReference();
            //return ref chSpan.GetPinnableReference();
            return ref wCasted.WCHAR.Span.GetPinnableReference();
        }

        internal static ref TWide MarshallArray(ref INCursesWCHARStr wCharStr)
        {
            if (!(wCharStr is NCursesWCHARStr<TWide> wCasted))
                throw new InvalidCastException("Wide character is in incorrect format");

            return ref wCasted.WCHAR.Span.GetPinnableReference();
        }
    }
}
