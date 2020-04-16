using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickEye.CanvasElements
{
    [Serializable]
    public class Container<T> : IGameObjectCollection<T>, IList<T>, IReadOnlyList<T> where T : Component
    {
        public Transform transform;
        public T itemPrefab;
        private List<T> _items = new List<T>();

        public T this [int i]
        {
            get => _items[i];
            set => _items[i] = value;
        }

        public int Count => _items.Count;

        public bool IsReadOnly => false;

        public T AddNew()
        {
            var e = UnityEngine.Object.Instantiate(itemPrefab, transform);
            Add(e);
            return e;
        }

        public void Add(T item)
        {
            if (item.transform.parent != transform)
                item.transform.SetParent(transform);
            _items.Add(item);
        }

        public bool Remove(T element)
        {
            if (_items.Remove(element))
            {
                UnityEngine.Object.Destroy(element);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            Remove(_items[index]);
        }

        public void Clear() => _items.Clear();

        public bool Contains(T item) => _items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public int IndexOf(T item) => _items.IndexOf(item);

        public void Insert(int index, T item)
        {
            if (item.transform.parent != transform)
                item.transform.SetParent(transform);
            _items.Insert(index, item);
        }

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}