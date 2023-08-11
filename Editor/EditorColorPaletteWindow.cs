using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace QuickEye.Utility.Editor
{
    internal class EditorColorPaletteWindow : EditorWindow
    {
        [MenuItem("Window/Editor Color Palette")]
        public static void OpenWindow()
        {
            GetWindow<EditorColorPaletteWindow>();
        }

        [SerializeField]
        private EditorColorPalette light = EditorColorPalette.Light;

        [SerializeField]
        private EditorColorPalette dark = EditorColorPalette.Dark;

        [SerializeField]
        private Vector2 scrollPos;

        [SerializeField]
        private bool isDarkSkinSelected;

        [SerializeField]
        private bool initialized;

        [SerializeField]
        private string searchString;

        private SearchField _searchField;

        private void OnEnable()
        {
            UpdateTitle();
            if (!initialized)
            {
                isDarkSkinSelected = EditorGUIUtility.isProSkin;
                initialized = true;
            }

            _searchField = new SearchField();
        }

        private void UpdateTitle()
        {
            try
            {
                var iconContent = EditorGUIUtility.IconContent("SceneViewRGB");
                titleContent = iconContent;
            }
            catch
            {
            }

            titleContent.text = "Editor Color Palette";
        }

        private void OnGUI()
        {
            using (new HorizontalScope(EditorStyles.toolbar))
            {
                isDarkSkinSelected = !GUILayout.Toggle(!isDarkSkinSelected, "Light", EditorStyles.toolbarButton);
                isDarkSkinSelected = GUILayout.Toggle(isDarkSkinSelected, "Dark", EditorStyles.toolbarButton);
            }

            searchString = _searchField.OnGUI(searchString);

            var so = new SerializedObject(this);
            var prop = so.FindProperty(isDarkSkinSelected ? nameof(dark) : nameof(light));
            EditorGUIUtility.labelWidth = 270;
            using (var scrollScope = new ScrollViewScope(scrollPos))
            {
                foreach (var property in GetPropChildren(prop))
                {
                    var label = GetDisplayNameOfPropertyBackingField(property.displayName);
                    if (!string.IsNullOrWhiteSpace(searchString) &&
                        !label.ToUpperInvariant().Contains(searchString.ToUpperInvariant()))
                        continue;
                    PropertyField(property, new GUIContent(label));
                }

                scrollPos = scrollScope.scrollPosition;
            }
        }

        private static string GetDisplayNameOfPropertyBackingField(string backingFieldName)
        {
            const int indexOfLessThanSign = 1;

            if (backingFieldName.StartsWith("<", StringComparison.CurrentCulture))
            {
                backingFieldName = backingFieldName.Substring(indexOfLessThanSign,
                    backingFieldName.IndexOf('>') - indexOfLessThanSign);
            }

            return backingFieldName;
        }

        private static IEnumerable<SerializedProperty> GetPropChildren(SerializedProperty property)
        {
            var iterator = property.Copy();
            var end = iterator.GetEndProperty();
            iterator.NextVisible(true);
            while (!SerializedProperty.EqualContents(iterator, end))
            {
                yield return iterator.Copy();
                if (!iterator.NextVisible(false))
                    break;
            }
        }
    }
}