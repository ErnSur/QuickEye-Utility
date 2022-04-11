using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    /// <summary>
    /// If you add `SingletonAsset` the singleton instance will instantiate a prefab from relevant resource path
    /// In this case it will instantiate a prefab under: Resources/Popup View
    /// </summary>
    [SingletonAsset("Popup View")]
    public class PopupView : Singleton<PopupView>
    {
        [SerializeField]
        private Text label;

        private void Start()
        {
            label.color = GlobalSettings.Instance.popupTextColor;
        }

        public void SetMessage(string message)
        {
            label.text = message;
        }
    }
}