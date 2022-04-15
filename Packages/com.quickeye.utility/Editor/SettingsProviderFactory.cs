using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    internal static class SettingsProviderFactory
    {
        [SettingsProviderGroup]
        private static SettingsProvider[] GetProviders()
        {
            var providers = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where Attribute.IsDefined(type, typeof(SettingsProviderAttribute))
                where Attribute.IsDefined(type, typeof(SingletonAssetAttribute))
                where typeof(ScriptableSingleton).IsAssignableFrom(type)
                let settingsAssetAttribute = type.GetCustomAttribute<SettingsProviderAttribute>()
                select CreateSettingsProvider(settingsAssetAttribute.SettingsWindowPath, type);

            return providers.ToArray();
        }

        private static SettingsProvider CreateSettingsProvider(string settingsWindowPath, Type type)
        {
            var prop = type.BaseType.GetProperty("Instance", 
                 BindingFlags.Public | BindingFlags.Static);
            var obj = prop?.GetValue(null) as ScriptableObject;
            
            var provider = AssetSettingsProvider.CreateProviderFromObject(
                settingsWindowPath, obj,
                SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(obj)));

            return provider;
        }
    }
}