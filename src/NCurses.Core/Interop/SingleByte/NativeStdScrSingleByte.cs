using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    internal class NativeStdScrSingleByte<TSingleByte, TChar, TMouseEvent> 
            : SingleByteWrapper<TSingleByte, TChar, TMouseEvent>, 
            INativeStdScrSingleByte<TSingleByte, SingleByteCharString<TSingleByte>>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeStdScrSingleByte(ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> wrapper)
            : base(wrapper) { }

        public void addch(in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.addch(ch), "addch");
        }

        public void addchnstr(in SingleByteCharString<TSingleByte> txt, int number)
        {
            NCursesException.Verify(this.Wrapper.addchnstr(in txt.GetPinnableReference(), number), "addchnstr");
        }

        public void addchstr(in SingleByteCharString<TSingleByte> txt)
        {
            NCursesException.Verify(this.Wrapper.addchstr(in txt.GetPinnableReference()), "addchstr");
        }

        public void attr_get(out ulong attrs, out ushort pair)
        {
            TSingleByte ch = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetNativeEmptyCharInternal();

            short limitedPair = 0;
            int extendedPair = 0;

            NCursesException.Verify(this.Wrapper.attr_get(ref ch, ref limitedPair, ref extendedPair), "attr_get");

            attrs = ch.Attributes;
            pair = extendedPair == 0 ? (ushort)limitedPair : (ushort)extendedPair;
        }

        public void attr_off(ulong attrs)
        {
            TSingleByte attr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.attr_off(attr, IntPtr.Zero), "attr_off");
        }

        public void attr_on(ulong attrs)
        {
            TSingleByte attr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.attr_on(attr, IntPtr.Zero), "attr_on");
        }

        public void attr_set(ulong attrs, ushort pair)
        {
            TSingleByte attr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);

            int extendedPair = (int)pair;
            NCursesException.Verify(this.Wrapper.attr_set(attr, (short)pair, ref extendedPair), "attr_set");
        }

        public void bkgd(in TSingleByte bkgd)
        {
            NCursesException.Verify(this.Wrapper.bkgd(bkgd), "bkgd");
        }

        public void bkgdset(in TSingleByte bkgd)
        {
            this.Wrapper.bkgdset(bkgd);
        }

        public void border(in TSingleByte ls, in TSingleByte rs, in TSingleByte ts, in TSingleByte bs, in TSingleByte tl, in TSingleByte tr, in TSingleByte bl, in TSingleByte br)
        {
            NCursesException.Verify(this.Wrapper.border(
                ls,
                rs,
                ts,
                bs,
                tl,
                tr,
                bl,
                br), "border");
        }

        public void chgat(int number, ulong attrs, short pair)
        {
            TSingleByte attr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.chgat(number, attr, pair, IntPtr.Zero), "chgat");
        }

        public void echochar(in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.echochar(ch), "echochar");
        }

        public void hline(in TSingleByte ch, int count)
        {
            NCursesException.Verify(this.Wrapper.hline(ch, count), "hline");
        }

        public void inch(out TSingleByte ch)
        {
            ch = this.Wrapper.inch();
        }

        public void inchnstr(ref SingleByteCharString<TSingleByte> txt, int count, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.inchnstr(ref txt.GetPinnableReference(), count), "inchnstr");
        }

        public void inchstr(ref SingleByteCharString<TSingleByte> txt, out int read)
        {
            //TODO: can overflow
            read = this.Wrapper.inchstr(ref txt.GetPinnableReference());
            NCursesException.Verify(read, "inchstr");
        }

        public void insch(in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.insch(ch), "insch");
        }

        public void mvaddch(int y, int x, in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.mvaddch(y, x, ch), "mvaddch");
        }

        public void mvaddchnstr(int y, int x, in SingleByteCharString<TSingleByte> chStr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvaddchnstr(y, x, in chStr.GetPinnableReference(), n), "mvaddchnstr");
        }

        public void mvaddchstr(int y, int x, in SingleByteCharString<TSingleByte> chStr)
        {
            NCursesException.Verify(this.Wrapper.mvaddchstr(y, x, in chStr.GetPinnableReference()), "mvaddchstr");
        }

        public void mvchgat(int y, int x, int number, ulong attrs, short pair)
        {
            TSingleByte attr = SingleByteCharFactoryInternal<TSingleByte>.Instance.GetAttributeInternal(attrs);
            NCursesException.Verify(this.Wrapper.mvchgat(y, x, number, attr, pair, IntPtr.Zero), "mvchgat");
        }

        public void mvhline(int y, int x, in TSingleByte ch, int count)
        {
            NCursesException.Verify(this.Wrapper.mvhline(y, x, ch, count), "mvhline");
        }

        public void mvinch(int y, int x, out TSingleByte ch)
        {
            ch = this.Wrapper.mvinch(y, x);
        }

        public void mvinchnstr(int y, int x, ref SingleByteCharString<TSingleByte> chstr, int count, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.mvinchnstr(y, x, ref chstr.GetPinnableReference(), count), "mvinchnstr");
        }

        public void mvinchstr(int y, int x, ref SingleByteCharString<TSingleByte> chstr, out int read)
        {
            //TODO: can overflow
            read = NCursesException.Verify(this.Wrapper.mvinchstr(y, x, ref chstr.GetPinnableReference()), "mvinchstr");
        }

        public void mvinsch(int y, int x, in TSingleByte ch)
        {
            NCursesException.Verify(this.Wrapper.mvinsch(y, x, ch), "mvinsch");
        }

        public void mvvline(int y, int x, in TSingleByte ch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvvline(y, x, ch, n), "mvvline");
        }

        public void vline(in TSingleByte ch, int n)
        {
            NCursesException.Verify(this.Wrapper.vline(ch, n), "vline");
        }
    }
}
