using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    public static class ScriptableJsonUtility
    {
        public static void WriteOverrides(ScriptableObject variant, ScriptableObject prototype, string assetPath)
        {
            var resolver = new ScriptableJsonContractResolver(prototype);
            var json = JsonConvert.SerializeObject(
                variant,
                Formatting.Indented,
                new JsonSerializerSettings { ContractResolver = resolver }
            );

            File.WriteAllText(assetPath, json);
            AssetDatabase.ImportAsset(assetPath);
        }

        public static void RevertProperty(SerializedProperty property, ScriptableObject prototype, string assetPath)
        {
            var prototypeSerObj = new SerializedObject(prototype);
            var prototypeProperty = prototypeSerObj.FindProperty(property.propertyPath);
            //property.boxedValue = prototypeProperty.boxedValue;
            property.serializedObject.ApplyModifiedProperties();
            //var variant = (ScriptableObject)property.serializedObject.targetObject;
            //WriteOverrides(variant, prototype, assetPath);
        }

        public static bool IsPropertyOverriden(SerializedProperty property, string assetPath)
        {
            var json = File.ReadAllText(assetPath);
            var jObject = JsonConvert.DeserializeObject<JObject>(json);
            var token = jObject?.SelectToken(UnityPropertyPathToJsonPath(property.propertyPath));
            return token != null;
        }

        private static string UnityPropertyPathToJsonPath(string propertyPath)
        {
//        Debug.Log($"Path {propertyPath}");
            return propertyPath;
        }

        //Origin code by Squirrel.Downy(Flithor)
        public static JToken AddTokenByPath(this JToken jToken, string path, object value)
        {
            JToken result = null;
            // "a.b.d[1]['my1.2.4'][4].af['micor.a.ee.f'].ra[6]"
            var pathParts = Regex.Split(path, @"(?=\[)|(?=\[\.)|(?<=])(?>\.)")
                // > { "a.b.d", "[1]", "['my1.2.4']", "[4]", "af", "['micor.a.ee.f']", "ra", "[6]" }
                .SelectMany(str => str.StartsWith("[") ? new[] { str } : str.Split('.'))
                // > { "a", "b", "d", "[1]", "['my1.2.4']", "[4]", "af", "['micor.a.ee.f']", "ra", "[6]" }
                .ToArray();
            JToken node = jToken;
            for (int i = 0; i < pathParts.Length; i++)
            {
                var pathPart = pathParts[i];
                var partNode = node.SelectToken(pathPart);
                //node is null or token with null value
                if (partNode == null || partNode.Type == JTokenType.Null)
                {
                    if (i < pathParts.Length - 1)
                    {
                        //the next level is array or object
                        //accept [0], not ['prop']
                        JToken nextToken = Regex.IsMatch(pathParts[i + 1], @"\[\d+\]") ? new JArray() : new JObject();
                        SetToken(node, pathPart, nextToken);
                    }
                    else if (i == pathParts.Length - 1)
                    {
                        //JToken.FromObject(null) will throw a exception
                        result = value == null ? null : JToken.FromObject(value);
                        SetToken(node, pathPart, result);
                    }

                    partNode = node.SelectToken(pathPart);
                }

                node = partNode;
            }

            //set new token
            void SetToken(JToken node, string pathPart, JToken jToken)
            {
                if (node.Type == JTokenType.Object)
                {
                    //get real prop name (convert "['prop']" to "prop")
                    var name = pathPart.Trim('[', ']', '\'');
                    ((JObject)node).Add(name, jToken);
                }
                else if (node.Type == JTokenType.Array)
                {
                    //get real index (convert "[0]" to 0)
                    var index = int.Parse(pathPart.Trim('[', ']'));
                    var jArray = (JArray)node;
                    //if index is bigger than array length, fill the array
                    while (index >= jArray.Count)
                        jArray.Add(null);
                    //set token
                    jArray[index] = jToken;
                }
            }
            
            return result;
        }
    }
}