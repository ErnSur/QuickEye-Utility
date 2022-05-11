using HarmonyLib;
using UnityEditor;

namespace QuickEye.Utility.Editor
{
    public static class HarmonyPatcher
    {
        [InitializeOnLoadMethod]
        public static void DoPatching()
        {
            var harmony = new Harmony("com.example.patch");
            harmony.PatchAll();
        }
    }
}