using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{

    // UIStyles is a ScriptableSingleton- a singleton that has ScriptableObject as a base class.
    // It uses following attributes to customize its usage:

    // SingletonAsset Attribute will define a path at which the singleton instance should be saved and loaded from.
    [SingletonAsset(ResourcesPath)]
    
    // CreateAssetAutomatically Attribute turns on a system that will create scriptable object file at specific path
    // if it cannot be loaded from its resources path 
    [CreateAssetAutomatically(AutoCreatePath)]
    
    // SettingsProvider Attribute combined with SingletonAsset Attribute
    // will create a settings entry in Project Settings Window.
    [SettingsProvider("Project/Custom Settings From Scriptable Singleton")]
    public class UIStyles : ScriptableSingleton<UIStyles>
    {
        private const string ResourcesPath = "UI Styles";
        private const string AutoCreatePath = "Assets/Settings/Resources/" + ResourcesPath;

        public Color popupTextColor = Color.black;
    }
}