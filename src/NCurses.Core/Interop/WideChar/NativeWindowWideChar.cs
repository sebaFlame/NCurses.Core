using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.Wrappers;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;

namespace NCurses.Core.Interop.WideChar
{
    internal class NativeWindowWideChar<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
            : WideCharWrapper<TWideChar, TChar>, 
            INativeWindowWideChar<TWideChar, WideCharString<TWideChar>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeWindowWideChar(IWideCharWrapper<TWideChar, TChar> wrapper)
            : base(wrapper) { }

        public void mvwaddnwstr(WindowBaseSafeHandle window, int y, int x, in WideCharString<TWideChar> wstr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwaddnwstr(window, y, x, in wstr.GetPinnableReference(), n), "mvwaddnwstr");
        }

        public void mvwaddwstr(WindowBaseSafeHandle window, int y, int x, in WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(this.Wrapper.mvwaddwstr(window, y, x, in wstr.GetPinnableReference()), "mvwaddwstr");
        }

        public bool mvwget_wch(WindowBaseSafeHandle window, int y, int x, out TWideChar wch, out Key key)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return VerifyInput(
                    "mvwget_wch-mvwgetch",
                    NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal.mvwgetch(window, y, x),
                    NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal.is_keypad(window),
                    NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal.is_nodelay(window),
                    out wch,
                    out key);
            }

            int wc = 0;

            return VerifyInput("mvwget_wch",
                this.Wrapper.mvwget_wch(window, y, x, ref wc),
                NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal.is_nodelay(window),
                in wc,
                out wch,
                out key);
        }

        public void mvwget_wstr(WindowBaseSafeHandle window, int y, int x, ref WideCharString<TWideChar> wstr)
        {
            Span<int> iSpan = MemoryMarshal.Cast<TWideChar, int>(wstr.CharSpan);
            NCursesException.Verify(this.Wrapper.mvwget_wch(window, y, x, ref iSpan.GetPinnableReference()), "mvwget_wch");
        }

        public void mvwgetn_wstr(WindowBaseSafeHandle window, int y, int x, ref WideCharString<TWideChar> wstr, int n)
        {
            Span<int> iSpan = MemoryMarshal.Cast<TWideChar, int>(wstr.CharSpan);
            NCursesException.Verify(this.Wrapper.mvwgetn_wstr(window, y, x, ref iSpan.GetPinnableReference(), n), "mvwgetn_wstr");
        }

        public void mvwinnwstr(WindowBaseSafeHandle window, int y, int x, ref WideCharString<TWideChar> wstr, int n, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.mvwinnwstr(window, y, x, ref wstr.GetPinnableReference(), n), "mvwinnwstr");
        }

        public void mvwins_nwstr(WindowBaseSafeHandle window, int y, int x, in WideCharString<TWideChar> wstr, int n)
        {
            NCursesException.Verify(this.Wrapper.mvwins_nwstr(window, y, x, in wstr.GetPinnableReference(), n), "mvwins_nwstr");
        }

        public void mvwins_wstr(WindowBaseSafeHandle window, int y, int x, in WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(this.Wrapper.mvwins_wstr(window, y, x, in wstr.GetPinnableReference()), "mvwins_wstr");
        }

        public void mvwinwstr(WindowBaseSafeHandle window, int y, int x, ref WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(this.Wrapper.mvwinwstr(window, y, x, ref wstr.GetPinnableReference()), "mvwinwstr");
        }

        public void waddnwstr(WindowBaseSafeHandle window, in WideCharString<TWideChar> wstr, int n)
        {
            NCursesException.Verify(this.Wrapper.waddnwstr(window, in wstr.GetPinnableReference(), n), "waddnwstr");
        }

        public void waddwstr(WindowBaseSafeHandle window, in WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(this.Wrapper.waddwstr(window, in wstr.GetPinnableReference()), "waddwstr");
        }

        public bool wget_wch(WindowBaseSafeHandle window, out TWideChar wch, out Key key)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return VerifyInput(
                    "wget_wch-wgetch",
                    NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal.wgetch(window),
                    NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal.is_keypad(window),
                    NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal.is_nodelay(window),
                    out wch,
                    out key);
            }

            int wc = 0;

            return VerifyInput("wget_wch",
                this.Wrapper.wget_wch(window, ref wc),
                NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal.is_nodelay(window),
                in wc,
                out wch,
                out key);
        }

        public void wget_wstr(WindowBaseSafeHandle window, ref WideCharString<TWideChar> wstr)
        {
            Span<int> iSpan = MemoryMarshal.Cast<TWideChar, int>(wstr.CharSpan);
            NCursesException.Verify(this.Wrapper.wget_wstr(window, ref iSpan.GetPinnableReference()), "wget_wstr");
        }

        public void wgetn_wstr(WindowBaseSafeHandle window, ref WideCharString<TWideChar> wstr, int n)
        {
            Span<int> iSpan = MemoryMarshal.Cast<TWideChar, int>(wstr.CharSpan);
            NCursesException.Verify(this.Wrapper.wgetn_wstr(window, ref iSpan.GetPinnableReference(), n), "wgetn_wstr");
        }

        public void winnwstr(WindowBaseSafeHandle window, ref WideCharString<TWideChar> wstr, int count, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.winnwstr(window, ref wstr.GetPinnableReference(), count), "winnwstr");
        }

        public void winwstr(WindowBaseSafeHandle window, ref WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(this.Wrapper.winwstr(window, ref wstr.GetPinnableReference()), "winwstr");
        }

        public void wins_nwstr(WindowBaseSafeHandle window, in WideCharString<TWideChar> wstr, int n)
        {
            NCursesException.Verify(Wrapper.wins_nwstr(window, in wstr.GetPinnableReference(), n), "wins_nwstr");
        }

        public void wins_wstr(WindowBaseSafeHandle window, in WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(Wrapper.wins_wstr(window, in wstr.GetPinnableReference()), "wins_wstr");
        }
    }
}
