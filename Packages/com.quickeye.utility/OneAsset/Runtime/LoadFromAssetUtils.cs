using System;
using System.Linq;
using System.Reflection;

namespace OneAsset
{
    internal static class LoadFromAssetUtils
    {
        public static bool HasAttribute(Type type)
        {
            return type.GetCustomAttributes<LoadFromAssetAttribute>().Any();
        }
        
        public static LoadFromAssetAttribute GetFirstAttribute(Type type)
        {
            return type.GetCustomAttributes<LoadFromAssetAttribute>().OrderByDescending(a => a.Priority).FirstOrDefault();
        }
        
        public static LoadFromAssetAttribute[] GetAttributesInOrder(Type type)
        {
            return type.GetCustomAttributes<LoadFromAssetAttribute>().OrderByDescending(a => a.Priority).ToArray();
        }
    }
}