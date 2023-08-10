using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.UI
{
    using static LoadableAssetGUI;

    // TODO: add an icon in the header drawer to indicate if the asset is loadable in runtime or just editor. It should have an icon of "?" in a circle. like help button. it could also display Path formatting rules for all of the options enabled
    internal class AssetPathHeaderDrawer : PostHeaderDrawer
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            PersistentPostHeaderManager.EditorCreated += editor =>
            {
                if (ShouldDrawHeader(editor, out var metadata))
                    PersistentPostHeaderManager.AddPostHeaderDrawer(new AssetPathHeaderDrawer(editor, metadata));
            };
        }

        private static bool ShouldDrawHeader(UnityEditor.Editor editor, out AssetMetadata metadata)
        {
            metadata = null;
            return editor.targets.Length == 1 &&
                   EditorUtility.IsPersistent(editor.target) &&
                   LoadFromAssetCache.TryGetEntry(editor.serializedObject.targetObject, out metadata) &&
                   metadata.LoadOptions.Paths.Length > 0;
        }

        private readonly AssetMetadata _metadata;

        private AssetPathHeaderDrawer(UnityEditor.Editor editor, AssetMetadata metadata) :
            base(editor)
        {
            _metadata = metadata;
        }

        public override void OnGUI()
        {
            var hasCorrectPath = _metadata.IsInLoadablePath(out var loadPath);
            if (!hasCorrectPath)
                loadPath = _metadata.LoadOptions.AssetPaths[0];

            using (new GUILayout.HorizontalScope())
            {
                var pathText = loadPath.ToString();
                var labelContent = new GUIContent(GetGuiContent(hasCorrectPath, pathText, _metadata.TypeName));
                var labelStyle = new GUIStyle(EditorStyles.label);
                labelStyle.margin.left += 2;
                labelStyle.margin.right += 2;
                labelContent.text = "Load Path";
                labelContent.tooltip = pathText;
                var labelRect = GUILayoutUtility.GetRect(labelContent, labelStyle, GUILayout.ExpandWidth(false));
                var textFieldRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.textField);

                GUI.enabled = false;
                EditorGUI.TextField(textFieldRect, pathText);
                GUI.enabled = true;

                if (GUI.RepeatButton(textFieldRect, GUIContent.none, GUIStyle.none))
                {
                    labelContent.text = "Copied";
                    GUIUtility.systemCopyBuffer = pathText;
                }

                using (new EditorGUIUtility.IconSizeScope(new Vector2(16, 16)))
                    GUI.Label(labelRect, labelContent, labelStyle);
            }
        }
    }
}