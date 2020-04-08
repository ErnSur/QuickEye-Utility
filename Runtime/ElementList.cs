using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickEye.CanvasElements
{
    [Serializable]
    public class ElementList<T> : IList<T> where T : Component
    {
        public RectTransform container;
        public T itemPrefab;
        public List<T> items = new List<T>();

        public T this[int i]
        {
            get => items[i];
            set => items[i] = value;
        }

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public T AddNew()
        {
            var e = UnityEngine.Object.Instantiate(itemPrefab, container);
            Add(e);
            return e;
        }

        public T Add(T item)
        {
            if (item.transform.parent != container)
                item.transform.SetParent(container);
            items.Add(item);
            return item;
        }

        public void Remove(T element)
        {
            if (!Contains(element))
                return;

            UnityEngine.Object.Destroy(element);
            items.Remove(element);
        }

        public void RemoveAt(int index)
        {
            Remove(this[index]);
        }

        #region Plain IList implementation
        public void Clear() => items.Clear();

        public bool Contains(T item) => items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => items.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => items.GetEnumerator();

        public int IndexOf(T item) => items.IndexOf(item);

        public void Insert(int index, T item) => items.Insert(index, item);

        void ICollection<T>.Add(T item) => Add(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        bool ICollection<T>.Remove(T item)
        {
            if (Contains(item))
            {
                Remove(item);
                return true;
            }
            return false;
        }
        #endregion
    }
}
