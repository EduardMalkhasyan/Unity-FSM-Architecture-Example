using System;
using UnityEngine;
using UnityEngine.UI;

namespace BugiGames.UI.Widget
{
    public class MainMenuWidget : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button levelsButton;
        [SerializeField] private Button moreAppsButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button removeAdsButton;
        [SerializeField] private Button creditsButton;

        public event Action OnPlayGame;
        public event Action OnGoToSettings;
        public event Action OnGoToLevels;
        public event Action OnMoreApps;
        public event Action OnShop;
        public event Action OnRemoveAds;
        public event Action OnCredits;

        private void OnEnable()
        {
            playButton.onClick.AddListener(() => OnPlayGame.Invoke());
            settingsButton.onClick.AddListener(() => OnGoToSettings.Invoke());
            levelsButton.onClick.AddListener(() => OnGoToLevels.Invoke());
            moreAppsButton.onClick.AddListener(() => OnMoreApps.Invoke());
            shopButton.onClick.AddListener(() => OnShop.Invoke());
            removeAdsButton.onClick.AddListener(() => OnRemoveAds.Invoke());
            creditsButton.onClick.AddListener(() => OnCredits.Invoke());
        }

        private void OnDisable()
        {
            playButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
            levelsButton.onClick.RemoveAllListeners();
            moreAppsButton.onClick.RemoveAllListeners();
            shopButton.onClick.RemoveAllListeners();
            removeAdsButton.onClick.RemoveAllListeners();
            creditsButton.onClick.RemoveAllListeners();
        }
    }
}
