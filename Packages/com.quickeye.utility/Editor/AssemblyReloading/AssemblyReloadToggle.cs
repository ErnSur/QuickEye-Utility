using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor.AssemblyReloading
{
    [InitializeOnLoad]
    public static class AssemblyReloadToggle
    {
        private const string LockReloadPrefsKey = "LockReloadAssemblies";
        private const string UIAssetsDirectory = "Packages/com.quickeye.utility/Editor/AssemblyReloading/UI Assets/";
        
        private const string UxmlPath = UIAssetsDirectory + "ExtendedStatusBar.uxml";
        private const string StyleSheetPathDark = UIAssetsDirectory + "ExtendedStatusBar.style.dark.uss";
        private const string StyleSheetPathLight = UIAssetsDirectory + "ExtendedStatusBar.style.light.uss";

        private static bool UserAssemblyLock
        {
            get => EditorPrefs.GetInt(LockReloadPrefsKey, 0) == 1;
            set => EditorPrefs.SetInt(LockReloadPrefsKey, value ? 1 : 0);
        }

        static AssemblyReloadToggle()
        {
            StatusBarExtender.StatusBarCreated += InitializeExtendedStatusBar;
        }

        private static void InitializeExtendedStatusBar(VisualElement rootVisualElement)
        {
            var extBarContainer = AddExtendedStatusBar(rootVisualElement);
            RegisterBarSizeCorrection(rootVisualElement, extBarContainer);
            rootVisualElement.Add(extBarContainer);
            var assemblyToggle = extBarContainer.Q<Toggle>("compilation-toggle");
            SetupAssemblyToggle(assemblyToggle);
        }

        private static void SetupAssemblyToggle(Toggle assemblyToggle)
        {
            assemblyToggle.value = UserAssemblyLock;
            UpdateButtonTooltip(assemblyToggle);
            assemblyToggle.RegisterValueChangedCallback(e =>
            {
                if (UserAssemblyLock == e.newValue)
                    return;
                UserAssemblyLock = e.newValue;
                if (UserAssemblyLock)
                {
                    EditorApplication.LockReloadAssemblies();
                }
                else
                {
                    EditorApplication.UnlockReloadAssemblies();
                    EditorUtility.RequestScriptReload();
                }

                UpdateButtonTooltip(assemblyToggle);
                Debug.Log($"UserAssemblyLock: {UserAssemblyLock}");
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