using MVPFramework.Events;

namespace MVPFramework.Widgets
{
    public class CompositeWidgetArgs : WidgetBaseArgs
    {
        public IWidgetFactory WidgetFactory { get; }

        protected CompositeWidgetArgs(IWidgetFactory widgetFactory, 
            IEventsStore eventsStore) : base(eventsStore)
        {
            WidgetFactory = widgetFactory;
        }
    }
}