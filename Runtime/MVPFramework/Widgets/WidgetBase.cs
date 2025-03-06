using System;
using MVPFramework.Events;
using UnityEngine.Events;

namespace MVPFramework.Widgets
{
    public abstract class WidgetBase<TView, TViewModel, TData> : IWidget<TView, TData>
        where TView : IWidgetView<TViewModel>
        where TData : TViewModel
    {
        private readonly IEventsStore eventsStore;
        protected TData Data;
        protected IWidgetProps Props;
        protected TView View;

        protected WidgetBase(WidgetBaseArgs args)
        {
            eventsStore = args.EventsStore;
        }

        public bool IsActivated { get; private set; }

        public virtual void Bind(TView view, TData data, bool activate = false)
        {
            View = view;
            Data = data;

            if (activate && !IsManualLifecycleMode)
            {
                Activate();
            }
        }

        public virtual void Activate()
        {
            if (View == null)
                return;

            View.Activate(Data);

            SubscribeEvents();
            IsActivated = true;
        }

        public void SetLifecycleMode(bool manual)
        {
            IsManualLifecycleMode = manual;
        }

        public virtual void SetProps(IWidgetProps widgetProps = null)
        {
            Props = widgetProps;
        }

        public bool IsManualLifecycleMode { get; private set; }

        public virtual void Deactivate()
        {
            UnsubscribeEvents();
            if (View == null)
                return;
            View.Deactivate();

            IsActivated = false;
        }

        public IWidgetView GetView()
        {
            if (View != null && View.IsAlive())
                return View;
            return default;
        }

        protected void SubscribeEvent(object subscriber, UnityEvent unityEvent, UnityAction handler)
        {
            eventsStore.SubscribeEvent(subscriber, unityEvent, handler);
        }

        protected void SubscribeEvent<T>(object subscriber, UnityEvent<T> unityEvent, UnityAction<T> handler)
        {
            eventsStore.SubscribeEvent(subscriber, unityEvent, handler);
        }
        
        protected void SubscribeEvent<T1, T2>(object subscriber, UnityEvent<T1, T2> unityEvent, UnityAction<T1, T2> handler)
        {
            eventsStore.SubscribeEvent(subscriber, unityEvent, handler);
        }
        
        protected void SubscribeSignal<T>(Action<T> handler)
        {
            eventsStore.SubscribeSignal(this, handler);
        }

        protected virtual void SubscribeEvents()
        {
        }

        protected void UnsubscribeEvents()
        {
            UnsubscribeEvents(this);
        }

        private void UnsubscribeEvents(object owner)
        {
            eventsStore.Unsubscribe(owner);
        }

        public virtual void Rebind(TData model)
        {
            Data = model;
            Deactivate();
            Activate();
        }

        public void DetachView()
        {
            View = default;
        }
    }

    public abstract class WidgetBase<TView, TData> : WidgetBase<TView, TData, TData>
        where TView : IWidgetView<TData>
    {
        protected WidgetBase(WidgetBaseArgs args) : base(args)
        {
        }
    }
}