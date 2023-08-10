using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OneAsset
{
    internal static class AssetLoadOptionsUtility
    {
        public static bool HasAttribute(Type type)
        {
            return type.GetCustomAttributes<LoadFromAssetAttribute>().Any();
        }

        public static LoadFromAssetAttribute[] GetAttributesInOrder(Type type)
        {
            return type.GetCustomAttributes<LoadFromAssetAttribute>().OrderByDescending(a => a.Priority).ToArray();
        }

        public static AssetLoadOptions GetLoadOptions(Type type)
        {
            var loadAttributes = GetAttributesInOrder(type);
            var fileExtension = "";
            if (typeof(ScriptableObject).IsAssignableFrom(type))
                fileExtension = ".asset";
            else if (typeof(Component).IsAssignableFrom(type))
                fileExtension = ".prefab";
            return GetLoadOptions(loadAttributes, fileExtension);
        }

        private static AssetLoadOptions GetLoadOptions(LoadFromAssetAttribute[] attributes, string fileExtension)
        {
            attributes = attributes.OrderByDescending(a => a.Priority).ToArray();

            var highestPriorityAttribute = attributes.FirstOrDefault();
            if (highestPriorityAttribute == null)
                return null;

            var options = new AssetLoadOptions(attributes.Select(a =>
            {
                var path = a.Path;
                if (!path.EndsWith(fileExtension))
                    path = $"{path}{fileExtension}";
                return path;
            }).ToArray())
            {
                AssetIsMandatory = highestPriorityAttribute.AssetIsMandatory,
                CreateAssetIfMissing = highestPriorityAttribute.CreateAssetIfMissing,
                LoadAndForget = highestPriorityAttribute.LoadAndForget
            };
            return options;
        }
    }
}