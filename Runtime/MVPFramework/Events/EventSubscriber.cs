using UnityEngine.Events;
using Zenject;

namespace MVPFramework.Events
{
    public class EventSubscriber<T1, T2> : IEventSubscriber
    {
        public static readonly StaticMemoryPool<UnityEvent<T1, T2>, UnityAction<T1, T2>, EventSubscriber<T1, T2>> Pool = new StaticMemoryPool<UnityEvent<T1, T2>, UnityAction<T1, T2>, EventSubscriber<T1, T2>>((eventArg, action, item) => item.OnSpawned(eventArg, action));

        private UnityEvent<T1, T2> unityEvent;
        private UnityAction<T1, T2> handler;

        private void OnSpawned(UnityEvent<T1, T2> eventArg, UnityAction<T1, T2> action)
        {
            handler = action;
            unityEvent = eventArg;
            unityEvent.AddListener(action);
        }

        public void Unsubscribe()
        {
            unityEvent.RemoveListener(handler);
        }

        public void OnDespawned()
        {
            unityEvent = default;
            handler = default;
            Pool.Despawn(this);
        }
    }
    
    public class EventSubscriber<T> : IEventSubscriber
    {
        public static readonly StaticMemoryPool<UnityEvent<T>, UnityAction<T>, EventSubscriber<T>> Pool = new StaticMemoryPool<UnityEvent<T>, UnityAction<T>, EventSubscriber<T>>((eventArg, action, item) => item.OnSpawned(eventArg, action));

        private UnityEvent<T> unityEvent;
        private UnityAction<T> handler;

        private void OnSpawned(UnityEvent<T> eventArg, UnityAction<T> action)
        {
            handler = action;
            unityEvent = eventArg;
            unityEvent.AddListener(action);
        }

        public void Unsubscribe()
        {
            unityEvent.RemoveListener(handler);
        }

        public void OnDespawned()
        {
            unityEvent = default;
            handler = default;
            Pool.Despawn(this);
        }
    }
    
    public class EventSubscriber : IEventSubscriber
    {
        public static readonly StaticMemoryPool<UnityEvent, UnityAction, EventSubscriber> Pool = new StaticMemoryPool<UnityEvent, UnityAction, EventSubscriber>((eventArg, action, item) => item.OnSpawned(eventArg, action));

        private UnityEvent unityEvent;
        private UnityAction handler;

        private void OnSpawned(UnityEvent eventArg, UnityAction action)
        {
            handler = action;
            unityEvent = eventArg;
            unityEvent.AddListener(action);
        }

        public void Unsubscribe()
        {
            unityEvent.RemoveListener(handler);
        }

        public void OnDespawned()
        {
            unityEvent = default;
            handler = default;
            Pool.Despawn(this);
        }
    }
}