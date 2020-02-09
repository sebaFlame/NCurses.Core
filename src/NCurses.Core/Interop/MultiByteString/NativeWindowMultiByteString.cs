using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.MultiByteString
{
    public interface INativeWindowMultiByteString
    {
        void mvwaddnwstr(IntPtr window, int y, int x, in string wstr, int n);
        void mvwaddwstr(IntPtr window, int y, int x, in string wstr);
        bool mvwget_wch(IntPtr window, int y, int x, out char wch, out Key key);
        void mvwget_wstr(IntPtr window, int y, int x, out string wstr);
        void mvwgetn_wstr(IntPtr window, int y, int x, out string wstr, int n);
        void mvwinnwstr(IntPtr window, int y, int x, out string wstr, int n, out int read);
        void mvwins_nwstr(IntPtr window, int y, int x, in string wstr, int n);
        void mvwins_wstr(IntPtr window, int y, int x, in string wstr);
        void mvwinwstr(IntPtr window, int y, int x, out string wstr);
        void waddnwstr(IntPtr window, in string wstr, int n);
        void waddwstr(IntPtr window, in string wstr);
        bool wget_wch(IntPtr window, out char wch, out Key key);
        void wget_wstr(IntPtr window, out string wstr);
        void wgetn_wstr(IntPtr window, out string wstr, int n);
        void winnwstr(IntPtr window, out string wstr, int count, out int read);
        void winwstr(IntPtr window, out string wstr);
        void wins_nwstr(IntPtr window, in string wstr, int n);
        void wins_wstr(IntPtr window, in string wstr);
    }
    
    internal class NativeWindowMultiByteString<TMultiByteString, TSingleByteString> : MultiByteStringWrapper<TMultiByteString, TSingleByteString>, INativeWindowMultiByteString
        where TMultiByteString : unmanaged
        where TSingleByteString : unmanaged
    {
        public void mvwaddnwstr(IntPtr window, int y, int x, in string wstr, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(this.Wrapper.mvwaddnwstr(window, y, x, MarshalStringReadonly(wstr, byteArray, byteLength), n), "mvwaddnwstr");
            }
        }

        public void mvwaddwstr(IntPtr window, int y, int x, in string wstr)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(this.Wrapper.mvwaddwstr(window, y, x, MarshalStringReadonly(wstr, byteArray, byteLength)), "mvwaddwstr");
            }
        }

        public unsafe bool mvwget_wch(IntPtr window, int y, int x, out char wch, out Key key)
        {
            unsafe
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return VerifyInput(
                        "mvwget_wch-mvwgetch",
                        NativeWindow.mvwgetch(window, y, x),
                        NativeWindow.is_keypad(window),
                        out wch,
                        out key);
                }

                int* wideCh = stackalloc int[1];
                Span<int> span = new Span<int>(wideCh, 1);

                return VerifyInput("mvwget_wch",
                    this.Wrapper.mvwget_wch(window, y, x, ref span.GetPinnableReference()),
                    span,
                    out wch,
                    out key);
            }
        }

        public void mvwget_wstr(IntPtr window, int y, int x, out string wstr)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.mvwget_wch(window, y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<int> span)), "mvwget_wch");
                wstr = ReadString(ref span);
            }
        }

        public void mvwgetn_wstr(IntPtr window, int y, int x, out string wstr, int n)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[n];
                NCursesException.Verify(this.Wrapper.mvwgetn_wstr(window, y, x, ref MarshalString(strPtr, n, out Span<int> span), n), "mvwgetn_wstr");
                wstr = ReadString(ref span);
            }
        }

        public void mvwinnwstr(IntPtr window, int y, int x, out string wstr, int n, out int read)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[n];
                read = NCursesException.Verify(this.Wrapper.mvwinnwstr(window, y, x, ref MarshalString(strPtr, n, out Span<TMultiByteString> span), n), "mvwinnwstr");
                wstr = ReadString(ref span);
            }
        }

        public void mvwins_nwstr(IntPtr window, int y, int x, in string wstr, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(this.Wrapper.mvwins_nwstr(window, y, x, MarshalStringReadonly(wstr, byteArray, byteLength), n), "mvwins_nwstr");
            }
        }

        public void mvwins_wstr(IntPtr window, int y, int x, in string wstr)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(this.Wrapper.mvwins_wstr(window, y, x, MarshalStringReadonly(wstr, byteArray, byteLength)), "mvwins_wstr");
            }
        }

        public void mvwinwstr(IntPtr window, int y, int x, out string wstr)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.mvwinwstr(window, y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TMultiByteString> span)), "mvwinwstr");
                wstr = ReadString(ref span);
            }
        }

        public void waddnwstr(IntPtr window, in string wstr, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(this.Wrapper.waddnwstr(window, MarshalStringReadonly(wstr, byteArray, byteLength), n), "waddnwstr");
            }
        }

        public void waddwstr(IntPtr window, in string wstr)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(this.Wrapper.waddwstr(window, MarshalStringReadonly(wstr, byteArray, byteLength)), "waddwstr");
            }
        }

        public unsafe bool wget_wch(IntPtr window, out char wch, out Key key)
        {
            unsafe
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return VerifyInput(
                        "wget_wch-wgetch",
                        NativeWindow.wgetch(window),
                        NativeWindow.is_keypad(window),
                        out wch,
                        out key);
                }

                int* wideCh = stackalloc int[1];
                Span<int> span = new Span<int>(wideCh, 1);

                return VerifyInput("wget_wch",
                    this.Wrapper.wget_wch(window, ref span.GetPinnableReference()),
                    span,
                    out wch,
                    out key);
            }
        }

        public void wget_wstr(IntPtr window, out string wstr)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.wget_wstr(window, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<int> span)), "wget_wstr");
                wstr = ReadString(ref span);
            }
        }

        public void wgetn_wstr(IntPtr window, out string wstr, int n)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[n];
                NCursesException.Verify(this.Wrapper.wgetn_wstr(window, ref MarshalString(strPtr, n, out Span<int> span), n), "wgetn_wstr");
                wstr = ReadString(ref span);
            }
        }

        public void winnwstr(IntPtr window, out string wstr, int count, out int read)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[count];
                read = NCursesException.Verify(this.Wrapper.winnwstr(window, ref MarshalString(strPtr, count, out Span<TMultiByteString> span), count), "winnwstr");
                wstr = ReadString(ref span);
            }
        }

        public void winwstr(IntPtr window, out string wstr)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.winwstr(window, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TMultiByteString> span)), "winwstr");
                wstr = ReadString(ref span);
            }
        }

        public void wins_nwstr(IntPtr window, in string wstr, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(Wrapper.wins_nwstr(window, MarshalStringReadonly(wstr, byteArray, byteLength), n), "wins_nwstr");
            }
        }

        public void wins_wstr(IntPtr window, in string wstr)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(Wrapper.wins_wstr(window, MarshalStringReadonly(wstr, byteArray, byteLength)), "wins_wstr");
            }
        }
    }
}
