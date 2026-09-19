using System;
using UnityEngine.Events;
using Zenject;

namespace MVPFramework.Events
{
    public interface IEventsSubscriber: IPoolable
    {
        void Subscribe(UnityEvent unityEvent, UnityAction handler);
        void Subscribe<T>(UnityEvent<T> unityEvent, UnityAction<T> handler);
        void Subscribe<T1, T2>(UnityEvent<T1, T2> unityEvent, UnityAction<T1, T2> handler);
        void Subscribe(Action subscribe, Action unsubscribe);
        void SubscribeSignal<T>(Action<T> handler);
        void Unsubscribe();
    }
}
