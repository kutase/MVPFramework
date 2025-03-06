using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Zenject;

namespace MVPFramework.Model
{
    [NoReflectionBaking]
    public class ConditionalCompositeReactiveProperty
    {
        public enum CombinationMode
        {
            Any, // OR
            All  // AND
        }

        // Список свойств и их предикатов для конвертации в bool
        private readonly List<PropertyData> _properties = new();
        private CombinationMode _mode = CombinationMode.Any;
        private bool cachedValue;

        // Событие изменения результирующего значения
        public readonly ReactivePropertyEvent<bool> OnResultChanged = new();

        // Режим комбинирования (Any = OR, All = AND)
        public CombinationMode Mode
        {
            get => _mode;
            set
            {
                if (_mode == value) return;
                _mode = value;
                UpdateResult(forceNotify: true);
            }
        }

        // Текущий результат комбинации
        public bool Value => cachedValue;

        private readonly bool emptyValue;

        public ConditionalCompositeReactiveProperty(bool emptyValue = false)
        {
            this.emptyValue = emptyValue;
        }

        // Добавляет свойство с предикатом (по умолчанию для bool)
        public void Add<T>(ReactiveProperty<T> property, Func<T, bool> predicate)// = null)
        {
            if (property == null || _properties.Any(p => p.Source == property)) 
                return;

            // Для bool используем значение напрямую, если предикат не задан
            // if (typeof(T) == typeof(bool) && predicate == null)
            // {
            //     predicate = value => (bool)(object)value;
            // }
            // else if (typeof(T) == typeof(bool) && predicate != null)
            // {
            //     throw new ArgumentException("");
            // }

            var data = new PropertyData
            {
                Source = property,
                Predicate = value => predicate.Invoke((T)value),
                LastValue = property.Value,
            };

            UnityAction<T> listener = newValue => UpdateResult(triggeredBy: data, newValue: newValue);

            // Подписка на изменения исходного свойства
            property.OnChanged.AddListener(listener);

            data.UnsubscribeAction = () => Unsubscribe(property, listener);

            _properties.Add(data);
            UpdateResult(forceNotify: true);
        }

        private void Unsubscribe<T>(ReactiveProperty<T> property, UnityAction<T> listener)
        {
            property.OnChanged.RemoveListener(listener);
        }

        // Удаляет свойство
        public void Remove<T>(ReactiveProperty<T> property)
        {
            var data = _properties.FirstOrDefault(p => p.Source == property);
            if (data.Source == null) return;

            data.UnsubscribeAction.Invoke();

            _properties.Remove(data);
            UpdateResult(forceNotify: true);
        }

        public void ClearAll()
        {
            foreach (var propertyData in _properties)
            {
                propertyData.UnsubscribeAction.Invoke();
            }

            _properties.Clear();

            UpdateResult(forceNotify: true);
        }

        // Обновляет результат и триггерит событие при необходимости
        private void UpdateResult(object triggeredBy = null, object newValue = null, 
                                bool forceNotify = false)
        {
            var previousResult = cachedValue;
            
            // Обновляем значение триггерного свойства
            if (triggeredBy != null && newValue != null)
            {
                var data = (PropertyData)triggeredBy;
                data.LastValue = newValue;
            }

            if (_properties.Count > 0)
            {
                // Вычисляем новый результат
                cachedValue = _mode switch
                {
                    CombinationMode.Any => _properties.Any(p => p.Predicate(p.LastValue)),
                    CombinationMode.All => _properties.All(p => p.Predicate(p.LastValue)),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            else
            {
                cachedValue = emptyValue;
            }

            if (forceNotify || cachedValue != previousResult)
                OnResultChanged.Invoke(cachedValue);
        }

        private class PropertyData
        {
            public object Source { get; set; }
            public object LastValue { get; set; }
            public Func<object, bool> Predicate { get; set; }
            public Action UnsubscribeAction { get; set; }
        }
    }
}