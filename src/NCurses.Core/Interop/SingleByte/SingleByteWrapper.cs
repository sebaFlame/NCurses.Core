﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    internal abstract class SingleByteWrapper<TSingleByte, TChar, TMouseEvent> //handles chtype and attr_t
        : INativeCharWrapper<ISingleByteChar, TSingleByte, ISingleByteCharString, SingleByteCharString<TSingleByte>>
            where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
            where TChar : unmanaged, IChar, IEquatable<TChar>
            where TMouseEvent : unmanaged, IMEVENT
    {
        internal ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> Wrapper { get; }

        internal SingleByteWrapper(ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> wrapper)
        {
            this.Wrapper = wrapper;
        }

        public SingleByteCharString<TSingleByte> CastString(in ISingleByteCharString wCharStr)
        {
            if (!(wCharStr is SingleByteCharString<TSingleByte> wCasted))
            {
                throw new InvalidCastException("Character is in incorrect format");
            }

            return wCasted;
        }

        public TSingleByte CastChar(in ISingleByteChar wChar)
        {
            if (!(wChar is TSingleByte wCasted))
            {
                throw new InvalidCastException("Character is in incorrect format");
            }

            return wCasted;
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
