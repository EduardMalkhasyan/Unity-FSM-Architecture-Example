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
        [Inject] private AdsController adsController;
        [Inject] private MainCanvas mainCanvas;
        [Inject] private Player player;

        private void Awake()
        {
            player.Deactivate();
            player.Setup();
            mainCanvas.ScreenSpaceOverlay();
            ShopData.Value.CheckIfShopItemsIsRight();
            GameSettings.Value.InitSettings();
            mainGameStates.EnterState<EnterImmediatePlayState>();

#if !PROJECT_DEBUGGER
            adsController.Initialize();
#endif
        }
    }
}



