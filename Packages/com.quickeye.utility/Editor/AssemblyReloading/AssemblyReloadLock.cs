using System.Reflection;
using UnityEditor;

namespace QuickEye.Utility.Editor.AssemblyReloading
{
    internal static class AssemblyReloadLock
    {
        private static readonly MethodInfo CanReloadAssembliesMethod = typeof(EditorApplication).GetMethod(
            "CanReloadAssemblies",
            BindingFlags.Static | BindingFlags.NonPublic);

        public static bool IsLocked
        {
            get
            {
                var result = CanReloadAssembliesMethod?.Invoke(null, null);
                if (result is bool canReload)
                    return !canReload;
                return false;
            }
        }

        public static void SetActive(bool lockAssemblyReload)
        {
            if (lockAssemblyReload)
                Enable();
            else
                Disable();
        }

        private static void Enable()
        {
            if (IsLocked)
                return;
            EditorApplication.LockReloadAssemblies();
        }

        private static void Disable()
        {
            if (!IsLocked)
                return;
            EditorApplication.UnlockReloadAssemblies();
            EditorUtility.RequestScriptReload();
        }
    }
}