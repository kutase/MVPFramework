using Zenject;

namespace MVPFramework.Widgets
{
    public interface IPoolableWidget : IWidget, IPoolable
    {
    }

    public interface IPoolableWidget<in TView, in TModel>: IPoolableWidget where TView : IWidgetView
    {
        void Init(TView view, TModel model);
    }
}