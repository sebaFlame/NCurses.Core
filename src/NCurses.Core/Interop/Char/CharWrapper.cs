﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace NCurses.Core.Interop.Char
{
    internal class CharWrapper<TChar> //handles char
        : INativeCharWrapper<IChar, TChar, ICharString, CharString<TChar>>
        where TChar : unmanaged, IChar, IEquatable<TChar>
    {
        internal ICharWrapper<TChar> Wrapper { get; }

        internal CharWrapper(ICharWrapper<TChar> wrapper)
        {
            this.Wrapper = wrapper;
        }

        public CharString<TChar> CastString(in ICharString wCharStr)
        {
            if (!(wCharStr is CharString<TChar> wCasted))
            {
                throw new InvalidCastException("Character is in incorrect format");
            }

            return wCasted;
        }

        public TChar CastChar(in IChar wChar)
        {
            if (!(wChar is TChar wCasted))
            {
                throw new InvalidCastException("Character is in incorrect format");
            }

            return wCasted;
        }

        //TODO: verify OR replace with __arglist
        /// <summary>
        /// Free the returned pointer after usage
        /// </summary>
        /// <param name="argList"></param>
        /// <returns></returns>
        internal unsafe IntPtr CreateVarArgList(CharString<TChar>[] strArg, byte** argList)
        {
            IntPtr ptr = Marshal.AllocHGlobal(strArg.Length * Marshal.SizeOf<IntPtr>());
            try
            {
                for (int i = 0; i < strArg.Length; i++)
                {
                    Marshal.WriteIntPtr(
                        ptr,
                        i * Marshal.SizeOf<IntPtr>(),
                        new IntPtr(Unsafe.AsPointer<TChar>(ref strArg[i].GetPinnableReference())));
                }
            }
            catch
            {
                Marshal.FreeHGlobal(ptr);
                throw;
            }

            return ptr;
        }
    }
}
