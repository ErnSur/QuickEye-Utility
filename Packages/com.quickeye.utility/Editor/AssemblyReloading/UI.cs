using UnityEditor;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor.AssemblyReloading
{
    [InitializeOnLoad]
    internal static class UI
    {
        private const string UIAssetsDirectory = "Packages/com.quickeye.utility/Editor/AssemblyReloading/UI Assets/";

        private const string UxmlPath = UIAssetsDirectory + "ExtendedStatusBar.uxml";
        private const string StyleSheetPathDark = UIAssetsDirectory + "ExtendedStatusBar.style.dark.uss";
        private const string StyleSheetPathLight = UIAssetsDirectory + "ExtendedStatusBar.style.light.uss";

        static UI()
        {
            StatusBarExtender.StatusBarCreated += InitializeExtendedStatusBar;
        }

        private static void InitializeExtendedStatusBar(VisualElement rootVisualElement)
        {
            var extBarContainer = AddExtendedStatusBar(rootVisualElement);
            RegisterBarSizeCorrection(rootVisualElement, extBarContainer);
            rootVisualElement.Add(extBarContainer);
            var assemblyReloadToggle = extBarContainer.Q<Toggle>("compilation-toggle");
            SetupAssemblyReloadToggle(assemblyReloadToggle);
        }

        private static void SetupAssemblyReloadToggle(Toggle assemblyReloadToggle)
        {
            assemblyReloadToggle.value = AssemblyReloadLock.IsLocked;
            UpdateButtonTooltip(assemblyReloadToggle);
            assemblyReloadToggle.RegisterValueChangedCallback(e =>
            {
                AssemblyReloadLock.IsLocked = e.newValue;
                UpdateButtonTooltip(assemblyReloadToggle);
            });
        }

        private static VisualElement AddExtendedStatusBar(VisualElement rootVisualElement)
        {
            var extTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            var themeStyleSheetPath = EditorGUIUtility.isProSkin ? StyleSheetPathDark : StyleSheetPathLight;
            var extStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(themeStyleSheetPath);

            rootVisualElement.styleSheets.Add(extStyleSheet);
            extTemplate.CloneTree(rootVisualElement);
            var extBarContainer = rootVisualElement.Q(null, "ext-status-bar");
            return extBarContainer;
        }

        private static void RegisterBarSizeCorrection(VisualElement rootVisualElement, VisualElement extBarContainer)
        {
            var originalBar = rootVisualElement.Q<IMGUIContainer>(null, "status-bar");
            extBarContainer.RegisterCallback<GeometryChangedEvent>(e =>
            {
                originalBar.style.right = extBarContainer.layout.width;
            });
        }

        private static void UpdateButtonTooltip(Toggle toggle)
        {
            toggle.tooltip = toggle.value ? "Assembly reload disabled" : "Assembly reload enabled";
        }
    }
}