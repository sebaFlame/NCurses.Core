using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Small;

namespace NCurses.Core.Interop.Wide
{
    internal interface INativeWindowWide
    {
        void box_set(IntPtr window, in INCursesWCHAR verch, in INCursesWCHAR horch);
        void mvwadd_wch(IntPtr window, int y, int x, in INCursesWCHAR wch);
        void mvwadd_wchnstr(IntPtr window, int y, int x, in INCursesWCHARStr wchStr, int n);
        void mvwadd_wchstr(IntPtr window, int y, int x, in INCursesWCHARStr wchStr);
        void mvwhline_set(IntPtr window, int y, int x, in INCursesWCHAR wch, int n);
        void mvwin_wch(IntPtr window, int y, int x, out INCursesWCHAR wch);
        void mvwin_wchnstr(IntPtr window, int y, int x, out INCursesWCHARStr wchStr, int n);
        void mvwin_wchstr(IntPtr window, int y, int x, out INCursesWCHARStr wchStr);
        void mvwins_wch(IntPtr window, int y, int x, in INCursesWCHAR wch);
        void mvwvline_set(IntPtr window, int y, int x, in INCursesWCHAR wch, int n);
        void wadd_wch(IntPtr window, in INCursesWCHAR wch);
        void wadd_wchnstr(IntPtr window, in INCursesWCHARStr wchStr, int n);
        void wadd_wchstr(IntPtr window, in INCursesWCHARStr wchStr);
        void wbkgrnd(IntPtr window, in INCursesWCHAR wch);
        void wbkgrndset(IntPtr window, in INCursesWCHAR wch);
        void wborder_set(IntPtr window, in INCursesWCHAR ls, in INCursesWCHAR rs, in INCursesWCHAR ts, in INCursesWCHAR bs, in INCursesWCHAR tl, 
            in INCursesWCHAR tr, in INCursesWCHAR bl, in INCursesWCHAR br);
        void wecho_wchar(IntPtr window, in INCursesWCHAR wch);
        void wgetbkgrnd(IntPtr window, out INCursesWCHAR wch);
        void whline_set(IntPtr window, in INCursesWCHAR wch, int n);
        void win_wch(IntPtr window, out INCursesWCHAR wch);
        void win_wchnstr(IntPtr window, out INCursesWCHARStr wchStr, int n);
        void win_wchstr(IntPtr window, out INCursesWCHARStr wchStr);
        void wins_wch(IntPtr window, in INCursesWCHAR wch);
        void wvline_set(IntPtr window, in INCursesWCHAR wch, int n);
    }

    internal class NativeWindowWide<TWide, TWideStr, TSMall, TSmallStr> : NativeWideBase<TWide, TWideStr, TSMall, TSmallStr>, INativeWindowWide
        where TWide : unmanaged, INCursesWCHAR
        where TWideStr : unmanaged
        where TSMall : unmanaged, INCursesSCHAR
        where TSmallStr : unmanaged
    {
        public NativeWindowWide()
        { }

        public void wadd_wch(IntPtr window, in INCursesWCHAR wch)
        {
            NCursesException.Verify(this.Wrapper.wadd_wch(window, MarshallArrayReadonly(wch)), "wadd_wch");
        }

        public void box_set(IntPtr window, in INCursesWCHAR verch, in INCursesWCHAR horch)
        {
            NCursesException.Verify(this.Wrapper.box_set(
                window, 
                MarshallArrayReadonly(verch),
                MarshallArrayReadonly(horch)), "box_set");
        }

        public void mvwadd_wch(IntPtr window, int y, int x, in INCursesWCHAR wch)
        {
            NCursesException.Verify(this.Wrapper.mvwadd_wch(window, y, x, MarshallArrayReadonly(wch)), "mvwadd_wch");
        }

        public void mvwadd_wchnstr(IntPtr window, int y, int x, in INCursesWCHARStr wchStr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwadd_wchnstr(window, y, x, MarshallArrayReadonly(wchStr), n), "mvwadd_wchnstr");
        }

        public void mvwadd_wchstr(IntPtr window, int y, int x, in INCursesWCHARStr wchStr)
        {
            NCursesException.Verify(this.Wrapper.mvwadd_wchstr(window, y, x, MarshallArrayReadonly(wchStr)), "mvwadd_wchstr");
        }

