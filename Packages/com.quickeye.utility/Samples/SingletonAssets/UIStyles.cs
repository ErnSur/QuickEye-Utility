using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    [SingletonAsset(ResourcesPath)]
    [CreateAssetAutomatically(AutoCreatePath)]
    [SettingsProvider("Project/Custom Settings From Scriptable Singleton")]
    public class UIStyles : ScriptableSingleton<UIStyles>
    {
        private const string ResourcesPath = "UI Styles";
        private const string AutoCreatePath = "Assets/Settings/Resources/" + ResourcesPath;

        public Color popupTextColor = Color.black;
    }
}