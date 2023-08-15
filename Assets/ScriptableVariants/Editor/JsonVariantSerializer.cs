using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    internal class JsonVariantSerializer : IVariantSerializer
    {
        private readonly JObject _jObject;

        public JsonVariantSerializer(string json)
        {
            _jObject = JsonConvert.DeserializeObject<JObject>(json);
        }

        public void Overwrite(ScriptableObject objectToOverwrite)
        {
            JsonUtility.FromJsonOverwrite(Serialize(), objectToOverwrite);
        }

        public void Set(SerializedProperty property)
        {
            property.serializedObject.ApplyModifiedProperties();
            var isArraySize = property.propertyType == SerializedPropertyType.ArraySize;
            var token = _jObject.SelectToken(GetJsonPropertyPath(property));
            if (token == null)
            {
                JToken newValue = isArraySize ? new JArray() : new JValue(property.GetBoxedValue());
                // also this will not work if user has a editor that does not use serialized properties
                var newToken = _jObject.AddTokenByPath(GetJsonPropertyPath(property), newValue);
                // TODO: on this line, add array values from prototype to this array
                if (isArraySize)
                    ((JArray)newToken).SetArraySize(property.intValue);
            }
            else
            {
                // TODO: Changing size of array should work
                if (isArraySize)
                {
                    ((JArray)token).SetArraySize(property.intValue);
                    return;
                }

                token.Replace(new JValue(property.GetBoxedValue()));
            }
        }

        public void Remove(SerializedProperty property)
        {
            try
            {
                var path = GetJsonPropertyPath(property);
                var token = _jObject.SelectToken(path);

                if (token.Parent is JArray)
                    token.Remove();
                else if (token.Parent is JProperty)
                    token.Parent.Remove();
                else
                {
                    Debug.Log($"Try to remove {property.propertyPath}. Token: {token?.GetType().Name ?? "null"}");
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public bool Contains(SerializedProperty property)
        {
            var path = GetJsonPropertyPath(property);
            var token = _jObject.SelectToken(path);
            if (token != null && property.propertyType == SerializedPropertyType.ArraySize)
            {
                if (!(token is JArray array))
                    throw new InvalidDataException($"{path} should be array");
                return array.Count != property.intValue;
            }

            return token != null;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(_jObject, Formatting.Indented);
        }

        private static string GetJsonPropertyPath(SerializedProperty prop)
        {
            return prop.propertyPath.Replace(".Array.data", "").Replace(".Array.size", "");
        }
    }
}