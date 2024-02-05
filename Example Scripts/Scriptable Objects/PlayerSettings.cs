using BugiGames.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BugiGames.ScriptableObject
{
    public class PlayerSettings : SOLoader<PlayerSettings>
    {
        #region Movement
        private const string Movement = "Movement";
        private const string speedStageDataKey = "speedStageDataKey";
        private const int startSpeedStageIndex = 0;

        [ShowInInspector, PropertyRange(0, 5), FoldoutGroup(Movement), InfoBox("Can be changed only in editor mode")]
        private int SpeedStageData
        {
            get
            {
                return PlayerPrefs.GetInt(speedStageDataKey, startSpeedStageIndex);
            }
            set
            {
                if (SpeedStagesCount > value && value >= 0)
                {
                    PlayerPrefs.SetInt(speedStageDataKey, value);
                }
                else
                {
                    if (value < 0)
                    {
                        Debug.LogWarning($"Speed stage cant be smaller than 0");
                    }
                    else
                    {
                        Debug.LogWarning($"You have achieved max speed stage: {SpeedStagesCount}");
                    }
                }
            }
        }

        [ShowInInspector, FoldoutGroup(Movement)]
        public float CurrentStageSpeed => speedStages[SpeedStageData];

        [ShowInInspector, FoldoutGroup(Movement)]
        public float CurrentSpeedStage => SpeedStageData;
        public int SpeedStagesCount => speedStages.Count;

        public void UpgradeSpeedStageAndSave()
        {
            SpeedStageData++;
        }

        public void DowngradeSpeedStageAndSave()
        {
            SpeedStageData--;
        }

        [SerializeField, FoldoutGroup(Movement)] private List<float> speedStages;

        [field: SerializeField, Range(4f, 12f), FoldoutGroup(Movement)]
        public float RotationSpeed { get; private set; } = 9f;
        #endregion

        #region Stack Properties
        private const string Stacking = "Stacking";

        private const string stackLimitStageDataKey = "stackLimitStageDataKey";
        private const int startStackLimitStageIndex = 0;

        [ShowInInspector, PropertyRange(0, 5), FoldoutGroup(Stacking), InfoBox("Can be changed only in editor mode," +
                                                                               " need restart")]
        private int StackLimitStageData
        {
            get
            {
                return PlayerPrefs.GetInt(stackLimitStageDataKey, startStackLimitStageIndex);
            }
            set
            {
                if (StackLimitStagesCount > value && value >= 0)
                {
                    PlayerPrefs.SetInt(stackLimitStageDataKey, value);
                }
                else
                {
                    if (value < 0)
                    {
                        Debug.LogWarning($"Speed stage cant be smaller than 0");
                    }
                    else
                    {
                        Debug.LogWarning($"You have achieved max stack limit stage: {StackLimitStagesCount}");
                    }
                }
            }
        }

        [ShowInInspector, FoldoutGroup(Stacking)]
        public int CurrentStackLimit => stackLimitStages[StackLimitStageData];

        [ShowInInspector, FoldoutGroup(Stacking)]
        public int CurrentStackLimitStage => StackLimitStageData;

        public int StackLimitStagesCount => stackLimitStages.Count;

        public void UpgradeStackLimitStageAndSave()
        {
            StackLimitStageData++;
        }

        public void DowngradeStackLimitStageAndSave()
        {
            StackLimitStageData--;
        }

        public int NextStackLimitBarrier()
        {
            var nextBarrier = 0;

            if (StackLimitStageData + 1 >= stackLimitStages.Count)
            {
                Debug.Log($"Next stack limit is achieved to max stage: {stackLimitStages.Count}");
            }
            else
            {
                nextBarrier = stackLimitStages[StackLimitStageData + 1] - CurrentStackLimit;
            }

            Debug.Log(nextBarrier);
            return nextBarrier;
        }

        [SerializeField, FoldoutGroup(Stacking)] private List<int> stackLimitStages;

        [field: SerializeField, Range(0.01f, 0.5f), FoldoutGroup(Stacking)]
        public float GetFoodFromPlaceFrequency { get; private set; } = 0.07f;

        [field: SerializeField, Range(0.01f, 0.5f), FoldoutGroup(Stacking)]
        public float MoveAllToBinFrequency { get; private set; } = 0.025f;

        [field: SerializeField, Range(0.01f, 0.5f), FoldoutGroup(Stacking)]
        public float MoveCurrentTypeToPickupFrequency { get; private set; } = 0.025f;

        [field: SerializeField, Range(0.01f, 0.5f), FoldoutGroup(Stacking)]
        public float FoodStackHolderResetPositionDuration { get; private set; } = 0.1f;

        [field: SerializeField, FoldoutGroup(Stacking)]
        public ParabolicCurveMoves CurveMoves { get; private set; }

        [Serializable]
        public class ParabolicCurveMoves
        {
            [field: SerializeField, Range(0.01f, 2f)] public float ToSelfDuration { get; private set; } = 0.65f;
            [field: SerializeField, Range(0.01f, 5f)] public float ToSelfHeight { get; private set; } = 1.45f;
            [field: SerializeField, Range(0.01f, 2f)] public float ToBinDuration { get; private set; } = 0.5f;
            [field: SerializeField, Range(0.01f, 5f)] public float ToBinHeight { get; private set; } = 1.2f;
            [field: SerializeField, Range(0.01f, 2f)] public float ToPickupDuration { get; private set; } = 0.7f;
            [field: SerializeField, Range(0.01f, 5f)] public float ToPickupHeight { get; private set; } = 1.7f;
        }
        #endregion

        [Button]
        public void ResetPlayerSettingsData()
        {
            SpeedStageData = startSpeedStageIndex;
            StackLimitStageData = startStackLimitStageIndex;
            DebugColor.LogViolet($"Reset data: {this.name}");
        }
    }
}
