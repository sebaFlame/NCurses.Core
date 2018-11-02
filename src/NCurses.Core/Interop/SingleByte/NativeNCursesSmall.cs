using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SingleByteString;

namespace NCurses.Core.Interop.SingleByte
{
    internal interface INativeNCursesSmall
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
        void unctrl(in INCursesSCHAR ch, out string str);
        void ungetmouse(in IMEVENT ev);
        void vid_attr(ulong attrs, short pair);
        void vid_puts(ulong attrs, short pair, Func<int, int> putc);
        void vidattr(ulong attrs);
        void vidputs(ulong attrs, Func<int, int> putc);
    }

    internal class NativeNCursesSmall<TSmall, TSmallStr> : NativeSmallBase<TSmall, TSmallStr>, INativeNCursesSmall
        where TSmall : unmanaged, INCursesSCHAR, IEquatable<TSmall>
        where TSmallStr : unmanaged
    {
        public NativeNCursesSmall()
        { }

        public void getmouse(out IMEVENT ev)
        {
            int size = Marshal.SizeOf<MEVENT<TSmall>>();
            Span<byte> span;

            unsafe
            {
                byte* arr = stackalloc byte[size];
                span = new Span<byte>(arr, size);

                Span<MEVENT<TSmall>> spanEv = MemoryMarshal.Cast<byte, MEVENT<TSmall>>(span);
                ref MEVENT<TSmall> spanRef = ref spanEv.GetPinnableReference();

                NCursesException.Verify(this.Wrapper.getmouse(ref spanRef), "getmouse");
                ev = spanEv[0]; //TODO: does this get nulled afterwards?
            }
        }

        public ulong mousemask(ulong newmask, out ulong oldmask)
        {
            INCursesSCHAR newMouseMask = new NCursesSCHAR<TSmall>(newmask);
            INCursesSCHAR oldMouseMask = new NCursesSCHAR<TSmall>(0);

            TSmall res = this.Wrapper.mousemask(MarshallArrayReadonly(newMouseMask), ref MarshallArray(ref oldMouseMask));

            INCursesSCHAR resMouseMask = new NCursesSCHAR<TSmall>(ref res);
            NCursesException.Verify(resMouseMask.Attributes == 0 ? -1 : 0, "mousemask");

            oldmask = oldMouseMask.Attributes;
            return resMouseMask.Attributes;
        }

        public ulong slk_attr()
        {
            TSmall val = this.Wrapper.slk_attr();
            INCursesSCHAR schar = new NCursesSCHAR<TSmall>(ref val);
            return schar.Attributes;
        }

        public void slk_attr_off(ulong attrs)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_off(MarshallArrayReadonly(ch), IntPtr.Zero), "slk_attr_off");
        }

        public void slk_attr_on(ulong attrs)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_on(MarshallArrayReadonly(ch), IntPtr.Zero), "slk_attr_on");
        }

        public void slk_attr_set(ulong attrs, short color_pair)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attr_set(MarshallArrayReadonly(ch), color_pair, IntPtr.Zero), "slk_attr_set");
        }

        public void slk_attroff(ulong attrs)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attroff(MarshallArrayReadonly(ch)), "slk_attroff");
        }

        public void slk_attron(ulong attrs)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attron(MarshallArrayReadonly(ch)), "slk_attron");
        }

        public void slk_attrset(ulong attrs)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.slk_attrset(MarshallArrayReadonly(ch)), "slk_attrset");
        }

        public ulong term_attrs()
        {
            TSmall val = this.Wrapper.term_attrs();
            INCursesSCHAR schar = new NCursesSCHAR<TSmall>(ref val);
            return schar.Attributes;
        }

        public ulong termattrs()
        {
            TSmall val = this.Wrapper.termattrs();
            INCursesSCHAR schar = new NCursesSCHAR<TSmall>(ref val);
            return schar.Attributes;
        }

        public void unctrl(in INCursesSCHAR sch, out string str)
        {
            str = NativeSmallStrBase<TSmallStr>.ReadString(ref this.Wrapper.unctrl(MarshallArrayReadonly(sch)));
        }

        public void ungetmouse(in IMEVENT ev)
        {
            if (!(ev is MEVENT<TSmall> castedEv))
                throw new InvalidCastException("Mouse event not of the correct type");

            int size = Marshal.SizeOf<MEVENT<TSmall>>();
            Span<byte> span;

            unsafe
            {
                byte* arr = stackalloc byte[size];
                span = new Span<byte>(arr, size);

                Span<MEVENT<TSmall>> spanEv = MemoryMarshal.Cast<byte, MEVENT<TSmall>>(span);
                spanEv[0] = castedEv;

                NCursesException.Verify(this.Wrapper.ungetmouse(spanEv.GetPinnableReference()), "ungetmouse");
            }
        }

        public void vid_attr(ulong attrs, short pair)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.vid_attr(MarshallArrayReadonly(ch), pair, IntPtr.Zero), "vid_attr");
        }

        public void vid_puts(ulong attrs, short pair, Func<int, int> putc)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
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
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.vidattr(MarshallArrayReadonly(ch)), "vidattr");
        }

        public void vidputs(ulong attrs, Func<int, int> putc)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
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
