using UnityEngine;

namespace QuickEye.Utility
{
    public abstract class GameEventBase : ScriptableObject
    {
#if UNITY_EDITOR
        [TextArea]
        [SerializeField]
        string _developerDescription;
#endif
        public bool WasRaised;
    }
}