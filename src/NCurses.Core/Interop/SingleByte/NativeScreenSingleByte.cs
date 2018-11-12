using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.SingleByte
{
    internal interface INativeScreenSingleByte
    {
        void getmouse_sp(IntPtr screen, out IMEVENT ev);
        ulong mousemask_sp(IntPtr screen, ulong newmask, out ulong oldmask);
        ulong slk_attr_sp(IntPtr screen);
        void slk_attr_set_sp(IntPtr screen, ulong attrs, short color_pair);
        void slk_attroff_sp(IntPtr screen, ulong attrs);
        void slk_attron_sp(IntPtr screen, ulong attrs);
        void slk_attrset_sp(IntPtr screen, ulong attrs);
        ulong term_attrs_sp(IntPtr screen);
        ulong termattrs_sp(IntPtr screen);
        void ungetmouse_sp(IntPtr screen, in IMEVENT ev);

        void vid_attr_sp(IntPtr screen, ulong attrs, short pair);
        void vid_puts_sp(IntPtr screen, ulong attrs, short pair, Func<int, int> putc);
        void vidattr_sp(IntPtr screen, ulong attrs);
        void vidputs_sp(IntPtr screen, ulong attrs, Func<int, int> putc);
    }

    internal class NativeScreenSingleByte<TSingleByte, TSingleByteString, TMouseEvent> : SingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent>, INativeScreenSingleByte
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public NativeScreenSingleByte()
        { }

        public void getmouse_sp(IntPtr screen, out IMEVENT ev)
        {
            int size = Marshal.SizeOf<TMouseEvent>();
            Span<byte> span;

            unsafe
            {
                byte* arr = stackalloc byte[size];
                span = new Span<byte>(arr, size);

                Span<TMouseEvent> spanEv = MemoryMarshal.Cast<byte, TMouseEvent>(span);
                ref TMouseEvent spanRef = ref spanEv.GetPinnableReference();

                NCursesException.Verify(this.Wrapper.getmouse_sp(screen, ref spanRef), "getmouse_sp");
                ev = spanEv[0];
            }
        }

        public ulong mousemask_sp(IntPtr screen, ulong newmask, out ulong oldmask)
        {
            ISingleByteChar newMouseMask = new SingleByteChar<TSingleByte>(newmask);
            ISingleByteChar oldMouseMask = new SingleByteChar<TSingleByte>(0);

            TSingleByte res = this.Wrapper.mousemask_sp(screen, MarshallArrayReadonly(newMouseMask), ref MarshallArray(ref oldMouseMask));

            ISingleByteChar resMouseMask = new SingleByteChar<TSingleByte>(ref res);
            NCursesException.Verify(resMouseMask.Attributes == 0 ? -1 : 0, "mousemask_sp");

            oldmask = oldMouseMask.Attributes;
            return resMouseMask.Attributes;
        }

        public ulong slk_attr_sp(IntPtr screen)
        {
            TSingleByte val = this.Wrapper.slk_attr_sp(screen);
            ISingleByteChar schar = new SingleByteChar<TSingleByte>(ref val);
            return schar.Attributes;
        }

        public void slk_attr_set_sp(IntPtr screen, ulong attrs, short color_pair)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_set_sp(screen, MarshallArrayReadonly(ch), color_pair, IntPtr.Zero), "slk_attr_set_sp");
        }

        public void slk_attroff_sp(IntPtr screen, ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attroff_sp(screen, MarshallArrayReadonly(ch)), "slk_attroff_sp");
        }

        public void slk_attron_sp(IntPtr screen, ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attron_sp(screen, MarshallArrayReadonly(ch)), "slk_attron_sp");
        }

        public void slk_attrset_sp(IntPtr screen, ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attrset_sp(screen, MarshallArrayReadonly(ch)), "slk_attrset_sp");
        }

        public ulong term_attrs_sp(IntPtr screen)
        {
            TSingleByte val = this.Wrapper.term_attrs_sp(screen);
            ISingleByteChar schar = new SingleByteChar<TSingleByte>(ref val);
            return schar.Attributes;
        }

        public ulong termattrs_sp(IntPtr screen)
        {
            TSingleByte val = this.Wrapper.termattrs_sp(screen);
            ISingleByteChar schar = new SingleByteChar<TSingleByte>(ref val);
            return schar.Attributes;
        }

        public void ungetmouse_sp(IntPtr screen, in IMEVENT ev)
        {
            if (!(ev is TMouseEvent castedEv))
                throw new InvalidCastException("Mouse event not of the correct type");

            int size = Marshal.SizeOf<TMouseEvent>();
            Span<byte> span;

            unsafe
            {
                byte* arr = stackalloc byte[size];
                span = new Span<byte>(arr, size);

                Span<TMouseEvent> spanEv = MemoryMarshal.Cast<byte, TMouseEvent>(span);
                spanEv[0] = castedEv;

                NCursesException.Verify(this.Wrapper.ungetmouse_sp(screen, spanEv.GetPinnableReference()), "ungetmouse_sp");
            }
        }

        public void vid_attr_sp(IntPtr screen, ulong attrs, short pair)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.vid_attr_sp(screen, MarshallArrayReadonly(ch), pair, IntPtr.Zero), "vid_attr_sp");
        }

        public void vid_puts_sp(IntPtr screen, ulong attrs, short pair, Func<int, int> putc)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            IntPtr func = Marshal.GetFunctionPointerForDelegate(putc);
            try
            {
                NCursesException.Verify(this.Wrapper.vid_puts_sp(screen, MarshallArrayReadonly(ch), pair, IntPtr.Zero, func), "vid_puts_sp");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }

        public void vidattr_sp(IntPtr screen, ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.vidattr_sp(screen, MarshallArrayReadonly(ch)), "vidattr_sp");
        }

        public void vidputs_sp(IntPtr screen, ulong attrs, Func<int, int> putc)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            IntPtr func = Marshal.GetFunctionPointerForDelegate(putc);
            try
            {
                NCursesException.Verify(this.Wrapper.vidputs_sp(screen, MarshallArrayReadonly(ch), func), "vidputs_sp");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }
    }
}
