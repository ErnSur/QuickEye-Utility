using System;
using System.Linq;
using System.Reflection;

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
            return GetLoadOptions(loadAttributes);
        }

        private static AssetLoadOptions GetLoadOptions(LoadFromAssetAttribute[] attributes)
        {
            attributes = attributes.OrderByDescending(a => a.Priority).ToArray();

            var highestPriorityAttribute = attributes.FirstOrDefault();
            if (highestPriorityAttribute == null)
                return null;

            var options = new AssetLoadOptions(attributes.Select(a => a.Path).ToArray())
            {
                AssetIsMandatory = highestPriorityAttribute.AssetIsMandatory,
                CreateAssetIfMissing = highestPriorityAttribute.CreateAssetIfMissing,
                LoadAndForget = highestPriorityAttribute.LoadAndForget
            };
            return options;
        }
    }
}