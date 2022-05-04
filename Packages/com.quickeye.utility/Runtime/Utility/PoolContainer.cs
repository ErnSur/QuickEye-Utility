using System;
using UnityEngine;

namespace QuickEye.Utility
{
    [Serializable]
    public class PoolContainer<T> : Container<T>, ISerializationCallbackReceiver
        where T : Component
    {
        [NonSerialized]
        public GameObjectPool<T> Pool;

        public PoolContainer()
        {
        }

        public PoolContainer(Transform transform, T itemPrefab) : base(transform, itemPrefab)
        {
            Pool = new GameObjectPool<T>(transform, itemPrefab, 0);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Pool = new GameObjectPool<T>(Transform, ItemPrefab, 0);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        protected override T GetNewItem()
        {
            var item = Pool.Rent();
            item.gameObject.SetActive(true);
            return item;
        }

        protected override void OnRemove(T item)
        {
            Pool.Return(item);
        }
    }
}