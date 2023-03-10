using System;
using UnityEngine;

namespace QuickEye.Utility
{
    /// <summary>
    /// AddNew - get object from pool
    /// Remove - Returns object to pool
    /// Clear - returns all objects to pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PoolContainer<T> : Container<T>, ISerializationCallbackReceiver
        where T : Component
    {
        public GameObjectPool<T> Pool { get; private set; }

        protected PoolContainer()
        {
        }

        public PoolContainer(Transform root, T itemPrefab) : base(root, itemPrefab)
        {
            Pool = new GameObjectPool<T>(root, itemPrefab, 0);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Pool = new GameObjectPool<T>(Root, ItemPrefab, 0);
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