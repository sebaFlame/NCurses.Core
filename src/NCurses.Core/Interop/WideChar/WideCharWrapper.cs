using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop.WideChar
{
    internal class WideCharWrapper<TWideChar, TChar> //handles wchar_t and wint_t
        : INativeCharWrapper<IChar, TWideChar, ICharString, WideCharString<TWideChar>>
        where TWideChar : unmanaged, IChar, IEquatable<TWideChar>
        where TChar : unmanaged, IChar, IEquatable<TChar>
    {
        internal IWideCharWrapper<TWideChar, TChar> Wrapper { get; }

        internal WideCharWrapper(IWideCharWrapper<TWideChar, TChar> wrapper)
        {
            this.Wrapper = wrapper;
        }

        public WideCharString<TWideChar> CastString(in ICharString wCharStr)
        {
            if (!(wCharStr is WideCharString<TWideChar> wCasted))
            {
                throw new InvalidCastException("MultiByte character is in incorrect format");
            }

            return wCasted;
        }

        public TWideChar CastChar(in IChar wChar)
        {
            if (!(wChar is TWideChar wCasted))
            {
                throw new InvalidCastException("MultiByte character is in incorrect format");
            }

            return wCasted;
        }

        public unsafe static T ToPrimitiveType<T>(in TWideChar wch)
            where T : unmanaged
        {
            TWideChar* arr = stackalloc TWideChar[1];
            arr[0] = wch;

            return Unsafe.Read<T>(arr);
        }

        internal static bool VerifyInput(string method, int ret, in int wc, out TWideChar wch, out Key key)
        {
            if (ret == (int)Key.CODE_YES)
            {
                wch = WideCharFactoryInternal<TWideChar>.Instance.GetNativeEmptyCharInternal();
                key = (Key)(short)wc;
                return true;
            }

            //TODO: can still be an escaped function key? (iterate over all function keys????)
            NCursesException.Verify(ret, method);
            key = default(Key);
            wch = WideCharFactoryInternal<TWideChar>.Instance.GetNativeCharInternal(wc);
            return false;
        }

        internal static bool VerifyInput(string method, int ret, bool hasKeyPad, out TWideChar wch, out Key key)
        {
            bool functionKey = NativeNCurses.VerifyInput(method, hasKeyPad, ret, out char ch, out key);

            //could be a unicode char
            if ((!functionKey && ret > 0 && (ret != ch))
                || (functionKey && ret > (int)Key.MAX))
            {
                unsafe
                {
                    int* byteArr = stackalloc int[1];
                    byteArr[0] = ret;

                    Span<int> iSpan = new Span<int>(byteArr, 1);
                    Span<TWideChar> cSpan = MemoryMarshal.Cast<int, TWideChar>(iSpan);
                    wch = cSpan[0];
                }
                return false;
            }

            //not a unicode char
            wch = WideCharFactoryInternal<TWideChar>.Instance.GetNativeCharInternal(ch);
            return functionKey;
        }
    }
}
