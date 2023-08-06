using System.Reflection;
using UnityEngine;

namespace OneAsset
{
    // Execute Order allows us to execute Awake before all non-singletons
    // This allows other scripts to access initialized singletons in their awake methods.
    [DefaultExecutionOrder(-10000)]
    public abstract class SingletonMonoBehaviour : MonoBehaviour
    {
        protected static bool IsAppQuitting;

        protected virtual void OnApplicationQuit()
        {
            IsAppQuitting = true;
        }
    }

    public class SingletonMonoBehaviour<T> : SingletonMonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;

        /// <summary>
        /// <para>Returns a instance of T.</para>
        /// <para>If no instance of the T exists, it will create a new one.</para>
        /// <para>If multiple instances of T exist, the first one executing the <see cref="Awake"/> will be preserved while the rest will self destruct on their <see cref="Awake"/>.</para>
        /// <para>If the only instance of T will be destroyed, the new one will be created on the next access to this property.</para>
        /// <para>If T has a <see cref="LoadFromAssetAttribute"/> the instance will be loaded from a prefab asset.</para>
        /// </summary>
        /// <exception cref="AssetIsMissingException">Thrown when T has a <see cref="LoadFromAssetAttribute"/> but no asset was found at path provided</exception>
        public static T Instance => GetInstance();

        /// <summary>
        /// MonoBehaviour's Awake Message. When overriden, class descendants need to call the base implementation of it to keep singleton behavior. 
        /// </summary>
        protected virtual void Awake() => Initialize();

        private void Initialize()
        {
            if (_instance != this)
                if (_instance != null || !(this is T))
                {
                    Destroy(gameObject);
                    throw new SingletonAlreadyExistsException(this);
                }

            ForceDontDestroyOnLoad();
            _instance = (T)this;
        }

        /// <summary>
        /// MonoBehaviour's OnDestroy Message. When overriden, class descendants need to call the base implementation of it to keep singleton behavior.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (_instance != this)
                return;
            _instance = null;
        }

        private void ForceDontDestroyOnLoad()
        {
            // `Object.DontDestroyOnLoad` only works for root GameObjects
            // That's why we want to leave parents that aren't singletons.
            var singletonParents = GetComponentsInParent<SingletonMonoBehaviour>();
            if (singletonParents.Length <= 1)
                transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        private static T GetInstance()
        {
            // When app is quitting singleton objects will be destroyed
            // In this case we shouldn't try to create a new singleton
            if (IsAppQuitting)
                return null;
            if (_instance == null)
                _instance = CreateInstance();

            return _instance;
        }

        private static T CreateInstance()
        {
            if (TryInstantiatePrefab(out var i))
                return i;

            var obj = new GameObject { name = typeof(T).Name };
            return obj.AddComponent<T>();
        }

        private static bool TryInstantiatePrefab(out T obj)
        {
            var attr = LoadFromAssetUtils.GetAttribute(typeof(T));
            if (attr == null)
                return obj = null;
            
            var resourcesPath = attr.GetResourcesPath(typeof(T));
            var prefab = Resources.Load<T>(resourcesPath);
            if (prefab == null)
                throw new AssetIsMissingException(typeof(T), resourcesPath);
            obj = Instantiate(prefab);
            obj.name = typeof(T).Name;
            return true;
        }
    }
}