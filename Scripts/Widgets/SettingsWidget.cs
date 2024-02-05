using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BugiGames.UI.Widget
{
    public class SettingsWidget : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private TMP_Dropdown qualityDropDown;

        public event Action OnGoBack;
        public event Action<int> OnQualityChanged;

        private void OnEnable()
        {
            backButton.onClick.AddListener(() => OnGoBack.Invoke());
            qualityDropDown.onValueChanged.AddListener((value) => OnQualityChanged.Invoke(value));
        }

        private void OnDisable()
        {
            backButton.onClick.RemoveAllListeners();
            qualityDropDown.onValueChanged.RemoveAllListeners();
        }

        public void SetQualityDropDown(int qualitySettingsLevel)
        {
            qualityDropDown.value = qualitySettingsLevel;
        }
    }
}
