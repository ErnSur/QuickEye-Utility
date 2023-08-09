using System;

namespace OneAsset
{
    /// <summary>
    /// Enables a system that will create scriptable object file if it cannot be loaded from <see cref="LoadFromAssetAttribute.Path"/>
    /// When resolving final asset path the <see cref="LoadFromAssetAttribute"/> with the highest priority will be used
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CreateAssetAutomaticallyAttribute : Attribute
    {
        public CreateAssetAutomaticallyAttribute()
        {
            
        }
        public CreateAssetAutomaticallyAttribute(string depricated)
        {
            
        }
    }
}