using UnityEditor;
using UnityEngine;

namespace UnityOne.Editor.EditorGUIExtension
{
    using static SingletonGUI;

    internal class SingletonHeaderDrawer : PostHeaderDrawer
    {
        [InitializeOnLoadMethod]
        private static void Init()
        {
            PersistentPostHeaderManager.EditorCreated += editor =>
            {
                if (ShouldDrawHeader(editor, out var metadata))
                    PersistentPostHeaderManager.AddPostHeaderDrawer(new SingletonHeaderDrawer(editor, metadata));
            };
        }

        public static bool ShouldDrawHeader(UnityEditor.Editor editor, out SingletonAssetCache.AssetMetadata metadata)
        {
            metadata = null;
            return editor.targets.Length == 1 &&
                   EditorUtility.IsPersistent(editor.target) &&
                   SingletonAssetCache.TryGetEntry(editor.serializedObject.targetObject, out metadata) &&
                   metadata.SingletonAssetAttribute != null;
        }

        SingletonAssetCache.AssetMetadata _metadata;

        public SingletonHeaderDrawer(UnityEditor.Editor editor, SingletonAssetCache.AssetMetadata metadata) :
            base(editor)
        {
            _metadata = metadata;
        }

        public override void OnGUI()
        {
            var resPath = _metadata.ResourcesPath;
            var hasCorrectPath = _metadata.IsInLoadablePath;
            using (new GUILayout.HorizontalScope())
            {
                var iconContent = new GUIContent(GetGuiContent(hasCorrectPath, resPath));
                var style = new GUIStyle(EditorStyles.label);
                iconContent.text = " Singleton Asset";
                //style.imagePosition = im
                using (new EditorGUIUtility.IconSizeScope(new Vector2(16, 16)))
                    GUILayout.Label(iconContent, style, GUILayout.ExpandWidth(false));

                //iconContent.image = null;
                //GUILayout.Label(iconContent, GUILayout.ExpandWidth(false));
                GUI.enabled = false;
                EditorGUILayout.TextField($"*/Resources/{resPath}");
                GUI.enabled = true;
            }
        }
    }
}