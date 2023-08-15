using System;
using System.Linq;
using QuickEye.Utility;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    /// <summary>
    /// Serializes all property overrides in a dictionary where key is propertyPath (foo.bar[0]) and value ("foo")
    /// no Json.NET dependency
    /// </summary>
    [Serializable]
    internal class DicVariantSerializer : IVariantSerializer
    {
        [SerializeField]
        // key: SerProp path
        private UnityDictionary<string, string> dic = new UnityDictionary<string, string>();

        public DicVariantSerializer(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }

        public void Overwrite(ScriptableObject objectToOverwrite)
        {
            var serObj = new SerializedObject(objectToOverwrite);
            foreach (var kvp in dic)
            {
                var prop = serObj.FindProperty(kvp.Key);
                prop.SetValue(kvp.Value);
            }

            // should it use ApplyModifiedPropertiesWithoutUndo?
            serObj.ApplyModifiedProperties();
        }

        public void Set(SerializedProperty property)
        {
            dic[property.propertyPath] = property.GetSerializableValue();
        }

        public void Remove(SerializedProperty property)
        {
            dic.Remove(property.propertyPath);
        }

        public bool Contains(SerializedProperty property)
        {
            return dic.ContainsKey(property.propertyPath);
        }

        public string Serialize()
        {
            return JsonUtility.ToJson(this,true);
        }

    }
}