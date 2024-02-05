using UnityEngine;

namespace BugiGames.StateMachine
{
    public abstract class MainAbstractGameState : IGameState
    {
        public abstract void Enter();
        public abstract void Exit();
        public virtual void Tick() { }
    }
}
