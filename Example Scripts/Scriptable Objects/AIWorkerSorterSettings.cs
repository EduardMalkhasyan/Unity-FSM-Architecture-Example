using BugiGames.Tools;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace BugiGames.ScriptableObject
{
    public class AIWorkerSorterSettings : SOLoader<AIWorkerSorterSettings>
    {
        #region Movement
        private const string Movement = "Movement";
        private const string AI_SpeedStageDataKey = "AI_SpeedStageDataKey";
        private const int startSpeedStageIndex = 0;

        [ShowInInspector, PropertyRange(0, 2), FoldoutGroup(Movement), InfoBox("Can be changed only in editor mode")]
        private int SpeedStageData
        {
            get
            {
                return PlayerPrefs.GetInt(AI_SpeedStageDataKey, startSpeedStageIndex);
            }
            set
            {
                if (SpeedStagesCount > value && value >= 0)
                {
                    PlayerPrefs.SetInt(AI_SpeedStageDataKey, value);
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
        #endregion

        #region Stack Properties
        private const string Stacking = "Stacking";

        private const string AI_StackLimitStageDataKey = "AI_StackLimitStageDataKey";
        private const int startStackLimitStageIndex = 0;

        [ShowInInspector, PropertyRange(0, 2), FoldoutGroup(Stacking), InfoBox("Can be changed only in editor mode," +
                                                                               " need restart")]
        private int StackLimitStageData
        {
            get
            {
                return PlayerPrefs.GetInt(AI_StackLimitStageDataKey, startStackLimitStageIndex);
            }
            set
            {
                if (StackLimitStagesCount > value && value >= 0)
                {
                    PlayerPrefs.SetInt(AI_StackLimitStageDataKey, value);
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
        public void ResetAIWorkerSorterSettingsData()
        {
            SpeedStageData = startSpeedStageIndex;
            StackLimitStageData = startStackLimitStageIndex;
            DebugColor.LogViolet($"Reset data: {this.name}");
        }
    }
}
