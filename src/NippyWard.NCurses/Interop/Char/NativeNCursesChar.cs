using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NippyWard.NCurses.Interop.Char
{
    internal class NativeNCursesChar<TChar> 
            : CharWrapper<TChar>, 
            INativeNCursesChar<TChar, CharString<TChar>>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
    {
        internal NativeNCursesChar(ICharWrapper<TChar> wrapper)
            : base(wrapper) { }

        public CharString<TChar> curses_version()
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.curses_version(), "curses_version"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public void define_key(in CharString<TChar>definition, int keycode)
        {
            NCursesException.Verify(this.Wrapper.define_key(in definition.GetPinnableReference(), keycode), "define_key");
        }

        public int key_defined(in CharString<TChar>definition)
        {
            return this.Wrapper.key_defined(in definition.GetPinnableReference());
        }

        public CharString<TChar> keybound(int keycode, int count)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.keybound(keycode, count), "keybound"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public CharString<TChar> keyname(int c)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.keyname(c), "keyname"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public TChar killchar()
        {
            return this.Wrapper.killchar();
        }

        public CharString<TChar> longname()
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.longname(), "longname"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public TChar erasechar()
        {
            return this.Wrapper.erasechar();
        }

        public void putp(in CharString<TChar>str)
        {

            NCursesException.Verify(this.Wrapper.putp(in str.GetPinnableReference()), "putp");
        }

        //TODO: filenames can be multibyte
        public void scr_dump(in CharString<TChar>filename)
        {
            NCursesException.Verify(this.Wrapper.scr_dump(in filename.GetPinnableReference()), "scr_dump");
        }

        //TODO: filenames can be multibyte
        public void scr_init(in CharString<TChar>filename)
        {
            NCursesException.Verify(this.Wrapper.scr_init(in filename.GetPinnableReference()), "scr_init");
        }

        //TODO: filenames can be multibyte
        public void scr_restore(in CharString<TChar>filename)
        {
            NCursesException.Verify(this.Wrapper.scr_restore(in filename.GetPinnableReference()), "scr_restore");
        }

        //TODO: filenames can be multibyte
        public void scr_set(in CharString<TChar>filename)
        {
            NCursesException.Verify(this.Wrapper.scr_set(filename.GetPinnableReference()), "scr_set");
        }

        public CharString<TChar> slk_label(int labnum)
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.slk_label(labnum), "slk_label"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public void slk_set(int labnum, in CharString<TChar>label, int fmt)
        {
            NCursesException.Verify(this.Wrapper.slk_set(labnum, in label.GetPinnableReference(), fmt), "slk_set");
        }

        public CharString<TChar> termname()
        {
            using (BufferState<TChar> BufferState = CharFactory<TChar>._Instance.GetNativeString(
                CharFactory<TChar>._CreateArrayBuffer,
                ref NCursesException.Verify(ref this.Wrapper.termname(), "termname"),
                out CharString<TChar> @string))
            {
                return @string;
            }
        }

        public int tigetflag(in CharString<TChar>capname)
        {
            return this.Wrapper.tigetflag(in capname.GetPinnableReference());
        }

        public int tigetnum(in CharString<TChar>capname)
        {
            return this.Wrapper.tigetnum(in capname.GetPinnableReference());
        }

        public int tigetstr(in CharString<TChar>capname)
        {
            return this.Wrapper.tigetstr(in capname.GetPinnableReference());
        }
    }
}
