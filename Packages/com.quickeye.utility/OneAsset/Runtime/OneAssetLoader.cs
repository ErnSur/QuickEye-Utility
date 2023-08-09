using System;
using System.Linq;
using System.Reflection;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OneAsset
{
    /// <summary>
    /// <para>Load or create ScriptableObjects while respecting the rules of following attributes:</para>
    /// <para><see cref="LoadFromAssetAttribute"/></para>
    /// <para><see cref="CreateAssetAutomaticallyAttribute"/></para>
    /// </summary>
    public static class OneAssetLoader
    {
        internal static TryCreateAsset CreateAssetAction;

        /// <summary>
        /// <para>Load or create an instance of T while respecting the rules of following attributes:</para>
        /// <para>If T has a <see cref="LoadFromAssetAttribute"/></para>
        /// <para>Try to load and return an asset from the <see cref="LoadFromAssetAttribute"/> path. If no asset was found at the path, create and return a new instance of T.</para>
        /// <para>If T has a <see cref="LoadFromAssetAttribute"/> and <see cref="CreateAssetAutomaticallyAttribute"/></para>
        /// <para>Try to load and return an asset from the <see cref="LoadFromAssetAttribute"/> path. If no asset was found at the path and code is running in the editor, create a new asset at <see cref="CreateAssetAutomaticallyAttribute"/> path. Otherwise create and return a new instance of T.</para>
        /// </summary>
        /// <typeparam name="T"><see cref="ScriptableObject"/> type</typeparam>
        /// <returns>New instance of T</returns>
        /// <exception cref="AssetIsMissingException">Thrown when T has a <see cref="LoadFromAssetAttribute.Mandatory"/> flag set to true and no asset was found at path provided</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when T has a <see cref="CreateAssetAutomaticallyAttribute"/> and there was an issue with <see cref="UnityEditor.AssetDatabase"/> action</exception>
        public static T LoadOrCreateInstance<T>() where T : ScriptableObject
        {
            return LoadOrCreateInstance(typeof(T)) as T;
        }

        /// <summary>
        /// <para>Load or create an instance of given type while respecting the rules of following attributes:</para>
        /// <para>If given type has a <see cref="LoadFromAssetAttribute"/></para>
        /// <para>Try to load and return an asset from the <see cref="LoadFromAssetAttribute"/> path. If no asset was found at the path, create and return a new instance of given type.</para>
        /// <para>If given type has a <see cref="LoadFromAssetAttribute"/> and <see cref="CreateAssetAutomaticallyAttribute"/></para>
        /// <para>Try to load and return an asset from the <see cref="LoadFromAssetAttribute"/> path. If no asset was found at the path and code is running in the editor, create a new asset at <see cref="CreateAssetAutomaticallyAttribute"/> path. Otherwise create and return a new instance of given type.</para>
        /// </summary>
        /// <returns>New ScriptableObject instance of given type</returns>
        /// <exception cref="AssetIsMissingException">Thrown when given type has a <see cref="LoadFromAssetAttribute.Mandatory"/> flag set to true and no asset was found at path provided</exception>
        /// <exception cref="EditorAssetFactoryException">Thrown only in editor, when given type has a <see cref="CreateAssetAutomaticallyAttribute"/> and there was an issue with <see cref="UnityEditor.AssetDatabase"/> action</exception>
        public static ScriptableObject LoadOrCreateInstance(Type scriptableObjectType)
        {
            var loadAttributes = LoadFromAssetUtils.GetAttributesInOrder(scriptableObjectType);
            var crateAssetAttribute = scriptableObjectType.GetCustomAttribute<CreateAssetAutomaticallyAttribute>();

            return LoadOrCreateInstance(scriptableObjectType, loadAttributes, crateAssetAttribute);
        }

        internal static ScriptableObject LoadOrCreateInstance(Type scriptableObjectType,
            LoadFromAssetAttribute[] loadFromAssetAttributes,
            CreateAssetAutomaticallyAttribute createAssetAutomaticallyAttribute)
        {
            if (loadFromAssetAttributes.Length == 0)
                return CreateInstance(scriptableObjectType);
            // Try to load asset from `LoadFromAssetAttribute` path
            if (TryLoadFromResources(scriptableObjectType, loadFromAssetAttributes, out var asset))
                return asset;
            // Try to create asset at `CreateAssetAutomaticallyAttribute` path
            if (TryCreateAsset(scriptableObjectType, createAssetAutomaticallyAttribute) &&
                TryLoadFromResources(scriptableObjectType, loadFromAssetAttributes, out asset))
                return asset;

            // if we came to this point and asset file exists on disk then that mean we are running before AssetDatabase initialized
            // if LoadFromAssetAttribute and CreateAssetAutomatically are preset
            // and we came to this point, it means that AssetDatabase failed to create an asset
            // OOORRR we are in buit game and game was build without the settings asset (what are the scenarios in which this is possible?)
            var highestPriorityAttribute = loadFromAssetAttributes[0];
            if (TryLoadUnsafe(scriptableObjectType, highestPriorityAttribute, out asset))
                return asset;

            // Throw Exception if class has `LoadFromAssetAttribute` and asset instance is mandatory 
            if (highestPriorityAttribute.Mandatory)
                throw new AssetIsMissingException(scriptableObjectType, highestPriorityAttribute.Path);
            // Create and return a new instance
            return CreateInstance(scriptableObjectType);
        }

        // TODO: Design an unsafe loading feature so that:
        // it doesn't require the `CreateAssetAutomatically` attribute
        // maybe LoadAssetFrom takes a absolute path?
        // because the absolute path is required for Unsafe load
        private static bool TryLoadUnsafe(Type scriptableObjectType, LoadFromAssetAttribute highestPriorityAttribute,
            out ScriptableObject instance)
        {
            if (!Application.isEditor
                || !highestPriorityAttribute.UnsafeLoad
                || !TryGetAbsoluteAssetPath(scriptableObjectType, out var absolutePath))
            {
                instance = null;
                return false;
            }
            // If someone would depend on unsafe load it would be annoying
            //Debug.LogWarning($"Loading {scriptableObjectType} outside AssetDatabase!");

#if UNITY_EDITOR
            // Ideally this code would be in editor assembly. But when this method is called from InitializeOnLoad
            // there is no guarantee that editor callback will be registered like with `CreateAssetAction`
            var i = InternalEditorUtility
                .LoadSerializedFileAndForget(absolutePath)
                .FirstOrDefault(o => o.GetType() == scriptableObjectType);

            instance = i as ScriptableObject;
            return true;
#endif
        }

        private static ScriptableObject CreateInstance(Type scriptableObjectType)
        {
            var obj = ScriptableObject.CreateInstance(scriptableObjectType);
            obj.name = scriptableObjectType.Name;
            return obj;
        }

        /// <summary>
        /// If in Editor, try to create an asset at path specified in <see cref="CreateAssetAutomaticallyAttribute"/>
        /// </summary>
        private static bool TryCreateAsset(Type type, CreateAssetAutomaticallyAttribute attr)
        {
            if (!Application.isEditor || attr == null || CreateAssetAction == null)
                return false;
            var obj = ScriptableObject.CreateInstance(type);
            try
            {
                CreateAssetAction(obj);
                return true;
            }
            catch (Exception e)
            {
                Object.DestroyImmediate(obj);
                throw new EditorAssetFactoryException(type, e);
            }
        }

        private static bool TryLoadFromResources(Type type, LoadFromAssetAttribute[] attributes,
            out ScriptableObject obj)
        {
            foreach (var attribute in attributes)
            {
                var path = attribute.TryGetResourcesPath(type);
                path = PathUtility.GetPathWithoutExtension(path);
                Debug.Log($"Load asset from: {path}");
                obj = Resources.Load(path, type) as ScriptableObject;
                if (obj != null)
                    return true;
            }

            obj = null;
            return false;
        }

        internal static bool TryGetAbsoluteAssetPath(Type type, out string absolutePath)
        {
            var loadFromAssetAttribute = LoadFromAssetUtils.GetFirstAttribute(type);
            if (loadFromAssetAttribute == null)
            {
                absolutePath = null;
                return false;
            }

            absolutePath = loadFromAssetAttribute.Path;
            return true;
        }

        public static Component CreateOrLoadGameObject(Type componentType)
        {
            if (TryInstantiatePrefab(componentType, out var i))
                return i;

            var obj = new GameObject { name = componentType.Name };
            return obj.AddComponent(componentType);
        }

        private static bool TryInstantiatePrefab(Type componentType, out Component component)
        {
            var loadFromAssetAttributes = LoadFromAssetUtils.GetAttributesInOrder(componentType);
            if (loadFromAssetAttributes.Length == 0)
            {
                component = null;
                return false;
            }

            foreach (var attr in loadFromAssetAttributes)
            {
                if (!PathUtility.ContainsFolder("Resources", attr.Path))
                {
                    continue;
                }

                var resourcesPath = attr.Path;
                var prefab = Resources.Load(resourcesPath, componentType);
                Debug.Log(prefab.GetType().Name);
                if (prefab == null)
                    continue;
                component = (Component)Object.Instantiate(prefab);
                component.name = componentType.Name;
                return true;
            }

            var highestPriorityAttr = loadFromAssetAttributes[0];
            if (highestPriorityAttr.Mandatory)
                throw new AssetIsMissingException(componentType, highestPriorityAttr.Path);
            component = null;
            return false;
        }
    }
}