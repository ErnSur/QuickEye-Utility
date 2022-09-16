using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuickEye.Utility
{
    [Serializable]
    public class UnityDic<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<KvP> list = new List<KvP>();

        public Dictionary<TKey, TValue> Storage { get; private set; } = new Dictionary<TKey, TValue>();
        private IDictionary<TKey, TValue> Dic => Storage;


        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            var duplicates = list.Where(kvp => kvp.duplicatedKey).ToArray();
            list.Clear();
            list.AddRange(Dic.Select(kvp => new KvP(kvp.Key, kvp.Value)));
            list.AddRange(duplicates);
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Dic.Clear();
            foreach (var kvp in list)
            {
                var key = kvp.Key;
                if (key != null && !ContainsKey(key))
                {
                    Dic.Add(key, kvp.Value);
                    kvp.duplicatedKey = false;
                }
                else
                {
                    kvp.duplicatedKey = true;
                }
            }
        }

        public TValue this[TKey key]
        {
            get => Dic[key];
            set => Dic[key] = value;
        }

        public ICollection<TKey> Keys => Dic.Keys;
        public ICollection<TValue> Values => Dic.Values;

        public void Add(TKey key, TValue value) => Dic.Add(key, value);

        public bool ContainsKey(TKey key) => Dic.ContainsKey(key);

        public bool Remove(TKey key) => Dic.Remove(key);

        public bool TryGetValue(TKey key, out TValue value) => Dic.TryGetValue(key, out value);

        // ICollection
        public int Count => Dic.Count;
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> pair)
        {
            Dic.Add(pair);
        }

        public void Clear()
        {
            Dic.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> pair)
        {
            return Dic.Contains(pair);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Dic.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> pair)
        {
            return Dic.Remove(pair);
        }

        // IEnumerable
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() =>
            Dic.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Dic.GetEnumerator();
        
        [Serializable]
        internal class KvP
        {
            public TKey Key;
            public TValue Value;
#if UNITY_EDITOR
            [SerializeField, HideInInspector]
            internal bool duplicatedKey;
#endif
            public KvP(TKey key, TValue value)
            {
                Key = key;
                Value = value;
                duplicatedKey = false;
            }

            public void Deconstruct(out TKey key, out TValue value)
            {
                key = Key;
                value = Value;
            }
        }
    }
}