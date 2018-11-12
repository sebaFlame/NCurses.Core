using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.MultiByte;

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
                if (ret is ISingleByteChar sret)
                {
                    SingleByteCharFactory.Instance.GetNativeChar(sret, out ISingleByteChar res);
                    return res;
                }
                else if (ret is IMultiByteChar wret)
                {
                    MultiByteCharFactory.Instance.GetNativeChar(wret, out IMultiByteChar res);
                    return res;
                }
                    
                throw new InvalidCastException("Unsupported character type found");
            }
        }

        INCursesChar IACSMap.this[char index] => this[index];
    }
}
