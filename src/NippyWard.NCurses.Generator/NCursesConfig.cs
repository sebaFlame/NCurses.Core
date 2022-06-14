using System;
using System.Collections.Generic;
using System.Text;

namespace NippyWard.NCurses.Generator
{
    public struct NCursesConfig
    {
        public string NativeNCurses { get; }
        public string NativePanel { get; }
        public int WCharSize { get; }
        public string Chtype { get; }

        public NCursesConfig(string nativeNCurses, string nativePanel, int wCharSize, string chtype)
        {
            this.NativeNCurses = nativeNCurses;
            this.NativePanel = nativePanel;
            this.WCharSize = wCharSize;
            this.Chtype = chtype;
        }
    }
}
