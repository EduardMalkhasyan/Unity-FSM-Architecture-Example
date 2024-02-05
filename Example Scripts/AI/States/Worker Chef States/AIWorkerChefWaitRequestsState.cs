using BugiGames.Main;
using BugiGames.ScriptableObject;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

namespace BugiGames.AI.State.Worker.Chef
{
    public class AIWorkerChefWaitRequestsState : AIAbstractGameState
    {
        [Inject] private Kitchen kitchen;
        [Inject] private AIWorkerChef workerChef;

        private CancellationTokenSource cancellationTokenSource;

        public override async void Enter()
        {
            workerChef.PlayCookAnimation();
            kitchen.PlayKitchenAnimation();
            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await UniTask.WaitUntil(() => kitchen.IsAnyRequestAvailable(),
                                              cancellationToken: cancellationTokenSource.Token);

                for (int i = 0; i < AIWorkerChefSettings.ImmutableValue.FindRequestsIterationsCount; i++)
                {
                    kitchen.FindRequests(OnSuccessRequest: (foodType, foodCount) =>
                    {
                        workerChef.CollectRequests(foodType, foodCount);
                    });

                    await UniTask.WaitForSeconds(AIWorkerChefSettings.ImmutableValue.FindRequestsIterationsDelay,
                                                 cancellationToken: cancellationTokenSource.Token);
                }

                workerChef.EnterState<AIWorkerChefCookFoodState>();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        public override void Exit()
        {
            cancellationTokenSource?.Cancel();
        }
    }
}
