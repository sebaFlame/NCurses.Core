using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SingleByteString;

namespace NCurses.Core.Interop.SingleByte
{
    internal interface INativeNCursesSingleByte
    {
        void getmouse(out IMEVENT ev);
        ulong mousemask(ulong newmask, out ulong oldmask);
        ulong slk_attr();
        void slk_attr_off(ulong attrs);
        void slk_attr_on(ulong attrs);
        void slk_attr_set(ulong attrs, short color_pair);
        void slk_attroff(ulong attrs);
        void slk_attron(ulong attrs);
        void slk_attrset(ulong attrs);
        ulong term_attrs();
        ulong termattrs();
        void unctrl(in ISingleByteChar ch, out string str);
        void ungetmouse(in IMEVENT ev);
        void vid_attr(ulong attrs, short pair);
        void vid_puts(ulong attrs, short pair, Func<int, int> putc);
        void vidattr(ulong attrs);
        void vidputs(ulong attrs, Func<int, int> putc);
    }

    internal class NativeNCursesSingleByte<TSingleByte, TSingleByteString, TMouseEvent> : SingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent>, INativeNCursesSingleByte
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public NativeNCursesSingleByte()
        { }

        public void getmouse(out IMEVENT ev)
        {
            int size = Marshal.SizeOf<TMouseEvent>();
            Span<byte> span;

            unsafe
            {
                byte* arr = stackalloc byte[size];
                span = new Span<byte>(arr, size);

                Span<TMouseEvent> spanEv = MemoryMarshal.Cast<byte, TMouseEvent>(span);
                ref TMouseEvent spanRef = ref spanEv.GetPinnableReference();

                NCursesException.Verify(this.Wrapper.getmouse(ref spanRef), "getmouse");
                ev = spanEv[0]; //TODO: does this get nulled afterwards?
            }
        }

        public ulong mousemask(ulong newmask, out ulong oldmask)
        {
            ISingleByteChar newMouseMask = new SingleByteChar<TSingleByte>(newmask);
            ISingleByteChar oldMouseMask = new SingleByteChar<TSingleByte>(0);

            TSingleByte res = this.Wrapper.mousemask(MarshallArrayReadonly(newMouseMask), ref MarshallArray(ref oldMouseMask));

            ISingleByteChar resMouseMask = new SingleByteChar<TSingleByte>(ref res);

            oldmask = (ulong)((SingleByteChar<TSingleByte>)oldMouseMask);
            return (ulong)((SingleByteChar<TSingleByte>)resMouseMask);
        }

        public ulong slk_attr()
        {
            TSingleByte val = this.Wrapper.slk_attr();
            ISingleByteChar schar = new SingleByteChar<TSingleByte>(ref val);
            return schar.Attributes;
        }

        public void slk_attr_off(ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_off(MarshallArrayReadonly(ch), IntPtr.Zero), "slk_attr_off");
        }

        public void slk_attr_on(ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_on(MarshallArrayReadonly(ch), IntPtr.Zero), "slk_attr_on");
        }

        public void slk_attr_set(ulong attrs, short color_pair)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_set(MarshallArrayReadonly(ch), color_pair, IntPtr.Zero), "slk_attr_set");
        }

        public void slk_attroff(ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attroff(MarshallArrayReadonly(ch)), "slk_attroff");
        }

        public void slk_attron(ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attron(MarshallArrayReadonly(ch)), "slk_attron");
        }

        public void slk_attrset(ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attrset(MarshallArrayReadonly(ch)), "slk_attrset");
        }

        public ulong term_attrs()
        {
            TSingleByte val = this.Wrapper.term_attrs();
            ISingleByteChar schar = new SingleByteChar<TSingleByte>(ref val);
            return schar.Attributes;
        }

        public ulong termattrs()
        {
            TSingleByte val = this.Wrapper.termattrs();
            ISingleByteChar schar = new SingleByteChar<TSingleByte>(ref val);
            return schar.Attributes;
        }

        public void unctrl(in ISingleByteChar sch, out string str)
        {
            str = SingleByteStringWrapper<TSingleByteString>.ReadString(ref this.Wrapper.unctrl(MarshallArrayReadonly(sch)));
        }

        public void ungetmouse(in IMEVENT ev)
        {
            if (!(ev is TMouseEvent castedEv))
                throw new InvalidCastException("Mouse event not of the correct type");

            unsafe
            {
                TMouseEvent* arr = stackalloc TMouseEvent[1];
                Span<TMouseEvent> span = new Span<TMouseEvent>(arr, 1);
                span[0] = castedEv;

                NCursesException.Verify(this.Wrapper.ungetmouse(span.GetPinnableReference()), "ungetmouse");
            }
        }

        public void vid_attr(ulong attrs, short pair)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.vid_attr(MarshallArrayReadonly(ch), pair, IntPtr.Zero), "vid_attr");
        }

        public void vid_puts(ulong attrs, short pair, Func<int, int> putc)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            IntPtr func = Marshal.GetFunctionPointerForDelegate(putc);
            try
            {
                NCursesException.Verify(this.Wrapper.vid_puts(MarshallArrayReadonly(ch), pair, IntPtr.Zero, func), "vid_puts");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }

        public void vidattr(ulong attrs)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.vidattr(MarshallArrayReadonly(ch)), "vidattr");
        }

        public void vidputs(ulong attrs, Func<int, int> putc)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            IntPtr func = Marshal.GetFunctionPointerForDelegate(putc);
            try
            {
                NCursesException.Verify(this.Wrapper.vidputs(MarshallArrayReadonly(ch), func), "vidputs");
            }
            finally
            {
                Marshal.FreeHGlobal(func);
            }
        }
    }
}
