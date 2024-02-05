using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace BugiGames.AI.State.Worker.Sorter
{
    public class AIWorkerSorterGoToCookingPlaceState : AIAbstractGameState
    {
        [Inject] private AIWorkerSorter workerSorter;

        public override void Enter()
        {
            workerSorter.PlaySimpleWalkAnimation();
            workerSorter.GoToCookingPlace(OnComplete: () => { EnterPickupPlaceState(); });
        }

        public override void Exit()
        {

        }

        public async void EnterPickupPlaceState()
        {
            try
            {
                workerSorter.PlayIdleWithItemAnimation();
                await UniTask.WaitUntil(() => workerSorter.FoodStack.AvailableStackIsEmpty() == false);

                workerSorter.EnterState<AIWorkerSorterGoToPickupPlaceState>();
            }
            catch (Exception error)
            {
                Debug.LogWarning(error);
            }
        }
    }
}
