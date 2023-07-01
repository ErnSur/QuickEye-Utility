using System;
using System.Collections.Generic;
using System.Linq;
using OneAsset.Editor.EditorGUIExtension;
using UnityEditor;

namespace OneAsset.Editor.UI
{
    [InitializeOnLoad]
    internal static class PersistentPostHeaderManager
    {
        public static event Action<UnityEditor.Editor> EditorCreated;

        private static readonly Dictionary<UnityEditor.Editor, List<PostHeaderDrawer>> DrawersWithTarget = new
            Dictionary<UnityEditor.Editor, List<PostHeaderDrawer>>();

        static PersistentPostHeaderManager()
        {
            UnityEditor.Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }

        public static void AddPostHeaderDrawer(PostHeaderDrawer drawer)
        {
            if (DrawersWithTarget.TryGetValue(drawer.Editor, out var drawers))
            {
                drawers.Add(drawer);
                return;
            }

            DrawersWithTarget[drawer.Editor] = new List<PostHeaderDrawer> { drawer };
        }

        private static void OnPostHeaderGUI(UnityEditor.Editor editor)
        {
            if (!DrawersWithTarget.ContainsKey(editor))
            {
                DrawersWithTarget.Add(editor, new List<PostHeaderDrawer>());
                EditorCreated?.Invoke(editor);
            }

            foreach (var drawer in DrawersWithTarget[editor])
            {
                drawer.OnGUI();
            }

            TryClearCache();
        }

        private static void TryClearCache()
        {
            foreach (var kvp in DrawersWithTarget.Where(kvp => kvp.Key == null).ToArray())
            {
                DrawersWithTarget.Remove(kvp.Key);
            }
        }
    }
}