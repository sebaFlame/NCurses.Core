using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.SingleByte
{
    internal class NativeScreenSingleByte<TSingleByte, TChar, TMouseEvent> 
            : SingleByteWrapper<TSingleByte, TChar, TMouseEvent>, 
            INativeScreenSingleByte<TSingleByte, SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeScreenSingleByte(ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> wrapper)
            : base(wrapper) { }

        public void getmouse_sp(IntPtr screen, out IMEVENT ev)
        {
            TMouseEvent mouseEvent = default;

            NCursesException.Verify(this.Wrapper.getmouse_sp(screen, ref mouseEvent), "getmouse_sp");
            ev = mouseEvent;
        }

        public ulong mousemask_sp(IntPtr screen, ulong newmask, out ulong oldmask)
        {
            TSingleByte newMouseMask = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(newmask);
            TSingleByte oldMouseMask = default;

            TSingleByte res = this.Wrapper.mousemask_sp(screen, newMouseMask, ref oldMouseMask);

            oldmask = ToPrimitiveType<ulong>(in oldMouseMask);
            return ToPrimitiveType<ulong>(in res);
        }

        public ulong slk_attr_sp(IntPtr screen)
        {
            TSingleByte val = this.Wrapper.slk_attr_sp(screen);
            return val.Attributes;
        }

        public void slk_attr_set_sp(IntPtr screen, ulong attrs, short color_pair)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_set_sp(screen, ch, color_pair, IntPtr.Zero), "slk_attr_set_sp");
        }

        public void slk_attroff_sp(IntPtr screen, ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attroff_sp(screen, ch), "slk_attroff_sp");
        }

        public void slk_attron_sp(IntPtr screen, ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attron_sp(screen, ch), "slk_attron_sp");
        }

        public void slk_attrset_sp(IntPtr screen, ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.slk_attrset_sp(screen, ch), "slk_attrset_sp");
        }

        public ulong term_attrs_sp(IntPtr screen)
        {
            TSingleByte val = this.Wrapper.term_attrs_sp(screen);
            return val.Attributes;
        }

        public ulong termattrs_sp(IntPtr screen)
        {
            TSingleByte val = this.Wrapper.termattrs_sp(screen);
            return val.Attributes;
        }

        public void ungetmouse_sp(IntPtr screen, in IMEVENT ev)
        {
            if (!(ev is TMouseEvent castedEv))
            {
                throw new InvalidCastException("Mouse event not of the correct type");
            }
                
            NCursesException.Verify(this.Wrapper.ungetmouse_sp(screen, in castedEv), "ungetmouse_sp");
        }

        public void vid_attr_sp(IntPtr screen, ulong attrs, short pair)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.vid_attr_sp(screen, ch, pair, IntPtr.Zero), "vid_attr_sp");
        }

        public void vid_puts_sp(IntPtr screen, ulong attrs, short pair, Func<int, int> putc)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            IntPtr func = Marshal.GetFunctionPointerForDelegate(putc);
            try
            {
                NCursesException.Verify(this.Wrapper.vid_puts_sp(screen, ch, pair, IntPtr.Zero, func), "vid_puts_sp");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }

        public void vidattr_sp(IntPtr screen, ulong attrs)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.vidattr_sp(screen, ch), "vidattr_sp");
        }

        public void vidputs_sp(IntPtr screen, ulong attrs, Func<int, int> putc)
        {
            TSingleByte ch = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            IntPtr func = Marshal.GetFunctionPointerForDelegate(putc);
            try
            {
                NCursesException.Verify(this.Wrapper.vidputs_sp(screen, ch, func), "vidputs_sp");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }
    }
}
