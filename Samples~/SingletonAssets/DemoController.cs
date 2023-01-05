using System;
using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    public class DemoController : MonoBehaviour
    {
        [SerializeField]
        private UIStyles uiStyles;
        
        private void Awake()
        {
            uiStyles = UIStyles.Instance;
            PopupView.Instance.SetMessage($"Hello {Environment.UserName}!");
        }
    }
}