using BugiGames.ScriptableObject;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;
using static UnityEngine.Rendering.DebugUI;

namespace BugiGames.Main
{
    public class Player : MonoBehaviour, ISelfReset
    {
        [Inject] private FloatingJoystick floatingJoystick;

        [SerializeField] private AvatarHolder avatarHolder;
        [field: SerializeField] public FoodStack FoodStack { get; private set; }
        [SerializeField] private PlayerMovementWithJoystick playerMovementWithJoystick;
        [SerializeField] private PlayerCollisionAsBoundDetector playerCollisionAsBoundDetector;

        [SerializeField] private Rigidbody selfRigidBody;
        [SerializeField] private Collider selfCollider;

        private Vector3 startPosition;

        private void Awake()
        {
            startPosition = transform.localPosition;
            floatingJoystick.OnJoystickPositionChanged += OnJoystickPositionChanged;
            floatingJoystick.OnJoystickLeave += OnJoystickLeave;
        }

        private void OnDestroy()
        {
            floatingJoystick.OnJoystickPositionChanged -= OnJoystickPositionChanged;
            floatingJoystick.OnJoystickLeave -= OnJoystickLeave;
        }

        private void OnJoystickLeave()
        {
            avatarHolder.currentAvatarTuple.body.SetAnimatorSpeed(1);
        }

        private void OnJoystickPositionChanged(float value)
        {
            avatarHolder.currentAvatarTuple.body.SetAnimatorSpeed(value);
        }

        private void AddMovementSubscription()
        {
            playerMovementWithJoystick.OnMovementStart += Run;
            playerMovementWithJoystick.OnMovementFinish += Idle;
            playerCollisionAsBoundDetector.OnDirectCollisionEnter += StopFromWallCollision;
            playerCollisionAsBoundDetector.OnDirectCollisionExit += TryMoveFromMovementJoystick;

            FoodStack.OnStackEmptied += TryMoveFromMovementJoystick;
            FoodStack.OnStackFilled += TryMoveFromMovementJoystick;
        }

        public void CancelMovementSubscription(Action callBack = null)
        {
            playerMovementWithJoystick.OnMovementStart -= Run;
            playerMovementWithJoystick.OnMovementFinish -= Idle;
            playerCollisionAsBoundDetector.OnDirectCollisionEnter += StopFromWallCollision;
            playerCollisionAsBoundDetector.OnDirectCollisionExit += TryMoveFromMovementJoystick;

            FoodStack.OnStackEmptied -= TryMoveFromMovementJoystick;
            FoodStack.OnStackFilled -= TryMoveFromMovementJoystick;

            callBack?.Invoke();
        }

        [Button]
        public void Idle()
        {
            if (FoodStack.StackIsEmpty())
            {
                avatarHolder.currentAvatarTuple.body.Idle();
            }
            else
            {
                avatarHolder.currentAvatarTuple.body.IdleWithItem();
            }
        }

        [Button]
        public void Run()
        {
            if (FoodStack.StackIsEmpty())
            {
                avatarHolder.currentAvatarTuple.body.Run();
            }
            else
            {
                avatarHolder.currentAvatarTuple.body.RunWithItem();
            }

            playerMovementWithJoystick.EnableSpeed();
        }

        private void TryMoveFromMovementJoystick()
        {
            if (floatingJoystick.IsJoystickMoved)
            {
                Run();
            }
            else
            {
                Idle();
            }
        }

        private void StopFromWallCollision()
        {
            playerMovementWithJoystick.StopSpeed();
            Idle();
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Setup()
        {
            AddMovementSubscription();
            FoodStack.Setup(currentStackLimit: PlayerSettings.ImmutableValue.CurrentStackLimit,
                            toPickupDuration: PlayerSettings.ImmutableValue.CurveMoves.ToPickupDuration,
                            toPickupHeight: PlayerSettings.ImmutableValue.CurveMoves.ToPickupHeight,
                            moveCurrentTypeToPickupFrequency: PlayerSettings.ImmutableValue.MoveCurrentTypeToPickupFrequency,
                            toBinDuration: PlayerSettings.ImmutableValue.CurveMoves.ToBinDuration,
                            toBinHeight: PlayerSettings.ImmutableValue.CurveMoves.ToBinHeight,
                            moveAllToBinFrequency: PlayerSettings.ImmutableValue.MoveAllToBinFrequency);
        }

        public void SelfReset()
        {
            CancelMovementSubscription();
            AddMovementSubscription();
            FoodStack.ResetItemsStack();
            Idle();
            transform.rotation = Quaternion.identity;
            transform.localPosition = startPosition;
        }
    }
}
