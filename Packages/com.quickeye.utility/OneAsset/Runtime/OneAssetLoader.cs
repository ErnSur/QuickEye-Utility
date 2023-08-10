using System;
using System.Linq;
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
            return LoadOrCreateInstance(scriptableObjectType, loadAttributes);
        }

        // TODO: support loading from AssetDatabase if path is not in resources folder
        internal static ScriptableObject LoadOrCreateInstance(Type scriptableObjectType,
            params LoadFromAssetAttribute[] loadFromAssetAttributesInOrder)
        {
            if (loadFromAssetAttributesInOrder.Length == 0)
                return CreateInstance(scriptableObjectType);
            // Try to load asset from `LoadFromAssetAttribute` path
            if (TryLoadFromResources(scriptableObjectType, loadFromAssetAttributesInOrder, out var asset))
                return asset;
            var highestPriorityAttribute = loadFromAssetAttributesInOrder[0];
            
            // Try to create asset at path
            if (highestPriorityAttribute.CreateAssetAutomatically && TryCreateAsset(scriptableObjectType,highestPriorityAttribute) &&
                TryLoadFromResources(scriptableObjectType, loadFromAssetAttributesInOrder, out asset))
                return asset;

            // if we came to this point and asset file exists on disk then that mean we are running before AssetDatabase initialized
            // if LoadFromAssetAttribute and CreateAssetAutomatically are preset
            // and we came to this point, it means that AssetDatabase failed to create an asset
            // OOORRR we are in buit game and game was build without the settings asset (what are the scenarios in which this is possible?)
            if (TryLoadUnsafe(scriptableObjectType, highestPriorityAttribute, out asset))
                return asset;

            // Throw Exception if class has `LoadFromAssetAttribute` and asset instance is mandatory 
            if (highestPriorityAttribute.Mandatory)
                throw new AssetIsMissingException(scriptableObjectType, highestPriorityAttribute.Path);
            // Create and return a new instance
            return CreateInstance(scriptableObjectType);
        }
        
        private static bool TryLoadUnsafe(Type scriptableObjectType, LoadFromAssetAttribute highestPriorityAttribute,
            out ScriptableObject instance)
        {
            if (!Application.isEditor
                || !highestPriorityAttribute.LoadAndForget)
            {
                instance = null;
                return false;
            }
#if UNITY_EDITOR
            // Ideally this code would be in editor assembly. But when this method is called from InitializeOnLoad
            // there is no guarantee that editor callback will be registered like with `CreateAssetAction`
            var i = InternalEditorUtility
                .LoadSerializedFileAndForget(highestPriorityAttribute.Path)
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
        /// If in Editor, try to create an asset
        /// </summary>
        private static bool TryCreateAsset(Type type,LoadFromAssetAttribute attribute)
        {
            if (!Application.isEditor || CreateAssetAction == null)
                return false;
            var obj = ScriptableObject.CreateInstance(type);
            try
            {
                CreateAssetAction(obj,attribute);
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
            foreach (var attribute in attributes.Where(a=>a.IsInResourcesFolder))
            {
                var path = attribute.ResourcesPath;
                obj = Resources.Load(path, type) as ScriptableObject;
                if (obj != null)
                    return true;
            }

            obj = null;
            return false;
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

            foreach (var attr in loadFromAssetAttributes.Where(a=>a.IsInResourcesFolder))
            {
                var resourcesPath = attr.ResourcesPath;
                var prefab = Resources.Load(resourcesPath, componentType);

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