using System;
using UnityEngine;

namespace QuickEye.Samples.SingletonAssets
{
    public class DemoController : MonoBehaviour
    {
        private void Awake()
        {
            // PopupView class is a GameObject Singleton
            PopupView.Instance.SetMessage($"Hello {Environment.UserName}!");
            
            // Example usage of different types of singletons:
            var s1 = TypeWithoutAnAsset.Instance;
            var s2 = TypeWithOptionalAsset.Instance;
            var s3 = TypeWithMandatoryAsset.Instance;
            var s4 = ProjectSettingsAsset.Instance;
        }
    }
}