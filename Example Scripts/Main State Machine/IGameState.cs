using UnityEngine;

namespace BugiGames.StateMachine
{
    public interface IGameState
    {
        void Enter();
        void Exit();
        void Tick();
    }
}

