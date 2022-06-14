using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.Char;

namespace NippyWard.NCurses.Interop.SingleByte
{
    internal class NativeNCursesSingleByte<TSingleByte, TChar, TMouseEvent> 
            : SingleByteWrapper<TSingleByte, TChar, TMouseEvent>, 
            INativeNCursesSingleByte<TSingleByte, SingleByteCharString<TSingleByte>, TMouseEvent>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeNCursesSingleByte(ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> wrapper)
            : base(wrapper) { }

        public void getmouse(out TMouseEvent ev)
        {
            ev = default;
            NCursesException.Verify(this.Wrapper.getmouse(ref ev), "getmouse");
        }

        public ulong mousemask(ulong newmask, out ulong oldmask)
        {
            TSingleByte newMouseMask = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(newmask);
            TSingleByte oldMouseMask = default;

            TSingleByte res = this.Wrapper.mousemask(newMouseMask, ref oldMouseMask);

            oldmask = ToPrimitiveType<ulong>(in oldMouseMask);
            return ToPrimitiveType<ulong>(in res);
        }

        public ulong slk_attr()
        {
            TSingleByte res = this.Wrapper.slk_attr();
            return res.Attributes;
        }

        public void slk_attr_off(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_off(ch, IntPtr.Zero), "slk_attr_off");
        }

        public void slk_attr_on(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_on(ch, IntPtr.Zero), "slk_attr_on");
        }

        public void slk_attr_set(ulong attrs, short color_pair)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_set(ch, color_pair, IntPtr.Zero), "slk_attr_set");
        }

        public void slk_attroff(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attroff(ch), "slk_attroff");
        }

        public void slk_attron(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attron(ch), "slk_attron");
        }

        public void slk_attrset(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attrset(ch), "slk_attrset");
        }

        public ulong term_attrs()
        {
            TSingleByte val = this.Wrapper.term_attrs();
            return val.Attributes;
        }

        public ulong termattrs()
        {
            TSingleByte val = this.Wrapper.termattrs();
            return val.Attributes;
        }

        public string unctrl(in TSingleByte sch)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreatePooledBuffer,
                ref NCursesException.Verify(ref this.Wrapper.unctrl(sch), "unctrl"),
                out CharString<TChar> @string))
            {
                return @string.ToString();
            }
        }

        public void ungetmouse(in TMouseEvent ev)
        {
            NCursesException.Verify(this.Wrapper.ungetmouse(in ev), "ungetmouse");
        }

        public void vid_attr(ulong attrs, short pair)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.vid_attr(ch, pair, IntPtr.Zero), "vid_attr");
        }

        public void vid_puts(ulong attrs, short pair, Func<int, int> putc)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            IntPtr func = Marshal.GetFunctionPointerForDelegate(putc);
            try
            {
                NCursesException.Verify(this.Wrapper.vid_puts(ch, pair, IntPtr.Zero, func), "vid_puts");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }

        public void vidattr(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.vidattr(ch), "vidattr");
        }

        public void vidputs(ulong attrs, Func<int, int> putc)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            IntPtr func = Marshal.GetFunctionPointerForDelegate(putc);
            try
            {
                NCursesException.Verify(this.Wrapper.vidputs(ch, func), "vidputs");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }
    }
}
