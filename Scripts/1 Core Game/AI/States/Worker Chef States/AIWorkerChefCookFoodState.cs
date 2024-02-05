using Zenject;

namespace BugiGames.AI.State.Worker.Chef
{
    public class AIWorkerChefCookFoodState : AIAbstractGameState
    {
        [Inject] private AIWorkerChef workerChef;

        public override void Enter()
        {
            workerChef.PlayHoldFoodIdleAnimation();

            workerChef.CookFoodAndPrepare(OnComplete: () =>
            {
                workerChef.EnterState<AIWorkerChefMoveToSendDestinationState>();
            });
        }

        public override void Exit()
        {

        }
    }
}
