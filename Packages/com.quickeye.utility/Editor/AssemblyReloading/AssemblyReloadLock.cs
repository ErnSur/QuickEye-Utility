using UnityEditor;

namespace QuickEye.Utility.Editor.AssemblyReloading
{
    internal static class AssemblyReloadLock
    {
        private static bool _isLocked;

        public static bool IsLocked
        {
            get => _isLocked;
            set
            {
                if (value == _isLocked)
                    return;
                if (value)
                    Enable();
                else
                    Disable();
                _isLocked = value;
            }
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