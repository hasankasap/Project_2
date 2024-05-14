using Game.BlockSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterMovementHandler characterMovement;
        [SerializeField] private CharacterAnimationHandler characterAnimation;
        [SerializeField] private Transform finish;
        [SerializeField] private Collider mainCol;
        [SerializeField] private CharacterSettings settings;
        private void OnEnable()
        {
            EventManager.StartListening(GameEvents.START_MOVEMENT, StartStraightMovement);
            EventManager.StartListening(GameEvents.STOP_MOVEMENT, StopStraightMovement);
        }
        private void OnDisable()
        {
            EventManager.StopListening(GameEvents.START_MOVEMENT, StartStraightMovement);
            EventManager.StopListening(GameEvents.STOP_MOVEMENT, StopStraightMovement);
        }
        private void Start()
        {
            characterMovement = GetComponent<CharacterMovementHandler>();
            characterAnimation = GetComponent<CharacterAnimationHandler>();
        }
        private void StopStraightMovement(object[] obj)
        {
            characterMovement.StopStraightMovement();
        }
        private void StartStraightMovement(object[] obj)
        {
            if (finish == null)
            {
                Debug.LogError("Finish transform missing please check prefab !!");
                return;
            }
            characterMovement.StartStraightMovement(finish, settings.StraightMovementSpeed, FinishAction); // add finish action here
        }
        private void FinishAction()
        {

        }
        private void Jump(MovingBlock target)
        {
            Vector3 pos = transform.position;
            if (target == null)
            {
                pos.z += 2f;
                pos.y -= 5f;
                characterMovement.StopStraightMovement();
                characterMovement.Jump(pos, settings.FallingDuration, settings.FallingJumpHeight, true);
                mainCol.enabled = false;
            }
            else
            {
                float target_X = target.Center.x;
                pos.x = target_X;
                if (Mathf.Abs(target_X - transform.position.x) <= settings.JumpingThreshold)
                {
                    characterMovement.SideMovement(pos, settings.SideMovementSpeed);
                }
                else
                {
                    characterMovement.Jump(pos, settings.JumpingHeight, settings.JumpingDuration);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Block"))
            {
                MovingBlock movingBlock = other.GetComponent<MovingBlock>();
                movingBlock.ChangeColliderActive(false);
                Jump(movingBlock.nextBlock);
            }
            if (other.CompareTag("Collectable"))
            {
                // collect action
            }
        }
    }
}