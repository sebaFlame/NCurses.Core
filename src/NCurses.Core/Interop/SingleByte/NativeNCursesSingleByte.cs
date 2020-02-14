using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.Char;

namespace NCurses.Core.Interop.SingleByte
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
            TSingleByte newMouseMask = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(newmask);
            TSingleByte oldMouseMask = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();

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
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_off(ch, IntPtr.Zero), "slk_attr_off");
        }

        public void slk_attr_on(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_on(ch, IntPtr.Zero), "slk_attr_on");
        }

        public void slk_attr_set(ulong attrs, short color_pair)
        {
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_set(ch, color_pair, IntPtr.Zero), "slk_attr_set");
        }

        public void slk_attroff(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.slk_attroff(ch), "slk_attroff");
        }

        public void slk_attron(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.slk_attron(ch), "slk_attron");
        }

        public void slk_attrset(ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
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
            return CharFactoryInternal<TChar>.Instance.CreateNativeString(ref this.Wrapper.unctrl(sch)).ToString();
        }

        public void ungetmouse(in TMouseEvent ev)
        {
            NCursesException.Verify(this.Wrapper.ungetmouse(in ev), "ungetmouse");
        }

        public void vid_attr(ulong attrs, short pair)
        {
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.vid_attr(ch, pair, IntPtr.Zero), "vid_attr");
        }

        public void vid_puts(ulong attrs, short pair, Func<int, int> putc)
        {
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
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
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.vidattr(ch), "vidattr");
        }

        public void vidputs(ulong attrs, Func<int, int> putc)
        {
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
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
