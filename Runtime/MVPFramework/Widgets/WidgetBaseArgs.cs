using DITools;
using MVPFramework.Events;

namespace MVPFramework.Widgets
{
    public class WidgetBaseArgs: IContainerConstructable
    {
        protected WidgetBaseArgs(IEventsStore eventsStore)
        {
            EventsStore = eventsStore;
        }

        public IEventsStore EventsStore { get; }
    }
}