using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
{
    internal interface INativeWindowMultiByte
    {
        void box_set(IntPtr window, in IMultiByteChar verch, in IMultiByteChar horch);
        void mvwadd_wch(IntPtr window, int y, int x, in IMultiByteChar wch);
        void mvwadd_wchnstr(IntPtr window, int y, int x, in IMultiByteCharString wchStr, int n);
        void mvwadd_wchstr(IntPtr window, int y, int x, in IMultiByteCharString wchStr);
        void mvwhline_set(IntPtr window, int y, int x, in IMultiByteChar wch, int n);
        void mvwin_wch(IntPtr window, int y, int x, out IMultiByteChar wch);
        void mvwin_wchnstr(IntPtr window, int y, int x, out IMultiByteCharString wchStr, int n);
        void mvwin_wchstr(IntPtr window, int y, int x, out IMultiByteCharString wchStr);
        void mvwins_wch(IntPtr window, int y, int x, in IMultiByteChar wch);
        void mvwvline_set(IntPtr window, int y, int x, in IMultiByteChar wch, int n);
        void wadd_wch(IntPtr window, in IMultiByteChar wch);
        void wadd_wchnstr(IntPtr window, in IMultiByteCharString wchStr, int n);
        void wadd_wchstr(IntPtr window, in IMultiByteCharString wchStr);
        void wbkgrnd(IntPtr window, in IMultiByteChar wch);
        void wbkgrndset(IntPtr window, in IMultiByteChar wch);
        void wborder_set(IntPtr window, in IMultiByteChar ls, in IMultiByteChar rs, in IMultiByteChar ts, in IMultiByteChar bs, in IMultiByteChar tl, 
            in IMultiByteChar tr, in IMultiByteChar bl, in IMultiByteChar br);
        void wecho_wchar(IntPtr window, in IMultiByteChar wch);
        void wgetbkgrnd(IntPtr window, out IMultiByteChar wch);
        void whline_set(IntPtr window, in IMultiByteChar wch, int n);
        void win_wch(IntPtr window, out IMultiByteChar wch);
        void win_wchnstr(IntPtr window, out IMultiByteCharString wchStr, int n);
        void win_wchstr(IntPtr window, out IMultiByteCharString wchStr);
        void wins_wch(IntPtr window, in IMultiByteChar wch);
        void wvline_set(IntPtr window, in IMultiByteChar wch, int n);
    }

    internal class NativeWindowMultiByte<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent> : MultiByteWrapper<TMultiByte, TMultiByteString, TSingleByte, TSingleByteString, TMouseEvent>, INativeWindowMultiByte
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TMultiByteString : unmanaged
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TSingleByteString : unmanaged
        where TMouseEvent : unmanaged, IMEVENT
    {
        public NativeWindowMultiByte()
        { }

        public void wadd_wch(IntPtr window, in IMultiByteChar wch)
        {
            NCursesException.Verify(this.Wrapper.wadd_wch(window, MarshallArrayReadonly(wch)), "wadd_wch");
        }

        public void box_set(IntPtr window, in IMultiByteChar verch, in IMultiByteChar horch)
        {
            NCursesException.Verify(this.Wrapper.box_set(
                window, 
                MarshallArrayReadonly(verch),
                MarshallArrayReadonly(horch)), "box_set");
        }

        public void mvwadd_wch(IntPtr window, int y, int x, in IMultiByteChar wch)
        {
            NCursesException.Verify(this.Wrapper.mvwadd_wch(window, y, x, MarshallArrayReadonly(wch)), "mvwadd_wch");
        }

        public void mvwadd_wchnstr(IntPtr window, int y, int x, in IMultiByteCharString wchStr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwadd_wchnstr(window, y, x, MarshallArrayReadonly(wchStr), n), "mvwadd_wchnstr");
        }

        public void mvwadd_wchstr(IntPtr window, int y, int x, in IMultiByteCharString wchStr)
        {
            NCursesException.Verify(this.Wrapper.mvwadd_wchstr(window, y, x, MarshallArrayReadonly(wchStr)), "mvwadd_wchstr");
        }

        public void mvwhline_set(IntPtr window, int y, int x, in IMultiByteChar wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwhline_set(window, y, x, MarshallArrayReadonly(wch), n), "mvwhline_set");
        }

        public void mvwin_wch(IntPtr window, int y, int x, out IMultiByteChar wch)
        {
            wch = new MultiByteChar<TMultiByte>('\0');
            NCursesException.Verify(Wrapper.mvwin_wch(window, y, x, ref MarshallArray(ref wch)), "mvwin_wch");
        }

        public void mvwin_wchnstr(IntPtr window, int y, int x, out IMultiByteCharString wchStr, int n)
        {
            wchStr = new MultiByteCharString<TMultiByte>(n);
            NCursesException.Verify(Wrapper.mvwin_wchnstr(window, y, x, ref MarshallArray(ref wchStr), n), "mvwin_wchnstr");
        }

        public void mvwin_wchstr(IntPtr window, int y, int x, out IMultiByteCharString wchStr)
        {
            wchStr = new MultiByteCharString<TMultiByte>(Constants.MAX_STRING_LENGTH);
            NCursesException.Verify(Wrapper.mvwin_wchstr(window, y, x, ref MarshallArray(ref wchStr)), "mvwin_wchstr");
        }

        public void mvwins_wch(IntPtr window, int y, int x, in IMultiByteChar wch)
        {
            NCursesException.Verify(this.Wrapper.mvwins_wch(window, y, x, MarshallArrayReadonly(wch)), "mvwins_wch");
        }

        public void mvwvline_set(IntPtr window, int y, int x, in IMultiByteChar wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwvline_set(window, y, x, MarshallArrayReadonly(wch), n), "mvwvline_set");
        }

        public void wadd_wchnstr(IntPtr window, in IMultiByteCharString wchStr, int n)
        {
            NCursesException.Verify(this.Wrapper.wadd_wchnstr(window, MarshallArrayReadonly(wchStr), n), "wadd_wchnstr");
        }

        public void wadd_wchstr(IntPtr window, in IMultiByteCharString wchStr)
        {
            NCursesException.Verify(this.Wrapper.wadd_wchstr(window, MarshallArrayReadonly(wchStr)), "wadd_wchstr");
        }

        public void wbkgrnd(IntPtr window, in IMultiByteChar wch)
        {
            NCursesException.Verify(this.Wrapper.wbkgrnd(window, MarshallArrayReadonly(wch)), "wbkgrnd");
        }

        public void wbkgrndset(IntPtr window, in IMultiByteChar wch)
        {
            this.Wrapper.wbkgrndset(window, MarshallArrayReadonly(wch));
        }

        public void wborder_set(IntPtr window, in IMultiByteChar ls, in IMultiByteChar rs, in IMultiByteChar ts, in IMultiByteChar bs, in IMultiByteChar tl, in IMultiByteChar tr, in IMultiByteChar bl, in IMultiByteChar br)
        {
            NCursesException.Verify(this.Wrapper.wborder_set(
                window,
                MarshallArrayReadonly(ls),
                MarshallArrayReadonly(rs),
                MarshallArrayReadonly(ts),
                MarshallArrayReadonly(bs),
                MarshallArrayReadonly(tl),
                MarshallArrayReadonly(tr),
                MarshallArrayReadonly(bl),
                MarshallArrayReadonly(br)), "wborder_set");
        }

        public void wecho_wchar(IntPtr window, in IMultiByteChar wch)
        {
            NCursesException.Verify(Wrapper.wecho_wchar(window, MarshallArrayReadonly(wch)), "wecho_wchar");
        }

        public void wgetbkgrnd(IntPtr window, out IMultiByteChar wch)
        {
            wch = new MultiByteChar<TMultiByte>('\0');
            NCursesException.Verify(Wrapper.wgetbkgrnd(window, ref MarshallArray(ref wch)), "wgetbkgrnd");
        }

        public void whline_set(IntPtr window, in IMultiByteChar wch, int n)
        {
            NCursesException.Verify(Wrapper.whline_set(window, MarshallArrayReadonly(wch), n), "whline_set");
        }

        public void win_wch(IntPtr window, out IMultiByteChar wch)
        {
            wch = new MultiByteChar<TMultiByte>('\0');
            NCursesException.Verify(Wrapper.win_wch(window, ref MarshallArray(ref wch)), "win_wch");
        }

        public void win_wchnstr(IntPtr window, out IMultiByteCharString wchStr, int n)
        {
            wchStr = new MultiByteCharString<TMultiByte>(n);
            NCursesException.Verify(Wrapper.win_wchnstr(window, ref MarshallArray(ref wchStr), n), "win_wchnstr");
        }

        public void win_wchstr(IntPtr window, out IMultiByteCharString wchStr)
        {
            //TODO: can overflow
            wchStr = new MultiByteCharString<TMultiByte>(Constants.MAX_STRING_LENGTH);
            NCursesException.Verify(Wrapper.win_wchstr(window, ref MarshallArray(ref wchStr)), "win_wchstr");
        }

        public void wins_wch(IntPtr window, in IMultiByteChar wch)
        {
            NCursesException.Verify(Wrapper.wins_wch(window, MarshallArrayReadonly(wch)), "wins_wch");
        }

        public void wvline_set(IntPtr window, in IMultiByteChar wch, int n)
        {
            NCursesException.Verify(this.Wrapper.wvline_set(window, MarshallArrayReadonly(wch), n), "wvline_set");
        }
    }
}
