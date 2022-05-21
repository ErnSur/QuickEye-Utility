using UnityEditor;

namespace QuickEye.Utility.Editor
{
    internal static class OpenPackagesManifestMenuItem
    {
        [MenuItem("Assets/Open Packages Manifest")]
        private static void OpenManifest()
        {
            EditorUtility.OpenWithDefaultApp("Packages/manifest.json");
        }
    }
}