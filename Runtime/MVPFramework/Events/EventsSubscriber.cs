using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace MVPFramework.Events
{
    public class EventsSubscriber: IEventsSubscriber
    {
        public EventsSubscriber()
        {
            eventSubscribers = new List<IEventSubscriber>();
        }

        private readonly List<IEventSubscriber> eventSubscribers;

        public void Subscribe(UnityEvent unityEvent, UnityAction handler)
        {
            var subscriber = EventSubscriber.Pool.Spawn(unityEvent, handler);
            eventSubscribers.Add(subscriber);
        }
            
        public void Subscribe<T>(UnityEvent<T> unityEvent, UnityAction<T> handler)
        {
            var subscriber = EventSubscriber<T>.Pool.Spawn(unityEvent, handler);
            eventSubscribers.Add(subscriber);
        }           
        
        public void Subscribe<T1, T2>(UnityEvent<T1, T2> unityEvent, UnityAction<T1, T2> handler)
        {
            var subscriber = EventSubscriber<T1, T2>.Pool.Spawn(unityEvent, handler);
            eventSubscribers.Add(subscriber);
        }

        public void SubscribeSignal<T>(Action<T> handler)
        {
            var subscriber = SignalSubscriber<T>.Pool.Spawn(handler);
            eventSubscribers.Add(subscriber);
        }

        public void Unsubscribe()
        {
            foreach (var subscriber in eventSubscribers) 
                subscriber.Unsubscribe();
        }
            
        public void OnDespawned()
        {
            foreach (var subscriber in eventSubscribers) 
                subscriber.OnDespawned();
            eventSubscribers.Clear();
        }

        public void OnSpawned()
        {
        }
    }
}