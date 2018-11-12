using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    public abstract class SingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent> //handles chtype and attr_t
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        private ISingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent> wrapper;
        internal ISingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent> Wrapper => 
            wrapper ?? (wrapper = NativeNCurses.NCursesCustomTypeWrapper as ISingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent>);

        internal static ref readonly TSingleByte MarshallArrayReadonly(in ISingleByteChar sChar)
        {
            if (!(sChar is SingleByteChar<TSingleByte> sCasted))
                throw new InvalidCastException("Small character is in incorrect format");

            //Span<TSmall> chSpan;
            //unsafe
            //{
            //    TSmall* arr = stackalloc TSmall[1];
            //    chSpan = new Span<TSmall>(arr, 1);
            //}

            //chSpan[0] = sCasted.GetCharReference();
            //ReadOnlySpan<TSmall> span = chSpan;
            //return ref span.GetPinnableReference();
            return ref sCasted.Char.Span.GetPinnableReference();
        }

        internal static ref readonly TSingleByte MarshallArrayReadonly(in ISingleByteCharString sCharStr)
        {
            if (!(sCharStr is SingleByteCharString<TSingleByte> sCasted))
                throw new InvalidCastException("Small character is in incorrect format");

            //ReadOnlySpan<TSmall> span = new ReadOnlySpan<TSmall>(sCasted.SCHAR);
            ReadOnlySpan<TSingleByte> span = sCasted.CharString.Span;
            return ref span.GetPinnableReference();
        }

        internal static ref TSingleByte MarshallArray(ref ISingleByteChar sChar)
        {
            if (!(sChar is SingleByteChar<TSingleByte> sCasted))
                throw new InvalidCastException("Small character is in incorrect format");

            //Span<TSmall> span;
            //unsafe
            //{
            //    TSmall* arr = stackalloc TSmall[1];
            //    span = new Span<TSmall>(arr, 1);
            //}

            //span[0] = sCasted.GetCharReference();
            //return ref span.GetPinnableReference();

            return ref sCasted.Char.Span.GetPinnableReference();
        }

        internal static ref TSingleByte MarshallArray(ref ISingleByteCharString sCharStr)
        {
            if (!(sCharStr is SingleByteCharString<TSingleByte> sCasted))
                throw new InvalidCastException("Small character is in incorrect format");

            //Span<TSmall> span = new Span<TSmall>(sCasted.SCHAR);
            Span<TSingleByte> span = sCasted.CharString.Span;
            return ref span.GetPinnableReference();
        }
    }
}
