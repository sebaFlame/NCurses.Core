using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.SingleByteString
{
    public interface INativeScreenSingleByteString
    {
        void define_key_sp(IntPtr screen, in string definition, int keycode);
        char erasechar_sp(IntPtr screen);
        int key_defined_sp(IntPtr screen, in string definition);
        string keybound_sp(IntPtr screen, int keycode, int count);
        string keyname_sp(IntPtr screen, int c);
        char killchar_sp(IntPtr screen);
        string longname_sp(IntPtr screen);
        void putp_sp(IntPtr screen, in string str);
        void scr_dump_sp(IntPtr screen, in string filename);
        void scr_init_sp(IntPtr screen, in string filename);
        void scr_restore_sp(IntPtr screen, in string filename);
        void scr_set_sp(IntPtr screen, in string filename);
        string slk_label_sp(IntPtr screen, int labnum);
        void slk_set_sp(IntPtr screen, int labnum, in string label, int fmt);

        string termname_sp(IntPtr screen);
        int tigetflag_sp(IntPtr screen, in string capname);
        int tigetnum_sp(IntPtr screen, in string capname);
        int tigetstr_sp(IntPtr screen, in string capname);
    }

    public class NativeScreenSingleByteString<TSingleByteString> : SingleByteStringWrapper<TSingleByteString>, INativeScreenSingleByteString
        where TSingleByteString : unmanaged
    {
        public void define_key_sp(IntPtr screen, in string definition, int keycode)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(definition)];
                NCursesException.Verify(this.Wrapper.define_key_sp(screen, MarshalStringReadonly(definition, byteArray, byteLength), keycode), "define_key_sp");
            }
        }

        public int key_defined_sp(IntPtr screen, in string definition)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(definition)];
                return this.Wrapper.key_defined_sp(screen, MarshalStringReadonly(definition, byteArray, byteLength));
            }
        }

        public string keybound_sp(IntPtr screen, int keycode, int count)
        {
            return ReadString(ref this.Wrapper.keybound_sp(screen, keycode, count));
        }

        public string keyname_sp(IntPtr screen, int c)
        {
            return ReadString(ref this.Wrapper.keyname_sp(screen, c));
        }

        public char killchar_sp(IntPtr screen)
        {
            return ReadChar(this.Wrapper.killchar_sp(screen));
        }

        public string longname_sp(IntPtr screen)
        {
            return ReadString(ref this.Wrapper.longname_sp(screen));
        }

        public char erasechar_sp(IntPtr screen)
        {
            return ReadChar(this.Wrapper.erasechar_sp(screen));
        }

        public void putp_sp(IntPtr screen, in string str)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(str)];
                NCursesException.Verify(this.Wrapper.putp_sp(screen, MarshalStringReadonly(str, byteArray, byteLength)), "putp_sp");
            }
        }

        public void scr_dump_sp(IntPtr screen, in string filename)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(filename)];
                NCursesException.Verify(this.Wrapper.scr_dump_sp(screen, MarshalStringReadonly(filename, byteArray, byteLength)), "scr_dump_sp");
            }
        }

        public void scr_init_sp(IntPtr screen, in string filename)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(filename)];
                NCursesException.Verify(this.Wrapper.scr_init_sp(screen, MarshalStringReadonly(filename, byteArray, byteLength)), "scr_init_sp");
            }
        }

        public void scr_restore_sp(IntPtr screen, in string filename)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(filename)];
                NCursesException.Verify(this.Wrapper.scr_restore_sp(screen, MarshalStringReadonly(filename, byteArray, byteLength)), "scr_restore_sp");
            }
        }

        public void scr_set_sp(IntPtr screen, in string filename)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(filename)];
                NCursesException.Verify(this.Wrapper.scr_set_sp(screen, MarshalStringReadonly(filename, byteArray, byteLength)), "scr_set_sp");
            }
        }

        public string slk_label_sp(IntPtr screen, int labnum)
        {
            return ReadString(ref this.Wrapper.slk_label_sp(screen, labnum));
        }

        public void slk_set_sp(IntPtr screen, int labnum, in string label, int fmt)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(label)];
                NCursesException.Verify(this.Wrapper.slk_set_sp(screen, labnum, MarshalStringReadonly(label, byteArray, byteLength), fmt), "slk_set_sp");
            }
        }

        public string termname_sp(IntPtr screen)
        {
            return ReadString(ref this.Wrapper.termname_sp(screen));
        }

        public int tigetflag_sp(IntPtr screen, in string capname)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(capname)];
                return this.Wrapper.tigetflag_sp(screen, MarshalStringReadonly(capname, byteArray, byteLength));
            }
        }

        public int tigetnum_sp(IntPtr screen, in string capname)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(capname)];
                return this.Wrapper.tigetnum_sp(screen, MarshalStringReadonly(capname, byteArray, byteLength));
            }            
        }

        public int tigetstr_sp(IntPtr screen, in string capname)
        {
            unsafe
            {
                int byteLength;
                byte* byteArray = stackalloc byte[byteLength = GetNullTerminatedStringLength(capname)];
                return this.Wrapper.tigetstr_sp(screen, MarshalStringReadonly(capname, byteArray, byteLength));
            }
        }
    }
}
