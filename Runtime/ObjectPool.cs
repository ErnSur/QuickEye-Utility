using System.Collections.Generic;
using UnityEngine;

namespace QuickEye.Utility
{
    public interface IObjectPool<T>
    {
        void ReturnToPool(T obj);
        void ReturnToPoolAll();
        T GetFromPool();
    }

    public class ObjectPool<T> : IObjectPool<T> where T : Component
    {
        private readonly List<T> _store = new List<T>();
        private readonly Transform _container;
        private readonly T _prefab;

        public int PooledObjectsCount
        {
            get { return _store.Count; }
        }

        public ObjectPool(Transform container, T prefab, uint poolStartSize)
        {
            _container = container;
            _prefab = prefab;

            PopulateStore(poolStartSize);
        }

        public void ReturnToPool(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_container);
        }

        public void ReturnToPoolAll()
        {
            for (int i = 0; i < _store.Count; i++)
            {
                ReturnToPool(_store[i]);
            }
        }

        public T GetFromPool()
        {
            for (int i = 0; i < _store.Count; i++)
            {
                if (!_store[i].gameObject.activeSelf)
                {
                    return _store[i];
                }
            }

            return CreateNewObject();
        }

        private void PopulateStore(float startSize)
        {
            for (int i = 0; i < startSize; i++)
            {
                CreateNewObject();
            }
        }

        private T CreateNewObject()
        {
            var newObject = Object.Instantiate(_prefab, _container);
            newObject.gameObject.SetActive(false);
            _store.Add(newObject);
            return newObject;
        }
    }
}