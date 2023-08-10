using System;
using System.IO;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace OneAsset
{
    public static partial class OneAssetLoader
    {
        private static bool TryLoadFromAssetDatabase(Type type, AssetPath path, out Object obj)
        {
#if UNITY_EDITOR
            var pathWithExtensions = path;
            if (!path.EndsWith(".asset"))
                pathWithExtensions = $"{path}.asset";
            obj = UnityEditor.AssetDatabase.LoadAssetAtPath(pathWithExtensions, type);
            return obj != null;
#else
            obj = null;
            return false;
#endif
        }

        private static bool TryLoadAndForget(Type type, AssetPath path, out Object obj)
        {
#if UNITY_EDITOR
            if (!File.Exists(path))
            {
                obj = null;
                return false;
            }
            
            // Ideally this code would be in editor assembly. But when this method is called from InitializeOnLoad
            // there is no guarantee that editor callback will be registered like with `CreateAssetAction`
            obj = UnityEditorInternal.InternalEditorUtility
                .LoadSerializedFileAndForget(path)
                .FirstOrDefault(o => o.GetType() == type);

            return obj != null;
#else
            obj = null;
            return false;
#endif
        }
    }
}