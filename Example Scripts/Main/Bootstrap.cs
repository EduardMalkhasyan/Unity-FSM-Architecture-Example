using BugiGames.StateMachine;
using BugiGames.GameState;
using UnityEngine;
using Zenject;
using BugiGames.ScriptableObject;
using BugiGames.Ads;
using BugiGames.UI;

namespace BugiGames.Main
{
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private MainGameStates mainGameStates;
        [Inject] private MainCanvas mainCanvas;
        [Inject] private Player player;

        private void Awake()
        {
            player.Deactivate();
            player.Setup();
            mainCanvas.ScreenSpaceOverlay();
            GameSettings.Value.InitSettings();
            mainGameStates.EnterState<EnterImmediatePlayState>();

#if !PROJECT_DEBUGGER
            // Can be use in production release
#endif
        }
    }
}



