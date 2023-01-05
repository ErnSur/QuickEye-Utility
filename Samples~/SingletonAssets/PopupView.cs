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
    public class PopupView : SingletonMonoBehaviour<PopupView>
    {
        [SerializeField]
        private Text label;

        private void Start()
        {
            // we are using another singleton instance- "UIStyles"
            // `UIStyles` singleton is different from `PopupView` in that it is a `ScriptableObject` singleton
            // and `PopupView` is a `MonoBehaviour` singleton.
            label.color = UIStyles.Instance.popupTextColor;
        }

        public void SetMessage(string message)
        {
            label.text = message;
        }
    }
}