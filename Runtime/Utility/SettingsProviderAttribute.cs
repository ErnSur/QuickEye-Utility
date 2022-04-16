using System;

namespace QuickEye.Utility
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SettingsProviderAttribute : Attribute
    {
        public SettingsProviderAttribute(string settingsWindowPath)
        {
            SettingsWindowPath = settingsWindowPath;
        }

        public string SettingsWindowPath { get; }
    }
}