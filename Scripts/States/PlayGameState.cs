using BugiGames.Camera;
using BugiGames.Main;
using BugiGames.ScriptableObject;
using BugiGames.StateMachine;
using BugiGames.UI;
using Zenject;

namespace BugiGames.GameState
{
    public class PlayGameState : MainAbstractGameState
    {
        [Inject] private UIScreensController screensController;
        [Inject] private VirtualCamera virtualCamera;
        [Inject] private MainGameStates mainGameStates;
        [Inject] private TutorialState tutorialState;
        [Inject] private Kitchen kitchen;

        public override void Enter()
        {
            screensController.ShowInstantUIScreen(UIScreenEnum.Gameplay);
            virtualCamera.SwitchCamera(VirtualCameraType.Close);

            kitchen.ActivateKitchen();
            TutorialObserver.OnTutorial += EnterTutorialDemonstration;
        }

        public override void Exit()
        {
            TutorialObserver.OnTutorial -= EnterTutorialDemonstration;
        }

        private void EnterTutorialDemonstration(TutorialEnum tutorialEnum)
        {
            var preset = TutorialProps.Value.GetTutorialPresetKVP(tutorialEnum);
            tutorialState.Setup(preset.tutorialObject, tutorialEnum);
            mainGameStates.EnterState<TutorialState>();
        }
    }
}

