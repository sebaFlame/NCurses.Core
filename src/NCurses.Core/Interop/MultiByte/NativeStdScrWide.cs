using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativeStdScrWide
    {
        void add_wch(in INCursesWCHAR wch);
        void add_wchnstr(in INCursesWCHARStr wchStr, int n);
        void add_wchstr(in INCursesWCHARStr wchStr);
        void bkgrnd(in INCursesWCHAR wch);
        void bkgrndset(in INCursesWCHAR wch);
        void border_set(in INCursesWCHAR ls, in INCursesWCHAR rs, in INCursesWCHAR ts, in INCursesWCHAR bs, in INCursesWCHAR tl, in INCursesWCHAR tr, in INCursesWCHAR bl, in INCursesWCHAR br);
        void echo_wchar(in INCursesWCHAR wch);
        void getbkgrnd(out INCursesWCHAR wch);
        void hline_set(in INCursesWCHAR wch, int n);
        void in_wch(out INCursesWCHAR wch);
        void in_wchnstr(out INCursesWCHARStr wchStr, int n);
        void in_wchstr(out INCursesWCHARStr wch);
        void ins_wch(in INCursesWCHAR wch);
        void mvadd_wch(int y, int x, in INCursesWCHAR wch);
        void mvadd_wchnstr(int y, int x, in INCursesWCHARStr wchStr, int n);
        void mvadd_wchstr(int y, int x, in INCursesWCHARStr wchStr);
        void mvhline_set(int y, int x, in INCursesWCHAR wch, int n);
        void mvin_wch(int y, int x, out INCursesWCHAR wch);
        void mvin_wchnstr(int y, int x, out INCursesWCHARStr wchStr, int n);
        void mvin_wchstr(int y, int x, out INCursesWCHARStr wchStr);
        void mvins_wch(int y, int x, in INCursesWCHAR wch);
        void mvvline_set(int y, int x, in INCursesWCHAR wch, int n);
        void vline_set(in INCursesWCHAR wch, int n);
    }

    public class NativeStdScrWide<TWide, TWideStr, TSmall, TSmallStr> : NativeWideBase<TWide, TWideStr, TSmall, TSmallStr>, INativeStdScrWide
        where TWide : unmanaged, INCursesWCHAR, IEquatable<TWide>
        where TWideStr : unmanaged
        where TSmall : unmanaged, INCursesSCHAR, IEquatable<TSmall>
        where TSmallStr : unmanaged
    {
        public NativeStdScrWide()
        { }

        public void add_wch(in INCursesWCHAR wch)
        {
            NCursesException.Verify(Wrapper.add_wch(MarshallArrayReadonly(wch)), "add_wch");
        }

        public void add_wchnstr(in INCursesWCHARStr wchStr, int n)
        {
            NCursesException.Verify(Wrapper.add_wchnstr(MarshallArrayReadonly(wchStr), n), "add_wchnstr");
        }

        public void add_wchstr(in INCursesWCHARStr wchStr)
        {
            NCursesException.Verify(Wrapper.add_wchstr(MarshallArrayReadonly(wchStr)), "add_wchstr");
        }

        public void bkgrnd(in INCursesWCHAR wch)
        {
            NCursesException.Verify(Wrapper.bkgrnd(MarshallArrayReadonly(wch)), "bkgrnd");
        }

        public void bkgrndset(in INCursesWCHAR wch)
        {
            Wrapper.bkgrndset(MarshallArrayReadonly(wch));
        }

        public void border_set(in INCursesWCHAR ls, in INCursesWCHAR rs, in INCursesWCHAR ts, in INCursesWCHAR bs, in INCursesWCHAR tl, in INCursesWCHAR tr, in INCursesWCHAR bl, in INCursesWCHAR br)
        {
            NCursesException.Verify(this.Wrapper.border_set(
                MarshallArrayReadonly(ls),
                MarshallArrayReadonly(rs),
                MarshallArrayReadonly(ts),
                MarshallArrayReadonly(bs),
                MarshallArrayReadonly(tl),
                MarshallArrayReadonly(tr),
                MarshallArrayReadonly(bl),
                MarshallArrayReadonly(br)), "border_set");
        }

        public void echo_wchar(in INCursesWCHAR wch)
        {
            NCursesException.Verify(Wrapper.echo_wchar(MarshallArrayReadonly(wch)), "echo_wchar");
        }

        public void getbkgrnd(out INCursesWCHAR wch)
        {
            wch = new NCursesWCHAR<TWide>('\0');
            NCursesException.Verify(Wrapper.getbkgrnd(ref MarshallArray(ref wch)), "getbkgrnd");
        }

        public void hline_set(in INCursesWCHAR wch, int n)
        {
            NCursesException.Verify(Wrapper.hline_set(MarshallArrayReadonly(wch), n), "hline_set");
        }

        public void in_wch(out INCursesWCHAR wch)
        {
            wch = new NCursesWCHAR<TWide>('\0');
            NCursesException.Verify(Wrapper.in_wch(ref MarshallArray(ref wch)), "in_wch");
        }

        public void in_wchnstr(out INCursesWCHARStr wchStr, int n)
        {
            wchStr = new NCursesWCHARStr<TWide>(n);
            NCursesException.Verify(Wrapper.in_wchnstr(ref MarshallArray(ref wchStr), n), "in_wchnstr");
        }

        public void in_wchstr(out INCursesWCHARStr wchStr)
        {
            //TODO: can overflow
            wchStr = new NCursesWCHARStr<TWide>(Constants.MAX_STRING_LENGTH);
            NCursesException.Verify(Wrapper.in_wchstr(ref MarshallArray(ref wchStr)), "in_wchstr");
        }

        public void ins_wch(in INCursesWCHAR wch)
        {
            NCursesException.Verify(Wrapper.ins_wch(MarshallArrayReadonly(wch)), "ins_wch");
        }

        public void mvadd_wch(int y, int x, in INCursesWCHAR wch)
        {
            NCursesException.Verify(Wrapper.mvadd_wch(y, x, MarshallArrayReadonly(wch)), "mvadd_wch");
        }

        public void mvadd_wchnstr(int y, int x, in INCursesWCHARStr wchStr, int n)
        {
            NCursesException.Verify(Wrapper.mvadd_wchnstr(y, x, MarshallArrayReadonly(wchStr), n), "mvadd_wchnstr");
        }

        public void mvadd_wchstr(int y, int x, in INCursesWCHARStr wchStr)
        {
            NCursesException.Verify(Wrapper.mvadd_wchstr(y, x, MarshallArrayReadonly(wchStr)), "mvadd_wchstr");
        }

        public void mvhline_set(int y, int x, in INCursesWCHAR wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvhline_set(y, x, MarshallArrayReadonly(wch), n), "mvhline_set");
        }

        public void mvin_wch(int y, int x, out INCursesWCHAR wch)
        {
            wch = new NCursesWCHAR<TWide>('\0');
            NCursesException.Verify(this.Wrapper.mvin_wch(y, x, ref MarshallArray(ref wch)), "mvin_wch");
        }

        public void mvin_wchstr(int y, int x, out INCursesWCHARStr wchStr)
        {
            //TODO: can overflow
            wchStr = new NCursesWCHARStr<TWide>(Constants.MAX_STRING_LENGTH);
            NCursesException.Verify(this.Wrapper.mvin_wchstr(y, x, ref MarshallArray(ref wchStr)), "mvin_wchnstr");
        }

        public void mvin_wchnstr(int y, int x, out INCursesWCHARStr wchStr, int n)
        {
            wchStr = new NCursesWCHARStr<TWide>(n);
            NCursesException.Verify(this.Wrapper.mvin_wchnstr(y, x, ref MarshallArray(ref wchStr), n), "mvin_wchnstr");
        }

        public void mvins_wch(int y, int x, in INCursesWCHAR wch)
        {
            NCursesException.Verify(this.Wrapper.mvins_wch(y, x, MarshallArrayReadonly(wch)), "mvins_wch");
        }

        public void mvvline_set(int y, int x, in INCursesWCHAR wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvvline_set(y, x, MarshallArrayReadonly(wch), n), "mvvline_set");
        }

        public void vline_set(in INCursesWCHAR wch, int n)
        {
            NCursesException.Verify(this.Wrapper.vline_set(MarshallArrayReadonly(wch), n), "vline_set");
        }
    }
}
