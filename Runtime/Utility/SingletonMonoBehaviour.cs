using System;
using System.Reflection;
using UnityEngine;

namespace QuickEye.Utility
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
            var attr = typeof(T).GetCustomAttribute<SingletonAssetAttribute>();
            if (attr != null)
            {
                var prefab = Resources.Load<T>(attr.ResourcesPath);
                if (prefab == null)
                    throw new SingletonAssetIsMissingException(attr.ResourcesPath, typeof(T));
                obj = Instantiate(prefab);
                obj.name = typeof(T).Name;
                return true;
            }

            return obj = null;
        }
    }

    public class SingletonAlreadyExistsException : Exception
    {
        internal SingletonAlreadyExistsException(SingletonMonoBehaviour obj) : base(
            $"Singleton of type {obj.GetType()} already exists. Destroying \"{GetGameObjectPath(obj.gameObject)}\"")
        {
        }

        private static string GetGameObjectPath(GameObject obj)
        {
            var path = $"/{obj.name}";
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = $"/{obj.name}{path}";
            }

            path = $"{obj.scene.name}{path}";
            return path;
        }
    }

    public class SingletonAssetIsMissingException : Exception
    {
        internal SingletonAssetIsMissingException(string assetPath, Type componentType) : base(
            $"Prefab at : {assetPath} has no {componentType.Name} component.")
        {
        }
    }
}