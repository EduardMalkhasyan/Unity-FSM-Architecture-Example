using BugiGames.Ads;
using BugiGames.Main;
using BugiGames.ScriptableObject;
using BugiGames.StateMachine;
using BugiGames.Tools;
using BugiGames.UI;
using BugiGames.UI.Widget;
using Zenject;

namespace BugiGames.GameState
{
    public class MainMenuState : MainAbstractGameState
    {
        [Inject] private UIScreensController screensController;
        [Inject] private MainMenuWidget mainMenuWidget;
        [Inject] private MainGameStates mainGameStates;
        [Inject] private AvatarHolder avatarHolder;
        [Inject] private AdsController adsController;
        [Inject] private OpenAppLink openAppLink;
        [Inject] private RendererActivator rendererActivator;
        [Inject] private UIMainBackground uIMainBackground;
        [Inject] private WaitLoadingSpinnerDots waitLoadingSpinnerDots;
        [Inject] private Player player;

        public override void Enter()
        {
            player.Deactivate();

            uIMainBackground.Open();
            rendererActivator.RenderUI();

            screensController.ShowUIScreen(UIScreenEnum.MainMenu);

            mainMenuWidget.OnGoToSettings += EnterSettings;
            mainMenuWidget.OnPlayGame += LoadAssetsAndEnterStart;
            mainMenuWidget.OnMoreApps += OpenMoreApps;
            mainMenuWidget.OnShop += OpenShop;
            mainMenuWidget.OnRemoveAds += OpenRemoveAds;
            mainMenuWidget.OnCredits += OpenCredits;

            TryShowBannerAddUntilItLoad();
        }

        public override void Exit()
        {
            mainMenuWidget.OnGoToSettings -= EnterSettings;
            mainMenuWidget.OnPlayGame -= LoadAssetsAndEnterStart;
            mainMenuWidget.OnMoreApps -= OpenMoreApps;
            mainMenuWidget.OnShop -= OpenShop;
            mainMenuWidget.OnRemoveAds -= OpenRemoveAds;
            mainMenuWidget.OnCredits -= OpenCredits;
        }

        private void EnterSettings()
        {
            mainGameStates.EnterState<SettingsState>();
        }

        private void OpenShop()
        {
            mainGameStates.EnterState<ShopState>();
        }

        private void OpenRemoveAds()
        {
            mainGameStates.EnterState<RemoveAdsState>();
        }

        private void OpenCredits()
        {
            mainGameStates.EnterState<CreditsState>();
        }

        private void LoadAssetsAndEnterStart()
        {
            waitLoadingSpinnerDots.EnableLoadingScreen();
            ShopData.Value.Load();

            LoadAvatar();
        }

        private void LoadAvatar()
        {
            avatarHolder.LoadAvatar(ShopDataTemp.CurrentShopItemFormTemp.path,
                                  () =>
                                  {
                                      waitLoadingSpinnerDots.DisableLoadingScreen();
                                      mainGameStates.EnterState<EnterImmediatePlayState>();
                                      player.Activate();
                                  });
        }

        private void TryShowBannerAddUntilItLoad()
        {
            if (IAPData.Value.IsAdsPurchased())
            {
                DebugColor.LogGreen("Ads purchased cannot be displayed.");
            }
            else
            {
                adsController.TryShowBannerBottom();
            }
        }

        private void OpenMoreApps()
        {
            openAppLink.OpenLink();
        }
    }
}
