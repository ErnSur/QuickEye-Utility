using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OneAsset
{
    public static class ScriptableObjectFactory
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
            // Try to load asset from `SingletonAssetAttribute` path
            if (TryLoadFromResources(scriptableObjectType, out var asset))
                return asset;
            // Try to create asset at `CreateAssetAutomaticallyAttribute` path
            if (TryCreateAsset(scriptableObjectType) && TryLoadFromResources(scriptableObjectType, out asset))
                return asset;
            // Throw Exception if class has `SingletonAssetAttribute` and asset instance is mandatory 
            var att = LoadFromAssetUtils.GetAttribute(scriptableObjectType);
            if (att?.Mandatory == true)
                throw new AssetIsMissingException(scriptableObjectType, att.GetResourcesPath(scriptableObjectType));
            // Create and return a new instance
            var obj = ScriptableObject.CreateInstance(scriptableObjectType);
            obj.name = scriptableObjectType.Name;
            return obj;
        }

        /// <summary>
        /// If in Editor, try to create an asset at path specified in `CreateAssetAutomaticallyAttribute`
        /// </summary>
        private static bool TryCreateAsset(Type type)
        {
            if (!Application.isEditor || CreateAssetAction == null)
                return false;
            var att = type.GetCustomAttribute<CreateAssetAutomaticallyAttribute>();
            if (att == null)
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

        private static bool TryLoadFromResources(Type type, out ScriptableObject obj)
        {
            var attr = LoadFromAssetUtils.GetAttribute(type);
            if (attr == null)
                return obj = null;
            var path = attr.GetResourcesPath(type);

            obj = Resources.Load(path, type) as ScriptableObject;
            return obj != null;
        }

    }
}