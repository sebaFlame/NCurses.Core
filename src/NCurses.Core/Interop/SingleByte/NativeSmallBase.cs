using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using schar = System.SByte;

namespace NCurses.Core.Interop.SingleByte
{
    public abstract class NativeSmallBase<TSmall, TSmallStr> //handles chtype and attr_t
        where TSmall : unmanaged, INCursesSCHAR
        where TSmallStr : unmanaged
    {
        private INCursesWrapperSmall<TSmall, TSmallStr> wrapper;
        internal INCursesWrapperSmall<TSmall, TSmallStr> Wrapper => 
            wrapper ?? (wrapper = NativeNCurses.NCursesCustomTypeWrapper as INCursesWrapperSmall<TSmall, TSmallStr>);

        internal static ref readonly TSmall MarshallArrayReadonly(in INCursesSCHAR sChar)
        {
            if (!(sChar is NCursesSCHAR<TSmall> sCasted))
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
            return ref sCasted.SCHAR.Span.GetPinnableReference();
        }

        internal static ref readonly TSmall MarshallArrayReadonly(in INCursesSCHARStr sCharStr)
        {
            if (!(sCharStr is NCursesSCHARStr<TSmall> sCasted))
                throw new InvalidCastException("Small character is in incorrect format");

            //ReadOnlySpan<TSmall> span = new ReadOnlySpan<TSmall>(sCasted.SCHAR);
            ReadOnlySpan<TSmall> span = sCasted.SCHAR.Span;
            return ref span.GetPinnableReference();
        }

        internal static ref TSmall MarshallArray(ref INCursesSCHAR sChar)
        {
            if (!(sChar is NCursesSCHAR<TSmall> sCasted))
                throw new InvalidCastException("Small character is in incorrect format");

            //Span<TSmall> span;
            //unsafe
            //{
            //    TSmall* arr = stackalloc TSmall[1];
            //    span = new Span<TSmall>(arr, 1);
            //}

            //span[0] = sCasted.GetCharReference();
            //return ref span.GetPinnableReference();

            return ref sCasted.SCHAR.Span.GetPinnableReference();
        }

        internal static ref TSmall MarshallArray(ref INCursesSCHARStr sCharStr)
        {
            if (!(sCharStr is NCursesSCHARStr<TSmall> sCasted))
                throw new InvalidCastException("Small character is in incorrect format");

            //Span<TSmall> span = new Span<TSmall>(sCasted.SCHAR);
            Span<TSmall> span = sCasted.SCHAR.Span;
            return ref span.GetPinnableReference();
        }
    }
}
