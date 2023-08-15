using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    internal struct CustomChangeCheck
    {
        private static readonly Stack<bool> _ChangedStack = new Stack<bool>();

        /// <summary>
        ///   <para>Starts a new code block to check for GUI changes.</para>
        /// </summary>
        public static void BeginChangeCheck()
        {
            _ChangedStack.Push(GUI.changed);
            GUI.changed = false;
        }

        /// <summary>
        ///   <para>Ends a code block and checks for any GUI changes.</para>
        /// </summary>
        /// <returns>
        ///   <para>Returns true if GUI state changed since the call to EditorGUI.BeginChangeCheck, otherwise false.</para>
        /// </returns>
        public static bool EndChangeCheck()
        {
            if (_ChangedStack.Count == 0)
            {
                GUI.changed = true;
                return true;
            }
            var changed = GUI.changed;
            GUI.changed |= _ChangedStack.Pop();
            return changed;
        }
    }
}