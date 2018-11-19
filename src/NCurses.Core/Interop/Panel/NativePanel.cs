using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Dynamic;

namespace NCurses.Core.Interop.Panel
{
    internal static class NativePanel
    {
        private static INCursesPanelWrapper wrapper;
        public static INCursesPanelWrapper NCursesWrapper
        {
            get
            {
                if (wrapper is null)
                    wrapper = (INCursesPanelWrapper)Activator.CreateInstance(DynamicTypeBuilder.CreateDefaultWrapper<INCursesPanelWrapper>(Constants.DLLPANELNAME));
                return wrapper;
            }
        }

        /// <summary>
        /// allocates   a   PANEL  structure, associates it with win, places
        /// the panel on the top of the stack(causes it to be  displayed
        /// above any other panel) and returns a pointer to the new panel.
        /// </summary>
        /// <param name="window">The window to create a panel for</param>
        /// <returns>A pointer to the newly created panel</returns>
        public static IntPtr new_panel(IntPtr window)
        {
            return NCursesException.Verify(NCursesWrapper.new_panel(window), "new_panel");
        }

        /// <summary>
        /// puts panel at the bottom of all panels.
        /// </summary>
        /// <param name="panel"></param>
        public static void bottom_panel(IntPtr panel)
        {
            NCursesException.Verify(NCursesWrapper.bottom_panel(panel), "bottom_panel");
        }

        /// <summary>
        /// puts  the given visible panel on top of all panels in the stack.
        /// </summary>
        /// <param name="panel"></param>
        public static void top_panel(IntPtr panel)
        {
            NCursesException.Verify(NCursesWrapper.top_panel(panel), "top_panel");
        }
        
        /// <summary>
        /// makes a hidden panel visible by placing it on top of the  panels
        /// in the panel stack.
        /// </summary>
        /// <param name="panel"></param>
        public static void show_panel(IntPtr panel)
        {
            NCursesException.Verify(NCursesWrapper.show_panel(panel), "show_panel");
        }

        /// <summary>
        /// refreshes  the  virtual  screen to reflect the relations between
        /// the panels in the stack, but does not call doupdate  to refresh
        /// the physical  screen.Use this  function and not wrefresh or
        /// wnoutrefresh.  update_panels may be called more than once before
        /// a call to doupdate, but doupdate is the function responsible for
        /// updating the physical screen.
        /// </summary>
        public static void update_panels()
        {
            NCursesWrapper.update_panels();
        }

        /// <summary>
        /// removes  the  given panel from the panel stack and thus hides it
        /// from view.The PANEL structure is not lost, merely removed from
        /// the stack.
        /// </summary>
        /// <param name="panel">The panel to hide</param>
        public static void hide_panel(IntPtr panel)
        {
            NCursesException.Verify(NCursesWrapper.hide_panel(panel), "hide_panel");
        }

        /// <summary>
        /// returns a pointer to the window of the given panel.
        /// </summary>
        /// <param name="panel">The panel to get the window for</param>
        /// <returns>A pointer to the window of the given panel</returns>
        public static IntPtr panel_window(IntPtr panel)
        {
            return NCursesException.Verify(NCursesWrapper.panel_window(panel), "panel_window");
        }

        /// <summary>
        /// replaces  the  current  window of panel with window (useful, for
        /// example if you want to resize a panel; if you're using  ncurses,
        /// you can  call replace_panel on the output of <see cref="NativeWindow.wresize(IntPtr, int, int)"/>).  It
        /// does not change the position of the panel in the stack.
        /// </summary>
        /// <param name="panel">The panel for which you want to replace the window</param>
        /// <param name="window">The window to add to the panel</param>
        public static void replace_panel(IntPtr panel, IntPtr window)
        {
            NCursesException.Verify(NCursesWrapper.replace_panel(panel, window), "replace_panel");
        }

        /// <summary>
        /// moves the given panel window so that its upper-left corner is at
        /// starty, startx.It does not change the position of the panel in
        /// the stack.Be sure to use this function, not mvwin, to move  a
        /// panel window.
        /// </summary>
        /// <param name="panel">The panel to move</param>
        /// <param name="starty">the line number to move the panel to</param>
        /// <param name="startx">The column number to move the panel to</param>
        public static void move_panel(IntPtr panel, int starty, int startx)
        {
            NCursesException.Verify(NCursesWrapper.move_panel(panel, starty, startx), "move_panel");
        }

        /// <summary>
        /// returns  TRUE if the panel is in the panel stack, FALSE if it is
        /// not.If the panel is a null pointer, return ERR.
        /// </summary>
        /// <param name="panel">The panel to check</param>
        /// <returns>TRUE if hidden</returns>
        public static bool panel_hidden(IntPtr panel)
        {
            return NCursesException.Verify(NCursesWrapper.panel_hidden(panel), "panel_hidden") == 1;
        }
        
        /// <summary>
        /// returns a pointer to the panel above pan.  If the panel argument
        /// is  (PANEL*)0, it returns a pointer to the bottom panel in the
        /// stack.
        /// </summary>
        /// <param name="panel">The panel to check</param>
        /// <returns>A pointer to the panel above the checked panel</returns>
        public static IntPtr panel_above(IntPtr panel)
        {
            return NCursesException.Verify(NCursesWrapper.panel_above(panel), "panel_above");
        }

        /// <summary>
        /// returns a pointer to the panel just below  pan.   If  the  panel
        /// argument is (PANEL*)0, it returns a pointer to the top panel in
        /// the stack.
        /// </summary>
        /// <param name="panel">The panel to check</param>
        /// <returns>A pointer to the panel below the checked panel</returns>
        public static IntPtr panel_below(IntPtr panel)
        {
            return NCursesException.Verify(NCursesWrapper.panel_below(panel), "panel_below");
        }

        /// <summary>
        /// sets the panel's user pointer.
        /// </summary>
        /// <param name="panel">The panel to set the user pointer for</param>
        /// <param name="userPtr">The user pointer to set</param>
        public static void set_panel_userptr(IntPtr panel, IntPtr userPtr)
        {
            NCursesException.Verify(NCursesWrapper.set_panel_userptr(panel, userPtr), "set_panel_userptr");
        }

        /// <summary>
        /// returns the user pointer for a given panel.
        /// </summary>
        public static IntPtr panel_userptr(IntPtr panel)
        {
            return NCursesException.Verify(NCursesWrapper.panel_userptr(panel), "panel_userptr");
        }

        /// <summary>
        /// removes the given panel from  the   stack  and  deallocates  the
        /// PANEL structure(but not its associated window).
        /// </summary>
        /// <param name="panel">The panel to remove from the stack</param>
        public static void del_panel(IntPtr panel)
        {
            NCursesException.Verify(NCursesWrapper.del_panel(panel), "del_panel");
        }
    }
}
