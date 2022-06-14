using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Window;
using NippyWard.NCurses.Pad;
using NippyWard.NCurses.Panel;
using NippyWard.NCurses.StdScr;

using NippyWard.NCurses.Interop;
using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.SafeHandles;
using NippyWard.NCurses.Interop.Wrappers;

namespace NippyWard.NCurses
{
    internal class WindowFactory<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : IWindowFactory
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        internal static StdScrBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateStdScrInternal(StdScrSafeHandle stdScrSafeHandle)
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteStdScr<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(stdScrSafeHandle);
            }
            else
            {
                return new SingleByteStdScr<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(stdScrSafeHandle);
            }
        }

        internal static StdScrBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateStdScrInternal(IWindow existingStdScr)
        {
            if(!(existingStdScr is StdScrBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> stdScr))
            {
                throw new InvalidOperationException("Incorrect StdScr type used");
            }

            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteStdScr<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(stdScr);
            }
            else
            {
                return new SingleByteStdScr<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(stdScr);
            }
        }

        internal static WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindowInternal(int nlines, int ncols, int begy, int begx)
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols, begy, begx);
            }
            else
            {
                return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols, begy, begx);
            }
        }

        internal static WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindowInternal(int nlines, int ncols)
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols);
            }
            else
            {
                return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols);
            }
        }

        internal static WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindowInternal()
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>();
            }
            else
            {
                return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>();
            }
        }

        internal static WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindowInternal(WindowBaseSafeHandle windowBaseSafeHandle)
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle);
            }
            else
            {
                return new SingleByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle);
            }
        }

        internal static PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreatePadInternal(IWindow window, int nlines, int ncols)
        {
            if (!(window is WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> castedWindow))
            {
                throw new InvalidOperationException("Incorrect window type used");
            }

            if (castedWindow.HasUnicodeSupport)
            {
                return new MultiBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols);
            }
            else
            {
                return new SingleBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols);
            }
        }

        internal static PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreatePadInternal(int nlines, int ncols)
        {
            if (NativeNCurses.HasUnicodeSupport)
            {
                return new MultiBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols);
            }
            else
            {
                return new SingleBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(nlines, ncols);
            }
        }

        internal static PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreatePanelInternal(IWindow window)
        {
            if(!(window is WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> castedWindow))
            {
                throw new InvalidOperationException("Incorrect StdScr type used");
            }

            return new PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(castedWindow);
        }

        public IWindow CreateStdScr(StdScrSafeHandle stdScrSafeHandle) => CreateStdScrInternal(stdScrSafeHandle);

        public IWindow CreateStdScr(IWindow existingStdScr) => CreateStdScrInternal(existingStdScr);

        public IWindow CreateWindow(int nlines, int ncols, int begy, int begx) => CreateWindowInternal(nlines, ncols, begy, begx);

        public IWindow CreateWindow(int nlines, int ncols) => CreateWindowInternal(nlines, ncols);

        public IWindow CreateWindow() => CreateWindowInternal();

        public IWindow CreateWindow(WindowBaseSafeHandle windowBaseSafeHandle) => CreateWindowInternal(windowBaseSafeHandle);

        public IPad CreatePad(IWindow window, int nlines, int ncols) => CreatePadInternal(window, nlines, ncols);

        public IPad CreatePad(int nlines, int ncols) => CreatePadInternal(nlines, ncols);

        public IPanel CreatePanel(IWindow window) => CreatePanelInternal(window);

        public bool HasWindows => WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.Windows.Count > 0;

        public bool HasPanels => PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.DictPanels.Count > 0;
    }
}
