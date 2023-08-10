using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OneAsset
{
    using static AssetLoadOptionsUtility;
    /// <summary>
    /// <para>Load or create ScriptableObjects and Prefabs</para>
    /// </summary>
    public static partial class OneAssetLoader
    {
        internal static TryCreateAsset CreateAssetAction;

        #region LoadOrCreateScriptableObject

        /// <summary>
        /// <para>Load or create an instance of T with load options</para>
        /// </summary>
        /// <param name="scriptableObjectType"><see cref="UnityEngine.ScriptableObject"/> type of instance</param>
        /// <param name="options">Options defining the behaviour or load operation</param>
        /// <returns>New instance of T or asset instance</returns>
        /// <exception cref="AssetIsMissingException">Thrown when <see cref="AssetLoadOptions.AssetIsMandatory"/> is enabled and no asset was found at provided paths</exception>
        public static ScriptableObject LoadOrCreateScriptableObject(Type scriptableObjectType, AssetLoadOptions options)
        {
            if (options == null || options.Paths.Length == 0)
                return CreateSo(scriptableObjectType);

            // Try to load asset
            if (TryLoad(scriptableObjectType, options, out var asset))
                return asset as ScriptableObject;

            // Try to create asset at path
            if (options.CreateAssetIfMissing &&
                TryCreateAsset(scriptableObjectType, options) &&
                TryLoad(scriptableObjectType, options, out asset))
                return asset as ScriptableObject;

            // Throw if asset is mandatory
            if (options.AssetIsMandatory)
                throw new AssetIsMissingException(scriptableObjectType, options.Paths[0]);

            // Create and return a new instance
            return CreateSo(scriptableObjectType);
        }
        
        /// <summary>
        /// <para>Load or create an instance of T with load options</para>
        /// </summary>
        /// <typeparam name="T"><see cref="ScriptableObject"/> type</typeparam>
        /// <returns>New instance of T or asset instance</returns>
        /// <exception cref="AssetIsMissingException">Thrown when <see cref="AssetLoadOptions.AssetIsMandatory"/> is enabled and no asset was found at provided paths</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when <see cref="AssetLoadOptions.CreateAssetIfMissing"/> is enabled and there was an issue with <see cref="UnityEditor.AssetDatabase"/></exception>
        public static T LoadOrCreateScriptableObject<T>(AssetLoadOptions options) where T : ScriptableObject
        {
            return LoadOrCreateScriptableObject(typeof(T), options) as T;
        }
        
        /// <summary>
        /// <para>Load or create an instance of ScriptableObject</para>
        /// <para>The <see cref="AssetLoadOptions"/> will be created based on <see cref="LoadFromAssetAttribute"/> of ScriptableObject type</para>
        /// </summary>
        /// <returns>New instance of ScriptableObject or asset instance</returns>
        /// <exception cref="AssetIsMissingException">Thrown when <see cref="AssetLoadOptions.AssetIsMandatory"/> is enabled and no asset was found at provided paths</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when <see cref="AssetLoadOptions.CreateAssetIfMissing"/> is enabled and there was an issue with <see cref="UnityEditor.AssetDatabase"/></exception>
        public static ScriptableObject LoadOrCreateScriptableObject(Type scriptableObjectType)
        {
            return LoadOrCreateScriptableObject(scriptableObjectType, GetLoadOptions(scriptableObjectType));
        }

        /// <summary>
        /// <para>Load or create an instance of T</para>
        /// <para>The <see cref="AssetLoadOptions"/> will be created based on <see cref="LoadFromAssetAttribute"/> of T</para>
        /// </summary>
        /// <typeparam name="T"><see cref="ScriptableObject"/> type</typeparam>
        /// <returns>New instance of T or asset instance</returns>
        /// <exception cref="AssetIsMissingException">Thrown when <see cref="AssetLoadOptions.AssetIsMandatory"/> is enabled and no asset was found at provided paths</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when <see cref="AssetLoadOptions.CreateAssetIfMissing"/> is enabled and there was an issue with <see cref="UnityEditor.AssetDatabase"/></exception>
        public static T LoadOrCreateScriptableObject<T>() where T : ScriptableObject
        {
            return LoadOrCreateScriptableObject(typeof(T)) as T;
        }
        
        #endregion

        #region LoadOrCreateGameObject

        public static Component LoadOrCreateGameObject(Type componentType, AssetLoadOptions options)
        {
            if (options == null || options.Paths.Length == 0)
                return CreateGameObject(componentType);

            // Try to load prefab
            if (TryLoad(componentType, options, out var prefab))
            {
                var component = (Component)Object.Instantiate(prefab);
                component.name = componentType.Name;
                return component;
            }

            // Throw if asset is mandatory
            if (options.AssetIsMandatory)
                throw new AssetIsMissingException(componentType, options.Paths[0]);


            return CreateGameObject(componentType);
        }

        public static T LoadOrCreateGameObject<T>(AssetLoadOptions options) where T : Component
        {
            return LoadOrCreateGameObject(typeof(T), options) as T;
        }
        
        public static Component LoadOrCreateGameObject(Type componentType)
        {
            var options = GetLoadOptions(componentType);
            return LoadOrCreateGameObject(componentType, options);
        }

        public static T LoadOrCreateGameObject<T>() where T : Component
        {
            return LoadOrCreateGameObject(typeof(T), GetLoadOptions(typeof(T))) as T;
        }

        #endregion

        private static Component CreateGameObject(Type componentType)
        {
            var obj = new GameObject { name = componentType.Name };
            return obj.AddComponent(componentType);
        }

        private static ScriptableObject CreateSo(Type scriptableObjectType)
        {
            var obj = ScriptableObject.CreateInstance(scriptableObjectType);
            obj.name = scriptableObjectType.Name;
            return obj;
        }
        
        private static bool TryCreateAsset(Type type, AssetLoadOptions options)
        {
            if (!Application.isEditor || CreateAssetAction == null)
                return false;
            var obj = ScriptableObject.CreateInstance(type);
            try
            {
                CreateAssetAction(obj, options);
                return true;
            }
            catch (Exception e)
            {
                Object.DestroyImmediate(obj);
                throw new EditorAssetFactoryException(type, e);
            }
        }

        private static bool TryLoad(Type type, AssetLoadOptions options, out Object obj)
        {
            foreach (var path in options.AssetPaths)
            {
                if (TryLoadFromResources(type, path, options, out obj))
                    return true;

                if (TryLoadFromAssetDatabase(type, path, out obj))
                    return true;

                if (options.LoadAndForget && TryLoadAndForget(type, path, out obj))
                    return true;
            }

            obj = null;
            return false;
        }

        private static bool TryLoadFromResources(Type type, AssetPath path, AssetLoadOptions options,
            out Object obj)
        {
            if (path.IsInResourcesFolder)
            {
                obj = Resources.Load(path.ResourcesPath, type);
                if (obj != null)
                    return true;
            }

            obj = null;
            return false;
        }
    }
}