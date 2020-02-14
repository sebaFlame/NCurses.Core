using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Buffers;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.Wrappers;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.WideChar
{
    internal class NativeStdScrWideChar<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
            : WideCharWrapper<TWideChar, TChar>, 
            INativeStdScrWideChar<TWideChar, WideCharString<TWideChar>>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal NativeStdScrWideChar(IWideCharWrapper<TWideChar, TChar> wrapper)
            : base(wrapper) { }

        public void addnwstr(in WideCharString<TWideChar> wstr, int n)
        {
            NCursesException.Verify(this.Wrapper.addnwstr(in wstr.GetPinnableReference(), n), "addnwstr");
        }

        public void addwstr(in WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(Wrapper.addwstr(in wstr.GetPinnableReference()), "addwstr");
        }

        public void innwstr(ref WideCharString<TWideChar> wstr, int count, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.innwstr(ref wstr.GetPinnableReference(), count), "innwstr");
        }

        public void ins_nwstr(in WideCharString<TWideChar> wstr, int n)
        {
            NCursesException.Verify(Wrapper.ins_nwstr(in wstr.GetPinnableReference(), n), "ins_nwstr");
        }

        public void ins_wstr(in WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(Wrapper.ins_wstr(in wstr.GetPinnableReference()), "ins_wstr");
        }

        public void inwstr(ref WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(this.Wrapper.inwstr(ref wstr.GetPinnableReference()), "inwstr");
        }

        /// <summary>
        /// read a character
        /// </summary>
        /// <param name="wch">the read wide character</param>
        /// <param name="key">the read function key</param>
        /// Windows only supports extended ASCII input!
        /// <returns>returns TRUE if a funtion key has been pressed</returns>
        public bool get_wch(out TWideChar wch, out Key key)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return VerifyInput(
                    "get_wch-getch",
                    NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.StdScrInternal.getch(),
                    NCurses.StdScr.KeyPad,
                    out wch,
                    out key);
            }

            int wc = 0;

            return VerifyInput(
                "get_wch", 
                this.Wrapper.get_wch(ref wc),
                in wc,
                out wch, 
                out key);
        }

        public void get_wstr(ref WideCharString<TWideChar> wstr)
        {
            Span<int> iSpan = MemoryMarshal.Cast<TWideChar, int>(wstr.CharSpan);
            NCursesException.Verify(this.Wrapper.get_wstr(ref iSpan.GetPinnableReference()), "get_wstr");
        }

        public void getn_wstr(ref WideCharString<TWideChar> wstr, int n)
        {
            Span<int> iSpan = MemoryMarshal.Cast<TWideChar, int>(wstr.CharSpan);
            NCursesException.Verify(this.Wrapper.get_wstr(ref iSpan.GetPinnableReference()), "get_wstr");
        }

        public void mvaddnwstr(int y, int x, in WideCharString<TWideChar> wstr, int n)
        {
            NCursesException.Verify(Wrapper.mvaddnwstr(y, x, in wstr.GetPinnableReference(), n), "mvaddnwstr");
        }

        public void mvaddwstr(int y, int x, in WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(Wrapper.mvaddwstr(y, x, in wstr.GetPinnableReference()), "mvaddwstr");
        }

        /// <summary>
        /// move cursor and read a character
        /// </summary>
        /// <param name="y">the line to move the cursor to</param>
        /// <param name="x">the column to move the cursor to</param>
        /// <param name="wch">the read wide character</param>
        /// <param name="key">the read function key</param>
        /// <returns>returns TRUE if a funtion key has been pressed</returns>
        public bool mvget_wch(int y, int x, out TWideChar wch, out Key key)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return VerifyInput(
                    "mvget_wch-mvgetch",
                    NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.StdScrInternal.mvgetch(y, x),
                    NCurses.StdScr.KeyPad,
                    out wch,
                    out key);
            }

            int wc = 0;

            return VerifyInput(
                "mvget_wch", 
                this.Wrapper.mvget_wch(y, x, ref wc),
                in wc,
                out wch,
                out key);
        }

        public void mvget_wstr(int y, int x, ref WideCharString<TWideChar> wstr)
        {
            Span<int> iSpan = MemoryMarshal.Cast<TWideChar, int>(wstr.CharSpan);
            NCursesException.Verify(this.Wrapper.mvget_wstr(y, x, ref iSpan.GetPinnableReference()), "mvget_wstr");
        }

        //NULL TERMINATOR!!
        public void mvgetn_wstr(int y, int x, ref WideCharString<TWideChar> wstr, int n)
        {
            Span<int> iSpan = MemoryMarshal.Cast<TWideChar, int>(wstr.CharSpan);
            NCursesException.Verify(this.Wrapper.mvgetn_wstr(y, x, ref iSpan.GetPinnableReference(), n), "mvgetn_wstr");
        }

        //NULL TERMINATOR!!
        public void mvinnwstr(int y, int x, ref WideCharString<TWideChar> wstr, int n, out int read)
        {
            read = NCursesException.Verify(this.Wrapper.mvinnwstr(y, x, ref wstr.GetPinnableReference(), n), "mvinnwstr");
        }

        public void mvins_nwstr(int y, int x, in WideCharString<TWideChar> wstr, int n)
        {
            NCursesException.Verify(Wrapper.mvins_nwstr(y, x, in wstr.GetPinnableReference(), n), "mvins_nwstr");
        }

        public void mvins_wstr(int y, int x, in WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(Wrapper.mvins_wstr(y, x, in wstr.GetPinnableReference()), "mvins_wstr");
        }

        public void mvinwstr(int y, int x, ref WideCharString<TWideChar> wstr)
        {
            NCursesException.Verify(this.Wrapper.mvinwstr(y, x, ref wstr.GetPinnableReference()), "mvinwstr");
        }
    }
}
