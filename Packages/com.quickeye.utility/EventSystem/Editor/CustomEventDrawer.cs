using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace QuickEye.EventSystem.Editor
{
    //[CustomPropertyDrawer(typeof(UnityEvent<>),true)]
    public class CustomEventDrawer : UnityEditorInternal.UnityEventDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var ve =base.CreatePropertyGUI(property);
            var label = ve.Q<Label>();
            var argName = TrimUnityEventName(GetFriendlyName(fieldInfo.FieldType));
            label.text = $"{ObjectNames.NicifyVariableName(fieldInfo.Name)} ( {ObjectNames.NicifyVariableName(argName)} )";
            return ve;
        }

        static string TrimUnityEventName(string typeName)
        {
            const string typeNameStart = "UnityEvent<";
            var startIndex = typeName.IndexOf(typeNameStart,StringComparison.InvariantCulture) + typeNameStart.Length;
            return typeName.Substring(startIndex, typeName.Length-(startIndex+1));
        }
        
        static string GetFriendlyName(Type type)
        {
            string friendlyName = type.Name;
            if (type.IsGenericType)
            {
                int iBacktick = friendlyName.IndexOf('`');
                if (iBacktick > 0)
                {
                    friendlyName = friendlyName.Remove(iBacktick);
                }
                friendlyName += "<";
                Type[] typeParameters = type.GetGenericArguments();
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    string typeParamName = GetFriendlyName(typeParameters[i]);
                    friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
                }
                friendlyName += ">";
            }

            return friendlyName;
        }
    }
}