using System;
using Zenject;

namespace MVPFramework.Events
{
    public class ActionSubscriber : IEventSubscriber
    {
        public static readonly StaticMemoryPool<Action, ActionSubscriber> Pool = new StaticMemoryPool<Action, ActionSubscriber>((action, item) => item.OnSpawned(action));

        private Action unsubscribeAction;

        private void OnSpawned(Action action)
        {
            unsubscribeAction = action;
        }

        public void Unsubscribe()
        {
            unsubscribeAction?.Invoke();
            unsubscribeAction = default;
        }

        public void OnDespawned()
        {
            unsubscribeAction = default;
            Pool.Despawn(this);
        }
    }
}
