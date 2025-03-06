using UnityEngine;

namespace MVPFramework.Widgets
{
    public interface IWidgetView<in TModel>: IWidgetView
    {
        void Activate(TModel model);
    }

    public interface IWidgetView
    {
        Transform Transform { get; }
        void Deactivate();
        bool IsAlive();
    }
}