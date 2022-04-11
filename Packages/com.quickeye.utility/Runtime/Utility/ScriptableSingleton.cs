using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace QuickEye.Utility
{
    // Rules:
    // class that derives from ScriptableSingleton<T>:
    // will create its instance automatically when referenced by Instance property
    // if class also has `SingletonAsset` attribute behaviour changes a little bit
    // class expects to have a asset at specific path if `SingletonAttribute.AssetIsMandatory` is set to true.
    // thus will throw an exception if `T.Instance` is accessed and asset is missing.
    // `SingletonAttribute.AutoCreateAsset`
    
    // Maybe create another attribute just for SOs
    // with assetNotMandatory flag
    public abstract class ScriptableSingleton : ScriptableObject
    {
        internal static event Action<ScriptableObject> SingletonInstantiated;

        protected static T GetOrCreateInstance<T>() where T : ScriptableObject
        {
            if (TryLoadFromResources<T>(out var asset))
                return asset;

            var obj = CreateInstance<T>();
            obj.name = typeof(T).Name;
            SingletonInstantiated?.Invoke(obj);
            return obj;
        }

        private static bool TryLoadFromResources<T>(out T obj) where T : ScriptableObject
        {
            var attr = typeof(T).GetCustomAttribute<SingletonAssetAttribute>();
            if (attr == null)
                return obj = null;
            obj = Resources.Load<T>(attr.ResourcesPath);
            if (!IsInsideResourcesFolder(attr.ResourcesPath))
                throw new SingletonAssetPathIsOutsideResources(typeof(T));
            return obj != null;
        }

        private static bool IsInsideResourcesFolder(string path)
        {
            var dirName = Path.GetDirectoryName(path);
            if (dirName == null)
                return false;
            var folders = dirName.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return folders.Contains("Resources");
        }
    }

    public abstract class ScriptableSingleton<T> : ScriptableSingleton where T : ScriptableSingleton<T>
    {
        private static T _instance;
        public static T Instance => GetInstance();

        private static T GetInstance()
        {
            if (_instance == null)
                _instance = GetOrCreateInstance<T>();
            return _instance;
        }
    }
}