using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;

#if NCURSES_VERSION_6
using chtype = System.UInt32;
#elif NCURSES_VERSION_5
using chtype = System.UInt64;
#endif

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

        public ulong attr
        {
            get
            {
                return (chtype)WCHARType.GetTypeInfo().GetField("attr").GetValue(this.WCHAR);
            }
            set
            {
                WCHARType.GetTypeInfo().GetField("attr").SetValue(this.WCHAR, (chtype)value);
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

        public NCursesWCHAR(char ch, ulong attrs)
        {
            if (wcharAttrConstructor == null)
                wcharAttrConstructor = WCHARType.GetTypeInfo().GetConstructor(new Type[] { typeof(char), typeof(chtype) });
            this.WCHAR = wcharAttrConstructor.Invoke(new object[] { ch, (chtype)attrs });
        }

        public NCursesWCHAR(ulong ch)
        {
            if (charConstructor == null)
                charConstructor = WCHARType.GetTypeInfo().GetConstructor(new Type[] { typeof(chtype) });
            this.WCHAR = charConstructor.Invoke(new object[] { (chtype)ch });
        }

        public NCursesWCHAR(ulong ch, ulong attrs)
        {
            if (charAttrConstructor == null)
                charAttrConstructor = WCHARType.GetTypeInfo().GetConstructor(new Type[] { typeof(chtype), typeof(chtype) });
            this.WCHAR = charAttrConstructor.Invoke(new object[] { (chtype)ch, (chtype)attrs });
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
}
