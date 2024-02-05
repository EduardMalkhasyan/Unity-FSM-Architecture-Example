using Cinemachine;
using UnityEngine;

namespace BugiGames.Main
{
    public class PlayerCinemachineSources : MonoBehaviour
    {
        [field: SerializeField] public CinemachineImpulseSource DynamiteImpulseSource { get; private set; }
        [field: SerializeField] public CinemachineImpulseSource RockImpulseSource { get; private set; }
    }
}
