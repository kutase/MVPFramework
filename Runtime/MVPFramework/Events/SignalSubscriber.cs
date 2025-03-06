using System;
using Signals;
using Zenject;

namespace MVPFramework.Events
{
    public class SignalSubscriber<T> : IEventSubscriber
    {
        public static readonly StaticMemoryPool<Action<T>, SignalSubscriber<T>> Pool = new StaticMemoryPool<Action<T>, SignalSubscriber<T>>((action, item) => item.OnSpawned(action));

        private Action<T> handler;

        private void OnSpawned(Action<T> action)
        {
            handler = action;
            SignalsHub.AddListener(action);
        }

        public void Unsubscribe()
        {
            SignalsHub.RemoveListener(handler);
        }

        public void OnDespawned()
        {
            handler = default;
            Pool.Despawn(this);
        }
    }
}