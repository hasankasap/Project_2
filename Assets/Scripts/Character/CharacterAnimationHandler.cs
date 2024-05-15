using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CharacterAnimationHandler : MonoBehaviour
    {
        private Animator characterAnimator;
        void Start()
        {
            characterAnimator = GetComponentInChildren<Animator>();
        }
        public void PlayRunningAnimation()
        {
            if (!characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                characterAnimator.SetTrigger("Run");
        }
        public void PlayDancingAnimation()
        {
            if (!characterAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
                characterAnimator.SetTrigger("Dance");
        }
    }
}