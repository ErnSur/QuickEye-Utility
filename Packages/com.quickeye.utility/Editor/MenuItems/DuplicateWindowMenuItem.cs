using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    public static class DuplicateWindowMenuItem
    {
        [MenuItem("Window/Duplicate Window %#D", priority = 10)]
        public static void DuplicateWindow()
        {
            var window = EditorWindow.focusedWindow;
            var newWindow = UnityEngine.Object.Instantiate(window);

            TryLockWindow(newWindow);

            newWindow.Show();
            newWindow.position = GetRectNextTo(window.position);
        }

        private static void TryLockWindow(EditorWindow newWindow)
        {
            var windowType = newWindow.GetType();
            if (EditorWindowLocker.TryGetWindowLocker(windowType, out var locker))
                locker.LockWindow(newWindow, true);
        }

        private static Rect GetRectNextTo(Rect rect)
        {
            return new Rect
            {
                x = Mathf.Max(rect.position.x - rect.size.x, 0),
                y = rect.position.y,
                size = rect.size
            };
        }
    }

    internal class EditorWindowLocker
    {
        private const string LockTrackerLockedPropertyName = "isLocked";

        private static readonly Type _LockerType = typeof(EditorGUIUtility).Assembly.GetTypes()
            .FirstOrDefault(t => t.Name == "EditorLockTracker");

        private static readonly Dictionary<Type, EditorWindowLocker> _CachedLockers =
            new Dictionary<Type, EditorWindowLocker>();

        public static bool TryGetWindowLocker(Type windowType, out EditorWindowLocker locker)
        {
            if (_CachedLockers.TryGetValue(windowType, out locker))
                return locker != null;

            if (typeof(EditorWindow).IsAssignableFrom(windowType) && GetLockTrackerField(windowType) != null)
                locker = new EditorWindowLocker(windowType);

            _CachedLockers[windowType] = locker;
            return locker != null;
        }

        // SceneHierarchyWindow needs special handling, it's not worth it
        private static FieldInfo GetLockTrackerField(Type windowType)
        {
            var fields = windowType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var lockerField = fields.FirstOrDefault(f => _LockerType.IsAssignableFrom(f.FieldType));
            return lockerField;
        }

        private readonly FieldInfo _lockTrackerField;
        private readonly PropertyInfo _isLockedProperty;

        private EditorWindowLocker(Type windowType)
        {
            _lockTrackerField = GetLockTrackerField(windowType);
            _isLockedProperty = _lockTrackerField.FieldType.GetProperty(LockTrackerLockedPropertyName,
                BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public void LockWindow(EditorWindow window, bool isLocked)
        {
            try
            {
                _isLockedProperty.SetValue(_lockTrackerField.GetValue(window), isLocked);
            }
            catch
            {
            }
        }
    }
}