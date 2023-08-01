using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.Utility
{
    public static class ScriptableObjectFactory
    {
        internal static TryCreateAsset CreateAssetAction;

        /// <summary>
        /// <para>Creates a new instance of T while respecting the rules of following attributes:</para>
        /// <para>If T has a <see cref="SingletonAssetAttribute"/></para>
        /// <para>Try to load and return an asset from the <see cref="SingletonAssetAttribute"/> path. If no asset was found at the path, create and return a new instance of T.</para>
        /// <para>If T has a <see cref="SingletonAssetAttribute"/> and <see cref="CreateAssetAutomaticallyAttribute"/></para>
        /// <para>Try to load and return an asset from the <see cref="SingletonAssetAttribute"/> path. If no asset was found at the path and code is running in the editor, create a new asset at <see cref="CreateAssetAutomaticallyAttribute"/> path. Otherwise create and return a new instance of T.</para>
        /// </summary>
        /// <typeparam name="T"><see cref="ScriptableObject"/> type</typeparam>
        /// <returns>New instance of T, </returns>
        /// <exception cref="SingletonAssetIsMissingException">Thrown when T has a <see cref="SingletonAssetAttribute.Mandatory"/> flag set to true and no asset was found at path provided</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when T has a <see cref="CreateAssetAutomaticallyAttribute"/> and there was an issue with <see cref="UnityEditor.AssetDatabase"/> action</exception>
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
                throw new SingletonAssetIsMissingException(typeof(T), att.GetResourcesPath(typeof(T)));
            // Create and return a new instance
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
}