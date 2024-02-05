using BugiGames.Main;
using Zenject;

namespace BugiGames.AI.State.Worker.Chef
{
    public class AIWorkerChefMoveToSendDestinationState : AIAbstractGameState
    {
        [Inject] private AIWorkerChef workerChef;
        [Inject] private Kitchen kitchen;

        public override void Enter()
        {
            workerChef.PlayWalkWithFoodAnimation();
            kitchen.StopKitchenAnimation();
            workerChef.GoToSendFood();
            workerChef.AIDestinationSetter.OnArrivedToTarget += SendFoodState;
        }

        public override void Exit()
        {
            workerChef.AIDestinationSetter.OnArrivedToTarget -= SendFoodState;
        }

        private void SendFoodState()
        {
            workerChef.EnterState<AIWorkerChefSendFoodState>();
        }
    }
}
