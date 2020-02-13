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
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TChar : unmanaged, IChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeNCursesMultiByte(IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> wrapper)
            : base(wrapper) { }

        public unsafe void getcchar(in TMultiByte wcval, out char wch, out ulong attrs, out short color_pair)
        {
            TWideChar ch = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyCharInternal();

            TSingleByte attrChar = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();

            color_pair = 0;

            NCursesException.Verify(this.Wrapper.getcchar(in wcval, ref ch, ref attrChar, ref color_pair, IntPtr.Zero), "getcchar");

            wch = ch.Char;
            attrs = attrChar.Attributes;
        }

        public unsafe void setcchar(out TMultiByte wcval, in char wch, ulong attrs, short color_pair)
        {
            TMultiByte output = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();

            TWideChar wideCh = WideCharFactoryInternal<TWideChar>.Instance.GetNativeCharInternal(wch);

            TSingleByte attrChar = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);

            NCursesException.Verify(this.Wrapper.setcchar(
                ref output, 
                wideCh, 
                attrChar, 
                color_pair, 
                IntPtr.Zero), "setcchar");

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
