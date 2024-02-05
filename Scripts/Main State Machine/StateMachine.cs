using System;

namespace BugiGames.StateMachine
{
    public class StateMachine
    {
        public Type CurrentStateClass => (currentState != null) ? currentState.GetType() : null;
        private IGameState currentState;

        public void Tick()
        {
            currentState?.Tick();
        }

        public void SetState(IGameState state)
        {
            currentState?.Exit();
            currentState = state;
            currentState.Enter();
        }
    }
}
