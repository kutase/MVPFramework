using MVPFramework.Presenter;

namespace MVPFramework.Model
{
    public interface IModel
    {
        void Activate(IScreenParams openParams = null);
        void OnShow();
        void Deactivate();
    }

    public interface IModel<out T>: IModel
    {
        T Data { get; }
    }
}