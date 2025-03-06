using System;
using UnityEngine.Events;

namespace MVPFramework.Events
{
    public interface IEventsStore
    {
        void SubscribeEvent(object subscriber, UnityEvent unityEvent, UnityAction handler);
        void SubscribeEvent<T>(object subscriber, UnityEvent<T> unityEvent, UnityAction<T> handler);
        void SubscribeEvent<T1, T2>(object subscriber, UnityEvent<T1, T2> unityEvent, UnityAction<T1, T2> handler);
        void SubscribeSignal<T>(object subscriber, Action<T> handler);
        void Unsubscribe(object subscriber);
    }
}