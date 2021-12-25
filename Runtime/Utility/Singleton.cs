using System;
using System.Reflection;
using UnityEngine;

namespace QuickEye.Utility
{
    public abstract class Singleton : MonoBehaviour
    {
        protected static bool IsAppQuitting;
    }

    public class Singleton<T> : Singleton where T : Singleton<T>
    {
        private static T instance;
        public static T Instance => GetInstance();

        protected virtual void Awake()
        {
            if (instance != null || !(this is T))
            {
                Destroy(gameObject);
                throw new SingletonAlreadyExistsException(this);
            }

            ForceDontDestroyOnLoad();
            instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            if (instance != this)
                return;
            instance = null;
            IsAppQuitting = true; // App exit is the only case when singletons should be destroyed.
        }

        private void ForceDontDestroyOnLoad()
        {
            // `Object.DontDestroyOnLoad` only works for root GameObjects
            // That's why we want to leave parents that aren't singletons.
            var singletonParents = GetComponentsInParent<Singleton>();
            if (singletonParents.Length <= 1)
                transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        private static T GetInstance()
        {
            // When app is quitting singleton objects will be destroyed
            // In this case we shouldn't try to create a new singleton
            // because this can create a loop of objects being created and destroyed
            if (IsAppQuitting)
                return null;
            if (instance == null)
                instance = CreateInstance();
            return instance;
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

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SingletonAssetAttribute : Attribute
    {
        public string ResourcesPath { get; }

        public SingletonAssetAttribute(string resourcesPath)
        {
            ResourcesPath = resourcesPath;
        }
    }

    public class SingletonAlreadyExistsException : Exception
    {
        internal SingletonAlreadyExistsException(Singleton obj) : base(
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