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
            INativeNCursesMultiByte<TMultiByte, MultiByteCharString<TMultiByte>>
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
            TWideChar ch = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyCharInternal();

            TSingleByte attrChar = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();

            short limitedPair = 0;
            //int extendedPair = 0;

            //TODO: replace IntPtr.Zero with a ref int when ubuntu switches to ncurses6.2+20210116
            NCursesException.Verify(this.Wrapper.getcchar(in wcval, ref ch, ref attrChar, ref limitedPair, IntPtr.Zero), "getcchar");

            wch = ch.Char;
            attrs = attrChar.Attributes;
            //color_pair = extendedPair == 0 ? (ushort)limitedPair : (ushort)extendedPair;
            color_pair = (ushort)limitedPair;
        }

        public void setcchar(out TMultiByte wcval, in char wch, ulong attrs, ushort color_pair)
        {
            TMultiByte output = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();

            TWideChar wideCh = WideCharFactoryInternal<TWideChar>.Instance.GetNativeCharInternal(wch);

            TSingleByte attrChar = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);

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

            WideCharString<TWideChar> wideStr = WideCharFactoryInternal<TWideChar>.Instance.CreateNativeString(ref strRef);

            str = wideStr.ToString();
        }
    }
}
