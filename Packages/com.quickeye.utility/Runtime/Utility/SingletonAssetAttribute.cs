using System;

namespace QuickEye.Utility
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SingletonAssetAttribute : Attribute
    {
        /// <summary>
        /// Path at which singleton asset should be found. Relative to the Resources folder.
        /// </summary>
        public string ResourcesPath { get; }

        /// <summary>
        /// Applicable only for `ScriptableSingleton`:
        /// If set to `true` singleton will throw an exception in case where there was no asset under `ResourcesPath`.
        /// If set to `false` singleton will dynamically create a new runtime instance if there is no asset present.
        /// By default `true`.
        /// </summary>
        public bool Mandatory { get; set; } = true;

        public SingletonAssetAttribute(string resourcesPath)
        {
            ResourcesPath = TrimPath(resourcesPath,"Resources");
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