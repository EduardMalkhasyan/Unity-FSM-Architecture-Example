using BugiGames.Main;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace BugiGames.Camera
{
    public class CameraShaker : MonoBehaviour
    {
        [Inject] private PlayerCinemachineSources playerCinemachineSources;

        [Button]
        public void DynamiteExplosionShake()
        {
            playerCinemachineSources.DynamiteImpulseSource.GenerateImpulse();
        }

        [Button]
        public void RockExplosionShake()
        {
            playerCinemachineSources.RockImpulseSource.GenerateImpulse();
        }
    }
}
