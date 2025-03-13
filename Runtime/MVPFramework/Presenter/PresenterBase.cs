using System;
using System.Collections.Generic;
using DITools;
using MVPFramework.Events;
using MVPFramework.Model;
using MVPFramework.View;
using MVPFramework.Widgets;
using UnityEngine.Events;
using Zenject;

namespace MVPFramework.Presenter
{
    public abstract class PresenterBase<TView, TModel, TScreenType> : IPresenter<TScreenType>, IContainerConstructable
        where TView : IScreenView
        where TModel : IModel
        where TScreenType : struct, IComparable
    {
        protected readonly IWidgetFactory WidgetFactory;
        private readonly IEventsStore eventsStore;

        private readonly LinkedList<IWidget> widgets = new();
        private readonly LinkedList<IWidget> pooledWidgets = new();
        protected readonly TModel Model;
        protected TView View;

        protected IScreenParams OpenParams = null;

        protected PresenterBase(PresenterBaseArgs args, TModel model)
        {
            WidgetFactory = args.WidgetFactory;
            eventsStore = args.EventsStore;
            Model = model;
        }

        public abstract TScreenType Type { get; }

        public PresenterState State { get; private set; } = PresenterState.NotInitialized;

        public void AddView(IScreenView view)
        {
            View = (TView) view;
        }

        public void AddOpenParams(IScreenParams openParams)
        {
            OpenParams = openParams;
        }

        public virtual void Activate()
        {
            if (State is PresenterState.NotInitialized || State is PresenterState.Inactive)
            {
                Model.Activate(OpenParams);
                OnCustomizeModel();
            }

            if (State == PresenterState.NotInitialized)
            {
                CreateWidgets();
                State = PresenterState.Inactive;
            }

            if (State == PresenterState.Inactive)
            {
                SubscribeEvents();
                OnActivate();
                OnBindWidgets();
                ActivateWidgets();
                OnWidgetsActivated();
            }

            if (State == PresenterState.Hidden)
            {
                Model.OnShow();
                OnShow();
            }

            State = PresenterState.Active;

            View.Show();
        }

        protected virtual void OnCustomizeModel()
        {
            
        }

        protected virtual void OnWidgetsActivated()
        {
            
        }

        public virtual void Deactivate()
        {
            if (State is PresenterState.NotInitialized || State is PresenterState.Inactive) return;

            Model.Deactivate();
            OnDeactivate();
            UnsubscribeEvents();
            DeactivateWidgets();
            State = PresenterState.Inactive;

            View.Hide();
        }

        public void Hide()
        {
            if (State != PresenterState.Active) return;

            OnHide();
            State = PresenterState.Hidden;
        }

        protected virtual void CreateWidgets()
        {
        }

        protected T CreateWidget<T>(IWidgetProps widgetProps = null, bool manualWidget = false) where T : IWidget
        {
            var widget = WidgetFactory.Create<T>();
            widget.SetLifecycleMode(manualWidget);
            widget.SetProps(widgetProps);
            widgets.AddLast(widget);
            return widget;
        }

        protected T SpawnWidget<T>(IWidgetProps widgetProps = null) where T : IPoolableWidget
        {
            var widget = WidgetFactory.Spawn<T>();
            widget.SetProps(widgetProps);
            pooledWidgets.AddLast(widget);
            return widget;
        }

        private void ActivateWidgets()
        {
            foreach (var widget in widgets)
                if (!widget.IsManualLifecycleMode)
                    widget.Activate();

            foreach (var widget in pooledWidgets)
                if (!widget.IsManualLifecycleMode)
                    widget.Activate();
        }

        private void DeactivateWidgets()
        {
            List<IWidget> poolableWidgets = null;
            foreach (var widget in widgets)
            {
                OnBeforeChildWidgetDeactivated(widget);
                if (!widget.IsManualLifecycleMode) 
                    widget.Deactivate();
            }

            foreach (var widget in pooledWidgets)
            {
                var poolableWidget = widget as IPoolableWidget;
                WidgetFactory.Despawn(poolableWidget);
                poolableWidgets ??= ListPool<IWidget>.Instance.Spawn();
                poolableWidgets.Add(widget);
            }

            if (poolableWidgets == null) return;

            foreach (var poolableWidget in poolableWidgets) 
                pooledWidgets.Remove(poolableWidget);

            ListPool<IWidget>.Instance.Despawn(poolableWidgets);
        }

        protected void RemoveWidget(IWidget widget)
        {
            if (widgets.Remove(widget))
                DeactivateWidget(widget);
        }

        protected void DespawnWidget(IWidget widget)
        {
            if (pooledWidgets.Remove(widget))
                DeactivateWidget(widget);
        }

        private void DeactivateWidget(IWidget widget)
        {
            OnBeforeChildWidgetDeactivated(widget);

            if (!widget.IsManualLifecycleMode)
                widget.Deactivate();

            if (!(widget is IPoolableWidget poolableWidget)) 
                return;

            WidgetFactory.Despawn(poolableWidget);
        }

        protected virtual void OnShow()
        {
        }

        protected virtual void OnActivate()
        {
        }

        protected virtual void OnBindWidgets()
        {
        }

        protected virtual void OnDeactivate()
        {
        }

        protected virtual void OnHide()
        {
        }

        protected virtual void OnBeforeChildWidgetDeactivated(IWidget widget)
        {
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

        protected virtual void UnsubscribeEvents()
        {
            eventsStore.Unsubscribe(this);
        }

        protected virtual void UnsubscribeEvents(object subscriber)
        {
            eventsStore.Unsubscribe(subscriber);
        }
    }
}