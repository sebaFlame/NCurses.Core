using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SingleByte
{
    internal interface INativeWindowSmall
    {
        void box(IntPtr window, in INCursesSCHAR verch, in INCursesSCHAR horch);
        INCursesSCHAR getbkgd(IntPtr window);
        void mvwaddch(IntPtr window, int y, int x, in INCursesSCHAR ch);
        void mvwaddchnstr(IntPtr window, int y, int x, in INCursesSCHARStr chstr, int n);
        void mvwaddchstr(IntPtr window, int y, int x, in INCursesSCHARStr chstr);
        void mvwchgat(IntPtr window, int y, int x, int number, ulong attrs, short pair);
        void mvwhline(IntPtr window, int y, int x, in INCursesSCHAR ch, int count);
        void mvwinch(IntPtr window, int y, int x, out INCursesSCHAR ch);
        void mvwinchnstr(IntPtr window, int y, int x, out INCursesSCHARStr chStr, int count, out int read);
        void mvwinchstr(IntPtr window, int y, int x, out INCursesSCHARStr chStr, out int read);
        void mvwinsch(IntPtr window, int y, int x, in INCursesSCHAR ch);
        void mvwvline(IntPtr window, int y, int x, in INCursesSCHAR ch, int n);
        void waddch(IntPtr window, in INCursesSCHAR ch);
        void waddchnstr(IntPtr window, in INCursesSCHARStr chstr, int number);
        void waddchstr(IntPtr window, in INCursesSCHARStr chstr);
        void wattr_get(IntPtr window, out ulong attrs, out short pair);
        void wattr_off(IntPtr window, ulong attrs);
        void wattr_on(IntPtr window, ulong attrs);
        void wattr_set(IntPtr window, ulong attrs, short pair);
        void wbkgd(IntPtr window, in INCursesSCHAR bkgd);
        void wbkgdset(IntPtr window, in INCursesSCHAR bkgd);
        void wborder(IntPtr window, in INCursesSCHAR ls, in INCursesSCHAR rs, in INCursesSCHAR ts, in INCursesSCHAR bs, in INCursesSCHAR tl, in INCursesSCHAR tr, in INCursesSCHAR bl, in INCursesSCHAR br);
        void wchgat(IntPtr window, int number, ulong attrs, short pair);
        void wechochar(IntPtr window, in INCursesSCHAR ch);
        void whline(IntPtr window, in INCursesSCHAR ch, int count);
        void winch(IntPtr window, out INCursesSCHAR ch);
        void winchnstr(IntPtr window, out INCursesSCHARStr txt, int count, out int read);
        void winchstr(IntPtr window, out INCursesSCHARStr txt, out int read);
        void winsch(IntPtr window, in INCursesSCHAR ch);
        void wvline(IntPtr window, in INCursesSCHAR ch, int n);
    }

    internal class NativeWindowSmall<TSmall, TSmallStr> : NativeSmallBase<TSmall, TSmallStr>, INativeWindowSmall
        where TSmall : unmanaged, INCursesSCHAR, IEquatable<TSmall>
        where TSmallStr : unmanaged
    {
        public NativeWindowSmall()
        { }

        public void box(IntPtr window, in INCursesSCHAR verch, in INCursesSCHAR horch)
        {
            NCursesException.Verify(this.Wrapper.box(window, MarshallArrayReadonly(verch), MarshallArrayReadonly(horch)), "box");
        }

        public INCursesSCHAR getbkgd(IntPtr window)
        {
            TSmall s = this.Wrapper.getbkgd(window);
            return new NCursesSCHAR<TSmall>(ref s);
        }

        public void mvwaddch(IntPtr window, int y, int x, in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.mvwaddch(window, y, x, MarshallArrayReadonly(ch)), "mvwaddch");
        }

        public void mvwaddchnstr(IntPtr window, int y, int x, in INCursesSCHARStr chstr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwaddchnstr(window, y, x, MarshallArrayReadonly(chstr), n), "mvwaddchnstr");
        }

        public void mvwaddchstr(IntPtr window, int y, int x, in INCursesSCHARStr chstr)
        {
            NCursesException.Verify(this.Wrapper.mvwaddchstr(window, y, x, MarshallArrayReadonly(chstr)), "mvwaddchstr");
        }

        public void mvwchgat(IntPtr window, int y, int x, int number, ulong attrs, short pair)
        {
            INCursesSCHAR a = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.mvwchgat(window, y, x, number, MarshallArrayReadonly(a), pair, IntPtr.Zero), "mvwchgat");
        }

        public void mvwhline(IntPtr window, int y, int x, in INCursesSCHAR ch, int count)
        {
            NCursesException.Verify(this.Wrapper.mvwhline(window, y, x, MarshallArrayReadonly(ch), count), "mvwhline");
        }

        public void mvwinch(IntPtr window, int y, int x, out INCursesSCHAR ch)
        {
            TSmall val = this.Wrapper.mvwinch(window, y, x);
            ch = new NCursesSCHAR<TSmall>(ref val);
        }

        public void mvwinchnstr(IntPtr window, int y, int x, out INCursesSCHARStr chStr, int count, out int read)
        {
            chStr = new NCursesSCHARStr<TSmall>(count);
            read = NCursesException.Verify(this.Wrapper.mvwinchnstr(window, y, x, ref MarshallArray(ref chStr), count), "mvwinchnstr");
        }

        public void mvwinchstr(IntPtr window, int y, int x, out INCursesSCHARStr chStr, out int read)
        {
            chStr = new NCursesSCHARStr<TSmall>(Constants.MAX_STRING_LENGTH);
            read = NCursesException.Verify(this.Wrapper.mvwinchstr(window, y, x, ref MarshallArray(ref chStr)), "mvwinchstr");
        }

        public void mvwinsch(IntPtr window, int y, int x, in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.mvwinsch(window, y, x, MarshallArrayReadonly(ch)), "mvwinsch");
        }

        public void mvwvline(IntPtr window, int y, int x, in INCursesSCHAR ch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwvline(window, y, x, MarshallArrayReadonly(ch), n), "mvwvline");
        }

        public void waddch(IntPtr window, in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.waddch(window, MarshallArrayReadonly(ch)), "waddch");
        }

        public void waddchnstr(IntPtr window, in INCursesSCHARStr chstr, int number)
        {
            NCursesException.Verify(this.Wrapper.waddchnstr(window, MarshallArrayReadonly(chstr), number), "waddchnstr");
        }

        public void waddchstr(IntPtr window, in INCursesSCHARStr chstr)
        {
            NCursesException.Verify(this.Wrapper.waddchstr(window, MarshallArrayReadonly(chstr)), "waddchstr");
        }

        public void wattr_get(IntPtr window, out ulong attrs, out short pair)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(0);
            pair = 0;
            NCursesException.Verify(this.Wrapper.wattr_get(window, ref MarshallArray(ref ch), ref pair, IntPtr.Zero), "wattr_get");
            attrs = ch.Attributes;
        }

        public void wattr_off(IntPtr window, ulong attrs)
        {
            INCursesSCHAR attr = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.wattr_off(window, MarshallArrayReadonly(attr), IntPtr.Zero), "wattr_off");
        }

        public void wattr_on(IntPtr window, ulong attrs)
        {
            INCursesSCHAR attr = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.wattr_on(window, MarshallArrayReadonly(attr), IntPtr.Zero), "wattr_on");
        }

        public void wattr_set(IntPtr window, ulong attrs, short pair)
        {
            INCursesSCHAR attr = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.wattr_set(window, MarshallArrayReadonly(attr), pair, IntPtr.Zero), "wattr_set");
        }

        public void wbkgd(IntPtr window, in INCursesSCHAR bkgd)
        {
            NCursesException.Verify(this.Wrapper.wbkgd(window, MarshallArrayReadonly(bkgd)), "wbkgd");
        }

        public void wbkgdset(IntPtr window, in INCursesSCHAR bkgd)
        {
            this.Wrapper.wbkgdset(window, MarshallArrayReadonly(bkgd));
        }

        public void wborder(IntPtr window, in INCursesSCHAR ls, in INCursesSCHAR rs, in INCursesSCHAR ts, in INCursesSCHAR bs, in INCursesSCHAR tl, in INCursesSCHAR tr, in INCursesSCHAR bl, in INCursesSCHAR br)
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
            INCursesSCHAR attr = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.wchgat(window, number, MarshallArrayReadonly(attr), pair, IntPtr.Zero), "wchgat");
        }

        public void wechochar(IntPtr window, in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.wechochar(window, MarshallArrayReadonly(ch)), "wechochar");
        }

        public void whline(IntPtr window, in INCursesSCHAR ch, int count)
        {
            NCursesException.Verify(this.Wrapper.whline(window, MarshallArrayReadonly(ch), count), "whline");
        }

        public void winch(IntPtr window, out INCursesSCHAR ch)
        {
            TSmall val = this.Wrapper.winch(window);
            ch = new NCursesSCHAR<TSmall>(ref val);
        }

        public void winchnstr(IntPtr window, out INCursesSCHARStr txt, int count, out int read)
        {
            txt = new NCursesSCHARStr<TSmall>(count);
            read = NCursesException.Verify(this.Wrapper.winchnstr(window, ref MarshallArray(ref txt), count), "winchnstr");
        }

        public void winchstr(IntPtr window, out INCursesSCHARStr txt, out int read)
        {
            txt = new NCursesSCHARStr<TSmall>(Constants.MAX_STRING_LENGTH);
            read = NCursesException.Verify(this.Wrapper.winchstr(window, ref MarshallArray(ref txt)), "winchstr");
        }

        public void winsch(IntPtr window, in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.winsch(window, MarshallArrayReadonly(ch)), "winsch");
        }

        public void wvline(IntPtr window, in INCursesSCHAR ch, int n)
        {
            NCursesException.Verify(this.Wrapper.wvline(window, MarshallArrayReadonly(ch), n), "wvline");
        }
    }
}
