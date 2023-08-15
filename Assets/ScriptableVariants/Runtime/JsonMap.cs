using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QuickEye.Utility;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    public class JsonMap : ScriptableObject
    {
        [SerializeField]
        private UnityDictionary<string, string> jsonFields = new();

        [MenuItem("Test/De JSon")]
        public static void Dejson()
        {
            Debug.Log($"Hejoom");
            var json = Resources.Load<TextAsset>("Eve 1").text;
            var d = FromJson(json);
            Selection.activeObject = d;
        }

        public static JsonMap FromJson(string json)
        {
            var i = CreateInstance<JsonMap>();
            var jObject = JsonConvert.DeserializeObject<JObject>(json);
            //jObject.SelectToken("name").Replace(new JValue("sd"));
            AddFields(jObject, i.jsonFields);
            return i;
        }
        
        //TODO:
        // for what?
        // not trivial https://stackoverflow.com/questions/45740850/how-to-add-new-property-in-json-string-by-using-json-path
        /// <summary>
        /// Create JSON with proper nested properties based on <see cref="jsonFields"/>
        /// </summary>
        public static string ToJson()
        {
            var o = new JObject();
            
            return JsonConvert.SerializeObject(o);
        }

        private static void AddFields(JContainer jContainer, IDictionary<string, string> dic)
        {
            foreach (var token in jContainer)
            {
                if (token is JContainer container)
                    AddFields(container, dic);
                else
                    dic.Add(token.Path, token.ToString());
            }
        }
    }
}