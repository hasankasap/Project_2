using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CharacterMovementHandler : MonoBehaviour
    {
        Tween straightMovement, sideMovement;
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
        public void SideMovement(Vector3 target, float speed)
        {
            if (sideMovement != null)
            {
                sideMovement.Kill();
            }
            Vector3 local = transform.InverseTransformPoint(target);
            sideMovement = model.DOLocalMoveX(local.x, speed).SetSpeedBased(true);
        }
        public void Jump(Vector3 target, float duration, float jumpHeight = .5f, bool jumpFront = false)
        {
            if (jumpFront) target.x = model.transform.position.x;
            else target.z = model.transform.position.z;
            Vector3 local = transform.InverseTransformPoint(target);
            model.DOLocalJump(local, jumpHeight, 1, duration);
        }
    }
}