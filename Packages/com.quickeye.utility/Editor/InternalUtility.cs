using System;
using System.Reflection;
using UnityEditor;

namespace QuickEye.Utility.Editor
{
    public static class InternalUtility
    {
        private static readonly object[] _FrameObjectParameter = { -1 };

        public static void FrameObjectInProjectWindow(int instanceId)
        {
            try
            {
                var method = typeof(ProjectWindowUtil).GetMethod("FrameObjectInProjectWindow",
                    BindingFlags.Static | BindingFlags.NonPublic);
                _FrameObjectParameter[0] = instanceId;
                method?.Invoke(null, _FrameObjectParameter);
            }
            catch (Exception e)
            {
                throw new InternalActionException(e);
            }
        }
        
        public sealed class InternalActionException : Exception
        {
            public InternalActionException(Exception innerException) : base("Could not execute internal action",innerException)
            {
                
            }
        }
    }
}