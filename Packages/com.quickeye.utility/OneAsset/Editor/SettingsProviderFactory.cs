using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor
{
    internal static class SettingsProviderFactory
    {
        [SettingsProviderGroup]
        private static SettingsProvider[] GetProviders()
        {
            var providers = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where Attribute.IsDefined(type, typeof(SettingsProviderAssetAttribute))
                where Attribute.IsDefined(type, typeof(LoadFromAssetAttribute))
                where typeof(OneScriptableObject).IsAssignableFrom(type)
                let settingsAssetAttribute = type.GetCustomAttribute<SettingsProviderAssetAttribute>()
                let provider = CreateSettingsProvider(settingsAssetAttribute.SettingsWindowPath, type)
                where provider != null
                select provider;

            return providers.ToArray();
        }

        private static SettingsProvider CreateSettingsProvider(string settingsWindowPath, Type type)
        {
            try
            {
                var prop = type.BaseType.GetProperty("Instance",
                    BindingFlags.Public | BindingFlags.Static);
                var obj = prop?.GetValue(null) as ScriptableObject;

                var provider = AssetSettingsProvider.CreateProviderFromObject(
                    settingsWindowPath, obj,
                    SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(obj)));

                return provider;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create a settings provider for: {type.Name}, {e}");
                return null;
            }
        }
    }
}