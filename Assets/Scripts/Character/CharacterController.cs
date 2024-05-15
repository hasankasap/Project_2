using DG.Tweening;
using Game.BlockSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace Game
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private CharacterMovementHandler characterMovement;
        [SerializeField] private CharacterAnimationHandler characterAnimation;
        [SerializeField] private Transform finish;
        [SerializeField] private Collider mainCol;
        [SerializeField] private CharacterSettings settings;
        [SerializeField] private Transform model;
        [SerializeField] private GameObject finishCam;
        private void OnEnable()
        {
            EventManager.StartListening(GameEvents.START_MOVEMENT, StartStraightMovement);
            EventManager.StartListening(GameEvents.STOP_MOVEMENT, StopStraightMovement);
            EventManager.StartListening(GameEvents.LEVEL_CREATED, OnLevelCreated);
            EventManager.StartListening(GameEvents.CONTINUE_GAME, OnContinue);
        }
        private void OnDisable()
        {
            EventManager.StopListening(GameEvents.START_MOVEMENT, StartStraightMovement);
            EventManager.StopListening(GameEvents.STOP_MOVEMENT, StopStraightMovement);
            EventManager.StopListening(GameEvents.LEVEL_CREATED, OnLevelCreated);
            EventManager.StopListening(GameEvents.CONTINUE_GAME, OnContinue);
        }
        private void Start()
        {
            characterMovement = GetComponent<CharacterMovementHandler>();
            characterAnimation = GetComponent<CharacterAnimationHandler>();
        }
        private void OnContinue(object[] obj)
        {
            finishCam.SetActive(false);
        }
        private void OnLevelCreated(object[] obj)
        {
            Level level = (Level)obj[0];
            if (level == null)
            {
                Debug.LogError("Level missing please check code or prefab !!");
                return;
            }
            finish = level.Finish;
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
            characterAnimation.PlayRunningAnimation();
            characterMovement.StartStraightMovement(finish, settings.StraightMovementSpeed, FinishAction); // add finish action here
        }
        private void FinishAction()
        {
            characterAnimation.PlayDancingAnimation();
            finishCam.SetActive(true);
            LevelManager.Instance.LevelComplete();
        }
        private void FailAction()
        {
            model.gameObject.SetActive(false);
            DOVirtual.DelayedCall(.5f, LevelManager.Instance.LevelFail, false).SetLink(gameObject);
        }
        private void Jump(MovingBlock target)
        {
            Vector3 pos = transform.position;
            if (target == null)
            {
                pos.z += 2f;
                pos.y -= 5f;
                characterMovement.StopStraightMovement();
                characterMovement.Jump(pos, settings.FallingDuration, settings.FallingJumpHeight, true, FailAction);
                mainCol.enabled = false;
            }
            else
            {
                float target_X = target.Center.x;
                pos.x = target_X;
                if (Mathf.Abs(target_X - model.transform.position.x) <= settings.JumpingThreshold)
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
        }
    }
}