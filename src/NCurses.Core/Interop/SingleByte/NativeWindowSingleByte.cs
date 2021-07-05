using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    internal class NativeWindowSingleByte<TSingleByte, TChar, TMouseEvent> 
        : SingleByteWrapper<TSingleByte, TChar, TMouseEvent>, 
        INativeWindowSingleByte<TSingleByte, SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeWindowSingleByte(ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> wrapper)
            : base(wrapper) { }

        public void box(WindowBaseSafeHandle window, in TSingleByte verch, in TSingleByte horch)
        {
            NCursesException.Verify(this.Wrapper.box(window, verch, horch), "box");
        }

        public TSingleByte getbkgd(WindowBaseSafeHandle window)
        {
            return this.Wrapper.getbkgd(window);
        }

        public void mvwaddch(WindowBaseSafeHandle window, int y, int x, in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.mvwaddch(window, y, x, ch), "mvwaddch");
        }

        public void mvwaddchnstr(WindowBaseSafeHandle window, int y, int x, in SingleByteCharString<TSingleByte> chstr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwaddchnstr(window, y, x, in chstr.GetPinnableReference(), n), "mvwaddchnstr");
        }

        public void mvwaddchstr(WindowBaseSafeHandle window, int y, int x, in SingleByteCharString<TSingleByte> chstr)
        {
            NCursesException.Verify(this.Wrapper.mvwaddchstr(window, y, x, in chstr.GetPinnableReference()), "mvwaddchstr");
        }

        public void mvwchgat(WindowBaseSafeHandle window, int y, int x, int number, ulong attrs, short pair)
        {
            TSingleByte a = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.mvwchgat(window, y, x, number, a, pair, IntPtr.Zero), "mvwchgat");
        }

        public void mvwhline(WindowBaseSafeHandle window, int y, int x, in TSingleByte ch, int count)
        {
            NCursesException.Verify(this.Wrapper.mvwhline(window, y, x, ch, count), "mvwhline");
        }

        public void mvwinch(WindowBaseSafeHandle window, int y, int x, out TSingleByte ch)
        {
            ch = this.Wrapper.mvwinch(window, y, x);
        }

        public void mvwinchnstr(WindowBaseSafeHandle window, int y, int x, ref SingleByteCharString<TSingleByte> chStr, int count, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.mvwinchnstr(window, y, x, ref chStr.GetPinnableReference(), count), "mvwinchnstr");
        }

        public void mvwinchstr(WindowBaseSafeHandle window, int y, int x, ref SingleByteCharString<TSingleByte> chStr, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.mvwinchstr(window, y, x, ref chStr.GetPinnableReference()), "mvwinchstr");
        }

        public void mvwinsch(WindowBaseSafeHandle window, int y, int x, in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.mvwinsch(window, y, x, ch), "mvwinsch");
        }

        public void mvwvline(WindowBaseSafeHandle window, int y, int x, in TSingleByte ch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwvline(window, y, x, ch, n), "mvwvline");
        }

        public void waddch(WindowBaseSafeHandle window, in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.waddch(window, ch), "waddch");
        }

        public void waddchnstr(WindowBaseSafeHandle window, in SingleByteCharString<TSingleByte> chstr, int number)
        {
            NCursesException.Verify(this.Wrapper.waddchnstr(window, in chstr.GetPinnableReference(), number), "waddchnstr");
        }

        public void waddchstr(WindowBaseSafeHandle window, in SingleByteCharString<TSingleByte> chstr)
        {
            NCursesException.Verify(this.Wrapper.waddchstr(window, in chstr.GetPinnableReference()), "waddchstr");
        }

        public void wattr_get(WindowBaseSafeHandle window, out ulong attrs, out ushort pair)
        {
            TSingleByte ch = default;

            short limitedPair = 0;
            int extendedPair = 0;

            NCursesException.Verify(this.Wrapper.wattr_get(window, ref ch, ref limitedPair, ref extendedPair), "wattr_get");

            attrs = ch.Attributes;
            pair = extendedPair == 0 ? (ushort)limitedPair : (ushort)extendedPair;
        }

        public void wattr_off(WindowBaseSafeHandle window, ulong attrs)
        {
            TSingleByte attr = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.wattr_off(window, attr, IntPtr.Zero), "wattr_off");
        }

        public void wattr_on(WindowBaseSafeHandle window, ulong attrs)
        {
            TSingleByte attr = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.wattr_on(window, attr, IntPtr.Zero), "wattr_on");
        }

        public void wattr_set(WindowBaseSafeHandle window, ulong attrs, ushort pair)
        {
            TSingleByte attr = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);

            int extendedPair = (int)pair;
            NCursesException.Verify(this.Wrapper.wattr_set(window, attr, (short)pair, ref extendedPair), "wattr_set");
        }

        public void wbkgd(WindowBaseSafeHandle window, in TSingleByte bkgd)
        {
            NCursesException.Verify(this.Wrapper.wbkgd(window, bkgd), "wbkgd");
        }

        public void wbkgdset(WindowBaseSafeHandle window, in TSingleByte bkgd)
        {
            this.Wrapper.wbkgdset(window, bkgd);
        }

        public void wborder(WindowBaseSafeHandle window, in TSingleByte ls, in TSingleByte rs, in TSingleByte ts, in TSingleByte bs, in TSingleByte tl, in TSingleByte tr, in TSingleByte bl, in TSingleByte br)
        {
            NCursesException.Verify(this.Wrapper.wborder(
                window,
                ls,
                rs,
                ts,
                bs,
                tl,
                tr,
                bl,
                br), "wborder");
        }

        public void wchgat(WindowBaseSafeHandle window, int number, ulong attrs, short pair)
        {
            TSingleByte attr = SingleByteCharFactory<TSingleByte>._Instance.GetNativeAttribute(attrs);
            NCursesException.Verify(this.Wrapper.wchgat(window, number, attr, pair, IntPtr.Zero), "wchgat");
        }

        public void wechochar(WindowBaseSafeHandle window, in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.wechochar(window, ch), "wechochar");
        }

        public void whline(WindowBaseSafeHandle window, in TSingleByte ch, int count)
        {
            NCursesException.Verify(this.Wrapper.whline(window, ch, count), "whline");
        }

        public void winch(WindowBaseSafeHandle window, out TSingleByte ch)
        {
            ch = this.Wrapper.winch(window);
        }

        public void winchnstr(WindowBaseSafeHandle window, ref SingleByteCharString<TSingleByte> txt, int count, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.winchnstr(window, ref txt.GetPinnableReference(), count), "winchnstr");
        }

        public void winchstr(WindowBaseSafeHandle window, ref SingleByteCharString<TSingleByte> txt, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.winchstr(window, ref txt.GetPinnableReference()), "winchstr");
        }

        public void winsch(WindowBaseSafeHandle window, in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.winsch(window, ch), "winsch");
        }

        public void wvline(WindowBaseSafeHandle window, in TSingleByte ch, int n)
        {
            NCursesException.Verify(this.Wrapper.wvline(window, ch, n), "wvline");
        }
    }
}
