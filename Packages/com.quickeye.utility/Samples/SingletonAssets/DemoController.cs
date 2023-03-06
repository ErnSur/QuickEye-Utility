using System;
using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    public class DemoController : MonoBehaviour
    {
        private void Awake()
        {
            // PopupView class is a MonoBehaviour Singleton
            PopupView.Instance.SetMessage($"Hello {Environment.UserName}!");
            
            // Example usage of different types of ScriptableObjectSingletons:
            var s1 = SingletonWithoutAnAsset.Instance;
            var s2 = SingletonWithOptionalAsset.Instance;
            var s3 = SingletonWithMandatoryAsset.Instance;
            var s4 = ProjectSettingsAsset.Instance;
        }
    }
}