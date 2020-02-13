using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Dynamic;
using NCurses.Core.Interop.Mouse;

namespace NCurses.Core.Interop.Wrappers
{
    internal class NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : ICustomTypeWrapper
        where TMultiByte : unmanaged, IMultiByteChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteChar, IEquatable<TSingleByte>
        where TChar : unmanaged, IChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        public INativeNCursesWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> NCurses => NCursesInternal;
        public INativeWindowWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> Window => WindowInternal;
        public INativeStdScrWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> StdScr => StdScrInternal;
        public INativeScreenWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> Screen => ScreenInternal;
        public INativePadWrapper<IMultiByteChar, IMultiByteCharString, IChar, ICharString, ISingleByteChar, ISingleByteCharString, IChar, ICharString, IMEVENT> Pad => PadInternal;
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

        internal static WindowFactory<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> WindowFactoryInternal { get; }

        static NativeCustomTypeWrapper()
        {
            object wrapper = Activator.CreateInstance(DynamicTypeBuilder.CreateCustomTypeWrapper(Constants.DLLNAME, NativeNCurses.HasUnicodeSupport));

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
