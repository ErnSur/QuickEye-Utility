using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    // Test if editor.CreateInspectorGUI() returns something.
    // if it does, useit if not use custom default UITK editor.
    // - problem: whats the UITK euivalent of SerializedPropertyScope?
    // if it fails check how Preset editor is made
    [CustomEditor(typeof(ScriptableJsonImporter))]
    public class ScriptableJsonImporterEditor : ScriptedImporterEditor
    {
        private Editor _editor;
        private ScriptableObject _tempSo;
        private SerializedProperty _prototypeProp;
        private SerializedObject _prototype;
        private IVariantSerializer _serializer;
        private string _path;
        private string _fileContent;
        private bool _serializedObjectModified;
        protected override bool needsApplyRevert => true;
        public override bool showImportedObject => false;

        public override void OnEnable()
        {
            base.OnEnable();
            Setup();
        }

        private void Setup()
        {
            _serializedObjectModified = false;
            _prototypeProp = serializedObject.FindProperty("prototype");
            if (_prototypeProp.objectReferenceValue != null && assetTarget != null)
            {
                _prototype = new SerializedObject(_prototypeProp.objectReferenceValue);
                _tempSo = (ScriptableObject)Instantiate(assetTarget);
                _tempSo.name = assetTarget.name;
                _editor = CreateEditor(_tempSo);
                _fileContent = File.ReadAllText(_path = AssetDatabase.GetAssetPath(target));
                _serializer = new DicVariantSerializer(_fileContent);
            }
            else
            {
                Debug.Log($"Missing prototype");
            }
        }

        public override void OnInspectorGUI()
        {
            ApplyRevertGUI();

            EditorGUILayout.PropertyField(_prototypeProp);
            EditorApplication.contextualPropertyMenu += OnContextClick;

            using (new PropertyWrapperScope(BeginProperty, EndProperty))
            {
                if (_editor != null)
                    _editor.OnInspectorGUI();
            }

            EditorApplication.contextualPropertyMenu -= OnContextClick;
        }

        private void BeginProperty(Rect rect, SerializedProperty property)
        {
            // BUG: because the last EndProperty is called on dispose and not just after the last property Field is rendered
            // there is a window where some other `EditorGUI.EndChangeCheck()` can be invoked
            EditorGUI.BeginChangeCheck();
            
            BeginOverrideProperty(rect, property);
        }

        private void BeginOverrideProperty(Rect rect, SerializedProperty property)
        {
            var modified = IsPropOverriden(property);
            rect.width = 2;
            rect.x = 0;
            // if modified
            if (modified)
                EditorGUI.DrawRect(rect, Color.cyan);
            // if modified
            GUI.skin.FindStyle("ControlLabel").fontStyle = modified ? FontStyle.Bold : FontStyle.Normal;
            GUI.skin.FindStyle("textfield").fontStyle = modified ? FontStyle.Bold : FontStyle.Normal;
        }

        private void EndProperty(Rect _, SerializedProperty property)
        {
            EndOverrideProperty();
            EndChangeCheck(property);
        }

        //Doesnt always work
        // workaround:
        // do the one change check on the entire editor.OnInspectorGUI
        // on change, find all changed properties? how?
        // by comparing the serializedObjects to its copy created before OnInspectorGUI?
        private void EndChangeCheck(SerializedProperty property)
        {
            if (EditorGUI.EndChangeCheck())
            {
                _serializer.Set(property);
                _serializedObjectModified = true;
            }
        }

        private static void EndOverrideProperty()
        {
            GUI.skin.FindStyle("ControlLabel").fontStyle = FontStyle.Normal;
            GUI.skin.FindStyle("textfield").fontStyle = FontStyle.Normal;
        }

        private void OnContextClick(GenericMenu menu, SerializedProperty property)
        {
            if (property.serializedObject != _editor.serializedObject)
                return;
            if (!IsPropOverriden(property))
                return;
            var prop = property.Copy();
            menu.AddItem(new GUIContent("Revert"), false, () => RevertProperty(prop));
        }

        private void RevertProperty(SerializedProperty property)
        {
            _serializer.Remove(property);
            var prototypeProp = _prototype.FindProperty(property.propertyPath);
            prototypeProp.PasteValueTo(property);
            _serializedObjectModified = true;
        }

        private bool IsPropOverriden(SerializedProperty property)
        {
            return _serializer.Contains(property);
        }

        protected override void OnHeaderGUI()
        {
            if (_editor != null)
                _editor.DrawHeader();
            else
                base.OnHeaderGUI();
        }

        private void OnDestroy()
        {
            DestroyImmediate(_editor);
            DestroyImmediate(_tempSo);
        }

        protected override void Apply()
        {
            base.Apply();
            var newFileContent = _serializer.Serialize();
            File.WriteAllText(_path, newFileContent);
            AssetDatabase.ImportAsset(_path);
            _serializedObjectModified = false;
            Setup();
        }

        protected override void ResetValues()
        {
            base.ResetValues();
            Setup();
        }

        public override bool HasPreviewGUI() => true;

        [SerializeField]
        private Vector2 previewScrollPos;

        public override void DrawPreview(Rect previewArea)
        {
            var style = new GUIStyle(EditorStyles.largeLabel);
            style.font = Resources.Load<Font>("com.quickeye.sjson/RobotoMono-Regular");
            style.fontSize = 13;
            GUI.BeginScrollView(previewArea, previewScrollPos, previewArea);
            GUI.Label(previewArea, _fileContent, style);
            GUI.EndScrollView();
        }

        public override bool HasModified() => _serializedObjectModified;
    }
}