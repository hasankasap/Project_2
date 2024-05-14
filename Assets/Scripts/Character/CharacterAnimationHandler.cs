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
            characterAnimator.SetTrigger("Run");
        }
        public void PlayDancingAnimation()
        {
            characterAnimator.SetTrigger("Dance");
        }
    }
}