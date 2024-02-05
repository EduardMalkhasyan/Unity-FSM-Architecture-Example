using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BugiGames.Main
{
    public class AvatarBody : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private const string Behavior = "Behavior";

        [Button]
        public void Idle()
        {
            animator.SetInteger(Behavior, 0);
        }

        [Button]
        public void Run()
        {
            animator.SetInteger(Behavior, 1);
        }

        [Button]
        public void RunWithItem()
        {
            animator.SetInteger(Behavior, 2);
        }

        [Button]
        public void IdleWithItem()
        {
            animator.SetInteger(Behavior, 3);
        }

        public void SetAnimatorSpeed(float speed)
        {
            animator.speed = speed;
        }
    }
}
