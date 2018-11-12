using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INativeStdScrSingleByte
    {
        void addch(in ISingleByteChar ch);
        void addchnstr(in ISingleByteCharString txt, int number);
        void addchstr(in ISingleByteCharString txt);
        void attr_get(out ulong attrs, out short pair);
        void attr_off(ulong attrs);
        void attr_on(ulong attrs);
        void attr_set(ulong attrs, short pair);
        void bkgd(in ISingleByteChar bkgd);
        void bkgdset(in ISingleByteChar bkgd);
        void border(in ISingleByteChar ls, in ISingleByteChar rs, in ISingleByteChar ts, in ISingleByteChar bs, in ISingleByteChar tl, in ISingleByteChar tr, in ISingleByteChar bl, in ISingleByteChar br);
        void chgat(int number, ulong attrs, short pair);
        void echochar(in ISingleByteChar ch);
        void hline(in ISingleByteChar ch, int count);
        void inch(out ISingleByteChar ch);
        void inchnstr(out ISingleByteCharString txt, int count, out int read);
        void inchstr(out ISingleByteCharString txt, out int read);
        void insch(in ISingleByteChar ch);
        void mvaddch(int y, int x, in ISingleByteChar ch);
        void mvaddchnstr(int y, int x, in ISingleByteCharString chstr, int n);
        void mvaddchstr(int y, int x, in ISingleByteCharString chStr);
        void mvchgat(int y, int x, int number, ulong attrs, short pair);
        void mvhline(int y, int x, in ISingleByteChar ch, int count);
        void mvinch(int y, int x, out ISingleByteChar ch);
        void mvinchnstr(int y, int x, out ISingleByteCharString chstr, int count, out int read);
        void mvinchstr(int y, int x, out ISingleByteCharString chstr, out int read);
        void mvinsch(int y, int x, in ISingleByteChar ch);
        void mvvline(int y, int x, in ISingleByteChar ch, int n);
        void vline(in ISingleByteChar ch, int n);
    }

    public class NativeStdScrSingleByte<TSingleByte, TSingleByteString, TMouseEvent> : SingleByteWrapper<TSingleByte, TSingleByteString, TMouseEvent>, INativeStdScrSingleByte
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public NativeStdScrSingleByte() { }

        public void addch(in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.addch(MarshallArrayReadonly(ch)), "addch");
        }

        public void addchnstr(in ISingleByteCharString txt, int number)
        {
            NCursesException.Verify(this.Wrapper.addchnstr(MarshallArrayReadonly(txt), number), "addchnstr");
        }

        public void addchstr(in ISingleByteCharString txt)
        {
            NCursesException.Verify(this.Wrapper.addchstr(MarshallArrayReadonly(txt)), "addchstr");
        }

        public void attr_get(out ulong attrs, out short pair)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(0);
            pair = 0;
            NCursesException.Verify(this.Wrapper.attr_get(ref MarshallArray(ref ch), ref pair, IntPtr.Zero), "attr_get");
            attrs = ch.Attributes;
        }

        public void attr_off(ulong attrs)
        {
            ISingleByteChar attr = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.attr_off(MarshallArrayReadonly(attr), IntPtr.Zero), "attr_off");
        }

        public void attr_on(ulong attrs)
        {
            ISingleByteChar attr = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.attr_on(MarshallArrayReadonly(attr), IntPtr.Zero), "attr_on");
        }

        public void attr_set(ulong attrs, short pair)
        {
            ISingleByteChar attr = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.attr_set(MarshallArrayReadonly(attr), pair, IntPtr.Zero), "attr_set");
        }

        public void bkgd(in ISingleByteChar bkgd)
        {
            NCursesException.Verify(this.Wrapper.bkgd(MarshallArrayReadonly(bkgd)), "bkgd");
        }

        public void bkgdset(in ISingleByteChar bkgd)
        {
            this.Wrapper.bkgdset(MarshallArrayReadonly(bkgd));
        }

        public void border(in ISingleByteChar ls, in ISingleByteChar rs, in ISingleByteChar ts, in ISingleByteChar bs, in ISingleByteChar tl, in ISingleByteChar tr, in ISingleByteChar bl, in ISingleByteChar br)
        {
            NCursesException.Verify(this.Wrapper.border(
                MarshallArrayReadonly(ls),
                MarshallArrayReadonly(rs),
                MarshallArrayReadonly(ts),
                MarshallArrayReadonly(bs),
                MarshallArrayReadonly(tl),
                MarshallArrayReadonly(tr),
                MarshallArrayReadonly(bl),
                MarshallArrayReadonly(br)), "border");
        }

        public void chgat(int number, ulong attrs, short pair)
        {
            ISingleByteChar attr = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.chgat(number, MarshallArrayReadonly(attr), pair, IntPtr.Zero), "chgat");
        }

        public void echochar(in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.echochar(MarshallArrayReadonly(ch)), "echochar");
        }

        public void hline(in ISingleByteChar ch, int count)
        {
            NCursesException.Verify(this.Wrapper.hline(MarshallArrayReadonly(ch), count), "hline");
        }

        public void inch(out ISingleByteChar ch)
        {
            TSingleByte val = this.Wrapper.inch();
            ch = new SingleByteChar<TSingleByte>(ref val);
        }

        public void inchnstr(out ISingleByteCharString txt, int count, out int read)
        {
            txt = new SingleByteCharString<TSingleByte>(count);
            read = NCursesException.Verify(this.Wrapper.inchnstr(ref MarshallArray(ref txt), count), "inchnstr");
        }

        public void inchstr(out ISingleByteCharString txt, out int read)
        {
            //TODO: can overflow
            txt = new SingleByteCharString<TSingleByte>(Constants.MAX_STRING_LENGTH);
            read = this.Wrapper.inchstr(ref MarshallArray(ref txt));
            NCursesException.Verify(read, "inchstr");
        }

        public void insch(in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.insch(MarshallArrayReadonly(ch)), "insch");
        }

        public void mvaddch(int y, int x, in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.mvaddch(y, x, MarshallArrayReadonly(ch)), "mvaddch");
        }

        public void mvaddchnstr(int y, int x, in ISingleByteCharString chStr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvaddchnstr(y, x, MarshallArrayReadonly(chStr), n), "mvaddchnstr");
        }

        public void mvaddchstr(int y, int x, in ISingleByteCharString chStr)
        {
            NCursesException.Verify(this.Wrapper.mvaddchstr(y, x, MarshallArrayReadonly(chStr)), "mvaddchstr");
        }

        public void mvchgat(int y, int x, int number, ulong attrs, short pair)
        {
            ISingleByteChar ch = new SingleByteChar<TSingleByte>(attrs);
            NCursesException.Verify(this.Wrapper.mvchgat(y, x, number, MarshallArrayReadonly(ch), pair, IntPtr.Zero), "mvchgat");
        }

        public void mvhline(int y, int x, in ISingleByteChar ch, int count)
        {
            NCursesException.Verify(this.Wrapper.mvhline(y, x, MarshallArrayReadonly(ch), count), "mvhline");
        }

        public void mvinch(int y, int x, out ISingleByteChar ch)
        {
            TSingleByte val = this.Wrapper.mvinch(y, x);
            ch = new SingleByteChar<TSingleByte>(ref val);
        }

        public void mvinchnstr(int y, int x, out ISingleByteCharString chstr, int count, out int read)
        {
            chstr = new SingleByteCharString<TSingleByte>(count);
            read = NCursesException.Verify(this.Wrapper.mvinchnstr(y, x, ref MarshallArray(ref chstr), count), "mvinchnstr");
        }

        public void mvinchstr(int y, int x, out ISingleByteCharString chstr, out int read)
        {
            //TODO: can overflow
            chstr = new SingleByteCharString<TSingleByte>(Constants.MAX_STRING_LENGTH);
            read = NCursesException.Verify(this.Wrapper.mvinchstr(y, x, ref MarshallArray(ref chstr)), "mvinchstr");
        }

        public void mvinsch(int y, int x, in ISingleByteChar ch)
        {
            NCursesException.Verify(this.Wrapper.mvinsch(y, x, MarshallArrayReadonly(ch)), "mvinsch");
        }

        public void mvvline(int y, int x, in ISingleByteChar ch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvvline(y, x, MarshallArrayReadonly(ch), n), "mvvline");
        }

        public void vline(in ISingleByteChar ch, int n)
        {
            NCursesException.Verify(this.Wrapper.vline(MarshallArrayReadonly(ch), n), "vline");
        }
    }
}
