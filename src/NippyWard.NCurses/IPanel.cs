﻿using System;

namespace NippyWard.NCurses
{
    public interface IPanel : IEquatable<IPanel>, IDisposable
    {
        bool Hidden { get; }

        IWindow WrappedWindow { get; set; }

        void Replace(IWindow window);

        IPanel Above();
        IPanel Below();

        void Bottom();
        void Hide();
        void Move(int starty, int startx);
        void Show();
        void Top();
    }
}