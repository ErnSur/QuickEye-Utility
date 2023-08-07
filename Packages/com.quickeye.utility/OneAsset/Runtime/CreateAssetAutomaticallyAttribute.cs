using System;

namespace OneAsset
{
    /// <summary>
    /// Enables a system that will create scriptable object file at specific path
    /// if it cannot be loaded from its resources path. Must be combined with <see cref="LoadFromAssetAttribute"/>
    /// When resolving final asset path the <see cref="LoadFromAssetAttribute"/> with the highest priority will be used
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CreateAssetAutomaticallyAttribute : Attribute
    {
        /// <summary>
        /// If asset cannot be loaded from its resources path it will be created.
        /// </summary>
        /// <param name="resourcesFolderPath">Path to "Resources" folder. Must be inside "Assets" folder.</param>
        public CreateAssetAutomaticallyAttribute(string resourcesFolderPath)
        {
            ResourcesFolderPath = resourcesFolderPath;
        }
        
        public string ResourcesFolderPath { get; }
    }
}