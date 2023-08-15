using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace QuickEye.ScriptableObjectVariants
{
    public class ScriptableJsonContractResolver : DefaultContractResolver
    {
        public readonly object Prototype;
        public ScriptableJsonContractResolver(object prototype)
        {
            Prototype = prototype;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var fieldInfo = member as FieldInfo;
            if (fieldInfo == null)
                return property;
            // No, overrides can have the same values as prototype but still be overries and should be serialized
            property.ShouldSerialize = instance =>
            {
                var variantValue = fieldInfo.GetValue(instance);
                var prototypeValue = fieldInfo.GetValue(Prototype);
                return variantValue != prototypeValue;
            };

            return property;
        }
    }
}