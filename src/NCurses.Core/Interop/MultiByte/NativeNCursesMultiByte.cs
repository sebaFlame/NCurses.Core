using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.MultiByteString;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.MultiByte
{
    internal interface INativeNCursesMultiByte
    {
        void getcchar(in IMultiByteChar wcval, out char wch, out ulong attrs, out short color_pair);
        void setcchar(out IMultiByteChar wcval, in char wch, ulong attrs, short color_pair);
        void wunctrl(in IMultiByteChar wch, out string str);
    }

    internal class NativeNCursesMultiByte<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent> : MultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent>, INativeNCursesMultiByte
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TMultiByteString : unmanaged
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public NativeNCursesMultiByte()
        { }

        public unsafe void getcchar(in IMultiByteChar wcval, out char wch, out ulong attrs, out short color_pair)
        {
            //wideChar reference
            ref readonly TMultiByte wcValRef = ref MarshallArrayReadonly(wcval);

            //array to save new character
            TMultiByteString* strPtr = stackalloc TMultiByteString[1];
            ref TMultiByteString ch = ref MultiByteStringWrapper<TMultiByteString, TSingleByteString>.MarshalString(strPtr, 1, out Span<TMultiByteString> chSpan);

            //INCursesSCHAR to save attributes
            ISingleByteChar attrChar = new SingleByteChar<TSingleByte>(0);
            ref TSingleByte attrsRef = ref SingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent>.MarshallArray(ref attrChar);

            //ref for color_pair
            byte[] pairArr = new byte[Marshal.SizeOf<short>()];
            Span<short> spanColorPair = MemoryMarshal.Cast<byte, short>(pairArr);
            ref short pair = ref spanColorPair.GetPinnableReference();

            NCursesException.Verify(this.Wrapper.getcchar(wcValRef, ref ch, ref attrsRef, ref pair, IntPtr.Zero), "getcchar");

            wch = MultiByteStringWrapper<TMultiByteString, TSingleByteString>.ReadChar(chSpan);
            attrs = attrChar.Attributes;
            color_pair = spanColorPair[0];
        }

        public unsafe void setcchar(out IMultiByteChar wcval, in char wch, ulong attrs, short color_pair)
        {
            wcval = new MultiByteChar<TMultiByte>('\0');

            int byteLength;
            byte* byteArray = stackalloc byte[byteLength = MultiByteStringWrapper<TMultiByteString, TSingleByteString>.GetCharLength()];
            ref readonly TMultiByteString wideCh = ref MultiByteStringWrapper<TMultiByteString, TSingleByteString>.MarshalChar(wch, byteArray, byteLength);

            ISingleByteChar schar = new SingleByteChar<TSingleByte>(attrs);

            NCursesException.Verify(this.Wrapper.setcchar(
                ref MarshallArray(ref wcval), 
                wideCh, 
                SingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent>.MarshallArrayReadonly(schar), 
                color_pair, 
                IntPtr.Zero), "setcchar");
        }

        public void wunctrl(in IMultiByteChar wch, out string str)
        {
            str = MultiByteStringWrapper<TMultiByteString, TSingleByteString>.ReadString(ref this.Wrapper.wunctrl(MarshallArrayReadonly(wch)));
        }
    }
}
