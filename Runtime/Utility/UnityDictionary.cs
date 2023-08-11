using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace QuickEye.Utility
{
    [Serializable]
    public class UnityDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<KvP> list = new List<KvP>();
        
        public UnityDictionary() { }
        public UnityDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        public UnityDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }
        
        #if UNITY_2021_1_OR_NEWER
        public UnityDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection) { }
        public UnityDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : base(collection, comparer) { }
        #endif
        public UnityDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public UnityDictionary(int capacity) : base(capacity) { }
        public UnityDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }
        protected UnityDictionary(SerializationInfo info, StreamingContext context) : base(info, context) { }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
#if UNITY_EDITOR
            var duplicates = list
                .Select((kvp, i) => (index: i, kvp))
                .Where(t => t.kvp.duplicatedKey).ToArray();
#endif
            var newPairs = this.Select(kvp => new KvP(kvp.Key, kvp.Value));
            list.Clear();
            list.AddRange(newPairs);
#if UNITY_EDITOR
            foreach (var (index, kvp) in duplicates)
                list.Insert(index, kvp);
#endif
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Clear();
            foreach (var kvp in list)
            {
                var key = kvp.key;
                var canAddKey = key != null && !ContainsKey(key);
                if (canAddKey)
                    Add(key, kvp.value);

                kvp.duplicatedKey = !canAddKey;
            }
#if !UNITY_EDITOR
            list.Clear();
#endif
        }

        [Serializable]
        internal class KvP
        {
            public TKey key;
            public TValue value;

            [SerializeField, HideInInspector]
            internal bool duplicatedKey;

            public KvP(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}