        public void mvwhline_set(IntPtr window, int y, int x, in INCursesWCHAR wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwhline_set(window, y, x, MarshallArrayReadonly(wch), n), "mvwhline_set");
        }

        public void mvwin_wch(IntPtr window, int y, int x, out INCursesWCHAR wch)
        {
            wch = new NCursesWCHAR<TWide>('\0');
            NCursesException.Verify(Wrapper.mvwin_wch(window, y, x, ref MarshallArray(ref wch)), "mvwin_wch");
        }

        public void mvwin_wchnstr(IntPtr window, int y, int x, out INCursesWCHARStr wchStr, int n)
        {
            wchStr = new NCursesWCHARStr<TWide>(n);
            NCursesException.Verify(Wrapper.mvwin_wchnstr(window, y, x, ref MarshallArray(ref wchStr), n), "mvwin_wchnstr");
        }

        public void mvwin_wchstr(IntPtr window, int y, int x, out INCursesWCHARStr wchStr)
        {
            wchStr = new NCursesWCHARStr<TWide>(1024);
            NCursesException.Verify(Wrapper.mvwin_wchstr(window, y, x, ref MarshallArray(ref wchStr)), "mvwin_wchstr");
        }

        public void mvwins_wch(IntPtr window, int y, int x, in INCursesWCHAR wch)
        {
            NCursesException.Verify(this.Wrapper.mvwins_wch(window, y, x, MarshallArrayReadonly(wch)), "mvwins_wch");
        }

        public void mvwvline_set(IntPtr window, int y, int x, in INCursesWCHAR wch, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwvline_set(window, y, x, MarshallArrayReadonly(wch), n), "mvwvline_set");
        }

        public void wadd_wchnstr(IntPtr window, in INCursesWCHARStr wchStr, int n)
        {
            NCursesException.Verify(this.Wrapper.wadd_wchnstr(window, MarshallArrayReadonly(wchStr), n), "wadd_wchnstr");
        }

        public void wadd_wchstr(IntPtr window, in INCursesWCHARStr wchStr)
        {
            NCursesException.Verify(this.Wrapper.wadd_wchstr(window, MarshallArrayReadonly(wchStr)), "wadd_wchstr");
        }

        public void wbkgrnd(IntPtr window, in INCursesWCHAR wch)
        {
            NCursesException.Verify(this.Wrapper.wbkgrnd(window, MarshallArrayReadonly(wch)), "wbkgrnd");
        }

        public void wbkgrndset(IntPtr window, in INCursesWCHAR wch)
        {
            this.Wrapper.wbkgrndset(window, MarshallArrayReadonly(wch));
        }

        public void wborder_set(IntPtr window, in INCursesWCHAR ls, in INCursesWCHAR rs, in INCursesWCHAR ts, in INCursesWCHAR bs, in INCursesWCHAR tl, in INCursesWCHAR tr, in INCursesWCHAR bl, in INCursesWCHAR br)
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

        public void wecho_wchar(IntPtr window, in INCursesWCHAR wch)
        {
            NCursesException.Verify(Wrapper.wecho_wchar(window, MarshallArrayReadonly(wch)), "wecho_wchar");
        }

        public void wgetbkgrnd(IntPtr window, out INCursesWCHAR wch)
        {
            wch = new NCursesWCHAR<TWide>('\0');
            NCursesException.Verify(Wrapper.wgetbkgrnd(window, ref MarshallArray(ref wch)), "wgetbkgrnd");
        }

        public void whline_set(IntPtr window, in INCursesWCHAR wch, int n)
        {
            NCursesException.Verify(Wrapper.whline_set(window, MarshallArrayReadonly(wch), n), "whline_set");
        }

        public void win_wch(IntPtr window, out INCursesWCHAR wch)
        {
            wch = new NCursesWCHAR<TWide>('\0');
            NCursesException.Verify(Wrapper.win_wch(window, ref MarshallArray(ref wch)), "win_wch");
        }

        public void win_wchnstr(IntPtr window, out INCursesWCHARStr wchStr, int n)
        {
            wchStr = new NCursesWCHARStr<TWide>(n);
            NCursesException.Verify(Wrapper.win_wchnstr(window, ref MarshallArray(ref wchStr), n), "win_wchnstr");
        }

        public void win_wchstr(IntPtr window, out INCursesWCHARStr wchStr)
        {
            //TODO: can overflow
            wchStr = new NCursesWCHARStr<TWide>(1024);
            NCursesException.Verify(Wrapper.win_wchstr(window, ref MarshallArray(ref wchStr)), "win_wchstr");
        }

        public void wins_wch(IntPtr window, in INCursesWCHAR wch)
        {
            NCursesException.Verify(Wrapper.wins_wch(window, MarshallArrayReadonly(wch)), "wins_wch");
        }

        public void wvline_set(IntPtr window, in INCursesWCHAR wch, int n)
        {
            NCursesException.Verify(this.Wrapper.wvline_set(window, MarshallArrayReadonly(wch), n), "wvline_set");
        }
    }
}
