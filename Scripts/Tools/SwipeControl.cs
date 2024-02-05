using System;
using UnityEngine;

namespace BugiGames.Tools
{
    public class SwipeControl : MonoBehaviour
    {
        [SerializeField] private float swipeThresholdPercentage = 20f;

        private Vector2 swipeInitialPosition;

        private bool isMobile;

        public event Action OnSwipeDown;
        public event Action OnSwipeUp;
        public event Action OnSwipeLeft;
        public event Action OnSwipeRight;

        private void Awake()
        {
            isMobile = Application.isMobilePlatform;
        }

        private void Update()
        {
            if (isMobile)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            swipeInitialPosition = touch.position;
                            break;
                        case TouchPhase.Ended:
                            Calculate(touch.position);
                            break;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    swipeInitialPosition = Input.mousePosition;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Calculate(Input.mousePosition);
                }
            }
        }

        private void Calculate(Vector3 finalPosition)
        {
            float screenPercentage = Mathf.Min(Screen.width, Screen.height) * swipeThresholdPercentage / 100f;

            float distance_X = Mathf.Abs(swipeInitialPosition.x - finalPosition.x);
            float distance_Y = Mathf.Abs(swipeInitialPosition.y - finalPosition.y);

            Debug.Log($"distance_X : {distance_X}, distance_Y : {distance_Y}, screenPercentage : {screenPercentage},");

            if (distance_X > screenPercentage || distance_Y > screenPercentage)
            {
                if (distance_X > distance_Y)
                {
                    if (swipeInitialPosition.x > finalPosition.x)
                    {
                        OnSwipeLeft?.Invoke();
                        Debug.Log("Swipe Left");
                    }
                    else
                    {
                        OnSwipeRight?.Invoke();
                        Debug.Log("Swipe Right");
                    }
                }
                else
                {
                    if (swipeInitialPosition.y > finalPosition.y)
                    {
                        OnSwipeDown?.Invoke();
                        Debug.Log("Swipe Down");
                    }
                    else
                    {
                        OnSwipeUp?.Invoke();
                        Debug.Log("Swipe Up");
                    }
                }
            }
        }

    }
}
