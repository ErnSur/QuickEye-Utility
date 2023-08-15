using System;
using System.Globalization;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    internal static class SerializedPropertyExtensions
    {
        public static object GetBoxedValue(this SerializedProperty property)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Enum: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.Integer: return property.longValue;
                case SerializedPropertyType.Boolean: return property.boolValue;
                case SerializedPropertyType.Float: return property.doubleValue;
                case SerializedPropertyType.String: return property.stringValue;
                case SerializedPropertyType.Color: return property.colorValue;
                case SerializedPropertyType.ObjectReference: return property.objectReferenceValue;
                case SerializedPropertyType.LayerMask: return property.intValue;
                case SerializedPropertyType.Vector2: return property.vector2Value;
                case SerializedPropertyType.Vector3: return property.vector3Value;
                case SerializedPropertyType.Vector4: return property.vector4Value;
                case SerializedPropertyType.Rect: return property.rectValue;
                case SerializedPropertyType.ArraySize: return property.intValue;
                case SerializedPropertyType.Character: return property.intValue;
                case SerializedPropertyType.AnimationCurve: return property.animationCurveValue;
                case SerializedPropertyType.Bounds: return property.boundsValue;
                case SerializedPropertyType.Gradient: return property.colorValue;
                case SerializedPropertyType.Quaternion: return property.quaternionValue;
                case SerializedPropertyType.ExposedReference: return property.exposedReferenceValue;
                case SerializedPropertyType.FixedBufferSize: return property.intValue;
                case SerializedPropertyType.Vector2Int: return property.vector2IntValue;
                case SerializedPropertyType.Vector3Int: return property.vector3IntValue;
                case SerializedPropertyType.RectInt: return property.rectIntValue;
                case SerializedPropertyType.BoundsInt: return property.boundsIntValue;
                case SerializedPropertyType.ManagedReference: return property.managedReferenceValue;
                case SerializedPropertyType.Hash128: return property.hash128Value;

                default:
                    throw new NotSupportedException(string.Format(
                        "The boxedValue property is not supported on \"{0}\" because it has an unsupported propertyType {1}.",
                        property.propertyPath, property.propertyType));
            }
        }
        public static void SetValue(this SerializedProperty property, string value)
        {
            
            switch (property.propertyType)
            {
                case SerializedPropertyType.Enum: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.LayerMask: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.ArraySize: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.Character: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.FixedBufferSize: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.Gradient: goto case SerializedPropertyType.Color;
                    
                case SerializedPropertyType.Integer: property.longValue= long.Parse(value);break;
                case SerializedPropertyType.Boolean: property.boolValue= bool.Parse(value);break;
                case SerializedPropertyType.Float: property.doubleValue= double.Parse(value);break;
                case SerializedPropertyType.String: property.stringValue= value;break;
                case SerializedPropertyType.Color: property.colorValue= JsonConvert.DeserializeObject<Color>(value);break;
                case SerializedPropertyType.Vector2: property.vector2Value= JsonConvert.DeserializeObject<Vector2>(value);break;
                case SerializedPropertyType.Vector3: property.vector3Value= JsonConvert.DeserializeObject<Vector3>(value);break;
                case SerializedPropertyType.Vector4: property.vector4Value= JsonConvert.DeserializeObject<Vector4>(value);break;
                case SerializedPropertyType.Rect: property.rectValue= JsonConvert.DeserializeObject<Rect>(value);break;
                case SerializedPropertyType.AnimationCurve: property.animationCurveValue= JsonConvert.DeserializeObject<AnimationCurve>(value);break;
                case SerializedPropertyType.Bounds: property.boundsValue= JsonConvert.DeserializeObject<Bounds>(value);break;
                case SerializedPropertyType.Quaternion: property.quaternionValue= JsonConvert.DeserializeObject<Quaternion>(value);break;
                case SerializedPropertyType.Vector2Int: property.vector2IntValue= JsonConvert.DeserializeObject<Vector2Int>(value);break;
                case SerializedPropertyType.Vector3Int: property.vector3IntValue= JsonConvert.DeserializeObject<Vector3Int>(value);break;
                case SerializedPropertyType.RectInt: property.rectIntValue= JsonConvert.DeserializeObject<RectInt>(value);break;
                case SerializedPropertyType.BoundsInt: property.boundsIntValue= JsonConvert.DeserializeObject<BoundsInt>(value);break;
                case SerializedPropertyType.Hash128: property.hash128Value= JsonConvert.DeserializeObject<Hash128>(value);break;
                case SerializedPropertyType.ObjectReference: property.objectReferenceValue= JsonConvert.DeserializeObject<ObjRef>(value).GetAsset();break;
                //case SerializedPropertyType.ExposedReference: property.exposedReferenceValue= exposedReference.Parse(value);break;
                //case SerializedPropertyType.ManagedReference: property.managedReferenceValue= managedReference.Parse(value);break;

                default:
                    throw new NotSupportedException(string.Format(
                        "The boxedValue property is not supported on \"{0}\" because it has an unsupported propertyType {1}.",
                        property.propertyPath, property.propertyType));
            }
        }
        
         public static string GetSerializableValue(this SerializedProperty property)
        {
            
            switch (property.propertyType)
            {
                case SerializedPropertyType.Enum: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.LayerMask: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.ArraySize: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.Character: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.FixedBufferSize: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.Gradient: goto case SerializedPropertyType.Color;
                    
                case SerializedPropertyType.Integer: return property.longValue.ToString();
                case SerializedPropertyType.Boolean:return property.boolValue.ToString();
                case SerializedPropertyType.Float:return property.doubleValue.ToString(CultureInfo.InvariantCulture);
                case SerializedPropertyType.String: return property.stringValue;
                case SerializedPropertyType.Color:return  JsonConvert.SerializeObject(property.colorValue);
                case SerializedPropertyType.Vector2:return  JsonConvert.SerializeObject(property.vector2Value);
                case SerializedPropertyType.Vector3:return  JsonConvert.SerializeObject(property.vector3Value);
                case SerializedPropertyType.Vector4:return  JsonConvert.SerializeObject(property.vector4Value);
                case SerializedPropertyType.Rect:return  JsonConvert.SerializeObject(property.rectValue);
                case SerializedPropertyType.AnimationCurve:return  JsonConvert.SerializeObject(property.animationCurveValue);
                case SerializedPropertyType.Bounds:return  JsonConvert.SerializeObject(property.boundsValue);
                case SerializedPropertyType.Quaternion:return  JsonConvert.SerializeObject(property.quaternionValue);
                case SerializedPropertyType.Vector2Int:return  JsonConvert.SerializeObject(property.vector2IntValue);
                case SerializedPropertyType.Vector3Int:return  JsonConvert.SerializeObject(property.vector3IntValue);
                case SerializedPropertyType.RectInt:return  JsonConvert.SerializeObject(property.rectIntValue);
                case SerializedPropertyType.BoundsInt:return  JsonConvert.SerializeObject(property.boundsIntValue);
                case SerializedPropertyType.Hash128:return  JsonConvert.SerializeObject(property.hash128Value);
                case SerializedPropertyType.ObjectReference: return JsonConvert.SerializeObject(new ObjRef(property.objectReferenceValue));
                //case SerializedPropertyType.ExposedReference: property.exposedReferenceValue= exposedReference.Parse(value);break;
                //case SerializedPropertyType.ManagedReference: property.managedReferenceValue= managedReference.Parse(value);break;

                default:
                    throw new NotSupportedException(string.Format(
                        "The boxedValue property is not supported on \"{0}\" because it has an unsupported propertyType {1}.",
                        property.propertyPath, property.propertyType));
            }
        }
        
        public static void PasteValueTo(this SerializedProperty property, SerializedProperty target)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Enum: goto case SerializedPropertyType.Integer;
                case SerializedPropertyType.Integer: target.longValue = property.longValue;break;
                case SerializedPropertyType.Boolean: target.boolValue = property.boolValue;break;
                case SerializedPropertyType.Float: target.doubleValue = property.doubleValue;break;
                case SerializedPropertyType.String: target.stringValue = property.stringValue;break;
                case SerializedPropertyType.Color: target.colorValue = property.colorValue;break;
                case SerializedPropertyType.ObjectReference: target.objectReferenceValue = property.objectReferenceValue;break;
                case SerializedPropertyType.LayerMask: target.intValue = property.intValue;break;
                case SerializedPropertyType.Vector2: target.vector2Value = property.vector2Value;break;
                case SerializedPropertyType.Vector3: target.vector3Value = property.vector3Value;break;
                case SerializedPropertyType.Vector4: target.vector4Value = property.vector4Value;break;
                case SerializedPropertyType.Rect: target.rectValue = property.rectValue;break;
                case SerializedPropertyType.ArraySize: target.intValue = property.intValue;break;
                case SerializedPropertyType.Character: target.intValue = property.intValue;break;
                case SerializedPropertyType.AnimationCurve: target.animationCurveValue = property.animationCurveValue;break;
                case SerializedPropertyType.Bounds: target.boundsValue = property.boundsValue;break;
                case SerializedPropertyType.Gradient: target.colorValue = property.colorValue;break;
                case SerializedPropertyType.Quaternion: target.quaternionValue = property.quaternionValue;break;
                case SerializedPropertyType.ExposedReference: target.exposedReferenceValue = property.exposedReferenceValue;break;
                case SerializedPropertyType.FixedBufferSize: target.intValue = property.intValue;break;
                case SerializedPropertyType.Vector2Int: target.vector2IntValue = property.vector2IntValue;break;
                case SerializedPropertyType.Vector3Int: target.vector3IntValue = property.vector3IntValue;break;
                case SerializedPropertyType.RectInt: target.rectIntValue = property.rectIntValue;break;
                case SerializedPropertyType.BoundsInt: target.boundsIntValue = property.boundsIntValue;break;
                case SerializedPropertyType.ManagedReference: target.managedReferenceValue = property.managedReferenceValue;break;
                case SerializedPropertyType.Hash128: target.hash128Value = property.hash128Value;break;

                default:
                    throw new NotSupportedException(string.Format(
                        "The boxedValue property is not supported on \"{0}\" because it has an unsupported propertyType {1}.",
                        property.propertyPath, property.propertyType));
            }

            target.serializedObject.ApplyModifiedProperties();
        }
    }
}