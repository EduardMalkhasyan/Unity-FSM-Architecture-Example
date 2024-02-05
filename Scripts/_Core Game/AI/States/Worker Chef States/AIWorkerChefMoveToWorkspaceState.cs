using Zenject;

namespace BugiGames.AI.State.Worker.Chef
{
    public class AIWorkerChefMoveToWorkspaceState : AIAbstractGameState
    {
        [Inject] private AIWorkerChef workerChef;

        public override void Enter()
        {
            workerChef.PlaySimpleWalkAnimation();
            workerChef.GoToWorkspace();
            workerChef.AIDestinationSetter.OnArrivedToTarget += WaitRequestsState;
        }

        public override void Exit()
        {
            workerChef.AIDestinationSetter.OnArrivedToTarget -= WaitRequestsState;
        }

        private void WaitRequestsState()
        {
            workerChef.EnterState<AIWorkerChefWaitRequestsState>();
        }
    }
}
