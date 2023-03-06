using System;
using System.Reflection;
using UnityEngine;

namespace QuickEye.Utility
{
    /// <summary>
    /// Class that derives from SingletonScriptableObject will:
    /// create its instance automatically when Instance property is used.
    /// If class also has `SingletonAsset` attribute, an asset has to be present at relevant path, unless a `SingletonAssetAttribute.Mandatory` is set to false.
    /// </summary>
    /// <typeparam name="T">Type of the singleton instance</typeparam>
    public abstract class SingletonScriptableObject<T> : SingletonScriptableObject
        where T : SingletonScriptableObject<T>
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
            // Try to load asset from `SingletonAssetAttribute` path
            if (TryLoadFromResources<T>(out var asset))
                return asset;
            // Try to create asset at `CreateAssetAutomaticallyAttribute` path
            if (TryCreateAsset<T>() && TryLoadFromResources(out asset))
                return asset;
            // Throw Exception if class has `SingletonAssetAttribute` and asset instance is mandatory 
            var att = typeof(T).GetCustomAttribute<SingletonAssetAttribute>();
            if (att?.Mandatory == true)
                throw new Exception($"Object of type: {typeof(T).FullName} requires singleton asset.");
            // Create and return singleton instance
            var obj = CreateInstance<T>();
            obj.name = typeof(T).Name;
            return obj;
        }
        
        /// <summary>
        /// If in Editor, try to create an asset at path specified in `CreateAssetAutomaticallyAttribute`
        /// </summary>
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
            throw new EditorAssetFactoryException(att.FullAssetPath);
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

    public class EditorAssetFactoryException : Exception
    {
        public EditorAssetFactoryException(string assetPath) : base(
            $"Editor failed to create singleton asset at:\n{assetPath}.")
        {
        }
    }

    internal delegate bool TryCreateAsset(ScriptableObject scriptableObject);
}