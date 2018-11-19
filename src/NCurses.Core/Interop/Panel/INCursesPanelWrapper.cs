using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop.Panel
{
    public interface INCursesPanelWrapper
    {
        //PANEL *new_panel(WINDOW *win);
        IntPtr new_panel(IntPtr win);
        //int bottom_panel(PANEL *pan);
        int bottom_panel(IntPtr pan);
        //int top_panel(PANEL *pan);
        int top_panel(IntPtr pan);
        //int show_panel(PANEL *pan);
        int show_panel(IntPtr pan);
        //void update_panels();
        void update_panels();
        //int hide_panel(PANEL *pan);
        int hide_panel(IntPtr pan);
        //WINDOW *panel_window(const PANEL *pan);
        IntPtr panel_window(IntPtr pan);
        //int replace_panel(PANEL *pan, WINDOW *window);
        int replace_panel(IntPtr pan, IntPtr window);
        //int move_panel(PANEL *pan, int starty, int startx);
        int move_panel(IntPtr pan, int starty, int startx);
        //int panel_hidden(const PANEL *pan);
        int panel_hidden(IntPtr pan);
        //PANEL *panel_above(const PANEL *pan);
        IntPtr panel_above(IntPtr pan);
        //PANEL *panel_below(const PANEL *pan);
        IntPtr panel_below(IntPtr pan);
        //int set_panel_userptr(PANEL *pan, const void *ptr);
        int set_panel_userptr(IntPtr pan, IntPtr ptr);
        //const void *panel_userptr(const PANEL *pan);
        IntPtr panel_userptr(IntPtr pan);
        //int del_panel(PANEL *pan);
        int del_panel(IntPtr pan);
    }
}
