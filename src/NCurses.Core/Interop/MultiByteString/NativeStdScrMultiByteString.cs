using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Buffers;

namespace NCurses.Core.Interop.MultiByteString
{
    public interface INativeStdScrMultiByteString
    {
        void addnwstr(string wstr, int n);
        void addwstr(string wstr);
        bool get_wch(out char wch, out Key key);
        void get_wstr(out string wstr);
        void getn_wstr(out string wstr, int n);
        void innwstr(out string wstr, int n, out int read);
        void ins_nwstr(in string wstr, int n);
        void ins_wstr(in string wstr);
        void inwstr(out string wstr);
        void mvaddnwstr(int y, int x, in string wstr, int n);
        void mvaddwstr(int y, int x, in string wstr);
        bool mvget_wch(int y, int x, out char wch, out Key key);
        void mvget_wstr(int y, int x, out string wstr);
        void mvgetn_wstr(int y, int x, out string wstr, int n);
        void mvinnwstr(int y, int x, out string wstr, int n, out int read);
        void mvins_nwstr(int y, int x, in string wstr, int n);
        void mvins_wstr(int y, int x, in string wstr);
        void mvinwstr(int y, int x, out string wstr);
    }

    internal class NativeStdScrMultiByteString<TMultiByteString, TSingleByteString> : MultiByteStringWrapper<TMultiByteString, TSingleByteString>, INativeStdScrMultiByteString, IDisposable
        where TMultiByteString : unmanaged
        where TSingleByteString : unmanaged
    {
        private Memory<TMultiByteString> input;
        private MemoryHandle inputHandle;

        //TODO: thread safety around input
        public NativeStdScrMultiByteString()
        {
            this.input = new Memory<TMultiByteString>(new TMultiByteString[1]);
            this.inputHandle = input.Pin();
        }

        ~NativeStdScrMultiByteString()
        {
            this.Dispose();
        }

        public void addnwstr(string wstr, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(this.Wrapper.addnwstr(MarshalStringReadonly(wstr, byteArray, byteLength), n), "addnwstr");
            }
        }

        public void addwstr(string wstr)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(Wrapper.addwstr(MarshalStringReadonly(wstr, byteArray, byteLength)), "addwstr");
            }
        }

        public void innwstr(out string wstr, int count, out int read)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[count];
                read = NCursesException.Verify(this.Wrapper.innwstr(ref MarshalString(strPtr, count, out Span<TMultiByteString> span), count), "innwstr");
                wstr = ReadString(ref span);
            }
        }

        public void ins_nwstr(in string wstr, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(Wrapper.ins_nwstr(MarshalStringReadonly(wstr, byteArray, byteLength), n), "ins_nwstr");
            }
        }

        public void ins_wstr(in string wstr)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(Wrapper.ins_wstr(MarshalStringReadonly(wstr, byteArray, byteLength)), "ins_wstr");
            }
        }

        public void inwstr(out string wstr)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.inwstr(ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TMultiByteString> span)), "inwstr");
                wstr = ReadString(ref span);
            }
        }

        //TODO: reuse same Memory<TWideStr> OR use a stackalloc on every call?
        //TODO: make overridable and return an ulong containing the char (8 bytes should be enough?) for manual processing
        /// <summary>
        /// read a character
        /// </summary>
        /// <param name="wch">the read wide character</param>
        /// <param name="key">the read function key</param>
        /// <returns>returns TRUE if a funtion key has been pressed</returns>
        public unsafe bool get_wch(out char wch, out Key key)
        {
            //zero input
            TMultiByteString* wideCh = stackalloc TMultiByteString[1];
            this.input.Span[0] = wideCh[0];

            return VerifyInput("get_wch", 
                this.Wrapper.get_wch(ref Unsafe.AsRef<TMultiByteString>(this.inputHandle.Pointer)), 
                this.input.Span, 
                out wch, 
                out key);
        }

        public void get_wstr(out string wstr)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.get_wstr(ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TMultiByteString> span)), "get_wstr");
                wstr = ReadString(ref span);
            }
        }

        public void getn_wstr(out string wstr, int n)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[n];
                NCursesException.Verify(this.Wrapper.get_wstr(ref MarshalString(strPtr, n, out Span<TMultiByteString> span)), "get_wstr");
                wstr = ReadString(ref span);
            }
        }

        public void mvaddnwstr(int y, int x, in string wstr, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(Wrapper.mvaddnwstr(y, x, MarshalStringReadonly(wstr, byteArray, byteLength), n), "mvaddnwstr");
            }
        }

        public void mvaddwstr(int y, int x, in string wstr)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(Wrapper.mvaddwstr(y, x, MarshalStringReadonly(wstr, byteArray, byteLength)), "mvaddwstr");
            }
        }

        //TODO: reuse same Memory<TWideStr> OR use a stackalloc on every call?
        /// <summary>
        /// move cursor and read a character
        /// </summary>
        /// <param name="y">the line to move the cursor to</param>
        /// <param name="x">the column to move the cursor to</param>
        /// <param name="wch">the read wide character</param>
        /// <param name="key">the read function key</param>
        /// <returns>returns TRUE if a funtion key has been pressed</returns>
        public unsafe bool mvget_wch(int y, int x, out char wch, out Key key)
        {
            TMultiByteString* wideCh = stackalloc TMultiByteString[1];
            this.input.Span[0] = wideCh[0];

            return VerifyInput("mvget_wch", this.Wrapper.mvget_wch(y, x, ref Unsafe.AsRef<TMultiByteString>(this.inputHandle.Pointer)),
                this.input.Span,
                out wch,
                out key);
        }

        public void mvget_wstr(int y, int x, out string wstr)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.mvget_wstr(y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TMultiByteString> span)), "mvget_wstr");
                wstr = ReadString(ref span);
            }
        }

        public void mvgetn_wstr(int y, int x, out string wstr, int n)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[n];
                NCursesException.Verify(this.Wrapper.mvgetn_wstr(y, x, ref MarshalString(strPtr, n, out Span<TMultiByteString> span), n), "mvgetn_wstr");
                wstr = ReadString(ref span);
            }
        }

        public void mvinnwstr(int y, int x, out string wstr, int n, out int read)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[n];
                read = NCursesException.Verify(this.Wrapper.mvinnwstr(y, x, ref MarshalString(strPtr, n, out Span<TMultiByteString> span), n), "mvinnwstr");
                wstr = ReadString(ref span);
            }
        }

        public void mvins_nwstr(int y, int x, in string wstr, int n)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(Wrapper.mvins_nwstr(y, x, MarshalStringReadonly(wstr, byteArray, byteLength), n), "mvins_nwstr");
            }
        }

        public void mvins_wstr(int y, int x, in string wstr)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(wstr)];
                NCursesException.Verify(Wrapper.mvins_wstr(y, x, MarshalStringReadonly(wstr, byteArray, byteLength)), "mvins_wstr");
            }
        }

        public void mvinwstr(int y, int x, out string wstr)
        {
            unsafe
            {
                TMultiByteString* strPtr = stackalloc TMultiByteString[Constants.MAX_STRING_LENGTH];
                NCursesException.Verify(this.Wrapper.mvinwstr(y, x, ref MarshalString(strPtr, Constants.MAX_STRING_LENGTH, out Span<TMultiByteString> span)), "mvinwstr");
                wstr = ReadString(ref span);
            }
        }

#region IDisposable
        public void Dispose()
        {
            if(!EqualityComparer<MemoryHandle>.Default.Equals(this.inputHandle, default(MemoryHandle)))
                this.inputHandle.Dispose();
            GC.SuppressFinalize(this);
        }
#endregion
    }
}
