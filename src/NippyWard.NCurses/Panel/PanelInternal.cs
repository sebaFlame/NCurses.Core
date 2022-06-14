using System;
using System.Collections.Generic;

using NippyWard.NCurses.Interop;
using NippyWard.NCurses.Interop.MultiByte;
using NippyWard.NCurses.Interop.SingleByte;
using NippyWard.NCurses.Interop.WideChar;
using NippyWard.NCurses.Interop.Char;
using NippyWard.NCurses.Interop.Mouse;
using NippyWard.NCurses.Interop.SafeHandles;
using NippyWard.NCurses.Interop.Wrappers;
using NippyWard.NCurses.Interop.Panel;

namespace NippyWard.NCurses.Panel
{
    public class PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : IPanel, IEquatable<PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>>, IDisposable
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        private WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> wrappedWindow;
        public WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> WrappedWindow
        {
            get
            {
                if (!(this.wrappedWindow is null))
                {
                    return this.wrappedWindow;
                }

                WindowBaseSafeHandle windowBaseSafeHandle = NativePanel.panel_window(this.PanelBaseSafeHandle);
                this.wrappedWindow = WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.CreateWindow(windowBaseSafeHandle);
                return this.wrappedWindow;
            }
            set
            {
                NativePanel.replace_panel(this.PanelBaseSafeHandle, value.WindowBaseSafeHandle);
                this.wrappedWindow = value;
            }
        }

        IWindow IPanel.WrappedWindow
        {
            get => this.WrappedWindow;
            set
            {
                if(!(value is WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> castedWindow))
                {
                    throw new InvalidOperationException("Invalid window type");
                }

                this.WrappedWindow = castedWindow;
            }
        }

        public bool Hidden => NativePanel.panel_hidden(this.PanelBaseSafeHandle);

        internal static Dictionary<PanelBaseSafeHandle, WeakReference<PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>>> DictPanels;

        internal static NativeNCursesInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> NCurses =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.NCursesInternal;
        internal static NativeWindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Window =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.WindowInternal;
        internal static NativeStdScrInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> StdScr =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.StdScrInternal;
        internal static NativeScreenInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Screen =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.ScreenInternal;
        internal static NativePadInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Pad =>
            NativeCustomTypeWrapper<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>.PadInternal;

        internal PanelBaseSafeHandle PanelBaseSafeHandle { get; private set; }

        private bool HasAddedRefToExistingSafeHandle = false;

        static PanelInternal()
        {
            DictPanels = new Dictionary<PanelBaseSafeHandle, WeakReference<PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>>>();
        }

        /// <summary>
        /// Create a new panel from a window and place it on top of the stack
        /// </summary>
        /// <param name="window"></param>
        public PanelInternal(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window)
        {
            this.PanelBaseSafeHandle = NativePanel.new_panel(window.WindowBaseSafeHandle);
            this.wrappedWindow = window;

            //only new panels are added
            DictPanels.Add(this.PanelBaseSafeHandle, new WeakReference<PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>>(this));
        }

        internal PanelInternal(PanelBaseSafeHandle panelBaseSafeHandle)
        {
            this.PanelBaseSafeHandle = panelBaseSafeHandle;
        }

        internal PanelInternal(PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingPanel)
        {
            this.PanelBaseSafeHandle = existingPanel.PanelBaseSafeHandle;

            this.PanelBaseSafeHandle.DangerousAddRef(ref this.HasAddedRefToExistingSafeHandle);
        }

        ~PanelInternal()
        {
            this.Dispose();
        }

        /// <summary>
        /// Make this panel visible and place it on top of the stack
        /// </summary>
        public void Show()
        {
            NativePanel.show_panel(this.PanelBaseSafeHandle);
        }

        /// <summary>
        /// Make this panel invisible
        /// </summary>
        public void Hide()
        {
            NativePanel.hide_panel(this.PanelBaseSafeHandle);
        }

        /// <summary>
        /// Place this panel on top of the stack
        /// </summary>
        public void Top()
        {
            NativePanel.top_panel(this.PanelBaseSafeHandle);
        }

        /// <summary>
        /// Palce this panel on the bottom of the stack
        /// </summary>
        public void Bottom()
        {
            NativePanel.bottom_panel(this.PanelBaseSafeHandle);
        }

        /// <summary>
        /// Move the position of this panel to <paramref name="startx"/> and <paramref name="starty"/>
        /// </summary>
        /// <param name="starty">The line to move the panel to</param>
        /// <param name="startx">The column to move the panel to</param>
        public void Move(int starty, int startx)
        {
            NativePanel.move_panel(this.PanelBaseSafeHandle, starty, startx);
        }

        public void Replace(IWindow window)
        {
            if(!(window is WindowBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> wBase))
            {
                throw new NotSupportedException("Unsupported window type");
            }

            NativePanel.replace_panel(this.PanelBaseSafeHandle, wBase.WindowBaseSafeHandle);
        }

        /// <summary>
        /// Get the panel above the current panel
        /// </summary>
        public PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Above()
        {
            PanelBaseSafeHandle panelBaseSafeHandle = NativePanel.panel_above(this.PanelBaseSafeHandle);
            if (DictPanels.TryGetValue(panelBaseSafeHandle, out WeakReference<PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>> weakRef)
                && weakRef.TryGetTarget(out PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>  existingPanel))
            {
                return new PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(existingPanel);
            }

            return new PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(panelBaseSafeHandle);
        }

        IPanel IPanel.Above() => this.Above();

        /// <summary>
        /// Get the panel below the current panel
        /// </summary>
        public PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Below()
        {
            PanelBaseSafeHandle panelBaseSafeHandle = NativePanel.panel_below(this.PanelBaseSafeHandle);
            if (DictPanels.TryGetValue(panelBaseSafeHandle, out WeakReference<PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>> weakRef)
                && weakRef.TryGetTarget(out PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingPanel))
            {
                return new PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(existingPanel);
            }

            return new PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(panelBaseSafeHandle);
        }

        IPanel IPanel.Below() => this.Below();

        /// <summary>
        /// Update virtual screen with the current panel stack
        /// Call <see cref="NCurses.Update"/> to update the screen
        /// </summary>
        public static void UpdatePanels()
        {
            NativePanel.update_panels();
        }

        #region Equality
        public override bool Equals(object obj)
        {
            if (obj is PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> panel)
            {
                return this.Equals(panel);
            }
            return false;
        }

        public bool Equals(IPanel other)
        {
            if (other is PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> panel)
            {
                return this.Equals(panel);
            }
            return false;
        }

        public bool Equals(PanelInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> other)
        {
            if (!(this.PanelBaseSafeHandle is null && other.PanelBaseSafeHandle is null))
            {
                return this.PanelBaseSafeHandle.Equals(other.PanelBaseSafeHandle);
            }
            else
            {
                return object.ReferenceEquals(this, other);
            }
        }

        public override int GetHashCode()
        {
            if (this.PanelBaseSafeHandle is null)
            {
                return base.GetHashCode();
            }

            return this.PanelBaseSafeHandle.GetHashCode();
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            if (this.PanelBaseSafeHandle is null)
            {
                return;
            }

            if (DictPanels.ContainsKey(this.PanelBaseSafeHandle))
            {
                DictPanels.Remove(this.PanelBaseSafeHandle);
            }

            if (this.HasAddedRefToExistingSafeHandle)
            {
                this.PanelBaseSafeHandle?.DangerousRelease();
            }
            else
            {
                this.PanelBaseSafeHandle?.Dispose();
            }

            this.PanelBaseSafeHandle = null;
            this.wrappedWindow = null;

            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
