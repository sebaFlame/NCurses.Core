using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.Platform;

namespace NippyWard.NCurses.Interop.Wrappers
{
    internal class NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : ICustomTypeWrapper
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        public INativeNCursesWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> NCurses => NCursesInternal;
        public INativeWindowWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> Window => WindowInternal;
        public INativeStdScrWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> StdScr => StdScrInternal;
        public INativeScreenWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> Screen => ScreenInternal;
        public INativePadWrapper<IMultiByteNCursesChar, IMultiByteNCursesCharString, IMultiByteChar, IMultiByteCharString, ISingleByteNCursesChar, ISingleByteNCursesCharString, ISingleByteChar, ISingleByteCharString, IMEVENT> Pad => PadInternal;
        public IWindowFactory WindowFactory => WindowFactoryInternal;

        internal static IMultiByteWrapper<TMultiByte,TWideChar,TSingleByte,TChar> MultiByteWrapper { get; }
        internal static ISingleByteWrapper<TSingleByte, TChar, TMouseEvent> SingleByteWrapper { get; }
        internal static IWideCharWrapper<TWideChar, TChar> WideCharWrapper { get; }
        internal static ICharWrapper<TChar> CharWrapper { get; }

        internal static NativeNCursesInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> NCursesInternal { get; }
        internal static NativeWindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> WindowInternal { get; }
        internal static NativeStdScrInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> StdScrInternal { get; }
        internal static NativeScreenInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> ScreenInternal { get; }
        internal static NativePadInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> PadInternal { get; }

        internal static PlatformWideCharEncoder<TWideChar> WideCharEncoder { get; }
        internal static WindowFactory<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> WindowFactoryInternal { get; }

        internal static CreateBuffer<TChar> CreatePooledBuffer;

        static NativeCustomTypeWrapper()
        {
            object wrapper = Activator.CreateInstance(Constants.NCursesCharWrapper);

            MultiByteWrapper = (IMultiByteWrapper<TMultiByte, TWideChar, TSingleByte, TChar>)wrapper;
            SingleByteWrapper = (ISingleByteWrapper<TSingleByte, TChar, TMouseEvent>)wrapper;
            WideCharWrapper = (IWideCharWrapper<TWideChar, TChar>)wrapper;
            CharWrapper = (ICharWrapper<TChar>)wrapper;

            NCursesInternal = new NativeNCursesInternal<
                TMultiByte,
                TWideChar,
                TSingleByte,
                TChar,
                TMouseEvent>(MultiByteWrapper, SingleByteWrapper, WideCharWrapper, CharWrapper);

            WindowInternal = new NativeWindowInternal<
                TMultiByte,
                TWideChar,
                TSingleByte,
                TChar,
                TMouseEvent>(MultiByteWrapper, SingleByteWrapper, WideCharWrapper, CharWrapper);

            StdScrInternal = new NativeStdScrInternal<
                TMultiByte,
                TWideChar,
                TSingleByte,
                TChar,
                TMouseEvent>(MultiByteWrapper, SingleByteWrapper, WideCharWrapper, CharWrapper);

            ScreenInternal = new NativeScreenInternal<
                TMultiByte,
                TWideChar,
                TSingleByte,
                TChar,
                TMouseEvent>(MultiByteWrapper, SingleByteWrapper, WideCharWrapper, CharWrapper);

            PadInternal = new NativePadInternal<
                TMultiByte,
                TWideChar,
                TSingleByte,
                TChar,
                TMouseEvent>(MultiByteWrapper, SingleByteWrapper);

            WindowFactoryInternal = new WindowFactory<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>();
        }
    }
}
