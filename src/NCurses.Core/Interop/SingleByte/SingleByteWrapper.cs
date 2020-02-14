using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    internal abstract class SingleByteWrapper<TSingleByte, TChar, TMouseEvent> //handles chtype and attr_t
        : INativeCharWrapper<ISingleByteNCursesChar, TSingleByte, ISingleByteNCursesCharString, SingleByteCharString<TSingleByte>>
            where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
            where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
            where TMouseEvent : unmanaged, IMEVENT
    {
        internal ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> Wrapper { get; }

        internal SingleByteWrapper(ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> wrapper)
        {
            this.Wrapper = wrapper;
        }

        public SingleByteCharString<TSingleByte> CastString(in ISingleByteNCursesCharString charStr)
        {
            if (!(charStr is SingleByteCharString<TSingleByte> wCasted))
            {
                throw new InvalidCastException("Character is in incorrect format");
            }

            return wCasted;
        }

        public TSingleByte CastChar(in ISingleByteNCursesChar ch)
        {
            if (!(ch is TSingleByte chCasted))
            {
                throw new InvalidCastException("Character is in incorrect format");
            }

            return chCasted;
        }

        public TMouseEvent CastMouseEvent(in IMEVENT mEvent)
        {
            if (!(mEvent is TMouseEvent casted))
            {
                throw new InvalidCastException("Mouse event is in incorrect format");
            }

            return casted;
        }

        public unsafe static T ToPrimitiveType<T>(in TSingleByte wch)
            where T : unmanaged
        {
            TSingleByte* arr = stackalloc TSingleByte[1];
            arr[0] = wch;

            return Unsafe.Read<T>(arr);
        }
    }
}
