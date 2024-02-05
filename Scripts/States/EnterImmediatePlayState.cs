using BugiGames.Camera;
using BugiGames.Main;
using BugiGames.ScriptableObject;
using BugiGames.StateMachine;
using BugiGames.Tools;
using BugiGames.UI;
using Zenject;

namespace BugiGames.GameState
{
    public class EnterImmediatePlayState : MainAbstractGameState
    {
        [Inject] private MainGameStates mainGameStates;
        [Inject] private GameReset gameReset;
        [Inject] private RendererActivator rendererActivator;
        [Inject] private UIMainBackground uIMainBackground;
        [Inject] private UIScreensController screensController;
        [Inject] private AvatarHolder avatarHolder;
        [Inject] private WaitLoadingSpinnerDots waitLoadingSpinnerDots;
        [Inject] private FadeInOut fadeInOut;
        [Inject] private Player player;
        [Inject] private VirtualCamera virtualCamera;

        public override void Enter()
        {
            uIMainBackground.Close();
            rendererActivator.RenderEverything();
            screensController.ShowUIScreen(UIScreenEnum.EnterImmediatePlay);
            LoadAssetsAndEnterPlayGameState();
        }

        public override void Exit()
        {

        }

        private void LoadAssetsAndEnterPlayGameState()
        {
            waitLoadingSpinnerDots.EnableLoadingScreen();
            fadeInOut.FadeIn();
            ShopData.Value.Load();

            LoadAvatar();
        }

        private void LoadAvatar()
        {
            avatarHolder.LoadAvatar(ShopDataTemp.CurrentShopItemFormTemp.path,
                                  () =>
                                  {
                                      gameReset.DoReset();
                                      virtualCamera.SwitchCamera(VirtualCameraType.Far);

                                      var delay = 0.5f;
                                      waitLoadingSpinnerDots.DisableLoadingScreenWithDelay(delay);
                                      fadeInOut.FadeOutWithDelay(delay, isRaycastTarget: false);
                                      player.Activate();
                                      mainGameStates.EnterState<PlayGameState>();
                                  });
        }
    }
}
