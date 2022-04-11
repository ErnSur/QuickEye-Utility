using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.AI;

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
        /// <summary>
        /// For `Singleton<T>` singletons a prefab path relative to resources folder.
        /// For `ScriptableSingleton<T>` a SO path relative to Assets folder.
        /// </summary>
        public string ResourcesPath { get; }

        public SingletonAssetAttribute(string resourcesPath)
        {
            ResourcesPath = resourcesPath;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ScriptableSingletonAssetAttribute : Attribute
    {
        /// <summary>
        /// Project metadata about Singleton Asset.
        /// </summary>
        /// <param name="path">Path at which singleton asset should be found. Relative to the Resources folder if `AutoCreateAsset` is set to `false` otherwise relative to the Assets folder.</param>
        /// <param name="assetIsMandatory">Should editor throw an exception if singleton asset in not present in the project.</param>
        /// <param name="autoCreateAsset">Automatically create asset in editor if it does not exists.</param>
        public ScriptableSingletonAssetAttribute(string path, bool assetIsMandatory = true, bool autoCreateAsset = false)
        {
            if (autoCreateAsset)
            {
                AssetsPath = path;
                ResourcesPath = GetResPath(path);
            }
            else
            {
                ResourcesPath = path;
            }

            AutoCreateAsset = autoCreateAsset;
            AssetIsMandatory = assetIsMandatory;
        }

        public string AssetsPath { get; }

        public string ResourcesPath { get; }

        // Requires Full assetPath
        public bool AutoCreateAsset { get; }

        //does not require full asset path
        public bool AssetIsMandatory { get; }

        private static string GetResPath(string path)
        {
            path = Path.GetFullPath(path);

            var resourcesFolder = $"{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}";
            var index = path.IndexOf(resourcesFolder, StringComparison.InvariantCulture);
            if (index == -1)
                throw new ArgumentException("path has to contain Resources folder");
            return path.Substring(index + resourcesFolder.Length);
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

    public class SingletonAssetPathIsOutsideResources : Exception
    {
        internal SingletonAssetPathIsOutsideResources(Type type) : base(
            $"Type {type.FullName} has singleton path defined but does not include Resources folder.")
        {
        }
    }
}