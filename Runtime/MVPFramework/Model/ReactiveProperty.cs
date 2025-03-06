using System;
using System.Collections.Generic;
using Zenject;

namespace MVPFramework.Model
{
    [NoReflectionBaking]
    public class ReactiveProperty<T> : IReactiveProperty
    {
        private static readonly IEqualityComparer<T> DefaultEqualityComparer = EqualityComparer<T>.Default;
        private readonly bool changeOnSameValue;
        public readonly ReactivePropertyEvent<T> OnChanged;
        public readonly ExtendedReactivePropertyEvent<T, T> OnBeforeValueChanged;

        private T value;

        public ReactiveProperty(T defaultValue, bool changeOnSameValue = false)
        {
            value = defaultValue;
            OnChanged = new ReactivePropertyEvent<T>();
            OnBeforeValueChanged = new ExtendedReactivePropertyEvent<T, T>();
            this.changeOnSameValue = changeOnSameValue;
        }

        public ReactiveProperty(bool changeOnSameValue = false) : this(default, changeOnSameValue)
        {
        }

        public ReactiveProperty<TOut> Select<TOut>(
            Func<T, TOut> selector, 
            Func<T, bool> predicate = null)
        {
            var derived = new ReactiveProperty<TOut>(selector(Value));

            OnChanged.AddListener(value =>
            {
                if (predicate == null || predicate(value))
                {
                    derived.Value = selector(value);
                }
            });

            return derived;
        }

        public ReactiveProperty<T> Where(Func<T, bool> predicate)
        {
            return Select(x => x, predicate);
        }

        public T Value
        {
            get => value;
            set
            {
                if (!changeOnSameValue && DefaultEqualityComparer.Equals(this.value, value))
                    return;

                OnBeforeValueChanged.Invoke(this.value, value);
                this.value = value;
                OnChanged.Invoke(this.value);
            }
        }

        public void SetSilently(T val)
        {
            value = val;
        }

        public void Reset()
        {
            SetSilently(default);
            ClearListeners();
        }

        public void Touch()
        {
            OnChanged.Invoke(value);
        }

        public void ClearListeners()
        {
            OnChanged.RemoveAllListeners();
            OnBeforeValueChanged.RemoveAllListeners();
        }
    }
}