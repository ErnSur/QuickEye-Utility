using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    internal interface IVariantSerializer
    {
        public void Overwrite(ScriptableObject objectToOverwrite);
        public void Set(SerializedProperty property);
        public void Remove(SerializedProperty property);
        public bool Contains(SerializedProperty property);
        public string Serialize();
    }
}