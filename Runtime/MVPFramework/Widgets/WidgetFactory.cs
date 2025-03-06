using System;
using System.Collections.Generic;
using DITools;
using Zenject;

namespace MVPFramework.Widgets
{
    public class WidgetFactory : IWidgetFactory, IContainerConstructable
    {
        private readonly IDictionary<Type, Stack<IWidget>> pools = new Dictionary<Type, Stack<IWidget>>();
        private readonly DiContainer diContainer;

        public WidgetFactory(DiContainer diContainer)
        {
            this.diContainer = diContainer;
        }

        public T Create<T>() where T : IWidget
        {
            return diContainer.Instantiate<T>();
        }
        
        public TWidget Spawn<TWidget>() where TWidget : IPoolableWidget
        {
            var type = typeof(TWidget);

            var pool = GetPool(type);

            var widget = pool.Count > 0
                ? (TWidget) pool.Pop()
                : Create<TWidget>();

            widget.OnSpawned();
            
            return widget;
        }

        public void Despawn<TWidget>(TWidget widget) where TWidget : IPoolableWidget
        {
            var pool = GetPool(widget.GetType());
            widget.OnDespawned();
            pool.Push(widget);
        }

        private Stack<IWidget> GetPool(Type type)
        {
            if (pools.TryGetValue(type, out var pool)) return pool;
            
            pool = new Stack<IWidget>();
            pools.Add(type, pool);
            return pool;
        }
    }
}