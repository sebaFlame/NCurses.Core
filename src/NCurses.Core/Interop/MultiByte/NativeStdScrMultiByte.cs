using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.MultiByte
{
    public interface INativeStdScrMultiByte
    {
        void add_wch(in IMultiByteChar wch);
        void add_wchnstr(in IMultiByteCharString wchStr, int n);
        void add_wchstr(in IMultiByteCharString wchStr);
        void bkgrnd(in IMultiByteChar wch);
        void bkgrndset(in IMultiByteChar wch);
        void border_set(in IMultiByteChar ls, in IMultiByteChar rs, in IMultiByteChar ts, in IMultiByteChar bs, in IMultiByteChar tl, in IMultiByteChar tr, in IMultiByteChar bl, in IMultiByteChar br);
        void echo_wchar(in IMultiByteChar wch);
        void getbkgrnd(out IMultiByteChar wch);
        void hline_set(in IMultiByteChar wch, int n);
        void in_wch(out IMultiByteChar wch);
        void in_wchnstr(out IMultiByteCharString wchStr, int n);
        void in_wchstr(out IMultiByteCharString wch);
        void ins_wch(in IMultiByteChar wch);
        void mvadd_wch(int y, int x, in IMultiByteChar wch);
        void mvadd_wchnstr(int y, int x, in IMultiByteCharString wchStr, int n);
        void mvadd_wchstr(int y, int x, in IMultiByteCharString wchStr);
        void mvhline_set(int y, int x, in IMultiByteChar wch, int n);
        void mvin_wch(int y, int x, out IMultiByteChar wch);
        void mvin_wchnstr(int y, int x, out IMultiByteCharString wchStr, int n);
        void mvin_wchstr(int y, int x, out IMultiByteCharString wchStr);
        void mvins_wch(int y, int x, in IMultiByteChar wch);
        void mvvline_set(int y, int x, in IMultiByteChar wch, int n);
        void vline_set(in IMultiByteChar wch, int n);
    }

    public class NativeStdScrMultiByte<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent> : MultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent>, INativeStdScrMultiByte
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TMultiByteString : unmanaged
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public NativeStdScrMultiByte()
        { }

        public void add_wch(in IMultiByteChar wch)
        {
            NCursesException.Verify(Wrapper.add_wch(MarshallArrayReadonly(wch)), "add_wch");
        }

        public void add_wchnstr(in IMultiByteCharString wchStr, int n)
        {
            NCursesException.Verify(Wrapper.add_wchnstr(MarshallArrayReadonly(wchStr), n), "add_wchnstr");
        }

        public void add_wchstr(in IMultiByteCharString wchStr)
        {
            NCursesException.Verify(Wrapper.add_wchstr(MarshallArrayReadonly(wchStr)), "add_wchstr");
        }

        public void bkgrnd(in IMultiByteChar wch)
        {
            NCursesException.Verify(Wrapper.bkgrnd(MarshallArrayReadonly(wch)), "bkgrnd");
        }

        public void bkgrndset(in IMultiByteChar wch)
        {
            Wrapper.bkgrndset(MarshallArrayReadonly(wch));
        }

        public void border_set(in IMultiByteChar ls, in IMultiByteChar rs, in IMultiByteChar ts, in IMultiByteChar bs, in IMultiByteChar tl, in IMultiByteChar tr, in IMultiByteChar bl, in IMultiByteChar br)
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

        public void echo_wchar(in IMultiByteChar wch)
        {
            NCursesException.Verify(Wrapper.echo_wchar(MarshallArrayReadonly(wch)), "echo_wchar");
        }

        public void getbkgrnd(out IMultiByteChar wch)
        {
            wch = new MultiByteChar<TMultiByte>('\0');
            NCursesException.Verify(Wrapper.getbkgrnd(ref MarshallArray(ref wch)), "getbkgrnd");
        }

        public void hline_set(in IMultiByteChar wch, int n)
        {
            NCursesException.Verify(Wrapper.hline_set(MarshallArrayReadonly(wch), n), "hline_set");
        }

        public void in_wch(out IMultiByteChar wch)
        {
            wch = new MultiByteChar<TMultiByte>('\0');
            NCursesException.Verify(Wrapper.in_wch(ref MarshallArray(ref wch)), "in_wch");
        }

        public void in_wchnstr(out IMultiByteCharString wchStr, int n)
        {
            wchStr = new MultiByteCharString<TMultiByte>(n);
            NCursesException.Verify(Wrapper.in_wchnstr(ref MarshallArray(ref wchStr), n), "in_wchnstr");
        }

        public void in_wchstr(out IMultiByteCharString wchStr)
        {
            //TODO: can overflow
            wchStr = new MultiByteCharString<TMultiByte>(Constants.MAX_STRING_LENGTH);
            NCursesException.Verify(Wrapper.in_wchstr(ref MarshallArray(ref wchStr)), "in_wchstr");
        }

        public void ins_wch(in IMultiByteChar wch)
        {
            NCursesException.Verify(Wrapper.ins_wch(MarshallArrayReadonly(wch)), "ins_wch");
        }

        public void mvadd_wch(int y, int x, in IMultiByteChar wch)
        {
            NCursesException.Verify(Wrapper.mvadd_wch(y, x, MarshallArrayReadonly(wch)), "mvadd_wch");
        }

        public void mvadd_wchnstr(int y, int x, in IMultiByteCharString wchStr, int n)
        {
            NCursesException.Verify(Wrapper.mvadd_wchnstr(y, x, MarshallArrayReadonly(wchStr), n), "mvadd_wchnstr");
        }

        public void mvadd_wchstr(int y, int x, in IMultiByteCharString wchStr)
        {
            NCursesException.Verify(Wrapper.mvadd_wchstr(y, x, MarshallArrayReadonly(wchStr)), "mvadd_wchstr");
        }

        public void mvhline_set(int y, int x, in IMultiByteChar wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvhline_set(y, x, MarshallArrayReadonly(wch), n), "mvhline_set");
        }

        public void mvin_wch(int y, int x, out IMultiByteChar wch)
        {
            wch = new MultiByteChar<TMultiByte>('\0');
            NCursesException.Verify(this.Wrapper.mvin_wch(y, x, ref MarshallArray(ref wch)), "mvin_wch");
        }

        public void mvin_wchstr(int y, int x, out IMultiByteCharString wchStr)
        {
            //TODO: can overflow
            wchStr = new MultiByteCharString<TMultiByte>(Constants.MAX_STRING_LENGTH);
            NCursesException.Verify(this.Wrapper.mvin_wchstr(y, x, ref MarshallArray(ref wchStr)), "mvin_wchnstr");
        }

        public void mvin_wchnstr(int y, int x, out IMultiByteCharString wchStr, int n)
        {
            wchStr = new MultiByteCharString<TMultiByte>(n);
            NCursesException.Verify(this.Wrapper.mvin_wchnstr(y, x, ref MarshallArray(ref wchStr), n), "mvin_wchnstr");
        }

        public void mvins_wch(int y, int x, in IMultiByteChar wch)
        {
            NCursesException.Verify(this.Wrapper.mvins_wch(y, x, MarshallArrayReadonly(wch)), "mvins_wch");
        }

        public void mvvline_set(int y, int x, in IMultiByteChar wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvvline_set(y, x, MarshallArrayReadonly(wch), n), "mvvline_set");
        }

        public void vline_set(in IMultiByteChar wch, int n)
        {
            NCursesException.Verify(this.Wrapper.vline_set(MarshallArrayReadonly(wch), n), "vline_set");
        }
    }
}
