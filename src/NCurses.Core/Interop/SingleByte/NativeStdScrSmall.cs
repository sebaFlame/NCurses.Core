using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SingleByte
{
    public interface INativeStdScrSmall
    {
        void addch(in INCursesSCHAR ch);
        void addchnstr(in INCursesSCHARStr txt, int number);
        void addchstr(in INCursesSCHARStr txt);
        void attr_get(out ulong attrs, out short pair);
        void attr_off(ulong attrs);
        void attr_on(ulong attrs);
        void attr_set(ulong attrs, short pair);
        void bkgd(in INCursesSCHAR bkgd);
        void bkgdset(in INCursesSCHAR bkgd);
        void border(in INCursesSCHAR ls, in INCursesSCHAR rs, in INCursesSCHAR ts, in INCursesSCHAR bs, in INCursesSCHAR tl, in INCursesSCHAR tr, in INCursesSCHAR bl, in INCursesSCHAR br);
        void chgat(int number, ulong attrs, short pair);
        void echochar(in INCursesSCHAR ch);
        void hline(in INCursesSCHAR ch, int count);
        void inch(out INCursesSCHAR ch);
        void inchnstr(out INCursesSCHARStr txt, int count, out int read);
        void inchstr(out INCursesSCHARStr txt, out int read);
        void insch(in INCursesSCHAR ch);
        void mvaddch(int y, int x, in INCursesSCHAR ch);
        void mvaddchnstr(int y, int x, in INCursesSCHARStr chstr, int n);
        void mvaddchstr(int y, int x, in INCursesSCHARStr chStr);
        void mvchgat(int y, int x, int number, ulong attrs, short pair);
        void mvhline(int y, int x, in INCursesSCHAR ch, int count);
        void mvinch(int y, int x, out INCursesSCHAR ch);
        void mvinchnstr(int y, int x, out INCursesSCHARStr chstr, int count, out int read);
        void mvinchstr(int y, int x, out INCursesSCHARStr chstr, out int read);
        void mvinsch(int y, int x, in INCursesSCHAR ch);
        void mvvline(int y, int x, in INCursesSCHAR ch, int n);
        void vline(in INCursesSCHAR ch, int n);
    }

    public class NativeStdScrSmall<TSmall, TSmallStr> : NativeSmallBase<TSmall, TSmallStr>, INativeStdScrSmall
        where TSmall : unmanaged, INCursesSCHAR
        where TSmallStr : unmanaged
    {
        public NativeStdScrSmall() { }

        public void addch(in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.addch(MarshallArrayReadonly(ch)), "addch");
        }

        public void addchnstr(in INCursesSCHARStr txt, int number)
        {
            NCursesException.Verify(this.Wrapper.addchnstr(MarshallArrayReadonly(txt), number), "addchnstr");
        }

        public void addchstr(in INCursesSCHARStr txt)
        {
            NCursesException.Verify(this.Wrapper.addchstr(MarshallArrayReadonly(txt)), "addchstr");
        }

        public void attr_get(out ulong attrs, out short pair)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(0);
            pair = 0;
            NCursesException.Verify(this.Wrapper.attr_get(ref MarshallArray(ref ch), ref pair, IntPtr.Zero), "attr_get");
            attrs = ch.Attributes;
        }

        public void attr_off(ulong attrs)
        {
            INCursesSCHAR attr = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.attr_off(MarshallArrayReadonly(attr), IntPtr.Zero), "attr_off");
        }

        public void attr_on(ulong attrs)
        {
            INCursesSCHAR attr = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.attr_on(MarshallArrayReadonly(attr), IntPtr.Zero), "attr_on");
        }

        public void attr_set(ulong attrs, short pair)
        {
            INCursesSCHAR attr = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.attr_set(MarshallArrayReadonly(attr), pair, IntPtr.Zero), "attr_set");
        }

        public void bkgd(in INCursesSCHAR bkgd)
        {
            NCursesException.Verify(this.Wrapper.bkgd(MarshallArrayReadonly(bkgd)), "bkgd");
        }

        public void bkgdset(in INCursesSCHAR bkgd)
        {
            this.Wrapper.bkgdset(MarshallArrayReadonly(bkgd));
        }

        public void border(in INCursesSCHAR ls, in INCursesSCHAR rs, in INCursesSCHAR ts, in INCursesSCHAR bs, in INCursesSCHAR tl, in INCursesSCHAR tr, in INCursesSCHAR bl, in INCursesSCHAR br)
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
            INCursesSCHAR attr = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.chgat(number, MarshallArrayReadonly(attr), pair, IntPtr.Zero), "chgat");
        }

        public void echochar(in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.echochar(MarshallArrayReadonly(ch)), "echochar");
        }

        public void hline(in INCursesSCHAR ch, int count)
        {
            NCursesException.Verify(this.Wrapper.hline(MarshallArrayReadonly(ch), count), "hline");
        }

        public void inch(out INCursesSCHAR ch)
        {
            TSmall val = this.Wrapper.inch();
            ch = new NCursesSCHAR<TSmall>(ref val);
        }

        public void inchnstr(out INCursesSCHARStr txt, int count, out int read)
        {
            txt = new NCursesSCHARStr<TSmall>(count);
            read = NCursesException.Verify(this.Wrapper.inchnstr(ref MarshallArray(ref txt), count), "inchnstr");
        }

        public void inchstr(out INCursesSCHARStr txt, out int read)
        {
            //TODO: can overflow
            txt = new NCursesSCHARStr<TSmall>(Constants.MAX_STRING_LENGTH);
            read = this.Wrapper.inchstr(ref MarshallArray(ref txt));
            NCursesException.Verify(read, "inchstr");
        }

        public void insch(in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.insch(MarshallArrayReadonly(ch)), "insch");
        }

        public void mvaddch(int y, int x, in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.mvaddch(y, x, MarshallArrayReadonly(ch)), "mvaddch");
        }

        public void mvaddchnstr(int y, int x, in INCursesSCHARStr chStr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvaddchnstr(y, x, MarshallArrayReadonly(chStr), n), "mvaddchnstr");
        }

        public void mvaddchstr(int y, int x, in INCursesSCHARStr chStr)
        {
            NCursesException.Verify(this.Wrapper.mvaddchstr(y, x, MarshallArrayReadonly(chStr)), "mvaddchstr");
        }

        public void mvchgat(int y, int x, int number, ulong attrs, short pair)
        {
            INCursesSCHAR ch = new NCursesSCHAR<TSmall>(attrs);
            NCursesException.Verify(this.Wrapper.mvchgat(y, x, number, MarshallArrayReadonly(ch), pair, IntPtr.Zero), "mvchgat");
        }

        public void mvhline(int y, int x, in INCursesSCHAR ch, int count)
        {
            NCursesException.Verify(this.Wrapper.mvhline(y, x, MarshallArrayReadonly(ch), count), "mvhline");
        }

        public void mvinch(int y, int x, out INCursesSCHAR ch)
        {
            TSmall val = this.Wrapper.mvinch(y, x);
            ch = new NCursesSCHAR<TSmall>(ref val);
        }

        public void mvinchnstr(int y, int x, out INCursesSCHARStr chstr, int count, out int read)
        {
            chstr = new NCursesSCHARStr<TSmall>(count);
            read = NCursesException.Verify(this.Wrapper.mvinchnstr(y, x, ref MarshallArray(ref chstr), count), "mvinchnstr");
        }

        public void mvinchstr(int y, int x, out INCursesSCHARStr chstr, out int read)
        {
            //TODO: can overflow
            chstr = new NCursesSCHARStr<TSmall>(Constants.MAX_STRING_LENGTH);
            read = NCursesException.Verify(this.Wrapper.mvinchstr(y, x, ref MarshallArray(ref chstr)), "mvinchstr");
        }

        public void mvinsch(int y, int x, in INCursesSCHAR ch)
        {
            NCursesException.Verify(this.Wrapper.mvinsch(y, x, MarshallArrayReadonly(ch)), "mvinsch");
        }

        public void mvvline(int y, int x, in INCursesSCHAR ch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvvline(y, x, MarshallArrayReadonly(ch), n), "mvvline");
        }

        public void vline(in INCursesSCHAR ch, int n)
        {
            NCursesException.Verify(this.Wrapper.vline(MarshallArrayReadonly(ch), n), "vline");
        }
    }
}
