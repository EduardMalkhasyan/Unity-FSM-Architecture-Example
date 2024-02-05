using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;

namespace BugiGames.ExtensionMethod
{
    public static class GameObjectExtensions
    {
        public static async UniTask SetActiveAsync(this GameObject gameObject, bool active, float delay)
        {
            await UniTask.WaitForSeconds(delay);
            gameObject.SetActive(active);
        }

        public static IEnumerator SetActiveCoroutine(this GameObject gameObject, bool active, float delay)
        {
            yield return new WaitForSeconds(delay);

            gameObject.SetActive(active);
        }

        public static void OnceCallParticleEffect(this MonoBehaviour monoBehaviour, ParticleSystem effect, Action OnFinish = null)
        {
            monoBehaviour.StartCoroutine(OnceCallParticleEffectCor(effect, OnFinish));
        }

        private static IEnumerator OnceCallParticleEffectCor(ParticleSystem effect, Action OnFinish = null)
        {
            effect.gameObject.SetActive(true);
            effect.Play();
            yield return new WaitUntil(() => effect.isPlaying == false);
            OnFinish?.Invoke();
            effect.gameObject.SetActive(false);
        }

        public static void FindGameObjectAndSetActiveFalse(params GameObject[] objects)
        {
            foreach (GameObject obj in objects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }
}
