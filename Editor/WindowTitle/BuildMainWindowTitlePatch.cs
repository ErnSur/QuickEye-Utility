using HarmonyLib;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor.WindowTitle
{
    [HarmonyPatch(typeof(EditorApplication), "BuildMainWindowTitle")]
    internal class BuildMainWindowTitlePatch
    {
        private static void Postfix(ref string __result)
        {
            if (WindowTitleSettings.EnableCustomTitle)
            {
                __result = WindowTitleSettings.WindowTitle;
            }
        }
    }
}