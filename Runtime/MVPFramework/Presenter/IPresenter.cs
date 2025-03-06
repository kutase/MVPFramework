using System;
using MVPFramework.View;

namespace MVPFramework.Presenter
{
    public interface IPresenter<TScreenType> where TScreenType : struct, IComparable
    {
        TScreenType Type { get; }
        PresenterState State { get; }
        void AddView(IScreenView view);
        void AddOpenParams(IScreenParams openParams);
        void Activate();
        void Deactivate();
        void Hide();
    }
}