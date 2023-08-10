using System;

namespace OneAsset
{
    /// <summary>
    /// Creates a settings entry in Project Settings Window.
    /// Target type has to have the <see cref="LoadFromAssetAttribute"/> and derive from <see cref="OneScriptableObject{T}"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SettingsProviderAssetAttribute : Attribute
    {
        public SettingsProviderAssetAttribute(string settingsWindowPath)
        {
            SettingsWindowPath = settingsWindowPath;
        }

        public string SettingsWindowPath { get; }
    }
}