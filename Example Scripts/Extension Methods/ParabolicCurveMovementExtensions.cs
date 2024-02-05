using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace BugiGames.Tools
{
    public static class ParabolicCurveMovementExtensions
    {
        public static async void MoveDynamicWithParabolicCurveTo(this Transform moveTransform, Transform targetTransform,
                                                                 float duration, float height,
                                                                 Action onComplete = null)
        {
            Vector3 initialPosition = moveTransform.position;
            float elapsedTime = 0f;
            var cancellationTokenSource = new CancellationTokenSource();

            moveTransform.gameObject.OnDestroyAsObservable()
                                    .Subscribe((unit) => cancellationTokenSource.Cancel());

            try
            {
                await UniTask.WaitWhile(() =>
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    float time = Mathf.Clamp01(elapsedTime / duration);

                    Vector3 initialToTarget = targetTransform.position - initialPosition;
                    Vector3 parabolicOffset = (Vector3.up * height) * Mathf.Sin(time * Mathf.PI);

                    moveTransform.position = initialPosition + initialToTarget * time + parabolicOffset;

                    elapsedTime += Time.fixedDeltaTime;

                    return moveTransform.position != targetTransform.position;

                }, PlayerLoopTiming.LastFixedUpdate);
            }
            catch (OperationCanceledException error)
            {
                Debug.LogWarning(error);
            }
            finally
            {
                onComplete?.Invoke();
            }
        }

        public static async void MoveStaticWithParabolicCurveTo(this Transform moveTransform, Vector3 targetPosition,
                                                                float duration, float height,
                                                                Action onComplete = null)
        {
            Vector3 initialPosition = moveTransform.position;
            float elapsedTime = 0f;
            var cancellationTokenSource = new CancellationTokenSource();

            moveTransform.gameObject.OnDestroyAsObservable()
                                    .Subscribe((unit) => cancellationTokenSource.Cancel());

            try
            {
                await UniTask.WaitWhile(() =>
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    float time = Mathf.Clamp01(elapsedTime / duration);

                    Vector3 initialToTarget = targetPosition - initialPosition;
                    Vector3 parabolicOffset = (Vector3.up * height) * Mathf.Sin(time * Mathf.PI);

                    moveTransform.position = initialPosition + initialToTarget * time + parabolicOffset;

                    elapsedTime += Time.fixedDeltaTime;

                    return moveTransform.position != targetPosition;

                }, PlayerLoopTiming.LastFixedUpdate);
            }
            catch (OperationCanceledException error)
            {
                Debug.LogWarning(error);
            }
            finally
            {
                onComplete?.Invoke();
            }
        }

        public static async void MoveAndRotateDynamicWithParabolicCurveTo(this Transform moveTransform,
                                                                          Transform targetTransform,
                                                                          float duration, float height,
                                                                          Action onComplete = null)
        {
            Vector3 initialPosition = moveTransform.position;
            Quaternion initialRotation = moveTransform.rotation;
            float elapsedTime = 0f;
            var cancellationTokenSource = new CancellationTokenSource();

            moveTransform.gameObject.OnDestroyAsObservable()
                                    .Subscribe((unit) => cancellationTokenSource.Cancel());

            try
            {
                await UniTask.WaitWhile(() =>
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    float time = Mathf.Clamp01(elapsedTime / duration);

                    Vector3 initialToTarget = targetTransform.position - initialPosition;
                    Vector3 parabolicOffset = (Vector3.up * height) * Mathf.Sin(time * Mathf.PI);

                    moveTransform.position = initialPosition + initialToTarget * time + parabolicOffset;

                    float rotationProgress = Mathf.Clamp01(elapsedTime / duration);
                    moveTransform.rotation = Quaternion.Slerp(initialRotation, targetTransform.rotation, rotationProgress);

                    elapsedTime += Time.fixedDeltaTime;

                    return moveTransform.position != targetTransform.position;
                }, PlayerLoopTiming.LastFixedUpdate);
            }
            catch (OperationCanceledException error)
            {
                Debug.LogWarning(error);
            }
            finally
            {
                onComplete?.Invoke();
            }
        }
    }
}