using Game.BlockSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterMovementHandler characterMovement;
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
            characterMovement.StartStraightMovement(finish, settings.StraightMovementSpeed, null); // add finish action here
        }
        private void Jump(MovingBlock target)
        {
            Vector3 pos = transform.position;
            if (target == null)
            {
                pos.z += 2f;
                pos.y -= 5f;
                characterMovement.StopStraightMovement();
                characterMovement.Jump(pos, settings.FallingDuration, settings.FallingJumpHeight);
                mainCol.enabled = false;
            }
            else
            {
                pos.x = target.transform.position.x;
                characterMovement.Jump(pos, settings.JumpingHeight, settings.JumpingDuration);
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
        private void OnTriggerExit(Collider other)
        {
            
        }
    }
}