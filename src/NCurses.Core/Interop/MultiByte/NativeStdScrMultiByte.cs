using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.MultiByte
{
    internal class NativeStdScrMultiByte<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
            : MultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>, 
            INativeStdScrMultiByte<TMultiByte, MultiByteCharString<TMultiByte, TWideChar, TSingleByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeStdScrMultiByte(IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> wrapper)
            : base(wrapper) { }

        public void add_wch(in TMultiByte wch)
        {
            NCursesException.Verify(Wrapper.add_wch(in wch), "add_wch");
        }

        public void add_wchnstr(in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr, int n)
        {
            NCursesException.Verify(Wrapper.add_wchnstr(in wchStr.GetPinnableReference(), n), "add_wchnstr");
        }

        public void add_wchstr(in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr)
        {
            NCursesException.Verify(Wrapper.add_wchstr(in wchStr.GetPinnableReference()), "add_wchstr");
        }

        public void bkgrnd(in TMultiByte wch)
        {
            NCursesException.Verify(Wrapper.bkgrnd(in wch), "bkgrnd");
        }

        public void bkgrndset(in TMultiByte wch)
        {
            Wrapper.bkgrndset(in wch);
        }

        public void border_set(in TMultiByte ls, in TMultiByte rs, in TMultiByte ts, in TMultiByte bs, in TMultiByte tl, in TMultiByte tr, in TMultiByte bl, in TMultiByte br)
        {
            NCursesException.Verify(this.Wrapper.border_set(
                in ls,
                in rs,
                in ts,
                in bs,
                in tl,
                in tr,
                in bl,
                in br), "border_set");
        }

        public void echo_wchar(in TMultiByte wch)
        {
            NCursesException.Verify(Wrapper.echo_wchar(in wch), "echo_wchar");
        }

        public void getbkgrnd(out TMultiByte wch)
        {
            wch = default;
            NCursesException.Verify(Wrapper.getbkgrnd(ref wch), "getbkgrnd");
        }

        public void hline_set(in TMultiByte wch, int n)
        {
            NCursesException.Verify(Wrapper.hline_set(in wch, n), "hline_set");
        }

        public void in_wch(out TMultiByte wch)
        {
            wch = default;
            NCursesException.Verify(Wrapper.in_wch(ref wch), "in_wch");
        }

        public void in_wchnstr(ref MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr, int n)
        {
            NCursesException.Verify(Wrapper.in_wchnstr(ref wchStr.GetPinnableReference(), n), "in_wchnstr");
        }

        public void in_wchstr(ref MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr)
        {
            NCursesException.Verify(Wrapper.in_wchstr(ref wchStr.GetPinnableReference()), "in_wchstr");
        }

        public void ins_wch(in TMultiByte wch)
        {
            NCursesException.Verify(Wrapper.ins_wch(in wch), "ins_wch");
        }

        public void mvadd_wch(int y, int x, in TMultiByte wch)
        {
            NCursesException.Verify(Wrapper.mvadd_wch(y, x, in wch), "mvadd_wch");
        }

        public void mvadd_wchnstr(int y, int x, in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr, int n)
        {
            NCursesException.Verify(Wrapper.mvadd_wchnstr(y, x, in wchStr.GetPinnableReference(), n), "mvadd_wchnstr");
        }

        public void mvadd_wchstr(int y, int x, in MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr)
        {
            NCursesException.Verify(Wrapper.mvadd_wchstr(y, x, in wchStr.GetPinnableReference()), "mvadd_wchstr");
        }

        public void mvhline_set(int y, int x, in TMultiByte wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvhline_set(y, x, in wch, n), "mvhline_set");
        }

        public void mvin_wch(int y, int x, out TMultiByte wch)
        {
            wch = default;
            NCursesException.Verify(this.Wrapper.mvin_wch(y, x, ref wch), "mvin_wch");
        }

        public void mvin_wchstr(int y, int x, ref MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr)
        {
            NCursesException.Verify(this.Wrapper.mvin_wchstr(y, x, ref wchStr.GetPinnableReference()), "mvin_wchnstr");
        }

        public void mvin_wchnstr(int y, int x, ref MultiByteCharString<TMultiByte, TWideChar, TSingleByte> wchStr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvin_wchnstr(y, x, ref wchStr.GetPinnableReference(), n), "mvin_wchnstr");
        }

        public void mvins_wch(int y, int x, in TMultiByte wch)
        {
            NCursesException.Verify(this.Wrapper.mvins_wch(y, x, in wch), "mvins_wch");
        }

        public void mvvline_set(int y, int x, in TMultiByte wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvvline_set(y, x, in wch, n), "mvvline_set");
        }

        public void vline_set(in TMultiByte wch, int n)
        {
            NCursesException.Verify(this.Wrapper.vline_set(in wch, n), "vline_set");
        }
    }
}
