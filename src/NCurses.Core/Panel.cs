using System;
using System.Collections.Generic;
using NCurses.Core.Interop.Panel;

namespace NCurses.Core
{
    public class Panel : IDisposable
    {
        private Window window;
        public Window Window
        {
            get => this.window;
            set
            {
                NativePanel.replace_panel(this.PanelPtr, value.WindowPtr);
                this.window = value;
            }
        }

        internal IntPtr PanelPtr { get; private set; }
        internal static Dictionary<IntPtr, Panel> DictPanel { get; private set; }

        public bool Hidden => NativePanel.panel_hidden(this.PanelPtr);

        static Panel()
        {
            DictPanel = new Dictionary<IntPtr, Panel>();
        }

        /// <summary>
        /// Create a new panel from a window and place it on top of the stack
        /// </summary>
        /// <param name="window"></param>
        public Panel(Window window)
        {
            DictPanel.Add(this.PanelPtr = NativePanel.new_panel(window.WindowPtr), this);
            this.window = window;
        }

        /// <summary>
        /// Make this panel visible and place it on top of the stack
        /// </summary>
        public void Show()
        {
            NativePanel.show_panel(this.PanelPtr);
        }

        /// <summary>
        /// Make this panel invisible
        /// </summary>
        public void Hide()
        {
            NativePanel.hide_panel(this.PanelPtr);
        }

        /// <summary>
        /// Place this panel on top of the stack
        /// </summary>
        public void Top()
        {
            NativePanel.top_panel(this.PanelPtr);
        }

        /// <summary>
        /// Palce this panel on the bottom of the stack
        /// </summary>
        public void Bottom()
        {
            NativePanel.bottom_panel(this.PanelPtr);
        }

        /// <summary>
        /// Move the position of this panel to <paramref name="startx"/> and <paramref name="starty"/>
        /// </summary>
        /// <param name="starty">The line to move the panel to</param>
        /// <param name="startx">The column to move the panel to</param>
        public void Move(int starty, int startx)
        {
            NativePanel.move_panel(this.PanelPtr, starty, startx);
        }

        /// <summary>
        /// Get the panel above the current panel
        /// </summary>
        public Panel Above()
        {
            if (DictPanel.TryGetValue(NativePanel.panel_above(this.PanelPtr), out Panel panel))
                return panel;
            throw new InvalidOperationException("Panel not found");
        }

        /// <summary>
        /// Get the panel below the current panel
        /// </summary>
        public Panel Below()
        {
            if (DictPanel.TryGetValue(NativePanel.panel_below(this.PanelPtr), out Panel panel))
                return panel;
            throw new InvalidOperationException("Panel not found");
        }

        /// <summary>
        /// Update virtual screen with the current panel stack
        /// Call <see cref="NCurses.Update"/> to update the screen
        /// </summary>
        public static void UpdatePanels()
        {
            NativePanel.update_panels();
        }

        public void Dispose()
        {
            NativePanel.del_panel(this.PanelPtr);
            DictPanel.Remove(this.PanelPtr);
        }
    }
}
