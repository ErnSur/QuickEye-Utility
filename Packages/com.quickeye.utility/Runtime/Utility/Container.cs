using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.Utility
{
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.DrawWithUnity]
#endif
    [Serializable]
    public class Container<T> : IList<T>, IReadOnlyList<T> where T : Component
    {
        [SerializeField]
        private T itemPrefab;

        [SerializeField]
        protected Transform root;

        [SerializeField]
        protected List<T> items = new List<T>();

        protected Container()
        {
        }

        public Container(Transform root, T itemPrefab)
        {
            this.root = root;
            this.itemPrefab = itemPrefab;
        }

        public T ItemPrefab => itemPrefab;

        public virtual Transform Root
        {
            get => root;
            set
            {
                if (root == value)
                    return;

                root = value;
                items.ForEach(i => i.transform.SetParent(root));
            }
        }

        public int Count => items.Count;

        public bool IsReadOnly => false;

        public T this[int i]
        {
            get => items[i];
            set => items[i] = value;
        }

        public void Add(T item)
        {
            if (item == null)
                return;
            if (item.transform.parent != root)
                item.transform.SetParent(root);
            items.Add(item);
        }

        public void Insert(int index, T item)
        {
            if (item == null)
                return;
            if (item.transform.parent != root)
                item.transform.SetParent(root);
            items.Insert(index, item);
        }

        public bool Remove(T item)
        {
            if (items.Remove(item))
            {
                OnRemove(item);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            Remove(items[index]);
        }

        public void Clear()
        {
            for (var i = items.Count - 1; i >= 0; i--)
                Remove(items[i]);
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T AddNew()
        {
            var item = GetNewItem();
            Add(item);
            return item;
        }

        protected virtual T GetNewItem()
        {
            return Object.Instantiate(itemPrefab, root);
        }

        protected virtual void OnRemove(T item)
        {
            Object.Destroy(item.gameObject);
        }
    }
}