using System.Collections.Generic;

namespace MVPFramework.Widgets
{
    public abstract class CompositeWidget<TView, TViewModel, TModel> : WidgetBase<TView, TViewModel, TModel>
        where TView : IWidgetView<TViewModel>
        where TModel: TViewModel
    {
        private readonly LinkedList<IWidget> widgets = new();
        private readonly IWidgetFactory widgetFactory;

        protected CompositeWidget(CompositeWidgetArgs args) : base(args)
        {
            widgetFactory = args.WidgetFactory;
        }

        protected T CreateWidget<T>(IWidgetProps widgetProps = null) where T : IWidget
        {
            var widget = widgetFactory.Create<T>();
            widget.SetProps(widgetProps ?? Props);
            widgets.AddLast(widget);
            return widget;
        }

        protected T SpawnWidget<T>(IWidgetProps widgetProps = null) where T : IPoolableWidget
        {
            var widget = widgetFactory.Spawn<T>();
            widget.SetProps(widgetProps ?? Props);
            widgets.AddLast(widget);
            return widget;
        }

        protected virtual void CreateWidgets()
        {
        }

        protected virtual void OnDeactivated()
        {
        }

        public override void Activate()
        {
            CreateWidgets();
            foreach (var widget in widgets)
                widget.Activate();

            base.Activate();
        }

        public override void Deactivate()
        {
            base.Deactivate();

            foreach (var widget in widgets) 
                DeactivateChildWidget(widget);

            widgets.Clear();
            OnDeactivated();
        }

        private void DeactivateChildWidget(IWidget widget)
        {
            OnBeforeChildWidgetDeactivated(widget);
            widget.Deactivate();

            if (widget is IPoolableWidget poolableWidget) 
                widgetFactory.Despawn(poolableWidget);
        }

        protected void RemoveChildControl(IWidget widget)
        {
            if (widgets.Remove(widget))
                DeactivateChildWidget(widget);
        }

        protected virtual void OnBeforeChildWidgetDeactivated(IWidget widget) { }
    }

    public abstract class CompositeWidget<TView, TModel> : CompositeWidget<TView, TModel, TModel>
        where TView : IWidgetView<TModel>
    {
        protected CompositeWidget(CompositeWidgetArgs args) : base(args)
        {
        }
    }
}