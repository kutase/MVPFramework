namespace MVPFramework.Widgets
{
    public interface IWidget
    {
        bool IsManualLifecycleMode { get; }
        bool IsActivated { get; }
        void Deactivate();
        void Activate();
        void SetLifecycleMode(bool manual);
        void SetProps(IWidgetProps widgetProps = null);
        IWidgetView GetView();
    }

    public interface IWidget<in TView, in TModel> : IWidget where TView : IWidgetView
    {
        void Bind(TView view, TModel data, bool activate = false);
    }
}