using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;

namespace MVPFramework.Model
{
    public class ReactiveList<T> : IEnumerable<T>
    {
        private readonly List<T> list;
        public readonly ReactivePropertyEvent<T> OnItemAdded;
        public readonly ReactivePropertyEvent<T> OnItemRemoved;

        public ReactiveList()
        {
            list = new List<T>();
            OnItemAdded = new ReactivePropertyEvent<T>();
            OnItemRemoved = new ReactivePropertyEvent<T>();
        }

        public T this[int i]
        {
            get => list[i];
            set => list[i] = value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public void Add(T value, bool silently = false)
        {
            list.Add(value);

            if (!silently)
                OnItemAdded.Invoke(value);
        }

        public void Remove(Predicate<T> match, bool silently = false)
        {
            var find = list.Find(match);
            if (find != null) Remove(find, silently);
        }

        public void Remove(T value, bool silently = false)
        {
            list.Remove(value);

            if (!silently)
                OnItemRemoved.Invoke(value);
        }

        public void Clear()
        {
            list.Clear();
        }

        public void RemoveAll()
        {
            var temp = ListPool<T>.Instance.Spawn();
            temp.AddRange(list);

            foreach (var item in temp) 
                Remove(item);

            ListPool<T>.Instance.Despawn(temp);
        }

        public void Sort(IComparer<T> comparer)
        {
            list.Sort(comparer);
        }

        public void Sort(Comparison<T> comparison)
        {
            list.Sort(comparison);
        }

        public int Count => list.Count;

        public int FindIndex(Predicate<T> match)
        {
            return list.FindIndex(match);
        }

        public T Find(Predicate<T> match)
        {
            return list.Find(match);
        }
        
        public void FindAll(Predicate<T> match, List<T> buffer)
        {
            buffer.Clear();
            foreach (var item in list)
            {
                if (match(item)) 
                    buffer.Add(item);
            }
        }
    }
}