using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BugiGames.Tools
{
    public class AddressableLoader
    {
        private readonly Dictionary<string, UnityEngine.Object> keyValuePairs = new Dictionary<string, UnityEngine.Object>();

        public async void TryLoadAssetAsync(string key, Action<UnityEngine.Object> successCB = null, Action failCB = null)
        {
            if (keyValuePairs.TryGetValue(key, out UnityEngine.Object value))
            {
                Debug.LogWarning($"\"{value}\" is already loaded, try get is from path \"{key}\"");
                return;
            }

            await LoadAssetTask(key, successCB, failCB);
        }

        public UnityEngine.Object TryGetAsset(string key)
        {
            if (keyValuePairs.TryGetValue(key, out UnityEngine.Object value))
            {
                Debug.Log($"Successfully received \"{value}\" of path \"{key}\"");
                return value;
            }
            else
            {
                Debug.LogWarning($"UnityEngine.Object of path \"{key}\" is null, check your path, or try load it first");
                return null;
            }
        }

        private async UniTask LoadAssetTask(string key, Action<UnityEngine.Object> successCB, Action failCB)
        {
            AsyncOperationHandle<UnityEngine.Object> asyncOperationHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(key);
            await asyncOperationHandle.Task;

            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                keyValuePairs.TryAdd(key, asyncOperationHandle.Result);
                successCB?.Invoke(asyncOperationHandle.Result);
                Debug.Log($"Successfully -LOADED- \"{asyncOperationHandle.Result}\" of path \"{key}\"");
            }
            else
            {
                failCB?.Invoke();
                Debug.LogWarning($"Cant load \"{asyncOperationHandle.Result}\" of path \"{key}\"");
            }
        }

        public void ReleaseAsset(string key, Action successCB = null, Action failCB = null)
        {
            if (keyValuePairs.TryGetValue(key, out UnityEngine.Object value))
            {
                Addressables.Release(value);
                keyValuePairs.Remove(key);
                successCB?.Invoke();
                Debug.Log($"Successfully -RELEASED- \"{value}\" of path \"{key}\"");
            }
            else
            {
                failCB?.Invoke();
                Debug.LogWarning($"UnityEngine.Object of path \"{key}\" is null, its already released, or dosent existed at all");
            }
        }
    }
}
