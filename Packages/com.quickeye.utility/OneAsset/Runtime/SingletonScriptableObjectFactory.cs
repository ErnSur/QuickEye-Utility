using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.Utility
{
    public static class SingletonScriptableObjectFactory
    {
        internal static TryCreateAsset CreateAssetAction;

        public static T LoadOrCreateInstance<T>() where T : ScriptableObject
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
                throw new Exception($"Singleton of type: {typeof(T).FullName} was requested, but it requires an asset that is not in the project.");
            // Create and return singleton instance
            var obj = ScriptableObject.CreateInstance<T>();
            obj.name = typeof(T).Name;
            return obj;
        }

        /// <summary>
        /// If in Editor, try to create an asset at path specified in `CreateAssetAutomaticallyAttribute`
        /// </summary>
        private static bool TryCreateAsset<T>() where T : ScriptableObject
        {
            if (!Application.isEditor || CreateAssetAction == null)
                return false;
            var att = typeof(T).GetCustomAttribute<CreateAssetAutomaticallyAttribute>();
            if (att == null)
                return false;
            var obj = ScriptableObject.CreateInstance<T>();
            try
            {
                CreateAssetAction(obj);
                return true;
            }
            catch (Exception e)
            {
                Object.DestroyImmediate(obj);
                throw new EditorAssetFactoryException(typeof(T), e);
            }
        }

        private static bool TryLoadFromResources<T>(out T obj) where T : ScriptableObject
        {
            var attr = typeof(T).GetCustomAttribute<SingletonAssetAttribute>();
            if (attr == null)
                return obj = null;
            var path = attr.GetResourcesPath(typeof(T));

            obj = Resources.Load<T>(path);
            return obj != null;
        }
    }

    public class EditorAssetFactoryException : Exception
    {
        public EditorAssetFactoryException(Type type, Exception innerException) : base(
            $"Editor failed to create singleton asset of type:\n{type.FullName}.", innerException) { }
    }

    internal delegate void TryCreateAsset(ScriptableObject scriptableObject);
}