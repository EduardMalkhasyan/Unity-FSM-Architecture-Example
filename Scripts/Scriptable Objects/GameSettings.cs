using BugiGames.Tools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BugiGames.ScriptableObject
{
    [InfoBox("If there is a need to modify or add code in other plugins, use comment below \b //BugiGames.Interposition")]
    public class GameSettings : SOLoader<GameSettings>
    {
        [field: SerializeField, Space] public string AndroidAppsMarketLink { get; private set; }
        [field: SerializeField] public string IOSAppsMarketLink { get; private set; }
        [field: SerializeField, Space] public string GameSceneAddress { get; private set; }

        [SerializeField, Space] private float gravity_Y;

        [SerializeField, Space] private int fpsCount;

        private const int defaultQualityLevel = 1;
        private const string qualityLevelKey = "qualityLevelKey";

        public void InitSettings()
        {
            SetNewGravity();
            SetGameFPS();
            SetQualityLevel(GetQualityLevel());
        }

        [ShowInInspector, PropertyRange(0, 3), InfoBox("Low = 0, Medium = 1, High = 2, Ultra = 3")]
        private int QualityLevelData
        {
            get
            {
                return PlayerPrefs.GetInt(qualityLevelKey, defaultQualityLevel);
            }
            set
            {
                PlayerPrefs.SetInt(qualityLevelKey, value);
            }
        }

        public int GetQualityLevel()
        {
            return QualityLevelData;
        }

        public void SetQualityLevel(int level)
        {
            QualityLevelData = level;
            QualitySettings.SetQualityLevel(level);
        }

        private void SetNewGravity()
        {
            Physics.gravity = new Vector3(0, gravity_Y, 0);
        }

        private void SetGameFPS()
        {
            Application.targetFrameRate = fpsCount;
        }

        [Button]
        private void ShowScreenResolution()
        {
            Debug.Log(Screen.currentResolution);
        }

        [Button]
        public void ResetGameSettingsData()
        {
            QualityLevelData = defaultQualityLevel;
            DebugColor.LogViolet($"Reset data: {this.name}");
        }

        [Button]
        public void DeleteAllData()
        {
            DataSaver.DeleteAll();
            DebugColor.LogViolet($"All data deleted: {this.name}");
        }
    }
}
