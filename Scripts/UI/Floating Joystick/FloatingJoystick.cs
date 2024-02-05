using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace BugiGames.Main
{
    public class FloatingJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, ISelfReset
    {
        [SerializeField] private RectTransform Background;
        [SerializeField] private RectTransform Handle;
        [SerializeField, Range(0, 2f)] private float handleLimit = 1f;
        [SerializeField, Range(0, 8f)] private float joyDirectionLimit = 2f;

        public float Vertical => input.y;
        public float Horizontal => input.x;
        public float RotationVertical => playerRotation.y;
        public float RotationHorizontal => playerRotation.x;

        public bool IsJoystickMoved { get; private set; }

        private Vector2 playerRotation;
        private Vector2 input = Vector2.zero;
        private Vector2 JoyPosition = Vector2.zero;

        public event Action<float> OnJoystickPositionChanged;
        public event Action OnJoystickLeave;

        public void OnPointerDown(PointerEventData eventData)
        {
            Background.gameObject.SetActive(true);
            OnDrag(eventData);
            JoyPosition = eventData.position;

            Vector2 mousePosition = eventData.position;
            Vector2 canvasPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(Background.parent as RectTransform,
                                                                    mousePosition, eventData.pressEventCamera,
                                                                    out canvasPosition);

            Background.transform.localPosition = canvasPosition;

            Handle.anchoredPosition = Vector2.zero;
            input = Vector2.zero;
            IsJoystickMoved = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 JoyDirection = eventData.position - JoyPosition;

            input = (JoyDirection.magnitude > Background.sizeDelta.x / joyDirectionLimit) ?
                     JoyDirection.normalized : JoyDirection / (Background.sizeDelta.x / joyDirectionLimit);

            Handle.anchoredPosition = (input * Background.sizeDelta.x / joyDirectionLimit) * handleLimit;

            playerRotation = (JoyDirection.magnitude > Background.sizeDelta.x / joyDirectionLimit) ?
                              JoyDirection.normalized : JoyDirection / (Background.sizeDelta.x / joyDirectionLimit);

            IsJoystickMoved = true;
            OnJoystickPositionChanged?.Invoke(HandleDistanceNormalized);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            Handle.anchoredPosition = Vector2.zero;
            IsJoystickMoved = false;
            OnJoystickLeave?.Invoke();
            Background.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            input = Vector2.zero;
            Handle.anchoredPosition = Vector2.zero;
            IsJoystickMoved = false;
        }

        public float LargestSide
        {
            get
            {
                if (Mathf.Abs(Handle.anchoredPosition.x) > Mathf.Abs(Handle.anchoredPosition.y))
                {
                    return Mathf.Abs(Handle.anchoredPosition.x);
                }
                else
                {
                    return Mathf.Abs(Handle.anchoredPosition.y);
                }
            }
        }

        public float HandleDistanceNormalized
        {
            get
            {
                float distance = Vector2.Distance(Handle.anchoredPosition, Vector2.zero);
                float maxDistance = (Background.sizeDelta.x * handleLimit) / joyDirectionLimit;
                return Mathf.Clamp01(distance / maxDistance);
            }
        }

        public void SelfReset()
        {
            input = Vector2.zero;
            Handle.anchoredPosition = Vector2.zero;
            IsJoystickMoved = false;
            Background.gameObject.SetActive(false);
        }
    }
}
