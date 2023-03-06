using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    /// <summary>
    /// Creates a new Project Settings Window page using <see cref="SingletonScriptableObject"/>
    /// with <see cref="SingletonAssetAttribute"/> and <see cref="SettingsProviderAssetAttribute"/>
    /// </summary>
    [SingletonAsset(ResourcesPath, Mandatory = true)]
    [CreateAssetAutomatically(AutoCreatePath)]
    [SettingsProviderAsset("Project/" + SettingsPageName)]
    public class ProjectSettingsAsset : SingletonScriptableObject<ProjectSettingsAsset>
    {
        private const string ResourcesPath = nameof(ProjectSettingsAsset);
        private const string AutoCreatePath = "Assets/Samples/Settings/Resources/" + ResourcesPath;
        private const string SettingsPageName = "Your Settings Page Name";

        [TextArea]
        public string Description =
            $"To see new Project Window Settings Page open menu Edit/Project Settings. And find a page named: {SettingsPageName}";
    }
}