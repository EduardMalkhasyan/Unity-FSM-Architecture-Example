using BugiGames.AI.State;
using BugiGames.AI.State.Worker.Sorter;
using BugiGames.Main;
using BugiGames.ScriptableObject;
using BugiGames.StateMachine;
using BugiGames.Tools;
using Cysharp.Threading.Tasks;
using Pathfinding;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;

namespace BugiGames.AI
{
    public class AIWorkerSorter : MonoBehaviour
    {
        [Inject] private AIGameStates aIGameStates;
        [field: SerializeField] public FoodStack FoodStack { get; private set; }
        [field: SerializeField] public AIDestinationSetter AIDestinationSetter { get; private set; }
        [SerializeField] private AIPath aIPath;

        [SerializeField] private Transform[] destinationsCookingCookie;
        [SerializeField] private Transform[] destinationsPickupCookie;
        [SerializeField] private Transform destinationWaiting;

        [SerializeField] private Animator animator;
        [SerializeField] private bool canStateShowDebug;

        private const string Behavior = "Behavior";

        private void Awake()
        {
            Setup();
            Work();
        }

        public void Setup()
        {
            FoodStack.Setup(currentStackLimit: AIWorkerSorterSettings.ImmutableValue.CurrentStackLimit,
                            toPickupDuration: AIWorkerSorterSettings.ImmutableValue.CurveMoves.ToPickupDuration,
                            toPickupHeight: AIWorkerSorterSettings.ImmutableValue.CurveMoves.ToPickupHeight,
                            moveCurrentTypeToPickupFrequency:
                            AIWorkerSorterSettings.ImmutableValue.MoveCurrentTypeToPickupFrequency,
                            toBinDuration: AIWorkerSorterSettings.ImmutableValue.CurveMoves.ToBinDuration,
                            toBinHeight: AIWorkerSorterSettings.ImmutableValue.CurveMoves.ToBinHeight,
                            moveAllToBinFrequency: AIWorkerSorterSettings.ImmutableValue.MoveAllToBinFrequency);
        }

        public void Work()
        {
            if (aIGameStates.CurrentStateType == null)
            {
                EnterState<AIWorkerSorterWaitingRequestState>();
            }
            else
            {
                Debug.Log("Already sorter working");
            }
        }

        private async void PassFromDestinations(Action OnComplete, Transform[] destinations)
        {
            try
            {
                for (int i = 0; i < destinations.Length; i++)
                {
                    int index = i;
                    AIDestinationSetter.Target = destinations[i];

                    void OnArrivedToTargetCall()
                    {
                        index++;

                        if (index == destinations.Length)
                        {
                            OnComplete?.Invoke();
                        }
                    }

                    AIDestinationSetter.OnArrivedToTarget += OnArrivedToTargetCall;
                    await UniTask.WaitUntil(() => index > i);
                    AIDestinationSetter.OnArrivedToTarget -= OnArrivedToTargetCall;
                }
            }
            catch (Exception error)
            {
                Debug.LogWarning(error);
            }
        }

        [Button]
        public void GoToPickupPlace(Action OnComplete = null)
        {
            PassFromDestinations(OnComplete, destinationsPickupCookie);
        }

        [Button]
        public void GoToCookingPlace(Action OnComplete = null)
        {
            PassFromDestinations(OnComplete, destinationsCookingCookie);
        }

        [Button]
        public void GoToWaiting(Action OnComplete = null)
        {
            AIDestinationSetter.Target = destinationWaiting;

            void OnArrivedToTargetCall()
            {
                OnComplete?.Invoke();
                AIDestinationSetter.OnArrivedToTarget -= OnArrivedToTargetCall;
            }

            AIDestinationSetter.OnArrivedToTarget += OnArrivedToTargetCall;
        }

        public void EnterState<T>() where T : AIAbstractGameState
        {
            aIGameStates.EnterState<T>(canStateShowDebug);
        }

        public void PlaySimpleWalkAnimation()
        {
            animator.SetInteger(Behavior, 3);
        }

        public void PlayIdleWithItemAnimation()
        {
            animator.SetInteger(Behavior, 2);
        }

        public void PlayWalkWithFoodAnimation()
        {
            animator.SetInteger(Behavior, 1);
        }

        public void PlaySimpleIdleAnimation()
        {
            animator.SetInteger(Behavior, 0);
        }

#if UNITY_EDITOR
        [ContextMenu("Show AI Current State")]
        public void ShowCurrentState()
        {
            DebugColor.LogBlue($"AI current state is: {aIGameStates.CurrentStateType.Name}", true);
        }
#endif
    }
}
