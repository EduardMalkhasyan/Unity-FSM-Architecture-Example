using Zenject;

namespace BugiGames.AI.State.Worker.Chef
{
    public class AIWorkerChefSendFoodState : AIAbstractGameState
    {
        [Inject] private AIWorkerChef workerChef;

        public override void Enter()
        {
            workerChef.PlayHoldFoodIdleAnimation();
            workerChef.SendCollectedFood(OnComplete: () =>
            {
                workerChef.EnterState<AIWorkerChefMoveToWorkspaceState>();
            });
        }

        public override void Exit()
        {

        }
    }
}
