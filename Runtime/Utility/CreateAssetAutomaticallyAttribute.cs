using System;

namespace QuickEye.Utility
{
    /// <summary>
    /// Enables a system that will create scriptable object file at specific path
    /// if it cannot be loaded from its resources path 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CreateAssetAutomaticallyAttribute : Attribute
    {
        /// <summary>
        /// If singleton cannot be loaded from its resources path it will be created.
        /// </summary>
        /// <param name="fullAssetPath">Path at which asset will be created. Must be nested in "Assets" and "Resources" folders.</param>
        public CreateAssetAutomaticallyAttribute(string fullAssetPath)
        {
            FullAssetPath = fullAssetPath;
        }

        public string FullAssetPath { get; }
    }
}