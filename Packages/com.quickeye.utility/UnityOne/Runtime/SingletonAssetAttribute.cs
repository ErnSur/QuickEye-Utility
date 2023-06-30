using System;
using UnityEditor;

namespace QuickEye.Utility
{
    /// <summary>
    /// Allows singleton instances to load from an asset.
    /// Can be used on <see cref="SingletonMonoBehaviour{T}"/> and <see cref="SingletonScriptableObject{T}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SingletonAssetAttribute : Attribute
    {
        private string ResourcesPath { get; }

        /// <summary>
        /// Relevant only for <see cref="SingletonScriptableObject{T}"/>:
        /// If set to `true` singleton will throw an exception in case where there was no asset under `ResourcesPath`.
        /// If set to `false` singleton will dynamically create a new runtime instance if there is no asset present.
        /// By default `true`.
        /// </summary>
        public bool Mandatory { get; set; } = true;
        public bool UseTypeNameAsFileName { get; set; }

        /// <summary>
        /// Path at which singleton asset should be found. Relative to the Resources folder.
        /// Doesn't have to contain file name if UseTypeNameAsFileName is set to true.
        /// Enter prefab path in case of <see cref="SingletonMonoBehaviour{T}"/>
        /// </summary>
        /// <param name="resourcesPath">
        /// Path at which singleton asset should be found. Relative to the Resources folder.
        /// Doesn't have to contain file name if UseTypeNameAsFileName is set to true.
        /// Enter prefab path in case of <see cref="SingletonMonoBehaviour{T}"/>,
        /// Enter asset path in case of <see cref="SingletonScriptableObject{T}"/>
        /// </param>
        public SingletonAssetAttribute(string resourcesPath)
        {
            ResourcesPath = TrimPath(resourcesPath, "Resources");
        }

        public string GetResourcesPath(Type owner)
        {
            return UseTypeNameAsFileName
                ? $"{ResourcesPath}/{ObjectNames.NicifyVariableName(owner.Name)}".TrimStart('/')
                : ResourcesPath;
        }


        private static string TrimPath(string path, string startDir)
        {
            path = path.TrimStart('/');
            startDir = startDir.TrimStart('/');
            if (path.StartsWith(startDir))
            {
                path = path.Substring(startDir.Length).TrimStart('/');
            }

            return path;
        }
    }
}