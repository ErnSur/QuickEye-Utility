using System;

namespace QuickEye.Utility
{
    /// <summary>
    /// Creates a settings entry in Project Settings Window.
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