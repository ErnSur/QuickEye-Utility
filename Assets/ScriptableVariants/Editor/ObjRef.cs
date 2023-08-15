using System;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    [Serializable]
    internal struct ObjRef
    {
        public string guid;
        public long fileId;
        public UnityEngine.Object GetAsset() => AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(guid));

        public ObjRef(string guid, long fileId)
        {
            this.guid = guid;
            this.fileId = fileId;
        }

        public ObjRef(UnityEngine.Object obj)
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out guid, out fileId);
        }
        public static implicit operator string(ObjRef obj) => JsonUtility.ToJson(obj);
    }
}