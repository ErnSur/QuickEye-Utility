using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QuickEye.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public class OutsideClickTrigger : MonoBehaviour
    {
        public static bool IsFunctionalityDisabled { get; set; }

        public UnityEvent ClickedOutside;

        [SerializeField]
        private RectTransform[] _safeRects;

        [SerializeField]
        private Button[] _buttonsToTrigger;

        [SerializeField]
        private Toggle[] _togglesToTrigger;

        private ISubmitHandler[] _elementsToTrigger;

        private void Awake()
        {
            _elementsToTrigger = _buttonsToTrigger.Concat<ISubmitHandler>(_togglesToTrigger).ToArray();
        }

        private void Update()
        {
            TriggerIfClickedOutside();
        }

        private void TriggerIfClickedOutside()
        {
            var prevFlag = Input.simulateMouseWithTouches;
            Input.simulateMouseWithTouches = true;

            if (Input.GetMouseButtonDown(0) && !IsPointerInsideSafeRect(Input.mousePosition) && !IsFunctionalityDisabled)
            {
                TriggerSubmitHandlers();
                ClickedOutside.Invoke();
            }

            Input.simulateMouseWithTouches = prevFlag;
        }

        private void TriggerSubmitHandlers()
        {
            var eventData = new BaseEventData(EventSystem.current);
            foreach (var element in _elementsToTrigger)
                element.OnSubmit(eventData);
        }

        private bool IsPointerInsideSafeRect(Vector2 pointerPos)
        {
            return _safeRects.Any(rect => RectTransformUtility.RectangleContainsScreenPoint(rect, pointerPos));
        }
    }
}