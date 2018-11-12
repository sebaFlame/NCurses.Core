using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NCurses.Core.Interop.SingleByteString
{
    public interface INativeNCursesSingleByteString
    {
        string curses_version();
        void define_key(in string definition, int keycode);
        char erasechar();
        int key_defined(in string definition);
        string keybound(int keycode, int count);
        string keyname(int c);
        char killchar();
        string longname();
        void putp(in string str);
        void scr_dump(in string filename);
        void scr_init(in string filename);
        void scr_restore(in string filename);
        void scr_set(in string filename);
        string slk_label(int labnum);
        void slk_set(int labnum, in string label, int fmt);
        string termname();
        int tigetflag(in string capname);
        int tigetnum(in string capname);
        int tigetstr(in string capname);
    }

    public class NativeNCursesSingleByteString<TSingleByteString> : SingleByteStringWrapper<TSingleByteString>, INativeNCursesSingleByteString
        where TSingleByteString : unmanaged
    {
        public string curses_version()
        {
            return ReadString(ref this.Wrapper.curses_version());
        }

        public void define_key(in string definition, int keycode)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(definition)];
                NCursesException.Verify(this.Wrapper.define_key(MarshalStringReadonly(definition, byteArray, byteLength), keycode), "define_key");
            }
        }

        public int key_defined(in string definition)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(definition)];
                return this.Wrapper.key_defined(MarshalStringReadonly(definition, byteArray, byteLength));
            }
        }

        public string keybound(int keycode, int count)
        {
            return ReadString(ref this.Wrapper.keybound(keycode, count));
        }

        public string keyname(int c)
        {
            return ReadString(ref this.Wrapper.keyname(c));
        }

        public char killchar()
        {
            return ReadChar(this.Wrapper.killchar());
        }

        public string longname()
        {
            return ReadString(ref this.Wrapper.longname());
        }

        public char erasechar()
        {
            return ReadChar(this.Wrapper.erasechar());
        }

        public void putp(in string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.putp(MarshalStringReadonly(str, byteArray, byteLength)), "putp");
            }
        }

        //TODO: filenames can be multibyte
        public void scr_dump(in string filename)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(filename)];
                NCursesException.Verify(this.Wrapper.scr_dump(MarshalStringReadonly(filename, byteArray, byteLength)), "scr_dump");
            }
        }

        //TODO: filenames can be multibyte
        public void scr_init(in string filename)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(filename)];
                NCursesException.Verify(this.Wrapper.scr_init(MarshalStringReadonly(filename, byteArray, byteLength)), "scr_init");
            }
        }

        //TODO: filenames can be multibyte
        public void scr_restore(in string filename)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(filename)];
                NCursesException.Verify(this.Wrapper.scr_restore(MarshalStringReadonly(filename, byteArray, byteLength)), "scr_restore");
            }
        }

        //TODO: filenames can be multibyte
        public void scr_set(in string filename)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(filename)];
                NCursesException.Verify(this.Wrapper.scr_set(MarshalStringReadonly(filename, byteArray, byteLength)), "scr_set");
            }
        }

        public string slk_label(int labnum)
        {
            return ReadString(ref this.Wrapper.slk_label(labnum));
        }

        public void slk_set(int labnum, in string label, int fmt)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(label)];
                NCursesException.Verify(this.Wrapper.slk_set(labnum, MarshalStringReadonly(label, byteArray, byteLength), fmt), "slk_set");
            }
        }

        public string termname()
        {
            return ReadString(ref this.Wrapper.termname());
        }

        public int tigetflag(in string capname)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(capname)];
                return this.Wrapper.tigetflag(MarshalStringReadonly(capname, byteArray, byteLength));
            }
        }

        public int tigetnum(in string capname)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(capname)];
                return this.Wrapper.tigetnum(MarshalStringReadonly(capname, byteArray, byteLength));
            }
        }

        public int tigetstr(in string capname)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(capname)];
                return this.Wrapper.tigetstr(MarshalStringReadonly(capname, byteArray, byteLength));
            }
        }
    }
}
