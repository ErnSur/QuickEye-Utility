using System;
using UnityEngine;
using static UnityEngine.GUILayout;
using Object = UnityEngine.Object;

namespace QuickEye.DevTools
{
    using UnityEditor;

    public class PropScreenshotMakerWindow : EditorWindow
    {
        [MenuItem("Quick Eye Dev/Prop Screenshot Maker")]
        public static void OpenWindow()
        {
            GetWindow<PropScreenshotMakerWindow>().titleContent = new GUIContent("Prop Screenshot Maker");
        }

        [SerializeField]
        private Object targetObject;

        [SerializeField]
        private string propertyPath;

        [SerializeField]
        private RectOffset padding;

        private Rect _propRect;
        private SerializedProperty _prop;

        private void OnEnable()
        {
            padding = new RectOffset(5, 5, 5, 5);
            TryGetProperty();
        }

        private void TryGetProperty()
        {
            if (targetObject == null || string.IsNullOrWhiteSpace(propertyPath))
                return;
            try
            {
                var serObject = new SerializedObject(targetObject);
                _prop = serObject.FindProperty(propertyPath);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnGUI()
        {
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                var so = new SerializedObject(this);
                EditorGUILayout.PropertyField(so.FindProperty(nameof(padding)));
                if (change.changed)
                    so.ApplyModifiedProperties();
            }

            using (var change = new EditorGUI.ChangeCheckScope())
            {
                targetObject = EditorGUILayout.ObjectField("Target", targetObject, typeof(Object), true);
                propertyPath = EditorGUILayout.TextField("Property Path", propertyPath);
                if (change.changed)
                    TryGetProperty();
            }

            if (_prop == null)
                return;

            if (Button("Grab Screen Shot"))
            {
                var r = padding.Add(_propRect);
                r.position = GUIUtility.GUIToScreenPoint(r.position);
                ScreenShotUtility.SaveScreenShot(r, _prop.displayName);
            }

            DrawProperty();
        }

        private void DrawProperty()
        {
            EditorGUIUtility.wideMode = true;
            using (new CenterScope())
            using (new HorizontalScope())
            {
                using (new CenterScope())
                using (new VerticalScope(Width(350)))
                {
                    //var content = new GUIContent(prop.displayName);
                    //var rect = GUILayoutUtility.GetRect(content, EditorStyles.objectField, null);
                    EditorGUILayout.PropertyField(_prop);
                    //EditorGUI.PropertyField(rect, prop, content);
                    if (Event.current.type == EventType.Repaint)
                        _propRect = GUILayoutUtility.GetLastRect();
                }
            }
        }

        public class CenterScope : IDisposable
        {
            public CenterScope()
            {
                FlexibleSpace();
            }

            public void Dispose()
            {
                FlexibleSpace();
            }
        }
    }
}