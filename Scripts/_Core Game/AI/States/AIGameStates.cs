using BugiGames.AI.State;
using BugiGames.Tools;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BugiGames.StateMachine
{
    [InfoBox("This component is bound 'FromNewComponentOnNewGameObject' and 'AsTransient' for multiple uses in AI space")]
    public class AIGameStates : MonoBehaviour
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

        public void EnterState<T>(bool canShowDebug = true) where T : AIAbstractGameState
        {
            if (stateMachine.CurrentStateClass == typeof(T)) return;

            if (canShowDebug)
            {
                DebugColor.LogBlue($"AI entering state: {typeof(T)}", bold: true);
            }

            var state = container.Resolve<T>();
            stateMachine.SetState(state);
        }
    }
}
