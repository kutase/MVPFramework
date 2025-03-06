using System;
using System.Collections.Generic;
using DITools;
using UnityEngine.Events;
using Zenject;

namespace MVPFramework.Events
{
    public class EventsStore : IEventsStore, IContainerConstructable
    {
        public EventsStore()
        {
            subscribersPool = new StaticMemoryPool<EventsSubscriber>(item => item.OnSpawned(), item => item.OnDespawned());
        }

        private readonly IDictionary<object, EventsSubscriber> subscribers = new Dictionary<object, EventsSubscriber>();
        private readonly StaticMemoryPool<EventsSubscriber> subscribersPool;
        
        public void SubscribeEvent(object subscriber, UnityEvent unityEvent, UnityAction handler)
        {
            var value = GetSubscriber(subscriber);
            value.Subscribe(unityEvent, handler);
        }

        public void SubscribeEvent<T>(object subscriber, UnityEvent<T> unityEvent, UnityAction<T> handler)
        {
            var value = GetSubscriber(subscriber);
            value.Subscribe(unityEvent, handler);
        }

        public void SubscribeEvent<T1, T2>(object subscriber, UnityEvent<T1, T2> unityEvent, UnityAction<T1, T2> handler)
        {
            var value = GetSubscriber(subscriber);
            value.Subscribe(unityEvent, handler);
        }

        public void SubscribeSignal<T>(object subscriber, Action<T> handler)
        {
            var value = GetSubscriber(subscriber);
            value.SubscribeSignal(handler);
        }

        public void Unsubscribe(object subscriber)
        {
            if (!subscribers.TryGetValue(subscriber, out var value))
                return;
            
            value.Unsubscribe();
            subscribersPool.Despawn(value); 
            subscribers.Remove(subscriber);
        }

        private EventsSubscriber GetSubscriber(object subscriber)
        {
            if (subscribers.TryGetValue(subscriber, out var value)) return value;
            
            value = subscribersPool.Spawn();
            subscribers[subscriber] = value;
            return value;
        }
    }
}