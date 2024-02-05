using BugiGames.ScriptableObject;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace BugiGames.Main
{
    public class PlayerCollisionAsBoundDetector : MonoBehaviour
    {
        [ShowInInspector, ReadOnly] private string collisionLayerMaskName => LayerMask.LayerToName(collisionLayerMaskIndex);
        [SerializeField, Range(0, 360)] private float angleThreshold = 160f;

        private int collisionLayerMaskIndex;

        public event Action OnDirectCollisionExit;
        public event Action OnDirectCollisionEnter;

        private void Awake()
        {
            collisionLayerMaskIndex = LayerMask.NameToLayer(LayerMaskNamesProps.Value.StoppableWall);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collisionLayerMaskIndex == collision.gameObject.layer)
            {
                float angle = Vector3.Angle(transform.forward, collision.contacts[0].normal);

                if (angle >= angleThreshold)
                {
                    OnDirectCollisionEnter?.Invoke();
                }
                else
                {
                    OnDirectCollisionExit?.Invoke();
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collisionLayerMaskIndex == collision.gameObject.layer)
            {
                OnDirectCollisionExit?.Invoke();
            }
        }
    }
}
