using BugiGames.Tools;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BugiGames.Camera
{
    public enum VirtualCameraType
    {
        Close,
        Far,
    }

    public class VirtualCamera : MonoBehaviour, ISelfReset
    {
        [SerializeField] private SerializableDictionary<VirtualCameraType, CinemachineVirtualCamera> virtualCameras;

        [Button]
        public void SwitchCamera(VirtualCameraType virtualCameraType)
        {
            foreach (var camera in virtualCameras.dictionary)
            {
                camera.Value.gameObject.SetActive(false);
            }

            virtualCameras.dictionary[virtualCameraType].gameObject.SetActive(true);
        }

        public void SetCamerasFollow(Transform followTransform)
        {
            foreach (var camera in virtualCameras.dictionary)
            {
                camera.Value.Follow = followTransform;
            }
        }

        public void SelfReset()
        {
            SwitchCamera(VirtualCameraType.Far);
        }
    }
}
