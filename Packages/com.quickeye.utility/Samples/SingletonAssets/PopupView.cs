using OneAsset;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.Samples.SingletonAssets
{
    /// <summary>
    /// Adding <see cref="LoadFromAssetAttribute"/> to the <see cref="OneGameObject"/> will add a new behaviour.
    /// First call to the <see cref="PopupView.Instance"/> will instantiate a prefab from path used in attribute
    /// In this case it will instantiate a prefab that is located at: Resources/Popup View.prefab
    /// </summary>
    [LoadFromAsset("Resources/Popup View.prefab")]
    public class PopupView : OneGameObject<PopupView>
    {
        [SerializeField]
        private Text label;

        public void SetMessage(string message)
        {
            label.text = message;
        }
    }
}