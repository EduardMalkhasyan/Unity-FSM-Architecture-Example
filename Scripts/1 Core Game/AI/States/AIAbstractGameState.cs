using BugiGames.StateMachine;

namespace BugiGames.AI.State
{
    public abstract class AIAbstractGameState : IGameState
    {
        public abstract void Enter();
        public abstract void Exit();
        public virtual void Tick() { }
    }
}
