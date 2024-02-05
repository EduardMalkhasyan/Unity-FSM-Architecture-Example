using BugiGames.Camera;
using BugiGames.ScriptableObject;
using BugiGames.StateMachine;
using BugiGames.UI;
using BugiGames.UI.Widget;
using UnityEngine.UIElements;
using Zenject;

namespace BugiGames.GameState
{
    public class SettingsState : MainAbstractGameState
    {
        [Inject] private UIScreensController screensController;
        [Inject] private SettingsWidget settingsWidget;
        [Inject] private MainGameStates mainGameStates;

        public override void Enter()
        {
            screensController.ShowUIScreen(UIScreenEnum.Settings);
            settingsWidget.SetQualityDropDown(GameSettings.Value.GetQualityLevel());
            settingsWidget.OnGoBack += EnterMainMenu;
            settingsWidget.OnQualityChanged += GameSettings.Value.SetQualityLevel;
        }

        public override void Exit()
        {
            settingsWidget.OnGoBack -= EnterMainMenu;
            settingsWidget.OnQualityChanged -= GameSettings.Value.SetQualityLevel;
        }

        public void EnterMainMenu()
        {
            mainGameStates.EnterState<MainMenuState>();
        }
    }
}
