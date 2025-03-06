using UnityEngine.Events;
using Zenject;

namespace MVPFramework.Model
{
    [NoReflectionBaking]
    public class CompositeReactiveProperty<T1, T2>
    {
        private readonly ReactiveProperty<T1> _prop1;
        private readonly ReactiveProperty<T2> _prop2;

        public readonly UnityEvent<T1, T2> OnChanged = new();

        public CompositeReactiveProperty(
            ReactiveProperty<T1> prop1,
            ReactiveProperty<T2> prop2)
        {
            _prop1 = prop1;
            _prop2 = prop2;
            
            _prop1.OnChanged.AddListener(HandleProp1Changed);
            _prop2.OnChanged.AddListener(HandleProp2Changed);
        }

        private void HandleProp1Changed(T1 newVal) => 
            OnChanged.Invoke(newVal, _prop2.Value);

        private void HandleProp2Changed(T2 newVal) => 
            OnChanged.Invoke(_prop1.Value, newVal);
    }

    // Для 3 типов
    [NoReflectionBaking]
    public class CompositeReactiveProperty<T1, T2, T3>
    {
        private readonly ReactiveProperty<T1> _prop1;
        private readonly ReactiveProperty<T2> _prop2;
        private readonly ReactiveProperty<T3> _prop3;

        public readonly UnityEvent<T1, T2, T3> OnChanged = new UnityEvent<T1, T2, T3>();

        public CompositeReactiveProperty(
            ReactiveProperty<T1> prop1,
            ReactiveProperty<T2> prop2,
            ReactiveProperty<T3> prop3)
        {
            _prop1 = prop1;
            _prop2 = prop2;
            _prop3 = prop3;

            _prop1.OnChanged.AddListener(HandleProp1Changed);
            _prop2.OnChanged.AddListener(HandleProp2Changed);
            _prop3.OnChanged.AddListener(HandleProp3Changed);
        }

        private void HandleProp1Changed(T1 newVal) => 
            OnChanged.Invoke(newVal, _prop2.Value, _prop3.Value);
        
        private void HandleProp2Changed(T2 newVal) => 
            OnChanged.Invoke(_prop1.Value, newVal, _prop3.Value);
        
        private void HandleProp3Changed(T3 newVal) => 
            OnChanged.Invoke(_prop1.Value, _prop2.Value, newVal);
    }

    // Для 4 типов
    [NoReflectionBaking]
    public class CompositeReactiveProperty<T1, T2, T3, T4>
    {
        private readonly ReactiveProperty<T1> _prop1;
        private readonly ReactiveProperty<T2> _prop2;
        private readonly ReactiveProperty<T3> _prop3;
        private readonly ReactiveProperty<T4> _prop4;

        public readonly UnityEvent<T1, T2, T3, T4> OnChanged = new UnityEvent<T1, T2, T3, T4>();

        public CompositeReactiveProperty(
            ReactiveProperty<T1> prop1,
            ReactiveProperty<T2> prop2,
            ReactiveProperty<T3> prop3,
            ReactiveProperty<T4> prop4)
        {
            _prop1 = prop1;
            _prop2 = prop2;
            _prop3 = prop3;
            _prop4 = prop4;

            _prop1.OnChanged.AddListener(HandleProp1Changed);
            _prop2.OnChanged.AddListener(HandleProp2Changed);
            _prop3.OnChanged.AddListener(HandleProp3Changed);
            _prop4.OnChanged.AddListener(HandleProp4Changed);
        }

        private void HandleProp1Changed(T1 newVal) => 
            OnChanged.Invoke(newVal, _prop2.Value, _prop3.Value, _prop4.Value);
        
        private void HandleProp2Changed(T2 newVal) => 
            OnChanged.Invoke(_prop1.Value, newVal, _prop3.Value, _prop4.Value);
        
        private void HandleProp3Changed(T3 newVal) => 
            OnChanged.Invoke(_prop1.Value, _prop2.Value, newVal, _prop4.Value);
        
        private void HandleProp4Changed(T4 newVal) => 
            OnChanged.Invoke(_prop1.Value, _prop2.Value, _prop3.Value, newVal);
    }

    public static class CompositeReactivePropertyFactory
    {
        public static CompositeReactiveProperty<T1, T2> Create<T1, T2>(ReactiveProperty<T1> p1,
            ReactiveProperty<T2> p2) => new(p1, p2);

        public static CompositeReactiveProperty<T1, T2, T3> Create<T1, T2, T3>(ReactiveProperty<T1> p1,
            ReactiveProperty<T2> p2, ReactiveProperty<T3> p3) => new(p1, p2, p3);

        public static CompositeReactiveProperty<T1, T2, T3, T4> Create<T1, T2, T3, T4>(ReactiveProperty<T1> p1,
            ReactiveProperty<T2> p2, ReactiveProperty<T3> p3, ReactiveProperty<T4> p4) => new(p1, p2, p3, p4);
    }
}