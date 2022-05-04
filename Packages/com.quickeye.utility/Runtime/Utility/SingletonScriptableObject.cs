using System;
using System.Reflection;
using UnityEngine;

namespace QuickEye.Utility
{
    /// <summary>
    /// class that derives from SingletonScriptableObject<T>:
    /// will create its instance automatically when Instance property is used
    /// if class also has `SingletonAsset` attribute an asset has to be present at relevant path, unless a `SingletonAssetAttribute.Mandatory` is set to false.
    /// </summary>
    public abstract class SingletonScriptableObject<T> : SingletonScriptableObject where T : SingletonScriptableObject<T>
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
    
    public abstract class SingletonScriptableObject : ScriptableObject
    {
        internal static TryCreateAsset TryCreateAssetAction;

        protected static T GetOrCreateInstance<T>() where T : ScriptableObject
        {
            if (TryLoadFromResources<T>(out var asset))
                return asset;
            if (TryCreateAsset<T>() && TryLoadFromResources(out asset))
                return asset;
            var att = typeof(T).GetCustomAttribute<SingletonAssetAttribute>();
            if (att?.Mandatory == true)
                throw new Exception($"Object of type: {typeof(T).FullName} requires singleton asset.");
            var obj = CreateInstance<T>();
            obj.name = typeof(T).Name;
            return obj;

        }

        private static bool TryCreateAsset<T>() where T : ScriptableObject
        {
            if (!Application.isEditor || TryCreateAssetAction == null)
                return false;
            var att = typeof(T).GetCustomAttribute<CreateAssetAutomaticallyAttribute>();
            if (att == null)
                return false;
            var obj = CreateInstance<T>();
            obj.name = typeof(T).Name;
            if (TryCreateAssetAction(obj))
                return true;
            throw new Exception($"Failed to create singleton asset at:\n{att.FullAssetPath}.");
        }

        private static bool TryLoadFromResources<T>(out T obj) where T : ScriptableObject
        {
            var attr = typeof(T).GetCustomAttribute<SingletonAssetAttribute>();
            if (attr == null)
                return obj = null;
            obj = Resources.Load<T>(attr.ResourcesPath);
            return obj != null;
        }
    }

    internal delegate bool TryCreateAsset(ScriptableObject scriptableObject);
}