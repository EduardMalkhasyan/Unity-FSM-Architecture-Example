using BugiGames.ScriptableObject;
using System;
using UnityEngine;
using Zenject;

namespace BugiGames.Main
{
    public class PlayerMovementWithJoystick : MonoBehaviour
    {
        [Inject] private FloatingJoystick floatingJoystick;

        private float vertical;
        private float horizontal;
        private float verticalRotation;
        private float horizontalRotation;
        private float speed;

        private Vector3 playerRotation;

        private bool isOnMovement = true;

        public event Action OnMovementStart;
        public event Action OnMovementFinish;

        private void FixedUpdate()
        {
#if UNITY_EDITOR
            if (floatingJoystick.IsJoystickMoved && (Input.touchCount == 1 || Input.GetMouseButton(0)))
#else
            if (floatingJoystick.IsJoystickMoved && Input.touchCount == 1)
#endif
            {
                if (isOnMovement)
                {
                    OnMovementStart?.Invoke();
                    isOnMovement = false;
                }

                vertical = floatingJoystick.Vertical;
                horizontal = floatingJoystick.Horizontal;

                transform.position += new Vector3(horizontal, 0f, vertical) * Time.fixedDeltaTime * speed;

                verticalRotation = floatingJoystick.RotationVertical;
                horizontalRotation = floatingJoystick.RotationHorizontal;

                playerRotation = new Vector3(horizontalRotation, 0, verticalRotation);

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(playerRotation),
                                                     PlayerSettings.ImmutableValue.RotationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                if (!isOnMovement)
                {
                    OnMovementFinish?.Invoke();
                    isOnMovement = true;
                }
            }
        }

        public void StopSpeed()
        {
            speed = 0;
        }

        public void EnableSpeed()
        {
            speed = PlayerSettings.ImmutableValue.CurrentStageSpeed;
        }
    }
}
