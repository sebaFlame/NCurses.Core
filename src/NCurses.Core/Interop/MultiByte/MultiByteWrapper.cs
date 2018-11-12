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
    public abstract class MultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent> // handles cchar_t
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TMultiByteString : unmanaged
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        private IMultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString> wrapper;
        internal IMultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString> Wrapper => 
            wrapper ?? (wrapper = NativeNCurses.NCursesCustomTypeWrapper as IMultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString>);

        internal static ref readonly TMultiByte MarshallArrayReadonly(in IMultiByteChar wChar)
        {
            if (!(wChar is MultiByteChar<TMultiByte> wCasted))
                throw new InvalidCastException("MultiByte character is in incorrect format");

            //Span<TMultiByte> chSpan;
            //unsafe
            //{
            //    TMultiByte* arr = stackalloc TMultiByte[1];
            //    chSpan = new Span<TMultiByte>(arr, 1);
            //}

            //chSpan[0] = wCasted.GetCharReference();
            //ReadOnlySpan<TMultiByte> span = chSpan;
            //return ref span.GetPinnableReference();
            return ref wCasted.Char.Span.GetPinnableReference();
        }

        internal static ref readonly TMultiByte MarshallArrayReadonly(in IMultiByteCharString wCharStr)
        {
            if (!(wCharStr is MultiByteCharString<TMultiByte> wCasted))
                throw new InvalidCastException("MultiByte character is in incorrect format");

            ReadOnlySpan<TMultiByte> span = wCasted.CharString.Span;
            return ref span.GetPinnableReference();
        }

        internal static ref TMultiByte MarshallArray(ref IMultiByteChar wChar)
        {
            if (!(wChar is MultiByteChar<TMultiByte> wCasted))
                throw new InvalidCastException("MultiByte character is in incorrect format");

            //Span<TMultiByte> chSpan;
            //unsafe
            //{
            //    TMultiByte* arr = stackalloc TMultiByte[1];
            //    chSpan = new Span<TMultiByte>(arr, 1);
            //}

            //chSpan[0] = wCasted.GetCharReference();
            //return ref chSpan.GetPinnableReference();
            return ref wCasted.Char.Span.GetPinnableReference();
        }

        internal static ref TMultiByte MarshallArray(ref IMultiByteCharString wCharStr)
        {
            if (!(wCharStr is MultiByteCharString<TMultiByte> wCasted))
                throw new InvalidCastException("MultiByte character is in incorrect format");

            return ref wCasted.CharString.Span.GetPinnableReference();
        }
    }
}
