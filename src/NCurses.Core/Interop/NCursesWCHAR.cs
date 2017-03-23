using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;

namespace NCurses.Core.Interop
{
    public class NCursesWCHAR
    {
        internal static Type WCHARType;
        internal object WCHAR;

        private static ConstructorInfo wcharConstructor;
        private static ConstructorInfo wcharAttrConstructor;
        private static ConstructorInfo charConstructor;
        private static ConstructorInfo charAttrConstructor;

        private static MethodInfo getSize;
        private static MethodInfo toPtr;
        private static MethodInfo toStruct;

        static NCursesWCHAR()
        {
            WCHARType = DynamicTypeBuilder.CreateWCHARType();
        }

        public byte[] chars
        {
            get
            {
                return WCHARType.GetTypeInfo().GetField("chars").GetValue(this.WCHAR) as byte[];
            }
            set
            {
                WCHARType.GetTypeInfo().GetField("chars").SetValue(this.WCHAR, value);
            }
        }

        public uint attr
        {
            get
            {
                return (uint)WCHARType.GetTypeInfo().GetField("attr").GetValue(this.WCHAR);
            }
            set
            {
                WCHARType.GetTypeInfo().GetField("attr").SetValue(this.WCHAR, value);
            }
        }

        public int ext_color
        {
            get
            {
                return (int)WCHARType.GetTypeInfo().GetField("ext_color").GetValue(this.WCHAR);
            }
            set
            {
                WCHARType.GetTypeInfo().GetField("ext_color").SetValue(this.WCHAR, value);
            }
        }

        internal int Size
        {
            get
            {
                if (getSize == null)
                    getSize = typeof(Marshal).GetTypeInfo().GetMethods()
                        .FirstOrDefault(x => x.Name.Equals("SizeOf") && x.IsGenericMethodDefinition && x.GetParameters().Length == 0)
                        .MakeGenericMethod(WCHARType);
                return (int)getSize.Invoke(null, new object[0]);
            }
        }

        public NCursesWCHAR()
        {
            this.WCHAR = Activator.CreateInstance(WCHARType);
        }

        public NCursesWCHAR(char ch)
        {
            if (wcharConstructor == null)
                wcharConstructor = WCHARType.GetTypeInfo().GetConstructor(new Type[] { typeof(char) });
            this.WCHAR = wcharConstructor.Invoke(new object[] { ch });
        }

        public NCursesWCHAR(char ch, uint attrs)
        {
            if (wcharAttrConstructor == null)
                wcharAttrConstructor = WCHARType.GetTypeInfo().GetConstructor(new Type[] { typeof(char), typeof(uint) });
            this.WCHAR = wcharAttrConstructor.Invoke(new object[] { ch, attrs });
        }

        public NCursesWCHAR(uint ch)
        {
            if (charConstructor == null)
                charConstructor = WCHARType.GetTypeInfo().GetConstructor(new Type[] { typeof(uint) });
            this.WCHAR = charConstructor.Invoke(new object[] { ch });
        }

        public NCursesWCHAR(uint ch, uint attrs)
        {
            if (charAttrConstructor == null)
                charAttrConstructor = WCHARType.GetTypeInfo().GetConstructor(new Type[] { typeof(uint), typeof(uint) });
            this.WCHAR = charAttrConstructor.Invoke(new object[] { ch, attrs });
        }

        internal IDisposable ToPointer(out IntPtr ptr)
        {
            NCurseWCHARDisposable disp = new NCurseWCHARDisposable(this);
            ptr = disp.Ptr;
            return disp;
        }

        internal void ToPointer(IntPtr ptr)
        {
            if (this.WCHAR == null)
                throw new NullReferenceException("Character hasn't been initialized yet");

            if (toPtr == null)
                toPtr = typeof(Marshal).GetTypeInfo().GetMethods()
                    .FirstOrDefault(x => x.Name.Equals("StructureToPtr") && x.IsGenericMethodDefinition && x.GetParameters().Length == 3)
                    .MakeGenericMethod(WCHARType);

            toPtr.Invoke(null, new object[] { this.WCHAR, ptr, true });
        }

        internal IntPtr ToPointer()
        {
            if (this.WCHAR == null)
                throw new NullReferenceException("Character hasn't been initialized yet");

            if (toPtr == null)
                toPtr = typeof(Marshal).GetTypeInfo().GetMethods()
                    .FirstOrDefault(x => x.Name.Equals("StructureToPtr") && x.IsGenericMethodDefinition && x.GetParameters().Length == 3)
                    .MakeGenericMethod(WCHARType);

            IntPtr ptr = Marshal.AllocHGlobal(this.Size);
            toPtr.Invoke(null, new object[] { this.WCHAR, ptr, true });
            return ptr;
        }

        internal void ToStructure(IntPtr ptr)
        {
            if (toStruct == null)
                toStruct = typeof(Marshal).GetTypeInfo().GetMethods()
                    .FirstOrDefault(x => x.Name.Equals("PtrToStructure") && x.IsGenericMethodDefinition && x.GetParameters().Length == 1)
                    .MakeGenericMethod(WCHARType);

            this.WCHAR = toStruct.Invoke(null, new object[] { ptr });
        }

        public char GetChar()
        {
            if (this.WCHAR == null)
                throw new NullReferenceException("Character hasn't been initialized yet");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return Encoding.Unicode.GetChars(this.chars)[0];
            else
                return Encoding.UTF32.GetChars(this.chars)[0];
        }

        #region dynamic type creation

        #endregion
    }

