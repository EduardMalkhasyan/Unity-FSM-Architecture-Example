using BugiGames.Tools;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BugiGames.Main
{
    public class GameReset
    {
        [Inject] private IEnumerable<ISelfReset> interfaceInstances;

        public event Action OnGameResetComplete;

        public void DoReset()
        {
            foreach (var instance in interfaceInstances)
            {
                instance.SelfReset();
                SelfResetMessage(instance);
            }

            OnGameResetComplete?.Invoke();
        }

        private void SelfResetMessage(ISelfReset self)
        {
            DebugColor.LogViolet($"Reset: {self.GetType().Name}");
        }
    }
}
