using BugiGames.Tools;
using UnityEngine;
using Zenject;

namespace BugiGames.StateMachine
{
    public class MainGameStates : MonoBehaviour
    {
        [Inject] private DiContainer container;
        private StateMachine stateMachine;
        public System.Type CurrentStateType => stateMachine.CurrentStateClass;

        private void Awake()
        {
            stateMachine = new StateMachine();
        }

        private void FixedUpdate()
        {
            stateMachine.Tick();
        }

        public void EnterState<T>() where T : MainAbstractGameState
        {
            if (stateMachine.CurrentStateClass == typeof(T)) return;
            DebugColor.LogBlue($"Entering state: {typeof(T)}");
            var state = container.Resolve<T>();
            stateMachine.SetState(state);
        }
    }
}
