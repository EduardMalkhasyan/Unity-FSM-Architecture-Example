using BugiGames.AI.State;
using BugiGames.AI.State.Worker.Chef;
using BugiGames.ExtensionMethod;
using BugiGames.Main;
using BugiGames.ScriptableObject;
using BugiGames.StateMachine;
using BugiGames.Tools;
using Cysharp.Threading.Tasks;
using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BugiGames.AI
{
    public class AIWorkerChef : MonoBehaviour
    {
        [Inject] private AIGameStates aIGameStates;

        [Inject] private CookingPlaces CookingPlaces;
        [SerializeField] private Transform[] foodStackPositions;

        [SerializeField] private Animator animator;

        [field: SerializeField] public AIDestinationSetter AIDestinationSetter { get; private set; }
        [SerializeField] private AIPath aIPath;

        [SerializeField] private Transform destinationWorkspace;
        [SerializeField] private Transform destinationSend;

        [SerializeField, ReadOnly] private List<FoodType> collectedFoodTypesForSend;
        [ShowInInspector, ReadOnly] private Dictionary<FoodType, int> foodRequestsKVP = new Dictionary<FoodType, int>();
        [SerializeField, ReadOnly] private bool isSendCompleted;
        [SerializeField, ReadOnly] private int foodStackIndex;

        [SerializeField] private bool canStateShowDebug;

        private const string Behavior = "Behavior";

        public void Work()
        {
            if (aIGameStates.CurrentStateType == null)
            {
                EnterState<AIWorkerChefWaitRequestsState>();
            }
            else
            {
                //Debug.Log("Already chef working");
            }
        }

        public void GoToSendFood()
        {
            AIDestinationSetter.Target = destinationSend;
        }

        public void GoToWorkspace()
        {
            AIDestinationSetter.Target = destinationWorkspace;
        }

        public void EnterState<T>() where T : AIAbstractGameState
        {
            aIGameStates.EnterState<T>(canStateShowDebug);
        }

        public void CollectRequests(FoodType foodType, int foodCount)
        {
            if (foodRequestsKVP.IsEmptyOrNull() == false)
            {
                if (foodRequestsKVP.ContainsKey(foodType))
                {
                    if (foodRequestsKVP[foodType] < foodCount)
                    {
                        foodRequestsKVP[foodType] = foodCount;
                    };
                }
            }

            foodRequestsKVP.TryAdd(foodType, foodCount);
        }


        public async void CookFoodAndPrepare(Action OnComplete)
        {
            var requestsCount = foodRequestsKVP.Count;
            var maxValue = foodRequestsKVP.OrderByDescending(count => count.Value).First().Value;
            var criticalIndex = maxValue * requestsCount;

            while (criticalIndex > foodStackPositions.Length)
            {
                requestsCount--;
                criticalIndex -= maxValue;
            }

            try
            {
                for (int j = 0; j < requestsCount; j++)
                {
                    var request = foodRequestsKVP.First();
                    int foodCount = request.Value;
                    FoodType foodType = request.Key;

                    for (int i = 0; i < foodCount; i++)
                    {
                        CookingPlaces.Place(foodType).TryCookFood(foodStackPositions[foodStackIndex],
                                                                  OnComplete: () =>
                                                                  {
                                                                      foodRequestsKVP.Remove(foodType);
                                                                      foodStackIndex++;
                                                                  });

                        await UniTask.WaitForSeconds(AIWorkerChefSettings.ImmutableValue.CookFoodFrequency);
                    }

                    collectedFoodTypesForSend.TryAdd(foodType);
                }
            }
            catch (Exception error)
            {
                Debug.LogWarning(error);
            }

            OnComplete.Invoke();
        }

        public async void SendCollectedFood(Action OnComplete)
        {
            try
            {
                for (int i = collectedFoodTypesForSend.Count - 1; i >= 0; i--)
                {
                    isSendCompleted = false;

                    SendFood(collectedFoodTypesForSend[i], () =>
                    {
                        isSendCompleted = true;
                    });

                    await UniTask.WaitUntil(() => isSendCompleted);
                }
            }
            catch (Exception error)
            {
                Debug.LogWarning(error);
            }

            collectedFoodTypesForSend.Clear();
            foodStackIndex = 0;

            OnComplete.Invoke();
        }

        private void SendFood(FoodType foodType, Action OnSendComplete)
        {
            CookingPlaces.Place(foodType).TrySendCookedFood(AIWorkerChefSettings.ImmutableValue.MoveCurveToFoodShelfDuration,
                                                            AIWorkerChefSettings.ImmutableValue.MoveCurveToFoodShelfHeight,
                                                            AIWorkerChefSettings.ImmutableValue.SendToFoodShelfFrequency,
                                                            OnComplete: () =>
                                                             {
                                                                 OnSendComplete.Invoke();
                                                             });
        }

        public void PlaySimpleWalkAnimation()
        {
            animator.SetInteger(Behavior, 3);
        }

        public void PlayCookAnimation()
        {
            animator.SetInteger(Behavior, 2);
        }

        public void PlayWalkWithFoodAnimation()
        {
            animator.SetInteger(Behavior, 1);
        }

        public void PlayHoldFoodIdleAnimation()
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
