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
            return type.GetCustomAttributes<LoadFromAssetAttribute>().OrderBy(a => a.Order).FirstOrDefault();
        }
        
        public static LoadFromAssetAttribute[] GetAttributesInOrder(Type type)
        {
            return type.GetCustomAttributes<LoadFromAssetAttribute>().OrderBy(a => a.Order).ToArray();
        }
    }
}