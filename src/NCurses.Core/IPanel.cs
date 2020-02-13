using System;

namespace NCurses.Core
{
    public interface IPanel : IEquatable<IPanel>, IDisposable
    {
        bool Hidden { get; }

        IWindow WrappedWindow { get; set; }

        IPanel Above();
        IPanel Below();

        void Bottom();
        void Hide();
        void Move(int starty, int startx);
        void Show();
        void Top();
    }
}