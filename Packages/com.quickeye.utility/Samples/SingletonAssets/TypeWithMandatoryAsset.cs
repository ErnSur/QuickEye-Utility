using OneAsset;
using UnityEngine;

namespace QuickEye.Samples.SingletonAssets
{
    /// <summary>
    /// A ScriptableObject singleton
    /// It defines a path from which it should be loaded from.
    /// And another path in which the asset should be created if it does not exists.
    ///
    /// The <see cref="LoadFromAssetAttribute"/> defines a path from which asset will be loaded when `Instance` is requested.
    /// 
    /// The <see cref="CreateAssetAutomaticallyAttribute"/> turns on a editor-only system that will create scriptable object file at specific path
    /// if it cannot be loaded from a path defined in <see cref="LoadFromAssetAttribute"/>
    /// </summary>
    [LoadFromAsset(Path, Mandatory = true, CreateAssetAutomatically = true)]
    public class TypeWithMandatoryAsset : OneScriptableObject<TypeWithMandatoryAsset>
    {
        private const string Path = "Assets/Samples/Settings/Resources/" + nameof(TypeWithMandatoryAsset);

        [TextArea(10,30)]
        public string Description =
            $"Call to `{nameof(TypeWithMandatoryAsset)}.Instance` will try to load an asset from Resources folder at path specified in `{nameof(LoadFromAssetAttribute)}.`" +
            $"If there is no asset at that path a new Asset will be created at: {Path}";
    }
}