using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Button))]
    public class BackButtonTrigger : MonoBehaviour
    {
        public static string BackButtonInputKey { get; set; } = "Cancel";
        private static readonly List<Button> BackButtonsActiveOnScene = new List<Button>();
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            BackButtonsActiveOnScene.Insert(0, _button);
        }

        private void OnDisable()
        {
            BackButtonsActiveOnScene.Remove(_button);
        }

        private void Update()
        {
            ListenToBackButtonPress();
        }

        private static void ListenToBackButtonPress()
        {
            if (Input.GetButtonDown(BackButtonInputKey) && BackButtonsActiveOnScene.Count > 0)
                BackButtonsActiveOnScene[0].onClick.Invoke();
        }
    }
}