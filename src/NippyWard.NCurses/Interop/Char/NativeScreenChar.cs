using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Interop.Char
{
    internal class NativeScreenChar<TChar> 
            : CharWrapper<TChar>, 
            INativeScreenChar<TChar, CharString<TChar>>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
    {
        internal NativeScreenChar(ICharWrapper<TChar> wrapper)
            : base(wrapper) { }

        public void define_key_sp(IntPtr screen, in CharString<TChar> definition, int keycode)
        {
            NCursesException.Verify(this.Wrapper.define_key_sp(screen, in definition.GetPinnableReference(), keycode), "define_key_sp");
        }

        public int key_defined_sp(IntPtr screen, in CharString<TChar> definition)
        {
            return this.Wrapper.key_defined_sp(screen, in definition.GetPinnableReference());
        }

        public CharString<TChar> keybound_sp(IntPtr screen, int keycode, int count)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.keybound_sp(screen, keycode, count), "keybound_sp"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public CharString<TChar> keyname_sp(IntPtr screen, int c)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.keyname_sp(screen, c), "keyname_sp"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public TChar killchar_sp(IntPtr screen)
        {
            return this.Wrapper.killchar_sp(screen);
        }

        public CharString<TChar> longname_sp(IntPtr screen)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.longname_sp(screen), "longname_sp"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public TChar erasechar_sp(IntPtr screen)
        {
            return this.Wrapper.erasechar_sp(screen);
        }

        public void putp_sp(IntPtr screen, in CharString<TChar> str)
        {
            NCursesException.Verify(this.Wrapper.putp_sp(screen, in str.GetPinnableReference()), "putp_sp");
        }

        public void scr_dump_sp(IntPtr screen, in CharString<TChar> filename)
        {
            NCursesException.Verify(this.Wrapper.scr_dump_sp(screen, in filename.GetPinnableReference()), "scr_dump_sp");
        }

        public void scr_init_sp(IntPtr screen, in CharString<TChar> filename)
        {
            NCursesException.Verify(this.Wrapper.scr_init_sp(screen, in filename.GetPinnableReference()), "scr_init_sp");
        }

        public void scr_restore_sp(IntPtr screen, in CharString<TChar> filename)
        {
            NCursesException.Verify(this.Wrapper.scr_restore_sp(screen, in filename.GetPinnableReference()), "scr_restore_sp");
        }

        public void scr_set_sp(IntPtr screen, in CharString<TChar> filename)
        {
            NCursesException.Verify(this.Wrapper.scr_set_sp(screen, in filename.GetPinnableReference()), "scr_set_sp");
        }

        public CharString<TChar> slk_label_sp(IntPtr screen, int labnum)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.slk_label_sp(screen, labnum), "slk_label_sp"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public void slk_set_sp(IntPtr screen, int labnum, in CharString<TChar> label, int fmt)
        {
            NCursesException.Verify(this.Wrapper.slk_set_sp(screen, labnum, in label.GetPinnableReference(), fmt), "slk_set_sp");
        }

        public CharString<TChar> termname_sp(IntPtr screen)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.termname_sp(screen), "termname_sp"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public int tigetflag_sp(IntPtr screen, in CharString<TChar> capname)
        {
            return this.Wrapper.tigetflag_sp(screen, in capname.GetPinnableReference());
        }

        public int tigetnum_sp(IntPtr screen, in CharString<TChar> capname)
        {
            return this.Wrapper.tigetnum_sp(screen, in capname.GetPinnableReference());
        }

        public int tigetstr_sp(IntPtr screen, in CharString<TChar> capname)
        {
            return this.Wrapper.tigetstr_sp(screen, in capname.GetPinnableReference());
        }
    }
}
