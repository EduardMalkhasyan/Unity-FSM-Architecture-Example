//using BugiGames.ScriptableObject;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BugiGames.Main
{
    public class FoodStack : MonoBehaviour
    {
        [Inject] private FoodStackSorter foodStackSorter;

        [SerializeField] private List<FoodStackHolder> foodStackHolders;
        [SerializeField, ReadOnly] private List<FoodStackHolder> foodStackHoldersLimited;
        [SerializeField, ReadOnly] private List<FoodStackHolder> availableFoodStackHolders;

        [SerializeField, ReadOnly] private int stackHolderTransformIndex;
        [SerializeField, ReadOnly] private int stackHolderFoodIndex;
        [SerializeField, ReadOnly] private bool moveToBinProcessed;

        [SerializeField, Space, ReadOnly] private int currentStackLimit;
        [SerializeField, ReadOnly] private float toPickupDuration;
        [SerializeField, ReadOnly] private float toPickupHeight;
        [SerializeField, ReadOnly] private float moveCurrentTypeToPickupFrequency;
        [SerializeField, ReadOnly] private float toBinDuration;
        [SerializeField, ReadOnly] private float toBinHeight;
        [SerializeField, ReadOnly] private float moveAllToBinFrequency;

        public event Action OnStackFilled;
        public event Action OnStackEmptied;

        public void Setup(int currentStackLimit, float toPickupDuration, float toPickupHeight,
                          float moveCurrentTypeToPickupFrequency, float toBinDuration,
                          float toBinHeight, float moveAllToBinFrequency)
        {
            foodStackHoldersLimited = new List<FoodStackHolder>(foodStackHolders);

            this.currentStackLimit = currentStackLimit;
            this.toPickupDuration = toPickupDuration;
            this.toPickupHeight = toPickupHeight;
            this.moveCurrentTypeToPickupFrequency = moveCurrentTypeToPickupFrequency;
            this.toBinDuration = toBinDuration;
            this.toBinHeight = toBinHeight;
            this.moveAllToBinFrequency = moveAllToBinFrequency;

            DowngradeFoodStackHoldersLimit(currentStackLimit);
        }

        public async void MoveCurrentFoodTypeForPickup(List<Food> availableFoodListForPut, FoodType foodType,
                                                       bool isPickupFoodProcessed, Action OnPass,
                                                       Action<Food> OnMoveComplete, Action OnProcessComplete
                                                       )
        {
            if (availableFoodListForPut.IsNullOrEmpty())
            {
                //Debug.Log($"Available food list for put is empty");
                return;
            }

            if (isPickupFoodProcessed)
            {
                //Debug.Log($"{foodType} pickup food is filled");
                return;
            }

            var currentFoodTypeList = new List<FoodStackHolder>();

            for (int i = 0; i < availableFoodStackHolders.Count; i++)
            {
                if (availableFoodStackHolders[i].IsSelfFoodExist &&
                    availableFoodStackHolders[i].SelfFoodType == foodType)
                {
                    currentFoodTypeList.Add(availableFoodStackHolders[i]);
                }
            }

            if (currentFoodTypeList.IsNullOrEmpty())
            {
                foodStackSorter.StopSortingStackRun();
                foodStackSorter.TrySortStackRun(foodStackHoldersLimited);
                //Debug.Log($"{foodType} type list is empty");
                return;
            }

            var availableFoodTransforms = new List<Transform>();

            foreach (var foodForPlace in availableFoodListForPut)
            {
                availableFoodTransforms.Add(foodForPlace.transform);
            }

            int chosenListCount;

            if (currentFoodTypeList.Count > availableFoodListForPut.Count)
            {
                chosenListCount = availableFoodListForPut.Count;
            }
            else
            {
                chosenListCount = currentFoodTypeList.Count;
            }

            OnPass.Invoke();

            for (int i = currentFoodTypeList.Count - 1; i >= currentFoodTypeList.Count - chosenListCount; i--)
            {
                if (StackIsEmpty())
                {
                    //Debug.Log("Stack is empty");
                    break;
                }

                var cachedHolder = currentFoodTypeList[i];
                var cachedIndex = i - (currentFoodTypeList.Count - chosenListCount);

                cachedHolder.SetSentStatus();
                var cachedFood = cachedHolder.CurrentFood;

                cachedHolder.MoveToTarget(availableFoodTransforms[cachedIndex],
                             toPickupDuration,
                             toPickupHeight,
                            () =>
                            {
                                DeactivateCurrentStackHolderFood(cachedHolder);
                                cachedHolder.ResetLocalPosition();

                                availableFoodListForPut[cachedIndex].ShowWithReplacedMesh(cachedFood.MeshRenderer,
                                                                                          cachedFood.MeshFilter);

                                OnMoveComplete?.Invoke(availableFoodListForPut[cachedIndex]);

                                // If cachedIndex is 0, sorting is based on the last piece
                                if (cachedIndex == 0)
                                {
                                    OnProcessComplete.Invoke();

                                    foodStackSorter.StopSortingStackRun();
                                    foodStackSorter.TrySortStackRun(foodStackHoldersLimited);
                                }
                            });

                DecrementStackHolderTransformIndex();
                await UniTask.WaitForSeconds(moveCurrentTypeToPickupFrequency);
            }
        }

        public void TrySortStackRunOnQuitPickup()
        {
            foodStackSorter.StopSortingStackRun();
            foodStackSorter.TrySortStackRun(foodStackHoldersLimited);
        }

        public async void MoveAllAvailableToBin(Transform target, Action OnStartProcess, Action OnFinishProcess)
        {
            foodStackSorter.StopSortingStackRun();

            if (AvailableStackIsEmpty())
            {
                //Debug.Log("Available stack is empty");
                return;
            }

            if (moveToBinProcessed)
            {
                // Debug.Log("Move to bin already processed");
                return;
            }

            moveToBinProcessed = true;
            OnStartProcess.Invoke();

            for (int i = availableFoodStackHolders.Count - 1; i >= 0; i--)
            {
                if (StackIsEmpty())
                {
                    // Debug.Log("Stack is empty");
                    break;
                }

                var cachedHolder = availableFoodStackHolders[i];
                var cachedIndex = i;

                cachedHolder.MoveToTarget(target,
                             toBinDuration,
                             toBinHeight,
                             () =>
                             {
                                 DeactivateCurrentStackHolderFood(cachedHolder);
                                 cachedHolder.ResetLocalPosition();

                                 // Last element check
                                 if (cachedIndex == 0)
                                 {
                                     OnFinishProcess.Invoke();
                                 }
                             });

                DecrementStackHolderTransformIndex();

                await UniTask.WaitForSeconds(moveAllToBinFrequency);
            }

            moveToBinProcessed = false;
        }

        public void ResetItemsStack()
        {
            foreach (var foodStackHolder in foodStackHoldersLimited)
            {
                foodStackHolder.HideFoodAndReset();
            }

            availableFoodStackHolders.Clear();
            stackHolderTransformIndex = 0;
            stackHolderFoodIndex = 0;
        }

        public Transform CurrentStackHolderTransform()
        {
            return foodStackHoldersLimited[stackHolderTransformIndex].transform;
        }

        public bool StackIsFull()
        {
            return stackHolderTransformIndex == foodStackHoldersLimited.Count;
        }

        public bool StackIsEmpty()
        {
            return stackHolderTransformIndex == 0;
        }

        public bool AvailableStackIsEmpty()
        {
            return availableFoodStackHolders.IsNullOrEmpty();
        }

        public bool IsAnyFoodInStack()
        {
            return availableFoodStackHolders.Any(holder => holder.SelfFoodType != FoodType.ForBin);
        }

        public bool IsAnyFoodForBinInStack()
        {
            return availableFoodStackHolders.Any(holder => holder.SelfFoodType == FoodType.ForBin);
        }

        public void IncrementStackHolderTransformIndex()
        {
            stackHolderTransformIndex++;
        }

        public void DecrementStackHolderTransformIndex()
        {
            stackHolderTransformIndex--;

            if (stackHolderTransformIndex == 0)
            {
                OnStackEmptied?.Invoke();
            }
        }

        public void DowngradeFoodStackHoldersLimit(int elementsToRemove)
        {
            if (elementsToRemove == foodStackHoldersLimited.Count)
            {
                //Debug.Log("There is no elements to remove achieved max stack limit count");
                return;
            }

            var downgradedLimit = foodStackHoldersLimited.Count - elementsToRemove;

            if (downgradedLimit > 0 && downgradedLimit <= foodStackHoldersLimited.Count)
            {
                foodStackHoldersLimited = foodStackHoldersLimited
                                          .Take(foodStackHoldersLimited.Count - downgradedLimit)
                                          .ToList();
            }
            else
            {
                //Debug.LogWarning("Invalid number of elements to remove. Check the range.");
            }
        }

        public void UpgradeFoodStackHoldersLimit(int elementsToAdd)
        {
            if (elementsToAdd > 0)
            {
                var elementsToAddList = foodStackHolders
                                        .Except(foodStackHoldersLimited)
                                        .Take(elementsToAdd)
                                        .ToList();

                foodStackHoldersLimited.AddRange(elementsToAddList);
            }
            else
            {
                //Debug.LogWarning("Invalid number of elements to add. It should be greater than 0.");
            }
        }

        public void ActivateStackHolderFood(Food food)
        {
            foodStackHoldersLimited[stackHolderFoodIndex].ShowFood(food);
            availableFoodStackHolders.Add(foodStackHoldersLimited[stackHolderFoodIndex]);
            stackHolderFoodIndex++;

            OnStackFilled?.Invoke();
        }

        public void DeactivateCurrentStackHolderFood(FoodStackHolder foodStackHolder)
        {
            foodStackHolder.HideFoodAndReset();
            availableFoodStackHolders.Remove(foodStackHolder);
            stackHolderFoodIndex--;
        }

#if UNITY_EDITOR
        [ContextMenu("Get All Holders")]
        private void GetAllHolders()
        {
            foodStackHolders = GetComponentsInChildren<FoodStackHolder>().ToList();
        }
#endif
    }
}
