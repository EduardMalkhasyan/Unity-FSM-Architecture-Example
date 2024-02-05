using Zenject;

namespace BugiGames.AI.State.Worker.Sorter
{
    public class AIWorkerSorterWaitingRequestState : AIAbstractGameState
    {
        [Inject] private AIWorkerSorter workerSorter;

        public override void Enter()
        {
            workerSorter.PlaySimpleWalkAnimation();
            workerSorter.GoToWaiting(OnComplete: () => { EnterCookingPlaceState(); });
        }

        public override void Exit()
        {

        }

        public void EnterCookingPlaceState()
        {
            workerSorter.PlaySimpleIdleAnimation();
            workerSorter.EnterState<AIWorkerSorterGoToCookingPlaceState>();
        }
    }
}
