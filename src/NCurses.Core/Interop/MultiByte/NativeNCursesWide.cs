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
    internal interface INativeNCursesWide
    {
        void getcchar(in INCursesWCHAR wcval, out char wch, out ulong attrs, out short color_pair);
        void setcchar(out INCursesWCHAR wcval, in char wch, ulong attrs, short color_pair);
        void wunctrl(in INCursesWCHAR wch, out string str);
    }

    internal class NativeNCursesWide<TWide, TWideStr, TSmall, TSmallStr, TMouseEvent> : NativeWideBase<TWide, TWideStr, TSmall, TSmallStr, TMouseEvent>, INativeNCursesWide
        where TWide : unmanaged, INCursesWCHAR, IEquatable<TWide>
        where TWideStr : unmanaged
        where TSmall : unmanaged, INCursesSCHAR, IEquatable<TSmall>
        where TSmallStr : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public NativeNCursesWide()
        { }

        public unsafe void getcchar(in INCursesWCHAR wcval, out char wch, out ulong attrs, out short color_pair)
        {
            //wideChar reference
            ref readonly TWide wcValRef = ref MarshallArrayReadonly(wcval);

            //array to save new character
            TWideStr* strPtr = stackalloc TWideStr[1];
            ref TWideStr ch = ref NativeWideStrBase<TWideStr, TSmallStr>.MarshalString(strPtr, 1, out Span<TWideStr> chSpan);

            //INCursesSCHAR to save attributes
            INCursesSCHAR attrChar = new NCursesSCHAR<TSmall>(0);
            ref TSmall attrsRef = ref NativeSmallBase<TSmall, TSmallStr, TMouseEvent>.MarshallArray(ref attrChar);

            //ref for color_pair
            byte[] pairArr = new byte[Marshal.SizeOf<short>()];
            Span<short> spanColorPair = MemoryMarshal.Cast<byte, short>(pairArr);
            ref short pair = ref spanColorPair.GetPinnableReference();

            NCursesException.Verify(this.Wrapper.getcchar(wcValRef, ref ch, ref attrsRef, ref pair, IntPtr.Zero), "getcchar");

            wch = NativeWideStrBase<TWideStr, TSmallStr>.ReadChar(chSpan);
            attrs = attrChar.Attributes;
            color_pair = spanColorPair[0];
        }

        public unsafe void setcchar(out INCursesWCHAR wcval, in char wch, ulong attrs, short color_pair)
        {
            wcval = new NCursesWCHAR<TWide>('\0');

            int byteLength;
            byte* byteArray = stackalloc byte[byteLength = NativeWideStrBase<TWideStr, TSmallStr>.GetCharLength()];
            ref readonly TWideStr wideCh = ref NativeWideStrBase<TWideStr, TSmallStr>.MarshalChar(wch, byteArray, byteLength);

            INCursesSCHAR schar = new NCursesSCHAR<TSmall>(attrs);

            NCursesException.Verify(this.Wrapper.setcchar(
                ref MarshallArray(ref wcval), 
                wideCh, 
                NativeSmallBase<TSmall, TSmallStr, TMouseEvent>.MarshallArrayReadonly(schar), 
                color_pair, 
                IntPtr.Zero), "setcchar");
        }

        public void wunctrl(in INCursesWCHAR wch, out string str)
        {
            str = NativeWideStrBase<TWideStr, TSmallStr>.ReadString(ref this.Wrapper.wunctrl(MarshallArrayReadonly(wch)));
        }
    }
}
