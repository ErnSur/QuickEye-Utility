using System;

namespace QuickEye.Utility
{
    public class SingletonAssetIsMissingException : Exception
    {
        internal SingletonAssetIsMissingException(Type assetType, string assetPath) : base(
            $"Asset of type {assetType} is missing from path: Resources/{assetPath}")
        {
        }
    }
}