using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BugiGames.Camera
{
    [ExecuteInEditMode]
    [SaveDuringPlay]
    public class LockCameraXYZ : CinemachineExtension
    {
        [FoldoutGroup("Lock Settings")][SerializeField] private bool lockX = false;
        [FoldoutGroup("Lock Settings")][ShowIf(nameof(lockX))][SerializeField] private float lockPosition_X = 10;

        [FoldoutGroup("Lock Settings")][SerializeField] private bool lockY = true;
        [FoldoutGroup("Lock Settings")][ShowIf(nameof(lockY))][SerializeField] private float lockPosition_Y = 10;

        [FoldoutGroup("Lock Settings")][SerializeField] private bool lockZ = false;
        [FoldoutGroup("Lock Settings")][ShowIf(nameof(lockZ))][SerializeField] private float lockPosition_Z = 10;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;

                if (lockX)
                {
                    pos.x = lockPosition_X;
                }
                if (lockY)
                {
                    pos.y = lockPosition_Y;
                }
                if (lockZ)
                {
                    pos.z = lockPosition_Z;
                }

                state.RawPosition = pos;
            }
        }
    }
}
