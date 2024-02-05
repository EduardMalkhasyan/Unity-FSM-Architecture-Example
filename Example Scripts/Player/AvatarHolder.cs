using BugiGames.ScriptableObject;
using BugiGames.Tools;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Zenject;

namespace BugiGames.Main
{
    public class AvatarHolder : MonoBehaviour
    {
        [Inject] private AddressableLoader addressableLoader;
        [Inject] private DiContainer diContainer;

        [ShowInInspector] public (string path, AvatarBody body) currentAvatarTuple;

        public void LoadAvatar(string path, Action OnLoadLevelCB = null)
        {
            TryReleaseAvatar();

            addressableLoader.TryLoadAssetAsync(path, (avatarPrefab) =>
            {
                currentAvatarTuple.path = path;
                InitAvatar(avatarPrefab);
                OnLoadLevelCB?.Invoke();
            });
        }

        public void InitAvatar(UnityEngine.Object avatarPrefab)
        {
            var avatar = avatarPrefab as GameObject;
            currentAvatarTuple.body = diContainer.InstantiatePrefab(avatar).GetComponent<AvatarBody>();

            currentAvatarTuple.body.transform.localPosition = Vector3.zero;
            currentAvatarTuple.body.transform.localRotation = Quaternion.identity;
            currentAvatarTuple.body.transform.SetParent(transform, false);
        }

        public void TryReleaseAvatar()
        {
            if (currentAvatarTuple.body == null)
            {
                return;
            }

            addressableLoader.ReleaseAsset(currentAvatarTuple.path, () =>
            {
                Destroy(currentAvatarTuple.body.gameObject);
            });
        }
    }
}
