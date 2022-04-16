using System;

namespace QuickEye.Utility
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CreateAssetAutomaticallyAttribute : Attribute
    {
        /// <summary>
        /// If singleton cannot be loaded from its resources path it will be created.
        /// </summary>
        /// <param name="fullAssetPath"></param>
        public CreateAssetAutomaticallyAttribute(string fullAssetPath)
        {
            FullAssetPath = fullAssetPath;
        }

        public string FullAssetPath { get; }
    }
}