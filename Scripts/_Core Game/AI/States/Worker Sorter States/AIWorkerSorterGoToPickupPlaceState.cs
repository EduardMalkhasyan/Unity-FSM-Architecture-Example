using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace BugiGames.AI.State.Worker.Sorter
{
    public class AIWorkerSorterGoToPickupPlaceState : AIAbstractGameState
    {
        [Inject] private AIWorkerSorter workerSorter;

        public override void Enter()
        {
            workerSorter.PlayWalkWithFoodAnimation();
            workerSorter.GoToPickupPlace(OnComplete: () => { EnterWaitingRequestState(); });
        }

        public override void Exit()
        {

        }

        public async void EnterWaitingRequestState()
        {
            try
            {
                workerSorter.PlayIdleWithItemAnimation();
                await UniTask.WaitUntil(() => workerSorter.FoodStack.AvailableStackIsEmpty() == true);

                workerSorter.EnterState<AIWorkerSorterWaitingRequestState>();
            }
            catch (Exception error)
            {
                Debug.LogWarning(error);
            }
        }
    }
}
