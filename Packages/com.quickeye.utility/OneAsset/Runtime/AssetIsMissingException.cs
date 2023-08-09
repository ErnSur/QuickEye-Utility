using System;

namespace OneAsset
{
    public class AssetIsMissingException : Exception
    {
        internal AssetIsMissingException(Type assetType, string assetPath) : base(
            $"Asset of type {assetType} is missing from path: {assetPath}")
        {
        }
    }
}