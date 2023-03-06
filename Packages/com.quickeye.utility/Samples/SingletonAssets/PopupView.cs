using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    /// <summary>
    /// Adding `SingletonAsset` attribute to the singleton class will add a new behaviour.
    /// First call to the `PopupView.Instance` will instantiate a prefab from path used in attribute (relative to any folder called "Resources")
    /// In this case it will instantiate a prefab that is located at: Resources/Popup View.prefab
    /// </summary>
    [SingletonAsset("Popup View")]
    public class PopupView : SingletonMonoBehaviour<PopupView>
    {
        [SerializeField]
        private Text label;

        public void SetMessage(string message)
        {
            label.text = message;
        }
    }
}