    internal class NCurseWCHARDisposable : IDisposable
    {
        public readonly IntPtr Ptr;

        private readonly NCursesWCHAR wch;

        internal NCurseWCHARDisposable(NCursesWCHAR wch, IntPtr ptr)
        {
            this.wch = wch;
            this.Ptr = ptr;
        }

        internal NCurseWCHARDisposable(NCursesWCHAR wch)
        {
            this.wch = wch;
            this.Ptr = wch.ToPointer();
            GC.AddMemoryPressure(this.wch.Size);
        }

        public void Dispose()
        {
            if (this.Ptr != IntPtr.Zero)
            {
                this.wch.ToStructure(this.Ptr);
                Marshal.FreeHGlobal(this.Ptr);
            }

            if (this.wch != null && this.wch.Size > 0)
                GC.RemoveMemoryPressure(this.wch.Size);
        }
    }

    internal class NCursesWCHAR2
    {
        internal NCURSES_CH_T_win CH_win;
        internal NCURSES_CH_T_nix CH_nix;

        public NCursesWCHAR2() { }

        public NCursesWCHAR2(char ch)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                this.CH_win = new NCURSES_CH_T_win(ch);
            else
                this.CH_nix = new NCURSES_CH_T_nix(ch);
        }

        public NCursesWCHAR2(char ch, uint attr)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                this.CH_win = new NCURSES_CH_T_win(ch, attr);
            else
                this.CH_nix = new NCURSES_CH_T_nix(ch, attr);
        }

        public NCursesWCHAR2(uint c)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                this.CH_win = new NCURSES_CH_T_win(c);
            else
                this.CH_nix = new NCURSES_CH_T_nix(c);
        }

        public NCursesWCHAR2(uint ch, uint attr)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                this.CH_win = new NCURSES_CH_T_win(ch, attr);
            else
                this.CH_nix = new NCURSES_CH_T_nix(ch, attr);
        }

        internal IntPtr AllocPointer()
        {
            IntPtr ptr;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this.CH_win));
            else
                ptr = Marshal.AllocHGlobal(Marshal.SizeOf(this.CH_nix));
            return ptr;
        }

        internal void ToPointer(IntPtr ptr)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Marshal.StructureToPtr(this.CH_win, ptr, true);
            else
                Marshal.StructureToPtr(this.CH_nix, ptr, true);
        }

        internal IntPtr ToPointer()
        {
            IntPtr ptr = AllocPointer();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Marshal.StructureToPtr(this.CH_win, ptr, true);
            else
                Marshal.StructureToPtr(this.CH_nix, ptr, true);
            return ptr;
        }

        internal void ToStructure(IntPtr ptr)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Marshal.PtrToStructure(ptr, this.CH_win);
            else
                Marshal.PtrToStructure(ptr, this.CH_nix);
        }

        internal char GetChar()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return Encoding.Unicode.GetChars(CH_win.chars)[0];
            else
                return Encoding.UTF32.GetChars(CH_nix.chars)[0];
        }
    }

    //TODO: create CustomMarshaller to create a null terminated array of NCURSES_CH_T(without size)
    [StructLayout(LayoutKind.Sequential)]
    internal struct NCURSES_CH_T_win /* cchar_t */
    {
        public NCURSES_CH_T_win(char ch)
            : this()
        {
            this.chars = new byte[10];
            bool completed;
            int charsUsed, bytesUsed;
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new InvalidOperationException("Incorrect struct for this platform");

            Encoding.Unicode.GetEncoder().Convert(new char[] { ch }, 0, 1, this.chars, 0, 10, false, out charsUsed, out bytesUsed, out completed);
            if (!completed)
                throw new InvalidOperationException("Failed to convert character for marshaling");
        }

        public NCURSES_CH_T_win(char ch, uint attr)
            : this(ch)
        {
            this.attr = attr;
        }

        //TODO: use Encoding.Unicode on bytes
        public NCURSES_CH_T_win(uint c)
            : this()
        {
            this.chars = new byte[10];
            BitConverter.GetBytes(c).CopyTo(this.chars, 0);
        }

        public NCURSES_CH_T_win(uint ch, uint attr)
            : this(ch)
        {
            this.attr = attr;
        }

        public uint attr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] chars;
        /// <summary>
        /// color pair, must be more than 16-bits
        /// </summary>
        public int ext_color;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct NCURSES_CH_T_nix /* cchar_t */
    {
        public NCURSES_CH_T_nix(char ch)
            : this()
        {
            this.chars = new byte[20];
            bool completed;
            int charsUsed, bytesUsed;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new InvalidOperationException("Incorrect struct for this platform");

            Encoding.UTF32.GetEncoder().Convert(new char[] { ch }, 0, 1, this.chars, 0, 20, false, out charsUsed, out bytesUsed, out completed);
            if (!completed)
                throw new InvalidOperationException("Failed to convert character for marshaling");
        }

        public NCURSES_CH_T_nix(char ch, uint attr)
            : this(ch)
        {
            this.attr = attr;
        }

        public NCURSES_CH_T_nix(uint c)
            : this()
        {
            this.chars = new byte[20];
            BitConverter.GetBytes(c).CopyTo(this.chars, 0);
        }

        public NCURSES_CH_T_nix(uint ch, uint attr)
            : this(ch)
        {
            this.attr = attr;
        }

        public uint attr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)] /* 5 * 4 on nix */
        public byte[] chars;
        /// <summary>
        /// color pair, must be more than 16-bits
        /// </summary>
        public int ext_color;
    }
}
