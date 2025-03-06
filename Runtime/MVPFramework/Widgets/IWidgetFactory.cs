namespace MVPFramework.Widgets
{
    public interface IWidgetFactory
    {
        T Create<T>()  where T: IWidget;
        TWidget Spawn<TWidget>() where TWidget : IPoolableWidget;
        void Despawn<TWidget>(TWidget widget) where TWidget : IPoolableWidget;
    }
}