using BugiGames.ScriptableObject;
using BugiGames.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace BugiGames.Main
{
    public class InitialSceneLoader : MonoBehaviour
    {
        private IEnumerator Start()
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(GameSettings.Value.GameSceneAddress);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                yield return handle.Result.ActivateAsync();
            }
        }
    }
}
