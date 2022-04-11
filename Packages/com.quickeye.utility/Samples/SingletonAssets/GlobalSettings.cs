using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    [SingletonAsset("Resources/Global Settings")]
    public class GlobalSettings : ScriptableSingleton<GlobalSettings>
    {
        public Color popupTextColor = Color.black;
    }
}