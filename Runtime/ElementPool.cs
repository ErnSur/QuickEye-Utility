using QuickEye.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickEye.CanvasElements
{
    [Serializable]
    public class ElementPool<TItem> : IEnumerable<TItem>, ISerializationCallbackReceiver
        where TItem : Component
    {
        public RectTransform container;
        public TItem itemPrefab;
        public List<TItem> items = new List<TItem>();
        public ObjectPool<TItem> pool;

        public TItem this[int i]
        {
            get => items[i];
            set => items[i] = value;
        }

        public TItem AddNew()
        {
            var e = pool.GetFromPool();
            e.gameObject.SetActive(true);
            items.Add(e);

            return e;
        }

        public void Remove(TItem item)
        {
            items.Remove(item);

            pool.ReturnToPool(item);
        }

        public void Clear() => pool.ReturnToPoolAll();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            pool = new ObjectPool<TItem>(container, itemPrefab, 0);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        public IEnumerator<TItem> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
    }
}
