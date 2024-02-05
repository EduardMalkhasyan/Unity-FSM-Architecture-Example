using BugiGames.Tools;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace BugiGames.UI
{
    public class UIScreensController : MonoBehaviour
    {
        [SerializeField] private SerializableDictionary<UIScreenEnum, UIScreen> uIScreensKVP;
        [SerializeField] private UIScreen currentScreen;

        private void Awake()
        {
            foreach (var uiScreen in uIScreensKVP.dictionary)
            {
                uiScreen.Value.HideInstant();
            }
        }

        [Button]
        public void ShowUIScreen(UIScreenEnum uIScreenEnum, Action OnCompleteCB = null)
        {
            if (currentScreen == null)
            {
                currentScreen = uIScreensKVP.dictionary[uIScreenEnum];
                currentScreen.Show(() =>
                {
                    OnCompleteCB?.Invoke();
                });
            }

            if (currentScreen != uIScreensKVP.dictionary[uIScreenEnum])
            {
                currentScreen.Hide(() =>
                {
                    uIScreensKVP.dictionary[uIScreenEnum].Show();
                    currentScreen = uIScreensKVP.dictionary[uIScreenEnum];
                    OnCompleteCB?.Invoke();
                });
            }
        }

        [Button]
        public void ShowUIScreenWithDelay(UIScreenEnum uIScreenEnum, Action OnCompleteCB = null, float delay = 0)
        {
            if (currentScreen == null)
            {
                currentScreen = uIScreensKVP.dictionary[uIScreenEnum];
                currentScreen.ShowWithDelay(() =>
                {
                    OnCompleteCB?.Invoke();
                }, delay);
            }

            if (currentScreen != uIScreensKVP.dictionary[uIScreenEnum])
            {
                currentScreen.Hide(() =>
                {
                    uIScreensKVP.dictionary[uIScreenEnum].ShowWithDelay(interval: delay);
                    currentScreen = uIScreensKVP.dictionary[uIScreenEnum];
                    OnCompleteCB?.Invoke();
                });
            }
        }

        [Button]
        public void ShowInstantUIScreen(UIScreenEnum uIScreenEnum, Action OnCompleteCB = null)
        {
            if (currentScreen == null)
            {
                currentScreen = uIScreensKVP.dictionary[uIScreenEnum];
                currentScreen.ShowInstant(() =>
                {
                    OnCompleteCB?.Invoke();
                });
            }

            if (currentScreen != uIScreensKVP.dictionary[uIScreenEnum])
            {
                currentScreen.HideInstant(() =>
                {
                    uIScreensKVP.dictionary[uIScreenEnum].ShowInstant();
                    currentScreen = uIScreensKVP.dictionary[uIScreenEnum];
                    OnCompleteCB?.Invoke();
                });
            }
        }
    }

    public enum UIScreenEnum
    {
        Gameplay,
        MainMenu,
        Settings,
        Shop,
        Tutorial,
        RemoveAds,
        EnterImmediatePlay,
        Credits
    }
}

