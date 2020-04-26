using System.Collections.Generic;
using UnityEngine;

namespace QuickEye.Utility
{
    [System.Serializable]
    public class GameObjectPool<T> where T : Component
    {
        [SerializeField]
        private T _original;

        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private int _startSize;

        private readonly Stack<T> _available = new Stack<T>();
        private readonly HashSet<T> _rented = new HashSet<T>();

        public int CountAll => CountRented + CountAvailable;
        public int CountRented => _rented.Count;
        public int CountAvailable => _available.Count;

        public T Original => _original;
        public Transform Parent => _parent;

        // Empty Ctr is needed for proper serialization
        public GameObjectPool() { }
        public GameObjectPool(Transform parent, T original, int size)
        {
            _parent = parent;
            _original = original;
            _startSize = size;
            Initialize();
        }

        public void Initialize()
        {
            for (int i = 0; i < _startSize; i++)
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
                Debug.LogWarning($"Trying to return already released object");
                return;
            }

            _rented.Remove(obj);

            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_parent);

            _available.Push(obj);
        }

        public void ReturnAll()
        {
            foreach (var obj in _rented)
                Return(obj);
        }

        private T CreateObject()
        {
            var newObject = Object.Instantiate(_original, _parent);
            newObject.gameObject.SetActive(false);
            return newObject;
        }
    }
}