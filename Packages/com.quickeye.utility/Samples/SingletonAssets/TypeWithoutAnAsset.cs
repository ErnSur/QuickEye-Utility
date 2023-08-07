using OneAsset;
using UnityEngine;

namespace QuickEye.Samples.SingletonAssets
{
    public class TypeWithoutAnAsset : OneScriptableObject<TypeWithoutAnAsset>
    {
        [TextArea]
        public string Description =
            $"Call to `{nameof(TypeWithoutAnAsset)}.Instance` will create a new object instance if it wasn't created before." +
            "It will just create an object instance without creating an Scriptable Object Asset inside the project.";
    }
}