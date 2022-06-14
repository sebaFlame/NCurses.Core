using System;

namespace NippyWard.NCurses.Interop.Platform
{
    public interface INativeLoader
    {
        IntPtr LoadModule(string moduleName);
        IntPtr GetSymbolPointer(IntPtr modulePtr, string symbolName);
        bool FreeModule(IntPtr modulePtr);
        void SetLocale();
    }
}
