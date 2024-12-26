using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRHampi.NPC
{
    public class GenericAnimationController<TEnum> : MonoBehaviour where TEnum : System.Enum
    {
        [Header("Animator Component")]
        [SerializeField] private Animator animator;
        

        private void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        public void PlayAnimation(TEnum animationState, float transitionDuration = 0.25f)
        {
            if (animator == null)
            {
                Debug.LogError("Animator is not assigned.");
                return;
            }

            string animationName = animationState.ToString();

            animator.CrossFade(animationName, transitionDuration);
        }

        public void StopAnimation(float transitionDuration = 0.25f)
        {
            if (animator == null)
            {
                Debug.LogError("Animator is not assigned.");
                return;
            }

            animator.CrossFade("Idle", transitionDuration); 
        }
    }
}

