using System;
using System.Collections.Generic;
using System.Text;

using NippyWard.NCurses.Interop.SafeHandles;

namespace NippyWard.NCurses.Interop.Panel
{
    public interface INCursesPanelWrapper
    {
        //PANEL *new_panel(WINDOW *win);
        NewPanelSafeHandle new_panel(WindowBaseSafeHandle win);
        //int bottom_panel(PANEL *pan);
        int bottom_panel(PanelBaseSafeHandle pan);
        //int top_panel(PANEL *pan);
        int top_panel(PanelBaseSafeHandle pan);
        //int show_panel(PANEL *pan);
        int show_panel(PanelBaseSafeHandle pan);
        //void update_panels();
        void update_panels();
        //int hide_panel(PANEL *pan);
        int hide_panel(PanelBaseSafeHandle pan);
        //WINDOW *panel_window(const PANEL *pan);
        WindowSafeHandle panel_window(PanelBaseSafeHandle pan);
        //int replace_panel(PANEL *pan, WINDOW *window);
        int replace_panel(PanelBaseSafeHandle pan, WindowBaseSafeHandle window);
        //int move_panel(PANEL *pan, int starty, int startx);
        int move_panel(PanelBaseSafeHandle pan, int starty, int startx);
        //int panel_hidden(const PANEL *pan);
        int panel_hidden(PanelBaseSafeHandle pan);
        //PANEL *panel_above(const PANEL *pan);
        PanelSafeHandle panel_above(PanelBaseSafeHandle pan);
        //PANEL *panel_below(const PANEL *pan);
        PanelSafeHandle panel_below(PanelBaseSafeHandle pan);
        //int set_panel_userptr(PANEL *pan, const void *ptr);
        int set_panel_userptr(PanelBaseSafeHandle pan, IntPtr ptr);
        //const void *panel_userptr(const PANEL *pan);
        IntPtr panel_userptr(PanelBaseSafeHandle pan);
        //int del_panel(PANEL *pan);
        int del_panel(IntPtr pan);
    }
}
