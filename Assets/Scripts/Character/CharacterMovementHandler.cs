using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CharacterMovementHandler : MonoBehaviour
    {
        Tween straightMovement;
        public Transform model;

        public void StartStraightMovement(Transform target, float speed, TweenCallback tweenCallback)
        {    
            straightMovement = transform.DOMoveZ(target.transform.position.z, speed).SetSpeedBased(true).SetEase(Ease.Linear).OnComplete(tweenCallback);
        }
        public void StopStraightMovement()
        {
            if (straightMovement != null)
            {
                straightMovement.Kill();
                straightMovement=null;
            }
        }
        public void Jump(Vector3 target, float duration, float jumpHeight = .5f)
        {
            target.z += .5f;
            Vector3 local = model.transform.InverseTransformPoint(target);
            model.DOLocalJump(local, jumpHeight, 1, duration);
        }
    }
}