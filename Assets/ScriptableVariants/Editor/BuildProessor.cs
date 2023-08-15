using System.Linq;
using UnityEngine;



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Object = UnityEngine.Object;

public class BuildProessor : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;
    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log($"Hejoo");
        foreach (var packedAsset in report.packedAssets)
        {
            foreach (var content in packedAsset.contents)
            {
                Debug.Log($"Ass: {content.sourceAssetPath}");
            }
        }
    }
}

public static class BuildDatabase
{
    [MenuItem("Test/Find Build People")]
    public static void NAME()
    {
        var buildPeople = GetAssetsIncludedInBuild<Person>().ToArray();
        foreach (var person in buildPeople)
        {
            Debug.Log($"Build per {person.name}");
        }

        Debug.Log($"Build people {buildPeople.Length}");
    }
    public static IEnumerable<Object> GetAssetsIncludedInBuild(Type assetType)
    {
        var assetsInScene = from scene in EditorBuildSettings.scenes
            from assetPath in AssetDatabase.GetDependencies(scene.path)
            let asset = AssetDatabase.LoadAssetAtPath(assetPath, assetType)
            where asset != null
            select asset;

        var assetsFromResources = Resources.LoadAll("", assetType);

        var previousFilterType = Debug.unityLogger.filterLogType;
        Debug.unityLogger.filterLogType = LogType.Error;
        var assetsFromStreamingAssets =
            from guid in AssetDatabase.FindAssets($"t:{assetType.Name}", new[] {"Assets/StreamingAssets"})
            let path = AssetDatabase.GUIDToAssetPath(guid)
            let asset = AssetDatabase.LoadAssetAtPath(path, assetType)
            where asset != null
            select asset;
        Debug.unityLogger.filterLogType = previousFilterType;

        return assetsInScene
            .Union(assetsFromResources)
            .Union(assetsFromStreamingAssets)
            .Where(t => !IsEditorAsset(t));
    }

    public static IEnumerable<T> GetAssetsIncludedInBuild<T>() where T : Object
    {
        return GetAssetsIncludedInBuild(typeof(T)).Cast<T>();
    }

    public static bool IsEditorAsset(Object asset)
    {
        return Regex.IsMatch(AssetDatabase.GetAssetPath(asset), @"\/[Ee]ditor\/");
    }
}