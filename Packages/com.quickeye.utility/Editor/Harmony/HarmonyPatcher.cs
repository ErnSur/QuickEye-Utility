using HarmonyLib;
using UnityEditor;

namespace QuickEye.Utility.Editor
{

    public static class HarmonyPatcher
    {
        public static Harmony Harmony;

        [InitializeOnLoadMethod]
        public static void DoPatching()
        {
            Harmony = new Harmony("com.quickeye.utility");
        }
    }
}