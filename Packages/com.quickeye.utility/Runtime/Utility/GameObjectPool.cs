using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.Utility
{
    [Serializable]
    public class GameObjectPool<T> where T : Component
    {
        [SerializeField]
        private T original;

        [SerializeField]
        private Transform parent;

        [SerializeField]
        private int startSize;

        private readonly Stack<T> _available = new Stack<T>();
        private readonly HashSet<T> _rented = new HashSet<T>();

        protected GameObjectPool()
        {
        }

        public GameObjectPool(Transform parent, T original, int size)
        {
            this.parent = parent;
            this.original = original;
            startSize = size;
            Initialize();
        }

        public int CountAll => CountRented + CountAvailable;
        public int CountRented => _rented.Count;
        public int CountAvailable => _available.Count;

        public T Original => original;
        public Transform Parent => parent;

        public void Initialize()
        {
            for (var i = 0; i < startSize; i++)
                _available.Push(CreateObject());
        }

        public virtual T Rent()
        {
            var obj = _available.Count > 0 ? _available.Pop() : CreateObject();
            _rented.Add(obj);
            return obj;
        }

        public void Return(T obj)
        {
            if (_available.Contains(obj))
            {
                Debug.LogWarning("Trying to return already released object");
                return;
            }

            _rented.Remove(obj);

            obj.gameObject.SetActive(false);
            obj.transform.SetParent(parent);

            _available.Push(obj);
        }

        public void ReturnAll()
        {
            foreach (var obj in _rented)
                Return(obj);
        }

        private T CreateObject()
        {
            var newObject = Object.Instantiate(original, parent);
            newObject.gameObject.SetActive(false);
            return newObject;
        }
    }
}