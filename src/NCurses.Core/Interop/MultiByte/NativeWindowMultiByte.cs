using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SingleByte;

namespace NCurses.Core.Interop.MultiByte
{
    internal class NativeWindowMultiByte<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> 
            : MultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>,
            INativeWindowMultiByte<TMultiByte, MultiByteCharString<TMultiByte>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeWindowMultiByte(IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar> wrapper)
            : base(wrapper) { }

        public void wadd_wch(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            NCursesException.Verify(this.Wrapper.wadd_wch(window, in wch), "wadd_wch");
        }

        public void box_set(WindowBaseSafeHandle window, in TMultiByte verch, in TMultiByte horch)
        {
            NCursesException.Verify(this.Wrapper.box_set(
                window, 
                verch,
                horch), "box_set");
        }

        public void mvwadd_wch(WindowBaseSafeHandle window, int y, int x, in TMultiByte wch)
        {
            NCursesException.Verify(this.Wrapper.mvwadd_wch(window, y, x, in wch), "mvwadd_wch");
        }

        public void mvwadd_wchnstr(WindowBaseSafeHandle window, int y, int x, in MultiByteCharString<TMultiByte> wchStr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwadd_wchnstr(window, y, x, in wchStr.GetPinnableReference(), n), "mvwadd_wchnstr");
        }

        public void mvwadd_wchstr(WindowBaseSafeHandle window, int y, int x, in MultiByteCharString<TMultiByte> wchStr)
        {
            NCursesException.Verify(this.Wrapper.mvwadd_wchstr(window, y, x, in wchStr.GetPinnableReference()), "mvwadd_wchstr");
        }

        public void mvwhline_set(WindowBaseSafeHandle window, int y, int x, in TMultiByte wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwhline_set(window, y, x, in wch, n), "mvwhline_set");
        }

        public void mvwin_wch(WindowBaseSafeHandle window, int y, int x, out TMultiByte wch)
        {
            wch = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            NCursesException.Verify(Wrapper.mvwin_wch(window, y, x, ref wch), "mvwin_wch");
        }

        public void mvwin_wchnstr(WindowBaseSafeHandle window, int y, int x, ref MultiByteCharString<TMultiByte> wchStr, int n)
        {
            NCursesException.Verify(Wrapper.mvwin_wchnstr(window, y, x, ref wchStr.GetPinnableReference(), n), "mvwin_wchnstr");
        }

        public void mvwin_wchstr(WindowBaseSafeHandle window, int y, int x, ref MultiByteCharString<TMultiByte> wchStr)
        {
            NCursesException.Verify(Wrapper.mvwin_wchstr(window, y, x, ref wchStr.GetPinnableReference()), "mvwin_wchstr");
        }

        public void mvwins_wch(WindowBaseSafeHandle window, int y, int x, in TMultiByte wch)
        {
            NCursesException.Verify(this.Wrapper.mvwins_wch(window, y, x, in wch), "mvwins_wch");
        }

        public void mvwvline_set(WindowBaseSafeHandle window, int y, int x, in TMultiByte wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwvline_set(window, y, x, in wch, n), "mvwvline_set");
        }

        public void wadd_wchnstr(WindowBaseSafeHandle window, in MultiByteCharString<TMultiByte> wchStr, int n)
        {
            NCursesException.Verify(this.Wrapper.wadd_wchnstr(window, in wchStr.GetPinnableReference(), n), "wadd_wchnstr");
        }

        public void wadd_wchstr(WindowBaseSafeHandle window, in MultiByteCharString<TMultiByte> wchStr)
        {
            NCursesException.Verify(this.Wrapper.wadd_wchstr(window, in wchStr.GetPinnableReference()), "wadd_wchstr");
        }

        public void wbkgrnd(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            NCursesException.Verify(this.Wrapper.wbkgrnd(window, in wch), "wbkgrnd");
        }

        public void wbkgrndset(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            this.Wrapper.wbkgrndset(window, in wch);
        }

        public void wborder_set(WindowBaseSafeHandle window, in TMultiByte ls, in TMultiByte rs, in TMultiByte ts, in TMultiByte bs, in TMultiByte tl, in TMultiByte tr, in TMultiByte bl, in TMultiByte br)
        {
            NCursesException.Verify(this.Wrapper.wborder_set(
                window,
                in ls,
                in rs,
                in ts,
                in bs,
                in tl,
                in tr,
                in bl,
                in br), "wborder_set");
        }

        public void wecho_wchar(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            NCursesException.Verify(Wrapper.wecho_wchar(window, in wch), "wecho_wchar");
        }

        public void wgetbkgrnd(WindowBaseSafeHandle window, out TMultiByte wch)
        {
            wch = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            NCursesException.Verify(Wrapper.wgetbkgrnd(window, ref wch), "wgetbkgrnd");
        }

        public void whline_set(WindowBaseSafeHandle window, in TMultiByte wch, int n)
        {
            NCursesException.Verify(Wrapper.whline_set(window, in wch, n), "whline_set");
        }

        public void win_wch(WindowBaseSafeHandle window, out TMultiByte wch)
        {
            wch = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeEmptyCharInternal();
            NCursesException.Verify(Wrapper.win_wch(window, ref wch), "win_wch");
        }

        public void win_wchnstr(WindowBaseSafeHandle window, ref MultiByteCharString<TMultiByte> wchStr, int n)
        {
            NCursesException.Verify(Wrapper.win_wchnstr(window, ref wchStr.GetPinnableReference(), n), "win_wchnstr");
        }

        public void win_wchstr(WindowBaseSafeHandle window, ref MultiByteCharString<TMultiByte> wchStr)
        {
            NCursesException.Verify(Wrapper.win_wchstr(window, ref wchStr.GetPinnableReference()), "win_wchstr");
        }

        public void wins_wch(WindowBaseSafeHandle window, in TMultiByte wch)
        {
            NCursesException.Verify(Wrapper.wins_wch(window, in wch), "wins_wch");
        }

        public void wvline_set(WindowBaseSafeHandle window, in TMultiByte wch, int n)
        {
            NCursesException.Verify(this.Wrapper.wvline_set(window, in wch, n), "wvline_set");
        }
    }
}
