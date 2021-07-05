using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.MultiByte
{
    internal class NativeNCursesMultiByte<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> 
            : MultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>, 
            INativeNCursesMultiByte<TMultiByte, MultiByteCharString<TMultiByte, TWideChar, TSingleByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeNCursesMultiByte(IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> wrapper)
            : base(wrapper) { }

        public void getcchar(in TMultiByte wcval, out char wch, out ulong attrs, out ushort color_pair)
        {
            TWideChar ch = default;
            TSingleByte attrChar = default;

            short limitedPair = 0;
            //int extendedPair = 0;

            //TODO: replace IntPtr.Zero with a ref int when ubuntu switches to ncurses6.2+20210116
            NCursesException.Verify(this.Wrapper.getcchar(in wcval, ref ch, ref attrChar, ref limitedPair, IntPtr.Zero), "getcchar");

            wch = (char)WideCharFactory<TWideChar>._Instance.GetChar(ch);
            attrs = attrChar.Attributes;
            color_pair = (ushort)limitedPair;
        }

        public void setcchar(out TMultiByte wcval, in char wch, ulong attrs, ushort color_pair)
        {
            TMultiByte output = default;

            TWideChar wideCh = WideCharFactory<TWideChar>._Instance.GetNativeChar(wch);

            TSingleByte attrChar = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);

            int extendedColor = color_pair;

            NCursesException.Verify(this.Wrapper.setcchar(
                ref output, 
                wideCh, 
                attrChar, 
                (short)color_pair,
                ref extendedColor), "setcchar");

            wcval = output;
        }

        public void wunctrl(in TMultiByte wch, out string str)
        {
            ref TWideChar strRef = ref this.Wrapper.wunctrl(CastChar(wch));

            using (BufferState<TWideChar> bufferState = WideCharFactory<TWideChar>._Instance.GetNativeString(WideCharFactory<TWideChar>._CreatePooledBuffer, ref strRef, out WideCharString<TWideChar> wideStr))
            {
                str = wideStr.ToString();
            }
        }
    }
}
