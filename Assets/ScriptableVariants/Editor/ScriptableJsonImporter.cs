using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;
using UnityEditor;

namespace QuickEye.ScriptableObjectVariants
{
    [ScriptedImporter(1, "sjson")]
    public class ScriptableJsonImporter : ScriptedImporter
    {
        public ScriptableObject prototype;

        /// <summary>
        /// Main asset = Same SO as `prototype` with applied overrides from sjson file.
        /// Secondary asset "Map" = For now just for debugging?
        /// or maybe the intention was to hold references to UnityEngine.Object so that they are included in build?
        /// but doesn't main asset already do that by creating new scriptableObject with those references?
        /// </summary>
        /// <param name="ctx"></param>
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var fileContent = File.ReadAllText(ctx.assetPath);
            if (string.IsNullOrEmpty(fileContent) || prototype == null)
                return;
            var so = Instantiate(prototype);
            ctx.DependsOnSourceAsset(AssetDatabase.GetAssetPath(prototype));
            IVariantSerializer serializer = new DicVariantSerializer(fileContent);
            serializer.Overwrite(so);
            
            var map = JsonMap.FromJson(fileContent);
            ctx.AddObjectToAsset("map", map);
            ctx.AddObjectToAsset("main", so);
            ctx.SetMainObject(so);
        }
    }
}