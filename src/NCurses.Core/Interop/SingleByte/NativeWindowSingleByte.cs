using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    internal interface INativeWindowSingleByte
    {
        void box(IntPtr window, in ISingleByteChar verch, in ISingleByteChar horch);
        ISingleByteChar getbkgd(IntPtr window);
        void mvwaddch(IntPtr window, int y, int x, in ISingleByteChar ch);
        void mvwaddchnstr(IntPtr window, int y, int x, in ISingleByteCharString chstr, int n);
        void mvwaddchstr(IntPtr window, int y, int x, in ISingleByteCharString chstr);
        void mvwchgat(IntPtr window, int y, int x, int number, ulong attrs, short pair);
        void mvwhline(IntPtr window, int y, int x, in ISingleByteChar ch, int count);
        void mvwinch(IntPtr window, int y, int x, out ISingleByteChar ch);
        void mvwinchnstr(IntPtr window, int y, int x, out ISingleByteCharString chStr, int count, out int read);
        void mvwinchstr(IntPtr window, int y, int x, out ISingleByteCharString chStr, out int read);
        void mvwinsch(IntPtr window, int y, int x, in ISingleByteChar ch);
        void mvwvline(IntPtr window, int y, int x, in ISingleByteChar ch, int n);
        void waddch(IntPtr window, in ISingleByteChar ch);
        void waddchnstr(IntPtr window, in ISingleByteCharString chstr, int number);
        void waddchstr(IntPtr window, in ISingleByteCharString chstr);
        void wattr_get(IntPtr window, out ulong attrs, out short pair);
        void wattr_off(IntPtr window, ulong attrs);
        void wattr_on(IntPtr window, ulong attrs);
        void wattr_set(IntPtr window, ulong attrs, short pair);
        void wbkgd(IntPtr window, in ISingleByteChar bkgd);
        void wbkgdset(IntPtr window, in ISingleByteChar bkgd);
        void wborder(IntPtr window, in ISingleByteChar ls, in ISingleByteChar rs, in ISingleByteChar ts, in ISingleByteChar bs, in ISingleByteChar tl, in ISingleByteChar tr, in ISingleByteChar bl, in ISingleByteChar br);
        void wchgat(IntPtr window, int number, ulong attrs, short pair);
        void wechochar(IntPtr window, in ISingleByteChar ch);
        void whline(IntPtr window, in ISingleByteChar ch, int count);
        void winch(IntPtr window, out ISingleByteChar ch);
        void winchnstr(IntPtr window, out ISingleByteCharString txt, int count, out int read);
        void winchstr(IntPtr window, out ISingleByteCharString txt, out int read);
        void winsch(IntPtr window, in ISingleByteChar ch);
        void wvline(IntPtr window, in ISingleByteChar ch, int n);
    }

    internal class NativeWindowSingleByte<TSingleByte, TSingleByteString, TMouseEvent> : SingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent>, INativeWindowSingleByte
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public NativeWindowSingleByte()
        { }

        public void box(IntPtr window, in ISingleByteChar verch, in ISingleByteChar horch)
        {
            NCursesException.Verify(this.Wrapper.box(window, MarshallArrayReadonly(verch), MarshallArrayReadonly(horch)), "box");
        }

        public ISingleByteChar getbkgd(IntPtr window)
        {
            TSingleByte s = this.Wrapper.getbkgd(window);
            return new SingleByteChar<TSingleByte>(ref s);
        }

        public void mvwaddch(IntPtr window, int y, int x, in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.mvwaddch(window, y, x, MarshallArrayReadonly(ch)), "mvwaddch");
        }

        public void mvwaddchnstr(IntPtr window, int y, int x, in ISingleByteCharString chstr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwaddchnstr(window, y, x, MarshallArrayReadonly(chstr), n), "mvwaddchnstr");
        }

        public void mvwaddchstr(IntPtr window, int y, int x, in ISingleByteCharString chstr)
        {
            NCursesException.Verify(this.Wrapper.mvwaddchstr(window, y, x, MarshallArrayReadonly(chstr)), "mvwaddchstr");
        }

        public void mvwchgat(IntPtr window, int y, int x, int number, ulong attrs, short pair)
        {
            ISingleByteChar a = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.mvwchgat(window, y, x, number, MarshallArrayReadonly(a), pair, IntPtr.Zero), "mvwchgat");
        }

        public void mvwhline(IntPtr window, int y, int x, in ISingleByteChar ch, int count)
        {
            NCursesException.Verify(this.Wrapper.mvwhline(window, y, x, MarshallArrayReadonly(ch), count), "mvwhline");
        }

        public void mvwinch(IntPtr window, int y, int x, out ISingleByteChar ch)
        {
            TSingleByte val = this.Wrapper.mvwinch(window, y, x);
            ch = new SingleByteChar<TSingleByte>(ref val);
        }

        public void mvwinchnstr(IntPtr window, int y, int x, out ISingleByteCharString chStr, int count, out int read)
        {
            chStr = new SingleByteCharString<TSingleByte>(count);
            read = NCursesException.Verify(this.Wrapper.mvwinchnstr(window, y, x, ref MarshallArray(ref chStr), count), "mvwinchnstr");
        }

        public void mvwinchstr(IntPtr window, int y, int x, out ISingleByteCharString chStr, out int read)
        {
            chStr = new SingleByteCharString<TSingleByte>(Constants.MAX_STRING_LENGTH);
            read = NCursesException.Verify(this.Wrapper.mvwinchstr(window, y, x, ref MarshallArray(ref chStr)), "mvwinchstr");
        }

        public void mvwinsch(IntPtr window, int y, int x, in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.mvwinsch(window, y, x, MarshallArrayReadonly(ch)), "mvwinsch");
        }

        public void mvwvline(IntPtr window, int y, int x, in ISingleByteChar ch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwvline(window, y, x, MarshallArrayReadonly(ch), n), "mvwvline");
        }

        public void waddch(IntPtr window, in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.waddch(window, MarshallArrayReadonly(ch)), "waddch");
        }

        public void waddchnstr(IntPtr window, in ISingleByteCharString chstr, int number)
        {
            NCursesException.Verify(this.Wrapper.waddchnstr(window, MarshallArrayReadonly(chstr), number), "waddchnstr");
        }

        public void waddchstr(IntPtr window, in ISingleByteCharString chstr)
        {
            NCursesException.Verify(this.Wrapper.waddchstr(window, MarshallArrayReadonly(chstr)), "waddchstr");
        }

        public void wattr_get(IntPtr window, out ulong attrs, out short pair)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(0);
            pair = 0;
            NCursesException.Verify(this.Wrapper.wattr_get(window, ref MarshallArray(ref ch), ref pair, IntPtr.Zero), "wattr_get");
            attrs = ch.Attributes;
        }

        public void wattr_off(IntPtr window, ulong attrs)
        {
            ISingleByteChar attr = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.wattr_off(window, MarshallArrayReadonly(attr), IntPtr.Zero), "wattr_off");
        }

        public void wattr_on(IntPtr window, ulong attrs)
        {
            ISingleByteChar attr = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.wattr_on(window, MarshallArrayReadonly(attr), IntPtr.Zero), "wattr_on");
        }

        public void wattr_set(IntPtr window, ulong attrs, short pair)
        {
            ISingleByteChar attr = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.wattr_set(window, MarshallArrayReadonly(attr), pair, IntPtr.Zero), "wattr_set");
        }

        public void wbkgd(IntPtr window, in ISingleByteChar bkgd)
        {
            NCursesException.Verify(this.Wrapper.wbkgd(window, MarshallArrayReadonly(bkgd)), "wbkgd");
        }

        public void wbkgdset(IntPtr window, in ISingleByteChar bkgd)
        {
            this.Wrapper.wbkgdset(window, MarshallArrayReadonly(bkgd));
        }

        public void wborder(IntPtr window, in ISingleByteChar ls, in ISingleByteChar rs, in ISingleByteChar ts, in ISingleByteChar bs, in ISingleByteChar tl, in ISingleByteChar tr, in ISingleByteChar bl, in ISingleByteChar br)
        {
            NCursesException.Verify(this.Wrapper.wborder(
                window,
                MarshallArrayReadonly(ls),
                MarshallArrayReadonly(rs),
                MarshallArrayReadonly(ts),
                MarshallArrayReadonly(bs),
                MarshallArrayReadonly(tl),
                MarshallArrayReadonly(tr),
                MarshallArrayReadonly(bl),
                MarshallArrayReadonly(br)), "wborder");
        }

        public void wchgat(IntPtr window, int number, ulong attrs, short pair)
        {
            ISingleByteChar attr = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.wchgat(window, number, MarshallArrayReadonly(attr), pair, IntPtr.Zero), "wchgat");
        }

        public void wechochar(IntPtr window, in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.wechochar(window, MarshallArrayReadonly(ch)), "wechochar");
        }

        public void whline(IntPtr window, in ISingleByteChar ch, int count)
        {
            NCursesException.Verify(this.Wrapper.whline(window, MarshallArrayReadonly(ch), count), "whline");
        }

        public void winch(IntPtr window, out ISingleByteChar ch)
        {
            TSingleByte val = this.Wrapper.winch(window);
            ch = new SingleByteChar<TSingleByte>(ref val);
        }

        public void winchnstr(IntPtr window, out ISingleByteCharString txt, int count, out int read)
        {
            txt = new SingleByteCharString<TSingleByte>(count);
            read = NCursesException.Verify(this.Wrapper.winchnstr(window, ref MarshallArray(ref txt), count), "winchnstr");
        }

        public void winchstr(IntPtr window, out ISingleByteCharString txt, out int read)
        {
            txt = new SingleByteCharString<TSingleByte>(Constants.MAX_STRING_LENGTH);
            read = NCursesException.Verify(this.Wrapper.winchstr(window, ref MarshallArray(ref txt)), "winchstr");
        }

        public void winsch(IntPtr window, in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.winsch(window, MarshallArrayReadonly(ch)), "winsch");
        }

        public void wvline(IntPtr window, in ISingleByteChar ch, int n)
        {
            NCursesException.Verify(this.Wrapper.wvline(window, MarshallArrayReadonly(ch), n), "wvline");
        }
    }
}
