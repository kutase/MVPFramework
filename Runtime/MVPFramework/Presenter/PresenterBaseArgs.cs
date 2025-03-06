using DITools;
using MVPFramework.Events;
using MVPFramework.Widgets;

namespace MVPFramework.Presenter
{
    public class PresenterBaseArgs: IContainerConstructable
    {
        public IWidgetFactory WidgetFactory { get; }
        public IEventsStore EventsStore { get; }

        public PresenterBaseArgs(IWidgetFactory widgetFactory, IEventsStore eventsStore)
        {
            WidgetFactory = widgetFactory;
            EventsStore = eventsStore;
        }
    }
}