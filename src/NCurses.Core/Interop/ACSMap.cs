using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.Small;
using NCurses.Core.Interop.Wide;

namespace NCurses.Core.Interop
{
    public class ACSMap<TChar> : IACSMap
        where TChar : INCursesChar
    {
        private IntPtr acs_map_handle;

        public ACSMap(IntPtr ptr)
        {
            acs_map_handle = ptr;
        }

        public unsafe INCursesChar this[char index]
        {
            get
            {
                int size = Marshal.SizeOf<TChar>();
                ReadOnlySpan<TChar> acsSpan = new ReadOnlySpan<TChar>(this.acs_map_handle.ToPointer(), 128);
                TChar ret = acsSpan[index];
                if (ret is INCursesSCHAR sret)
                    return SmallCharFactory.GetSmallChar(sret);
                else if (ret is INCursesWCHAR wret)
                    return WideCharFactory.GetWideChar(wret);
                throw new InvalidCastException("Unsupported character type found");
            }
        }

        INCursesChar IACSMap.this[char index] => this[index];
    }
}
