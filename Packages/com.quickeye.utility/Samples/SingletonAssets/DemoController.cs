using System;
using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    public class DemoController : MonoBehaviour
    {
        private void Awake()
        {
            PopupView.Instance.SetMessage($"Hello {Environment.UserName}!");
        }
    }
}